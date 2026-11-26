using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using BananaBridge.UI.Models;

namespace BananaBridge.UI
{
	public partial class AiStyleDialog : Form
	{
		private static readonly Dictionary<string, string> AllCategoryDefinitions = new Dictionary<string, string>
		{
			{ "Digital", "styles-digital.json" },
			{ "Global & Indigenous", "styles-global.json" },
			{ "Other", "styles-other.json" },
			{ "Photography & Realism", "styles-photography.json" },
			{ "American & Western", "styles-american.json" },
			{ "Ancient & Classical", "styles-ancient.json" },
			{ "Asian & Eastern", "styles-asian.json" },
			{ "European & Continental", "styles-european.json" },
			{ "Historical & Retro", "styles-historical.json" },
			{ "Horror & Dark Fantasy", "styles-horror.json" },
			{ "Literary & Illustrated", "styles-literary.json" },
			{ "Science Fiction & Cyberpunk", "styles-science-fiction.json" }
		};

		private Dictionary<string, string> _categoryResources = new Dictionary<string, string>();

		private List<StyleItem> _currentStyles = new List<StyleItem>();
		private Bitmap _generatedResult = null;
		private string _selectionPixelHash = string.Empty;
		private string _apiKey = string.Empty;

		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public Bitmap SourceCrop { get; set; }

		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public Bitmap MaskCrop { get; set; }

		public string ApiKey => _apiKey;
		public string Model => cbModel.SelectedItem?.ToString() ?? "";
		public Bitmap GeneratedResult => _generatedResult;
		public bool CopyToClipboard => chkCopyToClipboard.Checked;
		public bool ScaleToFit => chkScaleToFit.Checked;
		public int StyleStrength => tbStrength.Value;

		public string SelectedCategoryId => cbCategory.SelectedItem?.ToString() ?? "";
		public string SelectedStyleId => (cbStyles.SelectedItem as StyleItem)?.Id ?? "";
		public string SelectedStyleName => (cbStyles.SelectedItem as StyleItem)?.Name ?? "";

		public AiStyleDialog(
			string initialApiKey,
			string initialModel,
			Bitmap sourceCrop,
			Bitmap maskCrop,
			string selectionPixelHash,
			Bitmap initialResult,
			string initialCategory,
			string initialStyleId,
			int initialStrength,
			bool initialScaleToFit)
		{
			InitializeComponent();

			_apiKey = initialApiKey;
			SourceCrop = sourceCrop;
			MaskCrop = maskCrop;
			_selectionPixelHash = selectionPixelHash;
			chkScaleToFit.Checked = initialScaleToFit;

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

			// Populate Categories from active configuration (override or default)
			cbCategory.Items.Clear();
			_categoryResources.Clear();
			foreach (var kvp in AllCategoryDefinitions)
			{
				if (ConfigService.ConfigExists(kvp.Value))
				{
					_categoryResources[kvp.Key] = kvp.Value;
					cbCategory.Items.Add(kvp.Key);
				}
			}

			if (cbCategory.Items.Count > 0)
			{
				if (!string.IsNullOrEmpty(initialCategory) && cbCategory.Items.Contains(initialCategory))
				{
					cbCategory.SelectedItem = initialCategory;
				}
				else
				{
					cbCategory.SelectedIndex = 0;
				}
			}

			// Set initial strength
			if (initialStrength >= 10 && initialStrength <= 100)
			{
				tbStrength.Value = initialStrength;
				lblStrengthValue.Text = $"{initialStrength}%";
			}

			if (initialResult != null)
			{
				_generatedResult = initialResult;
				pbPreview.Image = new Bitmap(_generatedResult);
			}
			else if (sourceCrop != null)
			{
				pbPreview.Image = new Bitmap(sourceCrop);
			}

			// Try to select initial style
			if (!string.IsNullOrEmpty(initialStyleId) && cbStyles.Items.Count > 0)
			{
				for (int i = 0; i < cbStyles.Items.Count; i++)
				{
					if (cbStyles.Items[i] is StyleItem item && item.Id == initialStyleId)
					{
						cbStyles.SelectedIndex = i;
						break;
					}
				}
			}

			// CRITICAL: Disable Commit Layer until something is generated
			btnOK.Enabled = (_generatedResult != null);

			// Wire up characteristic checkboxes to dynamically update the style description
			chkSubstrate.CheckedChanged += (s, ev) => UpdateStyleDescription();
			chkRendering.CheckedChanged += (s, ev) => UpdateStyleDescription();
			chkColoring.CheckedChanged += (s, ev) => UpdateStyleDescription();
			chkLineQuality.CheckedChanged += (s, ev) => UpdateStyleDescription();
			chkAnatomy.CheckedChanged += (s, ev) => UpdateStyleDescription();
			chkFrame.CheckedChanged += (s, ev) => UpdateStyleDescription();
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
				models = new List<string> { "gemini-2.5-flash" };
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

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if (!this.DesignMode)
			{
				_apiKey = SecureStore.LoadApiKey();
			}
		}

		private void CbCategory_SelectedIndexChanged(object sender, EventArgs e)
		{
			string selectedCat = cbCategory.SelectedItem?.ToString();
			if (string.IsNullOrEmpty(selectedCat) || !_categoryResources.TryGetValue(selectedCat, out string resourceName))
			{
				return;
			}

			try
			{
				using (Stream stream = ConfigService.GetConfigStream(resourceName))
				{
					if (stream != null)
					{
						using (StreamReader reader = new StreamReader(stream))
						{
							string json = reader.ReadToEnd();
							var root = JsonSerializer.Deserialize<StyleRoot>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
							_currentStyles = root?.Styles ?? new List<StyleItem>();
						}
					}
				}

				cbStyles.Items.Clear();
				foreach (var style in _currentStyles)
				{
					cbStyles.Items.Add(style);
				}

				if (cbStyles.Items.Count > 0)
				{
					cbStyles.SelectedIndex = 0;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Failed to load category styles: {ex.Message}", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		private void CbStyles_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateStyleDescription();
		}

		private void UpdateStyleDescription()
		{
			if (cbStyles.SelectedItem is StyleItem selectedStyle)
			{
				var lines = new List<string>();
				lines.Add(selectedStyle.Description);
				lines.Add("");
				lines.Add("Selected Characteristics:");

				if (chkSubstrate.Checked && selectedStyle.BaseStyle != null && !string.IsNullOrEmpty(selectedStyle.BaseStyle.Substrate))
				{
					lines.Add($"- Substrate: {selectedStyle.BaseStyle.Substrate}");
				}
				if (chkRendering.Checked && selectedStyle.BaseStyle != null && !string.IsNullOrEmpty(selectedStyle.BaseStyle.Rendering))
				{
					lines.Add($"- Rendering: {selectedStyle.BaseStyle.Rendering}");
				}
				if (chkColoring.Checked && selectedStyle.BaseStyle != null && !string.IsNullOrEmpty(selectedStyle.BaseStyle.Pigmentation))
				{
					lines.Add($"- Coloring/Pigmentation: {selectedStyle.BaseStyle.Pigmentation}");
				}
				if (chkLineQuality.Checked && selectedStyle.Characteristics != null && !string.IsNullOrEmpty(selectedStyle.Characteristics.LineQuality))
				{
					lines.Add($"- Line Quality: {selectedStyle.Characteristics.LineQuality}");
				}
				if (chkAnatomy.Checked && selectedStyle.Characteristics != null && !string.IsNullOrEmpty(selectedStyle.Characteristics.Anatomy))
				{
					lines.Add($"- Anatomy/Posing: {selectedStyle.Characteristics.Anatomy}");
				}
				if (chkFrame.Checked)
				{
					if (selectedStyle.FrameStyle != null && !string.IsNullOrEmpty(selectedStyle.FrameStyle.StyleType))
					{
						lines.Add($"- Frame/Border: Draw an ornate frame or border of style: {selectedStyle.FrameStyle.StyleType}");
					}
					else
					{
						lines.Add($"- Frame/Border: Draw an ornate frame or border around the image");
					}
				}
				else
				{
					lines.Add("- Frame/Border: Off");
				}

				txtStyleDesc.Text = string.Join("\r\n", lines);
			}
		}

		private void TbStrength_Scroll(object sender, EventArgs e)
		{
			lblStrengthValue.Text = $"{tbStrength.Value}%";
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

		private async void BtnGenerate_Click(object sender, EventArgs e)
		{
			string apiKey = _apiKey;
			if (string.IsNullOrEmpty(apiKey))
			{
				MessageBox.Show("Please enter an API Key in Settings.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			if (!(cbStyles.SelectedItem is StyleItem styleItem))
			{
				MessageBox.Show("Please select a style first.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			btnGenerate.Enabled = false;
			prgLoading.Visible = true;

			try
			{
				// Assemble prompt based on selected characteristics and strength
				var parts = new List<string>();
				if (chkSubstrate.Checked && styleItem.BaseStyle != null && !string.IsNullOrEmpty(styleItem.BaseStyle.Substrate))
					parts.Add($"Substrate: {styleItem.BaseStyle.Substrate}");
				if (chkRendering.Checked && styleItem.BaseStyle != null && !string.IsNullOrEmpty(styleItem.BaseStyle.Rendering))
					parts.Add($"Rendering: {styleItem.BaseStyle.Rendering}");
				if (chkColoring.Checked && styleItem.BaseStyle != null && !string.IsNullOrEmpty(styleItem.BaseStyle.Pigmentation))
					parts.Add($"Pigmentation/Coloring: {styleItem.BaseStyle.Pigmentation}");
				if (chkLineQuality.Checked && styleItem.Characteristics != null && !string.IsNullOrEmpty(styleItem.Characteristics.LineQuality))
					parts.Add($"Line Quality: {styleItem.Characteristics.LineQuality}");
				if (chkAnatomy.Checked && styleItem.Characteristics != null && !string.IsNullOrEmpty(styleItem.Characteristics.Anatomy))
					parts.Add($"Anatomy/Posing: {styleItem.Characteristics.Anatomy}");
				if (chkFrame.Checked)
				{
					if (styleItem.FrameStyle != null && !string.IsNullOrEmpty(styleItem.FrameStyle.StyleType))
					{
						parts.Add($"Frame/Border: Draw an ornate frame or border around the image of style: {styleItem.FrameStyle.StyleType}");
					}
				}
				else
				{
					parts.Add("Frame/Border: NO frames, borders, margins, or ornate borders. The composition must be full-bleed and edge-to-edge.");
				}

				string styleDirective = string.Join("\n", parts);
				int strength = tbStrength.Value;
				string strengthGuide = "";
				if (strength <= 30)
					strengthGuide = "Apply a very subtle hint of the following style. Preserve the original image colors and details as much as possible, blending only a touch of the new style attributes.";
				else if (strength <= 70)
					strengthGuide = "Apply a balanced blend of the following style characteristics, transforming the image while maintaining recognizable details from the original content.";
				else
					strengthGuide = "Completely transform the image into the specified style, rewriting substrate, linework, and pigmentation/colors entirely to conform to the style directives.";

				string finalPrompt = $"{strengthGuide}\n\nSTYLE DIRECTIVES:\n{styleDirective}";
				if (!string.IsNullOrEmpty(styleItem.ImagePrompt))
				{
					finalPrompt += $"\n\nOverall Style Guide: {styleItem.ImagePrompt}";
				}

				Bitmap sourceCrop = this.SourceCrop;
				Bitmap maskCrop = this.MaskCrop;
				string model = cbModel.SelectedItem?.ToString() ?? "";
				string aspectRatio = this.SourceCrop != null ? GeminiClient.GetClosestAspectRatio(this.SourceCrop.Width, this.SourceCrop.Height) : null;

				string provider = SecureStore.LoadProvider();
				Bitmap result = await Task.Run(async () =>
				{
					IAiClient client = AiClientFactory.CreateClient(provider);
					return await client.SendEditRequestAsync(
						apiKey,
						model,
						new List<ChatTurn>(), // No chat history for one-shot style apply
						sourceCrop,
						maskCrop,
						finalPrompt,
						aspectRatio
					);
				});

				if (result != null)
				{
					_generatedResult = result;
					pbPreview.Image = new Bitmap(result);
					btnOK.Enabled = true; // Enable Commit Layer now that we have a result
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Request failed: {ex.Message}", "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				btnGenerate.Enabled = true;
				prgLoading.Visible = false;
			}
		}

		private void BtnOK_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(_apiKey))
			{
				SecureStore.SaveApiKey(_apiKey);
			}
		}
	}

	public class StyleRoot
	{
		public List<StyleItem> Styles { get; set; }
	}
}

