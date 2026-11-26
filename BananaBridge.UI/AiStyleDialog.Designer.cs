namespace BananaBridge.UI
{
    partial class AiStyleDialog
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
            this.grpStrength = new System.Windows.Forms.GroupBox();
            this.lblStrengthValue = new System.Windows.Forms.Label();
            this.tbStrength = new System.Windows.Forms.TrackBar();
            this.grpCharacteristics = new System.Windows.Forms.GroupBox();
            this.chkAnatomy = new System.Windows.Forms.CheckBox();
            this.chkFrame = new System.Windows.Forms.CheckBox();
            this.chkColoring = new System.Windows.Forms.CheckBox();
            this.chkLineQuality = new System.Windows.Forms.CheckBox();
            this.chkRendering = new System.Windows.Forms.CheckBox();
            this.chkSubstrate = new System.Windows.Forms.CheckBox();
            this.grpStyles = new System.Windows.Forms.GroupBox();
            this.lblStyle = new System.Windows.Forms.Label();
            this.cbStyles = new System.Windows.Forms.ComboBox();
            this.lblCategory = new System.Windows.Forms.Label();
            this.cbCategory = new System.Windows.Forms.ComboBox();
            this.txtStyleDesc = new System.Windows.Forms.TextBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.pnlPreview = new System.Windows.Forms.Panel();
            this.lblPreviewTitle = new System.Windows.Forms.Label();
            this.pbPreview = new System.Windows.Forms.PictureBox();
            this.prgLoading = new System.Windows.Forms.ProgressBar();
            this.pnlHeader.SuspendLayout();
            this.tlpConfig.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.tlpMain.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.grpStrength.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbStrength)).BeginInit();
            this.grpCharacteristics.SuspendLayout();
            this.grpStyles.SuspendLayout();
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
            this.lblTitle.Size = new System.Drawing.Size(225, 21);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "AI Style Applier";
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
            this.pnlLeft.Controls.Add(this.grpStrength);
            this.pnlLeft.Controls.Add(this.grpCharacteristics);
            this.pnlLeft.Controls.Add(this.grpStyles);
            this.pnlLeft.Controls.Add(this.btnGenerate);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLeft.Location = new System.Drawing.Point(3, 3);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.pnlLeft.Size = new System.Drawing.Size(344, 519);
            this.pnlLeft.TabIndex = 0;
            // 
            // grpStrength
            // 
            this.grpStrength.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpStrength.Controls.Add(this.lblStrengthValue);
            this.grpStrength.Controls.Add(this.tbStrength);
            this.grpStrength.Location = new System.Drawing.Point(3, 375);
            this.grpStrength.Name = "grpStrength";
            this.grpStrength.Size = new System.Drawing.Size(328, 75);
            this.grpStrength.TabIndex = 2;
            this.grpStrength.TabStop = false;
            this.grpStrength.Text = "Style Strength";
            // 
            // lblStrengthValue
            // 
            this.lblStrengthValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStrengthValue.AutoSize = true;
            this.lblStrengthValue.Location = new System.Drawing.Point(280, 20);
            this.lblStrengthValue.Name = "lblStrengthValue";
            this.lblStrengthValue.Size = new System.Drawing.Size(27, 13);
            this.lblStrengthValue.TabIndex = 1;
            this.lblStrengthValue.Text = "70%";
            // 
            // tbStrength
            // 
            this.tbStrength.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbStrength.Location = new System.Drawing.Point(6, 20);
            this.tbStrength.Maximum = 100;
            this.tbStrength.Minimum = 10;
            this.tbStrength.Name = "tbStrength";
            this.tbStrength.Size = new System.Drawing.Size(268, 45);
            this.tbStrength.TabIndex = 0;
            this.tbStrength.TickFrequency = 10;
            this.tbStrength.Value = 70;
            this.tbStrength.Scroll += new System.EventHandler(this.TbStrength_Scroll);
            // 
            // grpCharacteristics
            // 
            this.grpCharacteristics.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpCharacteristics.Controls.Add(this.chkAnatomy);
            this.grpCharacteristics.Controls.Add(this.chkFrame);
            this.grpCharacteristics.Controls.Add(this.chkColoring);
            this.grpCharacteristics.Controls.Add(this.chkLineQuality);
            this.grpCharacteristics.Controls.Add(this.chkRendering);
            this.grpCharacteristics.Controls.Add(this.chkSubstrate);
            this.grpCharacteristics.Location = new System.Drawing.Point(3, 260);
            this.grpCharacteristics.Name = "grpCharacteristics";
            this.grpCharacteristics.Size = new System.Drawing.Size(328, 110);
            this.grpCharacteristics.TabIndex = 1;
            this.grpCharacteristics.TabStop = false;
            this.grpCharacteristics.Text = "Style Characteristics";
            // 
            // chkAnatomy
            // 
            this.chkAnatomy.AutoSize = true;
            this.chkAnatomy.Checked = true;
            this.chkAnatomy.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAnatomy.Location = new System.Drawing.Point(120, 50);
            this.chkAnatomy.Name = "chkAnatomy";
            this.chkAnatomy.Size = new System.Drawing.Size(67, 17);
            this.chkAnatomy.TabIndex = 4;
            this.chkAnatomy.Text = "Anatomy";
            this.chkAnatomy.UseVisualStyleBackColor = true;
            // 
            // chkFrame
            // 
            this.chkFrame.AutoSize = true;
            this.chkFrame.Checked = false;
            this.chkFrame.CheckState = System.Windows.Forms.CheckState.Unchecked;
            this.chkFrame.Location = new System.Drawing.Point(120, 80);
            this.chkFrame.Name = "chkFrame";
            this.chkFrame.Size = new System.Drawing.Size(95, 17);
            this.chkFrame.TabIndex = 5;
            this.chkFrame.Text = "Frame/Border";
            this.chkFrame.UseVisualStyleBackColor = true;
            // 
            // chkColoring
            // 
            this.chkColoring.AutoSize = true;
            this.chkColoring.Checked = true;
            this.chkColoring.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkColoring.Location = new System.Drawing.Point(120, 20);
            this.chkColoring.Name = "chkColoring";
            this.chkColoring.Size = new System.Drawing.Size(121, 17);
            this.chkColoring.TabIndex = 3;
            this.chkColoring.Text = "Coloring/Pigmentation";
            this.chkColoring.UseVisualStyleBackColor = true;
            // 
            // chkLineQuality
            // 
            this.chkLineQuality.AutoSize = true;
            this.chkLineQuality.Checked = true;
            this.chkLineQuality.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLineQuality.Location = new System.Drawing.Point(10, 80);
            this.chkLineQuality.Name = "chkLineQuality";
            this.chkLineQuality.Size = new System.Drawing.Size(81, 17);
            this.chkLineQuality.TabIndex = 2;
            this.chkLineQuality.Text = "Line Quality";
            this.chkLineQuality.UseVisualStyleBackColor = true;
            // 
            // chkRendering
            // 
            this.chkRendering.AutoSize = true;
            this.chkRendering.Checked = true;
            this.chkRendering.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRendering.Location = new System.Drawing.Point(10, 50);
            this.chkRendering.Name = "chkRendering";
            this.chkRendering.Size = new System.Drawing.Size(75, 17);
            this.chkRendering.TabIndex = 1;
            this.chkRendering.Text = "Rendering";
            this.chkRendering.UseVisualStyleBackColor = true;
            // 
            // chkSubstrate
            // 
            this.chkSubstrate.AutoSize = true;
            this.chkSubstrate.Checked = true;
            this.chkSubstrate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSubstrate.Location = new System.Drawing.Point(10, 20);
            this.chkSubstrate.Name = "chkSubstrate";
            this.chkSubstrate.Size = new System.Drawing.Size(71, 17);
            this.chkSubstrate.TabIndex = 0;
            this.chkSubstrate.Text = "Substrate";
            this.chkSubstrate.UseVisualStyleBackColor = true;
            // 
            // grpStyles
            // 
            this.grpStyles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpStyles.Controls.Add(this.lblStyle);
            this.grpStyles.Controls.Add(this.cbStyles);
            this.grpStyles.Controls.Add(this.lblCategory);
            this.grpStyles.Controls.Add(this.cbCategory);
            this.grpStyles.Controls.Add(this.txtStyleDesc);
            this.grpStyles.Location = new System.Drawing.Point(3, 0);
            this.grpStyles.Name = "grpStyles";
            this.grpStyles.Size = new System.Drawing.Size(328, 255);
            this.grpStyles.TabIndex = 0;
            this.grpStyles.TabStop = false;
            this.grpStyles.Text = "Image Style Preset";
            // 
            // lblStyle
            // 
            this.lblStyle.AutoSize = true;
            this.lblStyle.Location = new System.Drawing.Point(6, 62);
            this.lblStyle.Name = "lblStyle";
            this.lblStyle.Size = new System.Drawing.Size(33, 13);
            this.lblStyle.TabIndex = 4;
            this.lblStyle.Text = "Style:";
            // 
            // cbStyles
            // 
            this.cbStyles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbStyles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStyles.Location = new System.Drawing.Point(70, 59);
            this.cbStyles.Name = "cbStyles";
            this.cbStyles.Size = new System.Drawing.Size(252, 21);
            this.cbStyles.TabIndex = 3;
            this.cbStyles.SelectedIndexChanged += new System.EventHandler(this.CbStyles_SelectedIndexChanged);
            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.Location = new System.Drawing.Point(6, 28);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(52, 13);
            this.lblCategory.TabIndex = 2;
            this.lblCategory.Text = "Category:";
            // 
            // cbCategory
            // 
            this.cbCategory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCategory.Location = new System.Drawing.Point(70, 25);
            this.cbCategory.Name = "cbCategory";
            this.cbCategory.Size = new System.Drawing.Size(252, 21);
            this.cbCategory.TabIndex = 0;
            this.cbCategory.SelectedIndexChanged += new System.EventHandler(this.CbCategory_SelectedIndexChanged);
            // 
            // txtStyleDesc
            // 
            this.txtStyleDesc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStyleDesc.Location = new System.Drawing.Point(9, 90);
            this.txtStyleDesc.Multiline = true;
            this.txtStyleDesc.Name = "txtStyleDesc";
            this.txtStyleDesc.ReadOnly = true;
            this.txtStyleDesc.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtStyleDesc.Size = new System.Drawing.Size(313, 155);
            this.txtStyleDesc.TabIndex = 1;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerate.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnGenerate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnGenerate.Location = new System.Drawing.Point(3, 465);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(328, 45);
            this.btnGenerate.TabIndex = 2;
            this.btnGenerate.Text = "Apply Style";
            this.btnGenerate.UseVisualStyleBackColor = false;
            this.btnGenerate.Click += new System.EventHandler(this.BtnGenerate_Click);
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
            this.pbPreview.Location = new System.Drawing.Point(10, 30);
            this.pbPreview.Name = "pbPreview";
            this.pbPreview.Size = new System.Drawing.Size(572, 444);
            this.pbPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbPreview.TabIndex = 1;
            this.pbPreview.TabStop = false;
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
            // AiStyleDialog
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
            this.Name = "AiStyleDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AI Style Applier";
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.tlpConfig.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            this.tlpMain.ResumeLayout(false);
            this.pnlLeft.ResumeLayout(false);
            this.grpStrength.ResumeLayout(false);
            this.grpStrength.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbStrength)).EndInit();
            this.grpCharacteristics.ResumeLayout(false);
            this.grpCharacteristics.PerformLayout();
            this.grpStyles.ResumeLayout(false);
            this.grpStyles.PerformLayout();
            this.pnlPreview.ResumeLayout(false);
            this.pnlPreview.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblModel;
        private System.Windows.Forms.ComboBox cbModel;
        private System.Windows.Forms.Button btnSettings;
        
        private System.Windows.Forms.GroupBox grpStyles;
        private System.Windows.Forms.ComboBox cbCategory;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.ComboBox cbStyles;
        private System.Windows.Forms.Label lblStyle;
        private System.Windows.Forms.TextBox txtStyleDesc;

        private System.Windows.Forms.GroupBox grpCharacteristics;
        private System.Windows.Forms.CheckBox chkSubstrate;
        private System.Windows.Forms.CheckBox chkRendering;
        private System.Windows.Forms.CheckBox chkLineQuality;
        private System.Windows.Forms.CheckBox chkColoring;
        private System.Windows.Forms.CheckBox chkAnatomy;
        private System.Windows.Forms.CheckBox chkFrame;

        private System.Windows.Forms.GroupBox grpStrength;
        private System.Windows.Forms.TrackBar tbStrength;
        private System.Windows.Forms.Label lblStrengthValue;

        private System.Windows.Forms.Button btnGenerate;
        
        private System.Windows.Forms.Panel pnlPreview;
        private System.Windows.Forms.PictureBox pbPreview;
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
    }
}

