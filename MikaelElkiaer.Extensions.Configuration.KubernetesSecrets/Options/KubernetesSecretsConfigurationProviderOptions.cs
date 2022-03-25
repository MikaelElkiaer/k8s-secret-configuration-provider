using System;
using System.Collections.Generic;
using System.Linq;
using MikaelElkiaer.Extensions.Configuration.KubernetesSecrets.Model;

namespace MikaelElkiaer.Extensions.Configuration.KubernetesSecrets.Options
{
    public class KubernetesSecretsConfigurationProviderOptions
    {
        public KubernetesSecretsConfigurationProviderOptions(string? substitutePrefix)
        {
            if (substitutePrefix != null)
                SubstitutePrefix = substitutePrefix;
        }

        public string SubstitutePrefix { get; } = "k8s:";
    }
}
