using System;
using System.IO;
using System.Reflection;

namespace BananaBridge.UI
{
	/// <summary>
	/// Service that resolves configuration JSON files by prioritizing custom overrides
	/// on disk before falling back to default embedded resources.
	/// </summary>
	public static class ConfigService
	{
		private static readonly Assembly UIAssembly = typeof(ConfigService).Assembly;

		/// <summary>
		/// Normalizes the configuration filename by stripping any namespace prefixes.
		/// </summary>
		public static string NormalizeFilename(string filename)
		{
			if (string.IsNullOrWhiteSpace(filename))
			{
				return string.Empty;
			}

			if (filename.StartsWith("BananaBridge.UI.", StringComparison.OrdinalIgnoreCase))
			{
				filename = filename.Substring("BananaBridge.UI.".Length);
			}

			return Path.GetFileName(filename);
		}

		/// <summary>
		/// Returns an open Stream for the requested configuration file.
		/// Checks external override folders on disk first, then falls back to assembly embedded resources.
		/// </summary>
		public static Stream GetConfigStream(string filename)
		{
			string cleanName = NormalizeFilename(filename);
			if (string.IsNullOrEmpty(cleanName))
			{
				return null;
			}

			try
			{
				// 1. Check directory next to the running assembly (Paint.NET Effects folder)
				string asmLocation = UIAssembly.Location;
				if (!string.IsNullOrEmpty(asmLocation))
				{
					string asmDir = Path.GetDirectoryName(asmLocation);
					if (!string.IsNullOrEmpty(asmDir))
					{
						// Check JsonConfigs.Override folder next to assembly
						string overridePath = Path.Combine(asmDir, "JsonConfigs.Override", cleanName);
						if (File.Exists(overridePath))
						{
							return File.OpenRead(overridePath);
						}

						// Check JsonConfigs folder next to assembly
						string installedPath = Path.Combine(asmDir, "JsonConfigs", cleanName);
						if (File.Exists(installedPath))
						{
							return File.OpenRead(installedPath);
						}

						// Check directly in assembly folder
						string directPath = Path.Combine(asmDir, cleanName);
						if (File.Exists(directPath))
						{
							return File.OpenRead(directPath);
						}
					}
				}
			}
			catch { }

			try
			{
				// 2. Check roaming ApplicationData (e.g. %APPDATA%\BananaBridge\JsonConfigs\)
				string appDataDir = Path.Combine(
					Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
					"BananaBridge",
					"JsonConfigs"
				);
				string appDataPath = Path.Combine(appDataDir, cleanName);
				if (File.Exists(appDataPath))
				{
					return File.OpenRead(appDataPath);
				}
			}
			catch { }

			try
			{
				// 3. Fallback to embedded resource within BananaBridge.UI.dll
				string resourceName = "BananaBridge.UI." + cleanName;
				Stream resourceStream = UIAssembly.GetManifestResourceStream(resourceName);
				if (resourceStream != null)
				{
					return resourceStream;
				}
			}
			catch { }

			return null;
		}

		/// <summary>
		/// Checks whether a given configuration resource is available (either on disk or embedded).
		/// </summary>
		public static bool ConfigExists(string filename)
		{
			using (Stream stream = GetConfigStream(filename))
			{
				return stream != null;
			}
		}

		/// <summary>
		/// Reads the full text of the configuration file.
		/// </summary>
		public static string GetConfigText(string filename)
		{
			using (Stream stream = GetConfigStream(filename))
			{
				if (stream == null)
				{
					return null;
				}

				using (StreamReader reader = new StreamReader(stream))
				{
					return reader.ReadToEnd();
				}
			}
		}
	}
}
