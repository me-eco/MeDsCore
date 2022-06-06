using System.Text.Json.Serialization;

namespace MeDsCore.Base;

public class RoleEntity
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("position")]
    public int Position { get; set; }
    [JsonPropertyName("permissions")]
    public string Permissions { get; set; }
}