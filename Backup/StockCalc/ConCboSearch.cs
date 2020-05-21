using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace StockCalc
{
    public partial class ConCboSearch : UserControl
    {
        public ConCboSearch()
        {
            InitializeComponent();
            mfnBindingCbo(mfnGetStock());
        }

        private DataTable mfnGetStock()
        {
            string strQuery = "";
            strQuery += "\r\n" + "SELECT Company FROM Code";

            return Network.GetDBTable(strQuery);
        }

        private void mfnBindingCbo(DataTable p_Dt)
        {
            foreach (DataRow dr in p_Dt.Rows)
            {
                cboStoSearch.Items.Add(dr["Company"].ToString());
            }
        }
    }
}
