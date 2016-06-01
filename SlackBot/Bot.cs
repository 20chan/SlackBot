/*
 * 사용자: philly
 * 날짜: 2016-06-01
 * 시간: 오후 1:50
 */
using System;
using System.Collections.Generic;

namespace SlackBot
{
	public class SlackBot
	{
		private Slack _slack;
		private List<string> _admins;
		private List<string> _banned;
		
		public SlackBot(string server, string channel, string nick, string password)
		{
			this._slack = new Slack(server, channel, nick, password);
			this._admins = new List<string>();
			this._banned = new List<string>();
			
			_slack.Connected += (b) => { _slack.SendMessages( _slack.Channel, new string[] { "> 코딩노예 봇 시작!", "> 사용법은 !도움말 을 입력해보세요!" }); };
			_slack.GotMessage += _slack_GotMessage;
		}

		private void _slack_GotMessage(string channel, string nick, string message)
		{
			if(message.StartsWith("!"))
			{
				try
				{
					string[] msg = message.Split(null);
					Execute(channel, nick, msg[0].Substring(1), msg.Length >= 2 ? message.Substring(msg[0].Length + 1) : "");
				}
				catch (Exception ex)
				{
					_slack.SendMessage(channel, "잘못된 사용법입니다.");
				}
			}
		}
		
		public void Execute(string channel, string nick, string command, string message)
		{
			if(command == "help")
			{
				string[] help = new string[] {
					"IRC SSL 연결을 사용하여 제작된 오픈소스 슬랙 봇입니다. 다양한 커맨드를 지원합니다.",
					"깃헙 : https://github.com/PhillyAI/SlackBot"
				};
				_slack.SendMessages(channel, help);
			}
			else if(command == "hs")
			{
				string[] result = Interpreter.InterpreteHaskell(message);
				_slack.SendMessages(channel, result);
			}
		}
	}
}
