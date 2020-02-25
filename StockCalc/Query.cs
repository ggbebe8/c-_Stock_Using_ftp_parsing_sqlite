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
    public partial class Query : Form
    {
        public Query()
        {
            InitializeComponent();
        }

        private void fnQuery()
        {
            string strSQL = txtQuery.Text;

            try
            {
                dteShow.DataSource = Network.GetDBTable(strSQL);
                MessageBox.Show("Äõ¸®¼º°ø");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void txtQuery_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
                fnQuery();
            else if (e.KeyCode == Keys.F4)
                txtQuery.Clear();
        }

    }
}