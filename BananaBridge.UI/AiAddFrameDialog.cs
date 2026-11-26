using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;
using BananaBridge.UI.Models;

namespace BananaBridge.UI
{
	public partial class AiAddFrameDialog : Form
	{
		private List<FrameStyleItem> _frameStyles = new List<FrameStyleItem>();
		private Bitmap _generatedResult = null;
		private string _apiKey = string.Empty;

		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public Bitmap SourceCrop { get; set; }

		public string ApiKey => _apiKey;
		public string Model => cbModel.SelectedItem?.ToString() ?? "";
		public Bitmap GeneratedResult => _generatedResult;
		public bool CopyToClipboard => chkCopyToClipboard.Checked;
		public bool TransparentCenter => chkTransparentCenter.Checked;
		public int FrameWeight => tbWeight.Value;

		public string SelectedStyleId => (cbStyles.SelectedItem as FrameStyleItem)?.StyleType ?? "";

		public AiAddFrameDialog(
			string initialApiKey,
			string initialModel,
			Bitmap sourceCrop,
			Bitmap initialResult,
			string initialStyleId,
			int initialWeight,
			bool initialTransparentCenter)
		{
			InitializeComponent();

			_apiKey = initialApiKey;
			SourceCrop = sourceCrop;
			tbWeight.Value = initialWeight >= 2 && initialWeight <= 30 ? initialWeight : 10;
			lblWeightValue.Text = $"{tbWeight.Value}%";
			chkTransparentCenter.Checked = initialTransparentCenter;

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

			LoadFrameStyles();

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
					if (cbStyles.Items[i] is FrameStyleItem item && item.StyleType == initialStyleId)
					{
						cbStyles.SelectedIndex = i;
						break;
					}
				}
			}

			// CRITICAL: Disable Commit Layer until something is generated
			btnOK.Enabled = (_generatedResult != null);
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

		private void LoadFrameStyles()
		{
			try
			{
				using (Stream stream = ConfigService.GetConfigStream("frames-styles.json"))
				{
					if (stream != null)
					{
						using (StreamReader reader = new StreamReader(stream))
						{
							string json = reader.ReadToEnd();
							var root = JsonSerializer.Deserialize<FrameStyleRoot>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
							_frameStyles = root?.VictorianSceneFrames ?? new List<FrameStyleItem>();
						}
					}
				}

				cbStyles.Items.Clear();
				foreach (var style in _frameStyles)
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
				MessageBox.Show($"Failed to load frame styles: {ex.Message}", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

		private void CbStyles_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cbStyles.SelectedItem is FrameStyleItem selectedStyle)
			{
				txtStyleDesc.Text = $"Tone: {selectedStyle.Tone}\r\n\r\n" +
				                   $"Substrate: {selectedStyle.BaseStyle?.Substrate}\r\n\r\n" +
				                   $"Rendering: {selectedStyle.BaseStyle?.Rendering}\r\n\r\n" +
				                   $"Pigmentation: {selectedStyle.BaseStyle?.Pigmentation}";
			}
		}

		private void TbWeight_Scroll(object sender, EventArgs e)
		{
			lblWeightValue.Text = $"{tbWeight.Value}%";
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

			if (!(cbStyles.SelectedItem is FrameStyleItem styleItem))
			{
				MessageBox.Show("Please select a frame style first.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			btnGenerate.Enabled = false;
			prgLoading.Visible = true;

			try
			{
				int thickness = tbWeight.Value;
				string addition = txtPromptAddition.Text.Trim();
				string additionText = !string.IsNullOrEmpty(addition) ? $" Additionally, incorporate these elements into the frame design: {addition}." : "";

				string finalPrompt = $"Draw an ornate, decorative frame around the borders of the image. " +
				                     $"The frame style should be '{styleItem.StyleType}'. " +
				                     $"Tone of the frame: {styleItem.Tone} " +
				                     $"Border specifications: Texture is {styleItem.BorderAttributes?.Texture ?? "default"}, ornateness is {styleItem.BorderAttributes?.Ornateness ?? "default"}, shading is {styleItem.BorderAttributes?.Shading ?? "default"}. " +
				                     $"Substrate is: {styleItem.BaseStyle?.Substrate} " +
				                     $"Rendering technique: {styleItem.BaseStyle?.Rendering} " +
				                     $"Colors/Pigmentation: {styleItem.BaseStyle?.Pigmentation}. " +
				                     $"The frame border must cover exactly the outer {thickness}% thickness of the image, keeping the inner image content visible and framed.{additionText}";

				Bitmap sourceCrop = this.SourceCrop;
				
				// For the frame, the mask is the entire image because we are generating a border around it.
				// We can create a mask where the outer thickness% is white (masked/editable) and the center is black (not editable/preserved),
				// which tells Gemini to only generate the frame in that border area! This is extremely smart!
				Bitmap maskCrop = new Bitmap(sourceCrop.Width, sourceCrop.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
				using (Graphics g = Graphics.FromImage(maskCrop))
				{
					g.Clear(Color.Black);
					int borderX = sourceCrop.Width * thickness / 100;
					int borderY = sourceCrop.Height * thickness / 100;
					// Fill outer area with White
					using (var brush = new SolidBrush(Color.White))
					{
						// Top
						g.FillRectangle(brush, 0, 0, sourceCrop.Width, borderY);
						// Bottom
						g.FillRectangle(brush, 0, sourceCrop.Height - borderY, sourceCrop.Width, borderY);
						// Left
						g.FillRectangle(brush, 0, borderY, borderX, sourceCrop.Height - 2 * borderY);
						// Right
						g.FillRectangle(brush, sourceCrop.Width - borderX, borderY, borderX, sourceCrop.Height - 2 * borderY);
					}
				}

				string model = cbModel.SelectedItem?.ToString() ?? "";
				string aspectRatio = sourceCrop != null ? GeminiClient.GetClosestAspectRatio(sourceCrop.Width, sourceCrop.Height) : null;

				string provider = SecureStore.LoadProvider();
				Bitmap result = await Task.Run(async () =>
				{
					IAiClient client = AiClientFactory.CreateClient(provider);
					return await client.SendEditRequestAsync(
						apiKey,
						model,
						new List<ChatTurn>(), // One-shot
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

	public class FrameStyleRoot
	{
		[JsonPropertyName("victorian_scene_frames")]
		public List<FrameStyleItem> VictorianSceneFrames { get; set; }
	}

	public class FrameStyleItem
	{
		[JsonPropertyName("style_type")]
		public string StyleType { get; set; }

		[JsonPropertyName("tone")]
		public string Tone { get; set; }

		[JsonPropertyName("border_attributes")]
		public FrameBorderAttributes BorderAttributes { get; set; }

		[JsonPropertyName("base_style")]
		public FrameBaseStyle BaseStyle { get; set; }

		public override string ToString()
		{
			// Convert style_type from snake_case to Title Case for nice display
			if (string.IsNullOrEmpty(StyleType)) return string.Empty;
			var parts = StyleType.Split('_');
			for (int i = 0; i < parts.Length; i++)
			{
				if (parts[i].Length > 0)
				{
					parts[i] = char.ToUpper(parts[i][0]) + parts[i].Substring(1);
				}
			}
			return string.Join(" ", parts);
		}
	}

	public class FrameBorderAttributes
	{
		[JsonPropertyName("weight")]
		public string Weight { get; set; }

		[JsonPropertyName("color")]
		public string Color { get; set; }

		[JsonPropertyName("texture")]
		public string Texture { get; set; }

		[JsonPropertyName("ornateness")]
		public string Ornateness { get; set; }

		[JsonPropertyName("shading")]
		public string Shading { get; set; }
	}

	public class FrameBaseStyle
	{
		[JsonPropertyName("substrate")]
		public string Substrate { get; set; }

		[JsonPropertyName("rendering")]
		public string Rendering { get; set; }

		[JsonPropertyName("pigmentation")]
		public string Pigmentation { get; set; }
	}
}

