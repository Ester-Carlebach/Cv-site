using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public static class Exstensions
    {
        public static void AddGitHubIntegration(this IServiceCollection services,Action<GitHubIntegrationOption> configuration)
        {
            services.Configure(configuration);
         }
    }
}
