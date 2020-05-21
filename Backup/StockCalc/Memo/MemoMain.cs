using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Security.AccessControl;
using System.IO;


/* ���̺� ����
 * 
SQLiteCommand sqlcmd;
 * 
strSQL = @"CREATE TABLE Memo (
                               'Seq' integer primary key,
                               'Date' text not null,
                               'MainCate' text not null,
                               'SmallCate' text,
                               'Title' text not null,
                               'Contents' text not null,
                               'File' text,
                               'Contents_Rtf' text,
                               'Color' Text,
                               'Valid' Text,
                               'ReportSeq' Text
                               'ReportType' Text,
                               )";
  
sqlcmd = new SQLiteCommand(strSQL, conn);
sqlcmd.ExecuteNonQuery();
sqlcmd.Dispose();
*/


/*
CREATE TABLE InitMemoOpt (
                               'chkDate' text not null,
                               'DateF' text not null,
                               'DateS' text not null,
                               'chkTitle' text not null,
                               'chkContents' text not null,
	                           'isCopy' text not null,
                               'chkWhite' text not null,
                               'chkGray' text not null,
                               'chkBlue' text not null,
                               'chkYellow' text not null,
                               'chkRed' text not null
                               )
*/


/*
strSQL = "INSERT INTO AfterService VALUES ('20180321','R','��û','�����Դϴ�','����','�����')";
sqlcmd = new SQLiteCommand(strSQL, conn);
sqlcmd.ExecuteNonQuery();
sqlcmd.Dispose();
*/



namespace StockCalc
{
    public partial class MemoMain : Form
    {
        #region ��������
        
        string strSQL = "";     //����

        string mSchQuery = "";  //�˻�����

        int mSelectedSeq;   //���õ� �ο�
        
        DataTable dt;             //��ü ���̺��� ��Ƶ� �����ͼ�
        
        DataTable dtCombo;     //�޺�����Ʈ 

        ContextMenu m = new ContextMenu();  //�˾�

        #endregion ��������_End


        #region ������

        public MemoMain()
        {
            InitializeComponent();
        }

        



        private void MemoMain_Load(object sender, EventArgs e)
        {
            //üũ�ڽ��� �ʱ�ȭ
            fnInitOpt();

            //����б�
            fnReadDB(dteF.Value.ToString("yyyyMMdd"), dteS.Value.ToString("yyyyMMdd"), "", "", "");

            //�޺��ڽ��ʱ�ȭ    //�˻���ư�� �������� �޺��ڽ��� �ʱ�ȭ��Ű�� �ʱ� ������, fnReadDB�� ���� ���� ����.
            fnComboReset();

            //�˾��޴� �ʱ�ȭ
            fnPopupInit();
        }

        #endregion ������_End

        #region �Լ�

        /// <summary>
        /// �˾� �ʱ�ȭ 
        ///  - ���߿� ����� ���� ���� �߰��� ���� ���� ������ ��.
        /// </summary>
        private void fnPopupInit()
        {
            m.MenuItems.Add(new MenuItem("���", Color_Click));
            m.MenuItems.Add(new MenuItem("ȸ��", Color_Click));
            m.MenuItems.Add(new MenuItem("�ϴ�", Color_Click));
            m.MenuItems.Add(new MenuItem("���", Color_Click));
            m.MenuItems.Add(new MenuItem("����", Color_Click));
        }

        /// <summary>
        /// üũ�ڽ� �ʱ�ȭ
        /// </summary>
        private void fnInitOpt()
        {
            DataTable dtOpt = new DataTable();
            strSQL = @" SELECT * 
                          FROM InitMemoOpt";

            dtOpt = Network.GetDBTable(strSQL);

            if (dtOpt.Rows.Count < 1)
            {
                strSQL = @"INSERT INTO InitMemoOpt (chkDate, DateF, DateS, chkTitle, chkWhite, chkGray, chkBlue, chkYellow, chkRed, chkContents, isCopy)
                           VALUES ('Y', '20000101', '20801231', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y')";

                Network.ExecDB(strSQL);

                strSQL = @" SELECT * 
                          FROM InitMemoOpt";
                dtOpt = Network.GetDBTable(strSQL);
            }

            if(dtOpt.Rows[0]["chkDate"].ToString().ToUpper().Equals("Y"))
            {
                chkDate.Checked = true;
            }
            else
            {
                chkDate.Checked = false;
            }
          
            dteF.Value = new DateTime(Convert.ToInt32(dtOpt.Rows[0]["DateF"].ToString().Substring(0,4))
                                    , Convert.ToInt32(dtOpt.Rows[0]["DateF"].ToString().Substring(4,2))
                                    , Convert.ToInt32(dtOpt.Rows[0]["DateF"].ToString().Substring(6,2)), 0, 0, 0);

            dteS.Value = new DateTime(Convert.ToInt32(dtOpt.Rows[0]["DateS"].ToString().Substring(0, 4))
                                    , Convert.ToInt32(dtOpt.Rows[0]["DateS"].ToString().Substring(4, 2))
                                    , Convert.ToInt32(dtOpt.Rows[0]["DateS"].ToString().Substring(6, 2)), 0, 0, 0);

            if (dtOpt.Rows[0]["chkTitle"].ToString().ToUpper().Equals("Y"))
            {
                chkTitle.Checked = true;
            }
            else
            {
                chkTitle.Checked = false;
            }

            if (dtOpt.Rows[0]["chkContents"].ToString().ToUpper().Equals("Y"))
            {
                chkContents.Checked = true;
            }
            else
            {
                chkContents.Checked = false;
            }

            if (dtOpt.Rows[0]["chkWhite"].ToString().ToUpper().Equals("Y"))
            {
                chkWhite.Checked = true;
            }
            else
            {
                chkWhite.Checked = false;
            }

            if (dtOpt.Rows[0]["chkGray"].ToString().ToUpper().Equals("Y"))
            {
                chkGray.Checked = true;
            }
            else
            {
                chkGray.Checked = false;
            }

            if (dtOpt.Rows[0]["chkBlue"].ToString().ToUpper().Equals("Y"))
            {
                chkBlue.Checked = true;
            }
            else
            {
                chkBlue.Checked = false;
            }
            if (dtOpt.Rows[0]["chkYellow"].ToString().ToUpper().Equals("Y"))
            {
                chkYellow.Checked = true;
            }
            else
            {
                chkYellow.Checked = false;
            }
            if (dtOpt.Rows[0]["chkRed"].ToString().ToUpper().Equals("Y"))
            {
                chkRed.Checked = true;
            }
            else
            {
                chkRed.Checked = false;
            }
        }

        /// <summary>
        ///  �޺��ʱ�ȭ    //�����Լ�
        /// </summary>
        private void fnComboReset()
        {
            string strCombMain = cboMain.Text;
            string strCombSmall = cboSmall.Text;

            strSQL = @"SELECT MainCate as '��з�', SmallCate as '�Һз�'
                         FROM Memo
                        WHERE Valid = 'Y'
                        GROUP BY SmallCate, MainCate";

            dtCombo = Network.GetDBTable(strSQL);

            cboMain.Items.Clear();
            cboSmall.Items.Clear();
            
            cboMain.Items.Add("<��ü>");
            

            //�޺��ڽ� ���ε�
            foreach (DataRow dr in dtCombo.Rows)
            {
                
                if(!cboMain.Items.Contains(dr["��з�"].ToString()))
                {
                    cboMain.Items.Add(dr["��з�"].ToString());
                }
                
            }

            //������ �޾� �� �޺��� �ʱ�ȭ
            cboMain.SelectedItem = strCombMain;
            cboSmall.SelectedItem = strCombSmall;

        }

        /// <summary>
        /// �׸��带 ����������.  -- ����
        /// �ܼ��˻� true, Resize false
        /// </summary>
        private void fnSortGrid(bool p_isSearch)
        {
            int intWidthSum = 0;

            if (p_isSearch)
            {

                foreach (DataGridViewColumn col in dataGridView1.Columns)
                {
                    col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                dataGridView1.Columns["��¥"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns["��з�"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns["�Һз�"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns["����"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns["Seq"].Visible = false;
                dataGridView1.Columns["FileDetail"].Visible = false;
                dataGridView1.Columns["Contents_Rtf"].Visible = false;
                dataGridView1.Columns["Color"].Visible = false;
                dataGridView1.Columns["reportSeq"].Visible = false;
                dataGridView1.Columns["reportType"].Visible = false;

                //MessageBox.Show(dataGridView1.RowTemplate.Height.ToString());
                //MessageBox.Show(dataGridView1.Height.ToString());
            }


            dataGridView1.Columns["��¥"].Width = 70;//Convert.ToInt32(dataGridView1.Size.Width * 0.08);//70;
            dataGridView1.Columns["��з�"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;//.Width = 90;//Convert.ToInt32(dataGridView1.Size.Width * 0.12);///70;
            dataGridView1.Columns["�Һз�"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; //.Width = 100;//Convert.ToInt32(dataGridView1.Size.Width * 0.13);///100;
            dataGridView1.Columns["����"].Width = 45; //Convert.ToInt32(dataGridView1.Size.Width * 0.07);///40;

            intWidthSum = dataGridView1.Columns["��¥"].Width
                          + dataGridView1.Columns["��з�"].Width
                          + dataGridView1.Columns["�Һз�"].Width
                          + 45;

            dataGridView1.Columns["����"].Width = dataGridView1.Size.Width > intWidthSum ? Convert.ToInt32((dataGridView1.Size.Width - intWidthSum) * 0.6 - 10) : 0;
            dataGridView1.Columns["����"].Width = dataGridView1.Size.Width > intWidthSum ? Convert.ToInt32((dataGridView1.Size.Width - intWidthSum) * 0.4 - 10) : 0;



            if (dataGridView1.Height -20 - (dataGridView1.RowTemplate.Height * dataGridView1.RowCount) > 0)
            {
                //��ũ�ѹ� �� ����� ���
                dataGridView1.Columns["����"].Width = 65;
                //MessageBox.Show(dataGridView1.RowTemplate.Height.ToString() + ", " + dataGridView1.Height.ToString() + ", " + dataGridView1.RowCount + ", " + (dataGridView1.RowTemplate.Height * dataGridView1.RowCount));
            }
            else
            {
                //��ũ�ѹ� ����� ���
                dataGridView1.Columns["����"].Width = 45;
            }
        }

        /// <summary>
        /// ��Ŀ���ֱ�  �����Լ�
        /// -99�� ��� ���� ���� ��Ŀ��
        /// �ٸ� ���ڴ� ����Ű�� �ο�
        /// </summary>
        /// <param name="p_RowNum"></param>
        private void fnFocusGrid(int p_SelectedRow)
        {
            if (dataGridView1.Rows.Count != 0)
            {
                int intSelectedRow = p_SelectedRow;

                //���õǾ��� �ο� ���� �� ū ��� ���� ������ ��Ŀ��.  
                if (dataGridView1.Rows.Count < p_SelectedRow || p_SelectedRow == -99)
                {
                    intSelectedRow = dataGridView1.Rows.Count - 1;
                }

                dataGridView1.Rows[intSelectedRow].Selected = true;
                dataGridView1.CurrentCell = dataGridView1.Rows[intSelectedRow].Cells["��¥"];
                dataGridView1.BeginEdit(true);
            }
        }


        private void fnPaint()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1["Color", i].Value.ToString() == "���")
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                }

                else if (dataGridView1["Color", i].Value.ToString() == "����")
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                }

                else if (dataGridView1["Color", i].Value.ToString() == "�ϴ�")
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Cyan;
                }

                else if (dataGridView1["Color", i].Value.ToString() == "ȸ��")
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
                }

                if (dataGridView1["reportType", i].Value.ToString() == "Buy")
                {
                    DataGridViewCellStyle style = new DataGridViewCellStyle();
                    style.BackColor = Color.Bisque;
                    dataGridView1.Rows[i].Cells["�Һз�"].Style = style;
                }
                else if (dataGridView1["reportType", i].Value.ToString() == "Sell")
                {
                    DataGridViewCellStyle style = new DataGridViewCellStyle();
                    style.BackColor = Color.LightCyan;
                    dataGridView1.Rows[i].Cells["�Һз�"].Style = style;
                }
            }
        }

        /// <summary>
        /// ����ȸ
        ///  - ��ũ�� ����
        ///  - ��˻��� ����, ó�������ϴ� �ε����ο�
        /// </summary>
        /// <param name="p_Query"></param>
        /// <param name="p_scrIndex"></param>
        private void fnReadDB(string p_Query, int p_scrIndex)
        {
            if (p_Query != "")
            {
                dt = Network.GetDBTable(mSchQuery);
                dataGridView1.DataSource = dt;
            }

            fnSortGrid(true);
            if (p_scrIndex > 0)
            {
                dataGridView1.FirstDisplayedScrollingRowIndex = p_scrIndex;
            }

            //�׸��� ��ĥ
            fnPaint();
        }

        /// <summary>
        /// DB SELECT �� �� �ʱ�ȭ
        /// </summary>
        /// <param name="p_isAll"></param>
        /// <param name="p_DateF"></param>
        /// <param name="p_DateS"></param>
        /// <param name="p_MainCate"></param>
        /// <param name="p_SmallCate"></param>
        /// <param name="p_Title"></param>
        /// <param name="p_Contents"></param>
        private void fnReadDB(string p_DateF, string p_DateS, string p_MainCate, string p_SmallCate, string p_Search)
        {
            
            mSchQuery = @" SELECT Seq as 'Seq'
                               , Date as '��¥'
                               , MainCate as '��з�'
                               , SmallCate as '�Һз�'
                               , REPLACE(Title,'��','''') as '����'
                               , REPLACE(Contents, '��','''') as '����'
                               , CASE WHEN File != ''
                                      THEN 'Y'
                                      ELSE '' END as '����'  
                               , File as 'FileDetail'
                               , Contents_Rtf as 'Contents_Rtf'
                               , Color as 'Color'
                               , reportSeq as 'reportSeq'
                               , reportType as 'reportType'
                          FROM Memo
                         WHERE Title != '' 
                           AND Valid = 'Y'";

            //if (p_SelectedRow != -99)
            //{
                mSchQuery += !chkDate.Checked ? " AND Date >= " + p_DateF + " AND Date <= " + p_DateS : "";
                if (cboMain.Text != "")
                {
                    mSchQuery += (p_MainCate.Equals("<��ü>") ? "" : " AND MainCate = '" + p_MainCate + "'");
                }
                if (cboSmall.Text != "")
                {
                    mSchQuery += " AND SmallCate = '" + p_SmallCate + "'" ;
                }
                if (chkWhite.Checked || chkGray.Checked || chkBlue.Checked || chkYellow.Checked || chkRed.Checked)
                {
                    mSchQuery += @" AND Color IN ("
                                                    + (chkWhite.Checked ? "''," : "")
                                                    + (chkGray.Checked ? "'ȸ��'," : "")
                                                    + (chkBlue.Checked ? "'�ϴ�'," : "")
                                                    + (chkYellow.Checked ? "'���'," : "")
                                                    + (chkRed.Checked ? "'����'," : ""); 

                    
                    mSchQuery = mSchQuery.Substring(0, mSchQuery.Length - 1);
                    mSchQuery += ")";
                }

                if (txtSearch.Text != "")
                {
                    mSchQuery += chkTitle.Checked && !chkContents.Checked ? " AND Title LIKE '%" + p_Search.Replace("'", "��") + "%'" : "";
                    mSchQuery += !chkTitle.Checked && chkContents.Checked ? " AND Contents LIKE '%" + p_Search.Replace("'", "��") + "%'" : "";
                    mSchQuery += chkTitle.Checked && chkContents.Checked ? " AND (Title LIKE '%" + p_Search.Replace("'", "��") + "%'" + " OR Contents LIKE '%" + p_Search.Replace("'", "��") + "%'" + ")" : "";
                }
            //}
                    /*
            else
            {
                mSchQuery += " AND Date >= " + p_DateF + " AND Date <= " + p_DateS;
            }
                     * */
            mSchQuery += " ORDER BY Date";

            dt = Network.GetDBTable(mSchQuery);

            dataGridView1.DataSource = dt;


            //���� ���� ��Ŀ��
            fnFocusGrid(-99);

            //�׸��� �÷� ����
            fnSortGrid(true);

            //�׸��� ��ĥ
            fnPaint();

        }
        
        /// <summary>
        /// �޷� �̵�
        /// </summary>
        /// <param name="p_Num"></param>
        private void fnMoveMonth(int p_Num)
        {
            DateTime dteTemp = dteF.Value.AddMonths(p_Num);
            dteF.Value = new DateTime(dteTemp.Year, dteTemp.Month, 1, 0, 0, 0);
            dteS.Value = new DateTime(dteTemp.Year, dteTemp.Month, DateTime.DaysInMonth(dteTemp.Year, dteTemp.Month), 0, 0, 0);
        }

        /// <summary>
        /// ������ �� �� ����
        /// </summary>
        private void fnSelectedOpen()
        {
            Dictionary<string, string> dicSelected = fnSelectedDic();
            EnrollForm EF = new EnrollForm(dicSelected);
            EF.StartPosition = FormStartPosition.CenterParent;


            if (EF.ShowDialog() == DialogResult.Cancel)
            {
            }

        }

        /// <summary>
        /// �����Լ� - ���õ� �ο��� ������ ���ϱ�
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> fnSelectedDic()
        {
            //��¥ ��з� �Һз� ���� ���� ���� reportSeq
            int selectedRow = dataGridView1.SelectedRows[0].Index;

            Dictionary<string, string> dicSelected = new Dictionary<string, string>();

            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                dicSelected.Add(dataGridView1.Columns[i].Name, dataGridView1[i, selectedRow].Value.ToString());

            }

            return dicSelected;
        }

        /// <summary>
        /// �ο� �����
        /// </summary>
        private void fnDel()
        {
            Dictionary<string, string> dicSelected = fnSelectedDic();
           
            //strSQL = @"DELETE FROM Memo WHERE Seq = " + dicSelected["Seq"];
            strSQL = @"
                       UPDATE Memo
                          SET Valid = 'N'
                        WHERE Seq = " + dicSelected["Seq"];


            if (Network.ExecDB(strSQL) > 0)
            {

                string strFilePath = @".\FileList\" + dicSelected["��¥"] + @"\";
                string[] strTemp;
                if(Directory.Exists(strFilePath) && dicSelected["FileDetail"] != "")
                {
                    //�����ֱ�
                    DirectorySecurity dSecurity = Directory.GetAccessControl(strFilePath);
                    dSecurity.AddAccessRule(new FileSystemAccessRule(System.Environment.UserDomainName + "\\" + System.Environment.UserName, FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                    Directory.SetAccessControl(strFilePath, dSecurity);

                    strTemp = dicSelected["FileDetail"].Split('��');

                   foreach(string a in strTemp)
                   {
                       if(a != "")
                       {
                           File.Delete(strFilePath + a);
                       }
                   }

                    //���丮�� �ƹ��͵� ������ ���丮 ������
                   DirectoryInfo DirInfo = new DirectoryInfo(strFilePath);
                   if (DirInfo.GetFiles().Length < 1)
                       DirInfo.Delete();

                }

                MessageBox.Show("���� �Ϸ�");
            }
        }

        /// <summary>
        /// �ű� ��ư
        /// </summary>
        private void fnNew()
        {
            DataTable dtt = new DataTable();

            strSQL = @"SELECT CASE WHEN Max(Seq) IS NULL THEN 0 ELSE Max(Seq) END AS 'Seq'
                         FROM Memo";
            dtt = Network.GetDBTable(strSQL);


            EnrollForm EF = new EnrollForm();
            EF.StartPosition = FormStartPosition.CenterParent;

            if (EF.ShowDialog() == DialogResult.Cancel)
            {
            }
        }

        /// <summary>
        /// ���� üũ�ڽ� �̺�Ʈ ����
        /// </summary>
        /// <param name="p_chk"></param>
        private void chkColorChanger(CheckBox p_chk)
        {
            if (!p_chk.Checked && chkTotal.Checked)
            {
                chkTotal.CheckedChanged -= chkTotal_CheckedChanged;
                chkTotal.Checked = false;
                chkTotal.CheckedChanged += chkTotal_CheckedChanged;
            }
            else if (chkWhite.Checked && chkGray.Checked && chkBlue.Checked && chkYellow.Checked && chkRed.Checked)
            {
                chkTotal.Checked = true;
            }
        }


        #endregion �Լ�_End

        #region �̺�Ʈ����

        private void btnBefore_Click(object sender, EventArgs e)
        {
            fnMoveMonth(-1);
        }

        private void btnAfter_Click(object sender, EventArgs e)
        {
            fnMoveMonth(1);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            fnReadDB(dteF.Value.ToString("yyyyMMdd"), dteS.Value.ToString("yyyyMMdd"), cboMain.Text, cboSmall.Text, txtSearch.Text);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            fnNew();
            //fnReadDB(dteF.Value.ToString("yyyyMMdd"), dteS.Value.ToString("yyyyMMdd"), mTempMainComb, mTempSmallComb, txtSearch.Text, dataGridView1.Rows.Count);
            fnReadDB(mSchQuery, dataGridView1.FirstDisplayedScrollingRowIndex);
            fnComboReset();
        }

        private void btnModi_Click(object sender, EventArgs e)
        {
            //int intSelected = dataGridView1.CurrentCell.RowIndex;
            fnSelectedOpen();
            //fnReadDB(dteF.Value.ToString("yyyyMMdd"), dteS.Value.ToString("yyyyMMdd"), mTempMainComb, mTempSmallComb, txtSearch.Text, intSelected);
            fnReadDB(mSchQuery, dataGridView1.FirstDisplayedScrollingRowIndex);
            fnComboReset();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            //int intSelected = dataGridView1.CurrentCell.RowIndex;
            fnDel();
            //fnReadDB(dteF.Value.ToString("yyyyMMdd"), dteS.Value.ToString("yyyyMMdd"), cboMain.Text, cboSmall.Text, txtSearch.Text, intSelected - 1);
            fnReadDB(mSchQuery, dataGridView1.FirstDisplayedScrollingRowIndex);
            fnComboReset();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int intSelected = dataGridView1.CurrentCell.RowIndex;
            fnSelectedOpen();
            //fnReadDB(dteF.Value.ToString("yyyyMMdd"), dteS.Value.ToString("yyyyMMdd"), cboMain.Text, cboSmall.Text, txtSearch.Text, intSelected);
            fnReadDB(mSchQuery, dataGridView1.FirstDisplayedScrollingRowIndex);
            fnComboReset();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MemoMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            string strDate = chkDate.Checked ? "Y" : "N";
            string strTitle = chkTitle.Checked ? "Y" : "N";
            string strContents = chkContents.Checked ? "Y" : "N";
            string strWhite = chkWhite.Checked ? "Y" : "N";
            string strGray = chkGray.Checked ? "Y" : "N";
            string strBlue = chkBlue.Checked ? "Y" : "N";
            string strYellow = chkYellow.Checked ? "Y" : "N";
            string strRed = chkRed.Checked ? "Y" : "N";

            strSQL = @"UPDATE InitMemoOpt
                          SET chkDate = '" + strDate + @"'
                             , DateF = '" + dteF.Value.ToString("yyyyMMdd") + @"'
                             , DateS = '" + dteS.Value.ToString("yyyyMMdd") + @"'
                             , chkTitle = '" + strTitle + @"'
                             , chkWhite = '" + strWhite + @"'
                             , chkGray = '" + strGray + @"'
                             , chkBlue = '" + strBlue + @"'
                             , chkYellow = '" + strYellow + @"'
                             , chkRed = '" + strRed + @"'
                             , chkContents = '" + strContents + @"'";

            Network.ExecDB(strSQL);

        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                fnReadDB(dteF.Value.ToString("yyyyMMdd"), dteS.Value.ToString("yyyyMMdd"), cboMain.Text, cboSmall.Text, txtSearch.Text);
                fnComboReset();
            }
        }

        private void cboMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboSmall.Items.Clear();
            cboSmall.Items.Add("");
            if (cboMain.Text.Equals("<��ü>"))
            {
                foreach (DataRow dr in dtCombo.Rows)
                {
                    if (!cboSmall.Items.Contains(dr["�Һз�"].ToString()))
                    {
                        cboSmall.Items.Add(dr["�Һз�"].ToString());
                    }
                }
            }


            foreach (DataRow dr in dtCombo.Rows)
            {
                if (dr["��з�"].ToString().Equals(cboMain.Text) && !cboSmall.Items.Contains(dr["�Һз�"].ToString()))
                {
                    cboSmall.Items.Add(dr["�Һз�"].ToString());
                }
            }
        }



        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (!base.ProcessCmdKey(ref msg, keyData)) // ������ ó�� ��������
            {
                // ���⿡ ó���ڵ带 �ִ´�.
                if (keyData.Equals(Keys.F1))
                {
                    fnNew();
                    //fnReadDB(dteF.Value.ToString("yyyyMMdd"), dteS.Value.ToString("yyyyMMdd"), cboMain.Text, cboSmall.Text, txtSearch.Text, dataGridView1.Rows.Count-1);
                    fnReadDB(mSchQuery, dataGridView1.FirstDisplayedScrollingRowIndex);
                    fnComboReset();
                    return true;
                }

                else if (keyData.Equals(Keys.F5))
                {
                    fnReadDB(dteF.Value.ToString("yyyyMMdd"), dteS.Value.ToString("yyyyMMdd"), cboMain.Text, cboSmall.Text, txtSearch.Text);
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

        private void Form1_Resize(object sender, EventArgs e)
        {
            try
            {
                fnSortGrid(false);
            }
            catch
            { }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text != "")
            {
                if (chkContents.Checked && chkTitle.Checked)
                { }
                else if (!chkContents.Checked)
                {
                    chkTitle.Checked = true;
                }
            }
        }


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

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {


                int currentMouseOverRow = dataGridView1.HitTest(e.X, e.Y).RowIndex;
                

                if (currentMouseOverRow >= 0)
                {

                    fnFocusGrid(currentMouseOverRow);

                    mSelectedSeq = Convert.ToInt32(dataGridView1["Seq", currentMouseOverRow].Value); 
                    //�ش� �÷��� ���� ĥ�ϱ� ���� ���������� Seq�� ����
                    //�̺�Ʈ�� �Ķ���͸� �ѱ� �� ������ ���� �������� �� �ᵵ �Ǵµ�...

                    m.Show(dataGridView1, new Point(e.X, e.Y));



                    //m.MenuItems.Add(new MenuItem(string.Format("Do something to row {0}", currentMouseOverRow.ToString())));
                }


            }
        }

        /// <summary>
        /// �����̺�Ʈ - �˾� Ŭ�� �� �� ���ϱ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Color_Click(object sender, EventArgs e)
        {
            string strChoiceColor = sender.ToString().Split(',')[2].Replace("Text: ", "").Replace(" ","");

            strChoiceColor = strChoiceColor == "���" ? "" : strChoiceColor;

            strSQL = @"UPDATE Memo
                          SET Color = '" + strChoiceColor + @"'
                        WHERE Seq = '" + mSelectedSeq + "'";

            Network.ExecDB(strSQL);

            fnReadDB(mSchQuery, dataGridView1.FirstDisplayedScrollingRowIndex);

        }


        ///���� üũ�ڽ�
        private void chkTotal_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTotal.Checked)
            {
                chkWhite.Checked = true;
                chkGray.Checked = true;
                chkBlue.Checked = true;
                chkYellow.Checked = true;
                chkRed.Checked = true;
            }
            else
            {
                chkWhite.Checked = false;
                chkGray.Checked = false;
                chkBlue.Checked = false;
                chkYellow.Checked = false;
                chkRed.Checked = false;
            }
        }

        private void chkWhite_CheckedChanged(object sender, EventArgs e)
        {
            chkColorChanger(chkWhite);
        }

        private void chkGray_CheckedChanged(object sender, EventArgs e)
        {
            chkColorChanger(chkGray);
        }

        private void chkBlue_CheckedChanged(object sender, EventArgs e)
        {
            chkColorChanger(chkBlue);
        }

        private void chkYellow_CheckedChanged(object sender, EventArgs e)
        {
            chkColorChanger(chkYellow);
        }

        private void chkRed_CheckedChanged(object sender, EventArgs e)
        {
            chkColorChanger(chkRed);
        }

        //���� üũ�ڽ�_End

        #endregion �̺�Ʈ����_End














    }
}