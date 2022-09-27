using System.Text.Json.Serialization;
using AutoMapper.Configuration.Annotations;

namespace UrlShortner.Api.Model;

public class LongUrlDto
{
    [JsonIgnore]
    public int Id { get; set; }
    public string Url { get; set; }
}