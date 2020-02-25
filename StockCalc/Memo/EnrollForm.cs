using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;
using System.Security.AccessControl;

namespace StockCalc
{
    public partial class EnrollForm : Form
    {
        #region 전역변수

        public string rMainCombText = "";

        public string rSmallCombText = "";

        int mSeq;   //저장할 때 Seq번호

        string mDate;   //예전 파일 날짜 -- 날짜 변경 시 파일을 옮기기 위함

        bool mIsNew = false; //신규 수정 여부 

        string mReportSeq = ""; //Buy와 Sell 테이블과 연결되어 있는 메모

        string mReportType = "";

        DataTable dtCombo;


        #endregion 전역변수_End

        #region 생성자
        /// <summary>
        /// 신규
        /// </summary>
        /// <param name="p_Main"></param>
        /// <param name="p_Small"></param>
        public EnrollForm()
        {
            InitializeComponent();

            dte.Value = DateTime.Now;

            mIsNew = true;

            fnComboInit();
            
            cboColor.SelectedItem = "흰색";
        }


        /// <summary>
        /// 수정
        /// </summary>
        /// <param name="p_Main"></param>
        /// <param name="p_Small"></param>
        /// <param name="p_dicSelected"></param>
        public EnrollForm(Dictionary<string,string> p_dicSelected)
        {
            InitializeComponent();

            //seq을 전역변수에 저장
            mSeq = Convert.ToInt32(p_dicSelected["Seq"]);

            //예전 날짜 저장
            mDate = p_dicSelected["날짜"];

            //신규 수정 여부 
            mIsNew = false;

            //콤보바인딩
            fnComboInit();

            //그 밖에 컨트롤 바인딩
            fnSelectedInit(p_dicSelected);

            if (p_dicSelected["reportSeq"] != "")
            {
                cboMain.Enabled = false;
                cboSmall.Enabled = false;
                dte.Enabled = false;
            }
        }
        
        /// <summary>
        /// Report에서 팝업으로 넘어오기
        /// </summary>
        /// <param name="p_repSeq">Buy Sell</param>
        /// <param name="p_repSeq">[0]테이블 [1]종목의 Seq</param>
        /// <param name="p_memSeq">메모의 Seq(기존 데이터가 있을 경우(수정)만)</param>
        public EnrollForm(string p_reportType, string p_reportSeq, string p_memSeq)
        {
            InitializeComponent();

            mReportSeq = p_reportSeq;

            mReportType = p_reportType;

            //콤보바인딩
            fnComboInit();

            string strSQL = "SELECT * FROM Memo WHERE Valid = 'Y' AND Seq = '" + p_memSeq + "';\r\n";

            if(p_reportType == "Buy")
                strSQL += "SELECT Name, Date FROM Buy WHERE Seq = '" + p_reportSeq + "'";
            else if(p_reportType == "Sell")
                strSQL += "SELECT Name, Date FROM Sell WHERE Seq = '" + p_reportSeq + "'";

            DataSet ds = Network.GetDBSet(strSQL);

            //수정으로 들어왔을 경우
            if (ds.Tables[0].Rows.Count > 0)
            {
                mSeq = Convert.ToInt32(ds.Tables[0].Rows[0]["Seq"].ToString());
                //mDate = ds.Tables[0].Rows[0]["Date"].ToString();
                dte.Value = DateTime.ParseExact(ds.Tables[0].Rows[0]["Date"].ToString(), "yyyyMMdd", null);
                //신규 수정 여부 
                mIsNew = false;
                //그 밖에 컨트롤 바인딩
                fnSelectedInit(ds.Tables[0]);

            }

            else
            {
                txtTitle.Text = p_reportType == "Buy" ? "Buy" : p_reportType == "Sell" ? "Sell" : "";
                dte.Value = DateTime.ParseExact(ds.Tables[1].Rows[0]["Date"].ToString(), "yyyyMMdd", null);
                //신규 수정 여부
                mIsNew = true;
            }
            
            cboMain.Text = "종목";
            cboSmall.Text = ds.Tables[1].Rows[0]["Name"].ToString();
            cboMain.Enabled = false;
            cboSmall.Enabled = false;
            dte.Enabled = false;
        }
        
        #endregion 생성자_End




        #region 함수

        /// <summary>
        /// 수정 시, 컨트롤 초기화
        /// </summary>
        /// <param name="p_dicSelected"></param>
        private void fnSelectedInit(Dictionary<string, string> p_dicSelected)
        {
            dte.Value = Convert.ToDateTime(p_dicSelected["날짜"].Substring(0, 4) + "." + p_dicSelected["날짜"].Substring(4, 2) + "." + p_dicSelected["날짜"].Substring(6, 2));
            cboMain.SelectedIndex = cboMain.FindStringExact(p_dicSelected["대분류"]);
            cboSmall.SelectedIndex = cboSmall.FindStringExact(p_dicSelected["소분류"]);
            cboColor.SelectedIndex = cboColor.FindStringExact(p_dicSelected["Color"] == "" ? "흰색" : p_dicSelected["Color"]);
            txtTitle.Text = p_dicSelected["제목"].Replace("♤", "'");
            //rtxtContents.Text = p_dicSelected["내용"].Replace("♤", "'");
            if (p_dicSelected["Contents_Rtf"] != "")
            {
                rtxtContents.Rtf = p_dicSelected["Contents_Rtf"].Replace("♤", "'");
            }
            else
            {
                rtxtContents.Text = p_dicSelected["내용"].Replace("♤", "'");
            }
        }

        /// <summary>
        /// 수정 시, 컨트롤 초기화
        /// </summary>
        /// <param name="p_dicSelected"></param>
        private void fnSelectedInit(DataTable p_dtMem)
        {
            dte.Value = Convert.ToDateTime(p_dtMem.Rows[0]["Date"].ToString().Substring(0, 4) + "."
                        + p_dtMem.Rows[0]["Date"].ToString().Substring(4, 2) + "."
                        + p_dtMem.Rows[0]["Date"].ToString().Substring(6, 2));
            cboMain.SelectedIndex = cboMain.FindStringExact(p_dtMem.Rows[0]["MainCate"].ToString());
            cboSmall.SelectedIndex = cboSmall.FindStringExact(p_dtMem.Rows[0]["SmallCate"].ToString());
            cboColor.SelectedIndex = cboColor.FindStringExact(p_dtMem.Rows[0]["Color"].ToString() == "" ? "흰색" : p_dtMem.Rows[0]["Color"].ToString());
            txtTitle.Text = p_dtMem.Rows[0]["Title"].ToString().Replace("♤", "'");
            if (p_dtMem.Rows[0]["Contents_Rtf"].ToString() != "")
            {
                rtxtContents.Rtf = p_dtMem.Rows[0]["Contents_Rtf"].ToString().Replace("♤", "'");
            }
            else
            {
                rtxtContents.Text = p_dtMem.Rows[0]["Contents"].ToString().Replace("♤", "'");
            }
        }

        /// <summary>
        /// 콤보 초기화
        /// </summary>
        /// <param name="p_Main"></param>
        /// <param name="p_Small"></param>
        private void fnComboInit()
        {
            cboMain.Items.Clear();
            cboSmall.Items.Clear();

            cboMain.Items.Add("<전체>");

            cboColor.Items.Add("흰색");
            cboColor.Items.Add("회색");
            cboColor.Items.Add("하늘");
            cboColor.Items.Add("노랑");
            cboColor.Items.Add("빨강");

            dtCombo = Network.GetDBTable(@"SELECT MainCate as '대분류', SmallCate as '소분류'
                                             FROM Memo
                                            WHERE Valid = 'Y'
                                            GROUP BY SmallCate, MainCate");

            foreach (DataRow dr in dtCombo.Rows)
            {
                if (!cboMain.Items.Contains(dr["대분류"].ToString()))
                {
                     cboMain.Items.Add(dr["대분류"].ToString());
                }
            }

        }


        /// <summary>
        /// 디비 저장  
        /// </summary>
        /// <param name="p_Seq"></param>
        /// <returns></returns>
        private bool fnSave(int p_Seq)
        {
            if (txtTitle.Text == "")
            {
                MessageBox.Show("제목에 내용이 없습니다.");
                return false;
            }

            string strSQL = "";
            string strCboMain = cboMain.Text.Equals("<전체>") ? "" : cboMain.Text;

            if (strCboMain.Replace(" ","").Equals(""))
            {
                MessageBox.Show("대분류의 내용이 없습니다.");
                return false;
            }

            //날짜를 바꿨을 경우 파일 경로도 다시 재조정

            if (mIsNew)
            {
                strSQL = @"INSERT INTO Memo ( Date, MainCate, SmallCate, Title, Contents,  Contents_Rtf, Color, Valid, ReportSeq, ReportType)
                            VALUES (
                                     '" + dte.Value.ToString("yyyyMMdd") + @"'
                                    ,'" + strCboMain + @"'
                                    ,'" + cboSmall.Text + @"'
                                    ,'" + txtTitle.Text.Replace("'","♤") + @"'
                                    ,'" + rtxtContents.Text.Replace("'", "♤") + @"'
                                    ,'" + rtxtContents.Rtf.Replace("'", "♤") + @"'
                                    ,'" + (cboColor.Text == "흰색" ? "" : cboColor.Text) + @"'
                                    ," + "'Y'" + @"
                                    ,'" + mReportSeq + @"'
                                    ,'" + mReportType + @"'
                                    )";
            }
            else
            {
                strSQL = @"UPDATE Memo 
                              SET Date = '" + dte.Value.ToString("yyyyMMdd") + @"', 
                                  MainCate = '" + strCboMain + @"',
                                  SmallCate = '" + cboSmall.Text + @"',
                                  Title = '" + txtTitle.Text.Replace("'", "♤") + @"',
                                  Contents = '" + rtxtContents.Text.Replace("'", "♤") + @"',
                                  Contents_Rtf = '" + rtxtContents.Rtf.Replace("'", "♤") + @"',
                                  Color = '" + (cboColor.Text == "흰색" ? "" : cboColor.Text) + @"'
                            WHERE Seq = " + mSeq;
            }

            try
            {
                int iresult = Network.ExecDB(strSQL);
                if (iresult > 0)
                    return true;
                else
                {
                    MessageBox.Show("저장실패");
                    return false;
                }
            }

            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return false;
            }


        }

        private void fnFontChange(int p_size, bool p_boldOpt)
        {
            int intSelectStart = rtxtContents.SelectionStart;
            int intSelectLength = rtxtContents.SelectionLength;
            int intSelectEnd = intSelectStart + intSelectLength;
            for (int x = intSelectStart; x < intSelectEnd; ++x)
            {
                rtxtContents.Select(x, 1);
                
                //굵기변경
                if(p_boldOpt)
                    rtxtContents.SelectionFont = new Font(rtxtContents.SelectionFont.FontFamily, rtxtContents.SelectionFont.Size, rtxtContents.SelectionFont.Bold ? FontStyle.Regular : FontStyle.Bold);

                else
                    rtxtContents.SelectionFont = new Font(rtxtContents.SelectionFont.FontFamily, rtxtContents.SelectionFont.Size + p_size, rtxtContents.SelectionFont.Bold ? FontStyle.Bold : FontStyle.Regular);
            }

            rtxtContents.Select(intSelectStart, intSelectLength);
        }

        #endregion 함수_End

        #region 이벤트 연결
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (fnSave(mSeq))
            {
                MessageBox.Show("저장되었습니다.");
                mDate = dte.Value.ToString("yyyyMMdd");
                mIsNew = false;
            }



        }

        private void btnSaveClose_Click(object sender, EventArgs e)
        {
            if (fnSave(mSeq))
            {
                MessageBox.Show("저장되었습니다.");
                this.Close();
            }

            else
            {
            }        
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnThick_Click(object sender, EventArgs e)
        {
            fnFontChange(0, true);
        }

        private void btnSizeUp_Click(object sender, EventArgs e)
        {
            fnFontChange(+1,false);
        }

        private void btnSizeDown_Click(object sender, EventArgs e)
        {
            fnFontChange(-1,false);
        }

        private void btnBlack_Click(object sender, EventArgs e)
        {
            rtxtContents.SelectionColor = Color.Black;
        }

        private void btnRed_Click(object sender, EventArgs e)
        {
            rtxtContents.SelectionColor = Color.Red;
        }

        private void btnBlue_Click(object sender, EventArgs e)
        {
            rtxtContents.SelectionColor = Color.Blue;
        }

        private void rtxtContents_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Alt && e.Shift && e.KeyCode == Keys.Q)
                {
                    fnFontChange(0, true);
                }

                else if (e.Alt && e.Shift && e.KeyCode == Keys.E)
                {
                    fnFontChange(+1, false);
                }

                else if (e.Alt && e.Shift && e.KeyCode == Keys.R)
                {
                    fnFontChange(-1, false);
                }
            }
            catch
            {}
        }
        
        private void cboMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboSmall.Items.Clear();
            
            if (cboMain.Text.Equals("<전체>"))
            {
                foreach (DataRow dr in dtCombo.Rows)
                {
                    if (!cboSmall.Items.Contains(dr["소분류"].ToString()))
                    {
                        cboSmall.Items.Add(dr["소분류"].ToString());
                    }
                }
            }


            foreach (DataRow dr in dtCombo.Rows)
            {
                if (dr["대분류"].ToString().Equals(cboMain.Text) && !cboSmall.Items.Contains(dr["소분류"].ToString()))
                {
                    cboSmall.Items.Add(dr["소분류"].ToString());
                }
            }
        }


        private void cboMain_Leave(object sender, EventArgs e)
        {
            cboSmall.Items.Clear();

            if (cboMain.Text.Equals("<전체>"))
            {
                foreach (DataRow dr in dtCombo.Rows)
                {
                    if (!cboSmall.Items.Contains(dr["소분류"].ToString()))
                    {
                        cboSmall.Items.Add(dr["소분류"].ToString());
                    }
                }
            }


            foreach (DataRow dr in dtCombo.Rows)
            {
                if (dr["대분류"].ToString().Equals(cboMain.Text) && !cboSmall.Items.Contains(dr["소분류"].ToString()))
                {
                    cboSmall.Items.Add(dr["소분류"].ToString());
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (!base.ProcessCmdKey(ref msg, keyData)) // 위에서 처리 안했으면
            {
                // 여기에 처리코드를 넣는다.
                if (keyData.Equals(Keys.F5))
                {
                    if (fnSave(mSeq))
                    {
                        MessageBox.Show("저장되었습니다.");
                        this.Close();
                    }
                    return true;
                }

                else if (keyData.Equals(Keys.F4))
                {
                    return true;
                }

                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        #endregion 이벤트 연결_End


    }
}