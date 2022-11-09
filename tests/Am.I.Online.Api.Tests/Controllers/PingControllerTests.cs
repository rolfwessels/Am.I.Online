using System.Net;
using System.Threading.Tasks;
using Am.I.Online.Api.Controllers;
using FluentAssertions;
using NUnit.Framework;

namespace Am.I.Online.Api.Tests.Controllers;

public class PingControllerTests : BaseApiTests
{
  [Test]
  public void Get_WhenCalled_ShouldReturnEnvironment()
  {
    // arrange
    var pingController = new PingController();
    // action
    var pingResponse = pingController.Get();
    // assert
    pingResponse.Env.Should().Be("local");
  }


  [Test]
  public void Get_WhenCalled_ShouldReturnVersion()
  {
    // arrange
    var pingController = new PingController();
    // action
    var pingResponse = pingController.Get();
    // assert
    pingResponse.Version.Should().Be("1.0.0");
  }

  [Test]
  public void Get_WhenCalled_ShouldReturnMachineName()
  {
    // arrange
    var pingController = new PingController();
    // action
    var pingResponse = pingController.Get();
    // assert
    pingResponse.MachineName.Should().NotBeEmpty();
  }

  [Test]
  [Category("Integration")]
  public async Task Insert_GivenUserAndSampleSampleData_ShouldAddReturnCountInt()
  {
    // arrange
    var client = Api.CreateClient();
    //action
    var add = await client.GetAsync($"/api/ping");
    // assert
    add.StatusCode.Should().Be(HttpStatusCode.OK);
  }
}
