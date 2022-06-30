using Microsoft.AspNetCore.Mvc;
using ApiGateway.Services;

namespace ApiGateway.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class MessagingTestController : ControllerBase 
{
    private readonly ILogger<AuthCookiesController> _logger;
    private readonly IRpcClientService _rpcClient;

    public MessagingTestController(
        ILogger<AuthCookiesController> logger,
        IRpcClientService rpcClient
    ) {
        _logger = logger;
        _rpcClient = rpcClient;
    }

    // POST: api/auth/rpctester
    [HttpPost]
    public IActionResult RpcMessageTest() {
        var response = _rpcClient.SendRpcMessage("Test Rpc Message.");
        _logger.LogInformation($"Rpc Response Message Received: {response}.");

        return Ok();
    }

    // POST: api/auth/messagealltester
    [HttpPost]
    public IActionResult MessageTest() {
        _rpcClient.SendMessage("Send Message to services via (topic).");

        return Ok();
    }
}