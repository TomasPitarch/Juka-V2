using UnityEngine;
using Zenject;


public class ScreensInstaller : MonoInstaller
{
    [SerializeField]
    private LoginView loginView;
    [SerializeField]
    private ConnectView connectView;
    [SerializeField]
    private TeamPikerView teamPikerView;
    
    public override void InstallBindings()
    {
        BindInitialConnect();
        BindLogin();
        BindTeamPiker();
    }
    private void BindLogin()
    {
        Container.Bind<LoginView>().FromInstance(loginView).AsSingle();
        Container.Bind<LoginModel>().FromInstance(new LoginModel()).AsSingle();
        Container.Bind<LoginPresenter>().AsSingle().NonLazy();
    }
    
    private void BindInitialConnect()
    {
        Container.Bind<ConnectView>().FromInstance(connectView).AsSingle();
        Container.Bind<ConnectPresenter>().AsSingle().NonLazy();
    }
    private void BindTeamPiker()
    {
        Container.Bind<TeamPikerView>().FromInstance(teamPikerView).AsSingle();
        Container.Bind<TeamPikerPresenter>().AsSingle().NonLazy();
    }

    
}
