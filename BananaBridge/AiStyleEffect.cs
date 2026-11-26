using PaintDotNet;
using PaintDotNet.Effects;
using System;
using System.Drawing;
using System.Windows.Forms;
using BananaBridge.UI;

namespace BananaBridge
{
	public class AiStyleToken : EffectConfigToken
	{
		public string ApiKey { get; set; } = string.Empty;
		public string Model { get; set; } = "models/gemini-2.5-flash-image";
		public int[] PixelColors { get; set; } = null;
		public int ImageWidth { get; set; } = 0;
		public int ImageHeight { get; set; } = 0;
		public string SelectedCategory { get; set; } = string.Empty;
		public string SelectedStyleId { get; set; } = string.Empty;
		public int StyleStrength { get; set; } = 70;
		public bool ScaleToFit { get; set; } = true;
		public string SelectionHash { get; set; } = string.Empty;

		public AiStyleToken()
		{
		}

		protected AiStyleToken(AiStyleToken other)
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
			this.SelectedCategory = other.SelectedCategory;
			this.SelectedStyleId = other.SelectedStyleId;
			this.StyleStrength = other.StyleStrength;
			this.ScaleToFit = other.ScaleToFit;
			this.SelectionHash = other.SelectionHash;
		}

		public override object Clone()
		{
			return new AiStyleToken(this);
		}
	}

	public static class PluginIcons
	{
		public static Image CreateStyleIcon()
		{
			Bitmap bmp = new Bitmap(16, 16);
			using (Graphics g = Graphics.FromImage(bmp))
			{
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
				g.FillEllipse(Brushes.SandyBrown, 1, 1, 14, 14);
				g.DrawEllipse(Pens.SaddleBrown, 1, 1, 14, 14);
				g.FillEllipse(Brushes.Crimson, 3, 4, 3, 3);
				g.FillEllipse(Brushes.Gold, 7, 3, 3, 3);
				g.FillEllipse(Brushes.RoyalBlue, 10, 6, 3, 3);
				g.FillEllipse(Brushes.LimeGreen, 7, 10, 3, 3);
				g.FillEllipse(Brushes.DarkGray, 3, 9, 3, 3);
			}
			return bmp;
		}

		public static Image CreateEditIcon()
		{
			Bitmap bmp = new Bitmap(16, 16);
			using (Graphics g = Graphics.FromImage(bmp))
			{
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
				g.FillRectangle(Brushes.DimGray, 2, 12, 4, 2);
				g.DrawLine(new Pen(Color.SaddleBrown, 2), 4, 12, 10, 6);
				g.FillEllipse(Brushes.Yellow, 10, 2, 5, 5);
				g.DrawLine(Pens.Gold, 12, 0, 12, 8);
				g.DrawLine(Pens.Gold, 8, 4, 16, 4);
			}
			return bmp;
		}

		public static Image CreateFrameIcon()
		{
			Bitmap bmp = new Bitmap(16, 16);
			using (Graphics g = Graphics.FromImage(bmp))
			{
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
				g.DrawRectangle(new Pen(Color.Gold, 3), 1, 1, 14, 14);
				g.DrawRectangle(Pens.SaddleBrown, 0, 0, 16, 16);
				g.FillRectangle(Brushes.LightSkyBlue, 3, 3, 10, 10);
				g.FillPolygon(Brushes.ForestGreen, new Point[] {
					new Point(3, 13),
					new Point(8, 7),
					new Point(13, 13)
				});
			}
			return bmp;
		}
	}

	[EffectCategory(EffectCategory.Effect)]
	public class AiStyleEffect : Effect<AiStyleToken>
	{
		public AiStyleEffect()
			: base("AI Style Applier",
				   PluginIcons.CreateStyleIcon(),
				   "AI Tools",
				   new EffectOptions { Flags = EffectFlags.Configurable })
		{
		}

		public override EffectConfigDialog CreateConfigDialog()
		{
			return new AiStylePaintNetDialog();
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
			bool scaleToFit = token.ScaleToFit;

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
							int cropX, cropY;
							if (scaleToFit)
							{
								double scale = Math.Max((double)selectionBounds.Width / imageWidth, (double)selectionBounds.Height / imageHeight);
								double scaledW = imageWidth * scale;
								double scaledH = imageHeight * scale;
								double offsetX = (selectionBounds.Width - scaledW) / 2.0;
								double offsetY = (selectionBounds.Height - scaledH) / 2.0;

								double dx = x - selectionBounds.Left;
								double dy = y - selectionBounds.Top;

								cropX = (int)Math.Round((dx - offsetX) / scale);
								cropY = (int)Math.Round((dy - offsetY) / scale);
							}
							else
							{
								cropX = x - selectionBounds.Left;
								cropY = y - selectionBounds.Top;
							}

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
						else
						{
							DstArgs.Surface[x, y] = SrcArgs.Surface[x, y];
						}
					}
				}
			}
		}
	}

	public class AiStylePaintNetDialog : EffectConfigDialog<AiStyleEffect, AiStyleToken>
	{
		private AiStyleToken _effectToken;

		protected override void InitDialogFromToken(AiStyleToken effectToken)
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
			Bitmap maskCrop = GenerateMask(bounds);
			string selectionPixelHash = ComputePixelHash(sourceCrop);

			if (string.IsNullOrEmpty(_effectToken.SelectionHash) || _effectToken.SelectionHash != selectionPixelHash)
			{
				_effectToken.SelectionHash = selectionPixelHash;
				_effectToken.PixelColors = null;
				_effectToken.ImageWidth = 0;
				_effectToken.ImageHeight = 0;
				_effectToken.SelectedCategory = string.Empty;
				_effectToken.SelectedStyleId = string.Empty;
			}

			Bitmap resultImage = null;
			if (_effectToken.PixelColors != null && _effectToken.ImageWidth > 0 && _effectToken.ImageHeight > 0)
			{
				resultImage = BananaToken.ConvertArgbArrayToBitmap(_effectToken.PixelColors, _effectToken.ImageWidth, _effectToken.ImageHeight);
			}

			using (var uiForm = new AiStyleDialog(
				_effectToken.ApiKey,
				_effectToken.Model,
				sourceCrop,
				maskCrop,
				selectionPixelHash,
				resultImage,
				_effectToken.SelectedCategory,
				_effectToken.SelectedStyleId,
				_effectToken.StyleStrength,
				_effectToken.ScaleToFit))
			{
				if (uiForm.ShowDialog(this) == DialogResult.OK)
				{
					_effectToken.ApiKey = uiForm.ApiKey;
					_effectToken.Model = uiForm.Model;
					_effectToken.SelectionHash = selectionPixelHash;
					_effectToken.SelectedCategory = uiForm.SelectedCategoryId;
					_effectToken.SelectedStyleId = uiForm.SelectedStyleId;
					_effectToken.StyleStrength = uiForm.StyleStrength;
					_effectToken.ScaleToFit = uiForm.ScaleToFit;

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

					if (uiForm.CopyToClipboard && uiForm.GeneratedResult != null)
					{
						CopyCompositeToClipboard(bounds, uiForm.GeneratedResult);
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

		protected override AiStyleToken CreateInitialToken()
		{
			return new AiStyleToken();
		}

		protected override void LoadIntoTokenFromDialog(AiStyleToken effectToken)
		{
			if (_effectToken != null)
			{
				effectToken.ApiKey = _effectToken.ApiKey;
				effectToken.Model = _effectToken.Model;
				effectToken.PixelColors = _effectToken.PixelColors != null ? (int[])_effectToken.PixelColors.Clone() : null;
				effectToken.ImageWidth = _effectToken.ImageWidth;
				effectToken.ImageHeight = _effectToken.ImageHeight;
				effectToken.SelectedCategory = _effectToken.SelectedCategory;
				effectToken.SelectedStyleId = _effectToken.SelectedStyleId;
				effectToken.StyleStrength = _effectToken.StyleStrength;
				effectToken.ScaleToFit = _effectToken.ScaleToFit;
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

		private Bitmap GenerateMask(Rectangle bounds)
		{
			Bitmap mask = new Bitmap(bounds.Width, bounds.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			using (Graphics g = Graphics.FromImage(mask))
			{
				g.Clear(Color.Transparent);
			}
			return mask;
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

		private void CopyCompositeToClipboard(Rectangle bounds, Bitmap generatedResult)
		{
			try
			{
				Clipboard.SetImage(generatedResult);
				MessageBox.Show(
					"The generated styled image has been copied to your Clipboard.\r\n" +
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

