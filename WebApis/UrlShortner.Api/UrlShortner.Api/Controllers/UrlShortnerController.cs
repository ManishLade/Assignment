using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using UrlShortner.Api.Model;
using UrlShortner.Data;
using UrlShortner.Infrastructure;

namespace UrlShortner.Api.Controllers;

[ApiController]
[Route("/")]
public class UrlShortnerController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUrlShortnerService _urlShortnerService;

    public UrlShortnerController(IUrlShortnerService urlShortnerService,
        IMapper mapper)
    {
        _urlShortnerService = urlShortnerService;
        _mapper = mapper;
    }

    public IConfiguration Configuration { get; set; }

    [HttpPost("urls")]
    public async Task<IActionResult> PostLongUrl(LongUrlDto longUrlDto)
    {
        if (!CheckUrl(longUrlDto.Url))
            return NotFound();

        if (!CallUrl(longUrlDto.Url))
            return NotFound();

        var longUrl = _mapper.Map<LongUrl>(longUrlDto);
        var id = await _urlShortnerService.SaveUrl(longUrl);
        var shortUrl = _urlShortnerService.IdToShortUrl(id);
        return CreatedAtAction(nameof(ShortUrlToLongUrl), new { shortUrl }, shortUrl);
    }

    [HttpGet("redirect/{*shortUrl}")]
    [EnableCors("AllowOrigin")]
    public IActionResult RedirectToLongUrl(string shortUrl)
    {
        LongUrl longUrl = null;
        var id = _urlShortnerService.ShortUrlToId(shortUrl);
        if (id == 0) return NotFound();

        longUrl = _urlShortnerService.RetriveFromDatabase(id);

        return RedirectPermanent(longUrl.Url);
    }

    [HttpGet("{*shortUrl}")]
    public IActionResult ShortUrlToLongUrl(string shortUrl)
    {
        var id = _urlShortnerService.ShortUrlToId(shortUrl);

        if (id == 0) return NotFound();

        var longUrl = _urlShortnerService.RetriveFromDatabase(id);

        if (longUrl == null) return NotFound();
        return RedirectPermanent(longUrl.Url);
    }

    private bool CheckUrl(string url)
    {
        Uri uriResult;
        var result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                     && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        return result;
    }

    private bool CallUrl(string url)
    {
        var request = WebRequest.Create(url);
        try
        {
            var response = request.GetResponse();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
}