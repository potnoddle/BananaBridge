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
	public partial class AiIsolatorDialog : Form
	{
		private string _apiKey = string.Empty;
		private string _selectionPixelHash = string.Empty;
		private Bitmap _sourceCrop = null;
		private Bitmap _generatedResult = null;
		private Bitmap _isolatedBaseImage = null;
		private Bitmap _maskImage = null;
		private Color _customOutlineColor = Color.Red;
		private bool _isGenerating = false;
		private bool _isInitializing = false;

		public string ApiKey => _apiKey;
		public string Model => cbModel.SelectedItem?.ToString() ?? "";
		public string Prompt => txtPrompt.Text.Trim();
		public Bitmap GeneratedResult => _generatedResult;
		public bool DrawOutline => chkDrawOutline.Checked;
		public Color OutlineColor
		{
			get
			{
				string selected = cbOutlineColor.SelectedItem?.ToString();
				if (selected == "Custom...")
				{
					return _customOutlineColor;
				}
				switch (selected)
				{
					case "Red": return Color.Red;
					case "Green": return Color.Green;
					case "Blue": return Color.Blue;
					case "Yellow": return Color.Yellow;
					default: return Color.Red;
				}
			}
		}
		public int OutlineWidth => (int)numOutlineWidth.Value;
		public bool ScaleToFit => chkScaleToFit.Checked;
		public bool CopyToClipboard => chkCopyToClipboard.Checked;

		public AiIsolatorDialog(
			string initialApiKey,
			string initialModel,
			Bitmap sourceCrop,
			string selectionPixelHash,
			Bitmap initialResult,
			string initialPrompt,
			bool initialDrawOutline,
			Color initialOutlineColor,
			int initialOutlineWidth,
			bool initialScaleToFit,
			bool initialCopyToClipboard)
		{
			_isInitializing = true;
			InitializeComponent();

			_apiKey = initialApiKey;
			_sourceCrop = sourceCrop;
			_selectionPixelHash = selectionPixelHash;
			_generatedResult = initialResult;

			txtPrompt.Text = initialPrompt;
			chkDrawOutline.Checked = initialDrawOutline;
			chkScaleToFit.Checked = initialScaleToFit;
			chkCopyToClipboard.Checked = initialCopyToClipboard;

			if (initialOutlineWidth >= (int)numOutlineWidth.Minimum && initialOutlineWidth <= (int)numOutlineWidth.Maximum)
			{
				numOutlineWidth.Value = initialOutlineWidth;
			}

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

			// Populate Outline Colors
			cbOutlineColor.Items.Add("Red");
			cbOutlineColor.Items.Add("Green");
			cbOutlineColor.Items.Add("Blue");
			cbOutlineColor.Items.Add("Yellow");
			cbOutlineColor.Items.Add("Custom...");

			// Find matching initial color
			if (initialOutlineColor == Color.Red) cbOutlineColor.SelectedIndex = 0;
			else if (initialOutlineColor == Color.Green) cbOutlineColor.SelectedIndex = 1;
			else if (initialOutlineColor == Color.Blue) cbOutlineColor.SelectedIndex = 2;
			else if (initialOutlineColor == Color.Yellow) cbOutlineColor.SelectedIndex = 3;
			else
			{
				_customOutlineColor = initialOutlineColor;
				cbOutlineColor.SelectedIndex = 4; // Custom
			}

			if (_generatedResult != null)
			{
				pbPreview.Image = new Bitmap(_generatedResult);
				btnOK.Enabled = true;
			}
			else if (_sourceCrop != null)
			{
				pbPreview.Image = new Bitmap(_sourceCrop);
				btnOK.Enabled = false;
			}

			UpdateOutlineControlsState();
			SetupPreviewBackground();
			_isInitializing = false;
		}

		private void SetupPreviewBackground()
		{
			// Generate a 16x16 checkerboard pattern image
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

		private void TxtPrompt_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter && !e.Shift)
			{
				e.SuppressKeyPress = true;
				BtnGenerate_Click(this, EventArgs.Empty);
			}
		}

		private async void BtnGenerate_Click(object sender, EventArgs e)
		{
			if (_isGenerating) return;

			string apiKey = _apiKey;
			string subject = txtPrompt.Text.Trim();

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

				// Build the prompt requesting a binary mask
				string subjectDescriptor = string.IsNullOrEmpty(subject) ? "the main subject" : $"'{subject}'";
				string finalPrompt = $"Isolate the subject {subjectDescriptor} from the image. " +
					"Return a black-and-white mask of the subject where the subject pixels are white (RGB 255, 255, 255) " +
					"and the background is black (RGB 0, 0, 0). Do not include any other objects, shadows, reflections, " +
					"text, or borders. Return ONLY the base64-encoded PNG image of the mask.";

				string aspectRatio = _sourceCrop != null ? GeminiClient.GetClosestAspectRatio(_sourceCrop.Width, _sourceCrop.Height) : null;

				Bitmap maskBmp = await Task.Run(async () =>
				{
					IAiClient client = AiClientFactory.CreateClient(provider);
					// Send the source image with empty history to get the mask
					return await client.SendEditRequestAsync(
						apiKey,
						model,
						new List<ChatTurn>(),
						_sourceCrop,
						null,
						finalPrompt,
						aspectRatio
					);
				});

				if (maskBmp != null)
				{
					_maskImage = EnsureMatchingSize(maskBmp, _sourceCrop.Width, _sourceCrop.Height);
					
					// Fill internal holes in the mask to preserve eyes, patterns, etc.
					FillInternalHoles(_maskImage);

					_isolatedBaseImage = ApplyMaskToSource(_sourceCrop, _maskImage);
					
					ApplyOutlineAndDisplay();
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

		private void BtnOK_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(_apiKey))
			{
				SecureStore.SaveApiKey(_apiKey);
			}
		}

		private void ChkDrawOutline_CheckedChanged(object sender, EventArgs e)
		{
			UpdateOutlineControlsState();
			if (!_isInitializing)
			{
				ApplyOutlineAndDisplay();
			}
		}

		private void CbOutlineColor_SelectedIndexChanged(object sender, EventArgs e)
		{
			string selected = cbOutlineColor.SelectedItem?.ToString();
			if (selected == "Custom...")
			{
				if (_isInitializing)
				{
					return;
				}

				using (ColorDialog dlg = new ColorDialog())
				{
					dlg.Color = _customOutlineColor;
					if (dlg.ShowDialog(this) == DialogResult.OK)
					{
						_customOutlineColor = dlg.Color;
						ApplyOutlineAndDisplay();
					}
					else
					{
						// Revert dropdown index to Red if canceled
						cbOutlineColor.SelectedIndex = 0;
					}
				}
			}
			else
			{
				if (!_isInitializing)
				{
					ApplyOutlineAndDisplay();
				}
			}
		}

		private void OutlineSetting_Changed(object sender, EventArgs e)
		{
			if (!_isInitializing)
			{
				ApplyOutlineAndDisplay();
			}
		}

		private void UpdateOutlineControlsState()
		{
			bool enabled = chkDrawOutline.Checked;
			cbOutlineColor.Enabled = enabled;
			numOutlineWidth.Enabled = enabled;
		}

		private void ApplyOutlineAndDisplay()
		{
			if (_isolatedBaseImage == null || _maskImage == null)
			{
				return;
			}

			Bitmap previewBmp = new Bitmap(_isolatedBaseImage);

			if (chkDrawOutline.Checked)
			{
				DrawBoundaryOutline(previewBmp, _maskImage, OutlineColor, OutlineWidth);
			}

			if (_generatedResult != null)
			{
				_generatedResult.Dispose();
			}
			_generatedResult = previewBmp;

			if (pbPreview.Image != null)
			{
				pbPreview.Image.Dispose();
			}
			pbPreview.Image = new Bitmap(_generatedResult);
		}

		private Bitmap ApplyMaskToSource(Bitmap source, Bitmap mask)
		{
			int width = source.Width;
			int height = source.Height;
			Bitmap result = new Bitmap(width, height, PixelFormat.Format32bppArgb);

			var rect = new Rectangle(0, 0, width, height);
			BitmapData srcData = source.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
			BitmapData maskData = mask.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
			BitmapData resData = result.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

			try
			{
				unsafe
				{
					byte* srcPtr = (byte*)srcData.Scan0;
					byte* maskPtr = (byte*)maskData.Scan0;
					byte* resPtr = (byte*)resData.Scan0;

					int srcStride = srcData.Stride;
					int maskStride = maskData.Stride;
					int resStride = resData.Stride;

					for (int y = 0; y < height; y++)
					{
						byte* srcRow = srcPtr + (y * srcStride);
						byte* maskRow = maskPtr + (y * maskStride);
						byte* resRow = resPtr + (y * resStride);

						for (int x = 0; x < width; x++)
						{
							// Calculate mask brightness (using blue channel as representative for grayscale)
							byte maskVal = maskRow[x * 4];

							// Scale original alpha by the mask's soft edge value (feathering)
							double maskFactor = maskVal / 255.0;

							resRow[x * 4] = srcRow[x * 4];         // B
							resRow[x * 4 + 1] = srcRow[x * 4 + 1]; // G
							resRow[x * 4 + 2] = srcRow[x * 4 + 2]; // R
							resRow[x * 4 + 3] = (byte)(srcRow[x * 4 + 3] * maskFactor); // A
						}
					}
				}
			}
			finally
			{
				source.UnlockBits(srcData);
				mask.UnlockBits(maskData);
				result.UnlockBits(resData);
			}

			return result;
		}

		public static void DrawBoundaryOutline(Bitmap target, Bitmap mask, Color outlineColor, int width)
		{
			int w = target.Width;
			int h = target.Height;
			double radius = width;

			// Store outline alpha factors in a buffer
			int[] outlineBuffer = new int[w * h];

			var rect = new Rectangle(0, 0, w, h);
			BitmapData maskData = mask.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

			try
			{
				unsafe
				{
					byte* maskScan0 = (byte*)maskData.Scan0;
					int stride = maskData.Stride;

					for (int y = 0; y < h; y++)
					{
						for (int x = 0; x < w; x++)
						{
							byte* currentPtr = maskScan0 + (y * stride) + (x * 4);
							byte maskVal = currentPtr[0];

							if (maskVal > 0)
							{
								// Find the shortest distance to a background pixel (maskVal == 0)
								double minSubpixelDist = double.MaxValue;
								int intRadius = (int)Math.Ceiling(radius + 1.0);

								for (int ny = Math.Max(0, y - intRadius); ny <= Math.Min(h - 1, y + intRadius); ny++)
								{
									for (int nx = Math.Max(0, x - intRadius); nx <= Math.Min(w - 1, x + intRadius); nx++)
									{
										byte* neighborPtr = maskScan0 + (ny * stride) + (nx * 4);
										if (neighborPtr[0] == 0)
										{
											double dist = Math.Sqrt((nx - x) * (nx - x) + (ny - y) * (ny - y));
											if (dist < minSubpixelDist)
											{
												minSubpixelDist = dist;
											}
										}
									}
								}

								// Smooth anti-aliased edge drawing using Euclidean distance
								if (minSubpixelDist <= radius)
								{
									double factor = maskVal / 255.0; // Blend with mask transparency
									Color blendedColor = Color.FromArgb(
										(byte)(outlineColor.A * factor),
										outlineColor.R,
										outlineColor.G,
										outlineColor.B
									);
									outlineBuffer[y * w + x] = blendedColor.ToArgb();
								}
								else if (minSubpixelDist < radius + 1.0)
								{
									// Outer edge anti-aliasing
									double factor = (1.0 - (minSubpixelDist - radius)) * (maskVal / 255.0);
									Color blendedColor = Color.FromArgb(
										(byte)(outlineColor.A * factor),
										outlineColor.R,
										outlineColor.G,
										outlineColor.B
									);
									outlineBuffer[y * w + x] = blendedColor.ToArgb();
								}
							}
						}
					}
				}
			}
			finally
			{
				mask.UnlockBits(maskData);
			}

			// Composite the outline onto the target image
			BitmapData targetData = target.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
			try
			{
				unsafe
				{
					byte* targetScan0 = (byte*)targetData.Scan0;
					int stride = targetData.Stride;

					for (int y = 0; y < h; y++)
					{
						for (int x = 0; x < w; x++)
						{
							int outlineArgb = outlineBuffer[y * w + x];
							if (outlineArgb != 0)
							{
								Color outCol = Color.FromArgb(outlineArgb);
								byte* pixelPtr = targetScan0 + (y * stride) + (x * 4);

								// Source-over alpha blending
								double outAlpha = outCol.A / 255.0;
								double targetAlpha = pixelPtr[3] / 255.0;

								double rAlpha = outAlpha + targetAlpha * (1.0 - outAlpha);
								if (rAlpha > 0)
								{
									pixelPtr[0] = (byte)((outCol.B * outAlpha + pixelPtr[0] * targetAlpha * (1.0 - outAlpha)) / rAlpha);
									pixelPtr[1] = (byte)((outCol.G * outAlpha + pixelPtr[1] * targetAlpha * (1.0 - outAlpha)) / rAlpha);
									pixelPtr[2] = (byte)((outCol.R * outAlpha + pixelPtr[2] * targetAlpha * (1.0 - outAlpha)) / rAlpha);
									pixelPtr[3] = (byte)(rAlpha * 255.0);
								}
							}
						}
					}
				}
			}
			finally
			{
				target.UnlockBits(targetData);
			}
		}

		private Bitmap EnsureMatchingSize(Bitmap image, int width, int height)
		{
			if (image.Width == width && image.Height == height && image.PixelFormat == PixelFormat.Format32bppArgb)
			{
				return image;
			}

			Bitmap resized = new Bitmap(width, height, PixelFormat.Format32bppArgb);
			using (Graphics g = Graphics.FromImage(resized))
			{
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
				g.DrawImage(image, 0, 0, width, height);
			}
			image.Dispose();
			return resized;
		}

		public static void FillInternalHoles(Bitmap mask)
		{
			if (mask == null) return;
			int w = mask.Width;
			int h = mask.Height;
			if (w <= 0 || h <= 0) return;

			bool[] isOuterBackground = new bool[w * h];
			var queue = new Queue<int>();
			var rect = new Rectangle(0, 0, w, h);
			BitmapData maskData = mask.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
			try
			{
				unsafe
				{
					byte* maskScan0 = (byte*)maskData.Scan0;
					int stride = maskData.Stride;

					// Top and bottom borders
					for (int x = 0; x < w; x++)
					{
						// Top border (y = 0)
						byte* topPtr = maskScan0 + (0 * stride) + (x * 4);
						if (topPtr[0] <= 127)
						{
							isOuterBackground[x] = true;
							queue.Enqueue(x);
						}
						// Bottom border (y = h - 1)
						byte* botPtr = maskScan0 + ((h - 1) * stride) + (x * 4);
						int botIdx = (h - 1) * w + x;
						if (botPtr[0] <= 127)
						{
							isOuterBackground[botIdx] = true;
							queue.Enqueue(botIdx);
						}
					}

					// Left and right borders
					for (int y = 1; y < h - 1; y++)
					{
						// Left border (x = 0)
						byte* leftPtr = maskScan0 + (y * stride) + (0 * 4);
						int leftIdx = y * w;
						if (leftPtr[0] <= 127)
						{
							isOuterBackground[leftIdx] = true;
							queue.Enqueue(leftIdx);
						}
						// Right border (x = w - 1)
						byte* rightPtr = maskScan0 + (y * stride) + ((w - 1) * 4);
						int rightIdx = y * w + (w - 1);
						if (rightPtr[0] <= 127)
						{
							isOuterBackground[rightIdx] = true;
							queue.Enqueue(rightIdx);
						}
					}

					int[] dx = { 0, 0, 1, -1 };
					int[] dy = { 1, -1, 0, 0 };

					while (queue.Count > 0)
					{
						int curr = queue.Dequeue();
						int cx = curr % w;
						int cy = curr / w;

						for (int i = 0; i < 4; i++)
						{
							int nx = cx + dx[i];
							int ny = cy + dy[i];
							if (nx >= 0 && nx < w && ny >= 0 && ny < h)
							{
								int nIdx = ny * w + nx;
								if (!isOuterBackground[nIdx])
								{
									byte* neighborPtr = maskScan0 + (ny * stride) + (nx * 4);
									if (neighborPtr[0] <= 127)
									{
										isOuterBackground[nIdx] = true;
										queue.Enqueue(nIdx);
									}
								}
							}
						}
					}

					// Write back the white pixels (255, 255, 255, 255) to the non-outer-background regions.
					// This will fill internal holes (which were not reached by BFS from outer background) and also preserve subject pixels.
					for (int y = 0; y < h; y++)
					{
						for (int x = 0; x < w; x++)
						{
							int idx = y * w + x;
							if (!isOuterBackground[idx])
							{
								byte* pixelPtr = maskScan0 + (y * stride) + (x * 4);
								pixelPtr[0] = 255; // B
								pixelPtr[1] = 255; // G
								pixelPtr[2] = 255; // R
								pixelPtr[3] = 255; // A
							}
						}
					}
				}
			}
			finally
			{
				mask.UnlockBits(maskData);
			}
		}
	}
}
