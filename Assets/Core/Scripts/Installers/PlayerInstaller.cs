using UnityEngine;
using Zenject;

namespace Core.Scripts.Character
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private Player playerInstance;

        public override void InstallBindings()
        {
            Container.Bind<Player>().FromInstance(playerInstance).AsSingle().Lazy();
        }
    }
}