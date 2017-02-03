using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading;
using System.Security.Cryptography.X509Certificates;

namespace SlackAPI
{
    public delegate void MessageCallback(string channel, string nickname, string message);
    public class Slack
    {
        private ManualResetEvent _updatePauseEvent = new ManualResetEvent(false);
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

        public string Server => _server;
        public string Nickname => _nick;

        public event Action<bool> Connected;
        public event MessageCallback GotMessage;
        public event Action<string> GotData;

        private Thread _updateThread;
        public Slack(string server, string nick, string password, string channel = "#general")
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
                throw new Exception("Can't connect to socket");
            }

            _netStream = this._sock.GetStream();
            _ssl = new SslStream(_netStream, false, new RemoteCertificateValidationCallback(Slack.ValidateCert));
            _ssl.AuthenticateAsClient("Slack");

            _output = new StreamWriter(_ssl);
            _input = new StreamReader(_ssl);
            SendData("PASS " + _password);
            SendData("NICK " + _nick);

            _updateThread = new Thread(new ThreadStart(UpdateMessage))
            {
                IsBackground = true
            };
        }
        ~Slack()
        {
            _updateThread.Abort();
        }

        private static bool ValidateCert(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; }

        public void Connect()
        {
            _updateThread.Start();
            ResumeUpdate();
        }
        public void Disconnect()
        {
            _updateThread.Abort();
        }
        
        private void UpdateMessage()
        {
            string str = _input.ReadLine();
            while (_updatePauseEvent.WaitOne())
            {
                GotData?.Invoke(str);
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

                        GotMessage?.Invoke(channel, nickname, message);
                    }
                    if (str.Split(new char[] { ' ' })[1] == "001")
                    {
                        SendData("MODE " + _nick);
                        SendData("JOIN " + _channel);
                        Connected?.Invoke(true);
                    }
                }
                str = _input.ReadLine();
                Thread.Sleep(10);
            }
        }

        private void PauseUpdate()
        {
            _updatePauseEvent.Reset();
        }
        private void ResumeUpdate()
        {
            _updatePauseEvent.Set();
        }

        public void SendData(string data)
        {
            _output.Write(data + "\r\n");
            _output.Flush();
        }
        public void SendMessage(string channel, string message)
        {
            if (message.Length >= 480)
            {
                throw new IndexOutOfRangeException("문자열의 길이는 480자 미만이어야 합니다.");
            }
            this.SendData("PRIVMSG " + channel + " :" + message);
        }
        public void SendMessages(string channel, string[] messages)
        {
            foreach (string message in messages)
            {
                SendMessage(channel, message);
            }
        }

        public void JoinChannel(string channel)
        {
            //TODO : 리턴을 bool 형식으로 해서 성공여부를 알려야됨.
            //그러려면 스레드에서 인풋스트림에서 긁어오는걸 어떻게든 막든지 따로 가져오든지 해야함.

            SendData($"JOIN {channel}");
        }

        public void InviteChannel(string nickname, string channel)
        {
            SendData($"INVITE {nickname} {channel}");
        }

        public string[] GetChannels()
        {
            throw new NotImplementedException();
        }
    }
}
