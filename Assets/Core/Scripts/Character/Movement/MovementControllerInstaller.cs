using UnityEngine;
using Zenject;

namespace Core.Scripts.Character
{
    public class MovementControllerInstaller : MonoInstaller
    {
        [SerializeField] MovementController _movementController;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<IMovementController>().FromInstance(_movementController).AsSingle()
                .NonLazy();
        }
    }
}