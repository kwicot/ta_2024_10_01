using UnityEngine;

namespace Core.Scripts.Character.Input
{
    public class JoystickInputController : MonoBehaviour, IInput
    {
        private void Update()
        {
            Horizontal = SimpleInput.GetAxis("Horizontal");
            Vertical = SimpleInput.GetAxis("Vertical");
        }

        public float Horizontal { get; private set; }
        public float Vertical { get; private set; }
    }
}