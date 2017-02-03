using System;
using Slack.SlackAPI;
using Slack.SlackBot;

namespace Slack.SlackBot.Test
{
    class Program
    {
        static SlackClient client;
        static SlackBot bot;
        static void Main(string[] args)
        {
            string[] gateway = System.IO.File.ReadAllLines("gateways.txt");
            client = new SlackClient(gateway[0], gateway[1], gateway[2]);
            client.Connect();
            bot = new SlackBot(client);
            bot.BindCallback(System.Reflection.Assembly.GetExecutingAssembly(), System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            while (true)
                Console.ReadLine();
        }

        [SlackBot.GotMessage()]
        static void OnMessage(string channel, string nickname, string message)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"{channel} ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"{nickname} : ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{message}");
        }

        [SlackBot.UserMentioned("@slackbot")]
        static void OnMentioned(string channel, string nickname, string message)
        {
            client.SendMessage(channel, $"저기, @{nickname}, 여기서 슬랙봇은 금지에요.");
        }
    }
}
