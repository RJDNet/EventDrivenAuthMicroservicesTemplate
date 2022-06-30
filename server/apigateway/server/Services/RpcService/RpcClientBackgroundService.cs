namespace ApiGateway.Services;

public class RpcClientBackgroundService : IHostedService
{
    private IRpcClientService _messageBroker;

    public RpcClientBackgroundService(IRpcClientService messageBroker) {
        _messageBroker = messageBroker;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        int sleepTime = 5000;
        Thread.Sleep(sleepTime);

        _messageBroker.InitialiseConnection();

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _messageBroker.Close();

        return Task.CompletedTask;
    }
}
