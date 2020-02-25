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
    public partial class Memo : Form
    {
        string mType = "";
        string mSeq = "";
        string mMemo = "";

        public Memo(string p_Type, string p_Seq, string p_Memo)
        {
            InitializeComponent();
            mType = p_Type;
            mSeq = p_Seq;
            mMemo = p_Memo;

            txtMemo.Text = mMemo;


        }

        private void fnSave(string p_Type, string p_Seq)
        {

            string strSQL = @"
                                UPDATE " + p_Type + @"
                                   SET Memo = '" + txtMemo.Text + @"'
                                 WHERE Seq = '" + p_Seq + @"';
                            ";

            int iresult = Network.ExecDB(strSQL);

            if (iresult > 0)
            {
                MessageBox.Show("저장 성공");
            }
            else
            {
                MessageBox.Show("저장 실패");
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            fnSave(mType == "매수" ? "Buy" : "Sell" ,mSeq);
        }

        private void btnSaveClose_Click(object sender, EventArgs e)
        {
            fnSave(mType == "매수" ? "Buy" : "Sell", mSeq);
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Memo_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}