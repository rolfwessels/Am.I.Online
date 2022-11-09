namespace Am.I.Online.Api.GraphQL.Movies;

public class ActorType : ObjectType<Actor>
{
  protected override void Configure(IObjectTypeDescriptor<Actor> descriptor)
  {
    descriptor.BindFieldsImplicitly();
  }
}
