using UnityEngine;
using Zenject;

namespace Core
{
    public class ZoneWindowInstaller : MonoInstaller
    {
        [SerializeField] private ZoneWindow zoneWindowInstance;

        public override void InstallBindings()
        {
            Container.Bind<ZoneWindow>().FromInstance(zoneWindowInstance).AsSingle().NonLazy();
        }
    }
}