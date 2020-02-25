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
        #region ��������
        Variables.DetailInfo[] mDetailInfo = new Variables.DetailInfo[] { };
        string mDbSource = @"Data Source = .\StockList.db";
        List<string> mliStock = new List<string>();
        #endregion ��������_End

        #region ������
        public Calc()
        {
            InitializeComponent();
            mfnInitControl();
        }
        #endregion ������_End

        #region �Լ�
        //��Ʈ�� �ʱ�ȭ
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

        //��� ����
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



        //�ֽ��� �ڵ带 ��������
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

        //���򼱱��ϱ�
        //p_calcDate : �� �� �������?
        //p_beforeDate : ���÷κ��� �� �� ���� ����� ���Ұ���?
        //ex. (60,30) 30�� ���� 60�� ����� ���Ѵ�. 
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

        //������ ������ ä���ִ�
        private bool mfnGetDetailFromWeb(string p_Name,int p_Howlong)
        {
            //�ֽ� �Ľ�. 60�� ����� ���ϱ� ���� ���ϰ��� �ϴ� ���� 60�� �� �Ľ��Ѵ�.
            string strCode = mfnSearchCode(p_Name.ToUpper());

            if (strCode == "")
                return false;

            mDetailInfo = Parser.GetDetailInfo(strCode, Convert.ToInt32(txtNum.Text) + 60);

            return true;
        }

        //��񿡼� ������ ä���ִ�
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
        #endregion �Լ�_End

        #region ������

        //60�� ���� ũ�� ��鸮�� �� (���� ���, ������)
        private int mfnCalc60Avg(int p_iHowlong)
        {

            //�� �� ������ �Ľ��� �ؿ���
            int iHowlong = p_iHowlong + 1;
            //��� �� �ں���. õ���� �ϵ��ڵ�
            int iMoney = 10000000;
            //�ֽ��� �� �� �����
            int iAmount = 0;
            //�ֽ��� ��� ���� ��
            int iChange = 0;
            //�ż���
            int iBuyPrice = -1;     // �ż��� �� �ʱ�ȭ�� �������� ���� ��������� ����.

            //�̵���ռ�
            int[] iAvgs61 = new int[iHowlong];      //60�ϼ��� ��¼��� �ƴϸ� �϶����ĸ� ��Ÿ���� ���� ���������� ������ ������� ����. ���۸��� ���ֳ� ������ �ʿ��� �� 10%���� ������. ������� ��������� ����
            int[] iAvgs60 = new int[iHowlong];
            int[] iAvgs45 = new int[iHowlong];
            int[] iAvgs20 = new int[iHowlong];
            int[] iAvgs5 = new int[iHowlong];
            //��� ���Ϳ� ���� �ۼ�Ʈ
            double dPer = 0;



            //���۰��� ����Ѵ�. 
            txtLog.AppendText(mDetailInfo[iHowlong -1].date + "\t���۰� : " + mDetailInfo[iHowlong -1].price + "\r\n");

            //�ݺ����� ������ ������ �������� ����Ѵ�. 
            for (int i = iHowlong - 1; i >= 0; i--)
            {
                iAvgs61[i] = mfnAvgPriceCalc(61, i);
                iAvgs60[i] = mfnAvgPriceCalc(60, i);
                iAvgs45[i] = mfnAvgPriceCalc(45, i);
                iAvgs20[i] = mfnAvgPriceCalc(20, i);
                iAvgs5[i] = mfnAvgPriceCalc(5, i);
                
               //++++ ���⼭�� �ʹ� ���� �Ľ��ؼ� ���ϱ� ���� 360�� ���� �߰�(Ȥ�� �� �̻�) 360�� �� ���� �ִ� �͸� �ż��Ѵٸ� ���۸��� ����� ���� �� ���� ���ϴ�. 
                // --> �ߴµ� ����. 
                //���۸��� ���ִ� ���� ���� ����� 60�� ���� ����� �� �����ϴ� ��. �ٵ� ��������δ� ������ �� ��. �׷��� �� ����. 
                
                //60�ϼ� ��ġ(����~����)
                //60�ϼ� ���� ������ ���� �־��
                //���
                //--> 60�ϼ� �������� �ż�
                if (iAmount == 0 && iAvgs60[i] >= iAvgs61[i] && iAvgs60[i] <= mDetailInfo[i].highPrice && iAvgs60[i] >= mDetailInfo[i].lowPrice //&& iAvgs60[i] <= mDetailInfo[i].price
                     /*&& mDetailInfo[i].startPrice < mDetailInfo[i].price*/)
                {
                    iBuyPrice = iAvgs60[i];
                    iAmount = iMoney / iAvgs60[i];
                    iChange = iMoney % iAvgs60[i];
                    iMoney = iChange + iAmount * iAvgs60[i];
                    dPer = (double)iMoney / 10000000 * 100 - 100;
                    txtLog.AppendText(mDetailInfo[i].date + " : Buy " + iAmount + ",\t\t\t\t20�ϼ�>60�ϼ�&&60�ϼ�����\r\n");
                }
                //20�� ���� 60�� �� ���� ���� �ְ�,
                //������ �� ���� ���̰� 3%���� �Ѵ�. 
                //�׸��� 60�� ���� �վ�� ��. �׷� �ż�
                    /*
                else if (iAmount == 0 && iAvgs60[i] <= iAvgs20[i] && 100 - (double)iAvgs60[i] / iAvgs20[i] * 100 <= 3 && iAvgs60[i] <= mDetailInfo[i].highPrice)
                {
                    iAmount = iMoney / iAvgs20[i];
                    iChange = iMoney % iAvgs20[i];
                    iMoney = iChange + iAmount * iAvgs20[i];
                    dPer = (double)iMoney / 10000000 * 100 - 100;
                    txtLog.AppendText(mDetailInfo[i].date + " : Buy " + iAmount + ",\tCurrent " + iMoney + "\t" + dPer.ToString("0.00") + "\t??!\r\n");
                }*/
                //////////////////////////////////////////////////////////////////////////////////�ŵ�����///////////////////////////////////////
                //60�ϼ��� �ٽ� ��ġ���� ���(������ ���� ���̿� �ִ� ���) 
                //--> 60�ϼ����� �ŵ�
                else if (iAmount != 0 && iAvgs60[i] <= mDetailInfo[i].highPrice && iAvgs60[i] >= mDetailInfo[i].lowPrice && Convert.ToInt32(iAvgs60[i] * 0.99) > mDetailInfo[i].lowPrice)
                {
                    iMoney = iChange + Convert.ToInt32(iAmount * iAvgs60[i] * 0.9967);
                    dPer = (double)iMoney / 10000000 * 100 - 100;
                    txtLog.AppendText(mDetailInfo[i].date + " : Sell " + iAmount + ",\tCurrent " + iMoney + "\t" + dPer.ToString("0.00") + "\t60�ϼ� ��ġ �ŵ�\r\n");
                    iAmount = 0;
                    iChange = 0;
                }
                //���ڱ� �������� ������ 45�� ���� �ؿ� ���� ���
                //-->�ð��� �ŵ�
                else if (iAmount != 0 && iAvgs45[i] >= mDetailInfo[i].highPrice && iAvgs60[i] >= mDetailInfo[i].highPrice)
                {
                    iMoney = iChange + Convert.ToInt32(iAmount * mDetailInfo[i].startPrice* 0.9967);
                    dPer = (double)iMoney / 10000000 * 100 - 100;
                    txtLog.AppendText(mDetailInfo[i].date + " : Sell " + iAmount + ",\tCurrent " + iMoney + "\t" + dPer.ToString("0.00") + "\t45�Ϻ��� �ؿ�\r\n");
                    iAmount = 0;
                    iChange = 0;
                }
                //������ 60�ϼ����� �Ʒ��̸�
                //-->������ �ŵ�
                else if (iAmount != 0 && iAvgs60[i] > mDetailInfo[i].price)
                {
                    iMoney = iChange + Convert.ToInt32(iAmount * mDetailInfo[i].price * 0.9967);
                    dPer = (double)iMoney / 10000000 * 100 - 100;
                    txtLog.AppendText(mDetailInfo[i].date + " : Sell " + iAmount + ",\tCurrent " + iMoney + "\t" + dPer.ToString("0.00") + "\t���� 60�Ϻ��� ��\r\n");
                    iAmount = 0;
                    iChange = 0;
                }
                //20�ϼ��� 60�ϼ��� ���̰� 4%�̻� ����,
                //�����̿��� ��� 
                //���� ���ʰ����� ������ ������ ��� 
                //-->�����ŵ�
                else if (iAmount != 0 && iAvgs60[i] < iAvgs20[i] && 100 - (double)iAvgs60[i] / iAvgs20[i] * 100 >= 4 && mDetailInfo[i].startPrice > mDetailInfo[i].price
                        && mDetailInfo[i+1].startPrice > mDetailInfo[i].price)
                {
                    iMoney = iChange + Convert.ToInt32(iAmount * mDetailInfo[i].price * 0.9967);
                    dPer = (double)iMoney / 10000000 * 100 - 100;
                    txtLog.AppendText(mDetailInfo[i].date + " : Sell " + iAmount + ",\tCurrent " + iMoney + "\t" + dPer.ToString("0.00") + "\t20-60 �ް�����,����\r\n");
                    iAmount = 0;
                    iChange = 0;
                }
                    /*
                //�ż� ���ݿ��� 3%�̻� �������� ���� ------> �߸��� �������� ®�µ�, ���� �� �������� �Ǿ���ȴ�...? ���߿� �м��غ��� 
                //-->������ �ŵ�
                else if (iAmount != 0 && (double)iBuyPrice*0.97 <= (double)mDetailInfo[i].price )
                {
                    iMoney = iChange + Convert.ToInt32(iAmount * mDetailInfo[i].price * 0.9967);
                    dPer = (double)iMoney / 10000000 * 100 - 100;
                    txtLog.AppendText(mDetailInfo[i].date + " : Sell " + iAmount + ",\tCurrent " + iMoney + "\t" + dPer.ToString("0.00") + "\t���� -3%\r\n");
                    iAmount = 0;
                    iChange = 0;
                    iBuyPrice = -1;
                }
                     * */

                /*
                //�ż������� �������� �����̸� �ŵ�
                else if (iAmount != 0 && mDetailInfo[i].startPrice > mDetailInfo[i].price && iBuyPrice > mDetailInfo[i].lowPrice)
                {
                    iMoney = iChange + Convert.ToInt32(iAmount * iBuyPrice * 0.9967);
                    dPer = (double)iMoney / 10000000 * 100 - 100;
                    txtLog.AppendText(mDetailInfo[i].date + " : Sell " + iAmount + ",\tCurrent " + iMoney + "\t" + dPer.ToString("0.00") + "\t�ż��� ���� �������� ���� �ż����� �ŵ�\r\n");
                    iAmount = 0;
                    iChange = 0;
                    iBuyPrice = -1;
                }
                */
                    /* ���� 
                //60�ϼ� > 20�ϼ� �� ���, ������ �� �ŵ� 
                else if (iAmount != 0 && iAvgs60[i] > iAvgs20[i] && mDetailInfo[i].startPrice > mDetailInfo[i].price)
                {
                    iMoney = iChange + Convert.ToInt32(iAmount * mDetailInfo[i].price * 0.9967);
                    dPer = (double)iMoney / 10000000 * 100 - 100;
                    txtLog.AppendText(mDetailInfo[i].date + " : Sell " + iAmount + ",\tCurrent " + iMoney + "\t" + dPer.ToString("0.00") + "\t60�ϼ� > 20�ϼ� �� ���, ������ �� �ŵ� \r\n");
                    iAmount = 0;
                    iChange = 0;
                }
                     */
                 
                /*
                //���� 20�ϼ� ���̸� �ŵ�
                else if (iAmount != 0 && iAvgs20[i] >= mDetailInfo[i].price)
                {
                    iMoney = iChange + Convert.ToInt32(iAmount * mDetailInfo[i].price * 0.9967);
                    dPer = (double)iMoney / 10000000 * 100 - 100;
                    txtLog.AppendText(mDetailInfo[i].date + " : Sell " + iAmount + ",\tCurrent " + iMoney + "\t" + dPer.ToString("0.00") + "\t45�Ϻ��� �ؿ�\r\n");
                    iAmount = 0;
                    iChange = 0;
                }*/
                /*
                //20�ϼ� ��ġ���� �ʰ� �������� �ŵ�
                else if (iAmount != 0 && iAvgs20[i] > mDetailInfo[i].price && iAvgs20[i] > mDetailInfo[i].startPrice)
                {
                    iMoney = iChange + Convert.ToInt32(iAmount * iAvgs60[i] * 0.9967);
                    dPer = (double)iMoney / 10000000 * 100 - 100;
                    txtLog.AppendText(mDetailInfo[i].date + " : Sell " + iAmount + ",\tCurrent " + iMoney + "\t" + dPer.ToString("0.00") + "\t20�� ��ġ���� �ʾ�\r\n");
                    iAmount = 0;
                    iChange = 0;
                }*/
                    /*
                //�ʹ� ���ϰ� �ö� 45�ϼ��� 20�� ���� ���̰� 5%�̻� ���̰� ���ٸ� ������ ���� �ŵ�
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

            txtLog.AppendText(mDetailInfo[0].date + "\t���簡 : " + mDetailInfo[0].price + "\r\n");
            txtLog.AppendText("ó���� �ۼ�Ʈ ���� : " + ((double)mDetailInfo[0].price / mDetailInfo[iHowlong - 1].price*100 - 100).ToString("0.00") + "\r\n");
        
            return iMoney;
        }

        private void mfnCalc45Avg(string p_StockName, int p_iHowlong)
        {
            //�ֽ��ڵ�
            string strStockCode = mfnSearchCode(p_StockName.ToUpper());
            //�� �� ������ �Ľ��� �ؿ���
            int iHowlong = p_iHowlong + 1;
            //��� �� �ں���. õ���� �ϵ��ڵ�
            int iMoney = 10000000;
            //�ֽ��� �� �� �����
            int iAmount = 0;
            //�ֽ��� ��� ���� ��
            int iChange = 0;
            //�ż���
            int iBuyPrice = -1;     // �ż��� �� �ʱ�ȭ�� �������� ���� ��������� ����.

            //�̵���ռ�
            int[] iAvgs61 = new int[iHowlong];      //60�ϼ��� ��¼��� �ƴϸ� �϶����ĸ� ��Ÿ���� ���� ���������� ������ ������� ����. ���۸��� ���ֳ� ������ �ʿ��� �� 10%���� ������. ������� ��������� ����
            int[] iAvgs60 = new int[iHowlong];
            int[] iAvgs45 = new int[iHowlong];
            int[] iAvgs20 = new int[iHowlong];
            int[] iAvgs5 = new int[iHowlong];
            //��� ���Ϳ� ���� �ۼ�Ʈ
            double dPer = 0;

            if (strStockCode == "")
                return;
            else
            {
                //�ֽ� �Ľ�. 60�� ����� ���ϱ� ���� ���ϰ��� �ϴ� ���� 60�� �� �Ľ��Ѵ�.  -- �̰� �Լ� �����ؾ���. 
                mDetailInfo = Parser.GetDetailInfo(strStockCode, iHowlong + 60);
                //���۰��� ����Ѵ�. 
                txtLog.AppendText(mDetailInfo[iHowlong - 1].date + "\t���۰� : " + mDetailInfo[iHowlong - 1].price + "\r\n");

                //�ݺ����� ������ ������ �������� ����Ѵ�. 
                for (int i = iHowlong - 1; i >= 0; i--)
                {
                    iAvgs61[i] = mfnAvgPriceCalc(61, i);
                    iAvgs60[i] = mfnAvgPriceCalc(60, i);
                    iAvgs45[i] = mfnAvgPriceCalc(45, i);
                    iAvgs20[i] = mfnAvgPriceCalc(20, i);
                    iAvgs5[i] = mfnAvgPriceCalc(5, i);

                    //45�ϼ� ��ġ(����~����)
                    //45�ϼ� ���� ������ ���� �־��
                    //��� 45�ϼ� ����
                    //--> ������ �ż�
                    if (iAmount == 0 /*&& iAvgs60[i] >= iAvgs61[i]*/ && iAvgs45[i] <= mDetailInfo[i].highPrice && iAvgs45[i] >= mDetailInfo[i].lowPrice// && iAvgs45[i] <= mDetailInfo[i].price
                         /*&& mDetailInfo[i].startPrice < mDetailInfo[i].price*/)
                    {
                        iBuyPrice = iAvgs45[i];//iAvgs45[i];
                        iAmount = iMoney / iAvgs45[i];
                        iChange = iMoney % iAvgs45[i];
                        iMoney = iChange + iAmount * iAvgs45[i];
                        dPer = (double)iMoney / 10000000 * 100 - 100;
                        txtLog.AppendText(mDetailInfo[i].date + " : Buy " + iAmount + ",\t\t\t\t��� 45�ϼ� ����-�����ż�\r\n");
                    }
                    //////////////////////////////////////////////////////////////////////////////////�ŵ�����///////////////////////////////////////
                    //45�ϼ� * 99% �� �ٽ� ��ġ���� ���(������ ���� ���̿� �ִ� ���) 
                    //--> 45�ϼ� * 99% ���� �ŵ�
                    else if (iAmount != 0 && iAvgs45[i] <= mDetailInfo[i].highPrice && iAvgs45[i] >= mDetailInfo[i].lowPrice && Convert.ToInt32(iAvgs45[i] * 0.99) >= mDetailInfo[i].lowPrice)
                    {
                        iMoney = iChange + Convert.ToInt32(iAmount * iAvgs45[i] * 0.99 * 0.9967);
                        dPer = (double)iMoney / 10000000 * 100 - 100;
                        txtLog.AppendText(mDetailInfo[i].date + " : Sell " + iAmount + ",\tCurrent " + iMoney + "\t" + dPer.ToString("0.00") + "\t45�ϼ� * 99% �� �ٽ� ��ġ���� ���\r\n");
                        iAmount = 0;
                        iChange = 0;
                    }
                        /*
                    //���ڱ� �������� ������ 45�� ���� �ؿ� ���� ���
                    //-->�ð��� �ŵ�
                    else if (iAmount != 0 && iAvgs20[i] >= mDetailInfo[i].highPrice && iAvgs45[i] >= mDetailInfo[i].highPrice)
                    {
                        iMoney = iChange + Convert.ToInt32(iAmount * mDetailInfo[i].startPrice * 0.9967);
                        dPer = (double)iMoney / 10000000 * 100 - 100;
                        txtLog.AppendText(mDetailInfo[i].date + " : Sell " + iAmount + ",\tCurrent " + iMoney + "\t" + dPer.ToString("0.00") + "\t45�Ϻ��� �ؿ�\r\n");
                        iAmount = 0;
                        iChange = 0;
                    }
                         * */
                    //������ 45�ϼ����� �Ʒ��̸�
                    //-->������ �ŵ�
                    else if (iAmount != 0 && iAvgs45[i] > mDetailInfo[i].price)
                    {
                        iMoney = iChange + Convert.ToInt32(iAmount * mDetailInfo[i].price * 0.9967);
                        dPer = (double)iMoney / 10000000 * 100 - 100;
                        txtLog.AppendText(mDetailInfo[i].date + " : Sell " + iAmount + ",\tCurrent " + iMoney + "\t" + dPer.ToString("0.00") + "\t���� 60�Ϻ��� ��\r\n");
                        iAmount = 0;
                        iChange = 0;
                    }
                    //20�ϼ��� 45�ϼ��� ���̰� 3%�̻� ����,
                    //�����̿��� ��� 
                    //���� ���ʰ����� ������ ������ ��� 
                    //-->�����ŵ�
                    else if (iAmount != 0 && iAvgs45[i] < iAvgs20[i] && 100 - (double)iAvgs45[i] / iAvgs20[i] * 100 >= 3 && mDetailInfo[i].startPrice > mDetailInfo[i].price
                            && mDetailInfo[i + 1].startPrice > mDetailInfo[i].price)
                    {
                        iMoney = iChange + Convert.ToInt32(iAmount * mDetailInfo[i].price * 0.9967);
                        dPer = (double)iMoney / 10000000 * 100 - 100;
                        txtLog.AppendText(mDetailInfo[i].date + " : Sell " + iAmount + ",\tCurrent " + iMoney + "\t" + dPer.ToString("0.00") + "\t20-45 �ް�����,����\r\n");
                        iAmount = 0;
                        iChange = 0;
                    }
                }

                txtLog.AppendText(mDetailInfo[0].date + "\t���簡 : " + mDetailInfo[0].price + "\r\n");
                txtLog.AppendText("ó���� �ۼ�Ʈ ���� : " + ((double)mDetailInfo[0].price / mDetailInfo[iHowlong - 1].price * 100 - 100).ToString("0.00") + "\r\n");
            }
        }

        #endregion ������_End

        #region �̺�Ʈ

        //�ұ���� (60�� ���� ũ�� ��鸮�� ��)
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

        //45�ϱ���
        private void btnSearch2_Click(object sender, EventArgs e)
        {
            txtLog.Clear();
            mfnCalc45Avg(txtName.Text, Convert.ToInt32(txtNum.Text));
        }

        //���հ��
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
            a = mfnCalc60Avg("�Ｚ����", Convert.ToInt32(txtNum.Text));
            b = mfnCalc60Avg("cj�����ÿ���", Convert.ToInt32(txtNum.Text));
            c = mfnCalc60Avg("sk�ڷ���", Convert.ToInt32(txtNum.Text));
            d = mfnCalc60Avg("��������Ʈ", Convert.ToInt32(txtNum.Text));
            ee = mfnCalc60Avg("�����", Convert.ToInt32(txtNum.Text));
            f = mfnCalc60Avg("�ѱ����°���", Convert.ToInt32(txtNum.Text));

            txtLog.Clear();

            txtLog.AppendText("�Ｚ���� : " + a.ToString() + "\r\n");
            txtLog.AppendText("CJ�����ÿ��� : " + b.ToString() + "\r\n");
            txtLog.AppendText("SK�ڷ��� : " + c.ToString() + "\r\n");
            txtLog.AppendText("��������Ʈ : " + d.ToString() + "\r\n");
            txtLog.AppendText("����� : " + ee.ToString() + "\r\n");
            txtLog.AppendText("�ѱ����°��� : " + f.ToString() + "\r\n");
            txtLog.AppendText("==>" + Convert.ToString(((a + b + c + d+ ee + f)/6)) + "\r\n");
            txtLog.AppendText("==>" + Convert.ToString((((double)a + b + c + d + ee + f) / 6)/60000000 * 100) + "\r\n");
            */
        }

        //���ֱ�(�������ΰ�ħ)
        private void btnDbInsert_Click(object sender, EventArgs e)
        {
            mfnInsertToDb();
        }

        //���ʹ����� �˻��ǰ� �Ѵ�. 
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

        #endregion �̺�Ʈ_End







    }
}