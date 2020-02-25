namespace StockCalc
{
    partial class ViewStock
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
            this.components = new System.ComponentModel.Container();
            this.dgvView = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSelectOpen = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.cboStoSearch = new System.Windows.Forms.ComboBox();
            this.btnChg = new System.Windows.Forms.Button();
            this.btnDel2 = new System.Windows.Forms.Button();
            this.btnAdd2 = new System.Windows.Forms.Button();
            this.cboInter = new System.Windows.Forms.ComboBox();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.labHold = new System.Windows.Forms.Label();
            this.labInterest = new System.Windows.Forms.Label();
            this.txtMemo = new System.Windows.Forms.TextBox();
            this.labMainMemo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvView)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvView
            // 
            this.dgvView.AllowUserToAddRows = false;
            this.dgvView.AllowUserToDeleteRows = false;
            this.dgvView.AllowUserToResizeRows = false;
            this.dgvView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvView.Location = new System.Drawing.Point(-4, 34);
            this.dgvView.MultiSelect = false;
            this.dgvView.Name = "dgvView";
            this.dgvView.ReadOnly = true;
            this.dgvView.RowHeadersVisible = false;
            this.dgvView.RowTemplate.Height = 23;
            this.dgvView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvView.Size = new System.Drawing.Size(391, 262);
            this.dgvView.TabIndex = 4;
            this.dgvView.TabStop = false;
            this.dgvView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvView_MouseDown);
            this.dgvView.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvView_CellMouseClick);
            this.dgvView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvView_CellDoubleClick);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnSelectOpen);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Location = new System.Drawing.Point(10, 9);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(406, 307);
            this.panel1.TabIndex = 5;
            // 
            // btnSelectOpen
            // 
            this.btnSelectOpen.Location = new System.Drawing.Point(0, 0);
            this.btnSelectOpen.Name = "btnSelectOpen";
            this.btnSelectOpen.Size = new System.Drawing.Size(13, 14);
            this.btnSelectOpen.TabIndex = 6;
            this.btnSelectOpen.UseVisualStyleBackColor = true;
            this.btnSelectOpen.Visible = false;
            this.btnSelectOpen.Click += new System.EventHandler(this.btnSelectOpen_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel2.Controls.Add(this.txtMemo);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.dgvView);
            this.panel2.Location = new System.Drawing.Point(3, 10);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(391, 292);
            this.panel2.TabIndex = 5;
            // 
            // panel3
            // 
            this.panel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel3.Controls.Add(this.cboStoSearch);
            this.panel3.Controls.Add(this.btnChg);
            this.panel3.Controls.Add(this.btnDel2);
            this.panel3.Controls.Add(this.btnAdd2);
            this.panel3.Controls.Add(this.cboInter);
            this.panel3.Controls.Add(this.btnDown);
            this.panel3.Controls.Add(this.btnUp);
            this.panel3.Controls.Add(this.btnDel);
            this.panel3.Controls.Add(this.btnAdd);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(391, 27);
            this.panel3.TabIndex = 5;
            // 
            // cboStoSearch
            // 
            this.cboStoSearch.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboStoSearch.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboStoSearch.FormattingEnabled = true;
            this.cboStoSearch.Location = new System.Drawing.Point(5, 4);
            this.cboStoSearch.Name = "cboStoSearch";
            this.cboStoSearch.Size = new System.Drawing.Size(80, 20);
            this.cboStoSearch.TabIndex = 41;
            // 
            // btnChg
            // 
            this.btnChg.Location = new System.Drawing.Point(352, 3);
            this.btnChg.Name = "btnChg";
            this.btnChg.Size = new System.Drawing.Size(37, 22);
            this.btnChg.TabIndex = 9;
            this.btnChg.Tag = "Chg";
            this.btnChg.Text = "변경";
            this.btnChg.UseVisualStyleBackColor = true;
            this.btnChg.Click += new System.EventHandler(this.btnChg_Click);
            // 
            // btnDel2
            // 
            this.btnDel2.Location = new System.Drawing.Point(314, 3);
            this.btnDel2.Name = "btnDel2";
            this.btnDel2.Size = new System.Drawing.Size(37, 22);
            this.btnDel2.TabIndex = 8;
            this.btnDel2.Tag = "Del2";
            this.btnDel2.Text = "삭제";
            this.btnDel2.UseVisualStyleBackColor = true;
            this.btnDel2.Click += new System.EventHandler(this.btnDel2_Click);
            // 
            // btnAdd2
            // 
            this.btnAdd2.Location = new System.Drawing.Point(276, 3);
            this.btnAdd2.Name = "btnAdd2";
            this.btnAdd2.Size = new System.Drawing.Size(37, 22);
            this.btnAdd2.TabIndex = 7;
            this.btnAdd2.Tag = "Add2";
            this.btnAdd2.Text = "추가";
            this.btnAdd2.UseVisualStyleBackColor = true;
            this.btnAdd2.Click += new System.EventHandler(this.btnAdd2_Click);
            // 
            // cboInter
            // 
            this.cboInter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboInter.FormattingEnabled = true;
            this.cboInter.Location = new System.Drawing.Point(204, 4);
            this.cboInter.Name = "cboInter";
            this.cboInter.Size = new System.Drawing.Size(71, 20);
            this.cboInter.TabIndex = 6;
            this.cboInter.SelectedIndexChanged += new System.EventHandler(this.cboInter_SelectedIndexChanged);
            this.cboInter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboInter_KeyDown);
            // 
            // btnDown
            // 
            this.btnDown.Location = new System.Drawing.Point(183, 2);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(18, 23);
            this.btnDown.TabIndex = 4;
            this.btnDown.Text = "▽";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnUp
            // 
            this.btnUp.Location = new System.Drawing.Point(162, 2);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(18, 23);
            this.btnUp.TabIndex = 3;
            this.btnUp.Text = "△";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(124, 2);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(37, 23);
            this.btnDel.TabIndex = 2;
            this.btnDel.Text = "삭제";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(87, 2);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(37, 23);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "추가";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 60000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // labHold
            // 
            this.labHold.AutoSize = true;
            this.labHold.Location = new System.Drawing.Point(34, 5);
            this.labHold.Name = "labHold";
            this.labHold.Size = new System.Drawing.Size(57, 12);
            this.labHold.TabIndex = 16;
            this.labHold.Text = "현재 주식";
            this.labHold.Click += new System.EventHandler(this.labHold_Click);
            // 
            // labInterest
            // 
            this.labInterest.AutoSize = true;
            this.labInterest.Location = new System.Drawing.Point(97, 5);
            this.labInterest.Name = "labInterest";
            this.labInterest.Size = new System.Drawing.Size(57, 12);
            this.labInterest.TabIndex = 17;
            this.labInterest.Text = "관심 종목";
            this.labInterest.Click += new System.EventHandler(this.labInterest_Click);
            // 
            // txtMemo
            // 
            this.txtMemo.Location = new System.Drawing.Point(265, 180);
            this.txtMemo.Multiline = true;
            this.txtMemo.Name = "txtMemo";
            this.txtMemo.Size = new System.Drawing.Size(122, 112);
            this.txtMemo.TabIndex = 6;
            this.txtMemo.Leave += new System.EventHandler(this.txtMemo_Leave);
            // 
            // labMainMemo
            // 
            this.labMainMemo.AutoSize = true;
            this.labMainMemo.Location = new System.Drawing.Point(162, 5);
            this.labMainMemo.Name = "labMainMemo";
            this.labMainMemo.Size = new System.Drawing.Size(57, 12);
            this.labMainMemo.TabIndex = 18;
            this.labMainMemo.Text = "메인 메모";
            this.labMainMemo.Click += new System.EventHandler(this.labMainMemo_Click);
            // 
            // ViewStock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(427, 325);
            this.Controls.Add(this.labMainMemo);
            this.Controls.Add(this.labHold);
            this.Controls.Add(this.labInterest);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ViewStock";
            this.Text = "Hold";
            this.Load += new System.EventHandler(this.Hold_Load);
            this.Resize += new System.EventHandler(this.Hold_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.dgvView)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvView;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label labHold;
        private System.Windows.Forms.Label labInterest;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnSelectOpen;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.ComboBox cboInter;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnDel2;
        private System.Windows.Forms.Button btnAdd2;
        private System.Windows.Forms.Button btnChg;
        private System.Windows.Forms.ComboBox cboStoSearch;
        private System.Windows.Forms.TextBox txtMemo;
        private System.Windows.Forms.Label labMainMemo;
    }
}