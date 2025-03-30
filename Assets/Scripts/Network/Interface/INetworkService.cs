using Cysharp.Threading.Tasks;


public interface INetworkService
{
    public bool IsConnected();
    public UniTask Connect();
}
