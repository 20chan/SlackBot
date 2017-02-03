using Slack.SlackAPI;
using Slack.SlackBot;

namespace Slack.SlackBot.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] gateway = System.IO.File.ReadAllLines("gateways.txt");
            SlackClient client = new SlackClient(gateway[0], gateway[1], gateway[2]);
            SlackBot bot = new SlackBot(client);
        }
    }
}
