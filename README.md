# General usage

## Configuration of application

In order to get started, the provider must be added.
For ASP.NET Core, via the hostbuilder:

```csharp
// ...
.ConfigureAppConfiguration((hc, c) =>
{
    c.AddKubernetesSecretsConfiguration();
});
```

or for any other .NET Core project:

```csharp
var configuration = new ConfigurationBuilder()
    .AddKubernetesSecretsConfiguration()
    .Build();
```

The provider is dependent on other providers in order to substitute values, and must therefore be defined after others it  depends upon.
Upon loading the provider, it will search for any values with the defined prefix (defaults to `k8s:`).
These values are then parsed, using the following format:

`k8s:SECRETNAME:SECRETKEY[:NAMESPACE]`

If namespace is left out, the secret it assumed to be in the default namespace.
Any unmatching values will result in an error.
If no errors are encountered, the provider will substitute the current values with the value obtained from the matched secret.

## Examples

Using an `appsettings.json` file with the content:

```json
{
    "key1": "k8s:secret1:fieldx",
    "key2": "k8s:secret2:fieldy:nondefault",
    "key3": "value3"
}
```

will result in keys 1 and 2 being substituted with the corresponding value from the referenced secret and field.
The third key will keep its value.

## Kubernetes configuration

The provider uses [KubernetesClient](https://github.com/kubernetes-client/csharp) in order to use Kubernetes APIs, as well as authenticate.
Currently authentication is only done implicitly, by utilizing the current Kubernetes context.

Personally, I have at least 2 contexts configured: development and production.
Usually I will be using the development context.
This means that the provider will be substituting configuration values using the secrets from my development Kubernetes cluster.
In order to switch to secrets defined in production, I simply switch my Kubernetes context.
