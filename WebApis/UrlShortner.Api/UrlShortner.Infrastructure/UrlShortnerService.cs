using UrlShortner.Data;

namespace UrlShortner.Infrastructure;

public interface IUrlShortnerService
{
    Task<int> SaveUrl(LongUrl longUrl);
    string IdToShortUrl(int id);
    int ShortUrlToId(string shortUrl);
    LongUrl RetriveFromDatabase(int id);
}

public class UrlShortnerService : IUrlShortnerService
{
    private readonly ILongUrlContext _context;
    private readonly int shortUrlLength = 6;

    public UrlShortnerService(ILongUrlContext context)
    {
        _context = context;
    }

    public async Task<int> SaveUrl(LongUrl longUrl)
    {
        _context.Add(longUrl);
        await _context.SaveChangesAsync();
        return longUrl.Id;
    }

    public string IdToShortUrl(int n)
    {
        // Map to store 62 possible characters
        var map = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

        var shorturl = "";

        // Convert given integer id to a base 62 number 
        while (n != 0)
        {
            // use above map to store actual character 
            // in short url 
            shorturl += map[n % 51];
            n = n / 51;
        }

        // Reverse shortURL to complete base conversion 
        var charArray = shorturl.ToCharArray();
        Array.Reverse(charArray);

        shorturl = new string(charArray);
        shorturl = ExpandUrlLength(shorturl);
        return shorturl;
    }

    public int ShortUrlToId(string shortURL)
    {
        if (shortURL.Length != shortUrlLength)
            return 0;

        shortURL = shortURL.Replace("Z", "");

        var id = 0; // initialize result 

        // A simple base conversion logic 
        for (var i = 0; i < shortURL.Length; i++)
            if ('a' <= shortURL[i] && shortURL[i] <= 'z')
                id = id * 51 + shortURL[i] - 'a';
            else if ('A' <= shortURL[i] && shortURL[i] <= 'Z')
                id = id * 51 + shortURL[i] - 'A' + 26;
            else
                return 0;
        return id;
    }

    public LongUrl RetriveFromDatabase(int id)
    {
        var ans = _context.longUrls.Where(x => x.Id == id);
        return ans != null && ans.Any() ? ans.First() : null;
    }

    private string ExpandUrlLength(string url)
    {
        var shortUrl = "";
        var diff = shortUrlLength - url.Length;
        for (var i = 0; i < diff; i++) shortUrl = "Z" + shortUrl;

        shortUrl += url;
        return shortUrl;
    }
}