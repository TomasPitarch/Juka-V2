using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonLoginService : ILoginService,IMatchmakingCallbacks
{
    private RoomOptions _defaultRoomOptions;
    private UniTaskCompletionSource<LoginResponse> _roomConnectionTcs;
    
    public PhotonLoginService()
    {
        PhotonNetwork.AddCallbackTarget(this);
        
        _defaultRoomOptions = new RoomOptions
        {
            IsOpen = true,
            MaxPlayers = 10
        };
    }
    public UniTask<LoginResponse> ConnectToDefaultRoom(string roomName="DefaultRoom")
    {
        _roomConnectionTcs = new UniTaskCompletionSource<LoginResponse>();
        PhotonNetwork.JoinOrCreateRoom(roomName, _defaultRoomOptions, TypedLobby.Default);
        return _roomConnectionTcs.Task;
    }

    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        Debug.Log("Friend List Update");
    }

    public void OnCreatedRoom()
    {
        Debug.Log("Room Created");
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Create Room Failed");
    }

    public void OnJoinedRoom()
    {
        LoginResponse response = new LoginResponse(LoginStatus.Success, "Connected to room", 200);
        _roomConnectionTcs?.TrySetResult(response);
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
        LoginResponse response = new LoginResponse(LoginStatus.Error, message, returnCode);
        _roomConnectionTcs?.TrySetResult(response);
    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
       Debug.Log("Join Random Failed");
    }

    public void OnLeftRoom()
    {
        Debug.Log("Left Room");
    }
}