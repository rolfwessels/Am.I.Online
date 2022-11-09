using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Am.I.Online.Api.Controllers;
using Bumbershoot.Utilities.Helpers;
using Prometheus;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Am.I.Online.Api;

public class MonitorValues : BackgroundService,IDisposable
{
  private static readonly Gauge RequestTime = Metrics
    .CreateGauge("aimionline_request_time", "Request time.");

  private static readonly Counter IpCounterChanged = Metrics
    .CreateCounter("aimionline_ip_changed", "Ip changed.");

  private static readonly ILogger _log = Log.ForContext(MethodBase.GetCurrentMethod()!.DeclaringType!);
  private readonly Settings _settings;
  private readonly HttpClient _httpClient;
  private string _oldIp = "";
  private DateTime? _lastStop = null;


  public MonitorValues(Settings settings)
  {
    _settings = settings;
    _httpClient = new HttpClient();
    
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    
    while (!stoppingToken.IsCancellationRequested)
    {
      var stopwatch = Stopwatch.StartNew();
      var hasError = await PlaceRequest(stoppingToken, _httpClient);
      stopwatch.Stop();
      LogServerDown(hasError);
      var stopwatchElapsedMilliseconds = hasError ? 0 : stopwatch.ElapsedMilliseconds;
      RequestTime.Set(stopwatchElapsedMilliseconds);
      _log.Debug("Called {0}", stopwatchElapsedMilliseconds);
      await Task.Delay(TimeSpan.FromSeconds(_settings.DelayInSeconds), stoppingToken);
    }
  }

  private void LogServerDown(bool hasError)
  {
    if (hasError)
    {
      if (_lastStop == null)
      {
        _log.Information("Server went down");
        _lastStop = DateTime.Now;
      }
    }
    else
    {
      if (_lastStop != null)
      {
        var sinceDown = DateTime.Now - _lastStop.Value;
        _log.Information("Server was down for  {0}", sinceDown.ShortTime());
        _lastStop = null;
      }
    }
  }

  private async Task<bool> PlaceRequest(CancellationToken stoppingToken, HttpClient httpClient)
  {
    var hasError = false;
    try
    {
      var ip = await httpClient.GetStringAsync(_settings.PingHost, stoppingToken);
      if (_oldIp != ip)
      {
        if (_oldIp == string.Empty) IpCounterChanged.Inc();
        _oldIp = ip;
        _log.Information("Ip changed: {oldIp}", _oldIp.Trim());
      }
    }
    catch (Exception e)
    {
      hasError = true;
      _log.Warning("Err:{0}", e.Message);
    }

    return hasError;
  }
}


