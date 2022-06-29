using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ApiGateway.Models;
using ApiGateway.Services;

namespace ApiGateway.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthCookiesController : ControllerBase 
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<AuthCookiesController> _logger;
    private readonly IRpcClientService _rpcClient;

    public AuthCookiesController(
        SignInManager<IdentityUser> signInManager, 
        UserManager<IdentityUser> userManager,
        ILogger<AuthCookiesController> logger,
        IRpcClientService rpcClient
    ) {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
        _rpcClient = rpcClient;
    }

    // POST: api/auth/authtester
    [HttpPost]
    // [Authorize]
    public IActionResult AuthTester([FromBody] UserDataModel Auth) {
        _logger.LogInformation("AuthCookiesController Test.");

        // var response = _rpcClient.SendRpcMessage("Test Rpc Message!!!!");
        // _logger.LogInformation($"Message Received: {response}");

        _rpcClient.SendMessage("Test Message!!!!");

        return Ok(new { Username = Auth.Username, Password = Auth.Password } );
    }

    // POST: api/auth/login
    [HttpPost]
    public async Task<IActionResult> LogIn([FromBody] UserDataModel Auth) {
        try 
        {
            var result = await _signInManager.PasswordSignInAsync(Auth.Username, Auth.Password, false, false);

            if(result.Succeeded) {
                _logger.LogInformation("User Login Successful.");
                return Ok(new { Message = "User Login Successful." } );
            }

            _logger.LogInformation("User Login Failed.");
            return BadRequest(new { Message = "User Login Failed." } );
        }
        catch 
        {
            _logger.LogInformation("User Login Failed.");
            return BadRequest(new { Message = "User Login Failed." } );
        }
    }

    // POST: api/auth/logout
    [HttpPost]
    public async Task<IActionResult> LogOut()
    {
        bool isSignedIn = _signInManager.IsSignedIn(HttpContext.User);

        if(isSignedIn) {
            try 
            {
                await _signInManager.SignOutAsync();

                _logger.LogInformation("User Logout Successful.");
                return Ok(new { Message = "User Logout Successful." } );
            }
            catch 
            {
                _logger.LogInformation("User Logout Failed.");
                return BadRequest(new { Message = "User Logout Failed." } );
            }
        }

        _logger.LogInformation("User Not Signed In.");
        return BadRequest(new { Message = "User Not Signed In." } );
    }

    // POST: api/auth/register
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] UserDataModel Auth) {
        try 
        {
            var user = new IdentityUser { UserName = Auth.Username, Email = Auth.Username };
            var result = await _userManager.CreateAsync(user, Auth.Password);

            if (result.Succeeded) {
                await _signInManager.SignInAsync(user, false);

                _logger.LogInformation("User Registration Successful.");
                return Ok(new { Message = "User Registration Successful." } );
            }

            _logger.LogInformation("User Registration Failed.");
            return BadRequest(new { Message = "User Registration Failed." } );
        }
        catch 
        {
            _logger.LogInformation("User Registration Failed.");
            return BadRequest(new { Message = "User Registration Failed." } );
        }
    }

    // DELETE: api/auth/deleteuser
    [HttpDelete]
    public async Task<IActionResult> DeleteUser([FromBody] UserDataModel Auth)
    {
        bool isSignedIn = _signInManager.IsSignedIn(HttpContext.User);

        if(isSignedIn) {
            try 
            {
                IdentityUser user = await _userManager.FindByNameAsync(Auth.Username);

                await _signInManager.SignOutAsync();
                var result = await _userManager.DeleteAsync(user);

                if(result.Succeeded) {
                    _logger.LogInformation("User Delete Successful.");
                    return Ok(new { Message = "User Delete Successful." } );
                }

                _logger.LogInformation("User Delete Failed.");
                return BadRequest(new { Message = "User Delete Failed." } );
            }
            catch 
            {
                _logger.LogInformation("User Delete Failed.");
                return BadRequest(new { Message = "User Delete Failed." } );
            }
        }

        _logger.LogInformation("User Not Signed In.");
        return BadRequest(new { Message = "User Not Signed In." } );
    }
}