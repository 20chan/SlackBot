/*
 * 사용자: philly
 * 날짜: 2016-06-01
 * 시간: 오후 1:31
 */
using System;
using System.Net;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Security.Cryptography.X509Certificates;

namespace SlackBot
{
	public class Slack
	{
		private NetworkStream _netStream;
		private TextReader _input;
		private TextWriter _output;
		
		private readonly int _port;
        private TcpClient _sock;
        private SslStream _ssl;
        
        private string _server;
        private string _nick;
        private string _channel;
        private string _password;
        
        public string Channel { get { return _channel; }}
		
        public event Action<bool> Connected;
        public event Action<string, string, string> GotMessage;
        
		private Thread _t;
		public Slack(string server, string nick, string channel, string password)
		{
			this._port = 6667;
            this._sock = new TcpClient();
            
            this._server = server;
            this._nick = nick;
            this._channel = channel;
            this._password = password;
            
            this._sock.Connect(_server, _port);
            
            if (!_sock.Connected)
            {
                throw new Exception("Can't connect server!");
            }
            
            _netStream = this._sock.GetStream();
            _ssl = new SslStream(_netStream, false, new RemoteCertificateValidationCallback(Slack.ValidateCert));
            _ssl.AuthenticateAsClient("Slack");
            
            _output = new StreamWriter(_ssl);
            _input = new StreamReader(_ssl);
            SendData("PASS " + _password);
            SendData("NICK " + _nick);
            
            _t = new Thread(new ThreadStart(this.UpdateMessage));
            _t.IsBackground = true;
            _t.Start();
		}
		
		~Slack()
		{
			_t.Abort();
		}
		
		private static bool ValidateCert(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; }
		
		private void SendData(string data)
		{
			_output.Write(data + "\r\n");
			_output.Flush();
		}
		
		public void SendMessage(string channel, string message)
		{
			this.SendData("PRIVMSG " + channel + " :" + message);
		}
		
		public void SendMessages(string channel, string[] messages)
		{
			foreach(string message in messages)
			{
				SendMessage(channel, message);
			}
		}
		
		private void UpdateMessage()
        {
            string str = _input.ReadLine();
            while (true)
            {
                if (str == null)
                {
                    return;
                }
                if (str.StartsWith("PING "))
                {
                	SendData(str.Replace("PING", "PONG"));
                    _output.Flush();
                }
                if (str[0] == ':')
                {
                    if (str.StartsWith(":") && (str.Split(new char[] { ' ' })[1] == "PRIVMSG"))
                    {
                        string nickname = str.Split(new char[] { ':' })[1].Split(new char[] { '!' })[0];
                        string channel = str.Split(new char[] { ' ' })[2];
                        string message = str.Substring((str.Split(new char[] { ':' })[0].Length + str.Split(new char[] { ':' })[1].Length) + 2);
                        
                        GotMessage(channel, nickname, message);
                    }
                    if (str.Split(new char[] { ' ' })[1] == "001")
                    {
                    	SendData("MODE " + _nick);
                    	SendData("JOIN " + _channel);
                    	
                    	if(Connected != null)
                    		Connected(true);
                    }
                }
                str = _input.ReadLine();
            }
        }
	}
}
