using System.Collections.Generic;
using System.Threading.Tasks;
using Am.I.Online.Api.GraphQL.Movies;
using FluentAssertions;
using NUnit.Framework;

namespace Am.I.Online.Api.Tests;

public class QueryTypeTests : BaseApiTests
{
  [Test]
  [Category("Integration")]
  public async Task Movies_GivenRequest_ShouldReturnMovies()
  {
    // arrange
    var httpClient = Api.CreateClient();
    // action
    var httpResponseMessage = await GraphqlQuery<MoviesResponse>(httpClient, @"query {
 movies {
   id
   title
   actorIds
   actors {
     id
     firstName
     lastName
   }

 }
}");
    // assert
    httpResponseMessage.Data.Movies.Should().HaveCount(2);
  }

  public record MoviesResponse(List<Movie> Movies);
}
