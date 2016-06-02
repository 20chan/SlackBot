/*
 * 사용자: philly
 * 날짜: 2016-06-01
 * 시간: 오후 10:17
 */
using System;
using System.Xml;
using System.Collections.Generic;
using System.Drawing;

namespace SlackBot
{
	/// <summary>
	/// 기상청 날씨 파싱 라이브러리
	/// </summary>
	public static class Weather
	{
		private static Dictionary<string, Point> regions = new Dictionary<string, Point>()
		{
			{"서울", new Point(59, 127)},
			{"세종", new Point(65, 103)},
			{"대전", new Point(67, 101)},
			{"부산", new Point(93, 73)},
			{"대구", new Point(88, 90)},
			{"인천", new Point(51, 131)},
			{"전남", new Point(51, 131)},
			{"제주", new Point(56, 33)},
			{"충남", new Point(65, 99)},
			{"충북", new Point(76, 111)},
			{"전북", new Point(55, 80)},
			{"강원", new Point(93, 131)},
			{"경기", new Point(69, 132)},
			{"경남", new Point(89, 68)},
			{"경북", new Point(91, 89)},
			{"광주", new Point(57, 74)},
			{"울산", new Point(102, 84)}
		};
		
		public static string[] GetWeather(string region)
		{
			if(regions.ContainsKey(region))
			{
				Point p = regions[region];
				return GetWeather(p.X, p.Y);
			}
			else
			{
				return new string[] { "지역이 테이블에 등록되지 않았습니다." };
			}
		}
		
		public static string[] GetWeather(int x, int y)
		{
			XmlDocument document = new XmlDocument();
            try
            {
            	document.Load("http://www.kma.go.kr/wid/queryDFS.jsp?gridx=" + x.ToString() + "&gridy=" + y.ToString());
            }
            catch
            {
                return null;
            }
            XmlNodeList elementsByTagName = document.GetElementsByTagName("hour");
            XmlNodeList list2 = document.GetElementsByTagName("temp");
            XmlNodeList list3 = document.GetElementsByTagName("wfKor");
            return new string[] {(elementsByTagName[0].InnerText + "시 : " + list3[0].InnerText + " (" + list2[0].InnerText + "℃)"), (elementsByTagName[1].InnerText + "시 : " + list3[1].InnerText + " (" + list2[1].InnerText + "℃)"), (elementsByTagName[2].InnerText + "시 : " + list3[2].InnerText + " (" + list2[2].InnerText + "℃)"), (elementsByTagName[3].InnerText + "시 : " + list3[3].InnerText + " (" + list2[3].InnerText + "℃)") };
		}
	}
}
