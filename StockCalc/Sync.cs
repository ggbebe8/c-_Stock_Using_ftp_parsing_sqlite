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
        string mAddress, mID;   //복호화를 위한 변수.

        public Sync(string p_address, string p_id)
        {
            InitializeComponent();

            mAddress = p_address;
            mID = p_id;
        }



        #region 이벤트
        /// <summary>
        /// 복호화 확인 버튼
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
                MessageBox.Show("키가 옳바르지 않음");
            }
        }

        /// <summary>
        /// 저장버튼
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
                MessageBox.Show("저장되었습니다. 재시작하세요");
            }
            else
            {
                MessageBox.Show("저장에 실패하였습니다.");
            }
        }

        /// <summary>
        /// 닫기버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion 이벤트_End
    }
}