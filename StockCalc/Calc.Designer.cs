namespace StockCalc
{
    partial class Calc
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
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtNum = new System.Windows.Forms.TextBox();
            this.btnSearch2 = new System.Windows.Forms.Button();
            this.btnSearch3 = new System.Windows.Forms.Button();
            this.btnDbInsert = new System.Windows.Forms.Button();
            this.txtDbList = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Location = new System.Drawing.Point(10, 58);
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(485, 222);
            this.txtLog.TabIndex = 0;
            this.txtLog.Text = "";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(195, 29);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(40, 30);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(100, 21);
            this.txtName.TabIndex = 1;
            this.txtName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtName_KeyDown);
            // 
            // txtNum
            // 
            this.txtNum.Location = new System.Drawing.Point(146, 29);
            this.txtNum.Name = "txtNum";
            this.txtNum.Size = new System.Drawing.Size(31, 21);
            this.txtNum.TabIndex = 2;
            this.txtNum.Text = "300";
            // 
            // btnSearch2
            // 
            this.btnSearch2.Location = new System.Drawing.Point(285, 29);
            this.btnSearch2.Name = "btnSearch2";
            this.btnSearch2.Size = new System.Drawing.Size(75, 23);
            this.btnSearch2.TabIndex = 4;
            this.btnSearch2.Text = "45일기준";
            this.btnSearch2.UseVisualStyleBackColor = true;
            this.btnSearch2.Click += new System.EventHandler(this.btnSearch2_Click);
            // 
            // btnSearch3
            // 
            this.btnSearch3.Location = new System.Drawing.Point(365, 5);
            this.btnSearch3.Name = "btnSearch3";
            this.btnSearch3.Size = new System.Drawing.Size(46, 23);
            this.btnSearch3.TabIndex = 5;
            this.btnSearch3.Text = "종합";
            this.btnSearch3.UseVisualStyleBackColor = true;
            this.btnSearch3.Click += new System.EventHandler(this.btnSearch3_Click);
            // 
            // btnDbInsert
            // 
            this.btnDbInsert.Location = new System.Drawing.Point(465, 5);
            this.btnDbInsert.Name = "btnDbInsert";
            this.btnDbInsert.Size = new System.Drawing.Size(30, 23);
            this.btnDbInsert.TabIndex = 6;
            this.btnDbInsert.Text = "@";
            this.btnDbInsert.UseVisualStyleBackColor = true;
            this.btnDbInsert.Click += new System.EventHandler(this.btnDbInsert_Click);
            // 
            // txtDbList
            // 
            this.txtDbList.Location = new System.Drawing.Point(41, 6);
            this.txtDbList.Name = "txtDbList";
            this.txtDbList.Size = new System.Drawing.Size(319, 21);
            this.txtDbList.TabIndex = 7;
            // 
            // Calc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 290);
            this.Controls.Add(this.txtDbList);
            this.Controls.Add(this.btnDbInsert);
            this.Controls.Add(this.btnSearch3);
            this.Controls.Add(this.btnSearch2);
            this.Controls.Add(this.txtNum);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtLog);
            this.Name = "Calc";
            this.Text = "Calc";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtLog;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtNum;
        private System.Windows.Forms.Button btnSearch2;
        private System.Windows.Forms.Button btnSearch3;
        private System.Windows.Forms.Button btnDbInsert;
        private System.Windows.Forms.TextBox txtDbList;
    }
}