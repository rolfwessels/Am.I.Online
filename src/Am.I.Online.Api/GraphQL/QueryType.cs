using Am.I.Online.Api.GraphQL.Movies;

namespace Am.I.Online.Api.GraphQL;

public class QueryType : ObjectType<QueryDataLoader>
{
  protected override void Configure(IObjectTypeDescriptor<QueryDataLoader> descriptor)
  {
    descriptor.BindFieldsExplicitly();
    descriptor
      .Field(f => f.GetMovies())
      .Type<ListType<MovieType>>();
  }
}
