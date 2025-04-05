using Cysharp.Threading.Tasks;
using Photon.Pun;
using Zenject;

public class LoginPresenter
{
    
    private LoginView _loginView;
    private LoginModel _loginModel;
    
     
    private ILoginService _loginService;
    private Navigator _navigator;

    public LoginPresenter(LoginView loginView, LoginModel loginModel, ILoginService loginService, Navigator navigator)
    {
        _loginView = loginView;
        _loginModel = loginModel;
        _loginService = loginService;
        _navigator = navigator;


        _loginView.OnConnectButtonClicked += ConnectToRoom;
        _loginView.DisableConnectButton();
    }
  
    private void ConnectToRoom()
    {
        _loginView.DisableConnectButton();
        PhotonNetwork.NickName = _loginView.GetNickName();
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
        _navigator.OpenScreen("Teampiker");
    }
}
