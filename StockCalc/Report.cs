using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace StockCalc
{
    public partial class Report : Form
    {
        #region 전역변수



        //읽어온 자료들
        DataSet mds;
        #endregion 전역변수_End

        #region 생성자 
        public Report()
        {
            InitializeComponent();
        }

        //그리드 더블클릭으로 들어왔을 경우
        public Report(string p_Name)
        {

            InitializeComponent();

            txtName.Text = p_Name;

        }


        private void Report_Load(object sender, EventArgs e)
        {
            chkDate.Checked = true;

            //콤보초기화
            fnInitComb();

            //디비읽기
            fnReadDB();

            //포커스초기화
            dataGridView1.ClearSelection();
        }

        #endregion 생성자_End

        #region 내부함수


        private void fnPaint()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1["종류", i].Value.ToString() == "매수")
                {
                    if (Convert.ToInt32(dataGridView1["잔량", i].Value.ToString()) > 0)
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightPink;
                    else
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
                }
                if (dataGridView1["종류", i].Value.ToString() == "매도")
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightYellow;
                }
            }
        }

        /// <summary>
        /// 콤보박스 초기화
        /// </summary>
        private void fnInitComb()
        {
            cboType.Items.Clear();

            cboType.Items.Add("<전체>");
            cboType.Items.Add("매수");
            cboType.Items.Add("매도");

            cboType.SelectedItem = "<전체>";
        }
        /// <summary>
        /// 디비읽기
        /// </summary>
        private void fnReadDB()
        {
            //디비연결

            string strSQL = "";
            mds = new DataSet();
            strSQL = @"SELECT b.Date AS 날짜
                              , '매수' AS 종류
                              , b.Name AS 종목명
                              , b.Quantity AS 수량
                              , b.Left AS 잔량
                              , b.Price AS 가격
                              , b.Quantity * Price AS 합계
                              , CASE WHEN IFNULL(m.ReportSeq,'') = ''
                                     THEN ''
                                     ELSE 'Y' END AS 메모
                              , b.Seq
                              , m.Seq AS memSeq
                         FROM Buy b
                         LEFT JOIN Memo m ON b.Seq = m.ReportSeq AND m.ReportType = 'Buy' AND m.Valid = 'Y'
                        WHERE b.Name LIKE '%%'"; // 이거 왜 이렇게 해놧지??
            if (cboType.Text == "매도")
            {
                strSQL += " AND Name = '!#@$%#^&#%^#'";     //매수일 경우만 검색이 되도록 더미값을 넣음
            }
            if(!chkDate.Checked)
            {
                strSQL += " AND Date >= '" + dteF.Value.ToString("yyyyMMdd") + "' AND Date <= '" + dteS.Value.ToString("yyyyMMdd") + "'";
            }
            if(txtName.Text.Replace(" ","") != "")
            {
                strSQL += " AND Name LIKE '%" + txtName.Text.ToUpper() + "%'";
            }
            strSQL += @" 
                        UNION ALL
                       SELECT s.Date AS 날짜
                              , '매도' AS 종류
                              , s.Name AS 종목명
                              , s.Quantity AS 수량
                              , ''
                              , s.Price AS 가격
                              , s.Quantity * Price AS 합계
                              , CASE WHEN IFNULL(m.ReportSeq,'') = ''
                                     THEN ''
                                     ELSE 'Y' END AS 메모
                              , s.Seq
                              , m.Seq AS memSeq
                         FROM Sell s
                         LEFT JOIN Memo m ON s.Seq = m.ReportSeq AND m.ReportType = 'Sell' AND m.Valid = 'Y'
                        WHERE s.Name LIKE '%%'";
            if (cboType.Text == "매수")
            {
                strSQL += " AND Name = '!#@$%#^&#%^#'";     //매도일 경우만 검색이 되도록 
            }
            if (!chkDate.Checked)
            {
                strSQL += " AND Date >= '" + dteF.Value.ToString("yyyyMMdd") + "' AND Date <= '" + dteF.Value.ToString("yyyyMMdd") + "'";
            }
            if (txtName.Text.Replace(" ", "") != "")
            {
                strSQL += " AND Name LIKE '%" + txtName.Text.ToUpper() + "%'";
            }
            strSQL += @" 
                       ORDER BY 날짜
                      ";
            strSQL += ";";

            /*
            strSQL += @"
                            SELECT CASE WHEN (SELECT COUNT(*) FROM Sell) < 1
                                        THEN -1
                                        ELSE (SELECT MAX(Seq) FROM Sell) END;
                        ";
             */

            mds = Network.GetDBSet(strSQL);

            dataGridView1.DataSource = mds.Tables[0];

            //그리드 초기화
            fnSortGrid(true); 

            //셀색칠
            fnPaint();
        }

        /// <summary>
        /// 그리드초기화
        /// </summary>
        /// <param name="p_isSearch"></param>
        private void fnSortGrid(bool p_isSearch)
        {
            if (p_isSearch)
            {
                foreach (DataGridViewColumn col in dataGridView1.Columns)
                {
                    col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                dataGridView1.Columns["날짜"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns["종류"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns["종목명"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns["수량"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns["가격"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns["합계"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns["잔량"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns["메모"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dataGridView1.Columns["수량"].DefaultCellStyle.Format = "#,##";
                dataGridView1.Columns["가격"].DefaultCellStyle.Format = "#,##";
                dataGridView1.Columns["합계"].DefaultCellStyle.Format = "#,##";
                dataGridView1.Columns["잔량"].DefaultCellStyle.Format = "#,##";

                //포커스 
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Selected = true;
                    dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["종목명"];
                    dataGridView1.BeginEdit(true);
                }
            }

            dataGridView1.Columns["날짜"].Width = 70;
            dataGridView1.Columns["종류"].Width = 50;
            dataGridView1.Columns["수량"].Width = 70;
            dataGridView1.Columns["가격"].Width = 70;
            dataGridView1.Columns["합계"].Width = 70;
            dataGridView1.Columns["잔량"].Width = 70;
            dataGridView1.Columns["메모"].Width = 40;
            //dataGridView1.Columns["잔량"].Visible = false;
            dataGridView1.Columns["memSeq"].Visible = false;
            dataGridView1.Columns["Seq"].Visible = false;

            if (dataGridView1.Height - 20 - (dataGridView1.RowTemplate.Height * dataGridView1.RowCount) > 0)
            {
                //스크롤바 안 생기는 경우
                dataGridView1.Columns["종목명"].Width = dataGridView1.Width - 443;
                //MessageBox.Show(dataGridView1.RowTemplate.Height.ToString() + ", " + dataGridView1.Height.ToString() + ", " + dataGridView1.RowCount + ", " + (dataGridView1.RowTemplate.Height * dataGridView1.RowCount));
            }
            else
            {
                //스크롤바 생기는 경우
                dataGridView1.Columns["종목명"].Width = dataGridView1.Width - 463;
            }
        }

        /// <summary>
        /// 현재의 포커스 주기
        /// </summary>
        /// <param name="p_scrIndex"></param>
        /// <param name="p_scrFocus"></param>
        private void fnFocus(int p_scrIndex, int p_scrFocus)
        {
            //포커스 
            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows[p_scrFocus].Selected = true;
                dataGridView1.CurrentCell = dataGridView1.Rows[p_scrFocus].Cells[0];
                dataGridView1.BeginEdit(true);
            }
            
            //스크롤 위치
            if (p_scrIndex != 0)
            {
                dataGridView1.FirstDisplayedScrollingRowIndex = p_scrIndex;
            }
        }

        #endregion 내부함수_End


        #region 이벤트연결 함수
        /// <summary>
        /// 달력 이동
        /// </summary>
        /// <param name="p_Num"></param>
        private void efnMoveMonth(int p_Num)
        {
            DateTime dteTemp = dteF.Value.AddMonths(p_Num);
            dteF.Value = new DateTime(dteTemp.Year, dteTemp.Month, 1, 0, 0, 0);
            dteS.Value = new DateTime(dteTemp.Year, dteTemp.Month, DateTime.DaysInMonth(dteTemp.Year, dteTemp.Month), 0, 0, 0);
        }

        /// <summary>
        /// 삭제 버튼 연결 
        /// </summary>
        private void efnDel()
        {
            //디비연결

            string strSQL = "";
            string strName = dataGridView1.CurrentRow.Cells["종목명"].Value.ToString(); //선택된 이름
            DataRow[] drr = mds.Tables[0].Select("종류 = '매도' AND 종목명 = '" + strName + "' AND 잔량 <> 수량", "Seq DESC");

            if (dataGridView1.CurrentRow.Cells["종류"].Value.ToString() == "매수")
            {
                if (dataGridView1.CurrentRow.Cells["잔량"].Value.ToString() != dataGridView1.CurrentRow.Cells["수량"].Value.ToString())
                {
                    MessageBox.Show("해당 종목의 가장 최근 매도를 먼저 지우십시오.\r\n(날짜 : " + drr[0]["날짜"].ToString() + ", 수량 : " + drr[0]["수량"].ToString() + ", 가격 : " + drr[0]["가격"].ToString() + ")");
                    return;
                }
                strSQL = @"
                           DELETE FROM Buy
                            WHERE Seq = '" + dataGridView1.CurrentRow.Cells["Seq"].Value.ToString() + @"';
                          ";
                int iresult = Network.ExecDB(strSQL);
                if (iresult > 0)
                {
                    MessageBox.Show("삭제성공");
                }
                else
                {
                    MessageBox.Show("삭제실패");
                }

            }

            else if (dataGridView1.CurrentRow.Cells["종류"].Value.ToString() == "매도")
            {
                string strSellSeq;// = mds.Tables[1].Rows[0][0].ToString();    //선택된 Seq

                int i = 0; //반복문 변수

                int intSellQ = Convert.ToInt32(dataGridView1.CurrentRow.Cells["수량"].Value.ToString());    //판매하고자 하는 수량 
                int intQ;   //buy의 남은 수량

                DataRow[] dr = mds.Tables[0].Select("종류 = '매수' AND 종목명 = '" + strName + "' AND 잔량 <> 수량", "Seq DESC");


                strSellSeq = drr[0]["Seq"].ToString();

                if (dataGridView1.CurrentRow.Cells["Seq"].Value.ToString() != strSellSeq)
                {
                    MessageBox.Show("해당 종목의 가장 최근 매도를 먼저 지우십시오.\r\n(날짜 : " + drr[0]["날짜"].ToString() + ", 수량 : " + drr[0]["수량"].ToString() + ", 가격 : " + drr[0]["가격"].ToString() + ")");
                    return;
                }
                else
                {
                    strSQL = @"
                                DELETE FROM Revenue
                                 WHERE SellSeq = '" + strSellSeq + @"';
                              ";

                    strSQL += @"
                               DELETE FROM Sell
                                WHERE Seq = '" + strSellSeq + @"'; 
                              ";

                    while (intSellQ > 0)
                    {
                        intQ = Convert.ToInt32(dr[i]["수량"].ToString()) - Convert.ToInt32(dr[i]["잔량"].ToString());

                        if (intSellQ >= intQ)
                        {
                            strSQL += @"
                                        UPDATE Buy
                                           SET Left = Quantity
                                         WHERE Seq = '" + dr[i]["Seq"].ToString() + @"';
                                       ";
                            intSellQ = intSellQ - intQ;
                        }
                        else
                        {
                            strSQL += @"
                                        UPDATE Buy
                                           SET Left = " + (Convert.ToInt32(dr[i]["잔량"].ToString()) + intSellQ) + @"
                                         WHERE Seq = '" + dr[i]["Seq"].ToString() + @"';
                                       ";
                            intSellQ = 0;
                        }

                        i++;
                    }


                    int iresult = Network.ExecDB(strSQL);
                    if (iresult > 0)
                    {
                        MessageBox.Show("삭제 성공");
                    }
                    else
                    {
                        MessageBox.Show("삭제실패");
                    }
                }
            }
            fnReadDB();
            
        }

        /// <summary>
        /// 메모 열기 
        /// </summary>
        private void efnMemo()
        {
            /*
            string strType = dataGridView1.CurrentRow.Cells["종류"].Value.ToString();
            string strSeq = dataGridView1.CurrentRow.Cells["Seq"].Value.ToString();
            string strMemo = dataGridView1.CurrentRow.Cells["Memo"].Value.ToString();
            int intScrIndex = dataGridView1.FirstDisplayedScrollingRowIndex;
            int intScrFocus = dataGridView1.CurrentRow.Index;

            Memo ME = new Memo(strType, strSeq, strMemo);
            ME.StartPosition = FormStartPosition.CenterParent;
            if (ME.ShowDialog() == DialogResult.OK)
            {
            }
            


            fnReadDB();
            fnFocus(intScrIndex, intScrFocus);
             * */
        }

        private void efnMemo(string p_reportType, string p_reportSeq, string p_memSeq)
        {
            int intScrIndex = dataGridView1.FirstDisplayedScrollingRowIndex;
            int intScrFocus = dataGridView1.CurrentRow.Index;

            EnrollForm ME = new EnrollForm(p_reportType, p_reportSeq, p_memSeq);
            ME.StartPosition = FormStartPosition.CenterParent;
            if (ME.ShowDialog() == DialogResult.OK)
            {}
            fnReadDB();
            fnFocus(intScrIndex, intScrFocus);
        }
        #endregion 이벤트연결 함수


        #region 이벤트
        /// <summary>
        /// 메모버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMemo_Click(object sender, EventArgs e)
        {
            efnMemo();
        }

        /// <summary>
        /// 삭제버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            efnDel();
        }


        /// <summary>
        /// 검색버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            fnReadDB();
        }

        /// <summary>
        /// 달력 화살표버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBefore_Click(object sender, EventArgs e)
        {
            efnMoveMonth(-1);
        }

        /// <summary>
        /// 달력 화살표버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAfter_Click(object sender, EventArgs e)
        {
            efnMoveMonth(1);
        }

        /// <summary>
        /// 전체날짜 체크 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDate.Checked)
            {
                btnBefore.Enabled = false;
                btnAfter.Enabled = false;
                dteF.Enabled = false;
                dteS.Enabled = false;
            }

            else
            {
                btnBefore.Enabled = true;
                btnAfter.Enabled = true;
                dteF.Enabled = true;
                dteS.Enabled = true;
            }
        }

        /// <summary>
        /// 콤보 읽기전용으로 만들기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Down && e.KeyCode != Keys.Up)
            {
                e.SuppressKeyPress = true;
            }
        }

        /// <summary>
        /// txtbox에서 엔터치면 검색
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
                fnReadDB();
        }

        /// <summary>
        /// 창 크기 조절 시 컬럼 크기 자동조정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Report_Resize(object sender, EventArgs e)
        {
            try
            {
                fnSortGrid(false);
            }
            catch { }
        }

        /// <summary>
        /// 더블클릭 시 메모
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string reportSeq = "";
            string memSeq = "";
            string reportType = "";

            reportType = dataGridView1.CurrentRow.Cells["종류"].Value.ToString() == "매수" ? "Buy"
                         : dataGridView1.CurrentRow.Cells["종류"].Value.ToString() == "매도" ? "Sell" : "";
            reportSeq = dataGridView1.CurrentRow.Cells["Seq"].Value.ToString();
            memSeq = dataGridView1.CurrentRow.Cells["memSeq"].Value.ToString();
           
            efnMemo(reportType, reportSeq, memSeq);
        }
        #endregion 이벤트_End



    }
}