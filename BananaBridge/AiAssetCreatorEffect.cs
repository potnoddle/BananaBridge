using PaintDotNet;
using PaintDotNet.Effects;
using System;
using System.Drawing;
using System.Windows.Forms;
using BananaBridge.UI;

namespace BananaBridge
{
	public class AiAssetCreatorToken : EffectConfigToken
	{
		public string ApiKey { get; set; } = string.Empty;
		public string Model { get; set; } = "models/gemini-2.5-flash-image";
		public int[] PixelColors { get; set; } = null;
		public int ImageWidth { get; set; } = 0;
		public int ImageHeight { get; set; } = 0;
		public string SelectionHash { get; set; } = string.Empty;
		public string Prompt { get; set; } = string.Empty;
		public bool ScaleToFit { get; set; } = true;
		public bool CopyToClipboard { get; set; } = true;

		// Saved Dropdown choices
		public string SelectedTab { get; set; } = "Character";
		public string StyleCategory { get; set; } = string.Empty;
		public string StyleName { get; set; } = string.Empty;
		public string SpeciesName { get; set; } = string.Empty;
		public string GenderName { get; set; } = string.Empty;
		public string PersonalityName { get; set; } = string.Empty;
		public string FashionName { get; set; } = string.Empty;
		public string AccessoryName { get; set; } = string.Empty;
		public string PaletteName { get; set; } = string.Empty;
		public string PoseName { get; set; } = string.Empty;
		public string LocationTypeName { get; set; } = string.Empty;
		public string StructureName { get; set; } = string.Empty;
		public string VisualStateName { get; set; } = string.Empty;
		public string AtmosphereName { get; set; } = string.Empty;
		public string PropTypeName { get; set; } = "mysterious artifact";
		public string MaterialName { get; set; } = string.Empty;
		public string ConditionName { get; set; } = string.Empty;
		public bool GenerateMultiView { get; set; } = false;

		public AiAssetCreatorToken()
		{
		}

		protected AiAssetCreatorToken(AiAssetCreatorToken other)
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
			this.ScaleToFit = other.ScaleToFit;
			this.CopyToClipboard = other.CopyToClipboard;

			this.SelectedTab = other.SelectedTab;
			this.StyleCategory = other.StyleCategory;
			this.StyleName = other.StyleName;
			this.SpeciesName = other.SpeciesName;
			this.GenderName = other.GenderName;
			this.PersonalityName = other.PersonalityName;
			this.FashionName = other.FashionName;
			this.AccessoryName = other.AccessoryName;
			this.PaletteName = other.PaletteName;
			this.PoseName = other.PoseName;
			this.LocationTypeName = other.LocationTypeName;
			this.StructureName = other.StructureName;
			this.VisualStateName = other.VisualStateName;
			this.AtmosphereName = other.AtmosphereName;
			this.PropTypeName = other.PropTypeName;
			this.MaterialName = other.MaterialName;
			this.ConditionName = other.ConditionName;
			this.GenerateMultiView = other.GenerateMultiView;
		}

		public override object Clone()
		{
			return new AiAssetCreatorToken(this);
		}
	}

	[EffectCategory(EffectCategory.Effect)]
	public class AiAssetCreatorEffect : Effect<AiAssetCreatorToken>
	{
		public AiAssetCreatorEffect()
			: base("AI Asset Creator",
				   (Image)null,
				   "AI Tools",
				   new EffectOptions { Flags = EffectFlags.Configurable })
		{
		}

		public override EffectConfigDialog CreateConfigDialog()
		{
			return new AiAssetCreatorPaintNetDialog();
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

	public class AiAssetCreatorPaintNetDialog : EffectConfigDialog<AiAssetCreatorEffect, AiAssetCreatorToken>
	{
		private AiAssetCreatorToken _effectToken;

		protected override void InitDialogFromToken(AiAssetCreatorToken effectToken)
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

			using (var uiForm = new BananaBridge.UI.AiAssetCreatorDialog(
				_effectToken.ApiKey,
				_effectToken.Model,
				resultImage,
				_effectToken.Prompt,
				_effectToken.SelectedTab,
				_effectToken.StyleCategory,
				_effectToken.StyleName,
				_effectToken.SpeciesName,
				_effectToken.GenderName,
				_effectToken.PersonalityName,
				_effectToken.FashionName,
				_effectToken.AccessoryName,
				_effectToken.PaletteName,
				_effectToken.PoseName,
				_effectToken.LocationTypeName,
				_effectToken.StructureName,
				_effectToken.VisualStateName,
				_effectToken.AtmosphereName,
				_effectToken.PropTypeName,
				_effectToken.MaterialName,
				_effectToken.ConditionName,
				_effectToken.ScaleToFit,
				_effectToken.CopyToClipboard,
				_effectToken.GenerateMultiView))
			{
				uiForm.SourceCrop = sourceCrop;
				if (uiForm.ShowDialog(this) == DialogResult.OK)
				{
					_effectToken.ApiKey = uiForm.ApiKey;
					_effectToken.Model = uiForm.Model;
					_effectToken.SelectionHash = selectionPixelHash;
					_effectToken.Prompt = uiForm.UserPrompt;
					_effectToken.ScaleToFit = uiForm.ScaleToFit;
					_effectToken.CopyToClipboard = uiForm.CopyToClipboard;

					_effectToken.SelectedTab = uiForm.SelectedTab;
					_effectToken.StyleCategory = uiForm.StyleCategory;
					_effectToken.StyleName = uiForm.StyleName;
					_effectToken.SpeciesName = uiForm.SpeciesName;
					_effectToken.GenderName = uiForm.GenderName;
					_effectToken.PersonalityName = uiForm.PersonalityName;
					_effectToken.FashionName = uiForm.FashionName;
					_effectToken.AccessoryName = uiForm.AccessoryName;
					_effectToken.PaletteName = uiForm.PaletteName;
					_effectToken.PoseName = uiForm.PoseName;
					_effectToken.LocationTypeName = uiForm.LocationTypeName;
					_effectToken.StructureName = uiForm.StructureName;
					_effectToken.VisualStateName = uiForm.VisualStateName;
					_effectToken.AtmosphereName = uiForm.AtmosphereName;
					_effectToken.PropTypeName = uiForm.PropTypeName;
					_effectToken.MaterialName = uiForm.MaterialName;
					_effectToken.ConditionName = uiForm.ConditionName;
					_effectToken.GenerateMultiView = uiForm.GenerateMultiView;

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
					"The generated image has been copied to your Clipboard.\r\n" +
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

		protected override AiAssetCreatorToken CreateInitialToken()
		{
			return new AiAssetCreatorToken();
		}

		protected override void LoadIntoTokenFromDialog(AiAssetCreatorToken effectToken)
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
				effectToken.ScaleToFit = _effectToken.ScaleToFit;
				effectToken.CopyToClipboard = _effectToken.CopyToClipboard;

				effectToken.SelectedTab = _effectToken.SelectedTab;
				effectToken.StyleCategory = _effectToken.StyleCategory;
				effectToken.StyleName = _effectToken.StyleName;
				effectToken.SpeciesName = _effectToken.SpeciesName;
				effectToken.GenderName = _effectToken.GenderName;
				effectToken.PersonalityName = _effectToken.PersonalityName;
				effectToken.FashionName = _effectToken.FashionName;
				effectToken.AccessoryName = _effectToken.AccessoryName;
				effectToken.PaletteName = _effectToken.PaletteName;
				effectToken.PoseName = _effectToken.PoseName;
				effectToken.LocationTypeName = _effectToken.LocationTypeName;
				effectToken.StructureName = _effectToken.StructureName;
				effectToken.VisualStateName = _effectToken.VisualStateName;
				effectToken.AtmosphereName = _effectToken.AtmosphereName;
				effectToken.PropTypeName = _effectToken.PropTypeName;
				effectToken.MaterialName = _effectToken.MaterialName;
				effectToken.ConditionName = _effectToken.ConditionName;
				effectToken.GenerateMultiView = _effectToken.GenerateMultiView;
			}
		}
	}
}
