using UnityEngine;
using Zenject;

namespace Core.Scripts.Character.Input
{
    public class InputServiceInstaller : MonoInstaller
    {
        [SerializeField] private GameObject keyboardInputControllerPrefab;
        [SerializeField] private GameObject joystickInputControllerPrefab;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<IInput>().FromComponentInNewPrefab(joystickInputControllerPrefab)
                .AsSingle().NonLazy();

            // if(Application.isEditor && Application.platform != RuntimePlatform.Android)
            //     Container.BindInterfacesAndSelfTo<IInput>().FromComponentInNewPrefab(keyboardInputControllerPrefab).AsSingle().NonLazy();
            // else
            //     Container.BindInterfacesAndSelfTo<IInput>().FromComponentInNewPrefab(joystickInputControllerPrefab).AsSingle().NonLazy();
        }
    }
}