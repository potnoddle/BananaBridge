using PaintDotNet;
using PaintDotNet.Effects;
using System;
using System.Drawing;
using System.Windows.Forms;
using BananaBridge.UI;

namespace BananaBridge
{
	public class AiIsolatorToken : EffectConfigToken
	{
		public string ApiKey { get; set; } = string.Empty;
		public string Model { get; set; } = "models/gemini-2.5-flash-image";
		public int[] PixelColors { get; set; } = null;
		public int ImageWidth { get; set; } = 0;
		public int ImageHeight { get; set; } = 0;
		public string SelectionHash { get; set; } = string.Empty;
		public string Prompt { get; set; } = string.Empty;
		public bool DrawOutline { get; set; } = false;
		public int OutlineColor { get; set; } = Color.Red.ToArgb();
		public int OutlineWidth { get; set; } = 2;
		public bool ScaleToFit { get; set; } = true;
		public bool CopyToClipboard { get; set; } = true;

		public AiIsolatorToken()
		{
		}

		protected AiIsolatorToken(AiIsolatorToken other)
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
			this.SelectionHash = other.SelectionHash;
			this.Prompt = other.Prompt;
			this.DrawOutline = other.DrawOutline;
			this.OutlineColor = other.OutlineColor;
			this.OutlineWidth = other.OutlineWidth;
			this.ScaleToFit = other.ScaleToFit;
			this.CopyToClipboard = other.CopyToClipboard;
		}

		public override object Clone()
		{
			return new AiIsolatorToken(this);
		}
	}

	[EffectCategory(EffectCategory.Effect)]
	public class AiIsolatorEffect : Effect<AiIsolatorToken>
	{
		public AiIsolatorEffect()
			: base("AI Outline Selector & Isolator",
				   (Image)null,
				   "AI Tools",
				   new EffectOptions { Flags = EffectFlags.Configurable })
		{
		}

		public override EffectConfigDialog CreateConfigDialog()
		{
			return new AiIsolatorPaintNetDialog();
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

	public class AiIsolatorPaintNetDialog : EffectConfigDialog<AiIsolatorEffect, AiIsolatorToken>
	{
		private AiIsolatorToken _effectToken;

		protected override void InitDialogFromToken(AiIsolatorToken effectToken)
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
				_effectToken.Prompt = string.Empty;
			}

			Bitmap resultImage = null;
			if (_effectToken.PixelColors != null && _effectToken.ImageWidth > 0 && _effectToken.ImageHeight > 0)
			{
				resultImage = BananaToken.ConvertArgbArrayToBitmap(_effectToken.PixelColors, _effectToken.ImageWidth, _effectToken.ImageHeight);
			}

			using (var uiForm = new BananaBridge.UI.AiIsolatorDialog(
				_effectToken.ApiKey,
				_effectToken.Model,
				sourceCrop,
				selectionPixelHash,
				resultImage,
				_effectToken.Prompt,
				_effectToken.DrawOutline,
				Color.FromArgb(_effectToken.OutlineColor),
				_effectToken.OutlineWidth,
				_effectToken.ScaleToFit,
				_effectToken.CopyToClipboard))
			{
				if (uiForm.ShowDialog(this) == DialogResult.OK)
				{
					_effectToken.ApiKey = uiForm.ApiKey;
					_effectToken.Model = uiForm.Model;
					_effectToken.SelectionHash = selectionPixelHash;
					_effectToken.Prompt = uiForm.Prompt;
					_effectToken.DrawOutline = uiForm.DrawOutline;
					_effectToken.OutlineColor = uiForm.OutlineColor.ToArgb();
					_effectToken.OutlineWidth = uiForm.OutlineWidth;
					_effectToken.ScaleToFit = uiForm.ScaleToFit;
					_effectToken.CopyToClipboard = uiForm.CopyToClipboard;

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

		private void CopyCompositeToClipboard(Rectangle bounds, Bitmap generatedResult)
		{
			try
			{
				Clipboard.SetImage(generatedResult);
				MessageBox.Show(
					"The isolated image has been copied to your Clipboard.\r\n" +
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

		protected override AiIsolatorToken CreateInitialToken()
		{
			return new AiIsolatorToken();
		}

		protected override void LoadIntoTokenFromDialog(AiIsolatorToken effectToken)
		{
			if (_effectToken != null)
			{
				effectToken.ApiKey = _effectToken.ApiKey;
				effectToken.Model = _effectToken.Model;
				effectToken.PixelColors = _effectToken.PixelColors != null ? (int[])_effectToken.PixelColors.Clone() : null;
				effectToken.ImageWidth = _effectToken.ImageWidth;
				effectToken.ImageHeight = _effectToken.ImageHeight;
				effectToken.SelectionHash = _effectToken.SelectionHash;
				effectToken.Prompt = _effectToken.Prompt;
				effectToken.DrawOutline = _effectToken.DrawOutline;
				effectToken.OutlineColor = _effectToken.OutlineColor;
				effectToken.OutlineWidth = _effectToken.OutlineWidth;
				effectToken.ScaleToFit = _effectToken.ScaleToFit;
				effectToken.CopyToClipboard = _effectToken.CopyToClipboard;
			}
		}
	}
}
