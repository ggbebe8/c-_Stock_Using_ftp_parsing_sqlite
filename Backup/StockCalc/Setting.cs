using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace StockCalc
{
    public partial class Setting : Form
    {
        string mAddress = "";
        string mID = "";

        public Setting(string p_address, string p_id)
        {
            InitializeComponent();
            this.mAddress = p_address;
            this.mID = p_id;

        }

        private void btnCode_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            txtPassWd.Visible = false;
            this.btnSync.Size = new System.Drawing.Size(63, 21);

            Code CO = new Code();
            CO.StartPosition = FormStartPosition.CenterParent;
            CO.ShowDialog();
        }

        private void btnSync_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            txtPassWd.Visible = false;

            Sync SY = new Sync(mAddress, mID);
            SY.StartPosition = FormStartPosition.CenterParent;
            SY.ShowDialog();
        }

        private void btnSql_Click(object sender, EventArgs e)
        {
            label1.Visible = true;
            txtPassWd.Visible = true;
        }
    }
}