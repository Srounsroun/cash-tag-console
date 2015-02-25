using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces;

namespace CashTag
{
    public class Tag
    {
        public List<ITweet> Tweets { get; set; }

        public string Name { get; set; }

        public Tag(string name)
        {
            this.Name = name;
            this.Tweets = new List<ITweet>();

            var searchParameter = Search.GenerateTweetSearchParameter(name);
            searchParameter.SearchType = SearchResultType.Popular;
            searchParameter.MaximumNumberOfResults = 100;
            searchParameter.Since = DateTime.Now.AddDays(-5);
            this.Tweets = Search.SearchTweets(searchParameter).ToList();
        
        }
    }
}
