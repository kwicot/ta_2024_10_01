using Core.Scripts.Item;
using UnityEngine;
using Zenject;

namespace Core.Scripts.Installers
{
    public class ItemsAnimationInstaller : MonoInstaller
    {
        [SerializeField] private ItemsAnimationController instance;

        public override void InstallBindings()
        {
            Container.Bind<ItemsAnimationController>().FromInstance(instance).AsSingle().NonLazy();
        }
    }
}