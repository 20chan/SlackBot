using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlackAPI
{
    public struct Message
    {
        public string FullMessage { get; private set; }

        public Message(string message)
        {
            FullMessage = message;
        }

        public string[] GetMentioned()
        {
            List<string> result = new List<string>();
            FullMessage.IndexOf('@');
            throw new NotImplementedException();
        }
    }
}
