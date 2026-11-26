using System;

namespace BananaBridge.UI
{
    public static class AiClientFactory
    {
        public static IAiClient CreateClient(string providerName)
        {
            if (string.IsNullOrEmpty(providerName) || providerName.Equals("Gemini", StringComparison.OrdinalIgnoreCase))
            {
                return new GeminiClient();
            }
            
            throw new NotSupportedException($"AI Provider '{providerName}' is not supported.");
        }
    }
}

