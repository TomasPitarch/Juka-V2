using System;
using Photon.Pun;
using UnityEngine;
using Zenject;

public class NetworkInstaller : MonoInstaller<NetworkInstaller>
{
    [SerializeField] 
    private PhotonGameServerService photonGameServerService;
    public override void InstallBindings()
    {
        Container.Bind<INetworkService>().To<PhotonNetworkService>().AsSingle();
        Container.Bind<ILoginService>().To<PhotonLoginService>().AsSingle();
        Container.Bind<IGameServerService>().FromInstance(photonGameServerService).AsSingle();
        
    }
}

