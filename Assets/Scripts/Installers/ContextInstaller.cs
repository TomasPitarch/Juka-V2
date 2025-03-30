
using UnityEngine;
using Zenject;

public class ContextInstaller : MonoInstaller<ContextInstaller>
{
   [SerializeField]
   private Navigator navigator;
   
   public override void InstallBindings()
   {
       Container.Bind<Navigator>().FromInstance(navigator).AsSingle();
   }
}
