namespace Am.I.Online.Api.GraphQL.Movies;

public class Movie
{
  public int Id { get; set; }
  public string Title { get; set; }
  public int[] ActorIds { get; set; }
}
