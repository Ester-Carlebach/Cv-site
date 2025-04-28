using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class RepositoryInfo
    {
        public string Name { get; set; }
        public List<string> Languages { get; set; } = new List<string>();
        public DateTimeOffset? LastCommit { get; set; }
        public int Stars { get; set; }
        public int PullRequestsCount { get; set; }
        public string Url { get; set; }

        public override string ToString()
        {
            string s= "-----------------\n"+Name;
            foreach (var item in Languages)
            {
                s += item + ",";
            }
            s += "\n" + LastCommit + "\n" + Stars + "\n" + PullRequestsCount + "\n" + Url;
            return s;
        }
    }
}
