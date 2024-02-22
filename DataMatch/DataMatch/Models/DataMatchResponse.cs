using System.Text.Json.Serialization;

namespace DataMatch.Models
{
    public class DataMatchResponse
    {
        [JsonPropertyName("diffResultType")]
        public string DiffResultType { get; set; }

        [JsonPropertyName("diffs"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<Diff>? Diffs { get; set; } = null;
    }

    public class Diff
    {
        [JsonPropertyName("offset")]
        public long Offset { get; set; }

        [JsonPropertyName("length")]
        public long Length { get; set; }
    }
}
