using System.Drawing;
using System.Windows.Forms;

namespace BananaBridge.UI
{
    partial class BananaDialog
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
			pnlHeader = new Panel();
			lblTitle = new Label();
			tlpConfig = new TableLayoutPanel();
			lblModel = new Label();
			cbModel = new ComboBox();
			btnSettings = new Button();
			pnlBottom = new Panel();
			chkCopyToClipboard = new CheckBox();
			chkScaleToFit = new CheckBox();
			chkGenerateFromScratch = new CheckBox();
			btnOK = new Button();
			btnCancel = new Button();
			tlpMain = new TableLayoutPanel();
			pnlLeft = new Panel();
			grpStyles = new GroupBox();
			cbStyles = new ComboBox();
			txtStyleDesc = new TextBox();
			grpChat = new GroupBox();
			txtChatHistory = new TextBox();
			txtPrompt = new TextBox();
			btnGenerate = new Button();
			pnlPreview = new Panel();
			lblPreviewTitle = new Label();
			pbPreview = new PictureBox();
			prgLoading = new ProgressBar();
			pnlHeader.SuspendLayout();
			tlpConfig.SuspendLayout();
			pnlBottom.SuspendLayout();
			tlpMain.SuspendLayout();
			pnlLeft.SuspendLayout();
			grpStyles.SuspendLayout();
			grpChat.SuspendLayout();
			pnlPreview.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)pbPreview).BeginInit();
			SuspendLayout();
			// 
			// pnlHeader
			// 
			pnlHeader.BackColor = Color.FromArgb(30, 30, 35);
			pnlHeader.Controls.Add(lblTitle);
			pnlHeader.Dock = DockStyle.Top;
			pnlHeader.Location = new Point(0, 0);
			pnlHeader.Margin = new Padding(8, 9, 8, 9);
			pnlHeader.Name = "pnlHeader";
			pnlHeader.Size = new Size(1938, 97);
			pnlHeader.TabIndex = 0;
			// 
			// lblTitle
			// 
			lblTitle.AutoSize = true;
			lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
			lblTitle.ForeColor = Color.White;
			lblTitle.Location = new Point(17, 21);
			lblTitle.Margin = new Padding(8, 0, 8, 0);
			lblTitle.Name = "lblTitle";
			lblTitle.Size = new Size(544, 54);
			lblTitle.TabIndex = 0;
			lblTitle.Text = "Banana AI Editor";
			// 
			// tlpConfig
			// 
			tlpConfig.ColumnCount = 3;
			tlpConfig.ColumnStyles.Add(new ColumnStyle());
			tlpConfig.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			tlpConfig.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 340F));
			tlpConfig.Controls.Add(lblModel, 0, 0);
			tlpConfig.Controls.Add(cbModel, 1, 0);
			tlpConfig.Controls.Add(btnSettings, 2, 0);
			tlpConfig.Dock = DockStyle.Top;
			tlpConfig.Location = new Point(0, 97);
			tlpConfig.Margin = new Padding(8, 9, 8, 9);
			tlpConfig.Name = "tlpConfig";
			tlpConfig.RowCount = 1;
			tlpConfig.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			tlpConfig.Size = new Size(1938, 72);
			tlpConfig.TabIndex = 1;
			// 
			// lblModel
			// 
			lblModel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
			lblModel.Location = new Point(8, 4);
			lblModel.Margin = new Padding(8, 0, 8, 0);
			lblModel.Name = "lblModel";
			lblModel.Size = new Size(142, 63);
			lblModel.TabIndex = 0;
			lblModel.Text = "Model:";
			lblModel.TextAlign = ContentAlignment.MiddleLeft;
			// 
			// cbModel
			// 
			cbModel.Dock = DockStyle.Fill;
			cbModel.DropDownStyle = ComboBoxStyle.DropDownList;
			cbModel.FormattingEnabled = true;
			cbModel.Location = new Point(166, 9);
			cbModel.Margin = new Padding(8, 9, 8, 9);
			cbModel.Name = "cbModel";
			cbModel.Size = new Size(1424, 49);
			cbModel.TabIndex = 1;
			// 
			// btnSettings
			// 
			btnSettings.Dock = DockStyle.Fill;
			btnSettings.Location = new Point(1606, 9);
			btnSettings.Margin = new Padding(8, 9, 8, 9);
			btnSettings.Name = "btnSettings";
			btnSettings.Size = new Size(324, 54);
			btnSettings.TabIndex = 2;
			btnSettings.Text = "Settings...";
			btnSettings.UseVisualStyleBackColor = true;
			btnSettings.Click += BtnSettings_Click;
			// 
			// pnlBottom
			// 
			pnlBottom.Controls.Add(chkScaleToFit);
			pnlBottom.Controls.Add(chkCopyToClipboard);
			pnlBottom.Controls.Add(chkGenerateFromScratch);
			pnlBottom.Controls.Add(btnOK);
			pnlBottom.Controls.Add(btnCancel);
			pnlBottom.Dock = DockStyle.Bottom;
			pnlBottom.Location = new Point(0, 1580);
			pnlBottom.Margin = new Padding(8, 9, 8, 9);
			pnlBottom.Name = "pnlBottom";
			pnlBottom.Size = new Size(1938, 240);
			pnlBottom.TabIndex = 2;
			// 
			// chkCopyToClipboard
			// 
			chkCopyToClipboard.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			chkCopyToClipboard.Checked = true;
			chkCopyToClipboard.CheckState = CheckState.Checked;
			chkCopyToClipboard.Location = new Point(57, 20);
			chkCopyToClipboard.Margin = new Padding(8, 9, 8, 9);
			chkCopyToClipboard.Name = "chkCopyToClipboard";
			chkCopyToClipboard.Size = new Size(943, 63);
			chkCopyToClipboard.TabIndex = 0;
			chkCopyToClipboard.Text = "Copy changes to clipboard (paste as new layer via Ctrl+Alt+V)";
			// 
			// chkScaleToFit
			// 
			chkScaleToFit.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			chkScaleToFit.Checked = true;
			chkScaleToFit.CheckState = CheckState.Checked;
			chkScaleToFit.Location = new Point(57, 95);
			chkScaleToFit.Margin = new Padding(8, 9, 8, 9);
			chkScaleToFit.Name = "chkScaleToFit";
			chkScaleToFit.Size = new Size(943, 63);
			chkScaleToFit.TabIndex = 3;
			chkScaleToFit.Text = "Scale generated image to fit selection bounds";
			// 
			// chkGenerateFromScratch
			// 
			chkGenerateFromScratch.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			chkGenerateFromScratch.Checked = false;
			chkGenerateFromScratch.CheckState = CheckState.Unchecked;
			chkGenerateFromScratch.Location = new Point(57, 170);
			chkGenerateFromScratch.Margin = new Padding(8, 9, 8, 9);
			chkGenerateFromScratch.Name = "chkGenerateFromScratch";
			chkGenerateFromScratch.Size = new Size(943, 63);
			chkGenerateFromScratch.TabIndex = 4;
			chkGenerateFromScratch.Text = "Generate from scratch (ignore background image)";
			// 
			// btnOK
			// 
			btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			btnOK.DialogResult = DialogResult.OK;
			btnOK.Location = new Point(1360, 63);
			btnOK.Margin = new Padding(8, 9, 8, 9);
			btnOK.Name = "btnOK";
			btnOK.Size = new Size(269, 79);
			btnOK.TabIndex = 1;
			btnOK.Text = "Commit Layer";
			btnOK.UseVisualStyleBackColor = true;
			btnOK.Click += BtnOK_Click;
			// 
			// btnCancel
			// 
			btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			btnCancel.DialogResult = DialogResult.Cancel;
			btnCancel.Location = new Point(1646, 63);
			btnCancel.Margin = new Padding(8, 9, 8, 9);
			btnCancel.Name = "btnCancel";
			btnCancel.Size = new Size(269, 79);
			btnCancel.TabIndex = 2;
			btnCancel.Text = "Cancel";
			btnCancel.UseVisualStyleBackColor = true;
			// 
			// tlpMain
			// 
			tlpMain.ColumnCount = 2;
			tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 500F));
			tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			tlpMain.Controls.Add(pnlLeft, 0, 0);
			tlpMain.Controls.Add(pnlPreview, 1, 0);
			tlpMain.Dock = DockStyle.Fill;
			tlpMain.Location = new Point(0, 169);
			tlpMain.Margin = new Padding(8, 9, 8, 9);
			tlpMain.Name = "tlpMain";
			tlpMain.RowCount = 1;
			tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			tlpMain.Size = new Size(1938, 1411);
			tlpMain.TabIndex = 3;
			// 
			// pnlLeft
			// 
			pnlLeft.Controls.Add(grpStyles);
			pnlLeft.Controls.Add(grpChat);
			pnlLeft.Dock = DockStyle.Fill;
			pnlLeft.Location = new Point(8, 9);
			pnlLeft.Margin = new Padding(8, 9, 8, 9);
			pnlLeft.Name = "pnlLeft";
			pnlLeft.Padding = new Padding(0, 0, 28, 0);
			pnlLeft.Size = new Size(484, 1393);
			pnlLeft.TabIndex = 0;
			// 
			// grpStyles
			// 
			grpStyles.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			grpStyles.Controls.Add(cbStyles);
			grpStyles.Controls.Add(txtStyleDesc);
			grpStyles.Location = new Point(0, 0);
			grpStyles.Margin = new Padding(8, 9, 8, 9);
			grpStyles.Name = "grpStyles";
			grpStyles.Padding = new Padding(8, 9, 8, 9);
			grpStyles.Size = new Size(448, 442);
			grpStyles.TabIndex = 0;
			grpStyles.TabStop = false;
			grpStyles.Text = "Image Restyling";
			// 
			// cbStyles
			// 
			cbStyles.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			cbStyles.DropDownStyle = ComboBoxStyle.DropDownList;
			cbStyles.Location = new Point(9, 58);
			cbStyles.Margin = new Padding(8, 9, 8, 9);
			cbStyles.Name = "cbStyles";
			cbStyles.Size = new Size(423, 49);
			cbStyles.TabIndex = 0;
			cbStyles.SelectedIndexChanged += CbStyles_SelectedIndexChanged;
			// 
			// txtStyleDesc
			// 
			txtStyleDesc.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			txtStyleDesc.Location = new Point(9, 173);
			txtStyleDesc.Margin = new Padding(8, 9, 8, 9);
			txtStyleDesc.Multiline = true;
			txtStyleDesc.Name = "txtStyleDesc";
			txtStyleDesc.ReadOnly = true;
			txtStyleDesc.ScrollBars = ScrollBars.Vertical;
			txtStyleDesc.Size = new Size(423, 251);
			txtStyleDesc.TabIndex = 1;
			// 
			// grpChat
			// 
			grpChat.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			grpChat.Controls.Add(txtChatHistory);
			grpChat.Controls.Add(txtPrompt);
			grpChat.Controls.Add(btnGenerate);
			grpChat.Location = new Point(0, 473);
			grpChat.Margin = new Padding(8, 9, 8, 9);
			grpChat.Name = "grpChat";
			grpChat.Padding = new Padding(8, 9, 8, 9);
			grpChat.Size = new Size(448, 920);
			grpChat.TabIndex = 1;
			grpChat.TabStop = false;
			grpChat.Text = "Conversational Canvas Editor";
			// 
			// txtChatHistory
			// 
			txtChatHistory.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			txtChatHistory.BackColor = Color.FromArgb(245, 245, 250);
			txtChatHistory.Location = new Point(9, 79);
			txtChatHistory.Margin = new Padding(8, 9, 8, 9);
			txtChatHistory.Multiline = true;
			txtChatHistory.Name = "txtChatHistory";
			txtChatHistory.ReadOnly = true;
			txtChatHistory.ScrollBars = ScrollBars.Vertical;
			txtChatHistory.Size = new Size(423, 330);
			txtChatHistory.TabIndex = 0;
			// 
			// txtPrompt
			// 
			txtPrompt.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			txtPrompt.Location = new Point(9, 427);
			txtPrompt.Margin = new Padding(8, 9, 8, 9);
			txtPrompt.Multiline = true;
			txtPrompt.Name = "txtPrompt";
			txtPrompt.Size = new Size(423, 342);
			txtPrompt.TabIndex = 1;
			txtPrompt.KeyDown += TxtPrompt_KeyDown;
			// 
			// btnGenerate
			// 
			btnGenerate.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			btnGenerate.BackColor = Color.LightSkyBlue;
			btnGenerate.Location = new Point(49, 815);
			btnGenerate.Margin = new Padding(8, 9, 8, 9);
			btnGenerate.Name = "btnGenerate";
			btnGenerate.Size = new Size(269, 63);
			btnGenerate.TabIndex = 2;
			btnGenerate.Text = "Generate Edit";
			btnGenerate.UseVisualStyleBackColor = false;
			btnGenerate.Click += BtnGenerate_Click;
			// 
			// pnlPreview
			// 
			pnlPreview.BackColor = Color.DarkGray;
			pnlPreview.BorderStyle = BorderStyle.FixedSingle;
			pnlPreview.Controls.Add(lblPreviewTitle);
			pnlPreview.Controls.Add(pbPreview);
			pnlPreview.Controls.Add(prgLoading);
			pnlPreview.Dock = DockStyle.Fill;
			pnlPreview.Location = new Point(508, 9);
			pnlPreview.Margin = new Padding(8, 9, 8, 9);
			pnlPreview.Name = "pnlPreview";
			pnlPreview.Size = new Size(1422, 1393);
			pnlPreview.TabIndex = 1;
			// 
			// lblPreviewTitle
			// 
			lblPreviewTitle.AutoSize = true;
			lblPreviewTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
			lblPreviewTitle.ForeColor = Color.Black;
			lblPreviewTitle.Location = new Point(28, 32);
			lblPreviewTitle.Margin = new Padding(8, 0, 8, 0);
			lblPreviewTitle.Name = "lblPreviewTitle";
			lblPreviewTitle.Size = new Size(224, 41);
			lblPreviewTitle.TabIndex = 0;
			lblPreviewTitle.Text = "Result Preview";
			// 
			// pbPreview
			// 
			pbPreview.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			pbPreview.BackColor = Color.LightGray;
			pbPreview.Location = new Point(28, 110);
			pbPreview.Margin = new Padding(8, 9, 8, 9);
			pbPreview.Name = "pbPreview";
			pbPreview.Size = new Size(1364, 1155);
			pbPreview.SizeMode = PictureBoxSizeMode.Zoom;
			pbPreview.TabIndex = 1;
			pbPreview.TabStop = false;
			// 
			// prgLoading
			// 
			prgLoading.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			prgLoading.Location = new Point(28, 1287);
			prgLoading.Margin = new Padding(8, 9, 8, 9);
			prgLoading.Name = "prgLoading";
			prgLoading.Size = new Size(1364, 63);
			prgLoading.Style = ProgressBarStyle.Marquee;
			prgLoading.TabIndex = 2;
			prgLoading.Visible = false;
			// 
			// BananaDialog
			// 
			AutoScaleDimensions = new SizeF(17F, 41F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1938, 1769);
			Controls.Add(tlpMain);
			Controls.Add(pnlBottom);
			Controls.Add(tlpConfig);
			Controls.Add(pnlHeader);
			Margin = new Padding(8, 9, 8, 9);
			MinimizeBox = false;
			MinimumSize = new Size(1925, 1703);
			Name = "BananaDialog";
			StartPosition = FormStartPosition.CenterParent;
			Text = "Banana AI Editor";
			pnlHeader.ResumeLayout(false);
			pnlHeader.PerformLayout();
			tlpConfig.ResumeLayout(false);
			pnlBottom.ResumeLayout(false);
			tlpMain.ResumeLayout(false);
			pnlLeft.ResumeLayout(false);
			grpStyles.ResumeLayout(false);
			grpStyles.PerformLayout();
			grpChat.ResumeLayout(false);
			grpChat.PerformLayout();
			pnlPreview.ResumeLayout(false);
			pnlPreview.PerformLayout();
			((System.ComponentModel.ISupportInitialize)pbPreview).EndInit();
			ResumeLayout(false);

		}

		#endregion

		private Panel pnlHeader;
        private Label lblTitle;
        private Label lblModel;
        private ComboBox cbModel;
        private Button btnSettings;
        
        private GroupBox grpChat;
        private TextBox txtChatHistory;
        private TextBox txtPrompt;
        private Button btnGenerate;
        
        private GroupBox grpStyles;
        private ComboBox cbStyles;
        private TextBox txtStyleDesc;
        
        private Panel pnlPreview;
        private PictureBox pbPreview;
        private Label lblPreviewTitle;
        private ProgressBar prgLoading;
        
        private CheckBox chkCopyToClipboard;
        private CheckBox chkScaleToFit;
        private CheckBox chkGenerateFromScratch;
        private Button btnOK;
        private Button btnCancel;
        private TableLayoutPanel tlpConfig;
        private Panel pnlBottom;
        private TableLayoutPanel tlpMain;
        private Panel pnlLeft;
    }
}

