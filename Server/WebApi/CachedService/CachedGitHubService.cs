using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Octokit;
using Service;

namespace Github.Api.CachedSevise
{
    public class CachedGitHubService : IGitHubService
    {

        private readonly IGitHubService _gitHubService;
        private readonly IMemoryCache _memoryCache;
        public readonly GitHubClient _client;
        public readonly GitHubIntegrationOption _option;
        private const string userPortfolioKey = "userPortfolioKey";

        public CachedGitHubService(IGitHubService gitHubService, IMemoryCache memoryCache, GitHubClient client, IOptions<GitHubIntegrationOption> option)
        {
            _gitHubService = gitHubService;
            _memoryCache = memoryCache;
            _client = client;
            _option = option.Value;
            try
            {
                Credentials tokenAuth = null;
                if (_option.Token is not null)
                {
                    tokenAuth = new Credentials(_option.Token);

                }
                _client.Credentials = tokenAuth;
            }
            catch (Exception ex)

            {
                throw new Exception("Failed to create GitHubClient.", ex);

            }
        }
     

        public async Task<List<RepositoryInfo>> GetPortfolio()
        {
            if (_memoryCache.TryGetValue(userPortfolioKey, out List<RepositoryInfo> portfolio))
                return portfolio;
            var cacheOption = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(300))
                .SetSlidingExpiration(TimeSpan.FromSeconds(100));
            portfolio = await _gitHubService.GetPortfolio();
            _memoryCache.Set(userPortfolioKey, portfolio, cacheOption);
            return portfolio;
        }

        public async Task<int> GetUserFollowersAsync(string userName)
        {
         var e=await  _client.Activity.Events.GetAllUserPerformed(_option.UserName);
            Console.WriteLine(e);
            return await _gitHubService.GetUserFollowersAsync(userName);
        }

        public async Task<List<Repository>> SearchRepositories(string? repository, string? user, string? lang)
        {
            if (_memoryCache.TryGetValue(userPortfolioKey, out List<Repository> portfolio))
                return portfolio;
            portfolio = await _gitHubService.SearchRepositories(repository, user, lang);
            _memoryCache.Set(userPortfolioKey, portfolio);
            return portfolio;
        }

        public Task<List<Repository>> SearchRepositoriesInCSharp()
        {
            return _gitHubService.SearchRepositoriesInCSharp();
        }
    }
}
