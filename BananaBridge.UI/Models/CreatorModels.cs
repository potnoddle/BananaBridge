using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BananaBridge.UI.Models
{
    public class ComicLayoutsData
    {
        [JsonPropertyName("layouts")]
        public List<ComicLayoutItem> Layouts { get; set; }
    }

    public class ComicLayoutItem
    {
        [JsonPropertyName("format")]
        public string Format { get; set; }

        [JsonPropertyName("height_inches")]
        public double HeightInches { get; set; }

        [JsonPropertyName("width_inches")]
        public double WidthInches { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        public override string ToString() => Format;
    }

    public class LocationTypesData
    {
        [JsonPropertyName("LocationProperties")]
        public List<LocationPropertyItem> LocationProperties { get; set; }
    }

    public class LocationPropertyItem
    {
        [JsonPropertyName("property_name")]
        public string PropertyName { get; set; }

        [JsonPropertyName("definition")]
        public string Definition { get; set; }

        [JsonPropertyName("examples")]
        public List<string> Examples { get; set; }
    }

    public class CharacterPersonalityData
    {
        [JsonPropertyName("traits")]
        public List<string> Traits { get; set; }

        [JsonPropertyName("voice_styles")]
        public List<string> VoiceStyles { get; set; }

        [JsonPropertyName("mannerisms")]
        public List<string> Mannerisms { get; set; }

        [JsonPropertyName("emotional_ranges")]
        public List<string> EmotionalRanges { get; set; }
    }

    public class CharacterVisualData
    {
        [JsonPropertyName("fashion")]
        public List<string> Fashion { get; set; }

        [JsonPropertyName("accessories")]
        public List<string> Accessories { get; set; }

        [JsonPropertyName("colorPalettes")]
        public List<string> ColorPalettes { get; set; }

        [JsonPropertyName("iconic_poses")]
        public List<string> IconicPoses { get; set; }

        [JsonPropertyName("consistent_traits")]
        public List<string> ConsistentTraits { get; set; }
    }

    public class CompositionalArmaturesData
    {
        [JsonPropertyName("compositional_armatures")]
        public List<CompositionCategory> CompositionalArmatures { get; set; }
    }

    public class CompositionCategory
    {
        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("layouts")]
        public List<CompositionLayoutItem> Layouts { get; set; }
    }

    public class CompositionLayoutItem
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class CreatorPreset
    {
        public string Tab { get; set; }
        public string Model { get; set; }
        public string Prompt { get; set; }
        public bool ScaleToFit { get; set; }
        public bool CopyToClipboard { get; set; }
        public string Layout { get; set; }
        public string StyleCategory { get; set; }
        public string Style { get; set; }
        public string Species { get; set; }
        public string Gender { get; set; }
        public string Personality { get; set; }
        public string Fashion { get; set; }
        public string Accessory { get; set; }
        public string Palette { get; set; }
        public string Pose { get; set; }
        public string LocationType { get; set; }
        public string Structure { get; set; }
        public string VisualState { get; set; }
        public string Atmosphere { get; set; }
        public string PropName { get; set; }
        public string Material { get; set; }
        public string Condition { get; set; }
        public string CompositionLayout { get; set; }
    }
}
