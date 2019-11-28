namespace WCF체팅_1대1_클라
{
    partial class Form1
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
            this.chatlist = new System.Windows.Forms.ListBox();
            this.seatbox = new System.Windows.Forms.TextBox();
            this.btnJoin = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.msgbox = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chatlist
            // 
            this.chatlist.FormattingEnabled = true;
            this.chatlist.ItemHeight = 12;
            this.chatlist.Location = new System.Drawing.Point(28, 84);
            this.chatlist.Name = "chatlist";
            this.chatlist.Size = new System.Drawing.Size(410, 304);
            this.chatlist.TabIndex = 0;
            // 
            // seatbox
            // 
            this.seatbox.Location = new System.Drawing.Point(238, 32);
            this.seatbox.Name = "seatbox";
            this.seatbox.Size = new System.Drawing.Size(100, 21);
            this.seatbox.TabIndex = 1;
            // 
            // btnJoin
            // 
            this.btnJoin.Location = new System.Drawing.Point(363, 32);
            this.btnJoin.Name = "btnJoin";
            this.btnJoin.Size = new System.Drawing.Size(75, 23);
            this.btnJoin.TabIndex = 2;
            this.btnJoin.Text = "로그인";
            this.btnJoin.UseVisualStyleBackColor = true;
            this.btnJoin.Click += new System.EventHandler(this.btnJoin_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(157, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "NickName";
            // 
            // msgbox
            // 
            this.msgbox.Location = new System.Drawing.Point(28, 423);
            this.msgbox.Name = "msgbox";
            this.msgbox.Size = new System.Drawing.Size(310, 21);
            this.msgbox.TabIndex = 1;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(363, 423);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "button1";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 480);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnJoin);
            this.Controls.Add(this.msgbox);
            this.Controls.Add(this.seatbox);
            this.Controls.Add(this.chatlist);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox chatlist;
        private System.Windows.Forms.TextBox seatbox;
        private System.Windows.Forms.Button btnJoin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox msgbox;
        private System.Windows.Forms.Button btnSend;
    }
}

