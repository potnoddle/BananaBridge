using System.Drawing;
using System.Windows.Forms;

namespace BananaBridge.UI
{
	partial class AiIsolatorDialog
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
			this.pnlHeader = new System.Windows.Forms.Panel();
			this.lblTitle = new System.Windows.Forms.Label();
			this.tlpConfig = new System.Windows.Forms.TableLayoutPanel();
			this.lblModel = new System.Windows.Forms.Label();
			this.cbModel = new System.Windows.Forms.ComboBox();
			this.btnSettings = new System.Windows.Forms.Button();
			this.pnlBottom = new System.Windows.Forms.Panel();
			this.chkScaleToFit = new System.Windows.Forms.CheckBox();
			this.chkCopyToClipboard = new System.Windows.Forms.CheckBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
			this.pnlLeft = new System.Windows.Forms.Panel();
			this.grpPrompt = new System.Windows.Forms.GroupBox();
			this.lblPromptHelp = new System.Windows.Forms.Label();
			this.txtPrompt = new System.Windows.Forms.TextBox();
			this.btnGenerate = new System.Windows.Forms.Button();
			this.grpOutline = new System.Windows.Forms.GroupBox();
			this.lblOutlineWidth = new System.Windows.Forms.Label();
			this.numOutlineWidth = new System.Windows.Forms.NumericUpDown();
			this.lblOutlineColor = new System.Windows.Forms.Label();
			this.cbOutlineColor = new System.Windows.Forms.ComboBox();
			this.chkDrawOutline = new System.Windows.Forms.CheckBox();
			this.pnlPreview = new System.Windows.Forms.Panel();
			this.lblPreviewTitle = new System.Windows.Forms.Label();
			this.pbPreview = new System.Windows.Forms.PictureBox();
			this.prgLoading = new System.Windows.Forms.ProgressBar();
			this.pnlHeader.SuspendLayout();
			this.tlpConfig.SuspendLayout();
			this.pnlBottom.SuspendLayout();
			this.tlpMain.SuspendLayout();
			this.pnlLeft.SuspendLayout();
			this.grpPrompt.SuspendLayout();
			this.grpOutline.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numOutlineWidth)).BeginInit();
			this.pnlPreview.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbPreview)).BeginInit();
			this.SuspendLayout();
			// 
			// pnlHeader
			// 
			this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(35)))));
			this.pnlHeader.Controls.Add(this.lblTitle);
			this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlHeader.Location = new System.Drawing.Point(0, 0);
			this.pnlHeader.Name = "pnlHeader";
			this.pnlHeader.Size = new System.Drawing.Size(1000, 60);
			this.pnlHeader.TabIndex = 0;
			// 
			// lblTitle
			// 
			this.lblTitle.AutoSize = true;
			this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			this.lblTitle.ForeColor = System.Drawing.Color.White;
			this.lblTitle.Location = new System.Drawing.Point(12, 18);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(262, 21);
			this.lblTitle.TabIndex = 0;
			this.lblTitle.Text = "AI Outline Selector & Isolator";
			// 
			// tlpConfig
			// 
			this.tlpConfig.ColumnCount = 3;
			this.tlpConfig.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpConfig.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpConfig.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
			this.tlpConfig.Controls.Add(this.lblModel, 0, 0);
			this.tlpConfig.Controls.Add(this.cbModel, 1, 0);
			this.tlpConfig.Controls.Add(this.btnSettings, 2, 0);
			this.tlpConfig.Dock = System.Windows.Forms.DockStyle.Top;
			this.tlpConfig.Location = new System.Drawing.Point(0, 60);
			this.tlpConfig.Name = "tlpConfig";
			this.tlpConfig.RowCount = 1;
			this.tlpConfig.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpConfig.Size = new System.Drawing.Size(1000, 45);
			this.tlpConfig.TabIndex = 1;
			// 
			// lblModel
			// 
			this.lblModel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblModel.Location = new System.Drawing.Point(3, 11);
			this.lblModel.Name = "lblModel";
			this.lblModel.Size = new System.Drawing.Size(60, 23);
			this.lblModel.TabIndex = 0;
			this.lblModel.Text = "  Model:";
			this.lblModel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbModel
			// 
			this.cbModel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cbModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbModel.FormattingEnabled = true;
			this.cbModel.Location = new System.Drawing.Point(69, 12);
			this.cbModel.Margin = new System.Windows.Forms.Padding(3, 12, 3, 3);
			this.cbModel.Name = "cbModel";
			this.cbModel.Size = new System.Drawing.Size(808, 21);
			this.cbModel.TabIndex = 1;
			// 
			// btnSettings
			// 
			this.btnSettings.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnSettings.Location = new System.Drawing.Point(883, 10);
			this.btnSettings.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
			this.btnSettings.Name = "btnSettings";
			this.btnSettings.Size = new System.Drawing.Size(114, 25);
			this.btnSettings.TabIndex = 2;
			this.btnSettings.Text = "Settings...";
			this.btnSettings.UseVisualStyleBackColor = true;
			this.btnSettings.Click += new System.EventHandler(this.BtnSettings_Click);
			// 
			// pnlBottom
			// 
			this.pnlBottom.Controls.Add(this.chkScaleToFit);
			this.pnlBottom.Controls.Add(this.chkCopyToClipboard);
			this.pnlBottom.Controls.Add(this.btnOK);
			this.pnlBottom.Controls.Add(this.btnCancel);
			this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlBottom.Location = new System.Drawing.Point(0, 620);
			this.pnlBottom.Name = "pnlBottom";
			this.pnlBottom.Size = new System.Drawing.Size(1000, 100);
			this.pnlBottom.TabIndex = 2;
			// 
			// chkScaleToFit
			// 
			this.chkScaleToFit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chkScaleToFit.Checked = true;
			this.chkScaleToFit.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkScaleToFit.Location = new System.Drawing.Point(20, 50);
			this.chkScaleToFit.Name = "chkScaleToFit";
			this.chkScaleToFit.Size = new System.Drawing.Size(400, 24);
			this.chkScaleToFit.TabIndex = 3;
			this.chkScaleToFit.Text = "Scale generated image to fit selection bounds";
			// 
			// chkCopyToClipboard
			// 
			this.chkCopyToClipboard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chkCopyToClipboard.Checked = true;
			this.chkCopyToClipboard.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkCopyToClipboard.Location = new System.Drawing.Point(20, 15);
			this.chkCopyToClipboard.Name = "chkCopyToClipboard";
			this.chkCopyToClipboard.Size = new System.Drawing.Size(400, 24);
			this.chkCopyToClipboard.TabIndex = 0;
			this.chkCopyToClipboard.Text = "Copy changes to clipboard (paste as new layer via Ctrl+Alt+V)";
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(740, 30);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(110, 40);
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "Commit Layer";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(860, 30);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(110, 40);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// tlpMain
			// 
			this.tlpMain.ColumnCount = 2;
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 350F));
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpMain.Controls.Add(this.pnlLeft, 0, 0);
			this.tlpMain.Controls.Add(this.pnlPreview, 1, 0);
			this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpMain.Location = new System.Drawing.Point(0, 105);
			this.tlpMain.Name = "tlpMain";
			this.tlpMain.RowCount = 1;
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpMain.Size = new System.Drawing.Size(1000, 515);
			this.tlpMain.TabIndex = 3;
			// 
			// pnlLeft
			// 
			this.pnlLeft.Controls.Add(this.grpPrompt);
			this.pnlLeft.Controls.Add(this.grpOutline);
			this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlLeft.Location = new System.Drawing.Point(3, 3);
			this.pnlLeft.Name = "pnlLeft";
			this.pnlLeft.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
			this.pnlLeft.Size = new System.Drawing.Size(344, 509);
			this.pnlLeft.TabIndex = 0;
			// 
			// grpPrompt
			// 
			this.grpPrompt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpPrompt.Controls.Add(this.lblPromptHelp);
			this.grpPrompt.Controls.Add(this.txtPrompt);
			this.grpPrompt.Controls.Add(this.btnGenerate);
			this.grpPrompt.Location = new System.Drawing.Point(3, 3);
			this.grpPrompt.Name = "grpPrompt";
			this.grpPrompt.Size = new System.Drawing.Size(331, 230);
			this.grpPrompt.TabIndex = 0;
			this.grpPrompt.TabStop = false;
			this.grpPrompt.Text = "Isolate Subject";
			// 
			// lblPromptHelp
			// 
			this.lblPromptHelp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblPromptHelp.ForeColor = System.Drawing.Color.DimGray;
			this.lblPromptHelp.Location = new System.Drawing.Point(10, 20);
			this.lblPromptHelp.Name = "lblPromptHelp";
			this.lblPromptHelp.Size = new System.Drawing.Size(311, 35);
			this.lblPromptHelp.TabIndex = 0;
			this.lblPromptHelp.Text = "Describe the subject you want to isolate (e.g., \"cat\", \"main building\"). Leave empty for auto-detect.";
			// 
			// txtPrompt
			// 
			this.txtPrompt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtPrompt.Location = new System.Drawing.Point(10, 60);
			this.txtPrompt.Multiline = true;
			this.txtPrompt.Name = "txtPrompt";
			this.txtPrompt.Size = new System.Drawing.Size(311, 110);
			this.txtPrompt.TabIndex = 1;
			this.txtPrompt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtPrompt_KeyDown);
			// 
			// btnGenerate
			// 
			this.btnGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnGenerate.BackColor = System.Drawing.Color.LightSkyBlue;
			this.btnGenerate.Location = new System.Drawing.Point(180, 180);
			this.btnGenerate.Name = "btnGenerate";
			this.btnGenerate.Size = new System.Drawing.Size(140, 40);
			this.btnGenerate.TabIndex = 2;
			this.btnGenerate.Text = "Isolate Subject";
			this.btnGenerate.UseVisualStyleBackColor = false;
			this.btnGenerate.Click += new System.EventHandler(this.BtnGenerate_Click);
			// 
			// grpOutline
			// 
			this.grpOutline.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpOutline.Controls.Add(this.lblOutlineWidth);
			this.grpOutline.Controls.Add(this.numOutlineWidth);
			this.grpOutline.Controls.Add(this.lblOutlineColor);
			this.grpOutline.Controls.Add(this.cbOutlineColor);
			this.grpOutline.Controls.Add(this.chkDrawOutline);
			this.grpOutline.Location = new System.Drawing.Point(3, 250);
			this.grpOutline.Name = "grpOutline";
			this.grpOutline.Size = new System.Drawing.Size(331, 180);
			this.grpOutline.TabIndex = 1;
			this.grpOutline.TabStop = false;
			this.grpOutline.Text = "Outline Customization";
			// 
			// lblOutlineWidth
			// 
			this.lblOutlineWidth.Location = new System.Drawing.Point(10, 130);
			this.lblOutlineWidth.Name = "lblOutlineWidth";
			this.lblOutlineWidth.Size = new System.Drawing.Size(100, 20);
			this.lblOutlineWidth.TabIndex = 4;
			this.lblOutlineWidth.Text = "Outline Width:";
			this.lblOutlineWidth.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// numOutlineWidth
			// 
			this.numOutlineWidth.Location = new System.Drawing.Point(120, 130);
			this.numOutlineWidth.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.numOutlineWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numOutlineWidth.Name = "numOutlineWidth";
			this.numOutlineWidth.Size = new System.Drawing.Size(80, 20);
			this.numOutlineWidth.TabIndex = 5;
			this.numOutlineWidth.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
			this.numOutlineWidth.ValueChanged += new System.EventHandler(this.OutlineSetting_Changed);
			// 
			// lblOutlineColor
			// 
			this.lblOutlineColor.Location = new System.Drawing.Point(10, 80);
			this.lblOutlineColor.Name = "lblOutlineColor";
			this.lblOutlineColor.Size = new System.Drawing.Size(100, 20);
			this.lblOutlineColor.TabIndex = 2;
			this.lblOutlineColor.Text = "Outline Color:";
			this.lblOutlineColor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbOutlineColor
			// 
			this.cbOutlineColor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbOutlineColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbOutlineColor.FormattingEnabled = true;
			this.cbOutlineColor.Location = new System.Drawing.Point(120, 80);
			this.cbOutlineColor.Name = "cbOutlineColor";
			this.cbOutlineColor.Size = new System.Drawing.Size(200, 21);
			this.cbOutlineColor.TabIndex = 3;
			this.cbOutlineColor.SelectedIndexChanged += new System.EventHandler(this.CbOutlineColor_SelectedIndexChanged);
			// 
			// chkDrawOutline
			// 
			this.chkDrawOutline.Location = new System.Drawing.Point(10, 30);
			this.chkDrawOutline.Name = "chkDrawOutline";
			this.chkDrawOutline.Size = new System.Drawing.Size(300, 24);
			this.chkDrawOutline.TabIndex = 1;
			this.chkDrawOutline.Text = "Draw outline around isolated subject";
			this.chkDrawOutline.UseVisualStyleBackColor = true;
			this.chkDrawOutline.CheckedChanged += new System.EventHandler(this.ChkDrawOutline_CheckedChanged);
			// 
			// pnlPreview
			// 
			this.pnlPreview.BackColor = System.Drawing.Color.DarkGray;
			this.pnlPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlPreview.Controls.Add(this.lblPreviewTitle);
			this.pnlPreview.Controls.Add(this.pbPreview);
			this.pnlPreview.Controls.Add(this.prgLoading);
			this.pnlPreview.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlPreview.Location = new System.Drawing.Point(353, 3);
			this.pnlPreview.Name = "pnlPreview";
			this.pnlPreview.Size = new System.Drawing.Size(644, 509);
			this.pnlPreview.TabIndex = 1;
			// 
			// lblPreviewTitle
			// 
			this.lblPreviewTitle.AutoSize = true;
			this.lblPreviewTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
			this.lblPreviewTitle.ForeColor = System.Drawing.Color.Black;
			this.lblPreviewTitle.Location = new System.Drawing.Point(10, 10);
			this.lblPreviewTitle.Name = "lblPreviewTitle";
			this.lblPreviewTitle.Size = new System.Drawing.Size(89, 15);
			this.lblPreviewTitle.TabIndex = 0;
			this.lblPreviewTitle.Text = "Result Preview";
			// 
			// pbPreview
			// 
			this.pbPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pbPreview.BackColor = System.Drawing.Color.LightGray;
			this.pbPreview.Location = new System.Drawing.Point(10, 40);
			this.pbPreview.Name = "pbPreview";
			this.pbPreview.Size = new System.Drawing.Size(622, 420);
			this.pbPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pbPreview.TabIndex = 1;
			this.pbPreview.TabStop = false;
			// 
			// prgLoading
			// 
			this.prgLoading.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.prgLoading.Location = new System.Drawing.Point(10, 470);
			this.prgLoading.Name = "prgLoading";
			this.prgLoading.Size = new System.Drawing.Size(622, 25);
			this.prgLoading.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			this.prgLoading.TabIndex = 2;
			this.prgLoading.Visible = false;
			// 
			// AiIsolatorDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1000, 720);
			this.Controls.Add(this.tlpMain);
			this.Controls.Add(this.pnlBottom);
			this.Controls.Add(this.tlpConfig);
			this.Controls.Add(this.pnlHeader);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(800, 600);
			this.Name = "AiIsolatorDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "AI Outline Selector & Isolator";
			this.pnlHeader.ResumeLayout(false);
			this.pnlHeader.PerformLayout();
			this.tlpConfig.ResumeLayout(false);
			this.pnlBottom.ResumeLayout(false);
			this.tlpMain.ResumeLayout(false);
			this.pnlLeft.ResumeLayout(false);
			this.grpPrompt.ResumeLayout(false);
			this.grpPrompt.PerformLayout();
			this.grpOutline.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numOutlineWidth)).EndInit();
			this.pnlPreview.ResumeLayout(false);
			this.pnlPreview.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbPreview)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel pnlHeader;
		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.TableLayoutPanel tlpConfig;
		private System.Windows.Forms.Label lblModel;
		private System.Windows.Forms.ComboBox cbModel;
		private System.Windows.Forms.Button btnSettings;
		private System.Windows.Forms.Panel pnlBottom;
		private System.Windows.Forms.CheckBox chkScaleToFit;
		private System.Windows.Forms.CheckBox chkCopyToClipboard;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.TableLayoutPanel tlpMain;
		private System.Windows.Forms.Panel pnlLeft;
		private System.Windows.Forms.GroupBox grpPrompt;
		private System.Windows.Forms.Label lblPromptHelp;
		private System.Windows.Forms.TextBox txtPrompt;
		private System.Windows.Forms.Button btnGenerate;
		private System.Windows.Forms.GroupBox grpOutline;
		private System.Windows.Forms.Label lblOutlineWidth;
		private System.Windows.Forms.NumericUpDown numOutlineWidth;
		private System.Windows.Forms.Label lblOutlineColor;
		private System.Windows.Forms.ComboBox cbOutlineColor;
		private System.Windows.Forms.CheckBox chkDrawOutline;
		private System.Windows.Forms.Panel pnlPreview;
		private System.Windows.Forms.Label lblPreviewTitle;
		private System.Windows.Forms.PictureBox pbPreview;
		private System.Windows.Forms.ProgressBar prgLoading;
	}
}
