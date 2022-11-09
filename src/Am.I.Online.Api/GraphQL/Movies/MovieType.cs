namespace Am.I.Online.Api.GraphQL.Movies;

public class MovieType : ObjectType<Movie>
{
  protected override void Configure(IObjectTypeDescriptor<Movie> descriptor)
  {
    descriptor.BindFieldsImplicitly();
    descriptor
      .Field("actors")
      .Type<ListType<ActorType>>()
      .Resolve((ctx, cancellationToken) =>
      {
        var parent = ctx.Parent<Movie>();
        return ctx.Resolver<ActorDataLoader>().LoadAsync(parent.ActorIds, cancellationToken);
      });
  }
}
