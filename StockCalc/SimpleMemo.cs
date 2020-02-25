using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace StockCalc
{
    public partial class SimpleMemo : Form
    {
        string mCodeNum = "";
        bool mIsContents = false;

        public SimpleMemo(string p_CodeNum, string p_StockName)
        {
            InitializeComponent();
            mCodeNum = p_CodeNum;
            fnInitCon();
            this.Text = p_StockName;
        }


        private void fnInitCon()
        {
            DataTable dt = Network.GetDBTable("SELECT Contents FROM SimpleMemo WHERE CodeNum = '" + mCodeNum + "'");

            if (dt.Rows.Count > 0)
            {
                txtMemo.Text = dt.Rows[0]["Contents"].ToString(); 
                mIsContents = true;
            }
        }

        private bool fnSave()
        {
            int iResult = 0;
            if (mIsContents)
            {
                iResult = Network.ExecDB("UPDATE SimpleMemo SET Contents = '" + txtMemo.Text + "' WHERE CodeNum = '" + mCodeNum + "'");
            }
            else
            {
                iResult = Network.ExecDB("INSERT INTO SimpleMemo (Contents, CodeNum) VALUES ('" + txtMemo.Text + "', '" + mCodeNum + "')");
            }
            return iResult > 0 ? true : false;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (fnSave())
                MessageBox.Show("저장 성공", "저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("저장 실패", "저장", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnSaveClose_Click(object sender, EventArgs e)
        {
            if (fnSave())
            {
                MessageBox.Show("저장 성공", "저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
                MessageBox.Show("저장 실패", "저장", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}