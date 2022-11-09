using Bumbershoot.Utilities.Helpers;
using Am.I.Online.Api.GraphQL.Movies;

namespace Am.I.Online.Api.GraphQL
{
  public class QueryDataLoader
  {
    public List<Movie> GetMovies() =>
      Seed.SeedData();

    public Movie? GetMovieById(int id) =>
      Seed.SeedData().FirstOrDefault(x => x.Id == id);
  }
}
