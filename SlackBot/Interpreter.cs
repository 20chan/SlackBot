/*
 * 사용자: philly
 * 날짜: 2016-06-01
 * 시간: 오후 1:55
 */
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using MSScriptControl;

namespace SlackBot
{
	public static class Interpreter
	{
		public static string[] InterpreteHaskell(string code)
		{
			string[] result = new string[]{};
			Thread t = new Thread(
				() =>
				{
					result = _interpreteHaskell(code);
				});
			var tmr = new System.Timers.Timer(5000);
			tmr.Elapsed += (object sender, System.Timers.ElapsedEventArgs e) => t.Interrupt();
			tmr.Start();
			t.Start();
			t.Join();
			return result;
		}
		
		private static string[] _interpreteHaskell(string code)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            Process process = new Process();
			try
			{
	            info.FileName = "ghci";
	            info.WindowStyle = ProcessWindowStyle.Hidden;
	            info.CreateNoWindow = true;
	            info.UseShellExecute = false;
	            info.RedirectStandardError = true;
	            info.RedirectStandardInput = true;
	            info.RedirectStandardOutput = true;
	            info.WorkingDirectory = @"C:\";
	            process.EnableRaisingEvents = false;
	            process.StartInfo = info;
	            process.Start();
	            process.StandardInput.Write(code + Environment.NewLine);
	            process.StandardInput.Close();
	            
	            if(!process.WaitForExit(5000))
	            {
	            	process.Kill();
	            	throw new ThreadInterruptedException();
	            }
	            
	            string str = process.StandardOutput.ReadToEnd();
	            return str.Split(new char[] { '\n' });
			}
			catch(ThreadInterruptedException)
			{
				try
				{
					process.Kill();
		            Process ghc = Process.GetProcessesByName("ghc")[0];
		            ghc.Kill();
					return new string[] { "실행이 너무 오래 걸립니다." };
				}
				catch
				{
					return new string[] { "실행이 너무 오래 걸립니다." };
				}
			}
			finally
			{
				process.Dispose();
			}
        }
		
		public static string Calc(string code)
		{
			MSScriptControl.ScriptControl sc = new MSScriptControl.ScriptControl();
			sc.Language = "VBScript";
			return sc.Eval(code).ToString();
		}
	}
}
