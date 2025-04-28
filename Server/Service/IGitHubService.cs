using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IGitHubService
    {
        public Task<int> GetUserFollowersAsync(string userName);
        public Task<List<Repository>> SearchRepositoriesInCSharp();
        public  Task<List<RepositoryInfo>> GetPortfolio();
       public Task<List<Repository>> SearchRepositories(string? repository,string? user,string? lang);

    }
}
