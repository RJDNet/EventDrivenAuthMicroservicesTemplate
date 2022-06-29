namespace ApiGateway.Services;

public interface IRpcClientService {
    public string SendRpcMessage(string message);
    public void SendMessage(string message);
    public void InitialiseConnection();
    public void Close();
}
