using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryCache.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InMemoryCacheController : ControllerBase
{
    private readonly IMemoryCache _cache;
    private readonly MemoryCacheEntryOptions _cacheExpirationOptions;

    public InMemoryCacheController(IMemoryCache cache)
    {
        _cache = cache;
        _cacheExpirationOptions = new MemoryCacheEntryOptions();
        _cacheExpirationOptions.Priority = CacheItemPriority.Normal;
    }

    // GET api/values/
    [HttpGet("{key}")]
    public ActionResult<string> Get(string key)
    {
        try
        {
            return _cache.Get<string>(key);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    // POST api/values
    [HttpPost]
    public IActionResult Post([FromBody] Request request)
    {
        try
        {
            _cacheExpirationOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(request.ExiprationTimeinMinutes);
            _cache.Set(request.Key, request.Value, _cacheExpirationOptions);
            return Ok("Key added in memmory");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}