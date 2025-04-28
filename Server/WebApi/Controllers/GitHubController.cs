using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Octokit;
using Service;

namespace Github.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GitHubController : ControllerBase
    {
        Service.IGitHubService GitHubSer;
        public GitHubController(Service.IGitHubService GitHubSer)
        {
            this.GitHubSer=GitHubSer;
        }
        [HttpGet]
        public async Task<int> get(string n)
        {

           return await GitHubSer.GetUserFollowersAsync(n);
        }
        [HttpGet("Portfolio")]
        public async Task<List<RepositoryInfo>> GetPortfolio()
        {

          return  await GitHubSer.GetPortfolio( );
        }
        [HttpGet("repo")]
        public async Task<List<Repository>> SearchRepositoriesInCSharp()
        {
            return await GitHubSer.SearchRepositoriesInCSharp();
        }
        [HttpGet("filter")]
        public async Task<List<Repository>> SearchRepositories(string? repository, string? user, string? lang)
        {
            return await GitHubSer.SearchRepositories(repository,user,lang);
        }
    }
}
