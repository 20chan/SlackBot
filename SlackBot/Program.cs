/*
 * 사용자: philly
 * 날짜: 2016-06-01
 * 시간: 오후 1:30
 */
using System;
using System.Windows.Forms;

namespace SlackBot
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program
	{
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
		
	}
}
