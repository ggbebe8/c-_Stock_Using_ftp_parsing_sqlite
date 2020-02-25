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
    public partial class Input : Form
    {
        //*****************��������******************//
        DataSet mDTHold;    //���纸�� �׸����


        //*****************������******************//
        #region ������
        public Input()
        {
            InitializeComponent();
            fnInitCbo();
        }

        private void Input_Load(object sender, EventArgs e)
        {
            fnHoldSearch();
        }
        #endregion ������_End


        //*****************�����Լ�******************//

        #region �޺��ʱ�ȭ
        /// <summary>
        /// �޺��ʱ�ȭ
        /// </summary>
        private void fnInitCbo()
        {

            cboType.Items.Clear();

            cboType.Items.Add("�ż�");
            cboType.Items.Add("�ŵ�");

            cboType.SelectedItem = "�ż�";

            string strQuery = "";
            strQuery += "\r\n" + "SELECT Company FROM Code";
            DataTable dt = Network.GetDBTable(strQuery);

            foreach (DataRow dr in dt.Rows)
            {
                cboStoSearch.Items.Add(dr["Company"].ToString());
            }
        }




        #endregion �޺��ʱ�ȭ_End

        #region �Է� �Լ�
        /// <summary>
        /// �Է� �Լ�
        /// </summary>
        private void efnDeal()
        {
            string strSQL = "";

            // ���� �� ���̸� ����
            if (txtPrice.Text == "")
            {
                MessageBox.Show("���� �Է��ϼ���.");
                return;
            }

            //���� �� ���̸� ����
            if (txtQuatity.Text == "")
            {
                MessageBox.Show("������ �Է��ϼ���.");
                return;
            }



            if (cboType.Text == "�ż�")
            {

                //�̸� �� ���̸� ����
                if (cboStoSearch.Text.Replace(" ", "") == "")
                {
                    MessageBox.Show("������� �Է��ϼ���.");
                    return;
                }

                if (mDTHold.Tables[2].Select("Company = '" + cboStoSearch.Text + "'").Length < 1)
                {
                    MessageBox.Show("�߸��� �����Դϴ�.");
                    return;
                }
                   
                else if (mDTHold.Tables[2].Select("Company = '" +  cboStoSearch.Text + "'").Length > 1)
                {
                    MessageBox.Show("Code�� �ߺ��� �̸��� ������ �ֽ��ϴ�.");
                    return;
                }

                strSQL = @"
                           INSERT INTO Buy (Date, Name, Price, Quantity, Left)
                           VALUES ( '" + dte.Value.ToString("yyyyMMdd") + @"'
                                    , '" + cboStoSearch.Text.ToUpper() + @"'
                                    , '" + txtPrice.Text + @"'
                                    , '" + txtQuatity.Text + @"'
                                    , '" + txtQuatity.Text + @"'
                                  )";
            }

            else if (cboType.Text == "�ŵ�")
            {
                //�̸� �� ���̸� ����
                if (cboName.Text.Replace(" ", "") == "")
                {
                    MessageBox.Show("������� �Է��ϼ���.");
                    return;
                }

                int intSellQ;
                int i = 0; //�ݺ�������
                int intLeftQ;    //���� ����
                int intLeftP;   //���� ����
                DataRow[] dr;

                intSellQ = Convert.ToInt32(txtQuatity.Text);

                //�̸� üũ
                dr = mDTHold.Tables[0].Select("����� = '" + cboName.Text.ToUpper() + "'");
                if (dr.Length != 1)
                {
                    MessageBox.Show("�ش��ϴ� ������ �ż��� ���� �����ϴ�.");
                    return;
                }

                if (Convert.ToInt32(dr[0]["����"].ToString()) < Convert.ToInt32(txtQuatity.Text))
                {
                    MessageBox.Show("�ż��� �������� �����ϴ�.");
                    return;
                }

                strSQL += @"
                            INSERT INTO Sell (Date, Name, Price, Quantity)
                            VALUES ( '" + dte.Value.ToString("yyyyMMdd") + @"'
                                     , '" + cboName.Text.ToUpper() + @"'
                                     , '" + txtPrice.Text + @"'
                                     , '" + txtQuatity.Text + @"'
                                   );
                         ";

                while (intSellQ > 0)
                {
                    dr = mDTHold.Tables[1].Select("Left <> 0 AND Name = '" + cboName.Text.ToUpper() + "'", "Seq ASC");
                    intLeftQ = Convert.ToInt32(dr[i]["Left"].ToString());
                    intLeftP = Convert.ToInt32(dr[i]["Price"].ToString());

                    if (intSellQ >= intLeftQ)
                    {
                        strSQL += @"UPDATE Buy
                                       SET Left = 0
                                     WHERE Seq = '" + dr[i]["Seq"].ToString() + @"';
                                   ";

                        strSQL += @"INSERT INTO Revenue (Date, Name, BPrice, SPrice, Quantity, SellSeq)
                                    VALUES ( '" + dte.Value.ToString("yyyyMMdd") + @"'
                                             , '" + cboName.Text.ToUpper() + @"'
                                             , '" + intLeftP + @"'
                                             , '" + txtPrice.Text + @"'
                                             , '" + intLeftQ + @"'
                                             , CASE WHEN (SELECT COUNT(*) FROM Sell) = 0
                                                    THEN 1
                                                    ELSE (SELECT MAX(Seq) FROM Sell) END
                                           );
                                   ";
                        intSellQ = intSellQ - intLeftQ;
                    }

                    else
                    {
                        strSQL += @"UPDATE Buy
                                       SET Left = " + (intLeftQ - intSellQ) + @"
                                     WHERE Seq = '" + dr[i]["Seq"].ToString() + @"';
                                   ";

                        strSQL += @"INSERT INTO Revenue (Date, Name, BPrice, SPrice, Quantity, SellSeq)
                                    VALUES ( '" + dte.Value.ToString("yyyyMMdd") + @"'
                                             , '" + cboName.Text.ToUpper() + @"'
                                             , '" + intLeftP + @"'
                                             , '" + txtPrice.Text + @"'
                                             , '" + intSellQ + @"'
                                             , CASE WHEN (SELECT COUNT(*) FROM Sell) = 0
                                                    THEN 1
                                                    ELSE (SELECT MAX(Seq) FROM Sell) END
                                            );
                                   ";

                        intSellQ = 0;
                    }

                    i++;
                }
            }

            int iresult = Network.ExecDB(strSQL);

            if (iresult > 0)
            {
                MessageBox.Show("����Ǿ����ϴ�.");
            }
            else
            {
                MessageBox.Show("���忡 �����Ͽ����ϴ�.");
            }

            txtPrice.Clear();
            cboStoSearch.Text = "";
            txtQuatity.Clear();
        }
        #endregion �Է� �Լ�_End

        #region ���纸���� SELECT

        /// <summary>
        /// ���纸���� SELECT
        /// </summary>
        private void fnHoldSearch()
        {
            string strSQL = "";
            mDTHold = new DataSet();

            strSQL = @"SELECT MAX(Name) AS �����
                            , SUM(Left * Price) / SUM(Left) AS ��հ�
                            , SUM(Left) AS ���� 
                            , SUM(Left * Price) AS �հ�  
                         FROM Buy
                        WHERE Left > 0
                        GROUP BY Name;";
            
            strSQL += @"SELECT * 
                          FROM Buy;";

            strSQL += @"SELECT *
                          FROM Code;";

            mDTHold = Network.GetDBSet(strSQL);

            //dgvHold.DataSource = mDTHold.Tables[0];

            //�÷��߰�
            //fnColumnAdd(dgvHold);

            // �÷�����
            //fnSortGrid(dgvHold, true);

            //�ŵ� �޺� ������ �ʱ�ȭ
            cboName.Items.Clear();
            foreach (DataRow dr in mDTHold.Tables[0].Rows)
            {
                if (!cboName.Items.Contains(dr["�����"].ToString()))
                {
                    cboName.Items.Add(dr["�����"].ToString());
                }
            }
        }

        #endregion ���纸���� SELECT_End


        //*****************�̺�Ʈ******************//

        #region �ŷ�����
        private void cboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboType.Text == "�ż�")
            {
                cboStoSearch.Visible = true;
                cboName.Visible = false;
            }
            else if (cboType.Text == "�ŵ�")
            {
                cboName.Visible = true;
                cboStoSearch.Visible = false;
            }
        }
        #endregion �ŷ�����

        #region �����
        private void cboName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboType.Text == "�ż�")
            {
                cboStoSearch.Visible = true;
                cboName.Visible = false;
            }
            else if (cboType.Text == "�ŵ�")
            {
                cboName.Visible = true;
                cboStoSearch.Visible = false;
            }
        }
        #endregion �����_End

        #region �Է¹�ư
        private void btnInput_Click(object sender, EventArgs e)
        {
            efnDeal();
            fnHoldSearch();
        }
        #endregion �Է¹�ư_End

        #region ����_���ڸ� �Էµǵ��� ���͸�
        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))    //���ڿ� �齺���̽��� ������ �������� �ٷ� ó��
            {
                e.Handled = true;
            }
        }
        #endregion ����_���ڸ� �Էµǵ��� ���͸�_End

        #region ����_���ڸ� �Էµǵ��� ���͸�
        private void txtQuatity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))    //���ڿ� �齺���̽��� ������ �������� �ٷ� ó��
            {
                e.Handled = true;
            }
        }
        #endregion ����_���ڸ� �Էµǵ��� ���͸�_End

        #region �޺��ڽ� �б��������� �����
        private void cboType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Down && e.KeyCode != Keys.Up)
            {
                e.SuppressKeyPress = true;
            }
        }
        #endregion �޺��ڽ� �б��������� �����_End


    }
}