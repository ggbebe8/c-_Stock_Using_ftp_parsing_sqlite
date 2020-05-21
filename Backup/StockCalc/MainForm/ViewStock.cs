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
    public partial class ViewStock : Form
    {
        #region **********전역변수**********
        string mInterListName = "";

        // 우클릭팝업
        ContextMenu m = new ContextMenu();
        #endregion 

        //현재 vs 관심 클릭에 대한 플래그 
        private enum enViewFlag
        {
            Hold, Interest
        }

        //판넬3이 열려있는 지 안열려있는 지 
        bool mISpanelVible = true;  

        #region **********생성자**********
        #endregion 생성자_End
        //생성자
        public ViewStock()
        {
            InitializeComponent();
            fnInitComb();
            //메인 메모 바인딩
            mfnMainMemoBinder();
        }

        //Load이벤트
        private void Hold_Load(object sender, EventArgs e)
        {
            string strFirstScreen = Network.ReadIniFile("OPTION", "FirstScreen", @".\setting.ini");
            if (strFirstScreen == "C")
                labHold_Click(null, null);
            else if (strFirstScreen == "I")
                labInterest_Click(null, null);
            else
                labMainMemo_Click(null, null);

            fnPopupInit();
        }

        #region **********외부노출함수**********
        #endregion

        public void mfnReLoad()
        {

            //셀의 포커스를 유지해야 하므로 row뿐만 아니라 col도 구해야 함. 

            int iSelectedRow = dgvView.CurrentCell == null ? -1 : dgvView.CurrentCell.RowIndex;

            int iSelectedCol = dgvView.CurrentCell == null ? -1 : dgvView.CurrentCell.ColumnIndex;

            // 관심목록을 추가만 해두고 종목은 추가 안했을 경우, 이거를 타게되면 추가한 관심목록이 사라지고 리셋되므로 이걸 추가.
            try
            {
                if (labHold.ForeColor == Color.Black)
                {
                    fnHoldSearch();
                }
                else if (labInterest.ForeColor == Color.Black)
                {
                    //콤보바인딩
                    fnCboBingding();
                    fnInterestSearch();
                }

            }
            catch
            {
            }
            try
            {
                if (dgvView.Rows.Count > 0 && iSelectedRow >= 0 && iSelectedCol >= 0)
                {
                    dgvView.Rows[iSelectedRow].Selected = true;
                    dgvView.CurrentCell = dgvView[iSelectedCol, iSelectedRow];
                }
            }
            catch
            { }


        }

        #region **********내부함수**********
        #endregion
        //콤보바인딩-목록
        private void fnInitComb()
        {
            string strQuery = "";
            strQuery += "\r\n" + "SELECT Company FROM Code";
            DataTable dt = Network.GetDBTable(strQuery);

            foreach (DataRow dr in dt.Rows)
            {
                cboStoSearch.Items.Add(dr["Company"].ToString());
            }

        }

        //팝업초기화
        private void fnPopupInit()
        {
            m.MenuItems.Add(new MenuItem("상세정보", popUpItem_Click));
            m.MenuItems.Add(new MenuItem("관련뉴스", popUpNews_Click));
            m.MenuItems.Add(new MenuItem("리포트", popUpReport_Click));
            m.MenuItems.Add(new MenuItem("간단메모", popUpSimpleMemo_Click));
        }

        //현재보유량
        /// <summary>
        /// 현재보유량 SELECT
        /// </summary>
        private void fnHoldSearch()
        {
            string strSQL = "";

            strSQL = @"SELECT MAX(Name) AS 종목명
                            , SUM(Left * Price) / SUM(Left) AS 평균가
                            , SUM(Left) AS 수량 
                            , SUM(Left * Price) AS 합계
                            , MAX(CodeNum) AS CodeNum  
                         FROM Buy
                         LEFT JOIN Code ON Name = Company
                        WHERE Left > 0
                        GROUP BY Name;";


            DataTable dtHold = Network.GetDBTable(strSQL);

            //dgvView.DataSource = dtHold;

            //dgvView.Columns["CodeNum"].Visible = false;

            dgvView.Columns.Clear();
            dgvView.Rows.Clear();

            //컬럼추가
            fnColumnAdd(enViewFlag.Hold, dtHold);

            // 컬럼정렬
            fnSortGrid(enViewFlag.Hold, true);

            //컬럼에 값 넣기
            fnInsertValue(enViewFlag.Hold, dtHold);
        }

        // 관심 읽기
        /// <summary>
        /// DB관심종목 읽기
        /// </summary>
        private void fnInterestSearch()
        {
            mInterListName = cboInter.Text;

            string strSQL = "";

            strSQL = @"SELECT Company AS 종목명
                             ,CodeNum
                             ,DisSeq
                         FROM Interest
                        WHERE IFNULL(InterName,'') = '" + cboInter.Text + @"'
                        ORDER BY DisSeq";

            DataTable dtInterest = Network.GetDBTable(strSQL);

            dgvView.Columns.Clear();
            dgvView.Rows.Clear();

            //컬럼추가
            fnColumnAdd(enViewFlag.Interest, dtInterest);
            
            //컬럼 정렬
            fnSortGrid(enViewFlag.Interest, true);

            //컬럼에 값 넣기
            fnInsertValue(enViewFlag.Interest, dtInterest);

        }

        //컬럼추가
        /// <summary>
        /// 현재가만 추가해줌. (Time이벤트에도 넣기 위해 일부러 뺌)
        /// </summary>
        /// <param name="p_strViewFlag">Hold or Interest</param>
        private void fnColumnAdd(enViewFlag p_ViewFlag, DataTable p_dt)
        {
            if (!dgvView.Columns.Contains("종목명"))
            {
                dgvView.Columns.Add("종목명", "종목명");
            }

            if (!dgvView.Columns.Contains("현재가"))
            {
                dgvView.Columns.Add("현재가", "현재가");
            }

            if (!dgvView.Columns.Contains("전일가"))
            {
                dgvView.Columns.Add("전일가", "전일가");
            }

            if (!dgvView.Columns.Contains("전일차이"))
            {
                dgvView.Columns.Add("전일차이", "전일차이");
            }

            if (!dgvView.Columns.Contains("차이%"))
            {
                dgvView.Columns.Add("차이%", "차이%");
            }

            if (!dgvView.Columns.Contains("거래량"))
            {
                dgvView.Columns.Add("거래량", "거래량");
            }

            if (!dgvView.Columns.Contains("CodeNum"))
            {
                dgvView.Columns.Add("CodeNum", "CodeNum");
            }

            if (p_ViewFlag == enViewFlag.Hold)
            {

                if (!dgvView.Columns.Contains("평균가"))
                {
                    dgvView.Columns.Add("평균가", "평균가");
                }

                if (!dgvView.Columns.Contains("수량"))
                {
                    dgvView.Columns.Add("수량", "수량");
                }

                if (!dgvView.Columns.Contains("합계"))
                {
                    dgvView.Columns.Add("합계", "합계");
                }

                if (!dgvView.Columns.Contains("예상수익"))
                {
                    dgvView.Columns.Add("예상수익", "예상수익");
                }

                if (!dgvView.Columns.Contains("예상수익%"))
                {
                    dgvView.Columns.Add("예상수익%", "예상수익%");
                }

                if (!dgvView.Columns.Contains("예상세금"))
                {
                    dgvView.Columns.Add("예상세금", "예상세금");
                }

            }


            dgvView.Columns["종목명"].DisplayIndex = 0;
            dgvView.Columns["현재가"].DisplayIndex = 1;
            dgvView.Columns["전일가"].DisplayIndex = 2;
            dgvView.Columns["전일차이"].DisplayIndex = 3;
            dgvView.Columns["차이%"].DisplayIndex = 4;
            dgvView.Columns["거래량"].DisplayIndex = 5;
            dgvView.Columns["CodeNum"].Visible = false;

            if (p_ViewFlag == enViewFlag.Hold)
            {
                dgvView.Columns["평균가"].DisplayIndex = 6;
                dgvView.Columns["수량"].DisplayIndex = 7;
                dgvView.Columns["합계"].DisplayIndex = 8;
            }
        }

        // 그리드정렬
        /// <summary>
        /// 그리드 초기화
        /// </summary>
        /// <param name="p_isSearch"></param>
        private void fnSortGrid(enViewFlag p_ViewFlag, bool p_isSearch)
        {
            if (p_isSearch)
            {
                foreach (DataGridViewColumn col in dgvView.Columns)
                {
                    col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                dgvView.Columns["종목명"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (p_ViewFlag == enViewFlag.Hold)
                {

                    dgvView.Columns["평균가"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvView.Columns["수량"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvView.Columns["합계"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvView.Columns["예상수익"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvView.Columns["예상수익%"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvView.Columns["예상세금"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


                    dgvView.Columns["평균가"].DefaultCellStyle.Format = "#,##";
                    dgvView.Columns["수량"].DefaultCellStyle.Format = "#,##";
                    dgvView.Columns["합계"].DefaultCellStyle.Format = "#,##";
                    dgvView.Columns["예상수익"].DefaultCellStyle.Format = "#,##";
                    dgvView.Columns["예상수익%"].DefaultCellStyle.Format = "#,#0.##";
                    dgvView.Columns["예상세금"].DefaultCellStyle.Format = "#,##";
                }
                dgvView.Columns["현재가"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvView.Columns["현재가"].DefaultCellStyle.Format = "#,##";
                dgvView.Columns["전일가"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvView.Columns["전일차이"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvView.Columns["차이%"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvView.Columns["거래량"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvView.Columns["전일가"].DefaultCellStyle.Format = "#,##";
                dgvView.Columns["전일차이"].DefaultCellStyle.Format = "#,##";
                dgvView.Columns["차이%"].DefaultCellStyle.Format = "#,##";
                dgvView.Columns["거래량"].DefaultCellStyle.Format = "#,##";


                //포커스 
                if (dgvView.Rows.Count > 0)
                {
                    dgvView.Rows[dgvView.Rows.Count - 1].Selected = true;
                    dgvView.CurrentCell = dgvView.Rows[dgvView.Rows.Count - 1].Cells["종목명"];
                    dgvView.BeginEdit(true);
                }
            }

            if (p_ViewFlag == enViewFlag.Hold)
            {
                dgvView.Columns["현재가"].Width = 70;
                dgvView.Columns["평균가"].Width = 70;
                dgvView.Columns["수량"].Width = 60;
                dgvView.Columns["합계"].Width = 70;
                dgvView.Columns["예상수익"].Width = 70;
                dgvView.Columns["예상수익%"].Width = 70;
                dgvView.Columns["예상세금"].Width = 70;
            }

            if (dgvView.Height - 20 - (dgvView.RowTemplate.Height * dgvView.RowCount) > 0)
            {
                //스크롤바 안 생기는 경우
                dgvView.Columns["종목명"].Width = 355 - 270;//dgvHold.Width - 270;

            }
            else
            {
                //스크롤바 생기는 경우
                dgvView.Columns["종목명"].Width = 355 - 290; //dgvHold.Width - 290;
            }

            dgvView.Columns["전일가"].Width = 70;
            dgvView.Columns["전일차이"].Width = 70;
            dgvView.Columns["차이%"].Width = 60;
            dgvView.Columns["거래량"].Width = 70;
        }

        //그리드에 값 넣기
        /// <summary>
        /// 그리드에 값 넣기
        /// </summary>
        /// <param name="p_ViewFlag">Hold,Interest</param>
        /// <param name="p_dt">테이블</param>
        private void fnInsertValue(enViewFlag p_ViewFlag, DataTable p_dt)
        {
            //만든 컬럼에 값 넣기
            string CodeNum = "";
            Dictionary<string, string> Value = new Dictionary<string, string>();

            for (int i = 0; i < p_dt.Rows.Count; i++)
            {
                dgvView.Rows.Add();
                CodeNum = p_dt.Rows[i]["CodeNum"].ToString();
                dgvView["종목명", i].Value = p_dt.Rows[i]["종목명"].ToString();

                Value = Parser.GetInfo(CodeNum);

                dgvView["현재가", i].Value = Value["NowV"];
                dgvView["전일가", i].Value = Value["YesterDayV"];
                dgvView["전일차이", i].Value = Value["Interval"];
                dgvView["차이%", i].Value = Value["Per"];
                dgvView["거래량", i].Value = Value["QV"];
                dgvView["CodeNum", i].Value = CodeNum;
                if (p_ViewFlag == enViewFlag.Hold)
                {
                    dgvView["평균가", i].Value = p_dt.Rows[i]["평균가"].ToString();
                    dgvView["수량", i].Value = p_dt.Rows[i]["수량"].ToString();
                    dgvView["합계", i].Value = p_dt.Rows[i]["합계"].ToString();

                    dgvView["예상수익", i].Value = ((Convert.ToDouble(Value["NowV"]) - Convert.ToDouble(dgvView["평균가", i].Value)) * Convert.ToDouble(dgvView["수량", i].Value))    //현재가 - 보유평균가
                                    - (Convert.ToDouble(Value["NowV"]) * Convert.ToDouble(dgvView["수량", i].Value) * 0.003);   //매도세금
                    dgvView["예상수익%", i].Value = (((Convert.ToDouble(Value["NowV"]) - Convert.ToDouble(dgvView["평균가", i].Value)) * Convert.ToDouble(dgvView["수량", i].Value))    //현재가 - 보유평균가
                                    - (Convert.ToDouble(Value["NowV"]) * Convert.ToDouble(dgvView["수량", i].Value) * 0.003))   //매도세금
                                    / (Convert.ToDouble(dgvView["평균가", i].Value) * Convert.ToDouble(dgvView["수량", i].Value)) * 100;   //%구하기
                    dgvView["예상세금", i].Value = (Convert.ToDouble(Value["NowV"]) * Convert.ToDouble(dgvView["수량", i].Value) * 0.003);
                }

            }

        }

        //콤보바인딩(관심목록)
        private void fnCboBingding()
        {
            string strSelItem = cboInter.SelectedItem == null ? "" : cboInter.SelectedItem.ToString();
            cboInter.Items.Clear();

            DataTable dtCombo = Network.GetDBTable("SELECT IFNULL(MAX(InterName),'') AS InterName FROM Interest GROUP BY InterName");
            

            foreach (DataRow dr in dtCombo.Rows)
            {
                if (!cboInter.Items.Contains(dr["InterName"].ToString()))
                {
                    cboInter.Items.Add(dr["InterName"].ToString());
                }
            }
            /*
            if (cboInter.Items.Count > 0)
                cboInter.SelectedIndex = 0;
            */
            if (strSelItem != "")
                cboInter.SelectedItem = strSelItem;
            else if (cboInter.Items.Count > 0)
                cboInter.SelectedIndex = 0;
        }

        //보유바인딩
        /// <summary>
        /// 보유바인딩
        /// </summary>
        private void mfnHoldBinder()
        {



            //디비읽기
            fnHoldSearch();

            //포커스 초기화
            dgvView.ClearSelection();

        }

        //관심바인딩
        /// <summary>
        /// 관심바인딩
        /// </summary>
        private void mfnInterestBinder()
        {
            //콤보바인딩
            fnCboBingding();

            //디비읽기
            fnInterestSearch();

            //포커스 초기화
            dgvView.ClearSelection();
        }

        //메인메모 바인딩
        private void mfnMainMemoBinder()
        {
            DataTable dt = Network.GetDBTable("SELECT Contents FROM SimpleMemo WHERE CodeNum = 'MainMemo'");

            if (dt.Rows.Count > 0)
            {
                txtMemo.Text = dt.Rows[0]["Contents"].ToString();
            }
            else
            {
                Network.ExecDB("INSERT INTO SimpleMemo (Contents, CodeNum) VALUES ('', 'MainMemo')");
            }
        }

        //버튼 상태 설정
        /// <summary>
        /// 버튼 상태 설정
        /// </summary>
        /// <param name="p_strBtnTag">버튼의 태그값</param>
        /// <param name="p_isEnable">버튼들을 Enable여부</param>
        private void fnChangeBtn(string p_strBtnTag, bool p_isEnable)
        {
            if (p_strBtnTag == "Add2" || p_strBtnTag == "Del2")
            {
                btnAdd2.Tag = "Add2.Comfirm";
                btnDel2.Tag = "Del2.Cancel";
            }


            else if (p_strBtnTag == "Chg")
            {
                btnAdd2.Tag = "Chg.Comfirm";
                btnDel2.Tag = "Chg.Cancel";
            }

            else if (p_strBtnTag == "Add2.Comfirm" || p_strBtnTag == "Del2.Cancel" || p_strBtnTag == "Chg.Comfirm" || p_strBtnTag == "Chg.Cancel")
            {
                btnAdd2.Tag = "Add2";
                btnDel2.Tag = "Del2";
            }


            if (p_isEnable)
            {
                btnAdd2.Text = "추가";
                btnDel2.Text = "삭제";
            }
            else
            {
                btnAdd2.Text = "확인";
                btnDel2.Text = "취소";
            }

            btnChg.Visible = p_isEnable;
            btnAdd.Enabled = p_isEnable;
            btnDel.Enabled = p_isEnable;
            btnUp.Enabled = p_isEnable;
            btnDown.Enabled = p_isEnable;
            cboStoSearch.Enabled = p_isEnable;
        }
        

        //추가버튼 연결
        private void efnInsert()
        {
            if (cboInter.Text.Replace(" ", "") == "")
            {
                MessageBox.Show("관심목록이 빈 값입니다.");
                return;
            }
            string strSQL = @"
                                SELECT *
                                  FROM Interest
                                 WHERE COMPANY = '" + cboStoSearch.Text.ToUpper() + @"'
                                   AND IFNULL(InterName,'') = '" + cboInter.Text + @"';

                                SELECT *
                                  FROM Code
                                 WHERE COMPANY = '" + cboStoSearch.Text.ToUpper() + @"'";
            DataSet ds = Network.GetDBSet(strSQL);

            if (ds.Tables[0].Rows.Count >= 1)
            {
                cboStoSearch.Text = "";
            }
            else if (ds.Tables[1].Rows.Count < 1)
            {
                MessageBox.Show("없는 종목입니다.");
            }
            else
            {
                strSQL = @"
                            INSERT INTO Interest(Company, CodeNum, DisSeq, InterName)
                                   SELECT Company
                                        , CodeNum
                                        , IFNULL((SELECT MAX(DisSeq) + 1 AS DisSeq 
                                             FROM Interest 
                                            WHERE IFNULL(InterName,'') = '" + cboInter.Text + @"'
                                            GROUP BY InterName),1) AS DisSeq
                                        , '" + cboInter.Text + @"' 
                                     FROM Code
                                    WHERE COMPANY = '" + cboStoSearch.Text.ToUpper() + "';";
                
                if (Network.ExecDB(strSQL) > 0)
                    MessageBox.Show("추가하였습니다.");
                else
                    MessageBox.Show("추가실패");
                fnInterestSearch();
            }
            cboStoSearch.Text = "";
        }


        //삭제버튼 연결
        private void efnDelete()
        {
            string strSQL = @"
                                SELECT *
                                  FROM Interest
                                 WHERE COMPANY = '" + cboStoSearch.Text.ToUpper() + @"'
                                   AND IFNULL(InterName,'') = '" + cboInter.Text + @"';";
            DataTable dt = Network.GetDBTable(strSQL);

            if (dt.Rows.Count == 1)
            {
                strSQL = @"
                            DELETE FROM Interest
                                  WHERE COMPANY = '" + cboStoSearch.Text.ToUpper() + @"'
                                    AND IFNULL(InterName,'') = '" + cboInter.Text + @"';";
                
                if (Network.ExecDB(strSQL) > 0)
                    MessageBox.Show("삭제하였습니다.");
                else
                    MessageBox.Show("삭제실패");
                mfnInterestBinder();
                //fnInterestSearch();
            }
            else
            {
                MessageBox.Show("관심종목에 없는 종목입니다.");
            }
            cboStoSearch.Text = "";
        }

        //관심추가2 버튼 연결
        private void efnAdd2()
        {
            if (btnAdd2.Tag.ToString() == "Add2")
            {
                cboInter.SelectedIndexChanged -= new System.EventHandler(this.cboInter_SelectedIndexChanged);
                cboInter.DropDownStyle = ComboBoxStyle.Simple;
                cboInter.Text = "";
                fnChangeBtn(btnAdd2.Tag.ToString(), false);
                cboInter.SelectedIndexChanged += new System.EventHandler(this.cboInter_SelectedIndexChanged);
            }
            else if (btnAdd2.Tag.ToString() == "Add2.Comfirm")
            {
                foreach (string itemName in cboInter.Items)
                {
                    if (cboInter.Text == itemName)
                    {
                        MessageBox.Show("같은 이름의 관심목록이 있습니다.");
                        return;
                    }
                }

                if (cboInter.Text.Replace(" ", "") == "")
                {
                    MessageBox.Show("값을 입력해주세요.");
                    return;
                }

                fnInterestSearch();
                string strTempCbo = cboInter.Text;

                cboInter.DropDownStyle = ComboBoxStyle.DropDownList;

                cboInter.Items.Add(strTempCbo);
                cboInter.SelectedIndex = cboInter.Items.IndexOf(strTempCbo);
                fnChangeBtn(btnAdd2.Tag.ToString(), true);

            }

            else if (btnAdd2.Tag.ToString() == "Chg.Comfirm")
            {
                foreach (string itemName in cboInter.Items)
                {
                    if (cboInter.Text == itemName)
                    {
                        MessageBox.Show("같은 이름의 관심목록이 있습니다.");
                        return;
                    }
                }

                if (cboInter.Text.Replace(" ", "") == "")
                {
                    MessageBox.Show("값을 입력해주세요.");
                    return;
                }

                if (Network.ExecDB(@"UPDATE Interest SET InterName = '" + cboInter.Text + @"' WHERE InterName = '" + mInterListName + "'") <= 0)
                {
                    MessageBox.Show("변경 실패");
                    return;
                }

                string strTempCbo = cboInter.Text;

                mfnInterestBinder();
                
                cboInter.DropDownStyle = ComboBoxStyle.DropDownList;

                //cboInter.Items.Add(strTempCbo);
                cboInter.SelectedIndex = cboInter.Items.IndexOf(strTempCbo);
                fnChangeBtn(btnAdd2.Tag.ToString(), true);
            }
        }

        //관심삭제2 버튼 연결
        private void efnDel2()
        {

            if (btnDel2.Tag.ToString() == "Del2")
            {
                if (MessageBox.Show("'주의'!! 관심목록 삭제입니다!!! 정말 삭제 하시겠습니가? ", "관심목록 삭제", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (Network.ExecDB("DELETE FROM Interest WHERE InterName = '" + cboInter.Text + "'") > 0)
                    {
                        mfnInterestBinder();
                    }
                    else
                        MessageBox.Show("삭제 실패");
                }
            }
            else if (btnDel2.Tag.ToString() == "Del2.Cancel" || btnDel2.Tag.ToString() == "Chg.Cancel")
            {
                fnChangeBtn(btnDel2.Tag.ToString(), true);
                cboInter.DropDownStyle = ComboBoxStyle.DropDownList;
                cboInter.SelectedItem = mInterListName;
            }
        }

        //관심변경 버튼 연결
        private void efnChg()
        {
            if (cboInter.Text.Replace(" ", "") == "")
                return;
            cboInter.SelectedIndexChanged -= new System.EventHandler(this.cboInter_SelectedIndexChanged);
            cboInter.DropDownStyle = ComboBoxStyle.Simple;
            fnChangeBtn(btnChg.Tag.ToString(), false);
            cboInter.SelectedIndexChanged += new System.EventHandler(this.cboInter_SelectedIndexChanged);

        }

        //관심 DisSeq Up버튼
        private void efnDisSeqUp()
        {
            int intSelectedRow;
            try
            {
                intSelectedRow = dgvView.CurrentCell.RowIndex;
                if (intSelectedRow < 1)
                    return;
            }
            catch (NullReferenceException)
            {
                return;
            }
            
            DataTable dtDisSeq = Network.GetDBTable(@"SELECT DisSeq 
                                                        FROM Interest 
                                                       WHERE InterName = '" + cboInter.Text + @"'
                                                         AND (Company = '" + dgvView["종목명",intSelectedRow - 1].Value.ToString() + @"'
                                                          OR  Company = '" + dgvView["종목명",intSelectedRow].Value.ToString() + @"')
                                                       ORDER BY DisSeq");
            if(dtDisSeq.Rows.Count != 2)
            {
                MessageBox.Show("SELECT 실패");
                return;
            }

            int intResult;

            intResult = Network.ExecDB(@"UPDATE Interest 
                                            SET DisSeq = '" + dtDisSeq.Rows[0][0].ToString() + @"'
                                          WHERE InterName = '" + cboInter.Text + @"'
                                            AND Company = '" + dgvView["종목명",intSelectedRow].Value.ToString() + @"';
                                
                                         UPDATE Interest
                                            SET DisSeq = '" + dtDisSeq.Rows[1][0].ToString() + @"'
                                          WHERE InterName = '" + cboInter.Text + @"'
                                            AND Company = '" + dgvView["종목명", intSelectedRow - 1].Value.ToString() + @"';");

            if (intResult < 1)
            {
                MessageBox.Show("Update 실패");
                return;
            }

            fnInterestSearch();
            dgvView.Rows[intSelectedRow - 1].Selected = true;
            dgvView.CurrentCell = dgvView.Rows[intSelectedRow - 1].Cells["종목명"];
            dgvView.BeginEdit(true);

        }

        //관심 DisSeq Down버튼
        private void efnDisSeqDown()
        {
            int intSelectedRow;
            try
            {
                intSelectedRow = dgvView.CurrentCell.RowIndex;
                if (intSelectedRow == dgvView.Rows.Count-1)
                    return;
            }
            catch (NullReferenceException)
            {
                return;
            }

            DataTable dtDisSeq = Network.GetDBTable(@"SELECT DisSeq 
                                                        FROM Interest 
                                                       WHERE InterName = '" + cboInter.Text + @"'
                                                         AND (Company = '" + dgvView["종목명", intSelectedRow].Value.ToString() + @"'
                                                          OR  Company = '" + dgvView["종목명", intSelectedRow + 1].Value.ToString() + @"')
                                                       ORDER BY DisSeq");
            if (dtDisSeq.Rows.Count != 2)
            {
                MessageBox.Show("SELECT 실패");
                return;
            }

            int intResult;

            intResult = Network.ExecDB(@"UPDATE Interest 
                                            SET DisSeq = '" + dtDisSeq.Rows[1][0].ToString() + @"'
                                          WHERE InterName = '" + cboInter.Text + @"'
                                            AND Company = '" + dgvView["종목명", intSelectedRow].Value.ToString() + @"';
                                
                                         UPDATE Interest
                                            SET DisSeq = '" + dtDisSeq.Rows[0][0].ToString() + @"'
                                          WHERE InterName = '" + cboInter.Text + @"'
                                            AND Company = '" + dgvView["종목명", intSelectedRow + 1].Value.ToString() + @"';");

            if (intResult < 1)
            {
                MessageBox.Show("Update 실패");
                return;
            }

            fnInterestSearch();
            dgvView.Rows[intSelectedRow + 1].Selected = true;
            dgvView.CurrentCell = dgvView.Rows[intSelectedRow + 1].Cells["종목명"];
            dgvView.BeginEdit(true);
        }


        #region **********이벤트**********
        #endregion

        // 타임이벤트
        /// <summary>
        /// 현재가를 60초마다 받아오기 위해서 돌림. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Variables.ISTIMER)
            {
                mfnReLoad();
            }
        }

        // 리사이즈이벤트
        private void Hold_Resize(object sender, EventArgs e)
        {
            
            if (labHold.ForeColor == Color.Black)
                fnSortGrid(enViewFlag.Hold, false);
            else if (labInterest.ForeColor == Color.Black)
                fnSortGrid(enViewFlag.Interest, false);
             
        }

        // 현재클릭
        private void labHold_Click(object sender, EventArgs e)
        {
            labHold.ForeColor = Color.Black;
            labInterest.ForeColor = Color.Gray;
            labMainMemo.ForeColor = Color.Gray;
            dgvView.Location = new System.Drawing.Point(0, 0);
            dgvView.Size = panel2.Size;
            panel3.Visible = false;
            btnSelectOpen.Visible = false;
            txtMemo.Visible = false;
            mfnHoldBinder();
        }

        // 관심클릭
        private void labInterest_Click(object sender, EventArgs e)
        {

            panel3.Visible = mISpanelVible ? true : false;
            btnSelectOpen.Visible = true;
            labHold.ForeColor = Color.Gray;
            labInterest.ForeColor = Color.Black;
            labMainMemo.ForeColor = Color.Gray;
            txtMemo.Visible = false;
            dgvView.Location = new System.Drawing.Point(0, panel3.Visible ? panel3.Size.Height : 0);
            dgvView.Size = new System.Drawing.Size(panel2.Size.Width, panel3.Visible ? panel2.Size.Height - panel3.Size.Height : panel2.Size.Height);
            mfnInterestBinder();

        }

        // 메인메모 클릭
        private void labMainMemo_Click(object sender, EventArgs e)
        {
            labHold.ForeColor = Color.Gray;
            labInterest.ForeColor = Color.Gray;
            labMainMemo.ForeColor = Color.Black;
            //dgvView.Location = new System.Drawing.Point(0, 0);
            //dgvView.Size = panel2.Size;
            panel3.Visible = false;
            btnSelectOpen.Visible = false;
            txtMemo.Visible = true;
            txtMemo.Dock = DockStyle.Fill;
        }

        // 관심추가 버튼클릭
        private void btnAdd_Click(object sender, EventArgs e)
        {
            efnInsert();
        }

        // 관심삭제 버튼클릭
        private void btnDel_Click(object sender, EventArgs e)
        {
            efnDelete();
        }

        // 관심추가 txt키다운
        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                efnInsert();
            }
        }

        // 보유종목에서 더블클릭 시, 자세한 정보를 띄운다. 
        private void dgvView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (labHold.ForeColor == Color.Black)
            {
                Report RE = new Report(dgvView["종목명", dgvView.CurrentCell.RowIndex].Value.ToString());
                RE.StartPosition = FormStartPosition.CenterParent;
                RE.ShowDialog();
            }
        }

        //관심에서 셀클릭했을 때, 이름 txtbox에 넣어주기
        private void dgvView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (labInterest.ForeColor == Color.Black)
            {
                try
                {
                    cboStoSearch.Text = dgvView["종목명", e.RowIndex].Value.ToString();
                }
                catch
                {
                    cboStoSearch.Text = "";
                }
            }
        }

        //관심 추가 삭제 판넬 안보이게 하기. 
        private void btnSelectOpen_Click(object sender, EventArgs e)
        {
            if (panel3.Visible)
            {
                dgvView.Location = new System.Drawing.Point(0, 0);
                dgvView.Size = panel2.Size;
                panel3.Visible = false;
                mISpanelVible = false;
            }
            else
            {
                dgvView.Location = new System.Drawing.Point(0, panel3.Size.Height);
                dgvView.Size = new System.Drawing.Size(panel2.Size.Width, panel2.Size.Height - panel3.Size.Height);
                panel3.Visible = true;
                mISpanelVible = true;
            }
        }

        //관심 DisSeq Up조정
        private void btnUp_Click(object sender, EventArgs e)
        {
            efnDisSeqUp();
        }

        //관심 DisSeq Down조정
        private void btnDown_Click(object sender, EventArgs e)
        {
            efnDisSeqDown();
        }

        //관심추가2 버튼
        private void btnAdd2_Click(object sender, EventArgs e)
        {
            efnAdd2();
        }

        //관심목록 삭제 
        private void btnDel2_Click(object sender, EventArgs e)
        {
            efnDel2();
        }

        //관심변경버튼 선택 시
        private void btnChg_Click(object sender, EventArgs e)
        {
            efnChg();
        }

        //관심목록 콤보 선택 시
        private void cboInter_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboStoSearch.Text = "";
            fnInterestSearch();
        }

        private void cboInter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && cboInter.DropDownStyle == ComboBoxStyle.Simple)
            {
                efnAdd2();
            }

        }

        //팝업
        private void dgvView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                m.Show(dgvView, new Point(e.X, e.Y));
            }
        }

        //팝업아이템클릭(상세정보)
        private void popUpItem_Click(object sender, EventArgs e)
        {
            Detail DT = new Detail(dgvView["CodeNum", dgvView.CurrentCell.RowIndex].Value.ToString());
            DT.StartPosition = FormStartPosition.CenterParent;
            DT.ShowDialog();
        }

        //팝업아이템클릭(관련뉴스)
        private void popUpNews_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://finance.naver.com/item/news_news.nhn?code=" + dgvView["CodeNum", dgvView.CurrentCell.RowIndex].Value.ToString() + "&page=&sm=title_entity_id.basic&clusterId=");
        }

        //팝업아이템클릭(리포트)
        private void popUpReport_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://vip.mk.co.kr/newSt/news/news_list2.php?sCode=110");
        }

        //팝업아이템클릭(간단메모)
        private void popUpSimpleMemo_Click(object sender, EventArgs e)
        {
            SimpleMemo SM = new SimpleMemo(dgvView["CodeNum", dgvView.CurrentCell.RowIndex].Value.ToString(), dgvView["종목명", dgvView.CurrentCell.RowIndex].Value.ToString());
            SM.StartPosition = FormStartPosition.CenterParent;
            SM.ShowDialog();
        }

        //메인메모에서 떠날 때 세이브한다. 
        private void txtMemo_Leave(object sender, EventArgs e)
        {
            Network.ExecDB("UPDATE SimpleMemo SET Contents = '" + txtMemo.Text + "' WHERE CodeNum = 'MainMemo'");
        }

    }
}