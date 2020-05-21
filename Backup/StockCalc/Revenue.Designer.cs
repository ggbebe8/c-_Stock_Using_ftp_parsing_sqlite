namespace StockCalc
{
    partial class Revenue
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Revenue));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnAfter = new System.Windows.Forms.Label();
            this.btnBefore = new System.Windows.Forms.Label();
            this.chkDate = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dteF = new System.Windows.Forms.DateTimePicker();
            this.dteS = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.txtRevSum = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label9 = new System.Windows.Forms.Label();
            this.txtSumD = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtSumM = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSumY = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dgvDate = new System.Windows.Forms.DataGridView();
            this.dgvMonth = new System.Windows.Forms.DataGridView();
            this.dgvYear = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMonth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvYear)).BeginInit();
            this.SuspendLayout();
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
            this.dataGridView1.Location = new System.Drawing.Point(6, 92);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(800, 414);
            this.dataGridView1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.txtName);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Location = new System.Drawing.Point(110, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(556, 67);
            this.panel1.TabIndex = 1;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(441, 11);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 44);
            this.btnSearch.TabIndex = 17;
            this.btnSearch.Text = "검색";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(56, 41);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(159, 21);
            this.txtName.TabIndex = 16;
            this.txtName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtName_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 12);
            this.label2.TabIndex = 15;
            this.label2.Text = "이름 :";
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
            this.panel2.Size = new System.Drawing.Size(396, 26);
            this.panel2.TabIndex = 7;
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
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(672, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 12);
            this.label3.TabIndex = 18;
            this.label3.Text = "수익합 :";
            // 
            // txtRevSum
            // 
            this.txtRevSum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRevSum.BackColor = System.Drawing.SystemColors.Control;
            this.txtRevSum.Location = new System.Drawing.Point(727, 62);
            this.txtRevSum.Name = "txtRevSum";
            this.txtRevSum.ReadOnly = true;
            this.txtRevSum.Size = new System.Drawing.Size(65, 21);
            this.txtRevSum.TabIndex = 2;
            this.txtRevSum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(820, 538);
            this.tabControl1.TabIndex = 19;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.txtRevSum);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(812, 512);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "수익검색";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Controls.Add(this.txtSumD);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.txtSumM);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.txtSumY);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.dgvDate);
            this.tabPage2.Controls.Add(this.dgvMonth);
            this.tabPage2.Controls.Add(this.dgvYear);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(812, 512);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "기간별수익";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(668, 486);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(49, 12);
            this.label9.TabIndex = 24;
            this.label9.Text = "수익합 :";
            // 
            // txtSumD
            // 
            this.txtSumD.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.txtSumD.BackColor = System.Drawing.SystemColors.Control;
            this.txtSumD.Location = new System.Drawing.Point(723, 482);
            this.txtSumD.Name = "txtSumD";
            this.txtSumD.ReadOnly = true;
            this.txtSumD.Size = new System.Drawing.Size(65, 21);
            this.txtSumD.TabIndex = 23;
            this.txtSumD.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(400, 486);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 12);
            this.label8.TabIndex = 22;
            this.label8.Text = "수익합 :";
            // 
            // txtSumM
            // 
            this.txtSumM.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.txtSumM.BackColor = System.Drawing.SystemColors.Control;
            this.txtSumM.Location = new System.Drawing.Point(455, 482);
            this.txtSumM.Name = "txtSumM";
            this.txtSumM.ReadOnly = true;
            this.txtSumM.Size = new System.Drawing.Size(65, 21);
            this.txtSumM.TabIndex = 21;
            this.txtSumM.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(128, 486);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 12);
            this.label7.TabIndex = 20;
            this.label7.Text = "수익합 :";
            // 
            // txtSumY
            // 
            this.txtSumY.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.txtSumY.BackColor = System.Drawing.SystemColors.Control;
            this.txtSumY.Location = new System.Drawing.Point(183, 482);
            this.txtSumY.Name = "txtSumY";
            this.txtSumY.ReadOnly = true;
            this.txtSumY.Size = new System.Drawing.Size(65, 21);
            this.txtSumY.TabIndex = 19;
            this.txtSumY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(660, 38);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 12);
            this.label6.TabIndex = 18;
            this.label6.Text = "<Date>";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(381, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 12);
            this.label5.TabIndex = 17;
            this.label5.Text = "<Month>";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(117, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 12);
            this.label4.TabIndex = 16;
            this.label4.Text = "<Year>";
            // 
            // dgvDate
            // 
            this.dgvDate.AllowUserToAddRows = false;
            this.dgvDate.AllowUserToDeleteRows = false;
            this.dgvDate.AllowUserToResizeRows = false;
            this.dgvDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.dgvDate.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDate.Location = new System.Drawing.Point(571, 62);
            this.dgvDate.MultiSelect = false;
            this.dgvDate.Name = "dgvDate";
            this.dgvDate.ReadOnly = true;
            this.dgvDate.RowHeadersVisible = false;
            this.dgvDate.RowTemplate.Height = 23;
            this.dgvDate.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDate.Size = new System.Drawing.Size(217, 414);
            this.dgvDate.TabIndex = 3;
            // 
            // dgvMonth
            // 
            this.dgvMonth.AllowUserToAddRows = false;
            this.dgvMonth.AllowUserToDeleteRows = false;
            this.dgvMonth.AllowUserToResizeRows = false;
            this.dgvMonth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.dgvMonth.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMonth.Location = new System.Drawing.Point(303, 62);
            this.dgvMonth.MultiSelect = false;
            this.dgvMonth.Name = "dgvMonth";
            this.dgvMonth.ReadOnly = true;
            this.dgvMonth.RowHeadersVisible = false;
            this.dgvMonth.RowTemplate.Height = 23;
            this.dgvMonth.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMonth.Size = new System.Drawing.Size(217, 414);
            this.dgvMonth.TabIndex = 2;
            this.dgvMonth.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvMonth_CellMouseClick);
            // 
            // dgvYear
            // 
            this.dgvYear.AllowUserToAddRows = false;
            this.dgvYear.AllowUserToDeleteRows = false;
            this.dgvYear.AllowUserToResizeRows = false;
            this.dgvYear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.dgvYear.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvYear.Location = new System.Drawing.Point(31, 62);
            this.dgvYear.MultiSelect = false;
            this.dgvYear.Name = "dgvYear";
            this.dgvYear.ReadOnly = true;
            this.dgvYear.RowHeadersVisible = false;
            this.dgvYear.RowTemplate.Height = 23;
            this.dgvYear.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvYear.Size = new System.Drawing.Size(217, 414);
            this.dgvYear.TabIndex = 1;
            this.dgvYear.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvYear_CellMouseClick);
            // 
            // Revenue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(844, 552);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Revenue";
            this.Text = "Report";
            this.Load += new System.EventHandler(this.Report_Load);
            this.Resize += new System.EventHandler(this.Report_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMonth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvYear)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label btnAfter;
        private System.Windows.Forms.Label btnBefore;
        private System.Windows.Forms.CheckBox chkDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dteF;
        private System.Windows.Forms.DateTimePicker dteS;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtRevSum;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dgvDate;
        private System.Windows.Forms.DataGridView dgvMonth;
        private System.Windows.Forms.DataGridView dgvYear;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtSumD;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtSumM;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSumY;
    }
}