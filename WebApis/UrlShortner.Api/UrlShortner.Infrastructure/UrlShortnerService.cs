using Microsoft.EntityFrameworkCore;
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
    private readonly LongUrlContext _context;
    private int shortUrlLength = 6;

    public UrlShortnerService(LongUrlContext context)
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
        char[] map = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

        string shorturl = "";

        // Convert given integer id to a base 62 number 
        while (n != 0)
        {
            // use above map to store actual character 
            // in short url 
            shorturl += map[n % 51];
            n = n / 51;
        }

        // Reverse shortURL to complete base conversion 
        char[] charArray = shorturl.ToCharArray();
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

        int id = 0; // initialize result 

        // A simple base conversion logic 
        for (int i = 0; i < shortURL.Length; i++)
        {
            if ('a' <= shortURL[i] && shortURL[i] <= 'z')
                id = id * 51 + shortURL[i] - 'a';
            else if ('A' <= shortURL[i] && shortURL[i] <= 'Z')
                id = id * 51 + shortURL[i] - 'A' + 26;
            else
                return 0;
        }
        return id;
    }

    public LongUrl RetriveFromDatabase(int id)
    {
        var ans = _context.longUrls.Where(x => x.Id == id);
        return ans != null && ans.Any() ? ans.First() : null;
    }

    private string ExpandUrlLength(string url)
    {
        string shortUrl = "";
        int diff = shortUrlLength - url.Length;
        for (int i = 0; i < diff; i++)
        {
            shortUrl = "Z" + shortUrl;
        }

        shortUrl += url;
        return shortUrl;
    }
}