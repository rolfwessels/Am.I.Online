using Am.I.Online.Api;
using Prometheus;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((_, lc) => lc
  .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
  .WriteTo.Console());
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<Settings>();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<MonitorValues>();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();
app.MapControllers();
app.UseEndpoints(e => e.MapMetrics("stats/metrics"));
app.Run();

namespace Am.I.Online.Api
{
  public partial class Program
  {
  }
}
