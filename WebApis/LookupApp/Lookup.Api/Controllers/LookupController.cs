using Lookup.Service;
using Microsoft.AspNetCore.Mvc;

namespace Lookup.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StateLookupController : ControllerBase
{
    private readonly ILookupService _lookupService;

    public StateLookupController(ILookupService lookupService)
    {
        _lookupService = lookupService;
    }

    [HttpPost]
    [Route("/api/states/lookup/{zipCode}")]
    public IActionResult Get(string zipCode)
    {
        var result = _lookupService.GetStateByZipCode(zipCode);
        return result.StatusCode == System.Net.HttpStatusCode.OK 
            ? Ok(result.State) 
            : StatusCode((int)result.StatusCode, result.ErrorMessage);
    }
}