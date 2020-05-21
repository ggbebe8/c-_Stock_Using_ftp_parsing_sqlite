namespace StockCalc
{
    partial class EnrollForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnrollForm));
            this.dte = new System.Windows.Forms.DateTimePicker();
            this.cboMain = new System.Windows.Forms.ComboBox();
            this.cboSmall = new System.Windows.Forms.ComboBox();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnSaveClose = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.rtxtContents = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnRed = new System.Windows.Forms.Button();
            this.btnBlack = new System.Windows.Forms.Button();
            this.btnBlue = new System.Windows.Forms.Button();
            this.btnSizeDown = new System.Windows.Forms.Button();
            this.btnSizeUp = new System.Windows.Forms.Button();
            this.btnThick = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.cboColor = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dte
            // 
            this.dte.Location = new System.Drawing.Point(69, 3);
            this.dte.Name = "dte";
            this.dte.Size = new System.Drawing.Size(113, 21);
            this.dte.TabIndex = 0;
            this.dte.Value = new System.DateTime(2018, 3, 20, 0, 0, 0, 0);
            // 
            // cboMain
            // 
            this.cboMain.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboMain.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboMain.BackColor = System.Drawing.Color.White;
            this.cboMain.FormattingEnabled = true;
            this.cboMain.Location = new System.Drawing.Point(256, 3);
            this.cboMain.Name = "cboMain";
            this.cboMain.Size = new System.Drawing.Size(118, 20);
            this.cboMain.TabIndex = 1;
            this.cboMain.SelectedIndexChanged += new System.EventHandler(this.cboMain_SelectedIndexChanged);
            this.cboMain.Leave += new System.EventHandler(this.cboMain_Leave);
            // 
            // cboSmall
            // 
            this.cboSmall.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSmall.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSmall.FormattingEnabled = true;
            this.cboSmall.Location = new System.Drawing.Point(435, 3);
            this.cboSmall.Name = "cboSmall";
            this.cboSmall.Size = new System.Drawing.Size(118, 20);
            this.cboSmall.TabIndex = 2;
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(69, 30);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(366, 21);
            this.txtTitle.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "날짜 :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(201, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 12);
            this.label2.TabIndex = 13;
            this.label2.Text = "대분류 :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(380, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 12);
            this.label3.TabIndex = 14;
            this.label3.Text = "소분류 :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 12);
            this.label4.TabIndex = 15;
            this.label4.Text = "제목 :";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(248, 294);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSaveClose
            // 
            this.btnSaveClose.Location = new System.Drawing.Point(365, 293);
            this.btnSaveClose.Name = "btnSaveClose";
            this.btnSaveClose.Size = new System.Drawing.Size(87, 23);
            this.btnSaveClose.TabIndex = 19;
            this.btnSaveClose.Text = "저장 후 닫기";
            this.btnSaveClose.UseVisualStyleBackColor = true;
            this.btnSaveClose.Click += new System.EventHandler(this.btnSaveClose_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(489, 293);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 20;
            this.btnClose.Text = "닫기";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // rtxtContents
            // 
            this.rtxtContents.AcceptsTab = true;
            this.rtxtContents.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rtxtContents.Location = new System.Drawing.Point(28, 94);
            this.rtxtContents.Name = "rtxtContents";
            this.rtxtContents.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtxtContents.Size = new System.Drawing.Size(573, 193);
            this.rtxtContents.TabIndex = 1;
            this.rtxtContents.Text = "";
            this.rtxtContents.KeyDown += new System.Windows.Forms.KeyEventHandler(this.rtxtContents_KeyDown);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnRed);
            this.panel1.Controls.Add(this.btnBlack);
            this.panel1.Controls.Add(this.btnBlue);
            this.panel1.Controls.Add(this.btnSizeDown);
            this.panel1.Controls.Add(this.btnSizeUp);
            this.panel1.Controls.Add(this.btnThick);
            this.panel1.Location = new System.Drawing.Point(81, 57);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(354, 22);
            this.panel1.TabIndex = 6;
            // 
            // btnRed
            // 
            this.btnRed.BackColor = System.Drawing.Color.Red;
            this.btnRed.Font = new System.Drawing.Font("굴림", 7F);
            this.btnRed.Location = new System.Drawing.Point(174, 1);
            this.btnRed.Name = "btnRed";
            this.btnRed.Size = new System.Drawing.Size(22, 20);
            this.btnRed.TabIndex = 0;
            this.btnRed.UseVisualStyleBackColor = false;
            this.btnRed.Click += new System.EventHandler(this.btnRed_Click);
            // 
            // btnBlack
            // 
            this.btnBlack.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnBlack.Font = new System.Drawing.Font("굴림", 7F);
            this.btnBlack.Location = new System.Drawing.Point(146, 1);
            this.btnBlack.Name = "btnBlack";
            this.btnBlack.Size = new System.Drawing.Size(22, 20);
            this.btnBlack.TabIndex = 5;
            this.btnBlack.UseVisualStyleBackColor = false;
            this.btnBlack.Click += new System.EventHandler(this.btnBlack_Click);
            // 
            // btnBlue
            // 
            this.btnBlue.BackColor = System.Drawing.Color.MediumBlue;
            this.btnBlue.FlatAppearance.BorderSize = 0;
            this.btnBlue.Font = new System.Drawing.Font("굴림", 7F);
            this.btnBlue.Location = new System.Drawing.Point(201, 1);
            this.btnBlue.Name = "btnBlue";
            this.btnBlue.Size = new System.Drawing.Size(22, 20);
            this.btnBlue.TabIndex = 1;
            this.btnBlue.UseVisualStyleBackColor = false;
            this.btnBlue.Click += new System.EventHandler(this.btnBlue_Click);
            // 
            // btnSizeDown
            // 
            this.btnSizeDown.Font = new System.Drawing.Font("굴림", 7F);
            this.btnSizeDown.Location = new System.Drawing.Point(90, 1);
            this.btnSizeDown.Name = "btnSizeDown";
            this.btnSizeDown.Size = new System.Drawing.Size(22, 20);
            this.btnSizeDown.TabIndex = 4;
            this.btnSizeDown.Text = "-";
            this.btnSizeDown.UseVisualStyleBackColor = true;
            this.btnSizeDown.Click += new System.EventHandler(this.btnSizeDown_Click);
            // 
            // btnSizeUp
            // 
            this.btnSizeUp.Font = new System.Drawing.Font("굴림", 7F);
            this.btnSizeUp.Location = new System.Drawing.Point(62, 1);
            this.btnSizeUp.Name = "btnSizeUp";
            this.btnSizeUp.Size = new System.Drawing.Size(22, 20);
            this.btnSizeUp.TabIndex = 3;
            this.btnSizeUp.Text = "+";
            this.btnSizeUp.UseVisualStyleBackColor = true;
            this.btnSizeUp.Click += new System.EventHandler(this.btnSizeUp_Click);
            // 
            // btnThick
            // 
            this.btnThick.Font = new System.Drawing.Font("굴림", 7F);
            this.btnThick.Location = new System.Drawing.Point(12, 1);
            this.btnThick.Name = "btnThick";
            this.btnThick.Size = new System.Drawing.Size(44, 20);
            this.btnThick.TabIndex = 2;
            this.btnThick.Text = "진하게";
            this.btnThick.UseVisualStyleBackColor = true;
            this.btnThick.Click += new System.EventHandler(this.btnThick_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(464, 33);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(25, 12);
            this.label7.TabIndex = 44;
            this.label7.Text = "색 :";
            // 
            // cboColor
            // 
            this.cboColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboColor.FormattingEnabled = true;
            this.cboColor.Location = new System.Drawing.Point(496, 30);
            this.cboColor.Name = "cboColor";
            this.cboColor.Size = new System.Drawing.Size(57, 20);
            this.cboColor.TabIndex = 43;
            // 
            // panel2
            // 
            this.panel2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.txtTitle);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.dte);
            this.panel2.Controls.Add(this.cboColor);
            this.panel2.Controls.Add(this.cboMain);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Controls.Add(this.cboSmall);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Location = new System.Drawing.Point(24, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(581, 85);
            this.panel2.TabIndex = 0;
            // 
            // EnrollForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 332);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSaveClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.rtxtContents);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EnrollForm";
            this.Text = "EnrollForm";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dte;
        private System.Windows.Forms.ComboBox cboMain;
        private System.Windows.Forms.ComboBox cboSmall;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnSaveClose;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.RichTextBox rtxtContents;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSizeDown;
        private System.Windows.Forms.Button btnSizeUp;
        private System.Windows.Forms.Button btnThick;
        private System.Windows.Forms.Button btnRed;
        private System.Windows.Forms.Button btnBlack;
        private System.Windows.Forms.Button btnBlue;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cboColor;
        private System.Windows.Forms.Panel panel2;
    }
}