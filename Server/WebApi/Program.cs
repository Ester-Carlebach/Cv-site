using Octokit;
using Service;
using Microsoft.Extensions.Configuration;
using Github.Api.CachedSevise;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMemoryCache();
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<GitHubIntegrationOption>(builder.Configuration.GetSection(nameof(GitHubIntegrationOption)));
builder.Services.AddGitHubIntegration(option => builder.Configuration.GetSection(nameof(GitHubIntegrationOption)).Bind(option));
// הוספת GitHubClient \למיכל ההזרקה
builder.Services.AddScoped<GitHubClient>(provider =>
    new GitHubClient(new ProductHeaderValue("my-github-app")));
builder.Services.AddScoped<IGitHubService, GitHubService>();
builder.Services.Decorate<IGitHubService, CachedGitHubService>();
   
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
