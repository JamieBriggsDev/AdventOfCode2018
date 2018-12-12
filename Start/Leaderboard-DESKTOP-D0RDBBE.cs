using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Start
{
    public class members
    {
        public int stars;
        public string name;
        public int local_score;
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

    public class CookieAwareWebClient : WebClient
    {
        private readonly CookieContainer m_container = new CookieContainer();

        protected override WebRequest GetWebRequest(Uri address)
        {


            WebRequest request = base.GetWebRequest(address);
            HttpWebRequest webRequest = request as HttpWebRequest;
            if (webRequest != null)
            {
                webRequest.CookieContainer = m_container;
            }
            return request;
        }
    }

    public class Leaderboard
    {
      

        LocalLeaderboard collection = new LocalLeaderboard();

        //Account account;

        public void GetLeaderBoard()
        {
            //Members = new List<Member>();
            string destination = "https://adventofcode.com/2018/leaderboard/private/view/402426.json";

            CookieAwareWebClient webClient = new CookieAwareWebClient();
            string x = webClient.DownloadString(destination);

            foreach(var member in collection.members)
            {
                Console.WriteLine($"Name:\t {member.Value.name}");
                Console.WriteLine($"ID:\t {member.Value.id}");
                Console.WriteLine($"Stars:\t {member.Value.stars}");
                Console.WriteLine($"Local Score:\t {member.Value.local_score}");
                Console.WriteLine($"Last Star timestamp:\t " +
                    $"{DateTimeOffset.FromUnixTimeSeconds(member.Value.last_star_ts).DateTime.ToLocalTime()}");
                Console.WriteLine();


            }

            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
        }

        // First get members from json file


    }




}
