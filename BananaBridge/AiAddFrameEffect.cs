using PaintDotNet;
using PaintDotNet.Effects;
using System;
using System.Drawing;
using System.Windows.Forms;
using BananaBridge.UI;

namespace BananaBridge
{
	public class AiAddFrameToken : EffectConfigToken
	{
		public string ApiKey { get; set; } = string.Empty;
		public string Model { get; set; } = "models/gemini-2.5-flash-image";
		public int[] PixelColors { get; set; } = null;
		public int ImageWidth { get; set; } = 0;
		public int ImageHeight { get; set; } = 0;
		public string SelectedStyleId { get; set; } = string.Empty;
		public int FrameWeight { get; set; } = 10;
		public bool TransparentCenter { get; set; } = true;
		public string SelectionHash { get; set; } = string.Empty;

		public AiAddFrameToken()
		{
		}

		protected AiAddFrameToken(AiAddFrameToken other)
			: base(other)
		{
			this.ApiKey = other.ApiKey;
			this.Model = other.Model;
			if (other.PixelColors != null)
			{
				this.PixelColors = (int[])other.PixelColors.Clone();
			}
			this.ImageWidth = other.ImageWidth;
			this.ImageHeight = other.ImageHeight;
			this.SelectedStyleId = other.SelectedStyleId;
			this.FrameWeight = other.FrameWeight;
			this.TransparentCenter = other.TransparentCenter;
			this.SelectionHash = other.SelectionHash;
		}

		public override object Clone()
		{
			return new AiAddFrameToken(this);
		}
	}

	[EffectCategory(EffectCategory.Effect)]
	public class AiAddFrameEffect : Effect<AiAddFrameToken>
	{
		public AiAddFrameEffect()
			: base("AI Add Frame",
				   PluginIcons.CreateFrameIcon(),
				   "AI Tools",
				   new EffectOptions { Flags = EffectFlags.Configurable })
		{
		}

		public override EffectConfigDialog CreateConfigDialog()
		{
			return new AiAddFramePaintNetDialog();
		}

		protected override void OnRender(Rectangle[] renderRects, int startIndex, int length)
		{
			if (length == 0) return;

			var token = this.Token;
			if (token == null || token.PixelColors == null || token.ImageWidth <= 0 || token.ImageHeight <= 0)
			{
				for (int i = startIndex; i < startIndex + length; i++)
				{
					Rectangle rect = renderRects[i];
					for (int y = rect.Top; y < rect.Bottom; y++)
					{
						for (int x = rect.Left; x < rect.Right; x++)
						{
							DstArgs.Surface[x, y] = SrcArgs.Surface[x, y];
						}
					}
				}
				return;
			}

			Rectangle selectionBounds = this.EnvironmentParameters.SelectionBounds;
			if (selectionBounds.IsEmpty)
			{
				selectionBounds = SrcArgs.Surface.Bounds;
			}

			int[] pixels = token.PixelColors;
			int imageWidth = token.ImageWidth;
			int imageHeight = token.ImageHeight;
			int frameWeight = token.FrameWeight;
			bool transparentCenter = token.TransparentCenter;

			int borderX = selectionBounds.Width * frameWeight / 100;
			int borderY = selectionBounds.Height * frameWeight / 100;

			var selection = this.EnvironmentParameters.GetSelectionAsPdnRegion();

			for (int i = startIndex; i < startIndex + length; i++)
			{
				Rectangle rect = renderRects[i];
				for (int y = rect.Top; y < rect.Bottom; y++)
				{
					for (int x = rect.Left; x < rect.Right; x++)
					{
						if (selection.IsVisible(x, y))
						{
							int relativeX = x - selectionBounds.Left;
							int relativeY = y - selectionBounds.Top;

							bool isBorder = (relativeX < borderX || relativeX >= selectionBounds.Width - borderX ||
											 relativeY < borderY || relativeY >= selectionBounds.Height - borderY);

							if (!isBorder && transparentCenter)
							{
								// Make center of the selection frame layer transparent
								DstArgs.Surface[x, y] = ColorBgra.FromBgra(0, 0, 0, 0);
							}
							else
							{
								// Map to generated frame pixels (always scaled to fit)
								int cropX = (int)Math.Round((double)relativeX * (imageWidth - 1) / Math.Max(1, selectionBounds.Width - 1));
								int cropY = (int)Math.Round((double)relativeY * (imageHeight - 1) / Math.Max(1, selectionBounds.Height - 1));

								if (cropX >= 0 && cropX < imageWidth && cropY >= 0 && cropY < imageHeight)
								{
									int argb = pixels[cropY * imageWidth + cropX];
									DstArgs.Surface[x, y] = ColorBgra.FromUInt32((uint)argb);
								}
								else
								{
									DstArgs.Surface[x, y] = SrcArgs.Surface[x, y];
								}
							}
						}
						else
						{
							DstArgs.Surface[x, y] = SrcArgs.Surface[x, y];
						}
					}
				}
			}
		}
	}

	public class AiAddFramePaintNetDialog : EffectConfigDialog<AiAddFrameEffect, AiAddFrameToken>
	{
		private AiAddFrameToken _effectToken;

		protected override void InitDialogFromToken(AiAddFrameToken effectToken)
		{
			_effectToken = effectToken;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			this.Size = new Size(0, 0);
			this.FormBorderStyle = FormBorderStyle.None;
			this.Opacity = 0;
			this.ShowInTaskbar = false;
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			if (_effectToken == null || this.EnvironmentParameters == null)
			{
				this.DialogResult = DialogResult.Cancel;
				this.Close();
				return;
			}

			var bounds = this.EnvironmentParameters.SelectionBounds;
			if (bounds.IsEmpty)
			{
				bounds = this.EnvironmentParameters.SourceSurface.Bounds;
			}

			Bitmap sourceCrop = CropSourceSurface(bounds);
			string selectionPixelHash = ComputePixelHash(sourceCrop);

			if (string.IsNullOrEmpty(_effectToken.SelectionHash) || _effectToken.SelectionHash != selectionPixelHash)
			{
				_effectToken.SelectionHash = selectionPixelHash;
				_effectToken.PixelColors = null;
				_effectToken.ImageWidth = 0;
				_effectToken.ImageHeight = 0;
				_effectToken.SelectedStyleId = string.Empty;
			}

			Bitmap resultImage = null;
			if (_effectToken.PixelColors != null && _effectToken.ImageWidth > 0 && _effectToken.ImageHeight > 0)
			{
				resultImage = BananaToken.ConvertArgbArrayToBitmap(_effectToken.PixelColors, _effectToken.ImageWidth, _effectToken.ImageHeight);
			}

			using (var uiForm = new AiAddFrameDialog(
				_effectToken.ApiKey,
				_effectToken.Model,
				sourceCrop,
				resultImage,
				_effectToken.SelectedStyleId,
				_effectToken.FrameWeight,
				_effectToken.TransparentCenter))
			{
				if (uiForm.ShowDialog(this) == DialogResult.OK)
				{
					_effectToken.ApiKey = uiForm.ApiKey;
					_effectToken.Model = uiForm.Model;
					_effectToken.SelectionHash = selectionPixelHash;
					_effectToken.SelectedStyleId = uiForm.SelectedStyleId;
					_effectToken.FrameWeight = uiForm.FrameWeight;
					_effectToken.TransparentCenter = uiForm.TransparentCenter;

					if (uiForm.GeneratedResult != null)
					{
						_effectToken.PixelColors = BananaToken.ConvertBitmapToArgbArray(uiForm.GeneratedResult, out int w, out int h);
						_effectToken.ImageWidth = w;
						_effectToken.ImageHeight = h;
					}
					else
					{
						_effectToken.PixelColors = null;
						_effectToken.ImageWidth = 0;
						_effectToken.ImageHeight = 0;
					}

					if (uiForm.GeneratedResult != null)
					{
						// Render composite with transparent center check to clipboard
						CopyFrameCompositeToClipboard(bounds, uiForm.GeneratedResult, uiForm.FrameWeight, uiForm.TransparentCenter);
					}

					this.FinishTokenUpdate();
					this.DialogResult = DialogResult.OK;
				}
				else
				{
					this.DialogResult = DialogResult.Cancel;
				}
			}

			this.Close();
		}

		protected override AiAddFrameToken CreateInitialToken()
		{
			return new AiAddFrameToken();
		}

		protected override void LoadIntoTokenFromDialog(AiAddFrameToken effectToken)
		{
			if (_effectToken != null)
			{
				effectToken.ApiKey = _effectToken.ApiKey;
				effectToken.Model = _effectToken.Model;
				effectToken.PixelColors = _effectToken.PixelColors != null ? (int[])_effectToken.PixelColors.Clone() : null;
				effectToken.ImageWidth = _effectToken.ImageWidth;
				effectToken.ImageHeight = _effectToken.ImageHeight;
				effectToken.SelectedStyleId = _effectToken.SelectedStyleId;
				effectToken.FrameWeight = _effectToken.FrameWeight;
				effectToken.TransparentCenter = _effectToken.TransparentCenter;
				effectToken.SelectionHash = _effectToken.SelectionHash;
			}
		}

		private Bitmap CropSourceSurface(Rectangle bounds)
		{
			var source = this.EnvironmentParameters.SourceSurface;
			Bitmap bmp = new Bitmap(bounds.Width, bounds.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			for (int y = 0; y < bounds.Height; y++)
			{
				for (int x = 0; x < bounds.Width; x++)
				{
					ColorBgra pixel = source[bounds.Left + x, bounds.Top + y];
					bmp.SetPixel(x, y, Color.FromArgb(pixel.A, pixel.R, pixel.G, pixel.B));
				}
			}
			return bmp;
		}

		private string ComputePixelHash(Bitmap bitmap)
		{
			using (var ms = new System.IO.MemoryStream())
			{
				bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
				using (var sha = System.Security.Cryptography.SHA256.Create())
				{
					byte[] hashBytes = sha.ComputeHash(ms.ToArray());
					return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
				}
			}
		}

		private void CopyFrameCompositeToClipboard(Rectangle bounds, Bitmap generatedResult, int weight, bool transparentCenter)
		{
			try
			{
				var source = this.EnvironmentParameters.SourceSurface;
				Bitmap composite = new Bitmap(source.Width, source.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

				// If transparentCenter, we copy original pixels for the center, and overwrite the border with the frame.
				// If NOT transparentCenter, we overwrite the entire selection bounds with the generated result.
				for (int y = 0; y < source.Height; y++)
				{
					for (int x = 0; x < source.Width; x++)
					{
						ColorBgra pixel = source[x, y];
						composite.SetPixel(x, y, Color.FromArgb(pixel.A, pixel.R, pixel.G, pixel.B));
					}
				}

				int borderX = bounds.Width * weight / 100;
				int borderY = bounds.Height * weight / 100;

				for (int y = 0; y < bounds.Height; y++)
				{
					for (int x = 0; x < bounds.Width; x++)
					{
						bool isBorder = (x < borderX || x >= bounds.Width - borderX ||
										 y < borderY || y >= bounds.Height - borderY);

						if (isBorder || !transparentCenter)
						{
							// Scale generatedResult pixel to bounds
							int rx = (int)Math.Round((double)x * (generatedResult.Width - 1) / Math.Max(1, bounds.Width - 1));
							int ry = (int)Math.Round((double)y * (generatedResult.Height - 1) / Math.Max(1, bounds.Height - 1));
							if (rx >= 0 && rx < generatedResult.Width && ry >= 0 && ry < generatedResult.Height)
							{
								composite.SetPixel(bounds.Left + x, bounds.Top + y, generatedResult.GetPixel(rx, ry));
							}
						}
					}
				}

				Clipboard.SetImage(composite);
				MessageBox.Show(
					"The composite framed image has been copied to your Clipboard.\r\n" +
					"To add it to a new layer, press Ctrl+Alt+V or Edit > Paste into New Layer.",
					"Layer Ready",
					MessageBoxButtons.OK,
					MessageBoxIcon.Information
				);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Failed to copy to Clipboard: {ex.Message}", "Clipboard Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}
	}
}

