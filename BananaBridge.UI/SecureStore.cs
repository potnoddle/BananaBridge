using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BananaBridge.UI
{
    public static class SecureStore
    {
        private static readonly string AppDataFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "BananaBridge"
        );
        private static readonly string KeyFilePath = Path.Combine(AppDataFolder, "api.key");
        private static readonly string ProviderFilePath = Path.Combine(AppDataFolder, "provider.txt");

        public static string LoadProvider()
        {
            try
            {
                if (File.Exists(ProviderFilePath))
                {
                    return File.ReadAllText(ProviderFilePath).Trim();
                }
            }
            catch {}
            return "Gemini";
        }

        public static void SaveProvider(string provider)
        {
            try
            {
                if (!Directory.Exists(AppDataFolder))
                {
                    Directory.CreateDirectory(AppDataFolder);
                }
                File.WriteAllText(ProviderFilePath, provider);
            }
            catch {}
        }

        public static void SaveApiKey(string apiKey)
        {
            try
            {
                if (!Directory.Exists(AppDataFolder))
                {
                    Directory.CreateDirectory(AppDataFolder);
                }

                if (string.IsNullOrEmpty(apiKey))
                {
                    if (File.Exists(KeyFilePath))
                    {
                        File.Delete(KeyFilePath);
                    }
                    return;
                }

                byte[] plainBytes = Encoding.UTF8.GetBytes(apiKey);
                byte[] encryptedBytes = ProtectedData.Protect(
                    plainBytes,
                    null,
                    DataProtectionScope.CurrentUser
                );

                File.WriteAllBytes(KeyFilePath, encryptedBytes);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to save API key: {ex.Message}");
            }
        }

        public static string LoadApiKey()
        {
            try
            {
                if (!File.Exists(KeyFilePath))
                {
                    return string.Empty;
                }

                byte[] encryptedBytes = File.ReadAllBytes(KeyFilePath);
                byte[] plainBytes = ProtectedData.Unprotect(
                    encryptedBytes,
                    null,
                    DataProtectionScope.CurrentUser
                );

                return Encoding.UTF8.GetString(plainBytes);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load API key: {ex.Message}");
                return string.Empty;
            }
        }
    }
}

