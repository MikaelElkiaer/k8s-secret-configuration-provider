using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace MikaelElkiaer.Extensions.Configuration.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                //.AddKubernetesSecretsConfiguration() // This will enable substition of existing keys with default prefix
                .AddKubernetesSecretsConfiguration(b => // Advanced configuration with non-default options
                {
                    //b.SetSubstitutePrefix("k8s:"); // Default prefix
                })
                .Build();

            Console.WriteLine("Configuration key-value-pairs:");
            foreach (var c in configuration.GetChildren().Where(x => x.Key.StartsWith("TEST_")))
            {
               Console.WriteLine($"{c.Key}={c.Value}");
            }
        }
    }
}
