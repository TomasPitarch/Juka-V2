using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonNetworkService:INetworkService,IConnectionCallbacks
{
    private bool _isConnected=false;
    private UniTaskCompletionSource _serverConnectionTcs;
    
    public PhotonNetworkService()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public void OnConnected()
    {
        _isConnected = true;
        _serverConnectionTcs?.TrySetResult();
    }

    public void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public void OnDisconnected(DisconnectCause cause)
    {
        _isConnected = false;
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {
        Debug.Log("Region List Received");
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
        Debug.Log("Custom Authentication Response");
    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {
        Debug.Log("Custom Authentication Failed");
    }

    public bool IsConnected()
    {
        return _isConnected;
    }
    public UniTask Connect()
    {
        _serverConnectionTcs = new UniTaskCompletionSource();
        PhotonNetwork.ConnectUsingSettings();
        return _serverConnectionTcs.Task;
    }
    
}
