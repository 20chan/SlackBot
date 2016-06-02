/*
 * 사용자: philly
 * 날짜: 2016-06-01
 * 시간: 오후 8:36
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SlackBot
{
	/// <summary>
	/// 저장할 수 있는 봇의 데이터.
	/// </summary>
	[Serializable]
	public class Data
	{
		private List<string> _adminList;
		private List<string> _banList;
		
		private Dictionary<string, string> _table;
		
		public List<string> AdminList { get { return _adminList; } set { _adminList = value; }}
		public List<string> BanList { get { return _banList; } set { _banList = value; }}
		
		public Data()
		{
			_adminList = new List<string>();
			_banList = new List<string>();
			_table = new Dictionary<string, string>();
		}
		
		public void Save(string path)
		{
			using (FileStream fs = new FileStream(path, FileMode.Create))
			{
				BinaryFormatter bf = new BinaryFormatter();
				bf.Serialize(fs, this);
			}
		}
		
		public static Data FromFile(string path)
		{
			using (FileStream fs = new FileStream(path, FileMode.Open))
			{
				BinaryFormatter bf = new BinaryFormatter();
				return (Data) bf.Deserialize(fs);
			}
		}
		
		public void SetTable(string key, string value)
		{
			if(_table.ContainsKey(key))
			{
				_table[key] = value;
			}
			else
			{
				_table.Add(key, value);
			}
		}
		
		public string GetTable(string key)
		{
			if(_table.ContainsKey(key))
			{
				return _table[key];
			}
			else
			{
				return "키가 존재하지 않습니다.";
			}
		}
	}
}