using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using k8s;
using Microsoft.Extensions.Configuration;
using MikaelElkiaer.Extensions.Configuration.KubernetesSecrets.Model;
using MikaelElkiaer.Extensions.Configuration.KubernetesSecrets.Options;

namespace MikaelElkiaer.Extensions.Configuration.KubernetesSecrets
{
    public class KubernetesSecretsConfigurationProvider : ConfigurationProvider
    {
        private readonly KubernetesSecretsConfigurationProviderOptions options;
        private readonly IEnumerable<KeyValuePair<string, string>> existingKeyValues;

        public KubernetesSecretsConfigurationProvider(KubernetesSecretsConfigurationProviderOptions options, IEnumerable<KeyValuePair<string, string>> existingKeyValues)
        {
            this.options = options;
            this.existingKeyValues = existingKeyValues;
        }

        public override void Load()
        {
            var config = KubernetesClientConfiguration.BuildDefaultConfig();
            var client = new Kubernetes(config);
            Console.WriteLine($"Context: {config.CurrentContext}");

            var existingSecrets = new List<Secret>();
            foreach (var v in existingKeyValues)
            {
                var regex = $@"^{options.SubstitutePrefix}([^:]+):([^:]+):?([^:]+)?$";
                var match = Regex.Match(v.Value, regex);
                if (!match.Success)
                    throw new Exception($"Failed to parse existing secret with value {v.Value}");

                var secretName = match.Groups[1].Value;
                var fieldName = match.Groups[2].Value;
                var namespaceName = match.Groups[3].Value;
                var secret = new Secret(secretName, fieldName, v.Key, namespaceName);
                existingSecrets.Add(secret);
            }

            var keyValuePairs = new Dictionary<string, string>();
            var errors = new List<Exception>();
            foreach (var s in existingSecrets)
            {
                try
                {
                    var secretResult = client.ReadNamespacedSecret(s.Name, s.Namespace);
                    if (secretResult is null)
                        throw new Exception($"Did not find secret with name {s.Name} in namespace {s.Namespace}");

                    if (!secretResult.Data.TryGetValue(s.Field, out var encodedValue))
                        throw new Exception($"Secret {s.Name} does not have a field {s.Field}");
                    var decodedValue = Encoding.UTF8.GetString(encodedValue);
                    keyValuePairs[s.OriginalKey] = decodedValue;
                }
                catch (Exception ex)
                {
                    errors.Add(new Exception($"Failed to retrieve value for {s.Name}:{s.Field}:{s.Namespace} - {ex.Message}"));
                }
            }

            if (errors.Any())
                throw new AggregateException("Failed to populate config from k8s.", errors);

            Data = keyValuePairs;
        }
    }
}
