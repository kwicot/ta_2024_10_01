using UnityEngine;
using Zenject;

namespace Core.Scripts.Character.Input
{
    public class InputServiceInstaller : MonoInstaller
    {
        [SerializeField] GameObject keyboardInputControllerPrefab;
        [SerializeField] GameObject joystickInputControllerPrefab;
        
        public override void InstallBindings()
        {
            if(Application.isEditor || Application.platform != RuntimePlatform.Android)
                Container.BindInterfacesAndSelfTo<IInput>().FromComponentInNewPrefab(keyboardInputControllerPrefab).AsSingle().NonLazy();
            else
                Container.BindInterfacesAndSelfTo<IInput>().FromComponentInNewPrefab(joystickInputControllerPrefab).AsSingle().NonLazy();
        }
    }
}