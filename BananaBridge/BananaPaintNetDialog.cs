using PaintDotNet;
using PaintDotNet.Effects;
using System;
using System.Drawing;
using System.Windows.Forms;
using BananaBridge.UI;
using BananaBridge.UI.Models;

namespace BananaBridge
{
    public class BananaPaintNetDialog : EffectConfigDialog<BananaEffect, BananaToken>
    {
        public BananaPaintNetDialog()
        {
            // Set size or options if needed, but the child form handles layout.
        }

        private BananaToken _effectToken;

        protected override void InitDialogFromToken(BananaToken effectToken)
        {
            _effectToken = effectToken;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            // Hide the wrapper dialog window
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

            // Clear old result and history if selection has changed
            if (string.IsNullOrEmpty(_effectToken.SelectionHash) || _effectToken.SelectionHash != selectionPixelHash)
            {
                _effectToken.SelectionHash = selectionPixelHash;
                _effectToken.PixelColors = null;
                _effectToken.ImageWidth = 0;
                _effectToken.ImageHeight = 0;
                _effectToken.ChatHistory = new System.Collections.Generic.List<BananaBridge.UI.Models.ChatTurn>();
                _effectToken.AppliedStyles = new System.Collections.Generic.List<string>();
            }

            Bitmap resultImage = null;
            if (_effectToken.PixelColors != null && _effectToken.ImageWidth > 0 && _effectToken.ImageHeight > 0)
            {
                resultImage = BananaToken.ConvertArgbArrayToBitmap(_effectToken.PixelColors, _effectToken.ImageWidth, _effectToken.ImageHeight);
            }

            using (var uiForm = new BananaBridge.UI.BananaDialog(
                _effectToken.ApiKey,
                _effectToken.Model,
                sourceCrop,
                maskCrop,
                selectionPixelHash,
                resultImage,
                _effectToken.ChatHistory,
                _effectToken.AppliedStyles,
                _effectToken.ScaleToFit,
                _effectToken.GenerateFromScratch))
            {
                if (uiForm.ShowDialog(this) == DialogResult.OK)
                {
                    _effectToken.ApiKey = uiForm.ApiKey;
                    _effectToken.Model = uiForm.Model;
                    _effectToken.SelectionHash = selectionPixelHash;
                    
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

                    _effectToken.ChatHistory = uiForm.ChatHistory;
                    _effectToken.AppliedStyles = uiForm.AppliedStyles;
                    _effectToken.ScaleToFit = uiForm.ScaleToFit;
                    _effectToken.GenerateFromScratch = uiForm.GenerateFromScratch;

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

        protected override BananaToken CreateInitialToken()
        {
            return new BananaToken();
        }

        protected override void LoadIntoTokenFromDialog(BananaToken effectToken)
        {
            if (_effectToken != null)
            {
                effectToken.ApiKey = _effectToken.ApiKey;
                effectToken.Model = _effectToken.Model;
                effectToken.PixelColors = _effectToken.PixelColors != null ? (int[])_effectToken.PixelColors.Clone() : null;
                effectToken.ImageWidth = _effectToken.ImageWidth;
                effectToken.ImageHeight = _effectToken.ImageHeight;
                effectToken.ChatHistory = _effectToken.ChatHistory;
                effectToken.AppliedStyles = _effectToken.AppliedStyles;
                effectToken.ScaleToFit = _effectToken.ScaleToFit;
                effectToken.GenerateFromScratch = _effectToken.GenerateFromScratch;
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
    }
}

