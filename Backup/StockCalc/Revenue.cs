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
        #region ��������

        //�о�� �ڷ��
        DataSet mds;
        #endregion ��������_End

        #region ������ 
        public Revenue()
        {
            InitializeComponent();
        }

        private void Report_Load(object sender, EventArgs e)
        {

            chkDate.Checked = true;

            //����б�
            fnReadDB();

            //��Ŀ���ʱ�ȭ
            dataGridView1.ClearSelection();

        }

        #endregion ������_End

        #region �����Լ�


        private void fnPaint()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1["Type", i].Value.ToString() == "�ż�")
                {
                    if (Convert.ToInt32(dataGridView1["Left", i].Value.ToString()) > 0)
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
                    else
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightPink;
                }
                if (dataGridView1["Type", i].Value.ToString() == "�ŵ�")
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightYellow;
                }
            }
        }

        /// <summary>
        /// ����б�
        /// </summary>
        private void fnReadDB()
        {
            string strSQL = "";
            mds = new DataSet();
            strSQL = @"SELECT Date AS ��¥
                            , Name AS �����
                            , BPrice AS �ż���
                            , SPrice AS �ŵ���
                            , Quantity AS ���� 
                            , BPrice * Quantity AS �ż��հ�
                            , SPrice * Quantity AS �ŵ��հ�
                            , SPrice * Quantity * 0.003 AS ������
                            , (SPrice * Quantity) - (BPrice * Quantity) - (SPrice * Quantity * 0.003) AS ����
                            , ((SPrice * Quantity) - (BPrice * Quantity) - (SPrice * Quantity * 0.003)) / (BPrice * Quantity) * 100 AS ����
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

            //�׸��� �ʱ�ȭ
            fnSortGrid(true);

            //����ĥ
            //fnPaint();
        }

        /// <summary>
        /// �׸����ʱ�ȭ
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
                dataGridView1.Columns["��¥"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns["�����"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns["�ż���"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns["�ŵ���"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns["����"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns["�ż��հ�"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns["�ŵ��հ�"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns["������"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns["����"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns["����"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


                dataGridView1.Columns["�ż���"].DefaultCellStyle.Format = "#,##";
                dataGridView1.Columns["�ŵ���"].DefaultCellStyle.Format = "#,##";
                dataGridView1.Columns["����"].DefaultCellStyle.Format = "#,##";
                dataGridView1.Columns["�ż��հ�"].DefaultCellStyle.Format = "#,##";
                dataGridView1.Columns["�ŵ��հ�"].DefaultCellStyle.Format = "#,##";
                dataGridView1.Columns["������"].DefaultCellStyle.Format = "#,##";
                dataGridView1.Columns["����"].DefaultCellStyle.Format = "#,##";
                dataGridView1.Columns["����"].DefaultCellStyle.Format = "0.##";


                //��ĥ�ϱ� // �հ赵 ���� 
               for (int i = 0; i < dataGridView1.Rows.Count; i++)
               {
                   if (Convert.ToDouble(dataGridView1["����", i].Value.ToString()) < 0)
                   {
                       dataGridView1["����",i].Style.ForeColor = Color.Blue;
                       dSum += Convert.ToDouble(dataGridView1["����", i].Value.ToString());
                   }
                   else if (Convert.ToDouble(dataGridView1["����", i].Value.ToString()) >= 0)
                   {
                       dataGridView1["����", i].Style.ForeColor = Color.Red;
                       dSum += Convert.ToDouble(dataGridView1["����", i].Value.ToString());
                   }

                   if (Convert.ToDouble(dataGridView1["����", i].Value.ToString()) < 0)
                   {
                       dataGridView1["����", i].Style.ForeColor = Color.Blue;
                   }
                   else if (Convert.ToDouble(dataGridView1["����", i].Value.ToString()) >= 0)
                   {
                       dataGridView1["����", i].Style.ForeColor = Color.Red;
                   }

               }

                //��Ŀ�� 
               if (dataGridView1.Rows.Count > 0)
               {
                   dataGridView1.Rows[dataGridView1.Rows.Count - 1].Selected = true;
                   dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["��¥"];
                   dataGridView1.BeginEdit(true);
               }

                //�հ豸�ϱ�
               txtRevSum.Text = dSum.ToString("#,##");
               txtRevSum.ForeColor = dSum >= 0 ? Color.Red : Color.Blue;

            }
            

            //Resize�� ���� if������ ������ �ξ���

            dataGridView1.Columns["��¥"].Width = 70;
            dataGridView1.Columns["�ż���"].Width = 70;
            dataGridView1.Columns["�ŵ���"].Width = 70;
            dataGridView1.Columns["����"].Width = 60;
            dataGridView1.Columns["�ż��հ�"].Width = 80;
            dataGridView1.Columns["�ŵ��հ�"].Width = 80;
            dataGridView1.Columns["������"].Width = 60;
            dataGridView1.Columns["����"].Width = 70;
            dataGridView1.Columns["����"].Width = 60;

            if (dataGridView1.Height - 20 - (dataGridView1.RowTemplate.Height * dataGridView1.RowCount) > 0)
            {
                //��ũ�ѹ� �� ����� ���
                dataGridView1.Columns["�����"].Width = dataGridView1.Width - 620;
                //MessageBox.Show(dataGridView1.RowTemplate.Height.ToString() + ", " + dataGridView1.Height.ToString() + ", " + dataGridView1.RowCount + ", " + (dataGridView1.RowTemplate.Height * dataGridView1.RowCount));
            }
            else
            {
                //��ũ�ѹ� ����� ���
                dataGridView1.Columns["�����"].Width = dataGridView1.Width - 640;
            }
            
        }



        /// <summary>
        /// �׸��� �ʱ�ȭ  //tab2 
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

                //��Ŀ�� 
                if (p_dgv.Rows.Count > 0)
                {
                    p_dgv.Rows[p_dgv.Rows.Count - 1].Selected = true;
                    p_dgv.CurrentCell = p_dgv.Rows[p_dgv.Rows.Count - 1].Cells[p_colDate];
                    p_dgv.BeginEdit(true);
                }
            }


            if (p_dgv.Height - 20 - (p_dgv.RowTemplate.Height * p_dgv.RowCount) > 0)
            {
                //��ũ�ѹ� �� ����� ���
                p_dgv.Columns[p_colDate].Width = p_dgv.Size.Width / 2;
                p_dgv.Columns[p_colRev].Width = p_dgv.Size.Width / 2;
                //MessageBox.Show(dataGridView1.RowTemplate.Height.ToString() + ", " + dataGridView1.Height.ToString() + ", " + dataGridView1.RowCount + ", " + (dataGridView1.RowTemplate.Height * dataGridView1.RowCount));
            }
            else
            {
                //��ũ�ѹ� ����� ���
                p_dgv.Columns[p_colDate].Width = p_dgv.Size.Width / 2 - 10;
                p_dgv.Columns[p_colRev].Width = p_dgv.Size.Width / 2 - 10;
            }
        }


        /// <summary>
        /// tab2 Year�ҷ�����. 
        /// </summary>
        private void fnReadDBY()
        {

            double dSum;
            string strSQL = @"SELECT MAX(SUBSTR(Date,1,4)) AS ��, SUM((SPrice * Quantity) - (BPrice * Quantity) - (SPrice * Quantity * 0.003)) AS ����
                                FROM Revenue
                                GROUP BY SUBSTR(Date,1,4);";
            strSQL += @"SELECT SUM(����) AS ��
                         FROM (
                              SELECT MAX(SUBSTR(Date,1,4)) AS ��, SUM((SPrice * Quantity) - (BPrice * Quantity) - (SPrice * Quantity * 0.003)) AS ����
                                FROM Revenue
                               GROUP BY SUBSTR(Date,1,4)
                              );";

            DataSet dsY = Network.GetDBSet(strSQL);

            dgvYear.DataSource = dsY.Tables[0];

            fnSortGrid(true, dgvYear, "��", "����");

            if (Convert.ToInt32(dsY.Tables[0].Rows.Count) > 0)
            {
                dSum = Convert.ToDouble(dsY.Tables[1].Rows[0][0].ToString());
                txtSumY.Text = dSum.ToString("#,##");
            }
        }


        private void fnReadDBM(string p_Year)
        {
            double dSum;

            string strSQL = @"SELECT MAX(SUBSTR(Date,5,2)) AS ��, SUM((SPrice * Quantity) - (BPrice * Quantity) - (SPrice * Quantity * 0.003)) AS ����
                                FROM Revenue
                               WHERE SUBSTR(Date,1,4) = '" + p_Year + @"' 
                               GROUP BY SUBSTR(Date,1,6);";

            strSQL += @"SELECT SUM(����) AS ��
                         FROM (
                              SELECT MAX(SUBSTR(Date,5,2)) AS ��, SUM((SPrice * Quantity) - (BPrice * Quantity) - (SPrice * Quantity * 0.003)) AS ����
                                FROM Revenue
                               WHERE SUBSTR(Date,1,4) = '" + p_Year + @"' 
                               GROUP BY SUBSTR(Date,1,6)
                              );";

            DataSet dsM = Network.GetDBSet(strSQL);

            dgvMonth.DataSource = dsM.Tables[0];

            fnSortGrid(true, dgvMonth, "��", "����");

            if (Convert.ToInt32(dsM.Tables[0].Rows.Count) > 0)
            {
                dSum = Convert.ToDouble(dsM.Tables[1].Rows[0][0].ToString());
                txtSumM.Text = dSum.ToString("#,##");
            }
        }


        private void fnReadDBD(string p_YearMonth)
        {
            double dSum;
            string strSQL = @"SELECT MAX(SUBSTR(Date,7,2)) AS ��, SUM((SPrice * Quantity) - (BPrice * Quantity) - (SPrice * Quantity * 0.003)) AS ����
                                FROM Revenue
                               WHERE SUBSTR(Date,1,6) = '" + p_YearMonth + @"' 
                               GROUP BY SUBSTR(Date,1,8)
                               ORDER BY ��;";

            strSQL += @"SELECT SUM(����) AS ��
                         FROM (
                              SELECT MAX(SUBSTR(Date,7,2)) AS ��, SUM((SPrice * Quantity) - (BPrice * Quantity) - (SPrice * Quantity * 0.003)) AS ����
                                FROM Revenue
                               WHERE SUBSTR(Date,1,6) = '" + p_YearMonth + @"' 
                               GROUP BY SUBSTR(Date,1,8)
                              );";

            DataSet dsD = Network.GetDBSet(strSQL);

            dgvDate.DataSource = dsD.Tables[0];

            fnSortGrid(true, dgvDate, "��", "����");
            if (dsD.Tables[0].Rows.Count > 0)
            {
                dSum = Convert.ToDouble(dsD.Tables[1].Rows[0][0].ToString());
                txtSumD.Text = dSum.ToString("#,##");
            }
        }

        #endregion �����Լ�_End


        #region �̺�Ʈ���� �Լ�
        /// <summary>
        /// �޷� �̵�
        /// </summary>
        /// <param name="p_Num"></param>
        private void efnMoveMonth(int p_Num)
        {
            DateTime dteTemp = dteF.Value.AddMonths(p_Num);
            dteF.Value = new DateTime(dteTemp.Year, dteTemp.Month, 1, 0, 0, 0);
            dteS.Value = new DateTime(dteTemp.Year, dteTemp.Month, DateTime.DaysInMonth(dteTemp.Year, dteTemp.Month), 0, 0, 0);
        }

        #endregion �̺�Ʈ���� �Լ�


        #region �̺�Ʈ
        /// <summary>
        /// �˻���ư
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            fnReadDB();
        }

        /// <summary>
        /// �޷� ȭ��ǥ��ư
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBefore_Click(object sender, EventArgs e)
        {
            efnMoveMonth(-1);
        }

        /// <summary>
        /// �޷� ȭ��ǥ��ư
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAfter_Click(object sender, EventArgs e)
        {
            efnMoveMonth(1);
        }

        /// <summary>
        /// ��ü��¥ üũ ��ư
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
        /// txtbox���� ����ġ�� �˻�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
                fnReadDB();
        }

        /// <summary>
        /// â ũ�� ���� �� �÷� ũ�� �ڵ�����
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

        #endregion �̺�Ʈ_End

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                fnReadDBY();
                fnReadDBM(dgvYear["��", dgvYear.CurrentCell.RowIndex].Value.ToString());
                fnReadDBD(dgvYear["��", dgvYear.CurrentCell.RowIndex].Value.ToString() + dgvMonth["��", dgvMonth.CurrentCell.RowIndex].Value.ToString());
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
                fnReadDBM(dgvYear["��", dgvYear.CurrentCell.RowIndex].Value.ToString());
                fnReadDBD(dgvYear["��", dgvYear.CurrentCell.RowIndex].Value.ToString() + dgvMonth["��", dgvMonth.CurrentCell.RowIndex].Value.ToString());
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
                fnReadDBD(dgvYear["��", dgvYear.CurrentCell.RowIndex].Value.ToString() + dgvMonth["��", dgvMonth.CurrentCell.RowIndex].Value.ToString());
            }
            catch (NullReferenceException)
            {
                fnReadDBD("");
            }

        }



        ///////////////////////////////////////////////////////////////////////////



    }
}