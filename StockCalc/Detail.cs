using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


/*
 * https://finance.naver.com/item/news_news.nhn?code=088350&page=&sm=title_entity_id.basic&clusterId=  -- 뉴스주소
 * 
 * 
 */

namespace StockCalc
{
    public partial class Detail : Form
    {
        #region 전역변수
        // 코드
        string mStockCode = "";

        // 계산종류
        private enum CalcType
        {
            Price,
            Volumn
        }

        // 원본파싱자료
        Variables.DetailInfo[] mDetailInfo = new Variables.DetailInfo[] { };

        private struct sAvg
        {
            public int Avg5;
            public int Avg12;
            public int Avg20;
            public int Avg60;
            public int Avg120;
            public int Current;

            public sAvg(int p_avg5, int p_avg12, int p_avg20, int p_avg60, int p_avg120, int p_cur)
            {
                Avg5 = p_avg5;
                Avg12 = p_avg12;
                Avg20 = p_avg20;
                Avg60 = p_avg60;
                Avg120 = p_avg120;
                Current = p_cur;
            }
        }


        #endregion 전역변수_End

        #region 생성자
        public Detail(string p_StockCode)
        {
            InitializeComponent();
            mStockCode = p_StockCode;
        }

        private void Detail_Load(object sender, EventArgs e)
        {
            mDetailInfo = Parser.GetDetailInfo(mStockCode, 120);
            mInitControl();
        }
        #endregion 생성자_End

        #region 초기화
        //지금은 로드이벤트에 담았지만, 나중엔 그냥 탭 클릭으로 넘겨야할 듯. 
        private void mInitControl()
        {
            string strTabPage = Network.ReadIniFile("SelectedDetailTab", "PageName", @".\setting.ini");

            TabDetail.SelectedTab = null;
            if (strTabPage == "tabPrice")
            {
                TabDetail.SelectedTab = tabPrice;
            }
            else if (strTabPage == "tabVol")
            {
                TabDetail.SelectedTab = tabVol;
            }
            else if (strTabPage == "tabSto")
            {
                TabDetail.SelectedTab = tabSto;
            }
            else
            {
                TabDetail.SelectedIndex = 0;
            }

        }

        #endregion 초기화_End

        #region 계산함수
        // AvgCalc
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
            catch(IndexOutOfRangeException)
            {
                intAvg = -1;
            }
            return intAvg;
        }

        // AvgVolCalc
        private int mfnAvgVolCalc(int p_calcDate, int p_beforeDate)
        {
            int intAvg = 0;
            try
            {
                for (int i = p_beforeDate; i < p_calcDate + p_beforeDate; i++)
                {
                    if (mDetailInfo[i].volumn == -1)
                    {
                        return 0;
                    }
                    else
                    {
                        intAvg += mDetailInfo[i].volumn;
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

        // DiffrentCalc
        private int mfnDiffCalc(int p_Compare, int p_Current, TextBox p_TxtBox)
        {
            if ( p_Current - p_Compare < 0)
            {
                p_TxtBox.ForeColor = Color.Blue;
            }
            else
            {
                p_TxtBox.ForeColor = Color.Red;
            }

            // 이렇게해야 ReadOnly의 Txt색상이 바뀜. 
            Color tmpColor = p_TxtBox.BackColor;
            p_TxtBox.BackColor = Color.White;
            p_TxtBox.BackColor = tmpColor;

            return p_Current - p_Compare;
        }


        //Stochastic Fast K
        //Stochastic : (CurrentPrice - NDayLowPrice) / (NDayHighPrice - NDayLowPrice ) * 100 
        private double mfnGetStoFastK(int n, int beforDay)
        {
            int LowP = int.MaxValue;
            int HighP = int.MinValue;

            double dFastPerK;
            try
            {
                for (int i = beforDay; i < n + beforDay; i++)
                {
                    if (LowP > mDetailInfo[i].lowPrice)
                    {
                        LowP = mDetailInfo[i].lowPrice;
                    }

                    if (HighP < mDetailInfo[i].highPrice)
                    {
                        HighP = mDetailInfo[i].highPrice;
                    }
                }

                dFastPerK = (Convert.ToDouble(mDetailInfo[beforDay].price - LowP) / Convert.ToDouble(HighP - LowP)) * 100;
            }
            catch (IndexOutOfRangeException)
            {
                return -1;
            }

            return dFastPerK;
        }

        //Stochastic Fast D, Stochastic Slow K
        private double mfnGetSmaStoFastD(int n, int m, int beforDay)
        {
            double dSum = 0;
            double dFastPerD = 0;
            double dResult = 0;
            try
            {
                for (int i = beforDay; i < m + beforDay; i++)
                {
                    dResult = mfnGetStoFastK(n, i);
                    if (dResult == -1)
                        return -1;
                    else
                        dSum += dResult;
                }

                dFastPerD = dSum / m;
            }
            catch(IndexOutOfRangeException)
            {
                return -1;
            }
            return dFastPerD;
        }

        //Stochastic Slow D
        private double mfnGetSmaStoSlowD(int n, int m, int t, int beforDay)
        {
            double dSum = 0;
            double dSlowPerD = 0;
            double dResult = 0;

            try
            {
                for (int i = beforDay; i < m + beforDay; i++)
                {
                    dResult = mfnGetSmaStoFastD(n, m, i);
                    if (dResult == -1)
                        return -1;
                    else
                        dSum += dResult;
                }
                dSlowPerD = dSum / t;
            }

            catch (IndexOutOfRangeException)
            {
                return -1;
            }
            return dSlowPerD;
        }

        //EMA Stochastic Fast D, Stochastic Slow K
        //EMA : BeforeEma + (2/m+1)*(CurrentValue - BeforeEma)
        private double mfnGetEmaStoFastD(int n, int m, int beforDay)
        {
            double dSum = 0;
            double dFastPerD = 0;
            double dResult = 0;

            try
            {
                for (int i = beforDay + m; i < m + beforDay + m; i++)
                {
                    dResult = mfnGetStoFastK(n, i);
                    if (dResult == -1)
                        return -1;
                    else
                        dSum += dResult;
                }

                dFastPerD = dSum / m;

                for (int i = m + beforDay - 1; i >= beforDay; i--)
                {
                    dFastPerD = dFastPerD + (2 / Convert.ToDouble(m + 1)) * (mfnGetStoFastK(n, i) - dFastPerD);
                }
            }

            catch (IndexOutOfRangeException)
            {
                return -1;
            }
            return dFastPerD;
        }


        //EMA Stochastic Slow D
        private double mfnGetEmaStoSlowD(int n, int m, int t, int beforDay)
        {
            double dSum = 0;
            double dSlowPerD = 0;
            double dResult = 0;

            try
            {
                for (int i = beforDay + t; i < t + beforDay + t; i++)
                {
                    dResult = mfnGetSmaStoFastD(n, m, i);
                    if (dResult == -1)
                        return -1;
                    else
                        dSum += dResult;
                }

                dSlowPerD = dSum / m;

                for (int i = t + beforDay - 1; i >= beforDay; i--)
                {
                    dSlowPerD = dSlowPerD + (2 / Convert.ToDouble(t + 1)) * (mfnGetSmaStoFastD(n, m, i) - dSlowPerD);
                }
            }
            catch (IndexOutOfRangeException)
            {
                return -1;
            }

            return dSlowPerD;
        }

        //Stochastic Color Changer
        private void mfnTxtStoColorChg(double p_TargetA, double p_TargetB, TextBox p_txtBox)
        {
            if (p_TargetA > p_TargetB)
                p_txtBox.ForeColor = Color.Red;
            else
                p_txtBox.ForeColor = Color.Blue;

            //이렇게해야 ReadOnly Txt색상이 바뀜
            Color tmpColor = p_txtBox.BackColor;
            p_txtBox.BackColor = Color.White;
            p_txtBox.BackColor = tmpColor;
        }




        #endregion 계산함수_End

        #region 유틸함수

        //Binding Combo
        private void mfnStoCboBind(ComboBox p_Cbo, string p_Selected)
        {
            p_Cbo.Items.Clear();
            p_Cbo.Items.Add("EMA");
            p_Cbo.Items.Add("SMA");

            if (p_Selected == "S")
                p_Cbo.SelectedItem = "SMA";
            else
                p_Cbo.SelectedItem = "EMA";
        }


        //TapAvgPrice
        private void mfnSelectedAvgPrice()
        {
            sAvg priceAvg = new sAvg(0, 0, 0, 0, 0, 0);
            int iBeforeDate = mfnStringToInt(txtBeforeDate.Text);

            //Get Avg
            priceAvg.Avg5 = mfnAvgPriceCalc(5,iBeforeDate);
            priceAvg.Avg12 = mfnAvgPriceCalc(12, iBeforeDate);
            priceAvg.Avg20 = mfnAvgPriceCalc(20, iBeforeDate);
            priceAvg.Avg60 = mfnAvgPriceCalc(60, iBeforeDate);
            priceAvg.Avg120 = mfnAvgPriceCalc(120, iBeforeDate);
            priceAvg.Current = mDetailInfo[iBeforeDate].price;

            //Input Avg TextBox
            txtAvgP5.Text = priceAvg.Avg5.ToString("#,##");
            txtAvgP12.Text = priceAvg.Avg12.ToString("#,##");
            txtAvgP20.Text = priceAvg.Avg20.ToString("#,##");
            txtAvgP60.Text = priceAvg.Avg60.ToString("#,##");
            txtAvgP120.Text = priceAvg.Avg120.ToString("#,##");
            //txtPrice.Text = priceAvg.Current.ToString("#,##");

            //Input Diff TextBox
            txtAvgPD5.Text = mfnDiffCalc(priceAvg.Avg5, priceAvg.Current, txtAvgPD5).ToString("#,##");
            txtAvgPD12.Text = mfnDiffCalc(priceAvg.Avg12, priceAvg.Current, txtAvgPD12).ToString("#,##");
            txtAvgPD20.Text = mfnDiffCalc(priceAvg.Avg20, priceAvg.Current, txtAvgPD20).ToString("#,##");
            txtAvgPD60.Text = mfnDiffCalc(priceAvg.Avg60, priceAvg.Current, txtAvgPD60).ToString("#,##");
            txtAvgPD120.Text = mfnDiffCalc(priceAvg.Avg120, priceAvg.Current, txtAvgPD120).ToString("#,##");

            //Input Per TextBox
            txtPDPer5.Text = ((mfnDiffCalc(priceAvg.Avg5, priceAvg.Current, txtPDPer5) / Convert.ToDouble(priceAvg.Avg5)) * 100).ToString("#,#0.##");
            txtPDPer12.Text = ((mfnDiffCalc(priceAvg.Avg12, priceAvg.Current, txtPDPer12) / Convert.ToDouble(priceAvg.Avg12)) * 100).ToString("#,#0.##");
            txtPDPer20.Text = ((mfnDiffCalc(priceAvg.Avg20, priceAvg.Current, txtPDPer20) / Convert.ToDouble(priceAvg.Avg20)) * 100).ToString("#,#0.##");
            txtPDPer60.Text = ((mfnDiffCalc(priceAvg.Avg60, priceAvg.Current, txtPDPer60) / Convert.ToDouble(priceAvg.Avg60)) * 100).ToString("#,#0.##");
            txtPDPer120.Text = ((mfnDiffCalc(priceAvg.Avg120, priceAvg.Current, txtPDPer120) / Convert.ToDouble(priceAvg.Avg5)) * 100).ToString("#,#0.##");

        }


        //SelectedAvgVolume
        private void mfnSelectedAvgVolume()
        {
            sAvg volumeAvg = new sAvg(0, 0, 0, 0, 0, 0);
            int iBeforeDate = mfnStringToInt(txtBeforeDate.Text);

            //Get Avg
            volumeAvg.Avg5 = mfnAvgVolCalc(5, iBeforeDate);
            volumeAvg.Avg12 = mfnAvgVolCalc(12, iBeforeDate);
            volumeAvg.Avg20 = mfnAvgVolCalc(20, iBeforeDate);
            volumeAvg.Avg60 = mfnAvgVolCalc(60, iBeforeDate);
            volumeAvg.Avg120 = mfnAvgVolCalc(120, iBeforeDate);
            volumeAvg.Current = mDetailInfo[iBeforeDate].volumn;

            //Input Avg TextBox
            txtAvgV5.Text = volumeAvg.Avg5.ToString("#,##");
            txtAvgV12.Text = volumeAvg.Avg12.ToString("#,##");
            txtAvgV20.Text = volumeAvg.Avg20.ToString("#,##");
            txtAvgV60.Text = volumeAvg.Avg60.ToString("#,##");
            txtAvgV120.Text = volumeAvg.Avg120.ToString("#,##");
            txtCurrentV.Text = volumeAvg.Current.ToString("#,##");

            //Input Diff TextBox
            txtAvgVD5.Text = mfnDiffCalc(volumeAvg.Avg5, volumeAvg.Current, txtAvgVD5).ToString("#,##");
            txtAvgVD12.Text = mfnDiffCalc(volumeAvg.Avg12, volumeAvg.Current, txtAvgVD12).ToString("#,##");
            txtAvgVD20.Text = mfnDiffCalc(volumeAvg.Avg20, volumeAvg.Current, txtAvgVD20).ToString("#,##");
            txtAvgVD60.Text = mfnDiffCalc(volumeAvg.Avg60, volumeAvg.Current, txtAvgVD60).ToString("#,##");
            txtAvgVD120.Text = mfnDiffCalc(volumeAvg.Avg120, volumeAvg.Current, txtAvgVD120).ToString("#,##");
        }


        //Selected Stochastic
        private void mfnSelectedStochastic()
        {


            string strStoF = Network.ReadIniFile("Chart:Stochastic", "Fast", @".\setting.ini");
            string strStoS = Network.ReadIniFile("Chart:Stochastic", "Slow", @".\setting.ini");
            int iBeforeDate = mfnStringToInt(txtBeforeDate.Text);

            if (strStoF == "")
                strStoF = "10,5,E";
            if (strStoS == "")
                strStoS = "10,5,5,E";

            txtStoFInput1.Text = (strStoF.Split(','))[0];
            txtStoFInput2.Text = (strStoF.Split(','))[1];
            txtStoSInput1.Text = (strStoS.Split(','))[0];
            txtStoSInput2.Text = (strStoS.Split(','))[1];
            txtStoSInput3.Text = (strStoS.Split(','))[2];

            string strAvgTypeF = strStoF.Split(',')[2];
            string strAvgTypeS = strStoS.Split(',')[3];

            mfnStoCboBind(cboStoF, strAvgTypeF);
            mfnStoCboBind(cboStoS, strAvgTypeS);

            int iFast1 = Convert.ToInt16(txtStoFInput1.Text.ToString());
            int iFast2 = Convert.ToInt16(txtStoFInput2.Text.ToString());
            int iSlow1 = Convert.ToInt16(txtStoSInput1.Text.ToString());
            int iSlow2 = Convert.ToInt16(txtStoSInput2.Text.ToString());
            int iSlow3 = Convert.ToInt16(txtStoSInput3.Text.ToString());

            double dStoFShowK = mfnGetStoFastK(iFast1, iBeforeDate);
            double dStoFShowD = strAvgTypeF == "S" ? mfnGetSmaStoFastD(iFast1, iFast2, iBeforeDate) : mfnGetEmaStoFastD(iFast1, iFast2, iBeforeDate);
            double dStoFShowKBefore = mfnGetStoFastK(iFast1, iBeforeDate + 1);
            double dStoFShowDBefore = strAvgTypeF == "S" ? mfnGetSmaStoFastD(iFast1, iFast2, iBeforeDate + 1) : mfnGetEmaStoFastD(iFast1, iFast2, iBeforeDate + 1);

            double dStoSShowK = strAvgTypeS == "S" ? mfnGetSmaStoFastD(iSlow1, iSlow2, iBeforeDate) : mfnGetEmaStoFastD(iSlow1, iSlow2, iBeforeDate);
            double dStoSShowD = strAvgTypeS == "S" ? mfnGetSmaStoSlowD(iSlow1, iSlow2, iSlow3, iBeforeDate) : mfnGetEmaStoSlowD(iSlow1, iSlow2, iSlow3, iBeforeDate);
            double dStoSShowKBefore = strAvgTypeS == "S" ? mfnGetSmaStoFastD(iSlow1, iSlow2, iBeforeDate + 1) : mfnGetEmaStoFastD(iSlow1, iSlow2, iBeforeDate + 1);
            double dStoSShowDBefore = strAvgTypeS == "S" ? mfnGetSmaStoSlowD(iSlow1, iSlow2, iSlow3, iBeforeDate + 1) : mfnGetEmaStoSlowD(iSlow1, iSlow2, iSlow3, iBeforeDate + 1);


            txtStoFShowK.Text = dStoFShowK.ToString("0.##");
            txtStoFShowD.Text = dStoFShowD.ToString("0.##");
            txtStoFShowKBefore.Text = dStoFShowKBefore.ToString("0.##");
            txtStoFShowDBefore.Text = dStoFShowDBefore.ToString("0.##");

            txtStoSShowK.Text = dStoSShowK.ToString("0.##");
            txtStoSShowD.Text = dStoSShowD.ToString("0.##");
            txtStoSShowKBefore.Text = dStoSShowKBefore.ToString("0.##");
            txtStoSShowDBefore.Text = dStoSShowDBefore.ToString("0.##");

            txtDiffFast.Text = (dStoFShowK - dStoFShowD).ToString("0.##");
            txtDiffFastBefore.Text = (dStoFShowKBefore - dStoFShowDBefore).ToString("0.##");
            txtDiffSlow.Text = (dStoSShowK - dStoSShowD).ToString("0.##");
            txtDiffSlowBefore.Text = (dStoSShowKBefore - dStoSShowDBefore).ToString("0.##");

            mfnTxtStoColorChg(dStoFShowK, dStoFShowD, txtStoFShowK);
            mfnTxtStoColorChg(dStoFShowKBefore, dStoFShowDBefore, txtStoFShowKBefore);

            mfnTxtStoColorChg(dStoSShowK, dStoSShowD, txtStoSShowK);
            mfnTxtStoColorChg(dStoSShowKBefore, dStoSShowDBefore, txtStoSShowKBefore);


            mfnTxtStoColorChg(dStoFShowK, dStoFShowD, txtDiffFast);
            mfnTxtStoColorChg(dStoFShowKBefore, dStoFShowDBefore, txtDiffFastBefore);

            mfnTxtStoColorChg(dStoSShowK, dStoSShowD, txtDiffSlow);
            mfnTxtStoColorChg(dStoSShowKBefore, dStoSShowDBefore, txtDiffSlowBefore);

        }

        private int mfnStringToInt(string p_str)
        {
            int iReturn = 0;

            try
            {
                iReturn = Convert.ToInt32(p_str);
            }
            catch (FormatException)
            {
                iReturn = 0;
            }
            return iReturn;

        }
        #endregion 유틸함수_End

        #region 연결함수

        //btnSearch1
        private void efnSearchStoF()
        {
            int iValid;
        
            if (!Int32.TryParse(txtStoFInput1.Text, out iValid) || !Int32.TryParse(txtStoFInput2.Text, out iValid))
            {
                MessageBox.Show("Wrong Value", "Search", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                Network.WriteIniFile("Chart:Stochastic", "Fast", txtStoFInput1.Text + "," + txtStoFInput2.Text + "," + (cboStoF.SelectedItem == "SMA" ? "S" : "E"), @".\setting.ini");
            }
            catch
            {
                MessageBox.Show("Fail Search", "Search", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            mfnSelectedStochastic();
        }

        //btnSearch2
        private void efnSearchStoS()
        {
            int iValid;

            if (!Int32.TryParse(txtStoSInput1.Text, out iValid) || !Int32.TryParse(txtStoSInput2.Text, out iValid) || !Int32.TryParse(txtStoSInput3.Text, out iValid))
            {
                MessageBox.Show("Wrong Value", "Search", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                Network.WriteIniFile("Chart:Stochastic", "Slow", txtStoSInput1.Text + "," + txtStoSInput2.Text + "," + txtStoSInput3.Text + "," + (cboStoS.SelectedItem == "SMA" ? "S" : "E"), @".\setting.ini");
            }
            catch
            {
                MessageBox.Show("Fail Search", "Search", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            mfnSelectedStochastic();
        }

        //btnBeforeDate
        private void efnBeforeDate()
        {
            int iBeforeDateCounter = mfnStringToInt(txtBeforeDate.Text);

            iBeforeDateCounter++;

            txtBeforeDate.Text = iBeforeDateCounter.ToString();

            efnTabSelectedIndexChanged();
        }

        //btnAfterDate
        private void efnAfterDate()
        {
            int iBeforeDateCounter = mfnStringToInt(txtBeforeDate.Text);

            if(iBeforeDateCounter > 0)
                iBeforeDateCounter--;

            txtBeforeDate.Text = iBeforeDateCounter.ToString();

            efnTabSelectedIndexChanged();
        }

        //TabDetail_SelectedIndexChanged
        private void efnTabSelectedIndexChanged()
        {
            int iBeforeDate = mfnStringToInt(txtBeforeDate.Text);

            txtCurrentP.Text = mDetailInfo[iBeforeDate].price.ToString("#,##");


            if (TabDetail.SelectedTab == tabPrice)
            {
                mfnSelectedAvgPrice();
            }
            else if (TabDetail.SelectedTab == tabVol)
            {
                mfnSelectedAvgVolume();
            }
            else if (TabDetail.SelectedTab == tabSto)
            {
                mfnSelectedStochastic();
            }
        }

        #endregion 연결함수_End

        #region 이벤트

        private void TabDetail_SelectedIndexChanged(object sender, EventArgs e)
        {
            efnTabSelectedIndexChanged();
        }
        
        //Button '조회'
        private void btnSearch1_Click(object sender, EventArgs e)
        {
            efnSearchStoF();
        }

        //Button '조회'
        private void btnSearch2_Click(object sender, EventArgs e)
        {
            efnSearchStoS();
        }

        //Button '<'
        private void btnBeforeDate_Click(object sender, EventArgs e)
        {
            efnBeforeDate();
        }

        //Button '>'
        private void btnAfterDate_Click(object sender, EventArgs e)
        {
            efnAfterDate();
        }

        //OnlyNumber Input
        private void txtOnlyNumber_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
            }

        }

        //FormClosed - Write TabPageName to Setting.ini
        private void Detail_FormClosed(object sender, FormClosedEventArgs e)
        {
            Network.WriteIniFile("SelectedDetailTab", "PageName", TabDetail.SelectedTab.Name, @".\setting.ini");
        }

        private void txtBeforeDate_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                if (txtBeforeDate.Text == "")
                {
                    txtBeforeDate.Text = "0";
                }
                efnTabSelectedIndexChanged();
            }
        }

        private void txtBeforeDate_Leave(object sender, EventArgs e)
        {
            if (txtBeforeDate.Text == "")
            {
                txtBeforeDate.Text = "0";
            }

            efnTabSelectedIndexChanged();
        }
        #endregion 이벤트_End






    }
}