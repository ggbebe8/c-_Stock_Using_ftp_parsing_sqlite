using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;



/* �߰��Ǿ����� �ϴ� ���
 * 1. FTP On Or Off (o)
 * 2. ���Ǳ��
 * 3. ���� ��ü����
 * 4. ������Ŭ���ϸ� report�� ����, �ڼ��� ������ ���� ����
 */

/* �����Ǵ� �̽�
 * ��¥:�ذῩ��:ū����:�󼼳���
 * 20181030:O:Sync����:
 *  - 1.����1���� ������ 2.Ŭ�� ������ ��, ���ε� ���þ��� 3. �ٽ� ����1 ���� �� ���� 4. ����1�� �ֽ��ӿ���, ���ε� �϶�� �޼��� �ȶ�
 *  - ������ �������� ��츸(hash�� ����� ���) ���ε带 Ž. ���� �������� �ʾұ⿡ ���ε� �������� ������. 
 *  - �ܼ� ��¥�� ������ ���� ���� ���°�, ������ �� �� ������� ���, �������� ����1�� ���ε带 �ϸ� �Ȱ��� �����ӿ��� ������ �ֽ��̱⿡ ����2���� �� �ٿ�
 *  - Ȥ�� �ٿ� ������ ������ ��¥�� ����ǰ�, ������ �� ������ ��¥�� �ֽ��̱⿡ �������� ���������� �� ���ε� �޼����� ��
 *  - �� ��¥�� ������ ���ϸ� �ȵ�
 *  - ���濩��(hash)�� ������ setting.ini�� �������� �����ؾ� ��. 
 *  - ���������� Sync�� ����������, �ű⿡ ���� �ΰ������� ����ص�. ���� ��� ���ε� �ð� ���� �͵� ���� �����. 
 *  - ���ư����� �ʹݿ� ftp ������ ���ϰ� �������� ���, D��� �Ķ���͸� �����صдٴ��� �ϰ� �����صδ� �͵� ������ �ʰ���?
 * 
 * 20181102:O:Sync����:
 *  - 1. ���������� ������ ini���� TouchFileDate���� ����. ������ ����Ǹ� ������ �� ���� ���
 *  - 2. ��������� ����. ������ ���ε� ������ ������ ����Ǿ��� ���! �� ����. �� �ٷ� ���ε� ���ϰ� ������ ���ε� �Ϸ��� �ϸ�, ������
 *  -    �������� �ʾ��� ���� ���ε� �϶�� �޼����� ���� �ʴ°���!!
 * 
 * 
 * */


/* �� 1
 * �÷� �߰��ϱ�
ALTER TABLE ���̺��
ADD �߰��� �÷���  ������ ����;

 */

/* �� 2
 * �÷� �����ϱ�
 * -sqlite�� �÷������� �������� �����Ƿ� �÷��� ������ ��, ����, �׸��� �̸��� �����Ѵ�. 
BEGIN TRANSACTION;
CREATE TABLE �ӽ� (
	'InterName' TEXT,
	'Company' TEXT ,
	'CodeNum' TEXT,
	'DisSeq' TEXT,
	PRIMARY KEY(InterName,Company)
);
INSERT INTO �ӽ� (Company, CodeNum) 
SELECT Company, CodeNum FROM ����;
DROP TABLE ����;
ALTER TABLE �ӽ� RENAME TO ����;
COMMIT;

���� ���̺�� ���� ���� ���̺��� �÷��� ���ٸ�, �Ʒ��� ���� ����� �� �ִ�.
INSERT INTO table_list_new SELECT no, round, groups, number FROM table_list; 
*/

/*���̺� ����
 * �ż� ���̺�
CREATE TABLE Buy   (
                       'Seq' INTEGER PRIMARY KEY,
                       'Date' TEXT NOT NULL,
                       'Name' TEXT NOT NULL,
                       'Price' INT NOT NULL,
                       'Quantity' INT NOT NULL,
                       'Left' INT NOT NULL      -- ���� ����
                     )
 * �ŵ� ���̺�
CREATE TABLE Sell   (
                       'Seq' INTEGER PRIMARY KEY,
                       'Date' TEXT NOT NULL,
                       'Name' TEXT NOT NULL,
                       'Price' INT NOT NULL,
                       'Quantity' INT NOT NULL
                     )
 * ���� ���̺�
CREATE TABLE Revenue (
                       'Seq' INTEGER PRIMARY KEY,
                       'Date' TEXT NOT NULL,
                       'Name' TEXT NOT NULL,
                       'BPrice' INT NOT NULL,
                       'SPrice' INT NOT NULL,
                       'Quantity' INT NOT NULL,
                       'SellSeq' INTEGER
                     )
 * �ֽ��ڵ� ���̺�
CREATE TABLE Interest (
	                    'InterName' TEXT,
	                    'Company' TEXT ,
	                    'CodeNum' TEXT,
	                    'DisSeq' INT,
	                     PRIMARY KEY(InterName,Company)
                      );
 * �������� ���̺�
 CREATE TABLE Interest (
                       'Company' TEXT NOT NULL PRIMARY KEY,
                       'CodeNum' TEXT NOT NULL
                     )
 * �������� ����ȭ�� ���� ftp ������ ���� ���̺�(�ο� ������ 1)
 CREATE TABLE FTP ( 
                  'IP' TEXT NOT NULL,
                  'ID' TEXT NOT NULL,
                  'PW' TEXT NOT NULL
                  )
 
 
 * ���̺� ����Ʈ ���� 
SELECT name 
  FROM sqlite_master 
 WHERE type IN ('table', 'view') 
   AND name NOT LIKE 'sqlite_%' 
UNION ALL 
SELECT name 
  FROM sqlite_temp_master 
 WHERE type IN ('table', 'view') 
 ORDER BY 1;

 * ���̺��ʱ�ȭ
DELETE FROM Sell;
DELETE FROM Interest;
DELETE FROM Memo;
DELETE FROM Revenue;
DELETE FROM Buy;
 
 * ���� ���̺�
buy 
sell 
Interest
Memo
Revenue
code
ftp
InitMemoOpt

 *���ܸ޸� ���̺� 
 CREATE TABLE SimpleMemo (
	                        'CodeNum' TEXT,
	                        'Contents' TEXT,
	                        PRIMARY KEY(CodeNum)
                         );
*/


// -- �� �ӿ� �� �ֱ� http://pcsak3.com/455

namespace StockCalc
{
    public partial class MdiParents : Form
    {
        #region ��������

        string mAddress = "";    //FTP �ּ�

        string mID = "";         //FTP ID

        string mPW = "";         //FTP PW

        string mMD5 = "";       //������ �ؽ��� �� ������. (�����ߴ��� ���θ� �ľ��ϱ� ���ؼ�)

        DateTime mServerVerDate;    //���� ���� ��¥ ����

        bool mFtpCon = false;

        bool mSyncYN = true;        //Sync����� ����� �������� ���� ����

        ViewStock VS = new ViewStock();     //��ü�����ϱ�

        bool mIsPanel1Visible = true;           //panel.visible�� �ʱ�ȭ�� �� false�θ� �Ѿ�� �̤� ������������ ���������. 

        #endregion ��������_End

        [DllImport("user32.dll")]   //����Ű ���
        private static extern int RegisterHotKey(int hwnd, int id, int fsModifiers, int vk);


        //��Ű����
        [DllImport("user32.dll")]
        private static extern int UnregisterHotKey(int hwnd, int id);

        #region ������
        public MdiParents()
        {
            InitializeComponent();

            //�޺��ڽ��ʱ�ȭ
            fnInitComb();

            //�гο� �� �ø���
            fnOpenChild((Form)VS);

            mIsPanel1Visible = Convert.ToString(Network.ReadIniFile("btnVisible","YN",@".\setting.ini")) == "Y" ? true : false;

        }

        private void StockCalc_Load(object sender, EventArgs e)
        {
            //��Ű ���
            RegisterHotKey((int)this.Handle, 0, 1 , (int)Keys.Z); //Alt+Z
            RegisterHotKey((int)this.Handle, 1, 1, (int)Keys.Q);  //Alt+Q  // ���α׷� �� ��, ��������� ��. 

            //���α׷��� �̹� ����Ǿ� ������ ���̱�
            if (fnKillProcess())
                return;
            
            //�������� �ʱ�ȭ�ϱ�.(FTP,Option)
            fnInitVar();

            
            //FTP ����ȭ
            if (mSyncYN)
            {
                fnFTPDownSync();
            }

            //���� ����ȭ ��, Contents.db�� ���� �ؽ����� ������. 
            mMD5 = Network.GetMD5(@".\Contents.db");
        }

        #endregion ������_End

        #region �����Լ�

        //�޺��ڽ� �ʱ�ȭ
        private void fnInitComb()
        {
            cboDB.SelectedIndexChanged -= new System.EventHandler(cboDB_SelectedIndexChanged);
            cboDB.Items.Clear();
            cboDB.Items.Add("R");
            cboDB.Items.Add("V");
            cboDB.SelectedItem = "R";
            cboDB.SelectedIndexChanged += new System.EventHandler(cboDB_SelectedIndexChanged);
        }

        // �������� �ʱ�ȭ �ϱ�(IP,ID,PW,Option)
        private void fnInitVar()
        {
            //��񿬰�
            string strResult = String.Empty;
            string strSQL = String.Empty;
            bool isSync = true;

           

            strSQL = @"
                        SELECT IP AS IP
                             , ID AS ID                             
                             , PW AS PW
                          FROM FTP;";

            //FTP�� �ϵ��ڵ�����! V�� �����͹����� �ȵǴϱ�!
            SQLiteConnection Conn = new SQLiteConnection(@"Data Source = .\Contents.db");
            DataSet ds = new DataSet();
            Conn.Open();
            SQLiteDataAdapter Ap = new SQLiteDataAdapter(strSQL, Conn);
            Ap.Fill(ds);
            Ap.Dispose();
            Conn.Dispose();


            if (ds.Tables[0].Rows.Count == 0)
            {
                mAddress = "";
                mID = "";
                mPW = "";
            }

            else
            {
                mAddress = ds.Tables[0].Rows[0]["IP"].ToString();
                mID = ds.Tables[0].Rows[0]["ID"].ToString();
                mPW = ds.Tables[0].Rows[0]["PW"].ToString();
            }

            if (strResult != "")
            {
                MessageBox.Show("����db�� �������� �� �����߻� : \r\n" + strResult);
            }

            //�ɼǰ�������(SyncYN)
            isSync = Network.ReadIniFile("OPTION", "SyncYN", @".\setting.ini") == "N" ? false : true;
            mSyncYN = isSync;

        }

        /// ���α׷� ����Ǿ� ���� ��� ����. 
        /// <returns>true : ������</returns>
        private bool fnKillProcess()
        {
            Process[] p = Process.GetProcessesByName("StockCalc");

            if (p.GetLength(0) > 1)
            {
                MessageBox.Show("���α׷��� �̹� ���� �� �Դϴ�.");
                Application.Exit();         //�̰ɷ� ����� �ǳ� Load�̺�Ʈ�� ������ Ž. ���� return������. 
                return true;
            }
            return false;
        }


        /// �гο� �� �ø���
        private void fnOpenChild(Form p_childForm)
        {
            panel4.Controls.Clear();

            p_childForm.TopLevel = false;

            this.Controls.Add(p_childForm);

            p_childForm.Parent = this.panel4;

            p_childForm.Text = "";

            p_childForm.ControlBox = false;

            p_childForm.Show();

            p_childForm.Dock = DockStyle.Fill;
        }

        /// �޴����� �ݱ�
        private void efnOpenMenu(bool p_isOpen)
        {
            if (p_isOpen)
            {
                panel1.Visible = false;
                mIsPanel1Visible = false;
                panel4.Size = new System.Drawing.Size(panel4.Size.Width, panel4.Size.Height + 27);
            }
            else
            {
                panel1.Visible = true;
                mIsPanel1Visible = true;
                panel4.Size = new System.Drawing.Size(panel4.Size.Width, panel4.Size.Height - 27);
                panel1.Location = new System.Drawing.Point(panel4.Location.X + 2, panel4.Location.Y + panel4.Size.Height);
            }
        }
        #endregion �����Լ�_End

        #region FTP Sync ����

        /// ������ ���ؼ� �� �����̸� �ٿ�.
        private void fnFTPDownSync()
        { 
            //���� ���ð� ������ ���ε� ��¥�� ��. ������ ���ε� ��¥�� �� ũ�� �ٿ�
            DateTime dtLocalFileDate;
            mServerVerDate = fnReadVer(out mFtpCon);

            //ftp ������ �ȵǾ����� ��� ����������.
            if (!mFtpCon)
                return;

            try
            {
                dtLocalFileDate = DateTime.ParseExact(Convert.ToString(Network.ReadIniFile("FileTouchDate", "Date", @".\setting.ini")), "yyyyMMddHHmmss", null);
            }
            catch
            {
                dtLocalFileDate = new DateTime(1,1,1);
            }

            if (dtLocalFileDate < mServerVerDate)
            {
                if (MessageBox.Show("������ ������ �ֽ��Դϴ�.\r\n�ֽŹ������� ����ȭ �Ͻðڽ��ϱ�?", "���� ����ȭ", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    //���� ������ ���.
                    try
                    {
                        File.Copy(@".\Contents.db", @".\Contents_Bak.db", true);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("���� ��� �� ���� : " + e.ToString());
                    }

                    string strResult = Network.FtpDown(mAddress, mID, mPW, "Contents.db");
                    if (strResult != "")
                    {
                        MessageBox.Show(strResult);
                    }

                    //����ȭ�� �������� ���, �ٿ� ���� ��¥�� ������ setting.ini�� �����صд�. upload�ÿ� ���Ͽ� ����ȭ���θ� �����ϴµ� ����Ѵ�.
                    else
                    {
                        Network.WriteIniFile("FileTouchDate", "Date", mServerVerDate.ToString("yyyyMMddHHmmss"), @".\setting.ini");
                    }
                }
            }
        }

        /// ������ ���ؼ� ������ �ֽ��̸� ���ε�. ���α׷� ���� ��.
        private void fnFTPUpSync()
        {

            //������ ���������� ��¥�� ���
            if (mMD5 != Network.GetMD5(@".\Contents.db"))
                Network.WriteIniFile("FileTouchDate", "Date", fnGetFileDate(@".\Contents.db").ToString("yyyyMMddHHmmss"), @".\setting.ini");

            //ftp������ �ȵǾ����� ��� ����������. 
            if (!mFtpCon)
                return;

            DateTime dtTouchDate = DateTime.ParseExact(Convert.ToString(Network.ReadIniFile("FileTouchDate", "Date", @".\setting.ini")), "yyyyMMddHHmmss", null);

            //ó�� ���ϳ�¥�� ���ؼ� ������ ���� ��� 
            //&& ������ ��¥���� �� �ֽ��� ���(�ٸ� Ŭ�� ���ε��� ��쵵 �����Ƿ� üũ�Ѵ�) ���ε� �Ѵ�. 
            if (dtTouchDate > mServerVerDate || mServerVerDate.Year == 1)
            {
                if (MessageBox.Show("������ ������ �ֽ��Դϴ�.\r\n������ ���ε� �Ͻðڽ��ϱ�?", "���� ����ȭ", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {

                    /* ���ε� �� ������ ��� */
                    // 3�� ������ �����, 2�� ������ 3����, 1�� ������ 2��, ������ ������ 1��
                    string strRe = "";

                    strRe = Network.FtpDel(mAddress, mID, mPW, "Contents_Bak3.db");

                    if (strRe != "")
                    {
                        MessageBox.Show(strRe);
                        MessageBox.Show("���ε� ����");
                        return;
                    }

                    strRe = Network.FtpRename(mAddress, mID, mPW, "Contents_Bak2.db", "Contents_Bak3.db");

                    if (strRe != "")
                    {
                        MessageBox.Show(strRe);
                        MessageBox.Show("���ε� ����");
                        return;
                    }

                    strRe = Network.FtpRename(mAddress, mID, mPW, "Contents_Bak1.db", "Contents_Bak2.db");

                    if (strRe != "")
                    {
                        MessageBox.Show(strRe);
                        MessageBox.Show("���ε� ����");
                        return;
                    }

                    strRe = Network.FtpRename(mAddress, mID, mPW, "Contents.db", "Contents_Bak1.db");

                    if (strRe != "")
                    {
                        MessageBox.Show(strRe);
                        MessageBox.Show("���ε� ����");
                        return;
                    }

                    string strResult = Network.FtpUp(mAddress, mID, mPW, "Contents.db");
                    if (strResult != "")
                    {
                        MessageBox.Show(strResult);
                    }

                    //����ȭ�� �������� ��� ������ ��¥ �ø�
                    else
                    {
                        fnWriteVer(dtTouchDate.ToString("yyyyMMddHHmmss"));
                        strResult = Network.FtpUp(mAddress, mID, mPW, "ver.ps");
                        if (strResult != "")
                        {
                            MessageBox.Show("ver.ps ���ε� ���� : " + strResult);
                        }
                    }
                }
            }
        }


        #endregion FTP Sync ����_End

        #region ���� �а� ����, ������ ��¥
        /// ver.ps ���� �б�. [0]: ���ε� [1]: �ٿ�ε�
        /// <returns>���� ���ε� ��¥</returns>
        private DateTime fnReadVer(out bool ftpConnect)
        {
            //���� �߰��ϱ� . 1 : ������ ���� ��  2 : �� ª�� �� 3 : ���ٸ� �������� ���̷� üũ����. 
            string strDate = "";
            DateTime dtFileDate = new DateTime();
            string strResult = "";
            // ??�̰� �� �س���?? �� �濡 ���� �ȵǴ� ��� ������ �̷��� �ص׳�?
            //for (int i = 0; i < 5; i++)
            //{
                strResult = Network.FtpDown(mAddress, mID, mPW, "ver.ps");
                
                //if (strResult == "")
                    //break;


            //}

            if (strResult == "")    //������ �ִ� �������� �ٿ�
            {
                try
                {
                    StreamReader objReader = new StreamReader(@".\ver.ps");
                    strDate = objReader.ReadLine().Trim();
                    objReader.Close();

                    dtFileDate = DateTime.ParseExact(strDate, "yyyyMMddHHmmss", null);

                    if (dtFileDate > DateTime.Now)
                        dtFileDate = new DateTime(1, 1, 1);
                    ftpConnect = true;
                }
                catch
                {
                    ftpConnect = false;
                }
            }
            else
            {
                MessageBox.Show("FTP ���� ���� : " + strResult);
                ftpConnect = false;
            }

            return dtFileDate;
        }


        /// ver.ps ���� ����. [0]: ���ε� [1]: �ٿ�ε�
        private bool fnWriteVer(string p_strDate)
        {
            //���⵵ �����߰�����. ���ٸ� ��������.
            try
            {
                StreamWriter objWriter = new StreamWriter(@".\ver.ps");
                objWriter.Write(p_strDate);
                objWriter.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// ������ ��¥ ���ϴ� �Լ�.
        /// <param name="p_path">���ϴ� ������ ���</param>
        /// <returns>���� ��¥</returns>
        private DateTime fnGetFileDate(string p_path)
        {
            DateTime dtFileDate = new DateTime();
            try
            {
                FileInfo file = new FileInfo(p_path);

                //���� DateTime�� string���� ���� ��, �ٽ� �����ϴ� ������
                //�� �������� �ۿ� �ʿ� ���µ�, �������� �������� ����Ǿ� 
                //���ε� �ϴ� �������� �߸��� �񱳸� �Ѵ� 
                dtFileDate = DateTime.ParseExact(file.LastWriteTime.ToString("yyyyMMddHHmmss"), "yyyyMMddHHmmss", null);
            }
            catch
            {
            }
            return dtFileDate;
        }
        #endregion ���� �а� ����_END

        #region �̺�Ʈ ���� �Լ�


        //��Ű ��ɵ�� (����Ű)
        protected override void WndProc(ref Message m) //���������ν��� �ݹ��Լ�
        {
            base.WndProc(ref m);

            if (m.Msg == (int)0x312) //��Ű�� �������� 312 ���� �޼����� �ްԵ�
            {
                if (m.WParam == (IntPtr)0x0) // �� Ű�� ID�� 0�̸� //Alt+Z  â ���� ǥ��
                {
                    if (this.WindowState == FormWindowState.Minimized)
                    {
                        this.WindowState = FormWindowState.Normal;
                        return;
                    }

                    if (this.Visible)
                    {
                        Variables.ISTIMER = false;
                        this.Hide();

                    }
                    else
                    {
                        Variables.ISTIMER = true;
                        VS.mfnReLoad();
                        this.Show();
                    }
                }

                else if (m.WParam == (IntPtr)0x1) // �� Ű�� ID�� 1�̸�� //Alt+Q  ���α׷� ����
                {
                    ����ToolStripMenuItem_Click(null, null);
                }

            }

        }


        //���α׷� �����, ��ġ���
        private void efnLocSave()
        {
            Network.WriteIniFile("LocStockCalc", "X", this.Location.X.ToString(), @".\setting.ini");
            Network.WriteIniFile("LocStockCalc", "Y", this.Location.Y.ToString(), @".\setting.ini");

            Network.WriteIniFile("SizeMain", "Width", this.Size.Width.ToString(), @".\setting.ini");
            Network.WriteIniFile("SizeMain", "Height", this.Size.Height.ToString(), @".\setting.ini");

            Network.WriteIniFile("btnVisible", "YN", mIsPanel1Visible ? "Y" : "N", @".\setting.ini");
        }

        #endregion


        #region ��ư_Event
        

        //Detail
        private void btnDetail_Click(object sender, EventArgs e)
        {
            Report RE = new Report();
            RE.StartPosition = FormStartPosition.CenterParent;
            RE.ShowDialog();
            //fnHoldSearch();
            
        }

        //Revenue
        private void btnRev_Click(object sender, EventArgs e)
        {
            Revenue RE = new Revenue();
            RE.StartPosition = FormStartPosition.CenterParent;
            RE.ShowDialog();
           // fnHoldSearch();
        }

        //Sync��ư
        private void btnSync_Click(object sender, EventArgs e)
        {
            Sync SY = new Sync(mAddress,mID);
            SY.StartPosition = FormStartPosition.CenterParent;
            SY.ShowDialog();
        }

        // �ڵ��ư
        private void btnCode_Click(object sender, EventArgs e)
        {
            Code cd = new Code();
            cd.StartPosition = FormStartPosition.CenterParent;
            cd.ShowDialog();
            VS.mfnReLoad();
        }

        //�޴���ġ��
        private void btnSelectOpen_Click(object sender, EventArgs e)
        {
            efnOpenMenu(mIsPanel1Visible);
        }

        //������ư
        private void btnQuery_Click(object sender, EventArgs e)
        {
            
            Query qu = new Query();
            qu.StartPosition = FormStartPosition.CenterParent;
            qu.ShowDialog();
            VS.mfnReLoad();
        }


        //�Է¹�ư
        private void btnInput_Click(object sender, EventArgs e)
        {
            Input IN = new Input();
            IN.StartPosition = FormStartPosition.CenterParent;
            IN.ShowDialog();
            VS.mfnReLoad();
        }

        //�޸�
        private void btnMemo_Click(object sender, EventArgs e)
        {
            MemoMain ME = new MemoMain();
            ME.StartPosition = FormStartPosition.CenterParent;
            ME.ShowDialog();
            // fnHoldSearch();
        }


        //�ɼ�
        private void btnOpt_Click(object sender, EventArgs e)
        {
            Option OP = new Option();
            OP.StartPosition = FormStartPosition.CenterParent;
            OP.ShowDialog();
        }

        //����
        private void btnCalc_Click(object sender, EventArgs e)
        {
            Calc CA = new Calc();
            CA.StartPosition = FormStartPosition.CenterParent;
            CA.ShowDialog();
        }
        #endregion ��ư_Event_End

        #region �̺�Ʈ

        //���� ��, ��񿬰� ����
        private void StockCalc_FormClosing(object sender, FormClosingEventArgs e)
        {
            //conn.Dispose();
            
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
             
        }

        //��Ƽ���� ������ Ŭ�� - ����
        private void ����ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            efnLocSave();

            if (mSyncYN)
            {
                fnFTPUpSync();
            }

            notifyIcon1.Visible = false;
            UnregisterHotKey((int)this.Handle, 0);
            UnregisterHotKey((int)this.Handle, 1);

            try
            {
                File.Delete(@".\ver.ps");
            }
            catch
            { }

            Application.Exit();
        }


        //��Ƽ���� Ŭ�� ��, �� Ȱ��ȭ
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            /*
            if (e.Button == MouseButtons.Left)
            {
                if (this.WindowState == FormWindowState.Minimized)
                {
                    this.WindowState = FormWindowState.Normal;
                    return;
                }

                if (this.Visible)
                    this.Hide();
                else
                    this.Show();
            }
             * */
        }

        //�ɼǹ�ư
        private void btnSetting_Click(object sender, EventArgs e)
        {
            
            if (panel2.Visible)
                panel2.Visible = false;
            else
                panel2.Visible = true;
            /*
            Setting ST = new Setting(mAddress, mID);
            ST.StartPosition = FormStartPosition.CenterParent;
            ST.ShowDialog();
            */
        }

        //����������
        private void MdiParents_Resize(object sender, EventArgs e)
        {
            /*
            btnCode.Location = new System.Drawing.Point(btnCode.Location.X, panel4.Location.Y + panel4.Size.Height + 2);
            btnSync.Location = new System.Drawing.Point(btnSync.Location.X, panel4.Location.Y + panel4.Size.Height + 2);
            btnSetting.Location = new System.Drawing.Point(btnSetting.Location.X, panel4.Location.Y + panel4.Size.Height + 2);
            btnDetail.Location = new System.Drawing.Point(btnDetail.Location.X, panel4.Location.Y + panel4.Size.Height + 2);
            btnRev.Location = new System.Drawing.Point(btnRev.Location.X, panel4.Location.Y + panel4.Size.Height + 2);
            */
            if (mIsPanel1Visible)
            {
                //panel4.Size = new System.Drawing.Size(panel4.Size.Width, panel4.Size.Height - 27);
                panel1.Location = new System.Drawing.Point(panel4.Location.X + 2, panel4.Location.Y + panel4.Size.Height);
            }

            panel2.Location = new System.Drawing.Point(panel1.Size.Width - panel2.Size.Width, panel1.Location.Y - panel1.Size.Height - 20);
        }

        //�� Shown
        private void MdiParents_Shown(object sender, EventArgs e)
        {
            if (!mIsPanel1Visible)
            {
                efnOpenMenu(true);
            }
        }

        //����� �޺�
        private void cboDB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDB.SelectedItem.ToString() == "R")
            {
                Variables.DBROUTE = @"Data Source = .\Contents.db";
                btnInitVDB.Visible = false;
            }
            else if (cboDB.SelectedItem.ToString() == "V")
            {
                if (File.Exists(@".\VContents.db"))
                {
                    Variables.DBROUTE = @"Data Source = .\VContents.db";
                }
                else
                {
                    MessageBox.Show("Not Found File _ VContents.db");
                    cboDB.SelectedItem = "R";
                    return;
                }

                btnInitVDB.Visible = true;
            }

            VS.mfnReLoad();
        }

        //VDB �ʱ�ȭ
        private void btnInitVDB_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("�ʱ�ȭ?", "����?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string strSQL = @"DELETE FROM Sell;
                                  DELETE FROM Interest;
                                  DELETE FROM Memo;
                                  DELETE FROM Revenue;
                                  DELETE FROM Buy;";

                SQLiteConnection Conn = new SQLiteConnection(@"Data Source = .\VContents.db");
                Conn.Open();
                SQLiteCommand sqlcmd = new SQLiteCommand(strSQL, Conn);
                int intReturn = sqlcmd.ExecuteNonQuery();
                sqlcmd.Dispose();
                Conn.Dispose();

                if (intReturn > 0)
                    MessageBox.Show("����");
                else
                    MessageBox.Show("����");

                VS.mfnReLoad();
            }

        }

        #endregion �̺�Ʈ_End



    }
}