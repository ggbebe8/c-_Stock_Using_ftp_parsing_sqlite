using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

/*
CREATE TABLE List   (
	                   'Name' TEXT NOT NULL,
                       'Date' TEXT NOT NULL,
                       'Price' INT NOT NULL,
                       'Volumn' INT NOT NULL,
                       'HighPrice' INT NOT NULL,
                       'LowPrice' INT NOT NULL,
                       'StartPrice' INT NOT NULL
                     ) 

*/


namespace StockCalc
{
    public partial class Calc : Form
    {
        #region 전역변수
        Variables.DetailInfo[] mDetailInfo = new Variables.DetailInfo[] { };
        string mDbSource = @"Data Source = .\StockList.db";
        List<string> mliStock = new List<string>();
        #endregion 전역변수_End

        #region 생성자
        public Calc()
        {
            InitializeComponent();
            mfnInitControl();
        }
        #endregion 생성자_End

        #region 함수
        //컨트롤 초기화
        private void mfnInitControl()
        {
            string strQuery = "";
            strQuery += "SELECT Name FROM List GROUP BY Name";
            DataTable dt = Network.GetDBTable(strQuery,mDbSource);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                    txtDbList.Text = dt.Rows[i]["Name"].ToString();
                else
                    txtDbList.Text += "," + dt.Rows[i]["Name"].ToString();
            }
            mliStock.Clear();
            if (txtDbList.Text.Contains(","))
            {
                for (int i = 0; i < txtDbList.Text.Split(',').Length; i++)
                {
                    mliStock.Add(txtDbList.Text.Split(',')[i].ToUpper());
                }
            }
            else if (txtDbList.Text.Replace(" ", "") == "")
            {
                return;
            }
            else
            {
                mliStock.Add(txtDbList.Text.ToUpper());
            }
        }

        //디비에 넣자
        private void mfnInsertToDb()
        {
            mliStock.Clear();
            string strCode = "";
            string strQuery = "";

            if(txtDbList.Text.Contains(","))
            {
                for (int i = 0; i < txtDbList.Text.Split(',').Length; i++)
                {
                    mliStock.Add(txtDbList.Text.Split(',')[i].ToUpper());
                }
            }
            else if (txtDbList.Text.Replace(" ", "") == "")
            {
                return;
            }
            else
            {
                mliStock.Add(txtDbList.Text.ToUpper());
            }

            for (int i = 0; i < mliStock.Count; i++)
            {
                strCode = mfnSearchCode(mliStock[i]);
                if(strCode == "")
                    return;
                else
                {
                    mDetailInfo = Parser.GetDetailInfo(strCode, Convert.ToInt32(txtNum.Text) + 60);

                    strQuery = "DELETE  FROM List WHERE Name = '" + mliStock[i] + "'";
                    Network.ExecDB(strQuery, mDbSource);


                    using (System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection(mDbSource))
                    {
                        conn.Open();
                        using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand(conn))
                        {
                            using(System.Data.SQLite.SQLiteTransaction tran = conn.BeginTransaction())
                            {
                                for(int j = 0; j < mDetailInfo.Length; j++)
                                {
                                    strQuery = "";
                                    strQuery += "INSERT INTO List (Name, Date, Price, Volumn, HighPrice, LowPrice, StartPrice)" + "\r\n";
                                    strQuery += "VALUES( " + "\r\n"; 
                                    strQuery += "'" + mliStock[i] + "',"; 
                                    strQuery += "'" + mDetailInfo[j].date + "',";
                                    strQuery +=  "'" + mDetailInfo[j].price + "',";
                                    strQuery += "'" + mDetailInfo[j].volumn + "',";
                                    strQuery += "'" + mDetailInfo[j].highPrice + "',";
                                    strQuery += "'" + mDetailInfo[j].lowPrice + "',";
                                    strQuery += "'" + mDetailInfo[j].startPrice + "');" + "\r\n";
                                    cmd.CommandText = strQuery;
                                    cmd.ExecuteNonQuery();
                                    
                                }
                                tran.Commit();
                            }
                        }
                        conn.Close();
                    }
                    /*
                    if (Network.ExecDB(strQuery, mDbSource) == 0)
                    {
                        MessageBox.Show("InsertError");
                        return;
                    }
                    */
                }

            }
        }



        //주식의 코드를 가져오기
        private string mfnSearchCode(string p_Name)
        {
            string strQuery = @" SELECT CodeNum
                                   FROM Code
                                  WHERE Company = '"+ p_Name.ToUpper() + "'";
            DataTable dt = Network.GetDBTable(strQuery);
            if (dt.Rows.Count < 1)
            {
                MessageBox.Show("Wrong Input : " + p_Name);
                return "";
            }
            else
            {
                return dt.Rows[0]["CodeNum"].ToString();
            }
        }

        //이평선구하기
        //p_calcDate : 몇 일 평균인지?
        //p_beforeDate : 오늘로부터 몇 일 전의 평균을 구할거지?
        //ex. (60,30) 30일 전의 60일 평균을 구한다. 
        private int mfnAvgPriceCalc(int p_calcDate, int p_beforeDate)
        {
            int intAvg = 0;

            try
            {
                for (int i = p_beforeDate; i < p_calcDate + p_beforeDate; i++)
                {
                    if (mDetailInfo[i].price == -1)
                    {
                        return 0;
                    }
                    else
                    {
                        intAvg += mDetailInfo[i].price;
                    }
                }
                intAvg = intAvg / p_calcDate;
            }
            catch (IndexOutOfRangeException)
            {
                intAvg = -1;
            }
            return intAvg;
        }

        //웹에서 디테일 채워주는
        private bool mfnGetDetailFromWeb(string p_Name,int p_Howlong)
        {
            //주식 파싱. 60일 평균을 구하기 위해 구하고자 하는 날에 60일 더 파싱한다.
            string strCode = mfnSearchCode(p_Name.ToUpper());

            if (strCode == "")
                return false;

            mDetailInfo = Parser.GetDetailInfo(strCode, Convert.ToInt32(txtNum.Text) + 60);

            return true;
        }

        //디비에서 디테일 채워주는
        private void mfnGetDetailFromDB(string p_Name, int p_HowLoing)
        {
            string strQuery = "";
            strQuery += "SELECT * FROM List WHERE Name = '" + p_Name + "' ORDER BY Date LIMIT " + (p_HowLoing + 60).ToString();
            DataTable dt = Network.GetDBTable(strQuery, mDbSource);
            Variables.DetailInfo[] vdTemp = new Variables.DetailInfo[p_HowLoing + 60];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                vdTemp[i].date = dt.Rows[i]["Date"].ToString();
                vdTemp[i].price = Convert.ToInt32(dt.Rows[i]["price"]);
                vdTemp[i].volumn = Convert.ToInt32(dt.Rows[i]["volumn"]);
                vdTemp[i].highPrice = Convert.ToInt32(dt.Rows[i]["highPrice"]);
                vdTemp[i].lowPrice = Convert.ToInt32(dt.Rows[i]["lowPrice"]);
                vdTemp[i].startPrice = Convert.ToInt32(dt.Rows[i]["startPrice"]);
            }

            mDetailInfo = vdTemp;
        }
        #endregion 함수_End

        #region 계산공식

        //60일 선이 크게 흔들리는 용 (작은 기업, 작전주)
        private int mfnCalc60Avg(int p_iHowlong)
        {

            //몇 일 정보를 파싱을 해오냐
            int iHowlong = p_iHowlong + 1;
            //계산 전 자본금. 천만원 하드코딩
            int iMoney = 10000000;
            //주식을 몇 주 샀는지
            int iAmount = 0;
            //주식을 사고 남은 돈
            int iChange = 0;
            //매수가
            int iBuyPrice = -1;     // 매수할 때 초기화는 시켰지만 아직 사용하지는 않음.

            //이동평균선
            int[] iAvgs61 = new int[iHowlong];      //60일선이 상승세냐 아니면 하락세냐를 나타내기 위한 변수이지만 지금은 사용하지 않음. 지글링은 없애나 수익이 필요할 때 10%정도 떨어짐. 검증결과 장기적으로 손해
            int[] iAvgs60 = new int[iHowlong];
            int[] iAvgs45 = new int[iHowlong];
            int[] iAvgs20 = new int[iHowlong];
            int[] iAvgs5 = new int[iHowlong];
            //계산 손익에 대한 퍼센트
            double dPer = 0;



            //시작가를 기록한다. 
            txtLog.AppendText(mDetailInfo[iHowlong -1].date + "\t시작가 : " + mDetailInfo[iHowlong -1].price + "\r\n");

            //반복문을 돌리며 조건을 기준으로 계산한다. 
            for (int i = iHowlong - 1; i >= 0; i--)
            {
                iAvgs61[i] = mfnAvgPriceCalc(61, i);
                iAvgs60[i] = mfnAvgPriceCalc(60, i);
                iAvgs45[i] = mfnAvgPriceCalc(45, i);
                iAvgs20[i] = mfnAvgPriceCalc(20, i);
                iAvgs5[i] = mfnAvgPriceCalc(5, i);
                
               //++++ 여기서는 너무 많이 파싱해서 구하기 어려운데 360일 선을 긋고(혹은 그 이상) 360일 선 위에 있는 것만 매수한다면 지글링을 상당히 없앨 수 있을 듯하다. 
                // --> 했는데 망함. 
                //지글링을 없애는 가장 좋은 방법은 60일 선이 상승일 때 구입하는 것. 근데 장기적으로는 수익이 덜 남. 그래서 뺀 것임. 
                
                //60일선 터치(저점~고점)
                //60일선 보다 종가가 위에 있어야
                //양봉
                //--> 60일선 가격으로 매수
                if (iAmount == 0 && iAvgs60[i] >= iAvgs61[i] && iAvgs60[i] <= mDetailInfo[i].highPrice && iAvgs60[i] >= mDetailInfo[i].lowPrice //&& iAvgs60[i] <= mDetailInfo[i].price
                     /*&& mDetailInfo[i].startPrice < mDetailInfo[i].price*/)
                {
                    iBuyPrice = iAvgs60[i];
                    iAmount = iMoney / iAvgs60[i];
                    iChange = iMoney % iAvgs60[i];
                    iMoney = iChange + iAmount * iAvgs60[i];
                    dPer = (double)iMoney / 10000000 * 100 - 100;
                    txtLog.AppendText(mDetailInfo[i].date + " : Buy " + iAmount + ",\t\t\t\t20일선>60일선&&60일선돌파\r\n");
                }
                //20일 선이 60일 선 보다 위에 있고,
                //하지만 그 범위 차이가 3%여야 한다. 
                //그리고 60일 선을 뚫어야 함. 그럼 매수
                    /*
                else if (iAmount == 0 && iAvgs60[i] <= iAvgs20[i] && 100 - (double)iAvgs60[i] / iAvgs20[i] * 100 <= 3 && iAvgs60[i] <= mDetailInfo[i].highPrice)
                {
                    iAmount = iMoney / iAvgs20[i];
                    iChange = iMoney % iAvgs20[i];
                    iMoney = iChange + iAmount * iAvgs20[i];
                    dPer = (double)iMoney / 10000000 * 100 - 100;
                    txtLog.AppendText(mDetailInfo[i].date + " : Buy " + iAmount + ",\tCurrent " + iMoney + "\t" + dPer.ToString("0.00") + "\t??!\r\n");
                }*/
                //////////////////////////////////////////////////////////////////////////////////매도조건///////////////////////////////////////
                //60일선을 다시 터치했을 경우(고점과 저점 사이에 있는 경우) 
                //--> 60일선에서 매도
                else if (iAmount != 0 && iAvgs60[i] <= mDetailInfo[i].highPrice && iAvgs60[i] >= mDetailInfo[i].lowPrice && Convert.ToInt32(iAvgs60[i] * 0.99) > mDetailInfo[i].lowPrice)
                {
                    iMoney = iChange + Convert.ToInt32(iAmount * iAvgs60[i] * 0.9967);
                    dPer = (double)iMoney / 10000000 * 100 - 100;
                    txtLog.AppendText(mDetailInfo[i].date + " : Sell " + iAmount + ",\tCurrent " + iMoney + "\t" + dPer.ToString("0.00") + "\t60일선 터치 매도\r\n");
                    iAmount = 0;
                    iChange = 0;
                }
                //갑자기 떨어져서 고점이 45일 보다 밑에 있을 경우
                //-->시가에 매도
                else if (iAmount != 0 && iAvgs45[i] >= mDetailInfo[i].highPrice && iAvgs60[i] >= mDetailInfo[i].highPrice)
                {
                    iMoney = iChange + Convert.ToInt32(iAmount * mDetailInfo[i].startPrice* 0.9967);
                    dPer = (double)iMoney / 10000000 * 100 - 100;
                    txtLog.AppendText(mDetailInfo[i].date + " : Sell " + iAmount + ",\tCurrent " + iMoney + "\t" + dPer.ToString("0.00") + "\t45일보다 밑에\r\n");
                    iAmount = 0;
                    iChange = 0;
                }
                //종가가 60일선보다 아래이면
                //-->종가에 매도
                else if (iAmount != 0 && iAvgs60[i] > mDetailInfo[i].price)
                {
                    iMoney = iChange + Convert.ToInt32(iAmount * mDetailInfo[i].price * 0.9967);
                    dPer = (double)iMoney / 10000000 * 100 - 100;
                    txtLog.AppendText(mDetailInfo[i].date + " : Sell " + iAmount + ",\tCurrent " + iMoney + "\t" + dPer.ToString("0.00") + "\t종가 60일보다 밑\r\n");
                    iAmount = 0;
                    iChange = 0;
                }
                //20일선과 60일선의 차이가 4%이상 나고,
                //음봉이였을 경우 
                //전일 시초가보다 종가가 떨어진 경우 
                //-->종가매도
                else if (iAmount != 0 && iAvgs60[i] < iAvgs20[i] && 100 - (double)iAvgs60[i] / iAvgs20[i] * 100 >= 4 && mDetailInfo[i].startPrice > mDetailInfo[i].price
                        && mDetailInfo[i+1].startPrice > mDetailInfo[i].price)
                {
                    iMoney = iChange + Convert.ToInt32(iAmount * mDetailInfo[i].price * 0.9967);
                    dPer = (double)iMoney / 10000000 * 100 - 100;
                    txtLog.AppendText(mDetailInfo[i].date + " : Sell " + iAmount + ",\tCurrent " + iMoney + "\t" + dPer.ToString("0.00") + "\t20-60 급격차이,음봉\r\n");
                    iAmount = 0;
                    iChange = 0;
                }
                    /*
                //매수 가격에서 3%이상 떨어지면 손절 ------> 잘못된 로직으로 짰는데, 뭔가 더 안정적이 되어버렸다...? 나중에 분석해보자 
                //-->종가에 매도
                else if (iAmount != 0 && (double)iBuyPrice*0.97 <= (double)mDetailInfo[i].price )
                {
                    iMoney = iChange + Convert.ToInt32(iAmount * mDetailInfo[i].price * 0.9967);
                    dPer = (double)iMoney / 10000000 * 100 - 100;
                    txtLog.AppendText(mDetailInfo[i].date + " : Sell " + iAmount + ",\tCurrent " + iMoney + "\t" + dPer.ToString("0.00") + "\t손절 -3%\r\n");
                    iAmount = 0;
                    iChange = 0;
                    iBuyPrice = -1;
                }
                     * */

                /*
                //매수가보다 떨어지는 음봉이면 매도
                else if (iAmount != 0 && mDetailInfo[i].startPrice > mDetailInfo[i].price && iBuyPrice > mDetailInfo[i].lowPrice)
                {
                    iMoney = iChange + Convert.ToInt32(iAmount * iBuyPrice * 0.9967);
                    dPer = (double)iMoney / 10000000 * 100 - 100;
                    txtLog.AppendText(mDetailInfo[i].date + " : Sell " + iAmount + ",\tCurrent " + iMoney + "\t" + dPer.ToString("0.00") + "\t매수가 보다 떨어지는 음봉 매수가에 매도\r\n");
                    iAmount = 0;
                    iChange = 0;
                    iBuyPrice = -1;
                }
                */
                    /* 실패 
                //60일선 > 20일선 일 경우, 음봉일 때 매도 
                else if (iAmount != 0 && iAvgs60[i] > iAvgs20[i] && mDetailInfo[i].startPrice > mDetailInfo[i].price)
                {
                    iMoney = iChange + Convert.ToInt32(iAmount * mDetailInfo[i].price * 0.9967);
                    dPer = (double)iMoney / 10000000 * 100 - 100;
                    txtLog.AppendText(mDetailInfo[i].date + " : Sell " + iAmount + ",\tCurrent " + iMoney + "\t" + dPer.ToString("0.00") + "\t60일선 > 20일선 일 경우, 음봉일 때 매도 \r\n");
                    iAmount = 0;
                    iChange = 0;
                }
                     */
                 
                /*
                //종가 20일선 밑이면 매도
                else if (iAmount != 0 && iAvgs20[i] >= mDetailInfo[i].price)
                {
                    iMoney = iChange + Convert.ToInt32(iAmount * mDetailInfo[i].price * 0.9967);
                    dPer = (double)iMoney / 10000000 * 100 - 100;
                    txtLog.AppendText(mDetailInfo[i].date + " : Sell " + iAmount + ",\tCurrent " + iMoney + "\t" + dPer.ToString("0.00") + "\t45일보다 밑에\r\n");
                    iAmount = 0;
                    iChange = 0;
                }*/
                /*
                //20일선 걸치지도 않고 떨어지면 매도
                else if (iAmount != 0 && iAvgs20[i] > mDetailInfo[i].price && iAvgs20[i] > mDetailInfo[i].startPrice)
                {
                    iMoney = iChange + Convert.ToInt32(iAmount * iAvgs60[i] * 0.9967);
                    dPer = (double)iMoney / 10000000 * 100 - 100;
                    txtLog.AppendText(mDetailInfo[i].date + " : Sell " + iAmount + ",\tCurrent " + iMoney + "\t" + dPer.ToString("0.00") + "\t20에 걸치지도 않아\r\n");
                    iAmount = 0;
                    iChange = 0;
                }*/
                    /*
                //너무 급하게 올라 45일선과 20일 선의 차이가 5%이상 차이가 난다면 과열로 보고 매도
                else if (iAmount != 0 && iAvgs45[i] <= iAvgs20[i] && 100 - (double)iAvgs45[i] / iAvgs20[i] * 100 >= 5 )
                {
                    iMoney = iChange + Convert.ToInt32(iAmount * iAvgs60[i] * 0.9967);
                    dPer = (double)iMoney / 10000000 * 100 - 100;
                    txtLog.AppendText(mDetailInfo[i].date + " : Sell " + iAmount + ",\tCurrent " + iMoney + "\t" + dPer.ToString("0.00") + "\r\n");
                    iAmount = 0;
                    iChange = 0;
                }
                     * */
            }

            txtLog.AppendText(mDetailInfo[0].date + "\t현재가 : " + mDetailInfo[0].price + "\r\n");
            txtLog.AppendText("처음과 퍼센트 차이 : " + ((double)mDetailInfo[0].price / mDetailInfo[iHowlong - 1].price*100 - 100).ToString("0.00") + "\r\n");
        
            return iMoney;
        }

        private void mfnCalc45Avg(string p_StockName, int p_iHowlong)
        {
            //주식코드
            string strStockCode = mfnSearchCode(p_StockName.ToUpper());
            //몇 일 정보를 파싱을 해오냐
            int iHowlong = p_iHowlong + 1;
            //계산 전 자본금. 천만원 하드코딩
            int iMoney = 10000000;
            //주식을 몇 주 샀는지
            int iAmount = 0;
            //주식을 사고 남은 돈
            int iChange = 0;
            //매수가
            int iBuyPrice = -1;     // 매수할 때 초기화는 시켰지만 아직 사용하지는 않음.

            //이동평균선
            int[] iAvgs61 = new int[iHowlong];      //60일선이 상승세냐 아니면 하락세냐를 나타내기 위한 변수이지만 지금은 사용하지 않음. 지글링은 없애나 수익이 필요할 때 10%정도 떨어짐. 검증결과 장기적으로 손해
            int[] iAvgs60 = new int[iHowlong];
            int[] iAvgs45 = new int[iHowlong];
            int[] iAvgs20 = new int[iHowlong];
            int[] iAvgs5 = new int[iHowlong];
            //계산 손익에 대한 퍼센트
            double dPer = 0;

            if (strStockCode == "")
                return;
            else
            {
                //주식 파싱. 60일 평균을 구하기 위해 구하고자 하는 날에 60일 더 파싱한다.  -- 이거 함수 수정해야함. 
                mDetailInfo = Parser.GetDetailInfo(strStockCode, iHowlong + 60);
                //시작가를 기록한다. 
                txtLog.AppendText(mDetailInfo[iHowlong - 1].date + "\t시작가 : " + mDetailInfo[iHowlong - 1].price + "\r\n");

                //반복문을 돌리며 조건을 기준으로 계산한다. 
                for (int i = iHowlong - 1; i >= 0; i--)
                {
                    iAvgs61[i] = mfnAvgPriceCalc(61, i);
                    iAvgs60[i] = mfnAvgPriceCalc(60, i);
                    iAvgs45[i] = mfnAvgPriceCalc(45, i);
                    iAvgs20[i] = mfnAvgPriceCalc(20, i);
                    iAvgs5[i] = mfnAvgPriceCalc(5, i);

                    //45일선 터치(저점~고점)
                    //45일선 보다 종가가 위에 있어야
                    //양봉 45일선 돌파
                    //--> 종가로 매수
                    if (iAmount == 0 /*&& iAvgs60[i] >= iAvgs61[i]*/ && iAvgs45[i] <= mDetailInfo[i].highPrice && iAvgs45[i] >= mDetailInfo[i].lowPrice// && iAvgs45[i] <= mDetailInfo[i].price
                         /*&& mDetailInfo[i].startPrice < mDetailInfo[i].price*/)
                    {
                        iBuyPrice = iAvgs45[i];//iAvgs45[i];
                        iAmount = iMoney / iAvgs45[i];
                        iChange = iMoney % iAvgs45[i];
                        iMoney = iChange + iAmount * iAvgs45[i];
                        dPer = (double)iMoney / 10000000 * 100 - 100;
                        txtLog.AppendText(mDetailInfo[i].date + " : Buy " + iAmount + ",\t\t\t\t양봉 45일선 돌파-종가매수\r\n");
                    }
                    //////////////////////////////////////////////////////////////////////////////////매도조건///////////////////////////////////////
                    //45일선 * 99% 을 다시 터치했을 경우(고점과 저점 사이에 있는 경우) 
                    //--> 45일선 * 99% 에서 매도
                    else if (iAmount != 0 && iAvgs45[i] <= mDetailInfo[i].highPrice && iAvgs45[i] >= mDetailInfo[i].lowPrice && Convert.ToInt32(iAvgs45[i] * 0.99) >= mDetailInfo[i].lowPrice)
                    {
                        iMoney = iChange + Convert.ToInt32(iAmount * iAvgs45[i] * 0.99 * 0.9967);
                        dPer = (double)iMoney / 10000000 * 100 - 100;
                        txtLog.AppendText(mDetailInfo[i].date + " : Sell " + iAmount + ",\tCurrent " + iMoney + "\t" + dPer.ToString("0.00") + "\t45일선 * 99% 을 다시 터치했을 경우\r\n");
                        iAmount = 0;
                        iChange = 0;
                    }
                        /*
                    //갑자기 떨어져서 고점이 45일 보다 밑에 있을 경우
                    //-->시가에 매도
                    else if (iAmount != 0 && iAvgs20[i] >= mDetailInfo[i].highPrice && iAvgs45[i] >= mDetailInfo[i].highPrice)
                    {
                        iMoney = iChange + Convert.ToInt32(iAmount * mDetailInfo[i].startPrice * 0.9967);
                        dPer = (double)iMoney / 10000000 * 100 - 100;
                        txtLog.AppendText(mDetailInfo[i].date + " : Sell " + iAmount + ",\tCurrent " + iMoney + "\t" + dPer.ToString("0.00") + "\t45일보다 밑에\r\n");
                        iAmount = 0;
                        iChange = 0;
                    }
                         * */
                    //종가가 45일선보다 아래이면
                    //-->종가에 매도
                    else if (iAmount != 0 && iAvgs45[i] > mDetailInfo[i].price)
                    {
                        iMoney = iChange + Convert.ToInt32(iAmount * mDetailInfo[i].price * 0.9967);
                        dPer = (double)iMoney / 10000000 * 100 - 100;
                        txtLog.AppendText(mDetailInfo[i].date + " : Sell " + iAmount + ",\tCurrent " + iMoney + "\t" + dPer.ToString("0.00") + "\t종가 60일보다 밑\r\n");
                        iAmount = 0;
                        iChange = 0;
                    }
                    //20일선과 45일선의 차이가 3%이상 나고,
                    //음봉이였을 경우 
                    //전일 시초가보다 종가가 떨어진 경우 
                    //-->종가매도
                    else if (iAmount != 0 && iAvgs45[i] < iAvgs20[i] && 100 - (double)iAvgs45[i] / iAvgs20[i] * 100 >= 3 && mDetailInfo[i].startPrice > mDetailInfo[i].price
                            && mDetailInfo[i + 1].startPrice > mDetailInfo[i].price)
                    {
                        iMoney = iChange + Convert.ToInt32(iAmount * mDetailInfo[i].price * 0.9967);
                        dPer = (double)iMoney / 10000000 * 100 - 100;
                        txtLog.AppendText(mDetailInfo[i].date + " : Sell " + iAmount + ",\tCurrent " + iMoney + "\t" + dPer.ToString("0.00") + "\t20-45 급격차이,음봉\r\n");
                        iAmount = 0;
                        iChange = 0;
                    }
                }

                txtLog.AppendText(mDetailInfo[0].date + "\t현재가 : " + mDetailInfo[0].price + "\r\n");
                txtLog.AppendText("처음과 퍼센트 차이 : " + ((double)mDetailInfo[0].price / mDetailInfo[iHowlong - 1].price * 100 - 100).ToString("0.00") + "\r\n");
            }
        }

        #endregion 계산공식_End

        #region 이벤트

        //소기업용 (60일 선이 크게 흔들리는 용)
        private void btnSearch_Click(object sender, EventArgs e)
        {
            txtLog.Clear();
            if (!mfnGetDetailFromWeb(txtName.Text.ToUpper(), Convert.ToInt32(txtNum.Text)))
            {
                MessageBox.Show("Passing Fail!");
                return;
            }
            mfnCalc60Avg(Convert.ToInt32(txtNum.Text));
        }

        //45일기준
        private void btnSearch2_Click(object sender, EventArgs e)
        {
            txtLog.Clear();
            mfnCalc45Avg(txtName.Text, Convert.ToInt32(txtNum.Text));
        }

        //종합계산
        private void btnSearch3_Click(object sender, EventArgs e)
        {
            int iSum = 0;
            int[] iNum = new int[mliStock.Count];
            if(mliStock.Count != txtDbList.Text.Split(',').Length)
            {
                MessageBox.Show("Plz Refresh");
                return;
            }
            for (int i = 0; i < mliStock.Count; i++)
            {
                if (mliStock[i] != txtDbList.Text.Split(',')[i])
                {
                    MessageBox.Show("Plz Refresh");
                    return;
                }
            }

            for(int i =0; i <mliStock.Count; i++)
            {
                mfnGetDetailFromDB(mliStock[i], Convert.ToInt32(txtNum.Text));
                iNum[i] = mfnCalc60Avg(Convert.ToInt32(txtNum.Text));
                
            }
            txtLog.Clear();
            for (int i = 0; i < mliStock.Count; i++)
            {
                txtLog.AppendText(mliStock[i] + " : " + iNum[i].ToString() + "\r\n");
                iSum += iNum[i];
            }

            txtLog.AppendText("==>" + Convert.ToInt32(iSum / mliStock.Count) + "\r\n");


            /*
            a = mfnCalc60Avg("삼성전자", Convert.ToInt32(txtNum.Text));
            b = mfnCalc60Avg("cj프레시웨이", Convert.ToInt32(txtNum.Text));
            c = mfnCalc60Avg("sk텔레콤", Convert.ToInt32(txtNum.Text));
            d = mfnCalc60Avg("엔씨소프트", Convert.ToInt32(txtNum.Text));
            ee = mfnCalc60Avg("가비아", Convert.ToInt32(txtNum.Text));
            f = mfnCalc60Avg("한국전력공사", Convert.ToInt32(txtNum.Text));

            txtLog.Clear();

            txtLog.AppendText("삼성전자 : " + a.ToString() + "\r\n");
            txtLog.AppendText("CJ프레시웨이 : " + b.ToString() + "\r\n");
            txtLog.AppendText("SK텔레콤 : " + c.ToString() + "\r\n");
            txtLog.AppendText("엔씨소프트 : " + d.ToString() + "\r\n");
            txtLog.AppendText("가비아 : " + ee.ToString() + "\r\n");
            txtLog.AppendText("한국전력공사 : " + f.ToString() + "\r\n");
            txtLog.AppendText("==>" + Convert.ToString(((a + b + c + d+ ee + f)/6)) + "\r\n");
            txtLog.AppendText("==>" + Convert.ToString((((double)a + b + c + d + ee + f) / 6)/60000000 * 100) + "\r\n");
            */
        }

        //디비넣기(정보새로고침)
        private void btnDbInsert_Click(object sender, EventArgs e)
        {
            mfnInsertToDb();
        }

        //엔터누르면 검색되게 한다. 
        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtName.ToString() != "")
                {
                    btnSearch_Click(null, null);
                }
            }
                /*
            else if (e.KeyValue == 113)
            {
                if (txtName.ToString() != "")
                {
                    btnSearch2_Click(null, null);
                }
            }
                 * */
            
        }

        #endregion 이벤트_End







    }
}