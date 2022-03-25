namespace MikaelElkiaer.Extensions.Configuration.KubernetesSecrets.Model
{
    public class Secret
    {
        public const string DEFAULT_NAMESPACE = "default";

        public Secret(string name, string field, string originalKey, string @namespace = DEFAULT_NAMESPACE)
        {
            Name = name;
            Field = field;
            OriginalKey = originalKey;
            Namespace = string.IsNullOrWhiteSpace(@namespace) ? DEFAULT_NAMESPACE : @namespace;
        }

        public string Name { get; }
        public string Field { get; }
        public string OriginalKey { get; }
        public string Namespace { get; }
    }
}
