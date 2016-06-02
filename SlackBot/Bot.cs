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
		private Data _data;
		
		public SlackBot(string server, string channel, string nick, string password)
		{
			this._slack = new Slack(server, channel, nick, password);
			this._data = new Data();
			
			_slack.Connected += (b) => { _slack.SendMessages( _slack.Channel, new string[] { "> 코딩노예 봇 시작!", "> 사용법은 !help 을 입력해보세요!" }); };
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
			if(command == "help" || command == "도움말")
			{
				string[] help = new string[] {
					"IRC SSL 연결을 사용하여 제작된 오픈소스 슬랙 봇입니다. 다양한 커맨드를 지원합니다.",
					"깃헙 : https://github.com/PhillyAI/SlackBot",
					"명령어는 !으로 시작하여 입력합니다.",
					"명령어 목록을를 보려면 !commands 를 입력해주세요."
				};
				_slack.SendMessages(channel, help);
			}
			else if(command == "command" || command == "cmd" || command == "명령어")
			{
				string[] commands = new string[] {
					"!help !도움말 : 도움말을 보여줍니다.",
					"!command !cmd !명령어 : 명령어 목록을 보여줍니다.",
					"!hs {code} !haskell {code} !하스켈 {code} : 하스켈 코드를 실행시킨 후 결과를 리턴합니다."
				};
				_slack.SendMessages(channel, commands);
			}
			else if(command == "hs" || command == "haskell" || command == "하스켈")
			{
				string[] result = Interpreter.InterpreteHaskell(message);
				_slack.SendMessages(channel, result);
			}
			else if(command == "날씨")
			{
				string[] result = Weather.GetWeather(message);
				_slack.SendMessages(channel, result);
			}
		}
		
		public void Save(string path)
		{
			this._data.Save(path);
		}
		
		public void Open(string path)
		{
			this._data = Data.FromFile(path);
		}
	}
}
