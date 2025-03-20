﻿using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine;
using System;
using System.Threading.Tasks;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using ExitGames.Client.Photon;

public class LogInTeamManager : MonoBehaviourPunCallbacks
{
    public event Action OnTeamAUpdate= delegate{ };
    public event Action OnTeamBUpdate= delegate { };
    public event Action OnTeamWaitUpdate= delegate { };
    public event Action<Player> OnPlayerJoin = delegate { };



    public List<Player> TeamA;
    public List<Player> TeamB;
    public List<Player> WaitTeam;

    List<Player> PlayersToSetTeam;

    [SerializeField]
    GameData data;

    [SerializeField]
    Button startButton;

    [SerializeField]
    Toggle readyToggle;

    int playersReady=0;

    bool _setPropertiesCompleted = false;

    // Start is called before the first frame update
    void Start()
    {
        startButton.interactable = false;


        TeamA = new List<Player>();
        TeamB = new List<Player>();
        WaitTeam = new List<Player>();


        PlayerJoinTeamWaitRequest();

        if(!PhotonNetwork.IsMasterClient)
        {
            Destroy(startButton.gameObject);
        }


        readyToggle.onValueChanged.AddListener(PlayerReady_Request);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            OnPlayerJoin(newPlayer);
        }
       
    }

    void PlayerReady_Request(bool checkValue)
    {
        photonView.RPC("PlayerCheck",RpcTarget.MasterClient,checkValue);
    }
    [PunRPC]
    public void PlayerCheck(bool checkValue)
    {
        if(checkValue)
        {
            playersReady++;
        }
        else
        {
            playersReady--;
        }

        if(PhotonNetwork.IsMasterClient)
        {
            StartCheck();
        }
    }

    private void StartCheck()
    {
        if (playersReady == data.MaxPlayersTeamA + data.MaxPlayersTeamB && WaitTeam.Count==0)
        {
            startButton.interactable=true;
        }
        else
        {
            startButton.interactable = false;

        }
    }

    public void PlayerJoinTeamWaitRequest()
    {
        photonView.RPC("SetPlayerOnWaitTeam", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer);
    }
    public void PlayerJoinTeamARequest()
    {
        photonView.RPC("SetPlayerOnTeamA", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer);
    }
    public void PlayerJoinTeamBRequest()
    {
        photonView.RPC("SetPlayerOnTeamB", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer);
    }

    [PunRPC]
    void SetPlayerOnWaitTeam(Player player)
    {
        
        if (WaitTeam.Contains(player))
        {
            return;
        }

        if (TeamA.Remove(player))
        {
            OnTeamAUpdate();
        }

        if (TeamB.Remove(player))
        {
            OnTeamBUpdate();
        }

        WaitTeam.Add(player);
        StartCheck();
        OnTeamWaitUpdate();
    }

    [PunRPC]
    void SetPlayerOnTeamA(Player player)
    {
        if (data.MaxPlayersTeamA == TeamA.Count)
        {
            return;
        }
        if(TeamA.Contains(player))
        {
            return;
        }

        if(TeamB.Remove(player))
        {
            OnTeamBUpdate();
        }
        
        if(WaitTeam.Remove(player))
        {
            OnTeamWaitUpdate();
        }


        TeamA.Add(player);
        StartCheck();
        OnTeamAUpdate();

         
    }

    [PunRPC]
    void SetPlayerOnTeamB(Player player)
    {
        if (data.MaxPlayersTeamB == TeamB.Count)
        {
            return;
        }
        if (TeamB.Contains(player))
        {
            return;
        }

        if (TeamA.Remove(player))
        {
            OnTeamAUpdate();
        }

        if (WaitTeam.Remove(player))
        {
            OnTeamWaitUpdate();
        }
    
        TeamB.Add(player);
        StartCheck();
        OnTeamBUpdate();
    }

    public void StartGameButton()
    {


        PlayersToSetTeam = new List<Player>();
        PlayersToSetTeam.AddRange(TeamA);
        PlayersToSetTeam.AddRange(TeamB);



            //Set Team A for all TeamA  Players//
            foreach (var player in TeamA)
            {
                var table = new Hashtable();
                table.Add("Team", Team.A);

                player.SetCustomProperties(table);

            }

            //Set Team B for all TeamB  Players//
            foreach (var player in TeamB)
            {
                var table = new Hashtable();
                table.Add("Team", Team.B);
                player.SetCustomProperties(table);
            }

            startButton.interactable = false;
            WaitingSetUpToStartGame();


    }

    async void WaitingSetUpToStartGame()
    {
        while(!_setPropertiesCompleted)
        {
            await Task.Yield();
        }
        PhotonNetwork.CurrentRoom.IsOpen = false;
        photonView.RPC("StartGamePlay", RpcTarget.All);
    }

    [PunRPC]
    void StartGamePlay()
    {
        PhotonNetwork.LoadLevel("TestScene");
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        if (!changedProps.ContainsKey("Team"))
        {
            return;
        }

        if(PlayersToSetTeam.Contains(targetPlayer))
        {
            PlayersToSetTeam.Remove(targetPlayer);
        }
        else
        {
            print("no contiene al player:" + targetPlayer.NickName);
        }
        

        if(PlayersToSetTeam.Count==0)
        {
            _setPropertiesCompleted = true;
        }
       
    }

}
