using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Bumbershoot.Utilities.Helpers;
using Am.I.Online.Api.GraphQL;
using Am.I.Online.Api.Tests.Controllers;
using FluentAssertions;
using NUnit.Framework;

namespace Am.I.Online.Api.Tests;

public class BaseApiTests
{
  private Lazy<TestApi> _api;

  protected TestApi Api => _api.Value;

  [OneTimeSetUp]
  public void BaseSetUp()
  {
    Console.Out.WriteLine("BaseSetUp");
    _api = new Lazy<TestApi>(() => new TestApi());
  }


  [OneTimeTearDown]
  public void BaseTearDown()
  {
    if (_api.IsValueCreated)
    {
      _api.Value.Dispose();
    }
  }

  public record Request(
    [property: JsonPropertyName("query")] string Query,
    [property: JsonPropertyName("variables")]
    string? Variables = null
  );

  // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);

  public record Response<T>(
    [property: JsonPropertyName("data")] T Data,
    [property: JsonPropertyName("errors")] IReadOnlyList<Error>? Errors
  );

  public record Error(
    [property: JsonPropertyName("message")]
    string Message,
    [property: JsonPropertyName("locations")]
    IReadOnlyList<Location> Locations,
    [property: JsonPropertyName("path")] IReadOnlyList<string> Path,
    [property: JsonPropertyName("extensions")]
    Extensions Extensions
  );

  public record Extensions(
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("field")] string Field,
    [property: JsonPropertyName("responseName")]
    string ResponseName,
    [property: JsonPropertyName("specifiedBy")]
    string SpecifiedBy
  );

  public record Location(
    [property: JsonPropertyName("line")] int Line,
    [property: JsonPropertyName("column")] int Column
  );


  protected static async Task<Response<T>> GraphqlQuery<T>(HttpClient httpClient, string query)
  {
    var content = JsonSerializer.Serialize(new Request(query));
    var httpResponseMessage =
      await httpClient.PostAsync("/graphql", new StringContent(content, Encoding.UTF8, "application/json"));
    httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
    var readAsStream = await httpResponseMessage.Content.ReadAsStringAsync();
    var options = new JsonSerializerOptions
    {
      PropertyNameCaseInsensitive = true
    };
    var deserializeAsync = JsonSerializer.Deserialize<Response<T>>(readAsStream, options)!;
    return deserializeAsync!;
  }
}
