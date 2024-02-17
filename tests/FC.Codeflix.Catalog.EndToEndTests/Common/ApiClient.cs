using FC.Codeflix.Catalog.EndToEndTests.Extensions.String;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Json;

namespace FC.Codeflix.Catalog.EndToEndTests.Common;

class SnakeCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        return name.ToSnakeCase();
    }
}

public class ApiClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _defaultSerializeOptions;

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _defaultSerializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<(HttpResponseMessage?, TOutput?)> Post<TOutput>(
        string route,
        object payload
    ) where TOutput : class
    {
        var response = await _httpClient.PostAsync(
            route,
            new StringContent(
                JsonSerializer.Serialize(
                    payload,
                    _defaultSerializeOptions
                ),
                Encoding.UTF8,
                "application/json"
            )
        );

        var output = await GetOutput<TOutput>(response);

        return (response, output);
    }

    public async Task<(HttpResponseMessage?, TOutput?)> Put<TOutput>(
        string route,
        object payload
    ) where TOutput : class
    {
        var response = await _httpClient.PutAsync(
            route,
            new StringContent(
                JsonSerializer.Serialize(
                    payload,
                    _defaultSerializeOptions
                ),
                Encoding.UTF8,
                "application/json"
            )
        );

        var output = await GetOutput<TOutput>(response);

        return (response, output);
    }

    public async Task<(HttpResponseMessage?, TOutput?)> Get<TOutput>(
        string route,
        object? queryStringParamsObject = null
    )
        where TOutput : class
    {
        var newQuery = PrepareGetRoute(route, queryStringParamsObject);

        var response = await _httpClient.GetAsync(newQuery);

        var output = await GetOutput<TOutput>(response);

        return (response, output);
    }

    public async Task<(HttpResponseMessage?, TOutput?)> Delete<TOutput>(
        string route
    ) where TOutput : class
    {
        var response = await _httpClient.DeleteAsync(route);

        var output = await GetOutput<TOutput>(response);

        return (response, output);
    }

    private async Task<TOutput?> GetOutput<TOutput>
    (
        HttpResponseMessage response
    )
        where TOutput : class
    {
        var outputString = await response.Content.ReadAsStringAsync();
        TOutput? output = null;
        if (!String.IsNullOrWhiteSpace(outputString))
        {
            output = JsonSerializer.Deserialize<TOutput>(
                outputString,
                _defaultSerializeOptions
            );
        }

        return output;
    }

    private string PrepareGetRoute(
        string route,
        object? queryStringParamsObject
    )
    {
        if (queryStringParamsObject == null)
        {
            return route;
        }

        var parametersJson = JsonSerializer.Serialize(
            queryStringParamsObject,
            _defaultSerializeOptions
        );
        var parametersDictionary = Newtonsoft.Json.JsonConvert
            .DeserializeObject<Dictionary<string, string>>(parametersJson);

        return QueryHelpers.AddQueryString(route, parametersDictionary!);
    }
}
