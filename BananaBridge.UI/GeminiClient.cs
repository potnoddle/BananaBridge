using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BananaBridge.UI.Models;

namespace BananaBridge.UI
{
    public class GeminiClient : IAiClient
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        public async Task<Bitmap> SendEditRequestAsync(
            string apiKey,
            string model,
            List<ChatTurn> history,
            Bitmap sourceImage,
            Bitmap maskImage,
            string newPrompt,
            string aspectRatio = null)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentException("API Key is required.");
            }

            // Prepare the URL
            // Default model if empty
            if (string.IsNullOrEmpty(model))
            {
                model = "models/gemini-2.5-flash-image";
            }

            // Ensure the model path is correct
            string modelName = model;
            if (!modelName.StartsWith("models/"))
            {
                modelName = "models/" + modelName;
            }

            string url = $"https://generativelanguage.googleapis.com/v1beta/{modelName}:generateContent?key={apiKey}";

            // Encode images to base64
            string sourceBase64 = sourceImage != null ? ImageToBase64Png(sourceImage) : null;
            string maskBase64 = maskImage != null ? ImageToBase64Png(maskImage) : null;

            // Build request json
            string jsonPayload = BuildRequestJson(history, sourceBase64, maskBase64, newPrompt, aspectRatio);
            string sanitizedPayloadLog = BuildSanitizedRequestJson(history, sourceBase64, maskBase64, newPrompt, aspectRatio);

            Logger.Log($"Sending API Request to model: '{modelName}'");
            Logger.Log($"API Request Payload (Sanitized):\n{sanitizedPayloadLog}");

            using (var requestContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json"))
            {
                var response = await HttpClient.PostAsync(url, requestContent);
                string responseBody = await response.Content.ReadAsStringAsync();

                Logger.Log($"API Response status: {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    Logger.Log($"API Request Failed. Error Details:\n{responseBody}");
                    throw new Exception($"Gemini API error ({response.StatusCode}): {responseBody}");
                }

                return ParseImageFromResponse(responseBody);
            }
        }

        public static string BuildRequestJson(
            List<ChatTurn> history,
            string sourceBase64,
            string maskBase64,
            string newPrompt,
            string aspectRatio = null)
        {
            var currentParts = new List<object>();
            currentParts.Add(new { text = newPrompt });

            if (sourceBase64 != null)
            {
                currentParts.Add(new
                {
                    inlineData = new
                    {
                        mimeType = "image/png",
                        data = sourceBase64
                    }
                });
            }

            if (maskBase64 != null)
            {
                currentParts.Add(new
                {
                    inlineData = new
                    {
                        mimeType = "image/png",
                        data = maskBase64
                    }
                });
            }

            var contents = new List<object>();
            foreach (var turn in history)
            {
                var partsList = new List<object>();
                foreach (var part in turn.Parts)
                {
                    if (!string.IsNullOrEmpty(part.Text))
                    {
                        partsList.Add(new { text = part.Text });
                    }
                    if (part.InlineData != null)
                    {
                        partsList.Add(new
                        {
                            inlineData = new
                            {
                                mimeType = part.InlineData.MimeType,
                                data = part.InlineData.Data
                            }
                        });
                    }
                }
                contents.Add(new
                {
                    role = turn.Role,
                    parts = partsList
                });
            }

            contents.Add(new
            {
                role = "user",
                parts = currentParts
            });

            var generationConfig = new Dictionary<string, object>
            {
                { "temperature", 0.4 }
            };

            if (!string.IsNullOrEmpty(aspectRatio))
            {
                generationConfig["imageConfig"] = new Dictionary<string, object>
                {
                    { "aspectRatio", aspectRatio }
                };
            }

            var payload = new
            {
                contents = contents,
                generationConfig = generationConfig
            };

            return JsonSerializer.Serialize(payload);
        }

        public static string BuildSanitizedRequestJson(
            List<ChatTurn> history,
            string sourceBase64,
            string maskBase64,
            string newPrompt,
            string aspectRatio = null)
        {
            string cleanSource = sourceBase64 != null ? $"[Base64 Image Data - Length: {sourceBase64.Length}]" : null;
            string cleanMask = maskBase64 != null ? $"[Base64 Image Data - Length: {maskBase64.Length}]" : null;

            var currentParts = new List<object>();
            currentParts.Add(new { text = newPrompt });

            if (cleanSource != null)
            {
                currentParts.Add(new
                {
                    inlineData = new
                    {
                        mimeType = "image/png",
                        data = cleanSource
                    }
                });
            }

            if (cleanMask != null)
            {
                currentParts.Add(new
                {
                    inlineData = new
                    {
                        mimeType = "image/png",
                        data = cleanMask
                    }
                });
            }

            var contents = new List<object>();
            foreach (var turn in history)
            {
                var partsList = new List<object>();
                foreach (var part in turn.Parts)
                {
                    if (!string.IsNullOrEmpty(part.Text))
                    {
                        partsList.Add(new { text = part.Text });
                    }
                    if (part.InlineData != null)
                    {
                        string dataPlaceholder = $"[Base64 Image Data - Length: {part.InlineData.Data?.Length ?? 0}]";
                        partsList.Add(new
                        {
                            inlineData = new
                            {
                                mimeType = part.InlineData.MimeType,
                                data = dataPlaceholder
                            }
                        });
                    }
                }
                contents.Add(new
                {
                    role = turn.Role,
                    parts = partsList
                });
            }

            contents.Add(new
            {
                role = "user",
                parts = currentParts
            });

            var generationConfig = new Dictionary<string, object>
            {
                { "temperature", 0.4 }
            };

            if (!string.IsNullOrEmpty(aspectRatio))
            {
                generationConfig["imageConfig"] = new Dictionary<string, object>
                {
                    { "aspectRatio", aspectRatio }
                };
            }

            var payload = new
            {
                contents = contents,
                generationConfig = generationConfig
            };

            return JsonSerializer.Serialize(payload, new JsonSerializerOptions { WriteIndented = true });
        }

        private static string ImageToBase64Png(Bitmap bitmap)
        {
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        private static Bitmap ParseImageFromResponse(string responseJson)
        {
            var textParts = new List<string>();
            try
            {
                using (JsonDocument doc = JsonDocument.Parse(responseJson))
                {
                    JsonElement root = doc.RootElement;
                    if (root.TryGetProperty("candidates", out JsonElement candidates) && candidates.ValueKind == JsonValueKind.Array && candidates.GetArrayLength() > 0)
                    {
                        JsonElement firstCandidate = candidates[0];
                        if (firstCandidate.TryGetProperty("content", out JsonElement content))
                        {
                            if (content.TryGetProperty("parts", out JsonElement parts) && parts.ValueKind == JsonValueKind.Array)
                            {
                                foreach (JsonElement part in parts.EnumerateArray())
                                {
                                    // 1. Check direct inlineData
                                    if (part.TryGetProperty("inlineData", out JsonElement inlineData))
                                    {
                                        if (inlineData.TryGetProperty("data", out JsonElement data))
                                        {
                                            string base64 = data.GetString();
                                            if (!string.IsNullOrEmpty(base64))
                                            {
                                                return Base64ToBitmap(base64);
                                            }
                                        }
                                    }

                                    // 2. Check if there's text
                                    if (part.TryGetProperty("text", out JsonElement textProp))
                                    {
                                        string text = textProp.GetString();
                                        if (!string.IsNullOrEmpty(text))
                                        {
                                            // Save the text to report to user if no image found
                                            textParts.Add(text);

                                            // Extract base64 if it's wrapped in markdown or is pure base64
                                            string base64 = CleanBase64(text);
                                            if (!string.IsNullOrEmpty(base64))
                                            {
                                                try
                                                {
                                                    return Base64ToBitmap(base64);
                                                }
                                                catch
                                                {
                                                    // Continue checking other parts if base64 conversion fails
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to parse API response: {ex.Message}. Raw response: {responseJson}", ex);
            }

            if (textParts.Count > 0)
            {
                string combinedText = string.Join("\n", textParts).Trim();
                throw new Exception($"The model returned a text response instead of an image: \"{combinedText}\". Please make sure you are using an image-generation/editing capable model (e.g., gemini-2.5-flash-image).");
            }

            throw new Exception($"No image data found in response. Raw response: {responseJson}");
        }

        private static string CleanBase64(string text)
        {
            text = text.Trim();
            // Remove markdown codeblock wrapper if present
            if (text.StartsWith("```"))
            {
                int firstLineEnd = text.IndexOf('\n');
                if (firstLineEnd != -1)
                {
                    text = text.Substring(firstLineEnd).Trim();
                }
                if (text.EndsWith("```"))
                {
                    text = text.Substring(0, text.Length - 3).Trim();
                }
            }

            // Remove data URI scheme prefix if present
            if (text.StartsWith("data:image", StringComparison.OrdinalIgnoreCase))
            {
                int commaIndex = text.IndexOf(',');
                if (commaIndex != -1)
                {
                    text = text.Substring(commaIndex + 1).Trim();
                }
            }

            // Simple validation of base64 chars
            text = text.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            return text;
        }

        private static Bitmap Base64ToBitmap(string base64)
        {
            byte[] bytes = Convert.FromBase64String(base64);
            using (var ms = new MemoryStream(bytes))
            {
                return new Bitmap(ms);
            }
        }
        public async Task<List<string>> ListModelsAsync(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentException("API Key is required.");
            }

            string url = $"https://generativelanguage.googleapis.com/v1beta/models?key={apiKey}";
            var response = await HttpClient.GetAsync(url);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Gemini API error ({response.StatusCode}): {responseBody}");
            }

            var modelsList = new List<string>();
            using (JsonDocument doc = JsonDocument.Parse(responseBody))
            {
                JsonElement root = doc.RootElement;
                if (root.TryGetProperty("models", out JsonElement models) && models.ValueKind == JsonValueKind.Array)
                {
                    foreach (JsonElement m in models.EnumerateArray())
                    {
                        bool supportsGenerate = false;
                        JsonElement actionsOrMethods;
                        if (m.TryGetProperty("supportedGenerationMethods", out actionsOrMethods) ||
                            m.TryGetProperty("supportedActions", out actionsOrMethods))
                        {
                            if (actionsOrMethods.ValueKind == JsonValueKind.Array)
                            {
                                foreach (JsonElement action in actionsOrMethods.EnumerateArray())
                                {
                                    if (action.GetString() == "generateContent")
                                    {
                                        supportsGenerate = true;
                                        break;
                                    }
                                }
                            }
                        }

                        if (supportsGenerate && m.TryGetProperty("name", out JsonElement nameProp))
                        {
                            string name = nameProp.GetString();
                            if (!string.IsNullOrEmpty(name))
                            {
                                // Strip "models/" prefix if present
                                if (name.StartsWith("models/"))
                                {
                                    name = name.Substring(7);
                                }
                                modelsList.Add(name);
                            }
                        }
                    }
                }
            }
            return modelsList;
        }

        public static string GetClosestAspectRatio(int width, int height)
        {
            if (width <= 0 || height <= 0) return "1:1";
            double ratio = (double)width / height;

            // Supported ratios:
            // 9:16 = 0.5625
            // 3:4  = 0.75
            // 1:1  = 1.0
            // 4:3  = 1.3333333333333333
            // 16:9 = 1.7777777777777777

            double[] targetRatios = { 0.5625, 0.75, 1.0, 1.3333333333333333, 1.7777777777777777 };
            string[] ratioStrings = { "9:16", "3:4", "1:1", "4:3", "16:9" };

            int bestIndex = 0;
            double minDiff = double.MaxValue;
            for (int i = 0; i < targetRatios.Length; i++)
            {
                double diff = Math.Abs(ratio - targetRatios[i]);
                if (diff < minDiff)
                {
                    minDiff = diff;
                    bestIndex = i;
                }
            }
            return ratioStrings[bestIndex];
        }
    }
}

