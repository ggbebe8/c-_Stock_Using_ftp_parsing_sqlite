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
    public partial class Revenue : Form
    {
        #region 전역변수

        //읽어온 자료들
        DataSet mds;
        #endregion 전역변수_End

        #region 생성자 
        public Revenue()
        {
            InitializeComponent();
        }

        private void Report_Load(object sender, EventArgs e)
        {

            chkDate.Checked = true;

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
                if (dataGridView1["Type", i].Value.ToString() == "매수")
                {
                    if (Convert.ToInt32(dataGridView1["Left", i].Value.ToString()) > 0)
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
                    else
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightPink;
                }
                if (dataGridView1["Type", i].Value.ToString() == "매도")
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightYellow;
                }
            }
        }

        /// <summary>
        /// 디비읽기
        /// </summary>
        private void fnReadDB()
        {
            string strSQL = "";
            mds = new DataSet();
            strSQL = @"SELECT Date AS 날짜
                            , Name AS 종목명
                            , BPrice AS 매수가
                            , SPrice AS 매도가
                            , Quantity AS 수량 
                            , BPrice * Quantity AS 매수합계
                            , SPrice * Quantity AS 매도합계
                            , SPrice * Quantity * 0.003 AS 수수료
                            , (SPrice * Quantity) - (BPrice * Quantity) - (SPrice * Quantity * 0.003) AS 손익
                            , ((SPrice * Quantity) - (BPrice * Quantity) - (SPrice * Quantity * 0.003)) / (BPrice * Quantity) * 100 AS 비율
                         FROM Revenue
                         WHERE Name LIKE '%%'";

           if(!chkDate.Checked)
            {
                strSQL += " AND Date >= '" + dteF.Value.ToString("yyyyMMdd") + "' AND Date <= '" + dteS.Value.ToString("yyyyMMdd") + "'";
            }

            if (txtName.Text.Replace(" ","") != "")
            {
                strSQL += " AND Name LIKE '%" + txtName.Text + "%'";
            }
            strSQL += ";";

            mds = Network.GetDBSet(strSQL);

            dataGridView1.DataSource = mds.Tables[0];

            //그리드 초기화
            fnSortGrid(true);

            //셀색칠
            //fnPaint();
        }

        /// <summary>
        /// 그리드초기화
        /// </summary>
        /// <param name="p_isSearch"></param>
        private void fnSortGrid(bool p_isSearch)
        {
            if (p_isSearch)
            {
                double dSum = 0;

                foreach (DataGridViewColumn col in dataGridView1.Columns)
                {
                    col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                dataGridView1.Columns["날짜"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns["종목명"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns["매수가"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns["매도가"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns["수량"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns["매수합계"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns["매도합계"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns["수수료"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns["손익"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns["비율"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


                dataGridView1.Columns["매수가"].DefaultCellStyle.Format = "#,##";
                dataGridView1.Columns["매도가"].DefaultCellStyle.Format = "#,##";
                dataGridView1.Columns["수량"].DefaultCellStyle.Format = "#,##";
                dataGridView1.Columns["매수합계"].DefaultCellStyle.Format = "#,##";
                dataGridView1.Columns["매도합계"].DefaultCellStyle.Format = "#,##";
                dataGridView1.Columns["수수료"].DefaultCellStyle.Format = "#,##";
                dataGridView1.Columns["손익"].DefaultCellStyle.Format = "#,##";
                dataGridView1.Columns["비율"].DefaultCellStyle.Format = "0.##";


                //색칠하기 // 합계도 같이 
               for (int i = 0; i < dataGridView1.Rows.Count; i++)
               {
                   if (Convert.ToDouble(dataGridView1["손익", i].Value.ToString()) < 0)
                   {
                       dataGridView1["손익",i].Style.ForeColor = Color.Blue;
                       dSum += Convert.ToDouble(dataGridView1["손익", i].Value.ToString());
                   }
                   else if (Convert.ToDouble(dataGridView1["손익", i].Value.ToString()) >= 0)
                   {
                       dataGridView1["손익", i].Style.ForeColor = Color.Red;
                       dSum += Convert.ToDouble(dataGridView1["손익", i].Value.ToString());
                   }

                   if (Convert.ToDouble(dataGridView1["비율", i].Value.ToString()) < 0)
                   {
                       dataGridView1["비율", i].Style.ForeColor = Color.Blue;
                   }
                   else if (Convert.ToDouble(dataGridView1["비율", i].Value.ToString()) >= 0)
                   {
                       dataGridView1["비율", i].Style.ForeColor = Color.Red;
                   }

               }

                //포커스 
               if (dataGridView1.Rows.Count > 0)
               {
                   dataGridView1.Rows[dataGridView1.Rows.Count - 1].Selected = true;
                   dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["날짜"];
                   dataGridView1.BeginEdit(true);
               }

                //합계구하기
               txtRevSum.Text = dSum.ToString("#,##");
               txtRevSum.ForeColor = dSum >= 0 ? Color.Red : Color.Blue;

            }
            

            //Resize를 위해 if문으로 구분을 두었음

            dataGridView1.Columns["날짜"].Width = 70;
            dataGridView1.Columns["매수가"].Width = 70;
            dataGridView1.Columns["매도가"].Width = 70;
            dataGridView1.Columns["수량"].Width = 60;
            dataGridView1.Columns["매수합계"].Width = 80;
            dataGridView1.Columns["매도합계"].Width = 80;
            dataGridView1.Columns["수수료"].Width = 60;
            dataGridView1.Columns["손익"].Width = 70;
            dataGridView1.Columns["비율"].Width = 60;

            if (dataGridView1.Height - 20 - (dataGridView1.RowTemplate.Height * dataGridView1.RowCount) > 0)
            {
                //스크롤바 안 생기는 경우
                dataGridView1.Columns["종목명"].Width = dataGridView1.Width - 620;
                //MessageBox.Show(dataGridView1.RowTemplate.Height.ToString() + ", " + dataGridView1.Height.ToString() + ", " + dataGridView1.RowCount + ", " + (dataGridView1.RowTemplate.Height * dataGridView1.RowCount));
            }
            else
            {
                //스크롤바 생기는 경우
                dataGridView1.Columns["종목명"].Width = dataGridView1.Width - 640;
            }
            
        }



        /// <summary>
        /// 그리드 초기화  //tab2 
        /// </summary>
        /// <param name="p_isSearch"></param>
        private void fnSortGrid(bool p_isSearch, DataGridView p_dgv, string p_colDate, string p_colRev)
        {
            if (p_isSearch)
            {
                foreach (DataGridViewColumn col in p_dgv.Columns)
                {
                    col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                p_dgv.Columns[p_colDate].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                p_dgv.Columns[p_colRev].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                p_dgv.Columns[p_colRev].DefaultCellStyle.Format = "#,##";

                //포커스 
                if (p_dgv.Rows.Count > 0)
                {
                    p_dgv.Rows[p_dgv.Rows.Count - 1].Selected = true;
                    p_dgv.CurrentCell = p_dgv.Rows[p_dgv.Rows.Count - 1].Cells[p_colDate];
                    p_dgv.BeginEdit(true);
                }
            }


            if (p_dgv.Height - 20 - (p_dgv.RowTemplate.Height * p_dgv.RowCount) > 0)
            {
                //스크롤바 안 생기는 경우
                p_dgv.Columns[p_colDate].Width = p_dgv.Size.Width / 2;
                p_dgv.Columns[p_colRev].Width = p_dgv.Size.Width / 2;
                //MessageBox.Show(dataGridView1.RowTemplate.Height.ToString() + ", " + dataGridView1.Height.ToString() + ", " + dataGridView1.RowCount + ", " + (dataGridView1.RowTemplate.Height * dataGridView1.RowCount));
            }
            else
            {
                //스크롤바 생기는 경우
                p_dgv.Columns[p_colDate].Width = p_dgv.Size.Width / 2 - 10;
                p_dgv.Columns[p_colRev].Width = p_dgv.Size.Width / 2 - 10;
            }
        }


        /// <summary>
        /// tab2 Year불러오기. 
        /// </summary>
        private void fnReadDBY()
        {

            double dSum;
            string strSQL = @"SELECT MAX(SUBSTR(Date,1,4)) AS 년, SUM((SPrice * Quantity) - (BPrice * Quantity) - (SPrice * Quantity * 0.003)) AS 손익
                                FROM Revenue
                                GROUP BY SUBSTR(Date,1,4);";
            strSQL += @"SELECT SUM(손익) AS 합
                         FROM (
                              SELECT MAX(SUBSTR(Date,1,4)) AS 년, SUM((SPrice * Quantity) - (BPrice * Quantity) - (SPrice * Quantity * 0.003)) AS 손익
                                FROM Revenue
                               GROUP BY SUBSTR(Date,1,4)
                              );";

            DataSet dsY = Network.GetDBSet(strSQL);

            dgvYear.DataSource = dsY.Tables[0];

            fnSortGrid(true, dgvYear, "년", "손익");

            if (Convert.ToInt32(dsY.Tables[0].Rows.Count) > 0)
            {
                dSum = Convert.ToDouble(dsY.Tables[1].Rows[0][0].ToString());
                txtSumY.Text = dSum.ToString("#,##");
            }
        }


        private void fnReadDBM(string p_Year)
        {
            double dSum;

            string strSQL = @"SELECT MAX(SUBSTR(Date,5,2)) AS 월, SUM((SPrice * Quantity) - (BPrice * Quantity) - (SPrice * Quantity * 0.003)) AS 손익
                                FROM Revenue
                               WHERE SUBSTR(Date,1,4) = '" + p_Year + @"' 
                               GROUP BY SUBSTR(Date,1,6);";

            strSQL += @"SELECT SUM(손익) AS 합
                         FROM (
                              SELECT MAX(SUBSTR(Date,5,2)) AS 월, SUM((SPrice * Quantity) - (BPrice * Quantity) - (SPrice * Quantity * 0.003)) AS 손익
                                FROM Revenue
                               WHERE SUBSTR(Date,1,4) = '" + p_Year + @"' 
                               GROUP BY SUBSTR(Date,1,6)
                              );";

            DataSet dsM = Network.GetDBSet(strSQL);

            dgvMonth.DataSource = dsM.Tables[0];

            fnSortGrid(true, dgvMonth, "월", "손익");

            if (Convert.ToInt32(dsM.Tables[0].Rows.Count) > 0)
            {
                dSum = Convert.ToDouble(dsM.Tables[1].Rows[0][0].ToString());
                txtSumM.Text = dSum.ToString("#,##");
            }
        }


        private void fnReadDBD(string p_YearMonth)
        {
            double dSum;
            string strSQL = @"SELECT MAX(SUBSTR(Date,7,2)) AS 일, SUM((SPrice * Quantity) - (BPrice * Quantity) - (SPrice * Quantity * 0.003)) AS 손익
                                FROM Revenue
                               WHERE SUBSTR(Date,1,6) = '" + p_YearMonth + @"' 
                               GROUP BY SUBSTR(Date,1,8)
                               ORDER BY 일;";

            strSQL += @"SELECT SUM(손익) AS 합
                         FROM (
                              SELECT MAX(SUBSTR(Date,7,2)) AS 일, SUM((SPrice * Quantity) - (BPrice * Quantity) - (SPrice * Quantity * 0.003)) AS 손익
                                FROM Revenue
                               WHERE SUBSTR(Date,1,6) = '" + p_YearMonth + @"' 
                               GROUP BY SUBSTR(Date,1,8)
                              );";

            DataSet dsD = Network.GetDBSet(strSQL);

            dgvDate.DataSource = dsD.Tables[0];

            fnSortGrid(true, dgvDate, "일", "손익");
            if (dsD.Tables[0].Rows.Count > 0)
            {
                dSum = Convert.ToDouble(dsD.Tables[1].Rows[0][0].ToString());
                txtSumD.Text = dSum.ToString("#,##");
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

        #endregion 이벤트연결 함수


        #region 이벤트
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

        #endregion 이벤트_End

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                fnReadDBY();
                fnReadDBM(dgvYear["년", dgvYear.CurrentCell.RowIndex].Value.ToString());
                fnReadDBD(dgvYear["년", dgvYear.CurrentCell.RowIndex].Value.ToString() + dgvMonth["월", dgvMonth.CurrentCell.RowIndex].Value.ToString());
            }
            catch (NullReferenceException)
            {
                fnReadDBY();
                fnReadDBM("");
                fnReadDBD("");
            }
        }

        private void dgvYear_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                fnReadDBM(dgvYear["년", dgvYear.CurrentCell.RowIndex].Value.ToString());
                fnReadDBD(dgvYear["년", dgvYear.CurrentCell.RowIndex].Value.ToString() + dgvMonth["월", dgvMonth.CurrentCell.RowIndex].Value.ToString());
            }
            catch (NullReferenceException)
            {
                fnReadDBM("");
                fnReadDBD("");
            }
        }

        private void dgvMonth_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                fnReadDBD(dgvYear["년", dgvYear.CurrentCell.RowIndex].Value.ToString() + dgvMonth["월", dgvMonth.CurrentCell.RowIndex].Value.ToString());
            }
            catch (NullReferenceException)
            {
                fnReadDBD("");
            }

        }



        ///////////////////////////////////////////////////////////////////////////



    }
}