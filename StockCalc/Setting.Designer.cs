namespace StockCalc
{
    partial class Setting
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
            this.btnSync = new System.Windows.Forms.Button();
            this.btnCode = new System.Windows.Forms.Button();
            this.btnSql = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPassWd = new System.Windows.Forms.MaskedTextBox();
            this.SuspendLayout();
            // 
            // btnSync
            // 
            this.btnSync.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSync.Location = new System.Drawing.Point(94, 12);
            this.btnSync.Name = "btnSync";
            this.btnSync.Size = new System.Drawing.Size(63, 21);
            this.btnSync.TabIndex = 18;
            this.btnSync.TabStop = false;
            this.btnSync.Text = "Sync";
            this.btnSync.UseVisualStyleBackColor = true;
            this.btnSync.Click += new System.EventHandler(this.btnSync_Click);
            // 
            // btnCode
            // 
            this.btnCode.Location = new System.Drawing.Point(12, 12);
            this.btnCode.Name = "btnCode";
            this.btnCode.Size = new System.Drawing.Size(63, 21);
            this.btnCode.TabIndex = 17;
            this.btnCode.Text = "Code";
            this.btnCode.UseVisualStyleBackColor = true;
            this.btnCode.Click += new System.EventHandler(this.btnCode_Click);
            // 
            // btnSql
            // 
            this.btnSql.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSql.Location = new System.Drawing.Point(178, 12);
            this.btnSql.Name = "btnSql";
            this.btnSql.Size = new System.Drawing.Size(63, 21);
            this.btnSql.TabIndex = 19;
            this.btnSql.TabStop = false;
            this.btnSql.Text = "Sql";
            this.btnSql.UseVisualStyleBackColor = true;
            this.btnSql.Click += new System.EventHandler(this.btnSql_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 12);
            this.label1.TabIndex = 20;
            this.label1.Text = "PW:";
            this.label1.Visible = false;
            // 
            // txtPassWd
            // 
            this.txtPassWd.Location = new System.Drawing.Point(69, 37);
            this.txtPassWd.Name = "txtPassWd";
            this.txtPassWd.PasswordChar = '*';
            this.txtPassWd.Size = new System.Drawing.Size(128, 21);
            this.txtPassWd.TabIndex = 22;
            this.txtPassWd.Visible = false;
            // 
            // Setting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(253, 38);
            this.Controls.Add(this.txtPassWd);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSql);
            this.Controls.Add(this.btnSync);
            this.Controls.Add(this.btnCode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Setting";
            this.Text = "Setting";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSync;
        private System.Windows.Forms.Button btnCode;
        private System.Windows.Forms.Button btnSql;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MaskedTextBox txtPassWd;
    }
}