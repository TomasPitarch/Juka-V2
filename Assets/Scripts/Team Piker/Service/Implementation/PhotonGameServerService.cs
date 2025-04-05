using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Zenject;

public class TeamPlayerModel
{
    public Player Player;
    public TeamGroup TeamGroup;
    public bool PropertiesReady;
    
    public TeamPlayerModel(Player player, TeamGroup teamGroup)
    {
        Player = player;
        TeamGroup = teamGroup;
    } 
}

public class PhotonGameServerService : MonoBehaviourPunCallbacks,IGameServerService
{ 
    
    private List<TeamPlayerModel> _playersList = new();
    
    private GameData _gameData;

    private bool _setPropertiesCompleted;
    
    [Inject]
    public void Initialize(GameData gameData)
    {
        _gameData = gameData;
    }
    
    
    #region IGameServerService
    
    public event Action OnBecomeServer;
    public event Action<string> OnPlayerJoin;
    public event Action<string> OnPlayerJoinTeamA;
    public event Action<string> OnPlayerJoinTeamB;
    public event Action<bool> OnReadyState;
    public event Action OnStartGame;
    
    private int _readyCount;
    
    public void RequestToJoinTeam(TeamGroup team)
    {
       photonView.RPC("RpcTeamRequest", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer.ActorNumber, team);
    }

    public void RequestToReady(bool ready)
    {
        photonView.RPC("RpcReadyRequest", RpcTarget.MasterClient,ready);
    }

    public void RequestToStartGame()
    {
        Hashtable tableA = new();
        tableA.Add("Team",TeamGroup.TeamA);
        Hashtable tableB = new ();
        tableB.Add("Team", TeamGroup.TeamB);
        
       
        foreach (TeamPlayerModel player in _playersList)
        {
            if(player.TeamGroup == TeamGroup.TeamA)
            {
                player.Player.SetCustomProperties(tableA);
            }
            else if(player.TeamGroup == TeamGroup.TeamB)
            {
                player.Player.SetCustomProperties(tableB);
            }
        }
        
        WaitingSetUpToStartGame();
    }
   
    
    
#endregion
   

    #region PunCallbacks
    public override void OnCreatedRoom()
    {
        OnBecomeServer?.Invoke();
    }
    public override void OnJoinedRoom()
    {
        //TODO:this consideration is optimal if never change the master client
        if (!PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }
        else
        {
            OnPlayerEnteredRoom(PhotonNetwork.LocalPlayer);
        }
      
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        _playersList.Add(new TeamPlayerModel(newPlayer, TeamGroup.WaitTeam));
        //Update to all players the new player
        photonView.RPC("RpcUpdateTeam", RpcTarget.All, TeamGroup.WaitTeam,
            PlayerNicknamesToString(_playersList.Where(player=>player.TeamGroup == TeamGroup.WaitTeam).ToList()));
        //Update the new player with the current players all ready in teams
        photonView.RPC("RpcUpdateTeam", newPlayer, TeamGroup.TeamA,
            PlayerNicknamesToString(_playersList.Where(player=>player.TeamGroup == TeamGroup.TeamA).ToList()));
        photonView.RPC("RpcUpdateTeam", newPlayer, TeamGroup.TeamB,
            PlayerNicknamesToString(_playersList.Where(player=>player.TeamGroup == TeamGroup.TeamB).ToList()));
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        TeamPlayerModel playerLeft = _playersList.Find(player => player.Player.ActorNumber == otherPlayer.ActorNumber);
        _playersList.Remove(playerLeft);
        photonView.RPC("RpcUpdateTeam", RpcTarget.All,playerLeft.TeamGroup ,PlayerNicknamesToString(_playersList.Where(player=>player.TeamGroup == playerLeft.TeamGroup).ToList()));
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
    
        if (!changedProps.ContainsKey("Team"))
        {
            return;
        }
    
        TeamPlayerModel player= _playersList.Find(player => player.Player.ActorNumber == targetPlayer.ActorNumber);
        player.PropertiesReady = true;
    
        if(_playersList.Count(teamPlayerModel => teamPlayerModel.PropertiesReady)==_gameData.TotalPlayersToStartGame())
        {
            _setPropertiesCompleted = true;
        }
       
    }
    

#endregion


    #region RPCs

    [PunRPC]
    private void RpcUpdateTeam(TeamGroup teamGroup,string TeamNicknames)
    {
        switch (teamGroup)
        {
            case TeamGroup.TeamA:
                OnPlayerJoinTeamA?.Invoke(TeamNicknames);
                break;
            case TeamGroup.TeamB:
                OnPlayerJoinTeamB?.Invoke(TeamNicknames);
                break;
            case TeamGroup.WaitTeam:
                OnPlayerJoin?.Invoke(TeamNicknames);
                break;
        }
    }
    [PunRPC]
    private void RpcTeamRequest(int actorNumber, TeamGroup teamGroup)
    {
        TeamPlayerModel player = _playersList.Find(player => player.Player.ActorNumber == actorNumber);
        TeamGroup oldTeam = player.TeamGroup;
        player.TeamGroup = teamGroup;
        
        photonView.RPC("RpcUpdateTeam", RpcTarget.All, teamGroup, PlayerNicknamesToString(_playersList.Where(player=>player.TeamGroup == teamGroup).ToList()));
        photonView.RPC("RpcUpdateTeam", RpcTarget.All, oldTeam, PlayerNicknamesToString(_playersList.Where(player=>player.TeamGroup == oldTeam).ToList()));
    }
    [PunRPC]
    private void RpcReadyRequest(bool ready)
    {
        _readyCount += ready ? 1 : -1;
        if (_readyCount == _gameData.TotalPlayersToStartGame())
        {
            photonView.RPC("RpcCanStartGame", RpcTarget.MasterClient,true);
        }
        else
        {
            photonView.RPC("RpcCanStartGame", RpcTarget.MasterClient,false);
        }

    }    
    [PunRPC]
    private void RpcCanStartGame(bool value)
    {
        OnReadyState?.Invoke(value);
    }
    [PunRPC]
    void RpcStartGame()
    {
        OnStartGame?.Invoke();
    }
    

    #endregion
    
    private string PlayerNicknamesToString(List<TeamPlayerModel> listOfNickNames)
    {
        string newString="";
    
        foreach (TeamPlayerModel playerNickname in listOfNickNames)
        {
            newString = newString + "/" + playerNickname.Player.NickName;
        }
    
        return newString;
    }
    private async UniTask WaitingSetUpToStartGame()
    {
        while(!_setPropertiesCompleted)
        {
            await UniTask.Yield();
        }
        PhotonNetwork.CurrentRoom.IsOpen = false;
        photonView.RPC("RpcStartGame", RpcTarget.All);
    }
    
}