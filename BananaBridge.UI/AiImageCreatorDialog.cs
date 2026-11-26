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
	public partial class AiImageCreatorDialog : Form
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

		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public Bitmap SourceCrop { get; set; }

		// Save/Restore Token Properties
		public string StyleCategory => cbStyleCategory.Text.Trim();
		public string StyleName => (cbStyle.SelectedItem as StyleItem)?.Name ?? cbStyle.Text.Trim();
		public string CompositionLayoutName => cbCompositionLayout.Text.Trim();

		public AiImageCreatorDialog(
			string initialApiKey,
			string initialModel,
			Bitmap initialResult,
			string initialPrompt,
			string initialStyleCategory,
			string initialStyle,
			bool initialScaleToFit,
			bool initialCopyToClipboard,
			string initialCompositionLayout = "")
		{
			InitializeComponent();

			_apiKey = initialApiKey;
			_generatedResult = initialResult;

			txtPrompt.Text = initialPrompt;
			chkScaleToFit.Checked = initialScaleToFit;
			chkCopyToClipboard.Checked = initialCopyToClipboard;

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

			// Restore previously selected values if possible
			RestoreSelectedValues(
				initialStyleCategory,
				initialStyle,
				initialCompositionLayout
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

			// Load styles
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

			// Load composition guides from layout.json
			using (var stream = ConfigService.GetConfigStream("layout.json"))
			{
				if (stream != null)
				{
					try
					{
						var data = JsonSerializer.Deserialize<CompositionalArmaturesData>(stream, jsonOptions);
						cbCompositionLayout.Items.Clear();
						cbCompositionLayout.Items.Add("(None)");
						if (data != null && data.CompositionalArmatures != null)
						{
							foreach (var cat in data.CompositionalArmatures)
							{
								if (cat.Layouts != null)
								{
									foreach (var lay in cat.Layouts)
									{
										cbCompositionLayout.Items.Add(lay.Name);
									}
								}
							}
						}
						cbCompositionLayout.SelectedIndex = 0;
					}
					catch { }
				}
			}
		}

		private void RestoreSelectedValues(
			string initialStyleCategory,
			string initialStyle,
			string initialCompositionLayout = "")
		{
			SetComboBoxValue(cbStyleCategory, initialStyleCategory);
			SetComboBoxValue(cbCompositionLayout, initialCompositionLayout);

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

			string userPrompt = txtPrompt.Text.Trim();
			string compLayout = cbCompositionLayout.Text.Trim();

			string prompt = "A scenic comic book image.";
			if (!string.IsNullOrEmpty(userPrompt))
			{
				prompt += $" Subject description: {userPrompt}.";
			}
			if (!string.IsNullOrEmpty(compLayout) && compLayout != "(None)")
			{
				prompt += $" Composition guide/armature: {compLayout}.";
			}
			if (!string.IsNullOrEmpty(stylePrompt))
			{
				prompt += $" Rendered in style: {stylePrompt}.";
			}
			return prompt;
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
							Tab = "Scene / Image",
							Model = Model,
							Prompt = UserPrompt,
							ScaleToFit = ScaleToFit,
							CopyToClipboard = CopyToClipboard,
							StyleCategory = StyleCategory,
							Style = StyleName,
							CompositionLayout = CompositionLayoutName
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
								preset.StyleCategory,
								preset.Style,
								preset.CompositionLayout
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
