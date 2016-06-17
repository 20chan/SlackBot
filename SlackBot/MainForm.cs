/*
 * 사용자: philly
 * 날짜: 2016-06-01
 * 시간: 오후 1:30
 */
 
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SlackBot
{
	public partial class MainForm : Form
    {
        string server;
        string nick;
        string channel;
        string password;

        SlackBot bot;
		public MainForm()
		{
			CheckForIllegalCrossThreadCalls = false;
			InitializeComponent();
			bot = new SlackBot(server, nick, channel, password);
			bot.GotMessage += bot_GotMessage;
			bot.AdminChanged += bot_AdminChanged;
			bot.BanChanged += bot_BanChanged;
			
			bot.AddAdmin(bot.Nickname);
		}

		void bot_GotMessage(string chan, string nick, string msg)
		{
			string s = "(" + chan + ") " + nick + " : " + msg + "\r\n";
			this.tbChat.AppendText(s);
			this.tbChat.ScrollToCaret();
		}
		
		void bot_AdminChanged(List<string> obj)
		{
			this.lbAdmin.Items.Clear();
			foreach(string ad in obj)
			{
				this.lbAdmin.Items.Add(ad);
			}
		}

		void bot_BanChanged(List<string> obj)
		{
			this.lbBan.Items.Clear();
			foreach(string ban in obj)
			{
				this.lbBan.Items.Add(ban);
			}
		}
		
		void TbConsoleKeyDown(object sender, KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Enter)
			{
				try
				{
					string l = tbConsole.Lines[tbConsole.Lines.Length - 1];
					tbConsole.AppendText("\r\n");
					string[] sp = l.Split(null);
					Execute(sp[0], l.Substring(sp[0].Length + 1));
				}
				catch(Exception ex)
				{
					AppendText("ERROR : " + ex.Message, Color.Red);
				}
			}
		}
		
		private void Execute(string command, string message)
		{
			try
			{
				switch(command)
				{
					case "admin_add":
						bot.AddAdmin(message);
						break;
					case "admin_remove":
						bot.RemoveAdmin(message);
						break;
					case "ban_add":
					case "ban":
						bot.AddBan(message);
						break;
					case "unban":
					case "ban_remove":
						bot.RemoveBan(message);
						break;
					case "send":
						bot.SendMessage(message.Split(null)[0], message.Substring(message.Split(null)[0].Length + 1));
						break;
					case "run":
						bot.SendMessage(message.Split(null)[0], message.Substring(message.Split(null)[0].Length + 1));
						bot._slack_GotMessage(message.Split(null)[0], bot.Nickname, message.Split(null)[1]);
						break;
					default:
						AppendText("No commands like that.", Color.Red);
						break;
				}
			}
			catch(Exception ex)
			{
				AppendText("ERROR : " + ex.Message, Color.Red);
			}
		}
		
		private void AppendText(string text, Color color)
        {
            if (string.IsNullOrWhiteSpace(text)) return;
            this.tbConsole.SelectionStart = this.tbConsole.TextLength;
            this.tbConsole.SelectionLength = 0;

            this.tbConsole.SelectionColor = color;
            this.tbConsole.AppendText(text);
            this.tbConsole.SelectionColor = this.tbConsole.ForeColor;

            this.tbConsole.Select(this.tbConsole.TextLength, 0);
            this.tbConsole.ScrollToCaret();
        }
		
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			bot.SendMessage("#general", "봇을 종료합니다..");
		}
	}
}
