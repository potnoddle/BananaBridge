using PaintDotNet;
using PaintDotNet.Effects;
using System;
using System.Drawing;

namespace BananaBridge
{
    public class BananaEffect : Effect<BananaToken>
    {
        public BananaEffect()
            : base("Banana AI Editor",
                   (Image)null,
                   "AI Tools", // Submenu Category name in Paint.NET Effects menu
                   new EffectOptions { Flags = EffectFlags.Configurable })
        {
        }

        public override EffectConfigDialog CreateConfigDialog()
        {
            return new BananaPaintNetDialog();
        }

        protected override void OnRender(Rectangle[] renderRects, int startIndex, int length)
        {
            if (length == 0) return;

            var token = this.Token;
            
            // If no result image has been generated yet, copy source to destination unmodified
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
                        // Check if pixel is within active selection bounds and selected
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
                            // Keep unselected pixels unmodified
                            DstArgs.Surface[x, y] = SrcArgs.Surface[x, y];
                        }
                    }
                }
            }
        }
    }
}

