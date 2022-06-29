namespace CSharpMicroservice.Services;

public class RpcServerBackgroundService : IHostedService
{
    private IRpcServerService _messageBroker;

    public RpcServerBackgroundService(IRpcServerService messageBroker) {
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
