namespace StockCalc
{
    partial class MemoMain
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MemoMain));
            this.btnNew = new System.Windows.Forms.Button();
            this.dteF = new System.Windows.Forms.DateTimePicker();
            this.dteS = new System.Windows.Forms.DateTimePicker();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.chkTotal = new System.Windows.Forms.CheckBox();
            this.chkRed = new System.Windows.Forms.CheckBox();
            this.chkYellow = new System.Windows.Forms.CheckBox();
            this.chkBlue = new System.Windows.Forms.CheckBox();
            this.chkGray = new System.Windows.Forms.CheckBox();
            this.chkWhite = new System.Windows.Forms.CheckBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.chkContents = new System.Windows.Forms.CheckBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.chkTitle = new System.Windows.Forms.CheckBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cboSmall = new System.Windows.Forms.ComboBox();
            this.cboMain = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnAfter = new System.Windows.Forms.Label();
            this.btnBefore = new System.Windows.Forms.Label();
            this.chkDate = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnModi = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnNew
            // 
            this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNew.Location = new System.Drawing.Point(218, 357);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 23);
            this.btnNew.TabIndex = 0;
            this.btnNew.Text = "신규";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // dteF
            // 
            this.dteF.Location = new System.Drawing.Point(99, 3);
            this.dteF.Name = "dteF";
            this.dteF.Size = new System.Drawing.Size(113, 21);
            this.dteF.TabIndex = 1;
            this.dteF.Value = new System.DateTime(2018, 3, 20, 0, 0, 0, 0);
            // 
            // dteS
            // 
            this.dteS.Location = new System.Drawing.Point(241, 2);
            this.dteS.Name = "dteS";
            this.dteS.Size = new System.Drawing.Size(117, 21);
            this.dteS.TabIndex = 3;
            this.dteS.Value = new System.DateTime(2018, 3, 20, 0, 0, 0, 0);
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Location = new System.Drawing.Point(4, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(713, 93);
            this.panel1.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 70);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(25, 12);
            this.label5.TabIndex = 15;
            this.label5.Text = "색 :";
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.chkTotal);
            this.panel5.Controls.Add(this.chkRed);
            this.panel5.Controls.Add(this.chkYellow);
            this.panel5.Controls.Add(this.chkBlue);
            this.panel5.Controls.Add(this.chkGray);
            this.panel5.Controls.Add(this.chkWhite);
            this.panel5.Location = new System.Drawing.Point(35, 64);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(347, 24);
            this.panel5.TabIndex = 11;
            // 
            // chkTotal
            // 
            this.chkTotal.AutoSize = true;
            this.chkTotal.Checked = true;
            this.chkTotal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTotal.Location = new System.Drawing.Point(4, 3);
            this.chkTotal.Name = "chkTotal";
            this.chkTotal.Size = new System.Drawing.Size(64, 16);
            this.chkTotal.TabIndex = 14;
            this.chkTotal.Text = "<전체>";
            this.chkTotal.UseVisualStyleBackColor = true;
            this.chkTotal.CheckedChanged += new System.EventHandler(this.chkTotal_CheckedChanged);
            // 
            // chkRed
            // 
            this.chkRed.AutoSize = true;
            this.chkRed.Checked = true;
            this.chkRed.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRed.Location = new System.Drawing.Point(290, 3);
            this.chkRed.Name = "chkRed";
            this.chkRed.Size = new System.Drawing.Size(48, 16);
            this.chkRed.TabIndex = 12;
            this.chkRed.Text = "빨강";
            this.chkRed.UseVisualStyleBackColor = true;
            this.chkRed.CheckedChanged += new System.EventHandler(this.chkRed_CheckedChanged);
            // 
            // chkYellow
            // 
            this.chkYellow.AutoSize = true;
            this.chkYellow.Checked = true;
            this.chkYellow.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkYellow.Location = new System.Drawing.Point(235, 3);
            this.chkYellow.Name = "chkYellow";
            this.chkYellow.Size = new System.Drawing.Size(48, 16);
            this.chkYellow.TabIndex = 13;
            this.chkYellow.Text = "노랑";
            this.chkYellow.UseVisualStyleBackColor = true;
            this.chkYellow.CheckedChanged += new System.EventHandler(this.chkYellow_CheckedChanged);
            // 
            // chkBlue
            // 
            this.chkBlue.AutoSize = true;
            this.chkBlue.Checked = true;
            this.chkBlue.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBlue.Location = new System.Drawing.Point(180, 3);
            this.chkBlue.Name = "chkBlue";
            this.chkBlue.Size = new System.Drawing.Size(48, 16);
            this.chkBlue.TabIndex = 12;
            this.chkBlue.Text = "하늘";
            this.chkBlue.UseVisualStyleBackColor = true;
            this.chkBlue.CheckedChanged += new System.EventHandler(this.chkBlue_CheckedChanged);
            // 
            // chkGray
            // 
            this.chkGray.AutoSize = true;
            this.chkGray.Checked = true;
            this.chkGray.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGray.Location = new System.Drawing.Point(125, 3);
            this.chkGray.Name = "chkGray";
            this.chkGray.Size = new System.Drawing.Size(48, 16);
            this.chkGray.TabIndex = 12;
            this.chkGray.Text = "회색";
            this.chkGray.UseVisualStyleBackColor = true;
            this.chkGray.CheckedChanged += new System.EventHandler(this.chkGray_CheckedChanged);
            // 
            // chkWhite
            // 
            this.chkWhite.AutoSize = true;
            this.chkWhite.Checked = true;
            this.chkWhite.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWhite.Location = new System.Drawing.Point(70, 3);
            this.chkWhite.Name = "chkWhite";
            this.chkWhite.Size = new System.Drawing.Size(48, 16);
            this.chkWhite.TabIndex = 12;
            this.chkWhite.Text = "흰색";
            this.chkWhite.UseVisualStyleBackColor = true;
            this.chkWhite.CheckedChanged += new System.EventHandler(this.chkWhite_CheckedChanged);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.chkContents);
            this.panel4.Controls.Add(this.btnSearch);
            this.panel4.Controls.Add(this.chkTitle);
            this.panel4.Controls.Add(this.txtSearch);
            this.panel4.Location = new System.Drawing.Point(422, 32);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(282, 55);
            this.panel4.TabIndex = 6;
            // 
            // chkContents
            // 
            this.chkContents.AutoSize = true;
            this.chkContents.Location = new System.Drawing.Point(141, 12);
            this.chkContents.Name = "chkContents";
            this.chkContents.Size = new System.Drawing.Size(48, 16);
            this.chkContents.TabIndex = 9;
            this.chkContents.Text = "내용";
            this.chkContents.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(209, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(65, 49);
            this.btnSearch.TabIndex = 10;
            this.btnSearch.Text = "검색";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // chkTitle
            // 
            this.chkTitle.AutoSize = true;
            this.chkTitle.Checked = true;
            this.chkTitle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTitle.Location = new System.Drawing.Point(87, 12);
            this.chkTitle.Name = "chkTitle";
            this.chkTitle.Size = new System.Drawing.Size(48, 16);
            this.chkTitle.TabIndex = 8;
            this.chkTitle.Text = "제목";
            this.chkTitle.UseVisualStyleBackColor = true;
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(17, 31);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(184, 21);
            this.txtSearch.TabIndex = 4;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.cboSmall);
            this.panel3.Controls.Add(this.cboMain);
            this.panel3.Location = new System.Drawing.Point(3, 32);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(413, 26);
            this.panel3.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(210, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 12);
            this.label3.TabIndex = 12;
            this.label3.Text = "소분류 :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "대분류 :";
            // 
            // cboSmall
            // 
            this.cboSmall.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSmall.FormattingEnabled = true;
            this.cboSmall.Location = new System.Drawing.Point(261, 3);
            this.cboSmall.Name = "cboSmall";
            this.cboSmall.Size = new System.Drawing.Size(118, 20);
            this.cboSmall.TabIndex = 8;
            // 
            // cboMain
            // 
            this.cboMain.BackColor = System.Drawing.Color.White;
            this.cboMain.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMain.FormattingEnabled = true;
            this.cboMain.Location = new System.Drawing.Point(52, 3);
            this.cboMain.Name = "cboMain";
            this.cboMain.Size = new System.Drawing.Size(118, 20);
            this.cboMain.TabIndex = 6;
            this.cboMain.SelectedIndexChanged += new System.EventHandler(this.cboMain_SelectedIndexChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnAfter);
            this.panel2.Controls.Add(this.btnBefore);
            this.panel2.Controls.Add(this.chkDate);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.dteF);
            this.panel2.Controls.Add(this.dteS);
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(413, 26);
            this.panel2.TabIndex = 6;
            // 
            // btnAfter
            // 
            this.btnAfter.AutoSize = true;
            this.btnAfter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.btnAfter.Location = new System.Drawing.Point(364, 6);
            this.btnAfter.Name = "btnAfter";
            this.btnAfter.Size = new System.Drawing.Size(15, 14);
            this.btnAfter.TabIndex = 13;
            this.btnAfter.Text = ">";
            this.btnAfter.Click += new System.EventHandler(this.btnAfter_Click);
            // 
            // btnBefore
            // 
            this.btnBefore.AutoSize = true;
            this.btnBefore.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.btnBefore.Location = new System.Drawing.Point(78, 6);
            this.btnBefore.Name = "btnBefore";
            this.btnBefore.Size = new System.Drawing.Size(15, 14);
            this.btnBefore.TabIndex = 11;
            this.btnBefore.Text = "<";
            this.btnBefore.Click += new System.EventHandler(this.btnBefore_Click);
            // 
            // chkDate
            // 
            this.chkDate.AutoSize = true;
            this.chkDate.Location = new System.Drawing.Point(3, 7);
            this.chkDate.Name = "chkDate";
            this.chkDate.Size = new System.Drawing.Size(72, 16);
            this.chkDate.TabIndex = 12;
            this.chkDate.Text = "전체날짜";
            this.chkDate.UseVisualStyleBackColor = true;
            this.chkDate.CheckedChanged += new System.EventHandler(this.chkDate_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(218, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(11, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "-";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 111);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(697, 240);
            this.dataGridView1.TabIndex = 5;
            this.dataGridView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseDown);
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            // 
            // btnModi
            // 
            this.btnModi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnModi.Location = new System.Drawing.Point(299, 357);
            this.btnModi.Name = "btnModi";
            this.btnModi.Size = new System.Drawing.Size(75, 23);
            this.btnModi.TabIndex = 6;
            this.btnModi.Text = "수정";
            this.btnModi.UseVisualStyleBackColor = true;
            this.btnModi.Click += new System.EventHandler(this.btnModi_Click);
            // 
            // btnDel
            // 
            this.btnDel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDel.Location = new System.Drawing.Point(380, 357);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(75, 23);
            this.btnDel.TabIndex = 7;
            this.btnDel.Text = "삭제";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(627, 355);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 22);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "닫기";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // MemoMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(724, 392);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnModi);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnNew);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MemoMain";
            this.Text = "Arragement";
            this.Load += new System.EventHandler(this.MemoMain_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MemoMain_FormClosing);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.DateTimePicker dteF;
        private System.Windows.Forms.DateTimePicker dteS;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ComboBox cboSmall;
        private System.Windows.Forms.ComboBox cboMain;
        private System.Windows.Forms.CheckBox chkContents;
        private System.Windows.Forms.CheckBox chkTitle;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.CheckBox chkDate;
        private System.Windows.Forms.Label btnBefore;
        private System.Windows.Forms.Label btnAfter;
        private System.Windows.Forms.Button btnModi;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.CheckBox chkYellow;
        private System.Windows.Forms.CheckBox chkBlue;
        private System.Windows.Forms.CheckBox chkGray;
        private System.Windows.Forms.CheckBox chkWhite;
        private System.Windows.Forms.CheckBox chkRed;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkTotal;

    }
}

