using Newtonsoft.Json.Serialization;

namespace FC.Codeflix.Catalog.EndToEndTests.Extensions.String;

public static class StringSnakeCaseExtension
{
    private static readonly NamingStrategy _snakeCaseNamingStrategy =
        new SnakeCaseNamingStrategy();

    public static string ToSnakeCase(this string stringToConver)
    {
        ArgumentNullException.ThrowIfNull(
            stringToConver,
            nameof(stringToConver)
        );

        return _snakeCaseNamingStrategy.GetPropertyName(
            stringToConver,
            false
        );
    }
}
