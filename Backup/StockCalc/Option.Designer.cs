namespace StockCalc
{
    partial class Option
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
            this.chkSync = new System.Windows.Forms.CheckBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSaveClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rdoCurrent = new System.Windows.Forms.RadioButton();
            this.rdoInterest = new System.Windows.Forms.RadioButton();
            this.rdoMainMemo = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkSync
            // 
            this.chkSync.AutoSize = true;
            this.chkSync.Checked = true;
            this.chkSync.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSync.Location = new System.Drawing.Point(17, 17);
            this.chkSync.Name = "chkSync";
            this.chkSync.Size = new System.Drawing.Size(100, 16);
            this.chkSync.TabIndex = 0;
            this.chkSync.Text = "서버와 동기화";
            this.chkSync.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(181, 206);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "닫기";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSaveClose
            // 
            this.btnSaveClose.Location = new System.Drawing.Point(87, 206);
            this.btnSaveClose.Name = "btnSaveClose";
            this.btnSaveClose.Size = new System.Drawing.Size(88, 23);
            this.btnSaveClose.TabIndex = 5;
            this.btnSaveClose.Text = "저장 후 닫기";
            this.btnSaveClose.UseVisualStyleBackColor = true;
            this.btnSaveClose.Click += new System.EventHandler(this.btnSaveClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(6, 206);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.chkSync);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnSaveClose);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(260, 237);
            this.panel1.TabIndex = 7;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.rdoMainMemo);
            this.panel2.Controls.Add(this.rdoInterest);
            this.panel2.Controls.Add(this.rdoCurrent);
            this.panel2.Location = new System.Drawing.Point(17, 44);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(227, 37);
            this.panel2.TabIndex = 7;
            // 
            // rdoCurrent
            // 
            this.rdoCurrent.AutoSize = true;
            this.rdoCurrent.Checked = true;
            this.rdoCurrent.Location = new System.Drawing.Point(9, 13);
            this.rdoCurrent.Name = "rdoCurrent";
            this.rdoCurrent.Size = new System.Drawing.Size(47, 16);
            this.rdoCurrent.TabIndex = 0;
            this.rdoCurrent.TabStop = true;
            this.rdoCurrent.Text = "현재";
            this.rdoCurrent.UseVisualStyleBackColor = true;
            // 
            // rdoInterest
            // 
            this.rdoInterest.AutoSize = true;
            this.rdoInterest.Location = new System.Drawing.Point(87, 13);
            this.rdoInterest.Name = "rdoInterest";
            this.rdoInterest.Size = new System.Drawing.Size(47, 16);
            this.rdoInterest.TabIndex = 1;
            this.rdoInterest.Text = "관심";
            this.rdoInterest.UseVisualStyleBackColor = true;
            // 
            // rdoMainMemo
            // 
            this.rdoMainMemo.AutoSize = true;
            this.rdoMainMemo.Location = new System.Drawing.Point(162, 13);
            this.rdoMainMemo.Name = "rdoMainMemo";
            this.rdoMainMemo.Size = new System.Drawing.Size(47, 16);
            this.rdoMainMemo.TabIndex = 2;
            this.rdoMainMemo.Text = "메모";
            this.rdoMainMemo.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "첫 화면";
            // 
            // Option
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.panel1);
            this.Name = "Option";
            this.Text = "Option";
            this.Load += new System.EventHandler(this.Option_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkSync;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSaveClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton rdoMainMemo;
        private System.Windows.Forms.RadioButton rdoInterest;
        private System.Windows.Forms.RadioButton rdoCurrent;
        private System.Windows.Forms.Label label1;
    }
}