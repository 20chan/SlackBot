/*
 * 사용자: philly
 * 날짜: 2016-06-01
 * 시간: 오후 1:50
 */
using System;
using System.Collections.Generic;
using ChatBot;

namespace SlackBot
{
	public class SlackBot
	{
		private Slack _slack;
		private Data _data;
		
		public event Action<List<string>> AdminChanged;
		public event Action<List<string>> BanChanged;
		public event Action<string, string, string> GotMessage;
		
		public List<string> AdminList { get { return _data.AdminList; }}
		public List<string> BanList { get { return _data.BanList; }}
		
		public string Nickname { get { return _slack.Nickname; }}
		
		private CBR ai_bot = new CBR();
		
		public SlackBot(string server, string nick, string channel, string password)
		{
			this._slack = new Slack(server, nick, channel, password);
			this._data = new Data();
			
			_slack.Connected += (b) => { SendMessages( _slack.Channel, new string[] { "> 코딩노예 봇 시작!", "> 사용법은 !help 을 입력해보세요!" }); };
			_slack.GotMessage += _slack_GotMessage;
			
			_data.AdminChanged += (List<string> obj) => { if(AdminChanged != null) AdminChanged(obj); };
			_data.BanChanged += (List<string> obj) => { if(BanChanged != null) BanChanged(obj); };
		}

		public void _slack_GotMessage(string channel, string nick, string message)
		{
			if(GotMessage != null)
			{
				GotMessage(channel, nick, message);
			}
			if(message.StartsWith("!"))
			{
				try
				{
					string[] msg = message.Split(null);
					Execute(channel, nick, msg[0].Substring(1), msg.Length >= 2 ? message.Substring(msg[0].Length + 1) : "");
				}
				catch
				{
					SendMessage(channel, "잘못된 사용법입니다.");
				}
			}
		}
		
		public void Execute(string channel, string nick, string command, string message)
		{
			if(BanList.Contains(nick))
			   return;
			if(command == "도움말" || command == "help")
			{
				string[] help = new string[] {
					"IRC SSL 연결을 사용하여 제작된 오픈소스 슬랙 봇입니다. 다양한 커맨드를 지원합니다.",
					"깃헙 : https://github.com/PhillyAI/SlackBot",
					"명령어는 !으로 시작하여 입력합니다.",
					"명령어 목록을를 보려면 !commands 를 입력해주세요."
				};
				SendMessages(channel, help);
			}
			else if(command == "commands" || command == "cmd" || command == "명령어")
			{
				string[] commands = new string[] {
					"!help !도움말 : 도움말을 보여줍니다.",
					"!command !cmd !명령어 : 명령어 목록을 보여줍니다.",
					"!hs {코드} !haskell {코드} !하스켈 {코드} : 하스켈 코드를 실행시킨 후 결과를 보여줍니다.",
					"!계산 {식} : 식을 계산합니다.",
					"!weather {지역} !날씨 {지역} : 날씨를 기상청에서 파싱해서 보여줍니다.",
					"!저장 {키} {값} : 키에 값을 저장합니다.",
					"!불러 {키} : 키에 저장된 값을 보여줍니다.",
					"!학습 {질문} | {응답} : 인공지능 봇에 학습을 시킵니다.",
					"!봇 {질문} : 봇에게 말을 겁니다."
				};
				SendMessages(channel, commands);
			}
			else if(command == "하스켈" || command == "haskell" || command == "hs")
			{
				string[] result = Interpreter.InterpreteHaskell(message);
				SendMessages(channel, result);
			}
			else if(command == "날씨" || command == "weather")
			{
				string[] result = Weather.GetWeather(message);
				SendMessages(channel, result);
			}
			else if(command == "저장" || command == "save")
			{
				try
				{
					string[] sp= message.Split(null);
					_data.SetTable(sp[0], message.Substring(sp[0].Length + 1));
					SendMessage(channel, "성공적으로 저장했습니다.");
				}
				catch
				{
					SendMessage(channel, "잘못된 사용법입니다.");
				}
			}
			else if(command == "불러" || command == "load")
			{
				string value = _data.GetTable(message);
				SendMessage(channel, value);
			}
			else if(command == "반복" || command == "repeat" || command == "re")
			{
				if(_data.AdminList.Contains(nick))
				{
					string[] sp = message.Split(null);
					int times = Convert.ToInt32(sp[0]);
					string cmd = message.Substring(sp[0].Length + 1);
					try
					{
						for(int i = 0; i < Convert.ToInt32(sp[0]); i++)
							_slack_GotMessage(channel, nick, cmd);
					}
					catch
					{
						SendMessage(channel, "잘못된 사용법입니다.");
					}
				}
				else
				{
					SendMessage(channel, "권한이 없습니다.");
				}
			}
			else if(command == "계산" || command == "calc")
			{
				SendMessage(channel, Interpreter.Calc(message));
			}
			else if(command == "학습")
			{
				try
				{
					string[] sp = message.Split('|');
					ai_bot.AddConversation(sp[0], sp[1]);
					SendMessage(channel, "성공적으로 학습시켰습니다.");
				}
				catch
				{
					SendMessage(channel, "잘못된 사용법입니다.");
				}
			}
			else if(command == "봇")
			{
				SendMessage(channel, "봇 : " + ai_bot.Eval(message));
			}
		}
		
		public void SendMessage(string channel, string message)
		{
			_slack.SendMessage(channel, message);
		}
		
		public void SendMessages(string channel, string[] messages)
		{
			_slack.SendMessages(channel, messages);
		}
		
		public void Save(string path)
		{
			this._data.Save(path);
		}
		
		public void Open(string path)
		{
			this._data = Data.FromFile(path);
		}
		
		public void AddAdmin(string id)
		{
			_data.AdminList.Add(id);
			AdminChanged(AdminList);
		}
		
		public void RemoveAdmin(string id)
		{
			_data.AdminList.Remove(id);
			AdminChanged(AdminList);
		}
		
		public void AddBan(string id)
		{
			_data.BanList.Add(id);
			BanChanged(BanList);
		}
		
		public void RemoveBan(string id)
		{
			_data.BanList.Remove(id);
			BanChanged(BanList);
		}
		
		public void SaveAIBot(string path)
		{
			ai_bot.SaveTable(path);
		}
		
		public void LoadAIBot(string path)
		{
			ai_bot.LoadTable(path);
		}
	}
}
