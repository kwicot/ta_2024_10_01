using Core.Scripts.Installers;
using UnityEngine;
using Zenject;

namespace Core
{
    public class ZoneWindowInstaller : MonoInstaller
    {
        [SerializeField] private ZoneWindow zoneWindowPrefab;
        [SerializeField] private int initialSize = 1;

        public override void InstallBindings()
        {
            Container.BindFactory<ZoneWindow, ZoneWindow.Factory>()
                .FromComponentInNewPrefab(zoneWindowPrefab)
                .AsSingle();

            Container.Bind<ZoneWindowPool>().AsSingle()
                .WithArguments(initialSize);
        }
    }
}