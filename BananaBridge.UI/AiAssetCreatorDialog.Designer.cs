namespace BananaBridge.UI
{
	partial class AiAssetCreatorDialog
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
			this.btnGenerate = new System.Windows.Forms.Button();
			this.btnLoad = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
			this.pnlLeft = new System.Windows.Forms.Panel();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabCharacter = new System.Windows.Forms.TabPage();
			this.lblSpecies = new System.Windows.Forms.Label();
			this.cbSpecies = new System.Windows.Forms.ComboBox();
			this.lblGender = new System.Windows.Forms.Label();
			this.cbGender = new System.Windows.Forms.ComboBox();
			this.lblPersonality = new System.Windows.Forms.Label();
			this.cbPersonality = new System.Windows.Forms.ComboBox();
			this.lblFashion = new System.Windows.Forms.Label();
			this.cbFashion = new System.Windows.Forms.ComboBox();
			this.lblAccessory = new System.Windows.Forms.Label();
			this.cbAccessory = new System.Windows.Forms.ComboBox();
			this.lblPalette = new System.Windows.Forms.Label();
			this.cbPalette = new System.Windows.Forms.ComboBox();
			this.lblPose = new System.Windows.Forms.Label();
			this.cbPose = new System.Windows.Forms.ComboBox();
			this.chkMultiView = new System.Windows.Forms.CheckBox();
			this.tabLocation = new System.Windows.Forms.TabPage();
			this.lblLocationType = new System.Windows.Forms.Label();
			this.cbLocationType = new System.Windows.Forms.ComboBox();
			this.lblStructure = new System.Windows.Forms.Label();
			this.cbStructure = new System.Windows.Forms.ComboBox();
			this.lblVisualState = new System.Windows.Forms.Label();
			this.cbVisualState = new System.Windows.Forms.ComboBox();
			this.lblAtmosphere = new System.Windows.Forms.Label();
			this.cbAtmosphere = new System.Windows.Forms.ComboBox();
			this.tabProp = new System.Windows.Forms.TabPage();
			this.lblPropName = new System.Windows.Forms.Label();
			this.txtPropName = new System.Windows.Forms.TextBox();
			this.lblMaterial = new System.Windows.Forms.Label();
			this.cbMaterial = new System.Windows.Forms.ComboBox();
			this.lblCondition = new System.Windows.Forms.Label();
			this.cbCondition = new System.Windows.Forms.ComboBox();
			this.grpStyle = new System.Windows.Forms.GroupBox();
			this.lblStyleCategory = new System.Windows.Forms.Label();
			this.cbStyleCategory = new System.Windows.Forms.ComboBox();
			this.lblStyle = new System.Windows.Forms.Label();
			this.cbStyle = new System.Windows.Forms.ComboBox();
			this.grpPrompt = new System.Windows.Forms.GroupBox();
			this.lblPromptHelp = new System.Windows.Forms.Label();
			this.txtPrompt = new System.Windows.Forms.TextBox();
			this.pnlPreview = new System.Windows.Forms.Panel();
			this.lblPreviewTitle = new System.Windows.Forms.Label();
			this.pbPreview = new System.Windows.Forms.PictureBox();
			this.prgLoading = new System.Windows.Forms.ProgressBar();
			this.pnlHeader.SuspendLayout();
			this.tlpConfig.SuspendLayout();
			this.pnlBottom.SuspendLayout();
			this.tlpMain.SuspendLayout();
			this.pnlLeft.SuspendLayout();
			this.tabControl.SuspendLayout();
			this.tabCharacter.SuspendLayout();
			this.tabLocation.SuspendLayout();
			this.tabProp.SuspendLayout();
			this.grpStyle.SuspendLayout();
			this.grpPrompt.SuspendLayout();
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
			this.pnlHeader.Size = new System.Drawing.Size(1020, 60);
			this.pnlHeader.TabIndex = 0;
			// 
			// lblTitle
			// 
			this.lblTitle.AutoSize = true;
			this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			this.lblTitle.ForeColor = System.Drawing.Color.White;
			this.lblTitle.Location = new System.Drawing.Point(12, 18);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(130, 21);
			this.lblTitle.TabIndex = 0;
			this.lblTitle.Text = "AI Asset Creator";
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
			this.tlpConfig.Size = new System.Drawing.Size(1020, 45);
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
			this.cbModel.Size = new System.Drawing.Size(828, 21);
			this.cbModel.TabIndex = 1;
			// 
			// btnSettings
			// 
			this.btnSettings.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnSettings.Location = new System.Drawing.Point(903, 10);
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
			this.pnlBottom.Controls.Add(this.btnGenerate);
			this.pnlBottom.Controls.Add(this.btnLoad);
			this.pnlBottom.Controls.Add(this.btnSave);
			this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlBottom.Location = new System.Drawing.Point(0, 680);
			this.pnlBottom.Name = "pnlBottom";
			this.pnlBottom.Size = new System.Drawing.Size(1020, 100);
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
			this.btnOK.Location = new System.Drawing.Point(760, 30);
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
			this.btnCancel.Location = new System.Drawing.Point(880, 30);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(110, 40);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnLoad
			// 
			this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnLoad.Location = new System.Drawing.Point(370, 30);
			this.btnLoad.Name = "btnLoad";
			this.btnLoad.Size = new System.Drawing.Size(110, 40);
			this.btnLoad.TabIndex = 4;
			this.btnLoad.Text = "Load Options...";
			this.btnLoad.UseVisualStyleBackColor = true;
			this.btnLoad.Click += new System.EventHandler(this.BtnLoad_Click);
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSave.Location = new System.Drawing.Point(490, 30);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(110, 40);
			this.btnSave.TabIndex = 5;
			this.btnSave.Text = "Save Options...";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
			// 
			// btnGenerate
			// 
			this.btnGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnGenerate.BackColor = System.Drawing.Color.LightSkyBlue;
			this.btnGenerate.Location = new System.Drawing.Point(610, 30);
			this.btnGenerate.Name = "btnGenerate";
			this.btnGenerate.Size = new System.Drawing.Size(140, 40);
			this.btnGenerate.TabIndex = 6;
			this.btnGenerate.Text = "Generate Asset";
			this.btnGenerate.UseVisualStyleBackColor = false;
			this.btnGenerate.Click += new System.EventHandler(this.BtnGenerate_Click);
			// 
			// tlpMain
			// 
			this.tlpMain.ColumnCount = 2;
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 380F));
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpMain.Controls.Add(this.pnlLeft, 0, 0);
			this.tlpMain.Controls.Add(this.pnlPreview, 1, 0);
			this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpMain.Location = new System.Drawing.Point(0, 105);
			this.tlpMain.Name = "tlpMain";
			this.tlpMain.RowCount = 1;
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpMain.Size = new System.Drawing.Size(1020, 575);
			this.tlpMain.TabIndex = 3;
			// 
			// pnlLeft
			// 
			this.pnlLeft.Controls.Add(this.tabControl);
			this.pnlLeft.Controls.Add(this.grpStyle);
			this.pnlLeft.Controls.Add(this.grpPrompt);
			this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlLeft.Location = new System.Drawing.Point(3, 3);
			this.pnlLeft.Name = "pnlLeft";
			this.pnlLeft.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
			this.pnlLeft.Size = new System.Drawing.Size(374, 569);
			this.pnlLeft.TabIndex = 0;
			// 
			// tabControl
			// 
			this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl.Controls.Add(this.tabCharacter);
			this.tabControl.Controls.Add(this.tabLocation);
			this.tabControl.Controls.Add(this.tabProp);
			this.tabControl.Location = new System.Drawing.Point(3, 109);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(361, 240);
			this.tabControl.TabIndex = 0;
			// 
			// tabCharacter
			// 
			this.tabCharacter.AutoScroll = true;
			this.tabCharacter.Controls.Add(this.lblSpecies);
			this.tabCharacter.Controls.Add(this.cbSpecies);
			this.tabCharacter.Controls.Add(this.lblGender);
			this.tabCharacter.Controls.Add(this.cbGender);
			this.tabCharacter.Controls.Add(this.lblPersonality);
			this.tabCharacter.Controls.Add(this.cbPersonality);
			this.tabCharacter.Controls.Add(this.lblFashion);
			this.tabCharacter.Controls.Add(this.cbFashion);
			this.tabCharacter.Controls.Add(this.lblAccessory);
			this.tabCharacter.Controls.Add(this.cbAccessory);
			this.tabCharacter.Controls.Add(this.lblPalette);
			this.tabCharacter.Controls.Add(this.cbPalette);
			this.tabCharacter.Controls.Add(this.lblPose);
			this.tabCharacter.Controls.Add(this.cbPose);
			this.tabCharacter.Controls.Add(this.chkMultiView);
			this.tabCharacter.Location = new System.Drawing.Point(4, 22);
			this.tabCharacter.Name = "tabCharacter";
			this.tabCharacter.Padding = new System.Windows.Forms.Padding(10);
			this.tabCharacter.Size = new System.Drawing.Size(353, 214);
			this.tabCharacter.TabIndex = 0;
			this.tabCharacter.Text = "Character";
			this.tabCharacter.UseVisualStyleBackColor = true;
			// 
			// lblSpecies
			// 
			this.lblSpecies.Location = new System.Drawing.Point(10, 10);
			this.lblSpecies.Name = "lblSpecies";
			this.lblSpecies.Size = new System.Drawing.Size(90, 20);
			this.lblSpecies.TabIndex = 0;
			this.lblSpecies.Text = "Species:";
			this.lblSpecies.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbSpecies
			// 
			this.cbSpecies.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
			this.cbSpecies.FormattingEnabled = true;
			this.cbSpecies.Location = new System.Drawing.Point(110, 10);
			this.cbSpecies.Name = "cbSpecies";
			this.cbSpecies.Size = new System.Drawing.Size(210, 21);
			this.cbSpecies.TabIndex = 1;
			// 
			// lblGender
			// 
			this.lblGender.Location = new System.Drawing.Point(10, 35);
			this.lblGender.Name = "lblGender";
			this.lblGender.Size = new System.Drawing.Size(90, 20);
			this.lblGender.TabIndex = 2;
			this.lblGender.Text = "Gender:";
			this.lblGender.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbGender
			// 
			this.cbGender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
			this.cbGender.FormattingEnabled = true;
			this.cbGender.Location = new System.Drawing.Point(110, 35);
			this.cbGender.Name = "cbGender";
			this.cbGender.Size = new System.Drawing.Size(210, 21);
			this.cbGender.TabIndex = 3;
			// 
			// lblPersonality
			// 
			this.lblPersonality.Location = new System.Drawing.Point(10, 60);
			this.lblPersonality.Name = "lblPersonality";
			this.lblPersonality.Size = new System.Drawing.Size(90, 20);
			this.lblPersonality.TabIndex = 4;
			this.lblPersonality.Text = "Personality:";
			this.lblPersonality.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbPersonality
			// 
			this.cbPersonality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
			this.cbPersonality.FormattingEnabled = true;
			this.cbPersonality.Location = new System.Drawing.Point(110, 60);
			this.cbPersonality.Name = "cbPersonality";
			this.cbPersonality.Size = new System.Drawing.Size(210, 21);
			this.cbPersonality.TabIndex = 5;
			// 
			// lblFashion
			// 
			this.lblFashion.Location = new System.Drawing.Point(10, 85);
			this.lblFashion.Name = "lblFashion";
			this.lblFashion.Size = new System.Drawing.Size(90, 20);
			this.lblFashion.TabIndex = 6;
			this.lblFashion.Text = "Fashion:";
			this.lblFashion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbFashion
			// 
			this.cbFashion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
			this.cbFashion.FormattingEnabled = true;
			this.cbFashion.Location = new System.Drawing.Point(110, 85);
			this.cbFashion.Name = "cbFashion";
			this.cbFashion.Size = new System.Drawing.Size(210, 21);
			this.cbFashion.TabIndex = 7;
			// 
			// lblAccessory
			// 
			this.lblAccessory.Location = new System.Drawing.Point(10, 110);
			this.lblAccessory.Name = "lblAccessory";
			this.lblAccessory.Size = new System.Drawing.Size(90, 20);
			this.lblAccessory.TabIndex = 8;
			this.lblAccessory.Text = "Accessory:";
			this.lblAccessory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbAccessory
			// 
			this.cbAccessory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
			this.cbAccessory.FormattingEnabled = true;
			this.cbAccessory.Location = new System.Drawing.Point(110, 110);
			this.cbAccessory.Name = "cbAccessory";
			this.cbAccessory.Size = new System.Drawing.Size(210, 21);
			this.cbAccessory.TabIndex = 9;
			// 
			// lblPalette
			// 
			this.lblPalette.Location = new System.Drawing.Point(10, 135);
			this.lblPalette.Name = "lblPalette";
			this.lblPalette.Size = new System.Drawing.Size(90, 20);
			this.lblPalette.TabIndex = 10;
			this.lblPalette.Text = "Palette:";
			this.lblPalette.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbPalette
			// 
			this.cbPalette.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
			this.cbPalette.FormattingEnabled = true;
			this.cbPalette.Location = new System.Drawing.Point(110, 135);
			this.cbPalette.Name = "cbPalette";
			this.cbPalette.Size = new System.Drawing.Size(210, 21);
			this.cbPalette.TabIndex = 11;
			// 
			// lblPose
			// 
			this.lblPose.Location = new System.Drawing.Point(10, 160);
			this.lblPose.Name = "lblPose";
			this.lblPose.Size = new System.Drawing.Size(90, 20);
			this.lblPose.TabIndex = 12;
			this.lblPose.Text = "Iconic Pose:";
			this.lblPose.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbPose
			// 
			this.cbPose.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
			this.cbPose.FormattingEnabled = true;
			this.cbPose.Location = new System.Drawing.Point(110, 160);
			this.cbPose.Name = "cbPose";
			this.cbPose.Size = new System.Drawing.Size(210, 21);
			this.cbPose.TabIndex = 13;
			// 
			// chkMultiView
			// 
			this.chkMultiView.Location = new System.Drawing.Point(110, 185);
			this.chkMultiView.Name = "chkMultiView";
			this.chkMultiView.Size = new System.Drawing.Size(230, 24);
			this.chkMultiView.TabIndex = 14;
			this.chkMultiView.Text = "Generate Multiple Views (5-view sheet)";
			this.chkMultiView.UseVisualStyleBackColor = true;
			// 
			// tabLocation
			// 
			this.tabLocation.Controls.Add(this.lblLocationType);
			this.tabLocation.Controls.Add(this.cbLocationType);
			this.tabLocation.Controls.Add(this.lblStructure);
			this.tabLocation.Controls.Add(this.cbStructure);
			this.tabLocation.Controls.Add(this.lblVisualState);
			this.tabLocation.Controls.Add(this.cbVisualState);
			this.tabLocation.Controls.Add(this.lblAtmosphere);
			this.tabLocation.Controls.Add(this.cbAtmosphere);
			this.tabLocation.Location = new System.Drawing.Point(4, 22);
			this.tabLocation.Name = "tabLocation";
			this.tabLocation.Padding = new System.Windows.Forms.Padding(10);
			this.tabLocation.Size = new System.Drawing.Size(353, 214);
			this.tabLocation.TabIndex = 1;
			this.tabLocation.Text = "Location";
			this.tabLocation.UseVisualStyleBackColor = true;
			// 
			// lblLocationType
			// 
			this.lblLocationType.Location = new System.Drawing.Point(10, 15);
			this.lblLocationType.Name = "lblLocationType";
			this.lblLocationType.Size = new System.Drawing.Size(90, 20);
			this.lblLocationType.TabIndex = 0;
			this.lblLocationType.Text = "Type:";
			this.lblLocationType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbLocationType
			// 
			this.cbLocationType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
			this.cbLocationType.FormattingEnabled = true;
			this.cbLocationType.Location = new System.Drawing.Point(110, 15);
			this.cbLocationType.Name = "cbLocationType";
			this.cbLocationType.Size = new System.Drawing.Size(230, 21);
			this.cbLocationType.TabIndex = 1;
			// 
			// lblStructure
			// 
			this.lblStructure.Location = new System.Drawing.Point(10, 47);
			this.lblStructure.Name = "lblStructure";
			this.lblStructure.Size = new System.Drawing.Size(90, 20);
			this.lblStructure.TabIndex = 2;
			this.lblStructure.Text = "Structure/Scale:";
			this.lblStructure.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbStructure
			// 
			this.cbStructure.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
			this.cbStructure.FormattingEnabled = true;
			this.cbStructure.Location = new System.Drawing.Point(110, 47);
			this.cbStructure.Name = "cbStructure";
			this.cbStructure.Size = new System.Drawing.Size(230, 21);
			this.cbStructure.TabIndex = 3;
			// 
			// lblVisualState
			// 
			this.lblVisualState.Location = new System.Drawing.Point(10, 79);
			this.lblVisualState.Name = "lblVisualState";
			this.lblVisualState.Size = new System.Drawing.Size(90, 20);
			this.lblVisualState.TabIndex = 4;
			this.lblVisualState.Text = "Visual State:";
			this.lblVisualState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbVisualState
			// 
			this.cbVisualState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
			this.cbVisualState.FormattingEnabled = true;
			this.cbVisualState.Location = new System.Drawing.Point(110, 79);
			this.cbVisualState.Name = "cbVisualState";
			this.cbVisualState.Size = new System.Drawing.Size(230, 21);
			this.cbVisualState.TabIndex = 5;
			// 
			// lblAtmosphere
			// 
			this.lblAtmosphere.Location = new System.Drawing.Point(10, 111);
			this.lblAtmosphere.Name = "lblAtmosphere";
			this.lblAtmosphere.Size = new System.Drawing.Size(90, 20);
			this.lblAtmosphere.TabIndex = 6;
			this.lblAtmosphere.Text = "Atmosphere:";
			this.lblAtmosphere.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbAtmosphere
			// 
			this.cbAtmosphere.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
			this.cbAtmosphere.FormattingEnabled = true;
			this.cbAtmosphere.Location = new System.Drawing.Point(110, 111);
			this.cbAtmosphere.Name = "cbAtmosphere";
			this.cbAtmosphere.Size = new System.Drawing.Size(230, 21);
			this.cbAtmosphere.TabIndex = 7;
			// 
			// tabProp
			// 
			this.tabProp.Controls.Add(this.lblPropName);
			this.tabProp.Controls.Add(this.txtPropName);
			this.tabProp.Controls.Add(this.lblMaterial);
			this.tabProp.Controls.Add(this.cbMaterial);
			this.tabProp.Controls.Add(this.lblCondition);
			this.tabProp.Controls.Add(this.cbCondition);
			this.tabProp.Location = new System.Drawing.Point(4, 22);
			this.tabProp.Name = "tabProp";
			this.tabProp.Padding = new System.Windows.Forms.Padding(10);
			this.tabProp.Size = new System.Drawing.Size(353, 214);
			this.tabProp.TabIndex = 2;
			this.tabProp.Text = "Prop";
			this.tabProp.UseVisualStyleBackColor = true;
			// 
			// lblPropName
			// 
			this.lblPropName.Location = new System.Drawing.Point(10, 15);
			this.lblPropName.Name = "lblPropName";
			this.lblPropName.Size = new System.Drawing.Size(90, 20);
			this.lblPropName.TabIndex = 0;
			this.lblPropName.Text = "Prop Name:";
			this.lblPropName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtPropName
			// 
			this.txtPropName.Location = new System.Drawing.Point(110, 15);
			this.txtPropName.Name = "txtPropName";
			this.txtPropName.Size = new System.Drawing.Size(230, 20);
			this.txtPropName.TabIndex = 1;
			this.txtPropName.Text = "mysterious artifact";
			// 
			// lblMaterial
			// 
			this.lblMaterial.Location = new System.Drawing.Point(10, 47);
			this.lblMaterial.Name = "lblMaterial";
			this.lblMaterial.Size = new System.Drawing.Size(90, 20);
			this.lblMaterial.TabIndex = 2;
			this.lblMaterial.Text = "Material:";
			this.lblMaterial.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbMaterial
			// 
			this.cbMaterial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
			this.cbMaterial.FormattingEnabled = true;
			this.cbMaterial.Location = new System.Drawing.Point(110, 47);
			this.cbMaterial.Name = "cbMaterial";
			this.cbMaterial.Size = new System.Drawing.Size(230, 21);
			this.cbMaterial.TabIndex = 3;
			// 
			// lblCondition
			// 
			this.lblCondition.Location = new System.Drawing.Point(10, 79);
			this.lblCondition.Name = "lblCondition";
			this.lblCondition.Size = new System.Drawing.Size(90, 20);
			this.lblCondition.TabIndex = 4;
			this.lblCondition.Text = "Condition:";
			this.lblCondition.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbCondition
			// 
			this.cbCondition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
			this.cbCondition.FormattingEnabled = true;
			this.cbCondition.Location = new System.Drawing.Point(110, 79);
			this.cbCondition.Name = "cbCondition";
			this.cbCondition.Size = new System.Drawing.Size(230, 21);
			this.cbCondition.TabIndex = 5;
			// 
			// grpStyle
			// 
			this.grpStyle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpStyle.Controls.Add(this.lblStyleCategory);
			this.grpStyle.Controls.Add(this.cbStyleCategory);
			this.grpStyle.Controls.Add(this.lblStyle);
			this.grpStyle.Controls.Add(this.cbStyle);
			this.grpStyle.Location = new System.Drawing.Point(3, 3);
			this.grpStyle.Name = "grpStyle";
			this.grpStyle.Size = new System.Drawing.Size(361, 100);
			this.grpStyle.TabIndex = 1;
			this.grpStyle.TabStop = false;
			this.grpStyle.Text = "Style Configuration";
			// 
			// lblStyleCategory
			// 
			this.lblStyleCategory.Location = new System.Drawing.Point(10, 25);
			this.lblStyleCategory.Name = "lblStyleCategory";
			this.lblStyleCategory.Size = new System.Drawing.Size(100, 20);
			this.lblStyleCategory.TabIndex = 0;
			this.lblStyleCategory.Text = "Style Category:";
			this.lblStyleCategory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbStyleCategory
			// 
			this.cbStyleCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbStyleCategory.FormattingEnabled = true;
			this.cbStyleCategory.Location = new System.Drawing.Point(120, 25);
			this.cbStyleCategory.Name = "cbStyleCategory";
			this.cbStyleCategory.Size = new System.Drawing.Size(230, 21);
			this.cbStyleCategory.TabIndex = 1;
			this.cbStyleCategory.SelectedIndexChanged += new System.EventHandler(this.CbStyleCategory_SelectedIndexChanged);
			// 
			// lblStyle
			// 
			this.lblStyle.Location = new System.Drawing.Point(10, 60);
			this.lblStyle.Name = "lblStyle";
			this.lblStyle.Size = new System.Drawing.Size(100, 20);
			this.lblStyle.TabIndex = 2;
			this.lblStyle.Text = "Sub-Style:";
			this.lblStyle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbStyle
			// 
			this.cbStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
			this.cbStyle.FormattingEnabled = true;
			this.cbStyle.Location = new System.Drawing.Point(120, 60);
			this.cbStyle.Name = "cbStyle";
			this.cbStyle.Size = new System.Drawing.Size(230, 21);
			this.cbStyle.TabIndex = 3;
			this.cbStyle.SelectedIndexChanged += new System.EventHandler(this.CbStyle_SelectedIndexChanged);
			// 
			// grpPrompt
			// 
			this.grpPrompt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpPrompt.Controls.Add(this.lblPromptHelp);
			this.grpPrompt.Controls.Add(this.txtPrompt);
			this.grpPrompt.Location = new System.Drawing.Point(3, 355);
			this.grpPrompt.Name = "grpPrompt";
			this.grpPrompt.Size = new System.Drawing.Size(361, 210);
			this.grpPrompt.TabIndex = 2;
			this.grpPrompt.TabStop = false;
			this.grpPrompt.Text = "Additional Description / Prompt";
			// 
			// lblPromptHelp
			// 
			this.lblPromptHelp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblPromptHelp.ForeColor = System.Drawing.Color.DimGray;
			this.lblPromptHelp.Location = new System.Drawing.Point(10, 20);
			this.lblPromptHelp.Name = "lblPromptHelp";
			this.lblPromptHelp.Size = new System.Drawing.Size(341, 35);
			this.lblPromptHelp.TabIndex = 0;
			this.lblPromptHelp.Text = "Add specific details or background notes to help describe the asset you want to generate.";
			// 
			// txtPrompt
			// 
			this.txtPrompt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtPrompt.Location = new System.Drawing.Point(10, 60);
			this.txtPrompt.Multiline = true;
			this.txtPrompt.Name = "txtPrompt";
			this.txtPrompt.Size = new System.Drawing.Size(341, 100);
			this.txtPrompt.TabIndex = 1;
			this.txtPrompt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtPrompt_KeyDown);
			// 
			// pnlPreview
			// 
			this.pnlPreview.BackColor = System.Drawing.Color.DarkGray;
			this.pnlPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlPreview.Controls.Add(this.lblPreviewTitle);
			this.pnlPreview.Controls.Add(this.pbPreview);
			this.pnlPreview.Controls.Add(this.prgLoading);
			this.pnlPreview.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlPreview.Location = new System.Drawing.Point(383, 3);
			this.pnlPreview.Name = "pnlPreview";
			this.pnlPreview.Size = new System.Drawing.Size(634, 569);
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
			this.pbPreview.Size = new System.Drawing.Size(612, 480);
			this.pbPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pbPreview.TabIndex = 1;
			this.pbPreview.TabStop = false;
			// 
			// prgLoading
			// 
			this.prgLoading.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.prgLoading.Location = new System.Drawing.Point(10, 530);
			this.prgLoading.Name = "prgLoading";
			this.prgLoading.Size = new System.Drawing.Size(612, 25);
			this.prgLoading.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			this.prgLoading.TabIndex = 2;
			this.prgLoading.Visible = false;
			// 
			// AiAssetCreatorDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1020, 780);
			this.Controls.Add(this.tlpMain);
			this.Controls.Add(this.pnlBottom);
			this.Controls.Add(this.tlpConfig);
			this.Controls.Add(this.pnlHeader);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(850, 650);
			this.Name = "AiAssetCreatorDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "AI Asset Creator";
			this.pnlHeader.ResumeLayout(false);
			this.pnlHeader.PerformLayout();
			this.tlpConfig.ResumeLayout(false);
			this.pnlBottom.ResumeLayout(false);
			this.tlpMain.ResumeLayout(false);
			this.pnlLeft.ResumeLayout(false);
			this.tabControl.ResumeLayout(false);
			this.tabCharacter.ResumeLayout(false);
			this.tabLocation.ResumeLayout(false);
			this.tabProp.ResumeLayout(false);
			this.grpStyle.ResumeLayout(false);
			this.grpPrompt.ResumeLayout(false);
			this.grpPrompt.PerformLayout();
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
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabCharacter;
		private System.Windows.Forms.Label lblSpecies;
		private System.Windows.Forms.ComboBox cbSpecies;
		private System.Windows.Forms.Label lblGender;
		private System.Windows.Forms.ComboBox cbGender;
		private System.Windows.Forms.Label lblPersonality;
		private System.Windows.Forms.ComboBox cbPersonality;
		private System.Windows.Forms.Label lblFashion;
		private System.Windows.Forms.ComboBox cbFashion;
		private System.Windows.Forms.Label lblAccessory;
		private System.Windows.Forms.ComboBox cbAccessory;
		private System.Windows.Forms.Label lblPalette;
		private System.Windows.Forms.ComboBox cbPalette;
		private System.Windows.Forms.Label lblPose;
		private System.Windows.Forms.ComboBox cbPose;
		private System.Windows.Forms.CheckBox chkMultiView;
		private System.Windows.Forms.TabPage tabLocation;
		private System.Windows.Forms.Label lblLocationType;
		private System.Windows.Forms.ComboBox cbLocationType;
		private System.Windows.Forms.Label lblStructure;
		private System.Windows.Forms.ComboBox cbStructure;
		private System.Windows.Forms.Label lblVisualState;
		private System.Windows.Forms.ComboBox cbVisualState;
		private System.Windows.Forms.Label lblAtmosphere;
		private System.Windows.Forms.ComboBox cbAtmosphere;
		private System.Windows.Forms.TabPage tabProp;
		private System.Windows.Forms.Label lblPropName;
		private System.Windows.Forms.TextBox txtPropName;
		private System.Windows.Forms.Label lblMaterial;
		private System.Windows.Forms.ComboBox cbMaterial;
		private System.Windows.Forms.Label lblCondition;
		private System.Windows.Forms.ComboBox cbCondition;
		private System.Windows.Forms.GroupBox grpStyle;
		private System.Windows.Forms.Label lblStyleCategory;
		private System.Windows.Forms.ComboBox cbStyleCategory;
		private System.Windows.Forms.Label lblStyle;
		private System.Windows.Forms.ComboBox cbStyle;
		private System.Windows.Forms.GroupBox grpPrompt;
		private System.Windows.Forms.Label lblPromptHelp;
		private System.Windows.Forms.TextBox txtPrompt;
		private System.Windows.Forms.Button btnGenerate;
		private System.Windows.Forms.Button btnLoad;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Panel pnlPreview;
		private System.Windows.Forms.Label lblPreviewTitle;
		private System.Windows.Forms.PictureBox pbPreview;
		private System.Windows.Forms.ProgressBar prgLoading;
	}
}
