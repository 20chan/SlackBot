using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;

namespace Slack.SlackAPI
{
    public static class Parser
    {
        private static Regex _mentionRegex;
        public static IEnumerable<string> GetMentionedIDs(string message)
        {
            if (_mentionRegex == null)
                _mentionRegex = new Regex("@[a-zA-Z1-9]*");
            foreach (Match m in _mentionRegex.Matches(message))
                yield return m.Value;
        }
    }
}
