using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octokit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Service
{
    public class GitHubIntegrationOption
    {
        public string UserName { get; set; }
        public string Token { get; set; }

    }
}
