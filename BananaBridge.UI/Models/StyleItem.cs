using System.Text.Json.Serialization;

namespace BananaBridge.UI.Models
{
	public class StyleItem
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		[JsonPropertyName("image_prompt")]
		public string ImagePrompt { get; set; }

		[JsonPropertyName("base_style")]
		public BaseStyleInfo BaseStyle { get; set; }

		[JsonPropertyName("characteristics")]
		public CharacteristicsInfo Characteristics { get; set; }

		[JsonPropertyName("frame_style")]
		public FrameStyleInfo FrameStyle { get; set; }

		[JsonPropertyName("genders")]
		public System.Collections.Generic.List<string> Genders { get; set; }

		[JsonPropertyName("species")]
		public System.Collections.Generic.List<string> Species { get; set; }

		[JsonPropertyName("character_types")]
		public System.Collections.Generic.List<string> CharacterTypes { get; set; }

		[JsonPropertyName("location_attributes")]
		public LocationAttributesInfo LocationAttributes { get; set; }

		[JsonPropertyName("color_palette")]
		public string ColorPalette { get; set; }

		[JsonPropertyName("lighting_style")]
		public string LightingStyle { get; set; }

		public override string ToString() => Name;
	}

	public class LocationAttributesInfo
	{
		[JsonPropertyName("types")]
		public System.Collections.Generic.List<string> Types { get; set; }

		[JsonPropertyName("structures")]
		public System.Collections.Generic.List<string> Structures { get; set; }

		[JsonPropertyName("atmospheres")]
		public System.Collections.Generic.List<string> Atmospheres { get; set; }
	}

	public class FrameStyleInfo
	{
		[JsonPropertyName("style_type")]
		public string StyleType { get; set; }
	}

	public class BaseStyleInfo
	{
		[JsonPropertyName("substrate")]
		public string Substrate { get; set; }

		[JsonPropertyName("rendering")]
		public string Rendering { get; set; }

		[JsonPropertyName("pigmentation")]
		public string Pigmentation { get; set; }
	}

	public class CharacteristicsInfo
	{
		[JsonPropertyName("line_quality")]
		public string LineQuality { get; set; }

		[JsonPropertyName("coloring")]
		public string Coloring { get; set; }

		[JsonPropertyName("anatomy")]
		public string Anatomy { get; set; }

		[JsonPropertyName("composition")]
		public string Composition { get; set; }
	}
}

