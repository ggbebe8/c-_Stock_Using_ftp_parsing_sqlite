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



/* 추가되었으면 하는 기능
 * 1. FTP On Or Off (o)
 * 2. 모의기능
 * 3. 관심 전체보기
 * 4. 오른쪽클릭하면 report로 갈지, 자세한 정보로 갈지 결정
 */

/* 문제되는 이슈
 * 날짜:해결여부:큰주제:상세내용
 * 20181030:O:Sync관련:
 *  - 1.로컬1에서 수정함 2.클라 종료할 때, 업로드 선택안함 3. 다시 로컬1 실행 후 종료 4. 로컬1이 최신임에도, 업로드 하라는 메세지 안뜸
 *  - 파일을 변경했을 경우만(hash가 변경된 경우) 업로드를 탐. 따라서 수정하지 않았기에 업로드 조건절을 지나감. 
 *  - 단순 날짜만 가지고 비교할 수는 없는게, 로컬을 두 개 사용했을 경우, 핑퐁마냥 로컬1이 업로드를 하면 똑같은 버전임에도 서버가 최신이기에 로컬2에서 또 다운
 *  - 혹은 다운 받으면 로컬의 날짜가 변경되고, 종료할 때 로컬의 날짜가 최신이기에 수정하지 못했음에도 또 업로드 메세지가 뜸
 *  - 즉 날짜만 가지고 비교하면 안돼
 *  - 변경여부(hash)만 가지고 setting.ini에 버전으로 관리해야 함. 
 *  - 버전만으로 Sync를 관리하지만, 거기에 따른 부가정보는 기록해둠. 예를 들면 업로드 시간 같은 것도 같이 적어둠. 
 *  - 나아가서는 초반에 ftp 접속을 못하고 수정했을 경우, D라는 파라미터를 저장해둔다던가 하고 저장해두는 것도 나쁘지 않겠지?
 * 
 * 20181102:O:Sync관련:
 *  - 1. 로컬파일의 버전은 ini에서 TouchFileDate에서 관리. 파일이 변경되면 종료할 때 여기 기록
 *  - 2. 여기까지는 좋아. 하지만 업로드 조건이 파일이 변경되었을 경우! 가 붙음. 즉 바로 업로드 안하고 다음에 업로드 하려고 하면, 파일을
 *  -    변경하지 않았을 경우는 업로드 하라는 메세지가 뜨지 않는거지!!
 * 
 * 
 * */


/* 팁 1
 * 컬럼 추가하기
ALTER TABLE 테이블명
ADD 추가할 컬럼명  데이터 유형;

 */

/* 팁 2
 * 컬럼 삭제하기
 * -sqlite는 컬럼삭제를 지원하지 않으므로 컬럼을 복사한 후, 삭제, 그리고 이름을 변경한다. 
BEGIN TRANSACTION;
CREATE TABLE 임시 (
	'InterName' TEXT,
	'Company' TEXT ,
	'CodeNum' TEXT,
	'DisSeq' TEXT,
	PRIMARY KEY(InterName,Company)
);
INSERT INTO 임시 (Company, CodeNum) 
SELECT Company, CodeNum FROM 원본;
DROP TABLE 원본;
ALTER TABLE 임시 RENAME TO 원본;
COMMIT;

기존 테이블과 새로 만든 테이블의 컬럼이 같다면, 아래와 같이 사용할 수 있다.
INSERT INTO table_list_new SELECT no, round, groups, number FROM table_list; 
*/

/*테이블 구조
 * 매수 테이블
CREATE TABLE Buy   (
                       'Seq' INTEGER PRIMARY KEY,
                       'Date' TEXT NOT NULL,
                       'Name' TEXT NOT NULL,
                       'Price' INT NOT NULL,
                       'Quantity' INT NOT NULL,
                       'Left' INT NOT NULL      -- 남은 수량
                     )
 * 매도 테이블
CREATE TABLE Sell   (
                       'Seq' INTEGER PRIMARY KEY,
                       'Date' TEXT NOT NULL,
                       'Name' TEXT NOT NULL,
                       'Price' INT NOT NULL,
                       'Quantity' INT NOT NULL
                     )
 * 손익 테이블
CREATE TABLE Revenue (
                       'Seq' INTEGER PRIMARY KEY,
                       'Date' TEXT NOT NULL,
                       'Name' TEXT NOT NULL,
                       'BPrice' INT NOT NULL,
                       'SPrice' INT NOT NULL,
                       'Quantity' INT NOT NULL,
                       'SellSeq' INTEGER
                     )
 * 주식코드 테이블
CREATE TABLE Interest (
	                    'InterName' TEXT,
	                    'Company' TEXT ,
	                    'CodeNum' TEXT,
	                    'DisSeq' INT,
	                     PRIMARY KEY(InterName,Company)
                      );
 * 관심종목 테이블
 CREATE TABLE Interest (
                       'Company' TEXT NOT NULL PRIMARY KEY,
                       'CodeNum' TEXT NOT NULL
                     )
 * 서버와의 동기화를 위해 ftp 정보를 담을 테이블(로우 무조건 1)
 CREATE TABLE FTP ( 
                  'IP' TEXT NOT NULL,
                  'ID' TEXT NOT NULL,
                  'PW' TEXT NOT NULL
                  )
 
 
 * 테이블 리스트 보기 
SELECT name 
  FROM sqlite_master 
 WHERE type IN ('table', 'view') 
   AND name NOT LIKE 'sqlite_%' 
UNION ALL 
SELECT name 
  FROM sqlite_temp_master 
 WHERE type IN ('table', 'view') 
 ORDER BY 1;

 * 테이블초기화
DELETE FROM Sell;
DELETE FROM Interest;
DELETE FROM Memo;
DELETE FROM Revenue;
DELETE FROM Buy;
 
 * 현재 테이블
buy 
sell 
Interest
Memo
Revenue
code
ftp
InitMemoOpt

 *간단메모 테이블 
 CREATE TABLE SimpleMemo (
	                        'CodeNum' TEXT,
	                        'Contents' TEXT,
	                        PRIMARY KEY(CodeNum)
                         );
*/


// -- 폼 속에 폼 넣기 http://pcsak3.com/455

namespace StockCalc
{
    public partial class MdiParents : Form
    {
        #region 전역변수

        string mAddress = "";    //FTP 주소

        string mID = "";         //FTP ID

        string mPW = "";         //FTP PW

        string mMD5 = "";       //파일의 해쉬한 값 가지기. (수정했는지 여부를 파악하기 위해서)

        DateTime mServerVerDate;    //서버 파일 날짜 저장

        bool mFtpCon = false;

        bool mSyncYN = true;        //Sync기능을 사용할 것인지에 대한 변수

        ViewStock VS = new ViewStock();     //객체생성하기

        bool mIsPanel1Visible = true;           //panel.visible이 초기화할 때 false로만 넘어와 ㅜㅜ 전역변수에서 비교해줘야함. 

        #endregion 전역변수_End

        [DllImport("user32.dll")]   //단축키 등록
        private static extern int RegisterHotKey(int hwnd, int id, int fsModifiers, int vk);


        //핫키제거
        [DllImport("user32.dll")]
        private static extern int UnregisterHotKey(int hwnd, int id);

        #region 생성자
        public MdiParents()
        {
            InitializeComponent();

            //콤보박스초기화
            fnInitComb();

            //패널에 폼 올리기
            fnOpenChild((Form)VS);

            mIsPanel1Visible = Convert.ToString(Network.ReadIniFile("btnVisible","YN",@".\setting.ini")) == "Y" ? true : false;

        }

        private void StockCalc_Load(object sender, EventArgs e)
        {
            //핫키 등록
            RegisterHotKey((int)this.Handle, 0, 1 , (int)Keys.Z); //Alt+Z
            RegisterHotKey((int)this.Handle, 1, 1, (int)Keys.Q);  //Alt+Q  // 프로그램 끌 때, 해제해줘야 함. 

            //프로그램이 이미 실행되어 있으면 죽이기
            if (fnKillProcess())
                return;
            
            //전역변수 초기화하기.(FTP,Option)
            fnInitVar();

            
            //FTP 동기화
            if (mSyncYN)
            {
                fnFTPDownSync();
            }

            //파일 동기화 후, Contents.db에 대한 해쉬값을 가지자. 
            mMD5 = Network.GetMD5(@".\Contents.db");
        }

        #endregion 생성자_End

        #region 내부함수

        //콤보박스 초기화
        private void fnInitComb()
        {
            cboDB.SelectedIndexChanged -= new System.EventHandler(cboDB_SelectedIndexChanged);
            cboDB.Items.Clear();
            cboDB.Items.Add("R");
            cboDB.Items.Add("V");
            cboDB.SelectedItem = "R";
            cboDB.SelectedIndexChanged += new System.EventHandler(cboDB_SelectedIndexChanged);
        }

        // 전역변수 초기화 하기(IP,ID,PW,Option)
        private void fnInitVar()
        {
            //디비연결
            string strResult = String.Empty;
            string strSQL = String.Empty;
            bool isSync = true;

           

            strSQL = @"
                        SELECT IP AS IP
                             , ID AS ID                             
                             , PW AS PW
                          FROM FTP;";

            //FTP만 하드코딩하자! V로 가져와버리면 안되니까!
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
                MessageBox.Show("서버db를 가져오는 중 오류발생 : \r\n" + strResult);
            }

            //옵션가져오기(SyncYN)
            isSync = Network.ReadIniFile("OPTION", "SyncYN", @".\setting.ini") == "N" ? false : true;
            mSyncYN = isSync;

        }

        /// 프로그램 실행되어 있을 경우 종료. 
        /// <returns>true : 실행중</returns>
        private bool fnKillProcess()
        {
            Process[] p = Process.GetProcessesByName("StockCalc");

            if (p.GetLength(0) > 1)
            {
                MessageBox.Show("프로그램이 이미 실행 중 입니다.");
                Application.Exit();         //이걸로 종료는 되나 Load이벤트는 끝까지 탐. 따라서 return시켜줌. 
                return true;
            }
            return false;
        }


        /// 패널에 폼 올리기
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

        /// 메뉴열고 닫기
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
        #endregion 내부함수_End

        #region FTP Sync 관련

        /// 서버랑 비교해서 옛 버전이면 다운.
        private void fnFTPDownSync()
        { 
            //현재 로컬과 서버의 업로드 날짜를 비교. 서버의 업로드 날짜가 더 크면 다운
            DateTime dtLocalFileDate;
            mServerVerDate = fnReadVer(out mFtpCon);

            //ftp 연결이 안되어있을 경우 빠져나오자.
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
                if (MessageBox.Show("서버의 파일이 최신입니다.\r\n최신버전으로 동기화 하시겠습니까?", "파일 동기화", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    //기존 파일은 백업.
                    try
                    {
                        File.Copy(@".\Contents.db", @".\Contents_Bak.db", true);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("파일 백업 중 오류 : " + e.ToString());
                    }

                    string strResult = Network.FtpDown(mAddress, mID, mPW, "Contents.db");
                    if (strResult != "")
                    {
                        MessageBox.Show(strResult);
                    }

                    //동기화를 성공했을 경우, 다운 받은 날짜의 정보를 setting.ini에 저장해둔다. upload시에 비교하여 동기화여부를 결정하는데 사용한다.
                    else
                    {
                        Network.WriteIniFile("FileTouchDate", "Date", mServerVerDate.ToString("yyyyMMddHHmmss"), @".\setting.ini");
                    }
                }
            }
        }

        /// 서버랑 비교해서 로컬이 최신이면 업로드. 프로그램 종료 시.
        private void fnFTPUpSync()
        {

            //파일을 수정했으면 날짜를 기록
            if (mMD5 != Network.GetMD5(@".\Contents.db"))
                Network.WriteIniFile("FileTouchDate", "Date", fnGetFileDate(@".\Contents.db").ToString("yyyyMMddHHmmss"), @".\setting.ini");

            //ftp연결이 안되어있을 경우 빠져나오자. 
            if (!mFtpCon)
                return;

            DateTime dtTouchDate = DateTime.ParseExact(Convert.ToString(Network.ReadIniFile("FileTouchDate", "Date", @".\setting.ini")), "yyyyMMddHHmmss", null);

            //처음 파일날짜와 비교해서 변경이 있을 경우 
            //&& 서버의 날짜보다 더 최신일 경우(다른 클라가 업로드한 경우도 있으므로 체크한다) 업로드 한다. 
            if (dtTouchDate > mServerVerDate || mServerVerDate.Year == 1)
            {
                if (MessageBox.Show("로컬의 파일이 최신입니다.\r\n서버에 업로드 하시겠습니까?", "파일 동기화", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {

                    /* 업로드 전 서버에 백업 */
                    // 3이 있으면 지우고, 2가 있으면 3으로, 1이 있으면 2로, 원본만 있으면 1로
                    string strRe = "";

                    strRe = Network.FtpDel(mAddress, mID, mPW, "Contents_Bak3.db");

                    if (strRe != "")
                    {
                        MessageBox.Show(strRe);
                        MessageBox.Show("업로드 실패");
                        return;
                    }

                    strRe = Network.FtpRename(mAddress, mID, mPW, "Contents_Bak2.db", "Contents_Bak3.db");

                    if (strRe != "")
                    {
                        MessageBox.Show(strRe);
                        MessageBox.Show("업로드 실패");
                        return;
                    }

                    strRe = Network.FtpRename(mAddress, mID, mPW, "Contents_Bak1.db", "Contents_Bak2.db");

                    if (strRe != "")
                    {
                        MessageBox.Show(strRe);
                        MessageBox.Show("업로드 실패");
                        return;
                    }

                    strRe = Network.FtpRename(mAddress, mID, mPW, "Contents.db", "Contents_Bak1.db");

                    if (strRe != "")
                    {
                        MessageBox.Show(strRe);
                        MessageBox.Show("업로드 실패");
                        return;
                    }

                    string strResult = Network.FtpUp(mAddress, mID, mPW, "Contents.db");
                    if (strResult != "")
                    {
                        MessageBox.Show(strResult);
                    }

                    //동기화를 성공했을 경우 서버에 날짜 올림
                    else
                    {
                        fnWriteVer(dtTouchDate.ToString("yyyyMMddHHmmss"));
                        strResult = Network.FtpUp(mAddress, mID, mPW, "ver.ps");
                        if (strResult != "")
                        {
                            MessageBox.Show("ver.ps 업로드 실패 : " + strResult);
                        }
                    }
                }
            }
        }


        #endregion FTP Sync 관련_End

        #region 파일 읽고 쓰기, 파일의 날짜
        /// ver.ps 파일 읽기. [0]: 업로드 [1]: 다운로드
        /// <returns>서버 업로드 날짜</returns>
        private DateTime fnReadVer(out bool ftpConnect)
        {
            //예외 추가하기 . 1 : 파일이 없을 때  2 : 안 짧릴 때 3 : 옯바른 정보인지 길이로 체크하자. 
            string strDate = "";
            DateTime dtFileDate = new DateTime();
            string strResult = "";
            // ??이거 왜 해놨지?? 한 방에 연결 안되는 경우 때문에 이렇게 해뒀나?
            //for (int i = 0; i < 5; i++)
            //{
                strResult = Network.FtpDown(mAddress, mID, mPW, "ver.ps");
                
                //if (strResult == "")
                    //break;


            //}

            if (strResult == "")    //서버에 있는 버전정보 다운
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
                MessageBox.Show("FTP 연결 실패 : " + strResult);
                ftpConnect = false;
            }

            return dtFileDate;
        }


        /// ver.ps 파일 쓰기. [0]: 업로드 [1]: 다운로드
        private bool fnWriteVer(string p_strDate)
        {
            //여기도 예외추가하자. 옯바른 길이인지.
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

        /// 파일의 날짜 구하는 함수.
        /// <param name="p_path">원하는 파일의 경로</param>
        /// <returns>파일 날짜</returns>
        private DateTime fnGetFileDate(string p_path)
        {
            DateTime dtFileDate = new DateTime();
            try
            {
                FileInfo file = new FileInfo(p_path);

                //굳이 DateTime을 string으로 변경 후, 다시 변경하는 이유는
                //초 단위까지 밖에 필요 없는데, 쓸데없는 정보까지 저장되어 
                //업로드 하는 로직에서 잘못된 비교를 한다 
                dtFileDate = DateTime.ParseExact(file.LastWriteTime.ToString("yyyyMMddHHmmss"), "yyyyMMddHHmmss", null);
            }
            catch
            {
            }
            return dtFileDate;
        }
        #endregion 파일 읽고 쓰기_END

        #region 이벤트 연결 함수


        //핫키 기능등록 (단축키)
        protected override void WndProc(ref Message m) //윈도우프로시저 콜백함수
        {
            base.WndProc(ref m);

            if (m.Msg == (int)0x312) //핫키가 눌러지면 312 정수 메세지를 받게됨
            {
                if (m.WParam == (IntPtr)0x0) // 그 키의 ID가 0이면 //Alt+Z  창 숨김 표시
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

                else if (m.WParam == (IntPtr)0x1) // 그 키의 ID가 1이면시 //Alt+Q  프로그램 종료
                {
                    종료ToolStripMenuItem_Click(null, null);
                }

            }

        }


        //프로그램 종료시, 위치기억
        private void efnLocSave()
        {
            Network.WriteIniFile("LocStockCalc", "X", this.Location.X.ToString(), @".\setting.ini");
            Network.WriteIniFile("LocStockCalc", "Y", this.Location.Y.ToString(), @".\setting.ini");

            Network.WriteIniFile("SizeMain", "Width", this.Size.Width.ToString(), @".\setting.ini");
            Network.WriteIniFile("SizeMain", "Height", this.Size.Height.ToString(), @".\setting.ini");

            Network.WriteIniFile("btnVisible", "YN", mIsPanel1Visible ? "Y" : "N", @".\setting.ini");
        }

        #endregion


        #region 버튼_Event
        

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

        //Sync버튼
        private void btnSync_Click(object sender, EventArgs e)
        {
            Sync SY = new Sync(mAddress,mID);
            SY.StartPosition = FormStartPosition.CenterParent;
            SY.ShowDialog();
        }

        // 코드버튼
        private void btnCode_Click(object sender, EventArgs e)
        {
            Code cd = new Code();
            cd.StartPosition = FormStartPosition.CenterParent;
            cd.ShowDialog();
            VS.mfnReLoad();
        }

        //메뉴펼치기
        private void btnSelectOpen_Click(object sender, EventArgs e)
        {
            efnOpenMenu(mIsPanel1Visible);
        }

        //쿼리버튼
        private void btnQuery_Click(object sender, EventArgs e)
        {
            
            Query qu = new Query();
            qu.StartPosition = FormStartPosition.CenterParent;
            qu.ShowDialog();
            VS.mfnReLoad();
        }


        //입력버튼
        private void btnInput_Click(object sender, EventArgs e)
        {
            Input IN = new Input();
            IN.StartPosition = FormStartPosition.CenterParent;
            IN.ShowDialog();
            VS.mfnReLoad();
        }

        //메모
        private void btnMemo_Click(object sender, EventArgs e)
        {
            MemoMain ME = new MemoMain();
            ME.StartPosition = FormStartPosition.CenterParent;
            ME.ShowDialog();
            // fnHoldSearch();
        }


        //옵션
        private void btnOpt_Click(object sender, EventArgs e)
        {
            Option OP = new Option();
            OP.StartPosition = FormStartPosition.CenterParent;
            OP.ShowDialog();
        }

        //계산기
        private void btnCalc_Click(object sender, EventArgs e)
        {
            Calc CA = new Calc();
            CA.StartPosition = FormStartPosition.CenterParent;
            CA.ShowDialog();
        }
        #endregion 버튼_Event_End

        #region 이벤트

        //나갈 때, 디비연결 끊기
        private void StockCalc_FormClosing(object sender, FormClosingEventArgs e)
        {
            //conn.Dispose();
            
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
             
        }

        //노티피콘 오른쪽 클릭 - 종료
        private void 종료ToolStripMenuItem_Click(object sender, EventArgs e)
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


        //노티피콘 클릭 시, 폼 활성화
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

        //옵션버튼
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

        //폼리사이즈
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

        //폼 Shown
        private void MdiParents_Shown(object sender, EventArgs e)
        {
            if (!mIsPanel1Visible)
            {
                efnOpenMenu(true);
            }
        }

        //디비선택 콤보
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

        //VDB 초기화
        private void btnInitVDB_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("초기화?", "리얼?", MessageBoxButtons.YesNo) == DialogResult.Yes)
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
                    MessageBox.Show("성공");
                else
                    MessageBox.Show("실패");

                VS.mfnReLoad();
            }

        }

        #endregion 이벤트_End



    }
}