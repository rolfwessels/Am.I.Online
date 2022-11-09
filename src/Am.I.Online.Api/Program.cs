using Am.I.Online.Api.GraphQL;
using Am.I.Online.Api.GraphQL.Movies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// builder.Services.AddSingleton<QueryDataLoader>();
builder.Services.AddGraphQLServer()
  .AddQueryType<QueryType>()
  .AddType<ActorType>()
  .AddType<MovieType>()
  .AddDataLoader<ActorDataLoader>()
  ;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();
app.MapGraphQL();

app.Run();

namespace Am.I.Online.Api
{
  public partial class Program
  {
  }
}
