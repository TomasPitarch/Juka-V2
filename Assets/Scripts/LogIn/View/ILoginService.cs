using Cysharp.Threading.Tasks;

public interface ILoginService
{
    public UniTask<LoginResponse> ConnectToDefaultRoom(string roomName="DefaultRoom");
}