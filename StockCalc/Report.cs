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
        #region ��������



        //�о�� �ڷ��
        DataSet mds;
        #endregion ��������_End

        #region ������ 
        public Report()
        {
            InitializeComponent();
        }

        //�׸��� ����Ŭ������ ������ ���
        public Report(string p_Name)
        {

            InitializeComponent();

            txtName.Text = p_Name;

        }


        private void Report_Load(object sender, EventArgs e)
        {
            chkDate.Checked = true;

            //�޺��ʱ�ȭ
            fnInitComb();

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
                if (dataGridView1["����", i].Value.ToString() == "�ż�")
                {
                    if (Convert.ToInt32(dataGridView1["�ܷ�", i].Value.ToString()) > 0)
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightPink;
                    else
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
                }
                if (dataGridView1["����", i].Value.ToString() == "�ŵ�")
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightYellow;
                }
            }
        }

        /// <summary>
        /// �޺��ڽ� �ʱ�ȭ
        /// </summary>
        private void fnInitComb()
        {
            cboType.Items.Clear();

            cboType.Items.Add("<��ü>");
            cboType.Items.Add("�ż�");
            cboType.Items.Add("�ŵ�");

            cboType.SelectedItem = "<��ü>";
        }
        /// <summary>
        /// ����б�
        /// </summary>
        private void fnReadDB()
        {
            //��񿬰�

            string strSQL = "";
            mds = new DataSet();
            strSQL = @"SELECT b.Date AS ��¥
                              , '�ż�' AS ����
                              , b.Name AS �����
                              , b.Quantity AS ����
                              , b.Left AS �ܷ�
                              , b.Price AS ����
                              , b.Quantity * Price AS �հ�
                              , CASE WHEN IFNULL(m.ReportSeq,'') = ''
                                     THEN ''
                                     ELSE 'Y' END AS �޸�
                              , b.Seq
                              , m.Seq AS memSeq
                         FROM Buy b
                         LEFT JOIN Memo m ON b.Seq = m.ReportSeq AND m.ReportType = 'Buy' AND m.Valid = 'Y'
                        WHERE b.Name LIKE '%%'"; // �̰� �� �̷��� �؇J��??
            if (cboType.Text == "�ŵ�")
            {
                strSQL += " AND Name = '!#@$%#^&#%^#'";     //�ż��� ��츸 �˻��� �ǵ��� ���̰��� ����
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
                       SELECT s.Date AS ��¥
                              , '�ŵ�' AS ����
                              , s.Name AS �����
                              , s.Quantity AS ����
                              , ''
                              , s.Price AS ����
                              , s.Quantity * Price AS �հ�
                              , CASE WHEN IFNULL(m.ReportSeq,'') = ''
                                     THEN ''
                                     ELSE 'Y' END AS �޸�
                              , s.Seq
                              , m.Seq AS memSeq
                         FROM Sell s
                         LEFT JOIN Memo m ON s.Seq = m.ReportSeq AND m.ReportType = 'Sell' AND m.Valid = 'Y'
                        WHERE s.Name LIKE '%%'";
            if (cboType.Text == "�ż�")
            {
                strSQL += " AND Name = '!#@$%#^&#%^#'";     //�ŵ��� ��츸 �˻��� �ǵ��� 
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
                       ORDER BY ��¥
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

            //�׸��� �ʱ�ȭ
            fnSortGrid(true); 

            //����ĥ
            fnPaint();
        }

        /// <summary>
        /// �׸����ʱ�ȭ
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
                dataGridView1.Columns["��¥"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns["����"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns["�����"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns["����"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns["����"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns["�հ�"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns["�ܷ�"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns["�޸�"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dataGridView1.Columns["����"].DefaultCellStyle.Format = "#,##";
                dataGridView1.Columns["����"].DefaultCellStyle.Format = "#,##";
                dataGridView1.Columns["�հ�"].DefaultCellStyle.Format = "#,##";
                dataGridView1.Columns["�ܷ�"].DefaultCellStyle.Format = "#,##";

                //��Ŀ�� 
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Selected = true;
                    dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["�����"];
                    dataGridView1.BeginEdit(true);
                }
            }

            dataGridView1.Columns["��¥"].Width = 70;
            dataGridView1.Columns["����"].Width = 50;
            dataGridView1.Columns["����"].Width = 70;
            dataGridView1.Columns["����"].Width = 70;
            dataGridView1.Columns["�հ�"].Width = 70;
            dataGridView1.Columns["�ܷ�"].Width = 70;
            dataGridView1.Columns["�޸�"].Width = 40;
            //dataGridView1.Columns["�ܷ�"].Visible = false;
            dataGridView1.Columns["memSeq"].Visible = false;
            dataGridView1.Columns["Seq"].Visible = false;

            if (dataGridView1.Height - 20 - (dataGridView1.RowTemplate.Height * dataGridView1.RowCount) > 0)
            {
                //��ũ�ѹ� �� ����� ���
                dataGridView1.Columns["�����"].Width = dataGridView1.Width - 443;
                //MessageBox.Show(dataGridView1.RowTemplate.Height.ToString() + ", " + dataGridView1.Height.ToString() + ", " + dataGridView1.RowCount + ", " + (dataGridView1.RowTemplate.Height * dataGridView1.RowCount));
            }
            else
            {
                //��ũ�ѹ� ����� ���
                dataGridView1.Columns["�����"].Width = dataGridView1.Width - 463;
            }
        }

        /// <summary>
        /// ������ ��Ŀ�� �ֱ�
        /// </summary>
        /// <param name="p_scrIndex"></param>
        /// <param name="p_scrFocus"></param>
        private void fnFocus(int p_scrIndex, int p_scrFocus)
        {
            //��Ŀ�� 
            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows[p_scrFocus].Selected = true;
                dataGridView1.CurrentCell = dataGridView1.Rows[p_scrFocus].Cells[0];
                dataGridView1.BeginEdit(true);
            }
            
            //��ũ�� ��ġ
            if (p_scrIndex != 0)
            {
                dataGridView1.FirstDisplayedScrollingRowIndex = p_scrIndex;
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

        /// <summary>
        /// ���� ��ư ���� 
        /// </summary>
        private void efnDel()
        {
            //��񿬰�

            string strSQL = "";
            string strName = dataGridView1.CurrentRow.Cells["�����"].Value.ToString(); //���õ� �̸�
            DataRow[] drr = mds.Tables[0].Select("���� = '�ŵ�' AND ����� = '" + strName + "' AND �ܷ� <> ����", "Seq DESC");

            if (dataGridView1.CurrentRow.Cells["����"].Value.ToString() == "�ż�")
            {
                if (dataGridView1.CurrentRow.Cells["�ܷ�"].Value.ToString() != dataGridView1.CurrentRow.Cells["����"].Value.ToString())
                {
                    MessageBox.Show("�ش� ������ ���� �ֱ� �ŵ��� ���� ����ʽÿ�.\r\n(��¥ : " + drr[0]["��¥"].ToString() + ", ���� : " + drr[0]["����"].ToString() + ", ���� : " + drr[0]["����"].ToString() + ")");
                    return;
                }
                strSQL = @"
                           DELETE FROM Buy
                            WHERE Seq = '" + dataGridView1.CurrentRow.Cells["Seq"].Value.ToString() + @"';
                          ";
                int iresult = Network.ExecDB(strSQL);
                if (iresult > 0)
                {
                    MessageBox.Show("��������");
                }
                else
                {
                    MessageBox.Show("��������");
                }

            }

            else if (dataGridView1.CurrentRow.Cells["����"].Value.ToString() == "�ŵ�")
            {
                string strSellSeq;// = mds.Tables[1].Rows[0][0].ToString();    //���õ� Seq

                int i = 0; //�ݺ��� ����

                int intSellQ = Convert.ToInt32(dataGridView1.CurrentRow.Cells["����"].Value.ToString());    //�Ǹ��ϰ��� �ϴ� ���� 
                int intQ;   //buy�� ���� ����

                DataRow[] dr = mds.Tables[0].Select("���� = '�ż�' AND ����� = '" + strName + "' AND �ܷ� <> ����", "Seq DESC");


                strSellSeq = drr[0]["Seq"].ToString();

                if (dataGridView1.CurrentRow.Cells["Seq"].Value.ToString() != strSellSeq)
                {
                    MessageBox.Show("�ش� ������ ���� �ֱ� �ŵ��� ���� ����ʽÿ�.\r\n(��¥ : " + drr[0]["��¥"].ToString() + ", ���� : " + drr[0]["����"].ToString() + ", ���� : " + drr[0]["����"].ToString() + ")");
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
                        intQ = Convert.ToInt32(dr[i]["����"].ToString()) - Convert.ToInt32(dr[i]["�ܷ�"].ToString());

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
                                           SET Left = " + (Convert.ToInt32(dr[i]["�ܷ�"].ToString()) + intSellQ) + @"
                                         WHERE Seq = '" + dr[i]["Seq"].ToString() + @"';
                                       ";
                            intSellQ = 0;
                        }

                        i++;
                    }


                    int iresult = Network.ExecDB(strSQL);
                    if (iresult > 0)
                    {
                        MessageBox.Show("���� ����");
                    }
                    else
                    {
                        MessageBox.Show("��������");
                    }
                }
            }
            fnReadDB();
            
        }

        /// <summary>
        /// �޸� ���� 
        /// </summary>
        private void efnMemo()
        {
            /*
            string strType = dataGridView1.CurrentRow.Cells["����"].Value.ToString();
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
        #endregion �̺�Ʈ���� �Լ�


        #region �̺�Ʈ
        /// <summary>
        /// �޸��ư
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMemo_Click(object sender, EventArgs e)
        {
            efnMemo();
        }

        /// <summary>
        /// ������ư
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            efnDel();
        }


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
        /// �޺� �б��������� �����
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

        /// <summary>
        /// ����Ŭ�� �� �޸�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string reportSeq = "";
            string memSeq = "";
            string reportType = "";

            reportType = dataGridView1.CurrentRow.Cells["����"].Value.ToString() == "�ż�" ? "Buy"
                         : dataGridView1.CurrentRow.Cells["����"].Value.ToString() == "�ŵ�" ? "Sell" : "";
            reportSeq = dataGridView1.CurrentRow.Cells["Seq"].Value.ToString();
            memSeq = dataGridView1.CurrentRow.Cells["memSeq"].Value.ToString();
           
            efnMemo(reportType, reportSeq, memSeq);
        }
        #endregion �̺�Ʈ_End



    }
}