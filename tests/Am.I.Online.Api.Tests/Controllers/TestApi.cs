using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace Am.I.Online.Api.Tests.Controllers;

public class TestApi : WebApplicationFactory<Program>
{
  protected override IHost CreateHost(IHostBuilder builder)
  {
    builder.ConfigureServices(services =>
    {
      // services.RemoveAll(typeof(IStorage));
      // services.AddTransient<IStorage, InMemoryStorage>(x => new InMemoryStorage().With(x => x.Add("home/rolf/asdf.sample", "key")));
    });

    return base.CreateHost(builder);
  }
}
