namespace BananaBridge.UI
{
    partial class AiEditDialog
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
            this.lblBrushColor = new System.Windows.Forms.Label();
            this.cbBrushColor = new System.Windows.Forms.ComboBox();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.grpChat = new System.Windows.Forms.GroupBox();
            this.txtChatHistory = new System.Windows.Forms.TextBox();
            this.txtPrompt = new System.Windows.Forms.TextBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.pnlPreview = new System.Windows.Forms.Panel();
            this.lblPreviewTitle = new System.Windows.Forms.Label();
            this.canvasPanel = new System.Windows.Forms.Panel();
            this.canvas = new BananaBridge.UI.InteractiveCanvas();
            this.prgLoading = new System.Windows.Forms.ProgressBar();
            this.pnlCanvasControls = new System.Windows.Forms.Panel();
            this.btnZoomFit = new System.Windows.Forms.Button();
            this.btnResetMask = new System.Windows.Forms.Button();
            this.lblBrushSize = new System.Windows.Forms.Label();
            this.tbBrushSize = new System.Windows.Forms.TrackBar();
            this.rbEraser = new System.Windows.Forms.RadioButton();
            this.rbBrush = new System.Windows.Forms.RadioButton();
            this.pnlHeader.SuspendLayout();
            this.tlpConfig.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.tlpMain.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.grpChat.SuspendLayout();
            this.pnlPreview.SuspendLayout();
            this.canvasPanel.SuspendLayout();
            this.pnlCanvasControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbBrushSize)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(35)))));
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(950, 50);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(10, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(166, 21);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "AI Image Editor";
            // 
            // tlpConfig
            // 
            this.tlpConfig.ColumnCount = 3;
            this.tlpConfig.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpConfig.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpConfig.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlpConfig.Controls.Add(this.lblModel, 0, 0);
            this.tlpConfig.Controls.Add(this.cbModel, 1, 0);
            this.tlpConfig.Controls.Add(this.btnSettings, 2, 0);
            this.tlpConfig.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpConfig.Location = new System.Drawing.Point(0, 50);
            this.tlpConfig.Name = "tlpConfig";
            this.tlpConfig.RowCount = 1;
            this.tlpConfig.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpConfig.Size = new System.Drawing.Size(950, 35);
            this.tlpConfig.TabIndex = 1;
            // 
            // lblModel
            // 
            this.lblModel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblModel.Location = new System.Drawing.Point(3, 7);
            this.lblModel.Name = "lblModel";
            this.lblModel.Size = new System.Drawing.Size(50, 20);
            this.lblModel.TabIndex = 0;
            this.lblModel.Text = "Model:";
            this.lblModel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbModel
            // 
            this.cbModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbModel.FormattingEnabled = true;
            this.cbModel.Location = new System.Drawing.Point(59, 3);
            this.cbModel.Name = "cbModel";
            this.cbModel.Size = new System.Drawing.Size(738, 21);
            this.cbModel.TabIndex = 1;
            // 
            // btnSettings
            // 
            this.btnSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSettings.Location = new System.Drawing.Point(803, 3);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(144, 29);
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
            this.pnlBottom.Location = new System.Drawing.Point(0, 610);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(950, 90);
            this.pnlBottom.TabIndex = 2;
            // 
            // chkScaleToFit
            // 
            this.chkScaleToFit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkScaleToFit.Checked = true;
            this.chkScaleToFit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkScaleToFit.Location = new System.Drawing.Point(20, 45);
            this.chkScaleToFit.Name = "chkScaleToFit";
            this.chkScaleToFit.Size = new System.Drawing.Size(400, 30);
            this.chkScaleToFit.TabIndex = 3;
            this.chkScaleToFit.Text = "Scale generated image to fit selection bounds";
            // 
            // chkCopyToClipboard
            // 
            this.chkCopyToClipboard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkCopyToClipboard.Checked = true;
            this.chkCopyToClipboard.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCopyToClipboard.Location = new System.Drawing.Point(20, 10);
            this.chkCopyToClipboard.Name = "chkCopyToClipboard";
            this.chkCopyToClipboard.Size = new System.Drawing.Size(400, 30);
            this.chkCopyToClipboard.TabIndex = 0;
            this.chkCopyToClipboard.Text = "Copy changes to clipboard (paste as new layer via Ctrl+Alt+V)";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(670, 25);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(125, 40);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "Commit Layer";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(805, 25);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(125, 40);
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
            this.tlpMain.Location = new System.Drawing.Point(0, 85);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 1;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Size = new System.Drawing.Size(950, 525);
            this.tlpMain.TabIndex = 3;
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.grpChat);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLeft.Location = new System.Drawing.Point(3, 3);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.pnlLeft.Size = new System.Drawing.Size(344, 519);
            this.pnlLeft.TabIndex = 0;
            // 
            // grpChat
            // 
            this.grpChat.Controls.Add(this.txtChatHistory);
            this.grpChat.Controls.Add(this.txtPrompt);
            this.grpChat.Controls.Add(this.btnGenerate);
            this.grpChat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpChat.Location = new System.Drawing.Point(0, 0);
            this.grpChat.Name = "grpChat";
            this.grpChat.Size = new System.Drawing.Size(334, 519);
            this.grpChat.TabIndex = 1;
            this.grpChat.TabStop = false;
            this.grpChat.Text = "Conversational Canvas Editor";
            // 
            // txtChatHistory
            // 
            this.txtChatHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtChatHistory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(250)))));
            this.txtChatHistory.Location = new System.Drawing.Point(10, 20);
            this.txtChatHistory.Multiline = true;
            this.txtChatHistory.Name = "txtChatHistory";
            this.txtChatHistory.ReadOnly = true;
            this.txtChatHistory.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtChatHistory.Size = new System.Drawing.Size(314, 280);
            this.txtChatHistory.TabIndex = 0;
            // 
            // txtPrompt
            // 
            this.txtPrompt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPrompt.Location = new System.Drawing.Point(10, 310);
            this.txtPrompt.Multiline = true;
            this.txtPrompt.Name = "txtPrompt";
            this.txtPrompt.Size = new System.Drawing.Size(314, 140);
            this.txtPrompt.TabIndex = 1;
            this.txtPrompt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtPrompt_KeyDown);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerate.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnGenerate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnGenerate.Location = new System.Drawing.Point(10, 460);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(314, 50);
            this.btnGenerate.TabIndex = 2;
            this.btnGenerate.Text = "Generate Edit";
            this.btnGenerate.UseVisualStyleBackColor = false;
            this.btnGenerate.Click += new System.EventHandler(this.BtnGenerate_Click);
            // 
            // pnlPreview
            // 
            this.pnlPreview.BackColor = System.Drawing.Color.DarkGray;
            this.pnlPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPreview.Controls.Add(this.lblPreviewTitle);
            this.pnlPreview.Controls.Add(this.canvasPanel);
            this.pnlPreview.Controls.Add(this.prgLoading);
            this.pnlPreview.Controls.Add(this.pnlCanvasControls);
            this.pnlPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPreview.Location = new System.Drawing.Point(353, 3);
            this.pnlPreview.Name = "pnlPreview";
            this.pnlPreview.Size = new System.Drawing.Size(594, 519);
            this.pnlPreview.TabIndex = 1;
            // 
            // lblPreviewTitle
            // 
            this.lblPreviewTitle.AutoSize = true;
            this.lblPreviewTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblPreviewTitle.ForeColor = System.Drawing.Color.Black;
            this.lblPreviewTitle.Location = new System.Drawing.Point(10, 10);
            this.lblPreviewTitle.Name = "lblPreviewTitle";
            this.lblPreviewTitle.Size = new System.Drawing.Size(111, 15);
            this.lblPreviewTitle.TabIndex = 0;
            this.lblPreviewTitle.Text = "Interactive Canvas";
            // 
            // canvasPanel
            // 
            this.canvasPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.canvasPanel.Controls.Add(this.canvas);
            this.canvasPanel.Location = new System.Drawing.Point(10, 75);
            this.canvasPanel.Name = "canvasPanel";
            this.canvasPanel.Size = new System.Drawing.Size(572, 399);
            this.canvasPanel.TabIndex = 3;
            // 
            // canvas
            // 
            this.canvas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.canvas.BrushSize = 20;
            this.canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.canvas.IsBrushMode = true;
            this.canvas.Location = new System.Drawing.Point(0, 0);
            this.canvas.MaskImage = null;
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(572, 399);
            this.canvas.SourceImage = null;
            this.canvas.TabIndex = 0;
            // 
            // prgLoading
            // 
            this.prgLoading.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.prgLoading.Location = new System.Drawing.Point(10, 485);
            this.prgLoading.Name = "prgLoading";
            this.prgLoading.Size = new System.Drawing.Size(572, 20);
            this.prgLoading.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.prgLoading.TabIndex = 2;
            this.prgLoading.Visible = false;
            // 
            // pnlCanvasControls
            // 
            this.pnlCanvasControls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlCanvasControls.Controls.Add(this.btnZoomFit);
            this.pnlCanvasControls.Controls.Add(this.btnResetMask);
            this.pnlCanvasControls.Controls.Add(this.cbBrushColor);
            this.pnlCanvasControls.Controls.Add(this.lblBrushColor);
            this.pnlCanvasControls.Controls.Add(this.lblBrushSize);
            this.pnlCanvasControls.Controls.Add(this.tbBrushSize);
            this.pnlCanvasControls.Controls.Add(this.rbEraser);
            this.pnlCanvasControls.Controls.Add(this.rbBrush);
            this.pnlCanvasControls.Location = new System.Drawing.Point(10, 30);
            this.pnlCanvasControls.Name = "pnlCanvasControls";
            this.pnlCanvasControls.Size = new System.Drawing.Size(572, 40);
            this.pnlCanvasControls.TabIndex = 4;
            // 
            // btnZoomFit
            // 
            this.btnZoomFit.Location = new System.Drawing.Point(490, 5);
            this.btnZoomFit.Name = "btnZoomFit";
            this.btnZoomFit.Size = new System.Drawing.Size(75, 30);
            this.btnZoomFit.TabIndex = 5;
            this.btnZoomFit.Text = "Zoom Fit";
            this.btnZoomFit.UseVisualStyleBackColor = true;
            this.btnZoomFit.Click += new System.EventHandler(this.BtnZoomFit_Click);
            // 
            // btnResetMask
            // 
            this.btnResetMask.Location = new System.Drawing.Point(405, 5);
            this.btnResetMask.Name = "btnResetMask";
            this.btnResetMask.Size = new System.Drawing.Size(80, 30);
            this.btnResetMask.TabIndex = 4;
            this.btnResetMask.Text = "Reset Mask";
            this.btnResetMask.UseVisualStyleBackColor = true;
            this.btnResetMask.Click += new System.EventHandler(this.BtnResetMask_Click);
            // 
            // lblBrushSize
            // 
            this.lblBrushSize.AutoSize = true;
            this.lblBrushSize.Location = new System.Drawing.Point(220, 12);
            this.lblBrushSize.Name = "lblBrushSize";
            this.lblBrushSize.Size = new System.Drawing.Size(58, 13);
            this.lblBrushSize.TabIndex = 3;
            this.lblBrushSize.Text = "Size: 20 px";
            // 
            // tbBrushSize
            // 
            this.tbBrushSize.Location = new System.Drawing.Point(135, 3);
            this.tbBrushSize.Maximum = 100;
            this.tbBrushSize.Minimum = 2;
            this.tbBrushSize.Name = "tbBrushSize";
            this.tbBrushSize.Size = new System.Drawing.Size(80, 45);
            this.tbBrushSize.TabIndex = 2;
            this.tbBrushSize.TickFrequency = 10;
            this.tbBrushSize.Value = 20;
            this.tbBrushSize.Scroll += new System.EventHandler(this.TbBrushSize_Scroll);
            // 
            // lblBrushColor
            // 
            this.lblBrushColor.AutoSize = true;
            this.lblBrushColor.Location = new System.Drawing.Point(280, 12);
            this.lblBrushColor.Name = "lblBrushColor";
            this.lblBrushColor.Size = new System.Drawing.Size(34, 13);
            this.lblBrushColor.TabIndex = 6;
            this.lblBrushColor.Text = "Color:";
            // 
            // cbBrushColor
            // 
            this.cbBrushColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBrushColor.FormattingEnabled = true;
            this.cbBrushColor.Location = new System.Drawing.Point(320, 8);
            this.cbBrushColor.Name = "cbBrushColor";
            this.cbBrushColor.Size = new System.Drawing.Size(80, 21);
            this.cbBrushColor.TabIndex = 7;
            // 
            // rbEraser
            // 
            this.rbEraser.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbEraser.Location = new System.Drawing.Point(70, 5);
            this.rbEraser.Name = "rbEraser";
            this.rbEraser.Size = new System.Drawing.Size(60, 30);
            this.rbEraser.TabIndex = 1;
            this.rbEraser.Text = "Eraser";
            this.rbEraser.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbEraser.UseVisualStyleBackColor = true;
            this.rbEraser.CheckedChanged += new System.EventHandler(this.RbTool_CheckedChanged);
            // 
            // rbBrush
            // 
            this.rbBrush.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbBrush.Checked = true;
            this.rbBrush.Location = new System.Drawing.Point(5, 5);
            this.rbBrush.Name = "rbBrush";
            this.rbBrush.Size = new System.Drawing.Size(60, 30);
            this.rbBrush.TabIndex = 0;
            this.rbBrush.TabStop = true;
            this.rbBrush.Text = "Brush";
            this.rbBrush.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbBrush.UseVisualStyleBackColor = true;
            this.rbBrush.CheckedChanged += new System.EventHandler(this.RbTool_CheckedChanged);
            // 
            // AiEditDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(950, 700);
            this.Controls.Add(this.tlpMain);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.tlpConfig);
            this.Controls.Add(this.pnlHeader);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(900, 650);
            this.Name = "AiEditDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AI Image Editor";
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.tlpConfig.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            this.tlpMain.ResumeLayout(false);
            this.pnlLeft.ResumeLayout(false);
            this.grpChat.ResumeLayout(false);
            this.grpChat.PerformLayout();
            this.pnlPreview.ResumeLayout(false);
            this.pnlPreview.PerformLayout();
            this.canvasPanel.ResumeLayout(false);
            this.pnlCanvasControls.ResumeLayout(false);
            this.pnlCanvasControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbBrushSize)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblModel;
        private System.Windows.Forms.ComboBox cbModel;
        private System.Windows.Forms.Button btnSettings;
        
        private System.Windows.Forms.GroupBox grpChat;
        private System.Windows.Forms.TextBox txtChatHistory;
        private System.Windows.Forms.TextBox txtPrompt;
        private System.Windows.Forms.Button btnGenerate;
        
        private System.Windows.Forms.Panel pnlPreview;
        private System.Windows.Forms.Label lblPreviewTitle;
        private System.Windows.Forms.ProgressBar prgLoading;
        
        private System.Windows.Forms.CheckBox chkCopyToClipboard;
        private System.Windows.Forms.CheckBox chkScaleToFit;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TableLayoutPanel tlpConfig;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Panel pnlLeft;
        
        private System.Windows.Forms.Panel canvasPanel;
        private InteractiveCanvas canvas;
        private System.Windows.Forms.Panel pnlCanvasControls;
        private System.Windows.Forms.RadioButton rbEraser;
        private System.Windows.Forms.RadioButton rbBrush;
        private System.Windows.Forms.TrackBar tbBrushSize;
        private System.Windows.Forms.Label lblBrushSize;
        private System.Windows.Forms.Button btnResetMask;
        private System.Windows.Forms.Button btnZoomFit;
        private System.Windows.Forms.Label lblBrushColor;
        private System.Windows.Forms.ComboBox cbBrushColor;
    }
}

