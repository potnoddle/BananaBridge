using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using BananaBridge.UI.Models;

namespace BananaBridge.UI
{
	public partial class AiAssetCreatorDialog : Form
	{
		private string _apiKey = string.Empty;
		private Bitmap _generatedResult = null;
		private bool _isGenerating = false;
		private Dictionary<string, List<StyleItem>> _comicStyles = new Dictionary<string, List<StyleItem>>();

		public string ApiKey => _apiKey;
		public string Model => cbModel.SelectedItem?.ToString() ?? "";
		public string Prompt => SynthesizePrompt();
		public string UserPrompt => txtPrompt.Text.Trim();
		public Bitmap GeneratedResult => _generatedResult;
		public bool ScaleToFit => chkScaleToFit.Checked;
		public bool CopyToClipboard => chkCopyToClipboard.Checked;
		public bool GenerateMultiView => chkMultiView.Checked;

		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public Bitmap SourceCrop { get; set; }

		// Save/Restore Token Properties
		public string SelectedTab => tabControl.SelectedTab.Text;
		public string StyleCategory => cbStyleCategory.Text.Trim();
		public string StyleName => (cbStyle.SelectedItem as StyleItem)?.Name ?? cbStyle.Text.Trim();
		public string SpeciesName => cbSpecies.Text.Trim();
		public string GenderName => cbGender.Text.Trim();
		public string PersonalityName => cbPersonality.Text.Trim();
		public string FashionName => cbFashion.Text.Trim();
		public string AccessoryName => cbAccessory.Text.Trim();
		public string PaletteName => cbPalette.Text.Trim();
		public string PoseName => cbPose.Text.Trim();
		public string LocationTypeName => cbLocationType.Text.Trim();
		public string StructureName => cbStructure.Text.Trim();
		public string VisualStateName => cbVisualState.Text.Trim();
		public string AtmosphereName => cbAtmosphere.Text.Trim();
		public string PropTypeName => txtPropName.Text.Trim();
		public string MaterialName => cbMaterial.Text.Trim();
		public string ConditionName => cbCondition.Text.Trim();

		public AiAssetCreatorDialog(
			string initialApiKey,
			string initialModel,
			Bitmap initialResult,
			string initialPrompt,
			string initialTab,
			string initialStyleCategory,
			string initialStyle,
			string initialSpecies,
			string initialGender,
			string initialPersonality,
			string initialFashion,
			string initialAccessory,
			string initialPalette,
			string initialPose,
			string initialLocationType,
			string initialStructure,
			string initialVisualState,
			string initialAtmosphere,
			string initialPropType,
			string initialMaterial,
			string initialCondition,
			bool initialScaleToFit,
			bool initialCopyToClipboard,
			bool initialGenerateMultiView)
		{
			InitializeComponent();

			_apiKey = initialApiKey;
			_generatedResult = initialResult;

			txtPrompt.Text = initialPrompt;
			chkScaleToFit.Checked = initialScaleToFit;
			chkCopyToClipboard.Checked = initialCopyToClipboard;
			chkMultiView.Checked = initialGenerateMultiView;

			LoadModels();

			if (!string.IsNullOrEmpty(initialModel))
			{
				string normalizedModel = initialModel.StartsWith("models/") ? initialModel.Substring(7) : initialModel;
				int index = cbModel.FindStringExact(normalizedModel);
				if (index != -1)
				{
					cbModel.SelectedIndex = index;
				}
			}

			// Load option data from embedded JSON resources
			LoadJsonResources();

			// Restore previously selected tab and values if possible
			RestoreSelectedValues(
				initialTab,
				initialStyleCategory,
				initialStyle,
				initialSpecies,
				initialGender,
				initialPersonality,
				initialFashion,
				initialAccessory,
				initialPalette,
				initialPose,
				initialLocationType,
				initialStructure,
				initialVisualState,
				initialAtmosphere,
				initialPropType,
				initialMaterial,
				initialCondition
			);

			if (_generatedResult != null)
			{
				pbPreview.Image = new Bitmap(_generatedResult);
				btnOK.Enabled = true;
			}
			else
			{
				btnOK.Enabled = false;
			}

			SetupPreviewBackground();
		}

		private void SetupPreviewBackground()
		{
			Bitmap bmp = new Bitmap(16, 16);
			using (Graphics g = Graphics.FromImage(bmp))
			{
				g.Clear(Color.White);
				using (Brush brush = new SolidBrush(Color.FromArgb(240, 240, 240)))
				{
					g.FillRectangle(brush, 0, 0, 8, 8);
					g.FillRectangle(brush, 8, 8, 8, 8);
				}
			}
			pbPreview.BackgroundImage = bmp;
			pbPreview.BackgroundImageLayout = ImageLayout.Tile;
		}

		private void LoadModels()
		{
			cbModel.Items.Clear();
			string folder = Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
				"BananaBridge"
			);
			string cachePath = Path.Combine(folder, "models_cache.json");

			List<string> models = null;
			if (File.Exists(cachePath))
			{
				try
				{
					string json = File.ReadAllText(cachePath);
					models = JsonSerializer.Deserialize<List<string>>(json);
				}
				catch { }
			}

			if (models == null || models.Count == 0)
			{
				try
				{
					using (Stream stream = ConfigService.GetConfigStream("default_models.json"))
					{
						if (stream != null)
						{
							using (StreamReader reader = new StreamReader(stream))
							{
								string json = reader.ReadToEnd();
								models = JsonSerializer.Deserialize<List<string>>(json);
							}
						}
					}
				}
				catch { }
			}

			if (models == null || models.Count == 0)
			{
				models = new List<string> { "gemini-2.5-flash-image" };
			}

			foreach (var model in models)
			{
				cbModel.Items.Add(model);
			}

			if (cbModel.Items.Count > 0)
			{
				cbModel.SelectedIndex = 0;
			}
		}

		private void LoadJsonResources()
		{
			var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

			// 1. Load styles
			using (var stream = ConfigService.GetConfigStream("comicbook-styles.json"))
			{
				if (stream != null)
				{
					_comicStyles = JsonSerializer.Deserialize<Dictionary<string, List<StyleItem>>>(stream, jsonOptions);
					cbStyleCategory.Items.Clear();
					cbStyleCategory.Items.Add("(None)");
					foreach (var cat in _comicStyles.Keys)
					{
						cbStyleCategory.Items.Add(cat);
					}
					if (cbStyleCategory.Items.Count > 0)
					{
						cbStyleCategory.SelectedIndex = 0;
					}
				}
			}

			// 2. Load locations-types
			using (var stream = ConfigService.GetConfigStream("locations-types.json"))
			{
				if (stream != null)
				{
					var data = JsonSerializer.Deserialize<LocationTypesData>(stream, jsonOptions);
					foreach (var prop in data.LocationProperties)
					{
						var cb = GetLocationComboBoxForProperty(prop.PropertyName);
						if (cb != null)
						{
							cb.Items.Add("(None)");
							foreach (var ex in prop.Examples)
							{
								cb.Items.Add(ex);
							}
							cb.SelectedIndex = 0;
						}
					}
				}
			}

			// 3. Load character personality
			using (var stream = ConfigService.GetConfigStream("character-personality-data.json"))
			{
				if (stream != null)
				{
					var data = JsonSerializer.Deserialize<CharacterPersonalityData>(stream, jsonOptions);
					cbPersonality.Items.Add("(None)");
					foreach (var t in data.Traits) cbPersonality.Items.Add(t);
					cbPersonality.SelectedIndex = 0;
				}
			}

			// 4. Load character visual data
			using (var stream = ConfigService.GetConfigStream("character-visual-data.json"))
			{
				if (stream != null)
				{
					var data = JsonSerializer.Deserialize<CharacterVisualData>(stream, jsonOptions);

					cbFashion.Items.Add("(None)");
					foreach (var f in data.Fashion) cbFashion.Items.Add(f);
					cbFashion.SelectedIndex = 0;

					cbAccessory.Items.Add("(None)");
					foreach (var a in data.Accessories) cbAccessory.Items.Add(a);
					cbAccessory.SelectedIndex = 0;

					cbPalette.Items.Add("(None)");
					foreach (var p in data.ColorPalettes) cbPalette.Items.Add(p);
					cbPalette.SelectedIndex = 0;

					cbPose.Items.Add("(None)");
					foreach (var po in data.IconicPoses) cbPose.Items.Add(po);
					cbPose.SelectedIndex = 0;
				}
			}

			// 5. Load character species
			bool loadedSpecies = false;
			using (var stream = ConfigService.GetConfigStream("character-species-data.json"))
			{
				if (stream != null)
				{
					try
					{
						var data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(stream, jsonOptions);
						cbSpecies.Items.Clear();
						if (data != null)
						{
							foreach (var key in data.Keys)
							{
								string displayKey = char.ToUpper(key[0]) + key.Substring(1);
								cbSpecies.Items.Add(displayKey);
							}
							if (cbSpecies.Items.Count > 0)
							{
								cbSpecies.SelectedIndex = 0;
								loadedSpecies = true;
							}
						}
					}
					catch
					{
						// Fallback to hardcoded list below
					}
				}
			}

			if (!loadedSpecies)
			{
				cbSpecies.Items.Clear();
				cbSpecies.Items.Add("Human");
				cbSpecies.Items.Add("Robot");
				cbSpecies.Items.Add("Alien");
				cbSpecies.Items.Add("Animal");
				cbSpecies.SelectedIndex = 0;
			}

			cbGender.Items.Add("Male");
			cbGender.Items.Add("Female");
			cbGender.Items.Add("Androgynous");
			cbGender.SelectedIndex = 0;

			// Prop basic fields
			cbMaterial.Items.Add("(None)");
			cbMaterial.Items.Add("Rusted Iron");
			cbMaterial.Items.Add("Polished Obsidian");
			cbMaterial.Items.Add("Ancient Oak Wood");
			cbMaterial.Items.Add("Glowing Energy");
			cbMaterial.Items.Add("Solid Gold");
			cbMaterial.Items.Add("Aged Bronze");
			cbMaterial.Items.Add("Polished Steel");
			cbMaterial.SelectedIndex = 0;

			cbCondition.Items.Add("(None)");
			cbCondition.Items.Add("Pristine");
			cbCondition.Items.Add("Worn/Cracked");
			cbCondition.Items.Add("Weathered/Dusty");
			cbCondition.Items.Add("Rusted/Corroded");
			cbCondition.Items.Add("Glowing/Humming");
			cbCondition.Items.Add("Ancient/Runic");
			cbCondition.SelectedIndex = 0;
		}

		private ComboBox GetLocationComboBoxForProperty(string propName)
		{
			if (propName.Contains("Category")) return cbLocationType;
			if (propName.Contains("Structure")) return cbStructure;
			if (propName.Contains("Light") || propName.Contains("Visual")) return cbVisualState;
			if (propName.Contains("Atmosphere")) return cbAtmosphere;
			return null;
		}

		private void RestoreSelectedValues(
			string initialTab,
			string initialStyleCategory,
			string initialStyle,
			string initialSpecies,
			string initialGender,
			string initialPersonality,
			string initialFashion,
			string initialAccessory,
			string initialPalette,
			string initialPose,
			string initialLocationType,
			string initialStructure,
			string initialVisualState,
			string initialAtmosphere,
			string initialPropType,
			string initialMaterial,
			string initialCondition)
		{
			if (!string.IsNullOrEmpty(initialTab))
			{
				foreach (TabPage tab in tabControl.TabPages)
				{
					if (tab.Text == initialTab)
					{
						tabControl.SelectedTab = tab;
						break;
					}
				}
			}

			SetComboBoxValue(cbStyleCategory, initialStyleCategory);
			SetComboBoxValue(cbSpecies, initialSpecies);
			SetComboBoxValue(cbGender, initialGender);
			SetComboBoxValue(cbPersonality, initialPersonality);
			SetComboBoxValue(cbFashion, initialFashion);
			SetComboBoxValue(cbAccessory, initialAccessory);
			SetComboBoxValue(cbPalette, initialPalette);
			SetComboBoxValue(cbPose, initialPose);
			SetComboBoxValue(cbLocationType, initialLocationType);
			SetComboBoxValue(cbStructure, initialStructure);
			SetComboBoxValue(cbVisualState, initialVisualState);
			SetComboBoxValue(cbAtmosphere, initialAtmosphere);
			SetComboBoxValue(cbMaterial, initialMaterial);
			SetComboBoxValue(cbCondition, initialCondition);

			if (!string.IsNullOrEmpty(initialPropType))
			{
				txtPropName.Text = initialPropType;
			}

			// Restore specific sub style item
			if (!string.IsNullOrEmpty(initialStyle))
			{
				for (int i = 0; i < cbStyle.Items.Count; i++)
				{
					if (cbStyle.Items[i] is StyleItem item && item.Name == initialStyle)
					{
						cbStyle.SelectedIndex = i;
						break;
					}
				}
			}
		}

		private void SetComboBoxValue(ComboBox cb, string val)
		{
			if (string.IsNullOrEmpty(val)) return;
			int idx = cb.FindStringExact(val);
			if (idx != -1)
			{
				cb.SelectedIndex = idx;
			}
			else
			{
				cb.Text = val;
			}
		}

		private void CbStyleCategory_SelectedIndexChanged(object sender, EventArgs e)
		{
			string selectedCat = cbStyleCategory.SelectedItem?.ToString();
			if (selectedCat == "(None)")
			{
				cbStyle.Items.Clear();
				cbStyle.Items.Add("(None)");
				cbStyle.SelectedIndex = 0;
				return;
			}

			if (string.IsNullOrEmpty(selectedCat) || !_comicStyles.ContainsKey(selectedCat))
			{
				return;
			}

			cbStyle.Items.Clear();
			cbStyle.Items.Add("(None)");
			foreach (var style in _comicStyles[selectedCat])
			{
				cbStyle.Items.Add(style);
			}

			if (cbStyle.Items.Count > 0)
			{
				cbStyle.SelectedIndex = 0;
			}
		}

		private void CbStyle_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cbStyle.SelectedItem is StyleItem selectedStyle)
			{
				UpdateInfluencedOptions(selectedStyle);
			}
		}

		private void UpdateInfluencedOptions(StyleItem style)
		{
			if (style == null) return;

			// 1. Update Species
			if (style.Species != null && style.Species.Count > 0)
			{
				cbSpecies.Items.Clear();
				foreach (var sp in style.Species)
				{
					cbSpecies.Items.Add(char.ToUpper(sp[0]) + sp.Substring(1));
				}
				cbSpecies.SelectedIndex = 0;
			}
			else
			{
				ReloadDefaultSpecies();
			}

			// 2. Update Gender
			if (style.Genders != null && style.Genders.Count > 0)
			{
				cbGender.Items.Clear();
				foreach (var g in style.Genders)
				{
					cbGender.Items.Add(char.ToUpper(g[0]) + g.Substring(1));
				}
				cbGender.SelectedIndex = 0;
			}
			else
			{
				cbGender.Items.Clear();
				cbGender.Items.Add("Male");
				cbGender.Items.Add("Female");
				cbGender.Items.Add("Androgynous");
				cbGender.SelectedIndex = 0;
			}

			// 3. Update Location attributes
			if (style.LocationAttributes != null)
			{
				if (style.LocationAttributes.Types != null && style.LocationAttributes.Types.Count > 0)
				{
					cbLocationType.Items.Clear();
					cbLocationType.Items.Add("(None)");
					foreach (var t in style.LocationAttributes.Types) cbLocationType.Items.Add(t);
					cbLocationType.SelectedIndex = 0;
				}
				if (style.LocationAttributes.Structures != null && style.LocationAttributes.Structures.Count > 0)
				{
					cbStructure.Items.Clear();
					cbStructure.Items.Add("(None)");
					foreach (var s in style.LocationAttributes.Structures) cbStructure.Items.Add(s);
					cbStructure.SelectedIndex = 0;
				}
				if (style.LocationAttributes.Atmospheres != null && style.LocationAttributes.Atmospheres.Count > 0)
				{
					cbAtmosphere.Items.Clear();
					cbAtmosphere.Items.Add("(None)");
					foreach (var a in style.LocationAttributes.Atmospheres) cbAtmosphere.Items.Add(a);
					cbAtmosphere.SelectedIndex = 0;
				}
			}
			else
			{
				ReloadDefaultLocations();
			}

			// 4. Update Color Palette
			if (!string.IsNullOrEmpty(style.ColorPalette))
			{
				string pal = style.ColorPalette;
				if (!cbPalette.Items.Contains(pal))
				{
					cbPalette.Items.Add(pal);
				}
				cbPalette.Text = pal;
			}

			// 5. Update Lighting Style
			if (!string.IsNullOrEmpty(style.LightingStyle))
			{
				string light = style.LightingStyle;
				if (!cbVisualState.Items.Contains(light))
				{
					cbVisualState.Items.Add(light);
				}
				cbVisualState.Text = light;
			}
		}

		private void ReloadDefaultSpecies()
		{
			var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
			using (var stream = ConfigService.GetConfigStream("character-species-data.json"))
			{
				if (stream != null)
				{
					try
					{
						var data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(stream, jsonOptions);
						cbSpecies.Items.Clear();
						if (data != null)
						{
							foreach (var key in data.Keys)
							{
								string displayKey = char.ToUpper(key[0]) + key.Substring(1);
								cbSpecies.Items.Add(displayKey);
							}
							if (cbSpecies.Items.Count > 0)
							{
								cbSpecies.SelectedIndex = 0;
							}
						}
					}
					catch { }
				}
			}
		}

		private void ReloadDefaultLocations()
		{
			var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
			using (var stream = ConfigService.GetConfigStream("locations-types.json"))
			{
				if (stream != null)
				{
					try
					{
						var data = JsonSerializer.Deserialize<LocationTypesData>(stream, jsonOptions);
						cbLocationType.Items.Clear();
						cbStructure.Items.Clear();
						cbVisualState.Items.Clear();
						cbAtmosphere.Items.Clear();

						foreach (var prop in data.LocationProperties)
						{
							var cb = GetLocationComboBoxForProperty(prop.PropertyName);
							if (cb != null)
							{
								cb.Items.Add("(None)");
								foreach (var ex in prop.Examples)
								{
									cb.Items.Add(ex);
								}
								cb.SelectedIndex = 0;
							}
						}
					}
					catch { }
				}
			}
		}

		private void TxtPrompt_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter && !e.Shift)
			{
				e.SuppressKeyPress = true;
				BtnGenerate_Click(this, EventArgs.Empty);
			}
		}

		private void BtnSettings_Click(object sender, EventArgs e)
		{
			using (var settingsDlg = new SettingsDialog(_apiKey))
			{
				if (settingsDlg.ShowDialog(this) == DialogResult.OK)
				{
					_apiKey = settingsDlg.ApiKey;
					string selectedModel = cbModel.SelectedItem?.ToString();
					LoadModels();
					if (!string.IsNullOrEmpty(selectedModel))
					{
						int index = cbModel.FindStringExact(selectedModel);
						if (index != -1)
						{
							cbModel.SelectedIndex = index;
						}
					}
				}
			}
		}

		private string SynthesizePrompt()
		{
			string stylePrompt = string.Empty;
			if (cbStyle.SelectedItem is StyleItem selectedStyle)
			{
				stylePrompt = selectedStyle.ImagePrompt;
			}
			else
			{
				string text = cbStyle.Text.Trim();
				if (!string.IsNullOrEmpty(text) && text != "(None)")
				{
					stylePrompt = text;
				}
			}

			string selectedTab = tabControl.SelectedTab.Text;
			string userPrompt = txtPrompt.Text.Trim();

			if (selectedTab == "Character")
			{
				string species = cbSpecies.Text.Trim();
				string gender = cbGender.Text.Trim();
				string personality = cbPersonality.Text.Trim();
				string fashion = cbFashion.Text.Trim();
				string accessory = cbAccessory.Text.Trim();
				string palette = cbPalette.Text.Trim();
				string pose = cbPose.Text.Trim();

				string charDesc = $"A high-quality character asset. Subject: {gender} {species}";
				if (!string.IsNullOrEmpty(userPrompt))
				{
					charDesc += $" named/described as: {userPrompt}";
				}
				charDesc += ".";

				if (!string.IsNullOrEmpty(personality) && personality != "(None)")
				{
					charDesc += $" Personality traits: {personality}.";
				}
				if (!string.IsNullOrEmpty(fashion) && fashion != "(None)")
				{
					charDesc += $" Clothing fashion: {fashion}.";
				}
				if (!string.IsNullOrEmpty(accessory) && accessory != "(None)")
				{
					charDesc += $" Accessories: {accessory}.";
				}
				if (!string.IsNullOrEmpty(palette) && palette != "(None)")
				{
					charDesc += $" Color palette: {palette}.";
				}
				if (!string.IsNullOrEmpty(pose) && pose != "(None)")
				{
					charDesc += $" Iconic pose: {pose}.";
				}
				if (chkMultiView.Checked)
				{
					charDesc += ", presented as a character concept sheet showing 5 views: main, front, back, left, and right";
				}
				if (!string.IsNullOrEmpty(stylePrompt))
				{
					charDesc += $" Rendered in style: {stylePrompt}.";
				}
				return charDesc;
			}
			else if (selectedTab == "Location")
			{
				string locType = cbLocationType.Text.Trim();
				string structure = cbStructure.Text.Trim();
				string visualState = cbVisualState.Text.Trim();
				string atmosphere = cbAtmosphere.Text.Trim();

				string locDesc = "An environment concept art location scene.";
				if (!string.IsNullOrEmpty(locType) && locType != "(None)")
				{
					locDesc += $" Type: {locType}.";
				}
				if (!string.IsNullOrEmpty(userPrompt))
				{
					locDesc += $" Details: {userPrompt}.";
				}
				if (!string.IsNullOrEmpty(structure) && structure != "(None)")
				{
					locDesc += $" Structure/Scale: {structure}.";
				}
				if (!string.IsNullOrEmpty(visualState) && visualState != "(None)")
				{
					locDesc += $" Visual Light & Condition: {visualState}.";
				}
				if (!string.IsNullOrEmpty(atmosphere) && atmosphere != "(None)")
				{
					locDesc += $" Atmosphere/Pace: {atmosphere}.";
				}
				if (!string.IsNullOrEmpty(stylePrompt))
				{
					locDesc += $" Rendered in style: {stylePrompt}.";
				}
				return locDesc;
			}
			else // Prop
			{
				string propName = txtPropName.Text.Trim();
				string material = cbMaterial.SelectedItem?.ToString() ?? "";
				string condition = cbCondition.SelectedItem?.ToString() ?? "";

				string propDesc = $"A prop object asset. Item Name: {propName}.";
				if (!string.IsNullOrEmpty(userPrompt))
				{
					propDesc += $" Additional Details: {userPrompt}.";
				}
				if (!string.IsNullOrEmpty(material) && material != "(None)")
				{
					propDesc += $" Material composition: {material}.";
				}
				if (!string.IsNullOrEmpty(condition) && condition != "(None)")
				{
					propDesc += $" Condition/State: {condition}.";
				}
				if (!string.IsNullOrEmpty(stylePrompt))
				{
					propDesc += $" Rendered in style: {stylePrompt}.";
				}
				return propDesc;
			}
		}

		private async void BtnGenerate_Click(object sender, EventArgs e)
		{
			if (_isGenerating) return;

			string apiKey = _apiKey;
			if (string.IsNullOrEmpty(apiKey))
			{
				MessageBox.Show("Please enter an API Key in Settings.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			_isGenerating = true;
			btnGenerate.Enabled = false;
			prgLoading.Visible = true;

			try
			{
				string model = cbModel.SelectedItem?.ToString() ?? "";
				string provider = SecureStore.LoadProvider();

				// Build prompt
				string synthesizedPrompt = SynthesizePrompt();

				// Request model to return only base64 PNG
				string finalPrompt = $"{synthesizedPrompt} Return ONLY the base64-encoded PNG image data. Do not include markdown wraps or other text.";
				string aspectRatio = this.SourceCrop != null ? GeminiClient.GetClosestAspectRatio(this.SourceCrop.Width, this.SourceCrop.Height) : null;

				Bitmap generatedBmp = await Task.Run(async () =>
				{
					IAiClient client = AiClientFactory.CreateClient(provider);
					return await client.SendEditRequestAsync(
						apiKey,
						model,
						new List<ChatTurn>(),
						null, // No source image for creator (text-to-image)
						null, // No mask
						finalPrompt,
						aspectRatio
					);
				});

				if (generatedBmp != null)
				{
					if (_generatedResult != null)
					{
						_generatedResult.Dispose();
					}
					_generatedResult = generatedBmp;

					if (pbPreview.Image != null)
					{
						pbPreview.Image.Dispose();
					}
					pbPreview.Image = new Bitmap(_generatedResult);
					btnOK.Enabled = true;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Request failed: {ex.Message}", "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				_isGenerating = false;
				btnGenerate.Enabled = true;
				prgLoading.Visible = false;
			}
		}

		private void BtnSave_Click(object sender, EventArgs e)
		{
			using (var sfd = new SaveFileDialog())
			{
				sfd.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
				sfd.Title = "Save Preset";
				if (sfd.ShowDialog(this) == DialogResult.OK)
				{
					try
					{
						var preset = new CreatorPreset
						{
							Tab = SelectedTab,
							Model = Model,
							Prompt = UserPrompt,
							ScaleToFit = ScaleToFit,
							CopyToClipboard = CopyToClipboard,
							StyleCategory = StyleCategory,
							Style = StyleName,
							Species = SpeciesName,
							Gender = GenderName,
							Personality = PersonalityName,
							Fashion = FashionName,
							Accessory = AccessoryName,
							Palette = PaletteName,
							Pose = PoseName,
							LocationType = LocationTypeName,
							Structure = StructureName,
							VisualState = VisualStateName,
							Atmosphere = AtmosphereName,
							PropName = PropTypeName,
							Material = MaterialName,
							Condition = ConditionName
						};

						string json = JsonSerializer.Serialize(preset, new JsonSerializerOptions { WriteIndented = true });
						File.WriteAllText(sfd.FileName, json);
					}
					catch (Exception ex)
					{
						MessageBox.Show($"Failed to save preset: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}
		}

		private void BtnLoad_Click(object sender, EventArgs e)
		{
			using (var ofd = new OpenFileDialog())
			{
				ofd.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
				ofd.Title = "Load Preset";
				if (ofd.ShowDialog(this) == DialogResult.OK)
				{
					try
					{
						string json = File.ReadAllText(ofd.FileName);
						var preset = JsonSerializer.Deserialize<CreatorPreset>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
						if (preset != null)
						{
							RestoreSelectedValues(
								preset.Tab,
								preset.StyleCategory,
								preset.Style,
								preset.Species,
								preset.Gender,
								preset.Personality,
								preset.Fashion,
								preset.Accessory,
								preset.Palette,
								preset.Pose,
								preset.LocationType,
								preset.Structure,
								preset.VisualState,
								preset.Atmosphere,
								preset.PropName,
								preset.Material,
								preset.Condition
							);

							if (!string.IsNullOrEmpty(preset.Model))
							{
								int idx = cbModel.FindStringExact(preset.Model);
								if (idx != -1) cbModel.SelectedIndex = idx;
							}
							if (preset.Prompt != null) txtPrompt.Text = preset.Prompt;
							chkScaleToFit.Checked = preset.ScaleToFit;
							chkCopyToClipboard.Checked = preset.CopyToClipboard;
						}
					}
					catch (Exception ex)
					{
						MessageBox.Show($"Failed to load preset: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}
		}

		private void BtnOK_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(_apiKey))
			{
				SecureStore.SaveApiKey(_apiKey);
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if (!this.DesignMode)
			{
				_apiKey = SecureStore.LoadApiKey();
			}
		}
	}
}
