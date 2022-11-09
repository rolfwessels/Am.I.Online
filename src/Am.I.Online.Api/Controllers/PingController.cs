using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace Am.I.Online.Api.Controllers;

[ApiController]
[Route(Constants.DefaultRoute)]
public class PingController : ControllerBase
{
  private readonly string _environmentName;
  private readonly string? _version;

  public PingController()
  {
    _environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLower() ?? "local";
    _version = FileVersionInfo.GetVersionInfo(GetType().Assembly.Location).ProductVersion!;
  }

  [HttpGet()]
  public PingResponse Get()
  {
    return new PingResponse(_environmentName, Environment.MachineName, _version ?? "0.0.0");
  }

  public record PingResponse(string Env, string MachineName, string Version);
}
