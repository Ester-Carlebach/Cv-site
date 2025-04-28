using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;

using Microsoft.Extensions.Options;
using Octokit;
namespace Service
{
    public class GitHubService : IGitHubService
    {

        public readonly GitHubClient _client;
        public readonly GitHubIntegrationOption _option;

        public GitHubService( IOptions<GitHubIntegrationOption> option)
        {
            try
            {
                _client = new GitHubClient(new ProductHeaderValue("my-github-app"));
                _option = option.Value;
                try
                 {
                    Credentials tokenAuth=null;
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
            catch (Exception ex)
            {
                throw new Exception("Failed to create GitHubClient.", ex);
            }
        }
        //פונקציות ממלכה ברוק
        public async Task<int> GetUserFollowersAsync(string userName)
        {
            var user = await _client.User.Get(userName);
            Console.WriteLine($"User: {user.Login}, Name: {user.Name}");

            return user.PublicRepos;
        }


        public async Task<List<Repository>> SearchRepositoriesInCSharp()
        {
            var request = new SearchRepositoriesRequest("repo-name") { Language = Language.CSharp };
            var result = await _client.Search.SearchRepo(request);
            return result.Items.ToList();
        }
        //מחזירה את רשימת ה-repositories שלך מ-GitHub כולל המידע הבא עבור כל repository: שפות פיתוח, קומיט אחרון, מס’ כוכבים, מס’ pull-requests, קישור לאתר.
        public async Task<List<RepositoryInfo>> GetPortfolio()
        {
            List<RepositoryInfo> repositoryInfos = new List<RepositoryInfo>();
            IReadOnlyList<Repository> repos = await _client.Repository.GetAllForCurrent();

            foreach (var item in repos)
            {
                RepositoryInfo r = new RepositoryInfo();
                r.Url = item.GitUrl;
                r.Name = item.Name;
                r.Stars = item.StargazersCount;
                Task<IReadOnlyList<PullRequest>> pullRequests = _client.PullRequest.GetAllForRepository(item.Id);
                IReadOnlyList<PullRequest> pullRequests1 = await pullRequests;
                r.PullRequestsCount = pullRequests1.Count();
                r.LastCommit = item.PushedAt;
                Task<IReadOnlyList<RepositoryLanguage>> myLang = _client.Repository.GetAllLanguages(item.Id);
                IReadOnlyList<RepositoryLanguage> languages = await myLang;
                foreach (var lan in languages)
                {
                    r.Languages.Add(lan.Name);
                }
                repositoryInfos.Add(r);
            }
            return repositoryInfos;
        }

        //פונקצית עזר להחזרת רפוסיטוריס ע"פ ת.ז
        public async Task<IReadOnlyList<RepositoryLanguage>> GetLanAsync(long Id)
        {
            Task<IReadOnlyList<RepositoryLanguage>> myLang = _client.Repository.GetAllLanguages(Id);
            IReadOnlyList<RepositoryLanguage> languages = await myLang;
            return languages;
        }
        //חיפוש רפוזיטורי ע"פ מאפיינים אופציונלים
        public async Task<List<Repository>> SearchRepositories(string? repository, string? user, string? lang)
        {
            IReadOnlyList<Repository> repositories1;
            
            repositories1 = await _client.Repository.GetAllForCurrent();
            List<Repository> repositories2 = new List<Repository>();
            if (repository != null)
                repositories1 = repositories1.Where(x => x.Name.Equals(repository)).ToList();
            if (user != null)
                repositories1 = repositories1.Where(x =>
                {
                    Console.WriteLine(x.FullName.Substring(0, x.FullName.LastIndexOf("/")));
                    return x.FullName.Substring(0,x.FullName.LastIndexOf("/")
                    ).Equals(user);
                }).ToList();
            if (lang != null)
            {
                foreach (var item in repositories1)
                {

                    IReadOnlyList<RepositoryLanguage> repos = await GetLanAsync(item.Id);
                    bool flag = false;
                    foreach (var item1 in repos)
                    {
                        if (item1.Name.Equals(lang))
                            flag = true;
                    }
                    if (flag)
                    {
                        repositories2.Add(item);
                    }


                }
            }
            else
            {
                foreach (var item in repositories1)
                {
                    repositories2.Add(item);
                }
            }
            return repositories2;

        }


    }
}
