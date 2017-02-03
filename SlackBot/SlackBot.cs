using System;
using System.Linq;
using System.Reflection;
using Slack.SlackAPI;

namespace Slack.SlackBot
{
    public class SlackBot
    {
        public SlackBot(SlackClient slack)
        {
            slack.GotMessage += Slack_GotMessage;
        }

        private void Slack_GotMessage(string channel, string nickname, string message)
        {
            var mentioned = Parser.GetMentionedIDs(message);
            if (mentioned.Length > 0)
            {
                var usermentioned = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                                    from type in assembly.GetTypes()
                                    from method in type.GetMethods()
                                    where method.IsDefined(typeof(UserMentioned))
                                    select new { Type = type, Method = method, ID = method.GetCustomAttribute<UserMentioned>().ID };

                foreach (var m in usermentioned)
                {
                    if (!mentioned.Contains(m.ID))
                        continue;
                    m.Method.Invoke(m.Type, new object[] { channel, nickname, message });
                }
            }
        }

        [AttributeUsage(AttributeTargets.Method)]
        public class UserMentioned : Attribute
        {
            public string ID { get; set; }
            public UserMentioned(string id)
            {
                ID = id;
            }
        }
    }
}
