
using UnityEngine;
using Zenject;

public class ContextInstaller : MonoInstaller<ContextInstaller>
{
   [SerializeField]
   private Navigator navigator;
   [SerializeField]
   private GameData gameData;
   
   public override void InstallBindings()
   {
       Container.BindInstance(navigator).AsSingle();
       Container.BindInstance(gameData).AsSingle();
   }
}
