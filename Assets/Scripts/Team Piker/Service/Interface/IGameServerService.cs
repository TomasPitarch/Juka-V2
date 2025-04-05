using System;
using Photon.Realtime;

public interface IGameServerService
{
    public event Action OnBecomeServer;
    public event Action<string> OnPlayerJoin;
    public event Action<string> OnPlayerJoinTeamA;
    public event Action<string> OnPlayerJoinTeamB;
    public event Action<bool> OnReadyState;
    public event Action OnStartGame;
    
    public void RequestToJoinTeam(TeamGroup team);
    public void RequestToReady(bool ready);
    public void RequestToStartGame();
}