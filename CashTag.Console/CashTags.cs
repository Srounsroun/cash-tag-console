using ConsoleTables.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Streaminvi;

namespace CashTag
{
    public class CashTags
    {
        public Dictionary<string,Tag> Tags { get; set; }

        public IFilteredStream FilteredStream { get; set; }

        public CashTags(IEnumerable<string> tags)
        {
            this.Tags = new Dictionary<string,Tag>();

            FilteredStream = Stream.CreateFilteredStream();

            foreach (var tag in tags)
            {
                if (!Tags.Keys.Contains(tag))
                {
                    this.Tags.Add(tag, new Tag(tag));
                    FilteredStream.AddTrack(tag);
                    Thread.Sleep(100);
                }
            }

            FilteredStream.MatchingTweetReceived += filteredStream_MatchingTweetReceived;

            WriteConsole();
        }

        public void Stop()
        {
            FilteredStream.StopStream();
        }

        public void Start()
        {
            FilteredStream.StartStreamMatchingAllConditions();
            WriteConsole();
        }

        void filteredStream_MatchingTweetReceived(object sender, Tweetinvi.Core.Events.EventArguments.MatchedTweetReceivedEventArgs e)
        {
            foreach(var track in e.MatchingTracks)
            {
                if(this.Tags.Keys.Contains(track))
                    this.Tags[track].Tweets.Add(e.Tweet);
            }

            WriteConsole();
        }

        public void WriteConsole()
        {
            Console.Clear();

            var now = DateTime.Now;
            Console.WriteLine("CASH TAG - " + now.ToShortDateString() + " " + now.ToLongTimeString());

            //var table = new ConsoleTable("Tag", "Tweets count", "Last Updated");
            var tags = new List<Tag>();
            foreach (var key in this.Tags.Keys)
            {
                tags.Add(this.Tags[key]);
            }

            foreach (var tag in tags.OrderByDescending(x => x.Tweets.Count()))
            { 
                var lastUpdateString = "";

                if(tag.Tweets.Count > 0)
                {
                    var lastUpdate = tag.Tweets.Max(x => x.CreatedAt);
                    lastUpdateString =  " - " + lastUpdate.ToShortDateString() + " " + lastUpdate.ToLongTimeString();

                    Console.WriteLine(tag.Name + " " + tag.Tweets.Count() + " " + lastUpdateString);
                    Console.WriteLine();

                    if (tag.Tweets.Count > 0)
                        Console.WriteLine(tag.Tweets[tag.Tweets.Count - 1].Text);

                    Console.WriteLine(" ------------ ");
                }

            }

            //table.Write();

            Console.WriteLine();
            Console.WriteLine("Press any key to stop this!");
        }

    }
}
