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
    public partial class Input : Form
    {
        //*****************전역변수******************//
        DataSet mDTHold;    //현재보유 그리드뷰


        //*****************생성자******************//
        #region 생성자
        public Input()
        {
            InitializeComponent();
            fnInitCbo();
        }

        private void Input_Load(object sender, EventArgs e)
        {
            fnHoldSearch();
        }
        #endregion 생성자_End


        //*****************내부함수******************//

        #region 콤보초기화
        /// <summary>
        /// 콤보초기화
        /// </summary>
        private void fnInitCbo()
        {

            cboType.Items.Clear();

            cboType.Items.Add("매수");
            cboType.Items.Add("매도");

            cboType.SelectedItem = "매수";

            string strQuery = "";
            strQuery += "\r\n" + "SELECT Company FROM Code";
            DataTable dt = Network.GetDBTable(strQuery);

            foreach (DataRow dr in dt.Rows)
            {
                cboStoSearch.Items.Add(dr["Company"].ToString());
            }
        }




        #endregion 콤보초기화_End

        #region 입력 함수
        /// <summary>
        /// 입력 함수
        /// </summary>
        private void efnDeal()
        {
            string strSQL = "";

            // 값이 빈 값이면 리턴
            if (txtPrice.Text == "")
            {
                MessageBox.Show("값을 입력하세요.");
                return;
            }

            //수량 빈 값이면 리턴
            if (txtQuatity.Text == "")
            {
                MessageBox.Show("수량을 입력하세요.");
                return;
            }



            if (cboType.Text == "매수")
            {

                //이름 빈 값이면 리턴
                if (cboStoSearch.Text.Replace(" ", "") == "")
                {
                    MessageBox.Show("종목명을 입력하세요.");
                    return;
                }

                if (mDTHold.Tables[2].Select("Company = '" + cboStoSearch.Text + "'").Length < 1)
                {
                    MessageBox.Show("잘못된 종목입니다.");
                    return;
                }
                   
                else if (mDTHold.Tables[2].Select("Company = '" +  cboStoSearch.Text + "'").Length > 1)
                {
                    MessageBox.Show("Code에 중복된 이름의 종목이 있습니다.");
                    return;
                }

                strSQL = @"
                           INSERT INTO Buy (Date, Name, Price, Quantity, Left)
                           VALUES ( '" + dte.Value.ToString("yyyyMMdd") + @"'
                                    , '" + cboStoSearch.Text.ToUpper() + @"'
                                    , '" + txtPrice.Text + @"'
                                    , '" + txtQuatity.Text + @"'
                                    , '" + txtQuatity.Text + @"'
                                  )";
            }

            else if (cboType.Text == "매도")
            {
                //이름 빈 값이면 리턴
                if (cboName.Text.Replace(" ", "") == "")
                {
                    MessageBox.Show("종목명을 입력하세요.");
                    return;
                }

                int intSellQ;
                int i = 0; //반복문변수
                int intLeftQ;    //남은 갯수
                int intLeftP;   //남은 가격
                DataRow[] dr;

                intSellQ = Convert.ToInt32(txtQuatity.Text);

                //이름 체크
                dr = mDTHold.Tables[0].Select("종목명 = '" + cboName.Text.ToUpper() + "'");
                if (dr.Length != 1)
                {
                    MessageBox.Show("해당하는 종목이 매수한 종목에 없습니다.");
                    return;
                }

                if (Convert.ToInt32(dr[0]["수량"].ToString()) < Convert.ToInt32(txtQuatity.Text))
                {
                    MessageBox.Show("매수한 수량보다 많습니다.");
                    return;
                }

                strSQL += @"
                            INSERT INTO Sell (Date, Name, Price, Quantity)
                            VALUES ( '" + dte.Value.ToString("yyyyMMdd") + @"'
                                     , '" + cboName.Text.ToUpper() + @"'
                                     , '" + txtPrice.Text + @"'
                                     , '" + txtQuatity.Text + @"'
                                   );
                         ";

                while (intSellQ > 0)
                {
                    dr = mDTHold.Tables[1].Select("Left <> 0 AND Name = '" + cboName.Text.ToUpper() + "'", "Seq ASC");
                    intLeftQ = Convert.ToInt32(dr[i]["Left"].ToString());
                    intLeftP = Convert.ToInt32(dr[i]["Price"].ToString());

                    if (intSellQ >= intLeftQ)
                    {
                        strSQL += @"UPDATE Buy
                                       SET Left = 0
                                     WHERE Seq = '" + dr[i]["Seq"].ToString() + @"';
                                   ";

                        strSQL += @"INSERT INTO Revenue (Date, Name, BPrice, SPrice, Quantity, SellSeq)
                                    VALUES ( '" + dte.Value.ToString("yyyyMMdd") + @"'
                                             , '" + cboName.Text.ToUpper() + @"'
                                             , '" + intLeftP + @"'
                                             , '" + txtPrice.Text + @"'
                                             , '" + intLeftQ + @"'
                                             , CASE WHEN (SELECT COUNT(*) FROM Sell) = 0
                                                    THEN 1
                                                    ELSE (SELECT MAX(Seq) FROM Sell) END
                                           );
                                   ";
                        intSellQ = intSellQ - intLeftQ;
                    }

                    else
                    {
                        strSQL += @"UPDATE Buy
                                       SET Left = " + (intLeftQ - intSellQ) + @"
                                     WHERE Seq = '" + dr[i]["Seq"].ToString() + @"';
                                   ";

                        strSQL += @"INSERT INTO Revenue (Date, Name, BPrice, SPrice, Quantity, SellSeq)
                                    VALUES ( '" + dte.Value.ToString("yyyyMMdd") + @"'
                                             , '" + cboName.Text.ToUpper() + @"'
                                             , '" + intLeftP + @"'
                                             , '" + txtPrice.Text + @"'
                                             , '" + intSellQ + @"'
                                             , CASE WHEN (SELECT COUNT(*) FROM Sell) = 0
                                                    THEN 1
                                                    ELSE (SELECT MAX(Seq) FROM Sell) END
                                            );
                                   ";

                        intSellQ = 0;
                    }

                    i++;
                }
            }

            int iresult = Network.ExecDB(strSQL);

            if (iresult > 0)
            {
                MessageBox.Show("저장되었습니다.");
            }
            else
            {
                MessageBox.Show("저장에 실패하였습니다.");
            }

            txtPrice.Clear();
            cboStoSearch.Text = "";
            txtQuatity.Clear();
        }
        #endregion 입력 함수_End

        #region 현재보유량 SELECT

        /// <summary>
        /// 현재보유량 SELECT
        /// </summary>
        private void fnHoldSearch()
        {
            string strSQL = "";
            mDTHold = new DataSet();

            strSQL = @"SELECT MAX(Name) AS 종목명
                            , SUM(Left * Price) / SUM(Left) AS 평균가
                            , SUM(Left) AS 수량 
                            , SUM(Left * Price) AS 합계  
                         FROM Buy
                        WHERE Left > 0
                        GROUP BY Name;";
            
            strSQL += @"SELECT * 
                          FROM Buy;";

            strSQL += @"SELECT *
                          FROM Code;";

            mDTHold = Network.GetDBSet(strSQL);

            //dgvHold.DataSource = mDTHold.Tables[0];

            //컬럼추가
            //fnColumnAdd(dgvHold);

            // 컬럼정렬
            //fnSortGrid(dgvHold, true);

            //매도 콤보 아이템 초기화
            cboName.Items.Clear();
            foreach (DataRow dr in mDTHold.Tables[0].Rows)
            {
                if (!cboName.Items.Contains(dr["종목명"].ToString()))
                {
                    cboName.Items.Add(dr["종목명"].ToString());
                }
            }
        }

        #endregion 현재보유량 SELECT_End


        //*****************이벤트******************//

        #region 거래종류
        private void cboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboType.Text == "매수")
            {
                cboStoSearch.Visible = true;
                cboName.Visible = false;
            }
            else if (cboType.Text == "매도")
            {
                cboName.Visible = true;
                cboStoSearch.Visible = false;
            }
        }
        #endregion 거래종류

        #region 종목명
        private void cboName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboType.Text == "매수")
            {
                cboStoSearch.Visible = true;
                cboName.Visible = false;
            }
            else if (cboType.Text == "매도")
            {
                cboName.Visible = true;
                cboStoSearch.Visible = false;
            }
        }
        #endregion 종목명_End

        #region 입력버튼
        private void btnInput_Click(object sender, EventArgs e)
        {
            efnDeal();
            fnHoldSearch();
        }
        #endregion 입력버튼_End

        #region 가격_숫자만 입력되도록 필터링
        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
            }
        }
        #endregion 가격_숫자만 입력되도록 필터링_End

        #region 수량_숫자만 입력되도록 필터링
        private void txtQuatity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
            }
        }
        #endregion 수량_숫자만 입력되도록 필터링_End

        #region 콤보박스 읽기전용으로 만들기
        private void cboType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Down && e.KeyCode != Keys.Up)
            {
                e.SuppressKeyPress = true;
            }
        }
        #endregion 콤보박스 읽기전용으로 만들기_End


    }
}