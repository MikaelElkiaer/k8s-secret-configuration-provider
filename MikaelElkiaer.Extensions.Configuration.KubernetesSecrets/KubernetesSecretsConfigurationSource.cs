using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using MikaelElkiaer.Extensions.Configuration.KubernetesSecrets.Options;

namespace MikaelElkiaer.Extensions.Configuration.KubernetesSecrets
{
    public class KubernetesSecretsConfigurationSource : IConfigurationSource
    {
        private readonly KubernetesSecretsConfigurationProviderOptions options;
        private readonly IEnumerable<KeyValuePair<string, string>> existingKeyValues;

        public KubernetesSecretsConfigurationSource(KubernetesSecretsConfigurationProviderOptions options, IEnumerable<KeyValuePair<string, string>> existingKeyValues)
        {
            this.options = options;
            this.existingKeyValues = existingKeyValues;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new KubernetesSecretsConfigurationProvider(options, existingKeyValues);
        }
    }
}
