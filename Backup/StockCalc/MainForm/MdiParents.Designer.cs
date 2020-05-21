namespace StockCalc
{
    partial class MdiParents
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MdiParents));
            this.btnDetail = new System.Windows.Forms.Button();
            this.btnRev = new System.Windows.Forms.Button();
            this.btnCode = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.종료ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSync = new System.Windows.Forms.Button();
            this.btnSetting = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnSelectOpen = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnInitVDB = new System.Windows.Forms.Button();
            this.cboDB = new System.Windows.Forms.ComboBox();
            this.btnMemo = new System.Windows.Forms.Button();
            this.btnInput = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOpt = new System.Windows.Forms.Button();
            this.btnQuery = new System.Windows.Forms.Button();
            this.btnCalc = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDetail
            // 
            this.btnDetail.Location = new System.Drawing.Point(92, 0);
            this.btnDetail.Name = "btnDetail";
            this.btnDetail.Size = new System.Drawing.Size(63, 21);
            this.btnDetail.TabIndex = 12;
            this.btnDetail.TabStop = false;
            this.btnDetail.Text = "Detail";
            this.btnDetail.UseVisualStyleBackColor = true;
            this.btnDetail.Click += new System.EventHandler(this.btnDetail_Click);
            // 
            // btnRev
            // 
            this.btnRev.Location = new System.Drawing.Point(166, 0);
            this.btnRev.Name = "btnRev";
            this.btnRev.Size = new System.Drawing.Size(63, 21);
            this.btnRev.TabIndex = 13;
            this.btnRev.TabStop = false;
            this.btnRev.Text = "Revenue";
            this.btnRev.UseVisualStyleBackColor = true;
            this.btnRev.Click += new System.EventHandler(this.btnRev_Click);
            // 
            // btnCode
            // 
            this.btnCode.Location = new System.Drawing.Point(9, 10);
            this.btnCode.Name = "btnCode";
            this.btnCode.Size = new System.Drawing.Size(63, 21);
            this.btnCode.TabIndex = 14;
            this.btnCode.Text = "Code";
            this.btnCode.UseVisualStyleBackColor = true;
            this.btnCode.Click += new System.EventHandler(this.btnCode_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "StockCalc";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.종료ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(99, 26);
            // 
            // 종료ToolStripMenuItem
            // 
            this.종료ToolStripMenuItem.Name = "종료ToolStripMenuItem";
            this.종료ToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.종료ToolStripMenuItem.Text = "종료";
            this.종료ToolStripMenuItem.Click += new System.EventHandler(this.종료ToolStripMenuItem_Click);
            // 
            // btnSync
            // 
            this.btnSync.Location = new System.Drawing.Point(83, 10);
            this.btnSync.Name = "btnSync";
            this.btnSync.Size = new System.Drawing.Size(63, 21);
            this.btnSync.TabIndex = 16;
            this.btnSync.TabStop = false;
            this.btnSync.Text = "Sync";
            this.btnSync.UseVisualStyleBackColor = true;
            this.btnSync.Click += new System.EventHandler(this.btnSync_Click);
            // 
            // btnSetting
            // 
            this.btnSetting.Location = new System.Drawing.Point(312, 0);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(22, 22);
            this.btnSetting.TabIndex = 17;
            this.btnSetting.TabStop = false;
            this.btnSetting.Text = "*";
            this.btnSetting.UseVisualStyleBackColor = true;
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel4.Location = new System.Drawing.Point(3, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(408, 195);
            this.panel4.TabIndex = 5;
            // 
            // btnSelectOpen
            // 
            this.btnSelectOpen.Location = new System.Drawing.Point(-1, -1);
            this.btnSelectOpen.Name = "btnSelectOpen";
            this.btnSelectOpen.Size = new System.Drawing.Size(13, 14);
            this.btnSelectOpen.TabIndex = 0;
            this.btnSelectOpen.UseVisualStyleBackColor = true;
            this.btnSelectOpen.Click += new System.EventHandler(this.btnSelectOpen_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.btnCalc);
            this.panel1.Controls.Add(this.btnInitVDB);
            this.panel1.Controls.Add(this.cboDB);
            this.panel1.Controls.Add(this.btnMemo);
            this.panel1.Controls.Add(this.btnInput);
            this.panel1.Controls.Add(this.btnRev);
            this.panel1.Controls.Add(this.btnDetail);
            this.panel1.Controls.Add(this.btnSetting);
            this.panel1.Location = new System.Drawing.Point(3, 202);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(408, 26);
            this.panel1.TabIndex = 18;
            // 
            // btnInitVDB
            // 
            this.btnInitVDB.Location = new System.Drawing.Point(3, 3);
            this.btnInitVDB.Name = "btnInitVDB";
            this.btnInitVDB.Size = new System.Drawing.Size(13, 14);
            this.btnInitVDB.TabIndex = 20;
            this.btnInitVDB.UseVisualStyleBackColor = true;
            this.btnInitVDB.Visible = false;
            this.btnInitVDB.Click += new System.EventHandler(this.btnInitVDB_Click);
            // 
            // cboDB
            // 
            this.cboDB.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboDB.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboDB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDB.FormattingEnabled = true;
            this.cboDB.Location = new System.Drawing.Point(365, 1);
            this.cboDB.Name = "cboDB";
            this.cboDB.Size = new System.Drawing.Size(38, 20);
            this.cboDB.TabIndex = 0;
            this.cboDB.SelectedIndexChanged += new System.EventHandler(this.cboDB_SelectedIndexChanged);
            // 
            // btnMemo
            // 
            this.btnMemo.Location = new System.Drawing.Point(240, 0);
            this.btnMemo.Name = "btnMemo";
            this.btnMemo.Size = new System.Drawing.Size(63, 21);
            this.btnMemo.TabIndex = 19;
            this.btnMemo.TabStop = false;
            this.btnMemo.Text = "Memo";
            this.btnMemo.UseVisualStyleBackColor = true;
            this.btnMemo.Click += new System.EventHandler(this.btnMemo_Click);
            // 
            // btnInput
            // 
            this.btnInput.Location = new System.Drawing.Point(18, 0);
            this.btnInput.Name = "btnInput";
            this.btnInput.Size = new System.Drawing.Size(63, 21);
            this.btnInput.TabIndex = 18;
            this.btnInput.TabStop = false;
            this.btnInput.Text = "Input";
            this.btnInput.UseVisualStyleBackColor = true;
            this.btnInput.Click += new System.EventHandler(this.btnInput_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.btnOpt);
            this.panel2.Controls.Add(this.btnQuery);
            this.panel2.Controls.Add(this.btnCode);
            this.panel2.Controls.Add(this.btnSync);
            this.panel2.Location = new System.Drawing.Point(98, 157);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(304, 43);
            this.panel2.TabIndex = 0;
            this.panel2.Visible = false;
            // 
            // btnOpt
            // 
            this.btnOpt.Location = new System.Drawing.Point(229, 11);
            this.btnOpt.Name = "btnOpt";
            this.btnOpt.Size = new System.Drawing.Size(63, 21);
            this.btnOpt.TabIndex = 18;
            this.btnOpt.TabStop = false;
            this.btnOpt.Text = "Option";
            this.btnOpt.UseVisualStyleBackColor = true;
            this.btnOpt.Click += new System.EventHandler(this.btnOpt_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(156, 11);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(63, 21);
            this.btnQuery.TabIndex = 17;
            this.btnQuery.TabStop = false;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // btnCalc
            // 
            this.btnCalc.Location = new System.Drawing.Point(337, 0);
            this.btnCalc.Name = "btnCalc";
            this.btnCalc.Size = new System.Drawing.Size(22, 22);
            this.btnCalc.TabIndex = 21;
            this.btnCalc.TabStop = false;
            this.btnCalc.Text = "c";
            this.btnCalc.UseVisualStyleBackColor = true;
            this.btnCalc.Click += new System.EventHandler(this.btnCalc_Click);
            // 
            // MdiParents
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 227);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.btnSelectOpen);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MdiParents";
            this.Text = "Calc";
            this.Load += new System.EventHandler(this.StockCalc_Load);
            this.Shown += new System.EventHandler(this.MdiParents_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StockCalc_FormClosing);
            this.Resize += new System.EventHandler(this.MdiParents_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDetail;
        private System.Windows.Forms.Button btnRev;
        private System.Windows.Forms.Button btnCode;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 종료ToolStripMenuItem;
        private System.Windows.Forms.Button btnSync;
        private System.Windows.Forms.Button btnSetting;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnSelectOpen;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.Button btnInput;
        private System.Windows.Forms.Button btnMemo;
        private System.Windows.Forms.Button btnOpt;
        private System.Windows.Forms.ComboBox cboDB;
        private System.Windows.Forms.Button btnInitVDB;
        private System.Windows.Forms.Button btnCalc;


    }
}

