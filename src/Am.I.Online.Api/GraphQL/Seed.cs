using Am.I.Online.Api.GraphQL.Movies;

namespace Am.I.Online.Api.GraphQL;

public class Seed
{
  public static List<Movie> SeedData()
  {
    var actors = Actors();

    var movies = new List<Movie>
    {
      new()
      {
        Id = 1,
        Title = "The Rise of the GraphQL Warrior",
        ActorIds = actors.Select(x => x.Id).Take(1).ToArray()
      },
      new()
      {
        Id = 2,
        Title = "The Rise of the GraphQL Warrior Part 2",
        ActorIds = actors.Select(x => x.Id).ToArray()
      }
    };
    return movies;
  }

  public static List<Actor> Actors()
  {
    var actors = new List<Actor>
    {
      new()
      {
        Id = 1,
        FirstName = "Bob",
        LastName = "Kante"
      },
      new()
      {
        Id = 2,
        FirstName = "Mary",
        LastName = "Poppins"
      }
    };
    return actors;
  }
}
