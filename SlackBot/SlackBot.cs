using System;
using System.Linq;
using System.Reflection;
using Slack.SlackAPI;

namespace Slack.SlackBot
{
    public class SlackBot
    {
        private struct MessageCallback
        {
            public Type Type;
            public MethodInfo Method;
        }
        private struct MentionCallback
        {
            public Type Type;
            public MethodInfo Method;
            public string ID;
        }

        private MentionCallback[] _userMentionedCallback;
        private MessageCallback[] _messageCallback;
        public SlackBot(SlackClient slack)
        {
            slack.GotMessage += Slack_GotMessage;
        }

        public void BindCallback(Assembly assembly = null, BindingFlags flags = 0)
        {
            _userMentionedCallback = (from ass in assembly == null ? AppDomain.CurrentDomain.GetAssemblies() : new Assembly[] { assembly }
                                     from type in ass.GetTypes()
                                     from method in type.GetMethods(flags)
                                     where method.IsDefined(typeof(UserMentioned))
                                     select new MentionCallback { Type = type, Method = method, ID = method.GetCustomAttribute<UserMentioned>().ID }).ToArray();

            _messageCallback = (from ass in assembly == null ? AppDomain.CurrentDomain.GetAssemblies() : new Assembly[] { assembly }
                                from type in assembly.GetTypes()
                                from method in type.GetMethods(flags)
                                where method.IsDefined(typeof(GotMessage))
                                select new MessageCallback { Type = type, Method = method }).ToArray();
        }

        private void Slack_GotMessage(string channel, string nickname, string message)
        {
            if (_messageCallback != null)
                foreach (var m in _messageCallback)
                {
                    m.Method.Invoke(m.Type, new object[] { channel, nickname, message });
                }
            if (_userMentionedCallback != null)
            {
                var mentioned = Parser.GetMentionedIDs(message);
                if (mentioned.Count() > 0)
                {
                    foreach (var m in _userMentionedCallback)
                    {
                        if (!mentioned.Contains(m.ID))
                            continue;
                        m.Method.Invoke(m.Type, new object[] { channel, nickname, message });
                    }
                }
            }
        }

        [AttributeUsage(AttributeTargets.Method)]
        public class GotMessage : Attribute
        {

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
