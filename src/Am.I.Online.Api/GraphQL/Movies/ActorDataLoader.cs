namespace Am.I.Online.Api.GraphQL.Movies;

public class ActorDataLoader : BatchDataLoader<int, Actor>
{
  public ActorDataLoader(IBatchScheduler batchScheduler)
    : base(batchScheduler)
  {
  }

  protected override Task<IReadOnlyDictionary<int, Actor>> LoadBatchAsync(IReadOnlyList<int> keys,
    CancellationToken cancellationToken)
  {
    return Task.FromResult<IReadOnlyDictionary<int, Actor>>(Seed.Actors().Where(x => keys.Contains(x.Id))
      .ToDictionary(x => x.Id));
  }
}
