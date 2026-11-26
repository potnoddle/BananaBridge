using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;
using BananaBridge.UI;
using BananaBridge.UI.Models;
using Xunit;

namespace BananaBridge.Tests
{
	public class StyleRestylingTests
	{
		[Theory]
		[InlineData(500, 500, "1:1")]
		[InlineData(300, 400, "3:4")]
		[InlineData(400, 300, "4:3")]
		[InlineData(900, 1600, "9:16")]
		[InlineData(1600, 900, "16:9")]
		[InlineData(640, 480, "4:3")]
		[InlineData(1920, 1080, "16:9")]
		public void TestGetClosestAspectRatio_ShouldReturnClosestMatch(int width, int height, string expectedRatio)
		{
			// Act
			string actualRatio = GeminiClient.GetClosestAspectRatio(width, height);

			// Assert
			Assert.Equal(expectedRatio, actualRatio);
		}

		[Fact]
		public void TestStyleItemDeserialization_ShouldPopulateImagePrompt()
		{
			// Arrange
			string json = @"[
				{
					""id"": ""f1b920a1"",
					""name"": ""Victorian Chromolithography"",
					""description"": ""An authentic 19th-century style."",
					""image_prompt"": ""A lush Victorian Chromolithograph image prompt.""
				}
			]";

			// Act
			var styles = JsonSerializer.Deserialize<List<StyleItem>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

			// Assert
			Assert.NotNull(styles);
			Assert.Single(styles);
			Assert.Equal("f1b920a1", styles[0].Id);
			Assert.Equal("Victorian Chromolithography", styles[0].Name);
			Assert.Equal("A lush Victorian Chromolithograph image prompt.", styles[0].ImagePrompt);
		}

		[Fact]
		public void TestPromptFormatting_WithStyleSelected()
		{
			// Arrange
			var style = new StyleItem
			{
				Id = "f1b920a1",
				Name = "Victorian Chromolithography",
				ImagePrompt = "A lush Victorian Chromolithograph style details."
			};
			string userPrompt = "Add a banana";

			// Act
			string finalPrompt = $"Style: {style.ImagePrompt}\r\nUser prompt: {userPrompt}";

			// Assert
			string expected = "Style: A lush Victorian Chromolithograph style details.\r\nUser prompt: Add a banana";
			Assert.Equal(expected, finalPrompt);
		}

		[Fact]
		public void TestBuildRequestJson_IncludesRestylingPromptAndImages()
		{
			// Arrange
			var history = new List<ChatTurn>();
			string sourceBase64 = "iVBORw0KGgoAAAANSUhEUgAA";
			string maskBase64 = "iVBORw0KGgoAAAANSUhEUgAB";
			string restyledPrompt = "Style: A lush Victorian Chromolithograph...\r\nUser prompt: Add a banana";

			// Act
			string jsonPayload = GeminiClient.BuildRequestJson(history, sourceBase64, maskBase64, restyledPrompt);

			// Assert
			using (JsonDocument doc = JsonDocument.Parse(jsonPayload))
			{
				JsonElement root = doc.RootElement;
				Assert.True(root.TryGetProperty("contents", out JsonElement contents));
				Assert.Equal(JsonValueKind.Array, contents.ValueKind);
				Assert.Equal(1, contents.GetArrayLength());

				JsonElement userTurn = contents[0];
				Assert.True(userTurn.TryGetProperty("role", out JsonElement role));
				Assert.Equal("user", role.GetString());

				Assert.True(userTurn.TryGetProperty("parts", out JsonElement parts));
				Assert.Equal(JsonValueKind.Array, parts.ValueKind);
				Assert.Equal(3, parts.GetArrayLength());

				// Part 1: Prompt Text
				Assert.True(parts[0].TryGetProperty("text", out JsonElement textProp));
				Assert.Contains("Style:", textProp.GetString());
				Assert.Contains("User prompt:", textProp.GetString());

				// Part 2: Source Image
				Assert.True(parts[1].TryGetProperty("inlineData", out JsonElement sourceInline));
				Assert.Equal("image/png", sourceInline.GetProperty("mimeType").GetString());
				Assert.Equal(sourceBase64, sourceInline.GetProperty("data").GetString());

				// Part 3: Mask Image
				Assert.True(parts[2].TryGetProperty("inlineData", out JsonElement maskInline));
				Assert.Equal("image/png", maskInline.GetProperty("mimeType").GetString());
				Assert.Equal(maskBase64, maskInline.GetProperty("data").GetString());
			}
		}

		[Fact]
		public void TestExtractRawPrompt_WithStyledAndPlainPrompts()
		{
			// Arrange
			string plainPrompt = "Make the background blue";
			string styledPrompt = "Style: A detailed anime art style.\r\nUser prompt: Make the background blue";

			// Act
			string extractedPlain = BananaDialog.ExtractRawPrompt(plainPrompt);
			string extractedStyled = BananaDialog.ExtractRawPrompt(styledPrompt);

			// Assert
			Assert.Equal("Make the background blue", extractedPlain);
			Assert.Equal("Make the background blue", extractedStyled);
		}

		[Fact]
		public void TestDrawBoundaryOutline_ShouldColorSubjectEdgesOnly()
		{
			// Arrange
			using (Bitmap target = new Bitmap(10, 10, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
			using (Bitmap mask = new Bitmap(10, 10, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
			{
				// Set a 4x4 white square in the center of the mask (x: 3..6, y: 3..6)
				for (int y = 0; y < 10; y++)
				{
					for (int x = 0; x < 10; x++)
					{
						if (x >= 3 && x <= 6 && y >= 3 && y <= 6)
						{
							mask.SetPixel(x, y, Color.White);
						}
						else
						{
							mask.SetPixel(x, y, Color.Black);
						}
						target.SetPixel(x, y, Color.Transparent);
					}
				}

				// Act - Draw outline with radius 1
				AiIsolatorDialog.DrawBoundaryOutline(target, mask, Color.Red, 1);

				// Assert
				// Check that edge pixels of the subject are red
				Assert.Equal(Color.Red.ToArgb(), target.GetPixel(3, 3).ToArgb());
				Assert.Equal(Color.Red.ToArgb(), target.GetPixel(6, 6).ToArgb());
				Assert.Equal(Color.Red.ToArgb(), target.GetPixel(3, 5).ToArgb());
				Assert.Equal(Color.Red.ToArgb(), target.GetPixel(5, 6).ToArgb());

				// Check that center pixels (x=4..5, y=4..5) are not colored
				Assert.Equal(Color.Transparent.ToArgb(), target.GetPixel(4, 4).ToArgb());
				Assert.Equal(Color.Transparent.ToArgb(), target.GetPixel(4, 5).ToArgb());
				Assert.Equal(Color.Transparent.ToArgb(), target.GetPixel(5, 4).ToArgb());
				Assert.Equal(Color.Transparent.ToArgb(), target.GetPixel(5, 5).ToArgb());

				// Check that background pixels (outside x=3..6, y=3..6) are not colored
				Assert.Equal(Color.Transparent.ToArgb(), target.GetPixel(0, 0).ToArgb());
				Assert.Equal(Color.Transparent.ToArgb(), target.GetPixel(2, 2).ToArgb());
				Assert.Equal(Color.Transparent.ToArgb(), target.GetPixel(7, 7).ToArgb());
			}
		}

		[Fact]
		public void TestFillInternalHoles_ShouldFillInnerHolesOnly()
		{
			// Arrange
			using (Bitmap mask = new Bitmap(8, 8, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
			{
				for (int y = 0; y < 8; y++)
				{
					for (int x = 0; x < 8; x++)
					{
						// Outer boundary: black
						if (x == 0 || x == 7 || y == 0 || y == 7)
						{
							mask.SetPixel(x, y, Color.Black);
						}
						// Inner hole (3..4, 3..4): black
						else if (x >= 3 && x <= 4 && y >= 3 && y <= 4)
						{
							mask.SetPixel(x, y, Color.Black);
						}
						// Rest of subject: white
						else
						{
							mask.SetPixel(x, y, Color.White);
						}
					}
				}

				// Act
				AiIsolatorDialog.FillInternalHoles(mask);

				// Assert
				// The outer boundary should remain black (0, 0, 0)
				Assert.Equal(Color.Black.ToArgb(), mask.GetPixel(0, 0).ToArgb());
				Assert.Equal(Color.Black.ToArgb(), mask.GetPixel(7, 7).ToArgb());
				Assert.Equal(Color.Black.ToArgb(), mask.GetPixel(0, 4).ToArgb());
				Assert.Equal(Color.Black.ToArgb(), mask.GetPixel(4, 7).ToArgb());

				// The inner hole (3..4, 3..4) should now be white (255, 255, 255, 255)
				Assert.Equal(Color.White.ToArgb(), mask.GetPixel(3, 3).ToArgb());
				Assert.Equal(Color.White.ToArgb(), mask.GetPixel(3, 4).ToArgb());
				Assert.Equal(Color.White.ToArgb(), mask.GetPixel(4, 3).ToArgb());
				Assert.Equal(Color.White.ToArgb(), mask.GetPixel(4, 4).ToArgb());

				// The rest of the subject should remain white
				Assert.Equal(Color.White.ToArgb(), mask.GetPixel(1, 1).ToArgb());
				Assert.Equal(Color.White.ToArgb(), mask.GetPixel(6, 6).ToArgb());
				Assert.Equal(Color.White.ToArgb(), mask.GetPixel(2, 5).ToArgb());
			}
		}

		[Fact]
		public void TestAssetCreatorPromptSynthesis_WithMultiViewChecked()
		{
			// Arrange
			var form = new AiAssetCreatorDialog(
				initialApiKey: "test-key",
				initialModel: "models/gemini-2.5-flash-image",
				initialResult: null,
				initialPrompt: "a heroic knight",
				initialTab: "Character",
				initialStyleCategory: "Digital Art",
				initialStyle: "3D Render",
				initialSpecies: "Human",
				initialGender: "Male",
				initialPersonality: "Brave",
				initialFashion: "Plate Armor",
				initialAccessory: "Sword",
				initialPalette: "Golden",
				initialPose: "Standing",
				initialLocationType: "",
				initialStructure: "",
				initialVisualState: "",
				initialAtmosphere: "",
				initialPropType: "",
				initialMaterial: "",
				initialCondition: "",
				initialScaleToFit: true,
				initialCopyToClipboard: true,
				initialGenerateMultiView: true
			);

			// Act
			var method = typeof(AiAssetCreatorDialog).GetMethod("SynthesizePrompt", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			Assert.NotNull(method);
			string? result = method.Invoke(form, null) as string;
			Assert.NotNull(result);

			// Assert
			Assert.Contains("presented as a character concept sheet showing 5 views: main, front, back, left, and right", result);
		}

		[Fact]
		public void TestImageCreatorPromptSynthesis()
		{
			// Arrange
			var form = new AiImageCreatorDialog(
				initialApiKey: "test-key",
				initialModel: "models/gemini-2.5-flash-image",
				initialResult: null,
				initialPrompt: "a beautiful sunrise over a mountain",
				initialStyleCategory: "Digital Art",
				initialStyle: "3D Render",
				initialScaleToFit: true,
				initialCopyToClipboard: true,
				initialCompositionLayout: "Golden Spiral"
			);

			// Act
			var method = typeof(AiImageCreatorDialog).GetMethod("SynthesizePrompt", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			Assert.NotNull(method);
			string? result = method.Invoke(form, null) as string;
			Assert.NotNull(result);

			// Assert
			Assert.Contains("a beautiful sunrise over a mountain", result);
			Assert.Contains("Composition guide/armature: Golden Spiral", result);
		}

		[Fact]
		public void TestBuildRequestJson_IncludesAspectRatioInImageConfig()
		{
			// Arrange
			var history = new List<ChatTurn>();
			string? sourceBase64 = null;
			string? maskBase64 = null;
			string prompt = "A futuristic city";
			string aspectRatio = "16:9";

			// Act
			string jsonPayload = GeminiClient.BuildRequestJson(history, sourceBase64, maskBase64, prompt, aspectRatio);

			// Assert
			using (JsonDocument doc = JsonDocument.Parse(jsonPayload))
			{
				JsonElement root = doc.RootElement;
				Assert.True(root.TryGetProperty("generationConfig", out JsonElement genConfig));
				Assert.True(genConfig.TryGetProperty("imageConfig", out JsonElement imgConfig));
				Assert.True(imgConfig.TryGetProperty("aspectRatio", out JsonElement aspectVal));
				Assert.Equal("16:9", aspectVal.GetString());
			}
		}
	}
}


