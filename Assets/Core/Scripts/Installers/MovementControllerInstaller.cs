using UnityEngine;
using Zenject;

namespace Core.Scripts.Character
{
    public class MovementControllerInstaller : MonoInstaller
    {
        [SerializeField] private MovementController _movementController;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<IMovementController>().FromInstance(_movementController).AsSingle()
                .NonLazy();
        }
    }
}