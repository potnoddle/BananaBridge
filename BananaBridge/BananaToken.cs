using BananaBridge.UI.Models;
using PaintDotNet.Effects;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace BananaBridge
{
	public class BananaToken : EffectConfigToken
	{
		public string ApiKey { get; set; } = string.Empty;
		public string Model { get; set; } = "models/gemini-2.5-flash-image";
		public List<ChatTurn> ChatHistory { get; set; } = new List<ChatTurn>();
		public int[] PixelColors { get; set; } = null;
		public int ImageWidth { get; set; } = 0;
		public int ImageHeight { get; set; } = 0;
		public List<string> AppliedStyles { get; set; } = new List<string>();
		public bool ScaleToFit { get; set; } = true;
		public bool GenerateFromScratch { get; set; } = false;
		public string SelectionHash { get; set; } = string.Empty;

		public BananaToken()
		{
		}

		protected BananaToken(BananaToken other)
			: base(other)
		{
			this.ApiKey = other.ApiKey;
			this.Model = other.Model;
			this.ChatHistory = new List<ChatTurn>(other.ChatHistory);
			if (other.PixelColors != null)
			{
				this.PixelColors = (int[])other.PixelColors.Clone();
			}
			this.ImageWidth = other.ImageWidth;
			this.ImageHeight = other.ImageHeight;
			this.AppliedStyles = new List<string>(other.AppliedStyles);
			this.ScaleToFit = other.ScaleToFit;
			this.GenerateFromScratch = other.GenerateFromScratch;
			this.SelectionHash = other.SelectionHash;
		}

		public override object Clone()
		{
			return new BananaToken(this);
		}

		public static int[] ConvertBitmapToArgbArray(Bitmap bitmap, out int width, out int height)
		{
			width = bitmap.Width;
			height = bitmap.Height;
			int[] argbData = new int[width * height];

			var rect = new Rectangle(0, 0, width, height);
			var bmpData = bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			try
			{
				IntPtr scan0 = bmpData.Scan0;
				for (int y = 0; y < height; y++)
				{
					IntPtr rowPtr = IntPtr.Add(scan0, y * bmpData.Stride);
					System.Runtime.InteropServices.Marshal.Copy(rowPtr, argbData, y * width, width);
				}
			}
			finally
			{
				bitmap.UnlockBits(bmpData);
			}
			return argbData;
		}

		public static Bitmap ConvertArgbArrayToBitmap(int[] argbData, int width, int height)
		{
			Bitmap bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			var rect = new Rectangle(0, 0, width, height);
			var bmpData = bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			try
			{
				IntPtr scan0 = bmpData.Scan0;
				for (int y = 0; y < height; y++)
				{
					IntPtr rowPtr = IntPtr.Add(scan0, y * bmpData.Stride);
					System.Runtime.InteropServices.Marshal.Copy(argbData, y * width, rowPtr, width);
				}
			}
			finally
			{
				bitmap.UnlockBits(bmpData);
			}
			return bitmap;
		}
	}
}


