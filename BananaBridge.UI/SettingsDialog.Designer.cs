using System.Drawing;
using System.Windows.Forms;

namespace BananaBridge.UI
{
    partial class SettingsDialog
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblApiKey = new System.Windows.Forms.Label();
            this.txtApiKey = new System.Windows.Forms.TextBox();
            this.btnRefreshModels = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblProvider = new System.Windows.Forms.Label();
            this.cbProvider = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // 
            // lblProvider
            // 
            this.lblProvider.Location = new System.Drawing.Point(20, 25);
            this.lblProvider.Name = "lblProvider";
            this.lblProvider.Size = new System.Drawing.Size(100, 20);
            this.lblProvider.TabIndex = 6;
            this.lblProvider.Text = "AI Provider:";
            this.lblProvider.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbProvider
            // 
            this.cbProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProvider.FormattingEnabled = true;
            this.cbProvider.Location = new System.Drawing.Point(120, 25);
            this.cbProvider.Name = "cbProvider";
            this.cbProvider.Size = new System.Drawing.Size(290, 21);
            this.cbProvider.TabIndex = 7;
            // 
            // lblApiKey
            // 
            this.lblApiKey.Location = new System.Drawing.Point(20, 60);
            this.lblApiKey.Name = "lblApiKey";
            this.lblApiKey.Size = new System.Drawing.Size(100, 20);
            this.lblApiKey.TabIndex = 0;
            this.lblApiKey.Text = "API Key:";
            this.lblApiKey.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtApiKey
            // 
            this.txtApiKey.Location = new System.Drawing.Point(120, 60);
            this.txtApiKey.Name = "txtApiKey";
            this.txtApiKey.PasswordChar = '*';
            this.txtApiKey.Size = new System.Drawing.Size(290, 20);
            this.txtApiKey.TabIndex = 1;
            // 
            // btnRefreshModels
            // 
            this.btnRefreshModels.Location = new System.Drawing.Point(120, 95);
            this.btnRefreshModels.Name = "btnRefreshModels";
            this.btnRefreshModels.Size = new System.Drawing.Size(150, 25);
            this.btnRefreshModels.TabIndex = 2;
            this.btnRefreshModels.Text = "Refresh Provider Models";
            this.btnRefreshModels.UseVisualStyleBackColor = true;
            this.btnRefreshModels.Click += new System.EventHandler(this.BtnRefreshModels_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.ForeColor = System.Drawing.Color.DimGray;
            this.lblStatus.Location = new System.Drawing.Point(120, 130);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(290, 35);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "Models are cached locally to keep startup fast.";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(215, 180);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(95, 30);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(315, 180);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 30);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // SettingsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(434, 225);
            this.Controls.Add(this.cbProvider);
            this.Controls.Add(this.lblProvider);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnRefreshModels);
            this.Controls.Add(this.txtApiKey);
            this.Controls.Add(this.lblApiKey);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Banana AI Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblApiKey;
        private System.Windows.Forms.TextBox txtApiKey;
        private System.Windows.Forms.Button btnRefreshModels;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblProvider;
        private System.Windows.Forms.ComboBox cbProvider;
    }
}

