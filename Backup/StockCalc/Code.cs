using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SQLite;

/* �������� �ڵ�ֱ�
 * string connectString =
                "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=d:\\testit.xlsx;Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1;\"";
            OleDbConnection conn = new OleDbConnection(connectString);
            OleDbDataAdapter da = new OleDbDataAdapter("Select * From [Sheet1$]", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
 * 
 * 
SQLiteCommand sqlcmd;
SQLiteConnection con;
con = new SQLiteConnection(@"Data Source=.\Contents.db");
con.Open();

sqlcmd = con.CreateCommand();
sqlcmd.CommandText = string.Format(@"INSERT INTO Code ( Company, CodeNum) 
                                     SELECT * FROM {0}",dt.TableName);
SQLiteDataAdapter ap = new SQLiteDataAdapter(sqlcmd);
*/



namespace StockCalc
{
    public partial class Code : Form
    {
       

        public Code()
        {
            InitializeComponent();

            //����б�
            fnReadDB();
        }
        /*
        private void fnImportEx()
        {
            string connectString =
                "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + System.Environment.CurrentDirectory + "\\StockCode.xlsx; Mode=ReadWrite;Extended Properties=\"Excel 12.0;HDR=YES;\";";
            OleDbConnection connect = new OleDbConnection(connectString);
            OleDbDataAdapter da = new OleDbDataAdapter("Select [ȸ���] AS Company, [�����ڵ�] AS CodeNum From [�ڽ���$]", connect);
            DataTable dt = new DataTable();
            da.Fill(dt);

            string strSQL = "";

            for (int i = 1500; i < dt.Rows.Count; i++)
            {
                strSQL += @"
                            INSERT INTO Code (Company, CodeNum)
                            VALUES ('" + dt.Rows[i]["Company"] + "', '" + dt.Rows[i]["CodeNum"] + "');";
            }

            SQLiteCommand sqlcmd;
            sqlcmd = conn.CreateCommand();
            sqlcmd.CommandText = string.Format(strSQL);
            SQLiteDataAdapter ap = new SQLiteDataAdapter(sqlcmd);
            ap.Fill(dt);
        }
        */
        private void fnReadDB()
        {

            string strSQL = "";

            strSQL = @"SELECT * 
                         FROM Code";
            if(txtName.Text.Replace(" ","") != "")
            {
                strSQL += @"
                        WHERE Company LIKE '%" + txtName.Text + "%' OR CodeNum LIKE '%" + txtName.Text + "%'";
            }

            dataGridView1.DataSource = Network.GetDBTable(strSQL);
        }

        private void efnDel()
        {

            string strSQL = "";
            if (txtDelName.Text.Replace(" ", "") == "")
            {
                MessageBox.Show("�Է����ּ���");
                return;
            }
            strSQL = @"
                        DELETE 
                         FROM Code";
            strSQL += @"
                        WHERE Company = '" + txtDelName.Text + "' OR CodeNum = '" + txtDelName.Text + "'";

            int iresult = Network.ExecDB(strSQL);

            if (iresult > 0)
            {
                MessageBox.Show("��������");
            }
            else
            {
                MessageBox.Show("��������");
            }
            txtDelName.Clear();
        }

        private void efnInsert()
        {

            string strSQL = "";

            if (txtInsertName.Text.Replace(" ", "") == "" || txtInsertCode.Text.Replace(" ", "") == "")
            {
                MessageBox.Show("����� �Է����ּ���");
                return;
            }
            strSQL = @"
                        INSERT INTO Code ( Company, CodeNum)
                        VALUES ('" + txtInsertName.Text + "', '" + txtInsertCode.Text + "');";

            int iresult = Network.ExecDB(strSQL);

            if (iresult > 0)
            {
                MessageBox.Show("�Է¼���");
            }
            else
            {
                MessageBox.Show("�Է½���");
            }
            txtInsertCode.Clear();
            txtInsertCode.Clear();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            fnReadDB();
        }

        private void Code_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            efnDel();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            efnInsert();
        }
    }
}