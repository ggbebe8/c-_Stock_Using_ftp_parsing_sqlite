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
    public partial class ViewStock : Form
    {
        #region **********��������**********
        string mInterListName = "";

        // ��Ŭ���˾�
        ContextMenu m = new ContextMenu();
        #endregion 

        //���� vs ���� Ŭ���� ���� �÷��� 
        private enum enViewFlag
        {
            Hold, Interest
        }

        //�ǳ�3�� �����ִ� �� �ȿ����ִ� �� 
        bool mISpanelVible = true;  

        #region **********������**********
        #endregion ������_End
        //������
        public ViewStock()
        {
            InitializeComponent();
            fnInitComb();
            //���� �޸� ���ε�
            mfnMainMemoBinder();
        }

        //Load�̺�Ʈ
        private void Hold_Load(object sender, EventArgs e)
        {
            string strFirstScreen = Network.ReadIniFile("OPTION", "FirstScreen", @".\setting.ini");
            if (strFirstScreen == "C")
                labHold_Click(null, null);
            else if (strFirstScreen == "I")
                labInterest_Click(null, null);
            else
                labMainMemo_Click(null, null);

            fnPopupInit();
        }

        #region **********�ܺγ����Լ�**********
        #endregion

        public void mfnReLoad()
        {

            //���� ��Ŀ���� �����ؾ� �ϹǷ� row�Ӹ� �ƴ϶� col�� ���ؾ� ��. 

            int iSelectedRow = dgvView.CurrentCell == null ? -1 : dgvView.CurrentCell.RowIndex;

            int iSelectedCol = dgvView.CurrentCell == null ? -1 : dgvView.CurrentCell.ColumnIndex;

            // ���ɸ���� �߰��� �صΰ� ������ �߰� ������ ���, �̰Ÿ� Ÿ�ԵǸ� �߰��� ���ɸ���� ������� ���µǹǷ� �̰� �߰�.
            try
            {
                if (labHold.ForeColor == Color.Black)
                {
                    fnHoldSearch();
                }
                else if (labInterest.ForeColor == Color.Black)
                {
                    //�޺����ε�
                    fnCboBingding();
                    fnInterestSearch();
                }

            }
            catch
            {
            }
            try
            {
                if (dgvView.Rows.Count > 0 && iSelectedRow >= 0 && iSelectedCol >= 0)
                {
                    dgvView.Rows[iSelectedRow].Selected = true;
                    dgvView.CurrentCell = dgvView[iSelectedCol, iSelectedRow];
                }
            }
            catch
            { }


        }

        #region **********�����Լ�**********
        #endregion
        //�޺����ε�-���
        private void fnInitComb()
        {
            string strQuery = "";
            strQuery += "\r\n" + "SELECT Company FROM Code";
            DataTable dt = Network.GetDBTable(strQuery);

            foreach (DataRow dr in dt.Rows)
            {
                cboStoSearch.Items.Add(dr["Company"].ToString());
            }

        }

        //�˾��ʱ�ȭ
        private void fnPopupInit()
        {
            m.MenuItems.Add(new MenuItem("������", popUpItem_Click));
            m.MenuItems.Add(new MenuItem("���ô���", popUpNews_Click));
            m.MenuItems.Add(new MenuItem("����Ʈ", popUpReport_Click));
            m.MenuItems.Add(new MenuItem("���ܸ޸�", popUpSimpleMemo_Click));
        }

        //���纸����
        /// <summary>
        /// ���纸���� SELECT
        /// </summary>
        private void fnHoldSearch()
        {
            string strSQL = "";

            strSQL = @"SELECT MAX(Name) AS �����
                            , SUM(Left * Price) / SUM(Left) AS ��հ�
                            , SUM(Left) AS ���� 
                            , SUM(Left * Price) AS �հ�
                            , MAX(CodeNum) AS CodeNum  
                         FROM Buy
                         LEFT JOIN Code ON Name = Company
                        WHERE Left > 0
                        GROUP BY Name;";


            DataTable dtHold = Network.GetDBTable(strSQL);

            //dgvView.DataSource = dtHold;

            //dgvView.Columns["CodeNum"].Visible = false;

            dgvView.Columns.Clear();
            dgvView.Rows.Clear();

            //�÷��߰�
            fnColumnAdd(enViewFlag.Hold, dtHold);

            // �÷�����
            fnSortGrid(enViewFlag.Hold, true);

            //�÷��� �� �ֱ�
            fnInsertValue(enViewFlag.Hold, dtHold);
        }

        // ���� �б�
        /// <summary>
        /// DB�������� �б�
        /// </summary>
        private void fnInterestSearch()
        {
            mInterListName = cboInter.Text;

            string strSQL = "";

            strSQL = @"SELECT Company AS �����
                             ,CodeNum
                             ,DisSeq
                         FROM Interest
                        WHERE IFNULL(InterName,'') = '" + cboInter.Text + @"'
                        ORDER BY DisSeq";

            DataTable dtInterest = Network.GetDBTable(strSQL);

            dgvView.Columns.Clear();
            dgvView.Rows.Clear();

            //�÷��߰�
            fnColumnAdd(enViewFlag.Interest, dtInterest);
            
            //�÷� ����
            fnSortGrid(enViewFlag.Interest, true);

            //�÷��� �� �ֱ�
            fnInsertValue(enViewFlag.Interest, dtInterest);

        }

        //�÷��߰�
        /// <summary>
        /// ���簡�� �߰�����. (Time�̺�Ʈ���� �ֱ� ���� �Ϻη� ��)
        /// </summary>
        /// <param name="p_strViewFlag">Hold or Interest</param>
        private void fnColumnAdd(enViewFlag p_ViewFlag, DataTable p_dt)
        {
            if (!dgvView.Columns.Contains("�����"))
            {
                dgvView.Columns.Add("�����", "�����");
            }

            if (!dgvView.Columns.Contains("���簡"))
            {
                dgvView.Columns.Add("���簡", "���簡");
            }

            if (!dgvView.Columns.Contains("���ϰ�"))
            {
                dgvView.Columns.Add("���ϰ�", "���ϰ�");
            }

            if (!dgvView.Columns.Contains("��������"))
            {
                dgvView.Columns.Add("��������", "��������");
            }

            if (!dgvView.Columns.Contains("����%"))
            {
                dgvView.Columns.Add("����%", "����%");
            }

            if (!dgvView.Columns.Contains("�ŷ���"))
            {
                dgvView.Columns.Add("�ŷ���", "�ŷ���");
            }

            if (!dgvView.Columns.Contains("CodeNum"))
            {
                dgvView.Columns.Add("CodeNum", "CodeNum");
            }

            if (p_ViewFlag == enViewFlag.Hold)
            {

                if (!dgvView.Columns.Contains("��հ�"))
                {
                    dgvView.Columns.Add("��հ�", "��հ�");
                }

                if (!dgvView.Columns.Contains("����"))
                {
                    dgvView.Columns.Add("����", "����");
                }

                if (!dgvView.Columns.Contains("�հ�"))
                {
                    dgvView.Columns.Add("�հ�", "�հ�");
                }

                if (!dgvView.Columns.Contains("�������"))
                {
                    dgvView.Columns.Add("�������", "�������");
                }

                if (!dgvView.Columns.Contains("�������%"))
                {
                    dgvView.Columns.Add("�������%", "�������%");
                }

                if (!dgvView.Columns.Contains("���󼼱�"))
                {
                    dgvView.Columns.Add("���󼼱�", "���󼼱�");
                }

            }


            dgvView.Columns["�����"].DisplayIndex = 0;
            dgvView.Columns["���簡"].DisplayIndex = 1;
            dgvView.Columns["���ϰ�"].DisplayIndex = 2;
            dgvView.Columns["��������"].DisplayIndex = 3;
            dgvView.Columns["����%"].DisplayIndex = 4;
            dgvView.Columns["�ŷ���"].DisplayIndex = 5;
            dgvView.Columns["CodeNum"].Visible = false;

            if (p_ViewFlag == enViewFlag.Hold)
            {
                dgvView.Columns["��հ�"].DisplayIndex = 6;
                dgvView.Columns["����"].DisplayIndex = 7;
                dgvView.Columns["�հ�"].DisplayIndex = 8;
            }
        }

        // �׸�������
        /// <summary>
        /// �׸��� �ʱ�ȭ
        /// </summary>
        /// <param name="p_isSearch"></param>
        private void fnSortGrid(enViewFlag p_ViewFlag, bool p_isSearch)
        {
            if (p_isSearch)
            {
                foreach (DataGridViewColumn col in dgvView.Columns)
                {
                    col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                dgvView.Columns["�����"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (p_ViewFlag == enViewFlag.Hold)
                {

                    dgvView.Columns["��հ�"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvView.Columns["����"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvView.Columns["�հ�"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvView.Columns["�������"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvView.Columns["�������%"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvView.Columns["���󼼱�"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


                    dgvView.Columns["��հ�"].DefaultCellStyle.Format = "#,##";
                    dgvView.Columns["����"].DefaultCellStyle.Format = "#,##";
                    dgvView.Columns["�հ�"].DefaultCellStyle.Format = "#,##";
                    dgvView.Columns["�������"].DefaultCellStyle.Format = "#,##";
                    dgvView.Columns["�������%"].DefaultCellStyle.Format = "#,#0.##";
                    dgvView.Columns["���󼼱�"].DefaultCellStyle.Format = "#,##";
                }
                dgvView.Columns["���簡"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvView.Columns["���簡"].DefaultCellStyle.Format = "#,##";
                dgvView.Columns["���ϰ�"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvView.Columns["��������"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvView.Columns["����%"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvView.Columns["�ŷ���"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvView.Columns["���ϰ�"].DefaultCellStyle.Format = "#,##";
                dgvView.Columns["��������"].DefaultCellStyle.Format = "#,##";
                dgvView.Columns["����%"].DefaultCellStyle.Format = "#,##";
                dgvView.Columns["�ŷ���"].DefaultCellStyle.Format = "#,##";


                //��Ŀ�� 
                if (dgvView.Rows.Count > 0)
                {
                    dgvView.Rows[dgvView.Rows.Count - 1].Selected = true;
                    dgvView.CurrentCell = dgvView.Rows[dgvView.Rows.Count - 1].Cells["�����"];
                    dgvView.BeginEdit(true);
                }
            }

            if (p_ViewFlag == enViewFlag.Hold)
            {
                dgvView.Columns["���簡"].Width = 70;
                dgvView.Columns["��հ�"].Width = 70;
                dgvView.Columns["����"].Width = 60;
                dgvView.Columns["�հ�"].Width = 70;
                dgvView.Columns["�������"].Width = 70;
                dgvView.Columns["�������%"].Width = 70;
                dgvView.Columns["���󼼱�"].Width = 70;
            }

            if (dgvView.Height - 20 - (dgvView.RowTemplate.Height * dgvView.RowCount) > 0)
            {
                //��ũ�ѹ� �� ����� ���
                dgvView.Columns["�����"].Width = 355 - 270;//dgvHold.Width - 270;

            }
            else
            {
                //��ũ�ѹ� ����� ���
                dgvView.Columns["�����"].Width = 355 - 290; //dgvHold.Width - 290;
            }

            dgvView.Columns["���ϰ�"].Width = 70;
            dgvView.Columns["��������"].Width = 70;
            dgvView.Columns["����%"].Width = 60;
            dgvView.Columns["�ŷ���"].Width = 70;
        }

        //�׸��忡 �� �ֱ�
        /// <summary>
        /// �׸��忡 �� �ֱ�
        /// </summary>
        /// <param name="p_ViewFlag">Hold,Interest</param>
        /// <param name="p_dt">���̺�</param>
        private void fnInsertValue(enViewFlag p_ViewFlag, DataTable p_dt)
        {
            //���� �÷��� �� �ֱ�
            string CodeNum = "";
            Dictionary<string, string> Value = new Dictionary<string, string>();

            for (int i = 0; i < p_dt.Rows.Count; i++)
            {
                dgvView.Rows.Add();
                CodeNum = p_dt.Rows[i]["CodeNum"].ToString();
                dgvView["�����", i].Value = p_dt.Rows[i]["�����"].ToString();

                Value = Parser.GetInfo(CodeNum);

                dgvView["���簡", i].Value = Value["NowV"];
                dgvView["���ϰ�", i].Value = Value["YesterDayV"];
                dgvView["��������", i].Value = Value["Interval"];
                dgvView["����%", i].Value = Value["Per"];
                dgvView["�ŷ���", i].Value = Value["QV"];
                dgvView["CodeNum", i].Value = CodeNum;
                if (p_ViewFlag == enViewFlag.Hold)
                {
                    dgvView["��հ�", i].Value = p_dt.Rows[i]["��հ�"].ToString();
                    dgvView["����", i].Value = p_dt.Rows[i]["����"].ToString();
                    dgvView["�հ�", i].Value = p_dt.Rows[i]["�հ�"].ToString();

                    dgvView["�������", i].Value = ((Convert.ToDouble(Value["NowV"]) - Convert.ToDouble(dgvView["��հ�", i].Value)) * Convert.ToDouble(dgvView["����", i].Value))    //���簡 - ������հ�
                                    - (Convert.ToDouble(Value["NowV"]) * Convert.ToDouble(dgvView["����", i].Value) * 0.003);   //�ŵ�����
                    dgvView["�������%", i].Value = (((Convert.ToDouble(Value["NowV"]) - Convert.ToDouble(dgvView["��հ�", i].Value)) * Convert.ToDouble(dgvView["����", i].Value))    //���簡 - ������հ�
                                    - (Convert.ToDouble(Value["NowV"]) * Convert.ToDouble(dgvView["����", i].Value) * 0.003))   //�ŵ�����
                                    / (Convert.ToDouble(dgvView["��հ�", i].Value) * Convert.ToDouble(dgvView["����", i].Value)) * 100;   //%���ϱ�
                    dgvView["���󼼱�", i].Value = (Convert.ToDouble(Value["NowV"]) * Convert.ToDouble(dgvView["����", i].Value) * 0.003);
                }

            }

        }

        //�޺����ε�(���ɸ��)
        private void fnCboBingding()
        {
            string strSelItem = cboInter.SelectedItem == null ? "" : cboInter.SelectedItem.ToString();
            cboInter.Items.Clear();

            DataTable dtCombo = Network.GetDBTable("SELECT IFNULL(MAX(InterName),'') AS InterName FROM Interest GROUP BY InterName");
            

            foreach (DataRow dr in dtCombo.Rows)
            {
                if (!cboInter.Items.Contains(dr["InterName"].ToString()))
                {
                    cboInter.Items.Add(dr["InterName"].ToString());
                }
            }
            /*
            if (cboInter.Items.Count > 0)
                cboInter.SelectedIndex = 0;
            */
            if (strSelItem != "")
                cboInter.SelectedItem = strSelItem;
            else if (cboInter.Items.Count > 0)
                cboInter.SelectedIndex = 0;
        }

        //�������ε�
        /// <summary>
        /// �������ε�
        /// </summary>
        private void mfnHoldBinder()
        {



            //����б�
            fnHoldSearch();

            //��Ŀ�� �ʱ�ȭ
            dgvView.ClearSelection();

        }

        //���ɹ��ε�
        /// <summary>
        /// ���ɹ��ε�
        /// </summary>
        private void mfnInterestBinder()
        {
            //�޺����ε�
            fnCboBingding();

            //����б�
            fnInterestSearch();

            //��Ŀ�� �ʱ�ȭ
            dgvView.ClearSelection();
        }

        //���θ޸� ���ε�
        private void mfnMainMemoBinder()
        {
            DataTable dt = Network.GetDBTable("SELECT Contents FROM SimpleMemo WHERE CodeNum = 'MainMemo'");

            if (dt.Rows.Count > 0)
            {
                txtMemo.Text = dt.Rows[0]["Contents"].ToString();
            }
            else
            {
                Network.ExecDB("INSERT INTO SimpleMemo (Contents, CodeNum) VALUES ('', 'MainMemo')");
            }
        }

        //��ư ���� ����
        /// <summary>
        /// ��ư ���� ����
        /// </summary>
        /// <param name="p_strBtnTag">��ư�� �±װ�</param>
        /// <param name="p_isEnable">��ư���� Enable����</param>
        private void fnChangeBtn(string p_strBtnTag, bool p_isEnable)
        {
            if (p_strBtnTag == "Add2" || p_strBtnTag == "Del2")
            {
                btnAdd2.Tag = "Add2.Comfirm";
                btnDel2.Tag = "Del2.Cancel";
            }


            else if (p_strBtnTag == "Chg")
            {
                btnAdd2.Tag = "Chg.Comfirm";
                btnDel2.Tag = "Chg.Cancel";
            }

            else if (p_strBtnTag == "Add2.Comfirm" || p_strBtnTag == "Del2.Cancel" || p_strBtnTag == "Chg.Comfirm" || p_strBtnTag == "Chg.Cancel")
            {
                btnAdd2.Tag = "Add2";
                btnDel2.Tag = "Del2";
            }


            if (p_isEnable)
            {
                btnAdd2.Text = "�߰�";
                btnDel2.Text = "����";
            }
            else
            {
                btnAdd2.Text = "Ȯ��";
                btnDel2.Text = "���";
            }

            btnChg.Visible = p_isEnable;
            btnAdd.Enabled = p_isEnable;
            btnDel.Enabled = p_isEnable;
            btnUp.Enabled = p_isEnable;
            btnDown.Enabled = p_isEnable;
            cboStoSearch.Enabled = p_isEnable;
        }
        

        //�߰���ư ����
        private void efnInsert()
        {
            if (cboInter.Text.Replace(" ", "") == "")
            {
                MessageBox.Show("���ɸ���� �� ���Դϴ�.");
                return;
            }
            string strSQL = @"
                                SELECT *
                                  FROM Interest
                                 WHERE COMPANY = '" + cboStoSearch.Text.ToUpper() + @"'
                                   AND IFNULL(InterName,'') = '" + cboInter.Text + @"';

                                SELECT *
                                  FROM Code
                                 WHERE COMPANY = '" + cboStoSearch.Text.ToUpper() + @"'";
            DataSet ds = Network.GetDBSet(strSQL);

            if (ds.Tables[0].Rows.Count >= 1)
            {
                cboStoSearch.Text = "";
            }
            else if (ds.Tables[1].Rows.Count < 1)
            {
                MessageBox.Show("���� �����Դϴ�.");
            }
            else
            {
                strSQL = @"
                            INSERT INTO Interest(Company, CodeNum, DisSeq, InterName)
                                   SELECT Company
                                        , CodeNum
                                        , IFNULL((SELECT MAX(DisSeq) + 1 AS DisSeq 
                                             FROM Interest 
                                            WHERE IFNULL(InterName,'') = '" + cboInter.Text + @"'
                                            GROUP BY InterName),1) AS DisSeq
                                        , '" + cboInter.Text + @"' 
                                     FROM Code
                                    WHERE COMPANY = '" + cboStoSearch.Text.ToUpper() + "';";
                
                if (Network.ExecDB(strSQL) > 0)
                    MessageBox.Show("�߰��Ͽ����ϴ�.");
                else
                    MessageBox.Show("�߰�����");
                fnInterestSearch();
            }
            cboStoSearch.Text = "";
        }


        //������ư ����
        private void efnDelete()
        {
            string strSQL = @"
                                SELECT *
                                  FROM Interest
                                 WHERE COMPANY = '" + cboStoSearch.Text.ToUpper() + @"'
                                   AND IFNULL(InterName,'') = '" + cboInter.Text + @"';";
            DataTable dt = Network.GetDBTable(strSQL);

            if (dt.Rows.Count == 1)
            {
                strSQL = @"
                            DELETE FROM Interest
                                  WHERE COMPANY = '" + cboStoSearch.Text.ToUpper() + @"'
                                    AND IFNULL(InterName,'') = '" + cboInter.Text + @"';";
                
                if (Network.ExecDB(strSQL) > 0)
                    MessageBox.Show("�����Ͽ����ϴ�.");
                else
                    MessageBox.Show("��������");
                mfnInterestBinder();
                //fnInterestSearch();
            }
            else
            {
                MessageBox.Show("�������� ���� �����Դϴ�.");
            }
            cboStoSearch.Text = "";
        }

        //�����߰�2 ��ư ����
        private void efnAdd2()
        {
            if (btnAdd2.Tag.ToString() == "Add2")
            {
                cboInter.SelectedIndexChanged -= new System.EventHandler(this.cboInter_SelectedIndexChanged);
                cboInter.DropDownStyle = ComboBoxStyle.Simple;
                cboInter.Text = "";
                fnChangeBtn(btnAdd2.Tag.ToString(), false);
                cboInter.SelectedIndexChanged += new System.EventHandler(this.cboInter_SelectedIndexChanged);
            }
            else if (btnAdd2.Tag.ToString() == "Add2.Comfirm")
            {
                foreach (string itemName in cboInter.Items)
                {
                    if (cboInter.Text == itemName)
                    {
                        MessageBox.Show("���� �̸��� ���ɸ���� �ֽ��ϴ�.");
                        return;
                    }
                }

                if (cboInter.Text.Replace(" ", "") == "")
                {
                    MessageBox.Show("���� �Է����ּ���.");
                    return;
                }

                fnInterestSearch();
                string strTempCbo = cboInter.Text;

                cboInter.DropDownStyle = ComboBoxStyle.DropDownList;

                cboInter.Items.Add(strTempCbo);
                cboInter.SelectedIndex = cboInter.Items.IndexOf(strTempCbo);
                fnChangeBtn(btnAdd2.Tag.ToString(), true);

            }

            else if (btnAdd2.Tag.ToString() == "Chg.Comfirm")
            {
                foreach (string itemName in cboInter.Items)
                {
                    if (cboInter.Text == itemName)
                    {
                        MessageBox.Show("���� �̸��� ���ɸ���� �ֽ��ϴ�.");
                        return;
                    }
                }

                if (cboInter.Text.Replace(" ", "") == "")
                {
                    MessageBox.Show("���� �Է����ּ���.");
                    return;
                }

                if (Network.ExecDB(@"UPDATE Interest SET InterName = '" + cboInter.Text + @"' WHERE InterName = '" + mInterListName + "'") <= 0)
                {
                    MessageBox.Show("���� ����");
                    return;
                }

                string strTempCbo = cboInter.Text;

                mfnInterestBinder();
                
                cboInter.DropDownStyle = ComboBoxStyle.DropDownList;

                //cboInter.Items.Add(strTempCbo);
                cboInter.SelectedIndex = cboInter.Items.IndexOf(strTempCbo);
                fnChangeBtn(btnAdd2.Tag.ToString(), true);
            }
        }

        //���ɻ���2 ��ư ����
        private void efnDel2()
        {

            if (btnDel2.Tag.ToString() == "Del2")
            {
                if (MessageBox.Show("'����'!! ���ɸ�� �����Դϴ�!!! ���� ���� �Ͻðڽ��ϰ�? ", "���ɸ�� ����", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (Network.ExecDB("DELETE FROM Interest WHERE InterName = '" + cboInter.Text + "'") > 0)
                    {
                        mfnInterestBinder();
                    }
                    else
                        MessageBox.Show("���� ����");
                }
            }
            else if (btnDel2.Tag.ToString() == "Del2.Cancel" || btnDel2.Tag.ToString() == "Chg.Cancel")
            {
                fnChangeBtn(btnDel2.Tag.ToString(), true);
                cboInter.DropDownStyle = ComboBoxStyle.DropDownList;
                cboInter.SelectedItem = mInterListName;
            }
        }

        //���ɺ��� ��ư ����
        private void efnChg()
        {
            if (cboInter.Text.Replace(" ", "") == "")
                return;
            cboInter.SelectedIndexChanged -= new System.EventHandler(this.cboInter_SelectedIndexChanged);
            cboInter.DropDownStyle = ComboBoxStyle.Simple;
            fnChangeBtn(btnChg.Tag.ToString(), false);
            cboInter.SelectedIndexChanged += new System.EventHandler(this.cboInter_SelectedIndexChanged);

        }

        //���� DisSeq Up��ư
        private void efnDisSeqUp()
        {
            int intSelectedRow;
            try
            {
                intSelectedRow = dgvView.CurrentCell.RowIndex;
                if (intSelectedRow < 1)
                    return;
            }
            catch (NullReferenceException)
            {
                return;
            }
            
            DataTable dtDisSeq = Network.GetDBTable(@"SELECT DisSeq 
                                                        FROM Interest 
                                                       WHERE InterName = '" + cboInter.Text + @"'
                                                         AND (Company = '" + dgvView["�����",intSelectedRow - 1].Value.ToString() + @"'
                                                          OR  Company = '" + dgvView["�����",intSelectedRow].Value.ToString() + @"')
                                                       ORDER BY DisSeq");
            if(dtDisSeq.Rows.Count != 2)
            {
                MessageBox.Show("SELECT ����");
                return;
            }

            int intResult;

            intResult = Network.ExecDB(@"UPDATE Interest 
                                            SET DisSeq = '" + dtDisSeq.Rows[0][0].ToString() + @"'
                                          WHERE InterName = '" + cboInter.Text + @"'
                                            AND Company = '" + dgvView["�����",intSelectedRow].Value.ToString() + @"';
                                
                                         UPDATE Interest
                                            SET DisSeq = '" + dtDisSeq.Rows[1][0].ToString() + @"'
                                          WHERE InterName = '" + cboInter.Text + @"'
                                            AND Company = '" + dgvView["�����", intSelectedRow - 1].Value.ToString() + @"';");

            if (intResult < 1)
            {
                MessageBox.Show("Update ����");
                return;
            }

            fnInterestSearch();
            dgvView.Rows[intSelectedRow - 1].Selected = true;
            dgvView.CurrentCell = dgvView.Rows[intSelectedRow - 1].Cells["�����"];
            dgvView.BeginEdit(true);

        }

        //���� DisSeq Down��ư
        private void efnDisSeqDown()
        {
            int intSelectedRow;
            try
            {
                intSelectedRow = dgvView.CurrentCell.RowIndex;
                if (intSelectedRow == dgvView.Rows.Count-1)
                    return;
            }
            catch (NullReferenceException)
            {
                return;
            }

            DataTable dtDisSeq = Network.GetDBTable(@"SELECT DisSeq 
                                                        FROM Interest 
                                                       WHERE InterName = '" + cboInter.Text + @"'
                                                         AND (Company = '" + dgvView["�����", intSelectedRow].Value.ToString() + @"'
                                                          OR  Company = '" + dgvView["�����", intSelectedRow + 1].Value.ToString() + @"')
                                                       ORDER BY DisSeq");
            if (dtDisSeq.Rows.Count != 2)
            {
                MessageBox.Show("SELECT ����");
                return;
            }

            int intResult;

            intResult = Network.ExecDB(@"UPDATE Interest 
                                            SET DisSeq = '" + dtDisSeq.Rows[1][0].ToString() + @"'
                                          WHERE InterName = '" + cboInter.Text + @"'
                                            AND Company = '" + dgvView["�����", intSelectedRow].Value.ToString() + @"';
                                
                                         UPDATE Interest
                                            SET DisSeq = '" + dtDisSeq.Rows[0][0].ToString() + @"'
                                          WHERE InterName = '" + cboInter.Text + @"'
                                            AND Company = '" + dgvView["�����", intSelectedRow + 1].Value.ToString() + @"';");

            if (intResult < 1)
            {
                MessageBox.Show("Update ����");
                return;
            }

            fnInterestSearch();
            dgvView.Rows[intSelectedRow + 1].Selected = true;
            dgvView.CurrentCell = dgvView.Rows[intSelectedRow + 1].Cells["�����"];
            dgvView.BeginEdit(true);
        }


        #region **********�̺�Ʈ**********
        #endregion

        // Ÿ���̺�Ʈ
        /// <summary>
        /// ���簡�� 60�ʸ��� �޾ƿ��� ���ؼ� ����. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Variables.ISTIMER)
            {
                mfnReLoad();
            }
        }

        // ���������̺�Ʈ
        private void Hold_Resize(object sender, EventArgs e)
        {
            
            if (labHold.ForeColor == Color.Black)
                fnSortGrid(enViewFlag.Hold, false);
            else if (labInterest.ForeColor == Color.Black)
                fnSortGrid(enViewFlag.Interest, false);
             
        }

        // ����Ŭ��
        private void labHold_Click(object sender, EventArgs e)
        {
            labHold.ForeColor = Color.Black;
            labInterest.ForeColor = Color.Gray;
            labMainMemo.ForeColor = Color.Gray;
            dgvView.Location = new System.Drawing.Point(0, 0);
            dgvView.Size = panel2.Size;
            panel3.Visible = false;
            btnSelectOpen.Visible = false;
            txtMemo.Visible = false;
            mfnHoldBinder();
        }

        // ����Ŭ��
        private void labInterest_Click(object sender, EventArgs e)
        {

            panel3.Visible = mISpanelVible ? true : false;
            btnSelectOpen.Visible = true;
            labHold.ForeColor = Color.Gray;
            labInterest.ForeColor = Color.Black;
            labMainMemo.ForeColor = Color.Gray;
            txtMemo.Visible = false;
            dgvView.Location = new System.Drawing.Point(0, panel3.Visible ? panel3.Size.Height : 0);
            dgvView.Size = new System.Drawing.Size(panel2.Size.Width, panel3.Visible ? panel2.Size.Height - panel3.Size.Height : panel2.Size.Height);
            mfnInterestBinder();

        }

        // ���θ޸� Ŭ��
        private void labMainMemo_Click(object sender, EventArgs e)
        {
            labHold.ForeColor = Color.Gray;
            labInterest.ForeColor = Color.Gray;
            labMainMemo.ForeColor = Color.Black;
            //dgvView.Location = new System.Drawing.Point(0, 0);
            //dgvView.Size = panel2.Size;
            panel3.Visible = false;
            btnSelectOpen.Visible = false;
            txtMemo.Visible = true;
            txtMemo.Dock = DockStyle.Fill;
        }

        // �����߰� ��ưŬ��
        private void btnAdd_Click(object sender, EventArgs e)
        {
            efnInsert();
        }

        // ���ɻ��� ��ưŬ��
        private void btnDel_Click(object sender, EventArgs e)
        {
            efnDelete();
        }

        // �����߰� txtŰ�ٿ�
        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                efnInsert();
            }
        }

        // �������񿡼� ����Ŭ�� ��, �ڼ��� ������ ����. 
        private void dgvView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (labHold.ForeColor == Color.Black)
            {
                Report RE = new Report(dgvView["�����", dgvView.CurrentCell.RowIndex].Value.ToString());
                RE.StartPosition = FormStartPosition.CenterParent;
                RE.ShowDialog();
            }
        }

        //���ɿ��� ��Ŭ������ ��, �̸� txtbox�� �־��ֱ�
        private void dgvView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (labInterest.ForeColor == Color.Black)
            {
                try
                {
                    cboStoSearch.Text = dgvView["�����", e.RowIndex].Value.ToString();
                }
                catch
                {
                    cboStoSearch.Text = "";
                }
            }
        }

        //���� �߰� ���� �ǳ� �Ⱥ��̰� �ϱ�. 
        private void btnSelectOpen_Click(object sender, EventArgs e)
        {
            if (panel3.Visible)
            {
                dgvView.Location = new System.Drawing.Point(0, 0);
                dgvView.Size = panel2.Size;
                panel3.Visible = false;
                mISpanelVible = false;
            }
            else
            {
                dgvView.Location = new System.Drawing.Point(0, panel3.Size.Height);
                dgvView.Size = new System.Drawing.Size(panel2.Size.Width, panel2.Size.Height - panel3.Size.Height);
                panel3.Visible = true;
                mISpanelVible = true;
            }
        }

        //���� DisSeq Up����
        private void btnUp_Click(object sender, EventArgs e)
        {
            efnDisSeqUp();
        }

        //���� DisSeq Down����
        private void btnDown_Click(object sender, EventArgs e)
        {
            efnDisSeqDown();
        }

        //�����߰�2 ��ư
        private void btnAdd2_Click(object sender, EventArgs e)
        {
            efnAdd2();
        }

        //���ɸ�� ���� 
        private void btnDel2_Click(object sender, EventArgs e)
        {
            efnDel2();
        }

        //���ɺ����ư ���� ��
        private void btnChg_Click(object sender, EventArgs e)
        {
            efnChg();
        }

        //���ɸ�� �޺� ���� ��
        private void cboInter_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboStoSearch.Text = "";
            fnInterestSearch();
        }

        private void cboInter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && cboInter.DropDownStyle == ComboBoxStyle.Simple)
            {
                efnAdd2();
            }

        }

        //�˾�
        private void dgvView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                m.Show(dgvView, new Point(e.X, e.Y));
            }
        }

        //�˾�������Ŭ��(������)
        private void popUpItem_Click(object sender, EventArgs e)
        {
            Detail DT = new Detail(dgvView["CodeNum", dgvView.CurrentCell.RowIndex].Value.ToString());
            DT.StartPosition = FormStartPosition.CenterParent;
            DT.ShowDialog();
        }

        //�˾�������Ŭ��(���ô���)
        private void popUpNews_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://finance.naver.com/item/news_news.nhn?code=" + dgvView["CodeNum", dgvView.CurrentCell.RowIndex].Value.ToString() + "&page=&sm=title_entity_id.basic&clusterId=");
        }

        //�˾�������Ŭ��(����Ʈ)
        private void popUpReport_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://vip.mk.co.kr/newSt/news/news_list2.php?sCode=110");
        }

        //�˾�������Ŭ��(���ܸ޸�)
        private void popUpSimpleMemo_Click(object sender, EventArgs e)
        {
            SimpleMemo SM = new SimpleMemo(dgvView["CodeNum", dgvView.CurrentCell.RowIndex].Value.ToString(), dgvView["�����", dgvView.CurrentCell.RowIndex].Value.ToString());
            SM.StartPosition = FormStartPosition.CenterParent;
            SM.ShowDialog();
        }

        //���θ޸𿡼� ���� �� ���̺��Ѵ�. 
        private void txtMemo_Leave(object sender, EventArgs e)
        {
            Network.ExecDB("UPDATE SimpleMemo SET Contents = '" + txtMemo.Text + "' WHERE CodeNum = 'MainMemo'");
        }

    }
}