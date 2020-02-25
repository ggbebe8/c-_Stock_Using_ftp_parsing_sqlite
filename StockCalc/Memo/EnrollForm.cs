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
        #region ��������

        public string rMainCombText = "";

        public string rSmallCombText = "";

        int mSeq;   //������ �� Seq��ȣ

        string mDate;   //���� ���� ��¥ -- ��¥ ���� �� ������ �ű�� ����

        bool mIsNew = false; //�ű� ���� ���� 

        string mReportSeq = ""; //Buy�� Sell ���̺�� ����Ǿ� �ִ� �޸�

        string mReportType = "";

        DataTable dtCombo;


        #endregion ��������_End

        #region ������
        /// <summary>
        /// �ű�
        /// </summary>
        /// <param name="p_Main"></param>
        /// <param name="p_Small"></param>
        public EnrollForm()
        {
            InitializeComponent();

            dte.Value = DateTime.Now;

            mIsNew = true;

            fnComboInit();
            
            cboColor.SelectedItem = "���";
        }


        /// <summary>
        /// ����
        /// </summary>
        /// <param name="p_Main"></param>
        /// <param name="p_Small"></param>
        /// <param name="p_dicSelected"></param>
        public EnrollForm(Dictionary<string,string> p_dicSelected)
        {
            InitializeComponent();

            //seq�� ���������� ����
            mSeq = Convert.ToInt32(p_dicSelected["Seq"]);

            //���� ��¥ ����
            mDate = p_dicSelected["��¥"];

            //�ű� ���� ���� 
            mIsNew = false;

            //�޺����ε�
            fnComboInit();

            //�� �ۿ� ��Ʈ�� ���ε�
            fnSelectedInit(p_dicSelected);

            if (p_dicSelected["reportSeq"] != "")
            {
                cboMain.Enabled = false;
                cboSmall.Enabled = false;
                dte.Enabled = false;
            }
        }
        
        /// <summary>
        /// Report���� �˾����� �Ѿ����
        /// </summary>
        /// <param name="p_repSeq">Buy Sell</param>
        /// <param name="p_repSeq">[0]���̺� [1]������ Seq</param>
        /// <param name="p_memSeq">�޸��� Seq(���� �����Ͱ� ���� ���(����)��)</param>
        public EnrollForm(string p_reportType, string p_reportSeq, string p_memSeq)
        {
            InitializeComponent();

            mReportSeq = p_reportSeq;

            mReportType = p_reportType;

            //�޺����ε�
            fnComboInit();

            string strSQL = "SELECT * FROM Memo WHERE Valid = 'Y' AND Seq = '" + p_memSeq + "';\r\n";

            if(p_reportType == "Buy")
                strSQL += "SELECT Name, Date FROM Buy WHERE Seq = '" + p_reportSeq + "'";
            else if(p_reportType == "Sell")
                strSQL += "SELECT Name, Date FROM Sell WHERE Seq = '" + p_reportSeq + "'";

            DataSet ds = Network.GetDBSet(strSQL);

            //�������� ������ ���
            if (ds.Tables[0].Rows.Count > 0)
            {
                mSeq = Convert.ToInt32(ds.Tables[0].Rows[0]["Seq"].ToString());
                //mDate = ds.Tables[0].Rows[0]["Date"].ToString();
                dte.Value = DateTime.ParseExact(ds.Tables[0].Rows[0]["Date"].ToString(), "yyyyMMdd", null);
                //�ű� ���� ���� 
                mIsNew = false;
                //�� �ۿ� ��Ʈ�� ���ε�
                fnSelectedInit(ds.Tables[0]);

            }

            else
            {
                txtTitle.Text = p_reportType == "Buy" ? "Buy" : p_reportType == "Sell" ? "Sell" : "";
                dte.Value = DateTime.ParseExact(ds.Tables[1].Rows[0]["Date"].ToString(), "yyyyMMdd", null);
                //�ű� ���� ����
                mIsNew = true;
            }
            
            cboMain.Text = "����";
            cboSmall.Text = ds.Tables[1].Rows[0]["Name"].ToString();
            cboMain.Enabled = false;
            cboSmall.Enabled = false;
            dte.Enabled = false;
        }
        
        #endregion ������_End




        #region �Լ�

        /// <summary>
        /// ���� ��, ��Ʈ�� �ʱ�ȭ
        /// </summary>
        /// <param name="p_dicSelected"></param>
        private void fnSelectedInit(Dictionary<string, string> p_dicSelected)
        {
            dte.Value = Convert.ToDateTime(p_dicSelected["��¥"].Substring(0, 4) + "." + p_dicSelected["��¥"].Substring(4, 2) + "." + p_dicSelected["��¥"].Substring(6, 2));
            cboMain.SelectedIndex = cboMain.FindStringExact(p_dicSelected["��з�"]);
            cboSmall.SelectedIndex = cboSmall.FindStringExact(p_dicSelected["�Һз�"]);
            cboColor.SelectedIndex = cboColor.FindStringExact(p_dicSelected["Color"] == "" ? "���" : p_dicSelected["Color"]);
            txtTitle.Text = p_dicSelected["����"].Replace("��", "'");
            //rtxtContents.Text = p_dicSelected["����"].Replace("��", "'");
            if (p_dicSelected["Contents_Rtf"] != "")
            {
                rtxtContents.Rtf = p_dicSelected["Contents_Rtf"].Replace("��", "'");
            }
            else
            {
                rtxtContents.Text = p_dicSelected["����"].Replace("��", "'");
            }
        }

        /// <summary>
        /// ���� ��, ��Ʈ�� �ʱ�ȭ
        /// </summary>
        /// <param name="p_dicSelected"></param>
        private void fnSelectedInit(DataTable p_dtMem)
        {
            dte.Value = Convert.ToDateTime(p_dtMem.Rows[0]["Date"].ToString().Substring(0, 4) + "."
                        + p_dtMem.Rows[0]["Date"].ToString().Substring(4, 2) + "."
                        + p_dtMem.Rows[0]["Date"].ToString().Substring(6, 2));
            cboMain.SelectedIndex = cboMain.FindStringExact(p_dtMem.Rows[0]["MainCate"].ToString());
            cboSmall.SelectedIndex = cboSmall.FindStringExact(p_dtMem.Rows[0]["SmallCate"].ToString());
            cboColor.SelectedIndex = cboColor.FindStringExact(p_dtMem.Rows[0]["Color"].ToString() == "" ? "���" : p_dtMem.Rows[0]["Color"].ToString());
            txtTitle.Text = p_dtMem.Rows[0]["Title"].ToString().Replace("��", "'");
            if (p_dtMem.Rows[0]["Contents_Rtf"].ToString() != "")
            {
                rtxtContents.Rtf = p_dtMem.Rows[0]["Contents_Rtf"].ToString().Replace("��", "'");
            }
            else
            {
                rtxtContents.Text = p_dtMem.Rows[0]["Contents"].ToString().Replace("��", "'");
            }
        }

        /// <summary>
        /// �޺� �ʱ�ȭ
        /// </summary>
        /// <param name="p_Main"></param>
        /// <param name="p_Small"></param>
        private void fnComboInit()
        {
            cboMain.Items.Clear();
            cboSmall.Items.Clear();

            cboMain.Items.Add("<��ü>");

            cboColor.Items.Add("���");
            cboColor.Items.Add("ȸ��");
            cboColor.Items.Add("�ϴ�");
            cboColor.Items.Add("���");
            cboColor.Items.Add("����");

            dtCombo = Network.GetDBTable(@"SELECT MainCate as '��з�', SmallCate as '�Һз�'
                                             FROM Memo
                                            WHERE Valid = 'Y'
                                            GROUP BY SmallCate, MainCate");

            foreach (DataRow dr in dtCombo.Rows)
            {
                if (!cboMain.Items.Contains(dr["��з�"].ToString()))
                {
                     cboMain.Items.Add(dr["��з�"].ToString());
                }
            }

        }


        /// <summary>
        /// ��� ����  
        /// </summary>
        /// <param name="p_Seq"></param>
        /// <returns></returns>
        private bool fnSave(int p_Seq)
        {
            if (txtTitle.Text == "")
            {
                MessageBox.Show("���� ������ �����ϴ�.");
                return false;
            }

            string strSQL = "";
            string strCboMain = cboMain.Text.Equals("<��ü>") ? "" : cboMain.Text;

            if (strCboMain.Replace(" ","").Equals(""))
            {
                MessageBox.Show("��з��� ������ �����ϴ�.");
                return false;
            }

            //��¥�� �ٲ��� ��� ���� ��ε� �ٽ� ������

            if (mIsNew)
            {
                strSQL = @"INSERT INTO Memo ( Date, MainCate, SmallCate, Title, Contents,  Contents_Rtf, Color, Valid, ReportSeq, ReportType)
                            VALUES (
                                     '" + dte.Value.ToString("yyyyMMdd") + @"'
                                    ,'" + strCboMain + @"'
                                    ,'" + cboSmall.Text + @"'
                                    ,'" + txtTitle.Text.Replace("'","��") + @"'
                                    ,'" + rtxtContents.Text.Replace("'", "��") + @"'
                                    ,'" + rtxtContents.Rtf.Replace("'", "��") + @"'
                                    ,'" + (cboColor.Text == "���" ? "" : cboColor.Text) + @"'
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
                                  Title = '" + txtTitle.Text.Replace("'", "��") + @"',
                                  Contents = '" + rtxtContents.Text.Replace("'", "��") + @"',
                                  Contents_Rtf = '" + rtxtContents.Rtf.Replace("'", "��") + @"',
                                  Color = '" + (cboColor.Text == "���" ? "" : cboColor.Text) + @"'
                            WHERE Seq = " + mSeq;
            }

            try
            {
                int iresult = Network.ExecDB(strSQL);
                if (iresult > 0)
                    return true;
                else
                {
                    MessageBox.Show("�������");
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
                
                //���⺯��
                if(p_boldOpt)
                    rtxtContents.SelectionFont = new Font(rtxtContents.SelectionFont.FontFamily, rtxtContents.SelectionFont.Size, rtxtContents.SelectionFont.Bold ? FontStyle.Regular : FontStyle.Bold);

                else
                    rtxtContents.SelectionFont = new Font(rtxtContents.SelectionFont.FontFamily, rtxtContents.SelectionFont.Size + p_size, rtxtContents.SelectionFont.Bold ? FontStyle.Bold : FontStyle.Regular);
            }

            rtxtContents.Select(intSelectStart, intSelectLength);
        }

        #endregion �Լ�_End

        #region �̺�Ʈ ����
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (fnSave(mSeq))
            {
                MessageBox.Show("����Ǿ����ϴ�.");
                mDate = dte.Value.ToString("yyyyMMdd");
                mIsNew = false;
            }



        }

        private void btnSaveClose_Click(object sender, EventArgs e)
        {
            if (fnSave(mSeq))
            {
                MessageBox.Show("����Ǿ����ϴ�.");
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


        private void cboMain_Leave(object sender, EventArgs e)
        {
            cboSmall.Items.Clear();

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
                if (keyData.Equals(Keys.F5))
                {
                    if (fnSave(mSeq))
                    {
                        MessageBox.Show("����Ǿ����ϴ�.");
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

        #endregion �̺�Ʈ ����_End


    }
}