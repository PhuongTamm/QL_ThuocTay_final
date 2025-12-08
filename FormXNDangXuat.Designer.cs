namespace QL_ThuocTay
{
    partial class FormXNDangXuat
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.btKhong = new System.Windows.Forms.Button();
            this.btCo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(212, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Xác nhận đăng xuất?";
            // 
            // btKhong
            // 
            this.btKhong.Location = new System.Drawing.Point(140, 165);
            this.btKhong.Name = "btKhong";
            this.btKhong.Size = new System.Drawing.Size(94, 49);
            this.btKhong.TabIndex = 0;
            this.btKhong.Text = "Không";
            this.btKhong.UseVisualStyleBackColor = true;
            this.btKhong.Click += new System.EventHandler(this.btKhong_Click);
            // 
            // btCo
            // 
            this.btCo.Location = new System.Drawing.Point(328, 165);
            this.btCo.Name = "btCo";
            this.btCo.Size = new System.Drawing.Size(94, 49);
            this.btCo.TabIndex = 1;
            this.btCo.Text = "Có";
            this.btCo.UseVisualStyleBackColor = true;
            this.btCo.Click += new System.EventHandler(this.btCo_Click);
            // 
            // FormXNDangXuat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(578, 294);
            this.Controls.Add(this.btCo);
            this.Controls.Add(this.btKhong);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FormXNDangXuat";
            this.Load += new System.EventHandler(this.FormXNDangXuat_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btKhong;
        private System.Windows.Forms.Button btCo;
    }
}