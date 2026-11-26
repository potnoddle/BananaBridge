using System;
using System.IO;

namespace BananaBridge.UI
{
	public static class Logger
	{
		private static readonly string LogFilePath = Path.Combine(
			Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
			"BananaBridge_debug_log.txt"
		);

		public static string FilePath => LogFilePath;

		public static void Log(string message)
		{
			try
			{
				string dir = Path.GetDirectoryName(LogFilePath);
				if (!Directory.Exists(dir))
				{
					Directory.CreateDirectory(dir);
				}
				string logLine = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {message}{Environment.NewLine}";
				File.AppendAllText(LogFilePath, logLine);
				System.Diagnostics.Debug.Write(logLine);
			}
			catch
			{
				System.Diagnostics.Debug.WriteLine($"[LOGGER ERROR] {message}");
			}
		}

		public static void Clear()
		{
			try
			{
				if (File.Exists(LogFilePath))
				{
					File.Delete(LogFilePath);
				}
			}
			catch { }
		}
	}
}

