using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ApiGateway.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class CsrfTokenController : ControllerBase 
{
    private readonly ILogger<AuthCookiesController> _logger;
    
    public CsrfTokenController(ILogger<AuthCookiesController> logger) 
    { 
        _logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    [IgnoreAntiforgeryToken]
    public IActionResult GetCsrfToken() {
        _logger.LogInformation("Get Csrf Token.");

        return Ok();
    }
}