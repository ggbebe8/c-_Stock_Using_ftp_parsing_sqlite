using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace StockCalc
{
    class Parser
    {
        /// MainParser ///
        public static Dictionary<string,string> GetInfo(string p_StockCode)        //parsing
        {
            Dictionary<string,string> dicParsing = new Dictionary<string,string>();
            String strURL = "http://finance.naver.com/item/sise_day.nhn?code=" + p_StockCode;
            String tempStr = GetHtmlString(strURL);

            if (tempStr == "False")
                return dicParsing;

            String[] SplitStr = tempStr.Split(':');

            double per = ((Convert.ToDouble(SplitStr[17].Replace(",", "")) / Convert.ToDouble(SplitStr[23].Replace(",", ""))) * 100);
            string strMinus = "";
            if (Convert.ToInt32(SplitStr[16].Replace(",", "")) - Convert.ToInt32(SplitStr[23].Replace(",", "")) < 0)
            {
                per = per * -1;
                strMinus = "-";
            }

            dicParsing.Add("Time", DateTime.Now.ToString("HH:mm:ss"));
            dicParsing.Add("NowV", SplitStr[16]);
            dicParsing.Add("Interval", strMinus + SplitStr[17]);
            dicParsing.Add("YesterDayV", SplitStr[23]);
            dicParsing.Add("Per",per.ToString("0.##"));
            dicParsing.Add("StartV", SplitStr[18]);
            dicParsing.Add("HighV", SplitStr[19] );
            dicParsing.Add("LowV", SplitStr[20] );
            dicParsing.Add("QV", SplitStr[21]);
            //strValue = DateTime.Now.ToString("HH:mm:ss") + "--" + "(" + SplitStr[16] + ")" + ":" + "(" + strMinus + SplitStr[17] + ")" + ":" + "(" + SplitStr[23] + ")" + ":" + "(" + per.ToString("0.##") + "%" + ")" + ":" + "(" + SplitStr[18] + ")" + ":" + "(" + SplitStr[19] + ")" + ":" + "(" + SplitStr[20] + ")" + ":" + "(" + SplitStr[21] + ")";

            return dicParsing;
        }
        
        // GetDetailInfo
        public static Variables.DetailInfo[] GetDetailInfo(string p_StockCode, int p_Date)
        {
            Variables.DetailInfo[] detailInfo = new Variables.DetailInfo[p_Date];

            /* Parsing EX
            xxxx:functionmouseOver(obj){:obj.style.backgroundColor="#f6f4e5";:}:
            functionmouseOut(obj){:obj.style.backgroundColor="#ffffff";:}:일별시세:
            날짜:종가:전일비:시가:고가:저가:거래량:
            2019.01.23:30,650:650:30,650:30,650:29,800:23,812:
            2019.01.22:30,000:650:30,200:30,500:29,450:47,576:
            2019.01.21:30,650:200:30,500:30,750:30,250:18,348:
            2019.01.18:30,450:100:30,850:30,900:29,850:23,615:
            2019.01.17:30,350:350:30,550:30,850:29,500:35,802:
            2019.01.16:30,700:850:29,500:30,800:29,400:52,867:
            2019.01.15:29,850:100:29,800:30,150:29,550:27,256:
            2019.01.14:29,750:1,200:28,850:30,450:28,850:71,833:
            2019.01.11:28,550:450:29,100:29,100:28,200:29,718:
            2019.01.10:29,000:0:29,000:30,000:28,700:62,221:
            페이지네비게이션:1:2:3:4:5:6:7:8:9:10:다음:맨뒤:
             
            */
            string strURL = ""; 
            string tempStr = "";
            string[] SplitStr = new string[] { };



            int x = 0;
            for (int i = 0; i < (p_Date/10); i++)
            {
                strURL = "http://finance.naver.com/item/sise_day.nhn?code=" + p_StockCode + "&page=" + Convert.ToString(i+1);
                tempStr = GetHtmlString(strURL);
                SplitStr = tempStr.Split(':');

                if (tempStr == "False")
                {
                    detailInfo[0].date = "0000.00.00";
                    detailInfo[0].price = -1;
                    detailInfo[0].volumn = -1;
                    detailInfo[0].highPrice = -1;
                    detailInfo[0].lowPrice = -1;
                    detailInfo[0].startPrice = -1;

                    return detailInfo;
                }
                if (x < p_Date)
                {
                    for (int j = 15; j < 85; j += 7)
                    {
                        // 혹여나 60일치가 모두 파싱이 안된 경우
                        if (!SplitStr[j].Contains("."))
                        {
                            detailInfo[x].date = "0000.00.00";
                            detailInfo[x].price = -1;
                            detailInfo[x].volumn = -1;
                            detailInfo[x].highPrice = -1;
                            detailInfo[x].lowPrice = -1;
                            detailInfo[x].startPrice = -1;
                            return detailInfo;
                        }
                        try
                        {
                            detailInfo[x].date = SplitStr[j];
                            detailInfo[x].price = Convert.ToInt32(SplitStr[j + 1].Replace(",", ""));
                            detailInfo[x].volumn = Convert.ToInt32(SplitStr[j + 6].Replace(",", ""));
                            detailInfo[x].highPrice = Convert.ToInt32(SplitStr[j + 4].Replace(",", ""));
                            detailInfo[x].lowPrice = Convert.ToInt32(SplitStr[j + 5].Replace(",", ""));
                            detailInfo[x].startPrice = Convert.ToInt32(SplitStr[j + 3].Replace(",", ""));
                        }
                        catch
                        {
                            detailInfo[x].date = "0000.00.00";
                            detailInfo[x].price = -1;
                            detailInfo[x].volumn = -1;
                            detailInfo[x].highPrice = -1;
                            detailInfo[x].lowPrice = -1;
                            detailInfo[x].startPrice = -1;
                            return detailInfo;
                        }
                        x++;
                    }
                }
            }
            return detailInfo;
        }

        // Parsing 
        // p_urlInfo : "url|*|StartValue|&|LastValue..."
        // return : value1|,|value2|,|....
        public static List<string> GetHtmlInfo(string p_urlInfo)
        {
            p_urlInfo = p_urlInfo.Replace("|*|", "|&|");

            //After p_urlInfo[1]
            List<string> liWantToFind = new List<string>();

            string[] strSplitInfo = new string[] { };
            string[] strSplitInfoLi = new string[] { };

            strSplitInfo = p_urlInfo.Split(new string[] { "|&|" }, StringSplitOptions.None);

            //Effectiveness Test about Parameters 
            if (strSplitInfo.Length < 3)
            {
                liWantToFind.Clear();
                liWantToFind.Add("Error");
                liWantToFind.Add("Wrong Parameters : Length < 3");
                return liWantToFind;
            }
            else if (strSplitInfo.Length % 2 != 1)
            {
                liWantToFind.Clear();
                liWantToFind.Add("Error");
                liWantToFind.Add("Wrong Parameters : Length % 2 != 1");
                return liWantToFind;
            }

            //All Parsing
            String strAllParsing = GetHtmlString(strSplitInfo[1]);

            //Loop Var
            bool isWhileStop = false;

            if (strAllParsing == "False")
            {
                liWantToFind.Clear();
                liWantToFind.Add("Error");
                liWantToFind.Add("Wrong Url");
                return liWantToFind;
            }
            int intFindFirstIndex = 0;
            int intFindLastIndex = 0;
            string strTemp = "";
            string strStart = "";
            string strLast = "";

            while (!isWhileStop)
            {
                for (int i = 1; i < strSplitInfo.Length; i += 2)
                {
                    strStart = strSplitInfo[i];
                    strLast = strSplitInfo[i + 1];

                    //Find Index 
                    intFindFirstIndex = strAllParsing.IndexOf(strStart, intFindFirstIndex) + strStart.Length;
                    intFindLastIndex = strAllParsing.IndexOf(strLast, intFindFirstIndex);

                    //if Can't find Index, get out of the loop
                    if (intFindFirstIndex - strStart.Length < 0 || intFindLastIndex < 0)
                    {
                        isWhileStop = true;
                        break;
                    }
                    for (int x = intFindFirstIndex; x < intFindLastIndex; x++)
                    {
                        strTemp += strAllParsing[x];
                    }
                    strTemp += "|,|";

                    intFindFirstIndex = intFindLastIndex + strStart.Length;

                }

                liWantToFind.Add(strTemp.ToUpper());
                strTemp = "";
            }

            return liWantToFind;


        }

        // GetHtml //
        private static String GetHtmlString(String url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
                String strHtml = reader.ReadToEnd();
                //strHtml = Regex.Replace(strHtml, @"<(.|\n)*?>", String.Empty);
                strHtml = Regex.Replace(strHtml, @"<(.|\n)*?>", String.Empty);
                strHtml = strHtml.Replace(" ", "").Replace("\t", "").Replace("//-->", "");
                String[] str = strHtml.Split(new Char[] { '\n' });
                strHtml = null;
                foreach (String s in str)
                {
                    if (s.Trim() != "")
                        strHtml += s + ":";
                }
                reader.Close();
                response.Close();
                return strHtml;
            }
            catch (WebException)
            {
                return "False";
            }
        }
    }
}
