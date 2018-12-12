using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Start
{
    public class CookieAwareWebClient : WebClient
    {
        public CookieContainer CookieContainer { get; set; }
        public Uri Uri { get; set; }

        public CookieAwareWebClient()
            : this(new CookieContainer())
        {
        }

        public CookieAwareWebClient(CookieContainer cookies)
        {
            this.CookieContainer = cookies;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            if (request is HttpWebRequest)
            {
                (request as HttpWebRequest).CookieContainer = this.CookieContainer;
            }
            HttpWebRequest httpRequest = (HttpWebRequest)request;
            httpRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            return httpRequest;
        }

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            WebResponse response = base.GetWebResponse(request);
            String setCookieHeader = response.Headers[HttpResponseHeader.SetCookie];

            if (setCookieHeader != null)
            {
                //do something if needed to parse out the cookie.
                if (setCookieHeader != null)
                {
                    Cookie cookie = new Cookie(); //create cookie
                    this.CookieContainer.Add(cookie);
                }
            }
            return response;
        }
    }

    public class members
    {
        public int stars;
        public string name;
        public int local_score;
        public Dictionary<int, Dictionary<string,  KeyValuePair<string, int>>> completion_day_level;
        public int last_star_ts;
        public int id;
    }


    public class LocalLeaderboard
    {
        public LocalLeaderboard()
        {
            @members = new Dictionary<string, members>();
        }
        public int @event;
        public Dictionary<string, members> @members;
        public int owner_id;

    }



    public class Leaderboard
    {

        LocalLeaderboard collection = new LocalLeaderboard();


        public void GetLeaderBoard()
        {
            CookieContainer cookieJar = new CookieContainer();
            cookieJar.Add(new Cookie("_ga", "GA1.2.374532528.1543674237", "/", ".adventofcode.com"));
            cookieJar.Add(new Cookie("_gid", "GA1.2.918319719.1544394264", "/", ".adventofcode.com"));
            cookieJar.Add(new Cookie("session", 
                "53616c7465645f5f27b0909eb624c88d32fc1bc1cbe202ac1880011b76672a8d3f06fef9dfd9cdc4797043737651bc1f",
                "/", ".adventofcode.com"));

            CookieAwareWebClient client = new CookieAwareWebClient(cookieJar);


            //Members = new List<Member>();

            client.DownloadFile("https://adventofcode.com/2018/leaderboard/private/view/402426.json", "Input\\402426.json");
            string json = "Input\\402426.json";
            //Console.WriteLine(json);
            using (StreamReader file = new StreamReader(json))
            {
                json = file.ReadToEnd();
                // Convert JSON to a series of objects
                collection = JsonConvert.DeserializeObject<LocalLeaderboard>(json);
                // Do computation
                //Console.WriteLine(collection.AllMembers.Count());
            }

            ////var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            collection.members.OrderBy(a => a.Value.local_score);

            foreach (var member in collection.members)
            {
                Console.WriteLine($"Name:\t {member.Value.name}");
                Console.WriteLine($"ID:\t {member.Value.id}");
                Console.WriteLine($"Stars:\t {member.Value.stars}");
                Console.WriteLine($"Local Score:\t {member.Value.local_score}");
                Console.WriteLine($"Last Star timestamp:\t " +
                    $"{DateTimeOffset.FromUnixTimeSeconds(member.Value.last_star_ts).DateTime.ToLocalTime()}");
                Console.WriteLine("                  1 1");
                Console.WriteLine("1 2 3 4 5 6 7 8 9 0 1");

                for(int i = 1; i < 25; i++)
                {
                    if (member.Value.completion_day_level.ContainsKey(i))
                        Console.Write("* ");
                    else
                        Console.Write("  ");
                }

                Console.WriteLine();
                


            }

            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
        }

        // First get members from json file


    }




}
