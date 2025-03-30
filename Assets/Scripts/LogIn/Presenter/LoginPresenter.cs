using Cysharp.Threading.Tasks;
using Photon.Pun;
using Zenject;

public class LoginPresenter
{
    
    private LoginView _loginView;
    private LoginModel _loginModel;
    
     
    private ILoginService _loginService;

    public LoginPresenter(LoginView loginView, LoginModel loginModel, ILoginService loginService)
    {
        _loginView = loginView;
        _loginModel = loginModel;
        _loginService = loginService;


        _loginView.OnConnectButtonClicked += ConnectToRoom;
        _loginView.DisableConnectButton();
    }
  
    private void ConnectToRoom()
    {
        _loginView.DisableConnectButton();
        _loginService.ConnectToDefaultRoom().ContinueWith(OnLoginResponse);
    }
    private void OnLoginResponse(LoginResponse response)
    {
        _loginView.DisableConnectButton();  
        switch (response.Status)
        {
            case LoginStatus.Success:
                LoginSuccess();
                break;
            case LoginStatus.InvalidNickName:
                break;
            case LoginStatus.Error:
                break;
        }
    }

    private void LoginSuccess()
    {
        //TODO:Move out of here, implement a non generic nickname and
        // use the navigator instead of a change of scene
        PhotonNetwork.LocalPlayer.NickName = "Generic";
        PhotonNetwork.LoadLevel("TeamsScene");
    }
}
