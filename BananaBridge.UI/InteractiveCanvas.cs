using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BananaBridge.UI
{
	public class InteractiveCanvas : UserControl
	{
		private Bitmap _sourceImage;
		private Bitmap _maskImage;
		private float _zoomScale = 1.0f;
		private float _offsetX = 0;
		private float _offsetY = 0;
		private bool _isPanning = false;
		private Point _lastMousePos;
		private bool _isDrawing = false;
		private bool _isBrushMode = true; // true = Paint (white), false = Erase (black)
		private int _brushSize = 20;
		private Color _brushColor = Color.White;

		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public Bitmap SourceImage
		{
			get => _sourceImage;
			set
			{
				_sourceImage = value;
				ZoomFit();
			}
		}

		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public Bitmap MaskImage
		{
			get => _maskImage;
			set
			{
				_maskImage = value;
				Invalidate();
			}
		}

		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public bool IsBrushMode
		{
			get => _isBrushMode;
			set => _isBrushMode = value;
		}

		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public int BrushSize
		{
			get => _brushSize;
			set => _brushSize = value;
		}

		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public Color BrushColor
		{
			get => _brushColor;
			set => _brushColor = value;
		}

		public InteractiveCanvas()
		{
			this.DoubleBuffered = true;
			this.BackColor = Color.FromArgb(50, 50, 50);
		}

		public void ZoomFit()
		{
			if (_sourceImage == null) return;

			float scaleX = (float)Width / _sourceImage.Width;
			float scaleY = (float)Height / _sourceImage.Height;
			_zoomScale = Math.Min(scaleX, scaleY) * 0.95f;
			if (_zoomScale <= 0) _zoomScale = 1.0f;

			_offsetX = (Width - _sourceImage.Width * _zoomScale) / 2f;
			_offsetY = (Height - _sourceImage.Height * _zoomScale) / 2f;

			Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			if (_sourceImage == null) return;

			Graphics g = e.Graphics;
			g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
			g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

			// Save current transform
			var originalTransform = g.Transform;

			// Apply zoom and offset
			g.TranslateTransform(_offsetX, _offsetY);
			g.ScaleTransform(_zoomScale, _zoomScale);

			// Draw source image
			g.DrawImage(_sourceImage, 0, 0);

			// Draw translucent mask overlay
			if (_maskImage != null)
			{
				using (var ia = new System.Drawing.Imaging.ImageAttributes())
				{
					// Color matrix that preserves colors and renders with 40% opacity (translucent)
					var colorMatrix = new System.Drawing.Imaging.ColorMatrix(new float[][]
					{
						new float[] { 1, 0, 0, 0, 0 },
						new float[] { 0, 1, 0, 0, 0 },
						new float[] { 0, 0, 1, 0, 0 },
						new float[] { 0, 0, 0, 0.4f, 0 }, // Scale Alpha to 40%
						new float[] { 0, 0, 0, 0, 1 }
					});
					ia.SetColorMatrix(colorMatrix);

					g.DrawImage(_maskImage, new Rectangle(0, 0, _sourceImage.Width, _sourceImage.Height),
						0, 0, _maskImage.Width, _maskImage.Height, GraphicsUnit.Pixel, ia);
				}
			}

			// Restore transform
			g.Transform = originalTransform;
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (_sourceImage == null) return;

			if (e.Button == MouseButtons.Right || e.Button == MouseButtons.Middle)
			{
				_isPanning = true;
				_lastMousePos = e.Location;
				Cursor = Cursors.Hand;
			}
			else if (e.Button == MouseButtons.Left)
			{
				_isDrawing = true;
				_lastMousePos = e.Location;
				PaintOnMask(e.Location, e.Location);
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (_sourceImage == null) return;

			if (_isPanning)
			{
				float dx = e.X - _lastMousePos.X;
				float dy = e.Y - _lastMousePos.Y;
				_offsetX += dx;
				_offsetY += dy;
				_lastMousePos = e.Location;
				Invalidate();
			}
			else if (_isDrawing && e.Button == MouseButtons.Left)
			{
				PaintOnMask(_lastMousePos, e.Location);
				_lastMousePos = e.Location;
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			_isPanning = false;
			_isDrawing = false;
			Cursor = Cursors.Default;
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			if (_sourceImage == null) return;

			// Zoom centered on mouse cursor
			float oldScale = _zoomScale;
			float zoomFactor = 1.1f;
			if (e.Delta < 0)
			{
				_zoomScale /= zoomFactor;
			}
			else
			{
				_zoomScale *= zoomFactor;
			}

			// Clamp zoom scale
			_zoomScale = Math.Max(0.05f, Math.Min(100.0f, _zoomScale));

			// Adjust offsets so mouse cursor remains over the same image point
			float mouseImgX = (e.X - _offsetX) / oldScale;
			float mouseImgY = (e.Y - _offsetY) / oldScale;

			_offsetX = e.X - mouseImgX * _zoomScale;
			_offsetY = e.Y - mouseImgY * _zoomScale;

			Invalidate();
		}

		private void PaintOnMask(Point fromPoint, Point toPoint)
		{
			if (_maskImage == null || _sourceImage == null) return;

			// Map control points to image coordinates
			float fromImgX = (fromPoint.X - _offsetX) / _zoomScale;
			float fromImgY = (fromPoint.Y - _offsetY) / _zoomScale;
			float toImgX = (toPoint.X - _offsetX) / _zoomScale;
			float toImgY = (toPoint.Y - _offsetY) / _zoomScale;

			// Paint on the mask bitmap
			using (Graphics g = Graphics.FromImage(_maskImage))
			{
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

				if (_isBrushMode)
				{
					g.CompositingMode = CompositingMode.SourceOver;
					using (Pen pen = new Pen(_brushColor, _brushSize))
					{
						pen.StartCap = LineCap.Round;
						pen.EndCap = LineCap.Round;
						pen.LineJoin = LineJoin.Round;
						g.DrawLine(pen, fromImgX, fromImgY, toImgX, toImgY);
					}
					using (Brush brush = new SolidBrush(_brushColor))
					{
						float radius = _brushSize / 2f;
						g.FillEllipse(brush, toImgX - radius, toImgY - radius, _brushSize, _brushSize);
					}
				}
				else
				{
					// Eraser mode: clear back to transparent
					g.CompositingMode = CompositingMode.SourceCopy;
					using (Pen pen = new Pen(Color.Transparent, _brushSize))
					{
						pen.StartCap = LineCap.Round;
						pen.EndCap = LineCap.Round;
						pen.LineJoin = LineJoin.Round;
						g.DrawLine(pen, fromImgX, fromImgY, toImgX, toImgY);
					}
					float radius = _brushSize / 2f;
					g.FillEllipse(Brushes.Transparent, toImgX - radius, toImgY - radius, _brushSize, _brushSize);
				}
			}

			Invalidate();
		}
	}
}

