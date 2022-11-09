using Bumbershoot.Utilities;

namespace Am.I.Online.Api;

public class Settings : BaseSettings
{
  public Settings(IConfiguration configuration) : base(configuration, "")
  {
  }

  public int DelayInSeconds => ReadConfigValue("DelayInSeconds", 10);
  public string PingHost => ReadConfigValue("PingHost", "http://ipv4.icanhazip.com");
}
