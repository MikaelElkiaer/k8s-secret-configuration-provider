using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using MikaelElkiaer.Extensions.Configuration.KubernetesSecrets;
using MikaelElkiaer.Extensions.Configuration.KubernetesSecrets.Model;
using MikaelElkiaer.Extensions.Configuration.KubernetesSecrets.Options;

namespace Microsoft.Extensions.Configuration
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddKubernetesSecretsConfiguration(this IConfigurationBuilder builder, Action<KubernetesSecretsConfigurationProviderOptionsBuilder>? optionsBuilderDelegate = null)
        {
            var optionsBuilder = new KubernetesSecretsConfigurationProviderOptionsBuilder();
            optionsBuilderDelegate?.Invoke(optionsBuilder);

            var options = optionsBuilder.Build();

            IEnumerable<KeyValuePair<string, string>> existingKeyValues = Enumerable.Empty<KeyValuePair<string, string>>();
            var tempConfig = builder.Build();
            existingKeyValues = tempConfig.AsEnumerable().Where(c => c.Value != null && c.Value.StartsWith(options.SubstitutePrefix));

            return builder.Add(new KubernetesSecretsConfigurationSource(options, existingKeyValues));
        }
    }
}
