using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace StockCalc
{
    public partial class Option : Form
    {
        public Option()
        {
            InitializeComponent();
        }

        private void fnSave()
        {
            Network.WriteIniFile("OPTION", "SyncYN", chkSync.Checked ? "Y" : "N", @".\setting.ini");
            Network.WriteIniFile("OPTION", "FirstScreen", rdoCurrent.Checked ? "C" : rdoInterest.Checked ? "I" : "M", @".\setting.ini");
        }

        private void fnInitCon()
        {
            chkSync.Checked = Network.ReadIniFile("OPTION", "SyncYN", @".\setting.ini") == "N" ? false : true;
            string strFirstScreen = Network.ReadIniFile("OPTION", "FirstScreen", @".\setting.ini");
            if (strFirstScreen == "C")
                rdoCurrent.Checked = true;
            else if (strFirstScreen == "I")
                rdoInterest.Checked = true;
            else
                rdoMainMemo.Checked = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            
            MessageBox.Show("저장 되었습니다.");
        }

        private void btnSaveClose_Click(object sender, EventArgs e)
        {
            fnSave();
            MessageBox.Show("저장 되었습니다.");
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Option_Load(object sender, EventArgs e)
        {
            fnInitCon();
        }
    }
}