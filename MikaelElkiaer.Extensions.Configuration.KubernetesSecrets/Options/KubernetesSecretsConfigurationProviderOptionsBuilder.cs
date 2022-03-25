using System;
using System.Collections.Generic;
using MikaelElkiaer.Extensions.Configuration.KubernetesSecrets.Model;

namespace MikaelElkiaer.Extensions.Configuration.KubernetesSecrets.Options
{
    public class KubernetesSecretsConfigurationProviderOptionsBuilder
    {
        private string? substitutePrefix;

        public KubernetesSecretsConfigurationProviderOptionsBuilder SetSubstitutePrefix(string prefix)
        {
            substitutePrefix = prefix;

            return this;
        }

        public KubernetesSecretsConfigurationProviderOptions Build()
        {
            return new KubernetesSecretsConfigurationProviderOptions(substitutePrefix);
        }
    }
}
