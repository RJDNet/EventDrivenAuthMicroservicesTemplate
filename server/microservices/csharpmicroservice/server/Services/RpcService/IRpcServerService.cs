namespace CSharpMicroservice.Services;

public interface IRpcServerService {
    public void InitialiseConnection();
    public void Close();
}