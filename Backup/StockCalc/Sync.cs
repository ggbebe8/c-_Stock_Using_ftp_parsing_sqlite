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
    public partial class Sync : Form
    {
        string mAddress, mID;   //��ȣȭ�� ���� ����.

        public Sync(string p_address, string p_id)
        {
            InitializeComponent();

            mAddress = p_address;
            mID = p_id;
        }



        #region �̺�Ʈ
        /// <summary>
        /// ��ȣȭ Ȯ�� ��ư
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtDecKey.Text == "angel")
            {
                txtAddress.Text = Network.Decrypt(mAddress);
                txtID.Text = Network.Decrypt(mID);
            }
            else
            {
                MessageBox.Show("Ű�� �ǹٸ��� ����");
            }
        }

        /// <summary>
        /// �����ư
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            string strSql = "";

            strSql = @"
                        DELETE FROM FTP;";
            strSql += @"
                        INSERT INTO FTP ( 'IP', 'ID', 'PW')
                        VALUES ( '" + Network.Encrypt(txtAddress.Text) + @"',
                                 '" + Network.Encrypt(txtID.Text) + @"',
                                 '" + Network.Encrypt(txtPassWd.Text) + @"' );";

            int iresult = Network.ExecDB(strSql);

            if (iresult > 0)
            {
                MessageBox.Show("����Ǿ����ϴ�. ������ϼ���");
            }
            else
            {
                MessageBox.Show("���忡 �����Ͽ����ϴ�.");
            }
        }

        /// <summary>
        /// �ݱ��ư
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion �̺�Ʈ_End
    }
}