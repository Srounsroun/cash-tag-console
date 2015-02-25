using CommandLine;
using CommandLine.Text;
using ConsoleTables.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Logic.Model.Parameters;

namespace CashTag.SimpleConsole
{
    // Define a class to receive parsed values
    class Options
    {
       
        [Option('i', "input file path", Required = false, DefaultValue = @"cashtag.txt",
          HelpText = "Input file to be processed.")]
        public string InputFile { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }

    class Program
    { 

        static void Main(string[] args)
        {
            string LocalPath = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\").LocalPath;
           
            var access_token = "7611822-mxRQ6rojK9RyE8nJO0dJcK4PJVrEfvn8g6g1QIVdcA";
            var access_token_secret = "1Km6Zd0ZTQgu7mrL6nHTDt5idsdkK03MMh3pOoli9IfNr";

            var consumer_key = "Yy1zYgxWFRKdUWMuzPkqWyNjm";
            var consumer_secret = "59Q369BgteBFygj7nw00YZPRNBe15L9aXEQdDHO5LELB4OEsaQ";

            TwitterCredentials.SetCredentials(access_token, access_token_secret, consumer_key, consumer_secret);

            var tags = new List<string>();
            var options = new Options();

            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                var inputfile = options.InputFile;
                if (File.Exists(inputfile))
                {
                    Console.WriteLine("Opening \"" + inputfile + "\"");

                    string line;

                    // Read the file and display it line by line.
                    System.IO.StreamReader file = new System.IO.StreamReader(inputfile);

                    while ((line = file.ReadLine()) != null)
                    {
                        tags.Add(line);
                    }

                    file.Close();


                    Console.WriteLine("Found " + tags.Count() + " tag(s)");

                    Console.WriteLine("Press any key to Start!");

                    CashTags cashTags = new CashTags(tags);
                    Console.ReadLine();
                    cashTags.Start();

                    Console.ReadLine();
                    cashTags.Stop();
                }
                else
                    Console.WriteLine("No existing input file!" + inputfile);
            }

        }
    }
}
