using Photon.Pun;

public class TeamPikerPresenter
{
    private TeamPikerView _view;
    private IGameServerService _gameServerService;
    
    
    public TeamPikerPresenter(TeamPikerView view, IGameServerService gameServerService)
    {
        _view = view;
        _view.OnTeamButtonClick += SelectTeamHandler;
        _view.OnReadyToggleClick += ReadyCheckHandler;
        _view.OnStartButtonClick += StartGameRequestHandler;
       
        
        _gameServerService = gameServerService;
        _gameServerService.OnBecomeServer += BecomeServerHandler;
        _gameServerService.OnPlayerJoin += PlayerJoinHandler; 
        _gameServerService.OnPlayerJoinTeamA += PlayerJoinTeamAHandler;
        _gameServerService.OnPlayerJoinTeamB += PlayerJoinTeamBHandler;
        _gameServerService.OnReadyState += ReadyStateHandler;
        _gameServerService.OnStartGame += StartGameHandler;
        
    }

    private void StartGameHandler()
    {
        //TODO: Remove the photon network dependency and the scene load logic from presenter
        PhotonNetwork.LoadLevel("TestScene");
    }
    private void StartGameRequestHandler()
    {
        _gameServerService.RequestToStartGame();
    }
    private void ReadyStateHandler(bool value)
    {
        _view.ReadyState(value);
    }
    private void PlayerJoinTeamBHandler(string teamBNicknames)
    {
        _view.TeamUpdate(teamBNicknames, TeamGroup.TeamB);
    }
    private void PlayerJoinTeamAHandler(string teamANicknames)
    {
        _view.TeamUpdate(teamANicknames, TeamGroup.TeamA);
    }
    private void PlayerJoinHandler(string waitTeamNicknames)
    {
        _view.TeamUpdate(waitTeamNicknames, TeamGroup.WaitTeam);
    }
    private void BecomeServerHandler()
    {
        _view.TurnOnServerView();
    }
    private void ReadyCheckHandler(bool value)
    {
        _gameServerService.RequestToReady(value);
    }
    private void SelectTeamHandler(TeamGroup team)
    {
          _gameServerService.RequestToJoinTeam(team);   
    }
    
}

