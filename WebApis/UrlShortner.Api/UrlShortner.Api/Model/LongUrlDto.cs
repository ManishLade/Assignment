using System.Text.Json.Serialization;

namespace UrlShortner.Api.Model;

public class LongUrlDto
{
    [JsonIgnore] public int Id { get; set; }

    public string Url { get; set; }
}