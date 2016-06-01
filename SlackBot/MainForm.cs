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
		public MainForm()
		{
			InitializeComponent();
			SlackBot bot = new SlackBot("maboojjang.irc.slack.com", "codingslave", "#general", "maboojjang.VLRj0y3mcO9a9xGDjrvl");
		}
	}
}
