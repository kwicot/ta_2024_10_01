using UnityEngine;

namespace Core.Scripts.Character.Input
{
    public class JoystickInputController : MonoBehaviour, IInput
    {
        public float Horizontal { get; private set; }
        public float Vertical { get; private set; }


        private void Update()
        {
            Horizontal = SimpleInput.GetAxis("Horizontal");
            Vertical = SimpleInput.GetAxis("Vertical");
        }
    }
}