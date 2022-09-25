namespace InMemoryCache.API;

public class Request
{
    public string Key { get; set; }
    public string Value { get; set; }

    public int ExiprationTimeinMinutes { get; set; }
}