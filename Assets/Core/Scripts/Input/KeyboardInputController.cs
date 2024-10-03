using UnityEngine;

namespace Core.Scripts.Character.Input
{
    public class KeyboardInputController : MonoBehaviour, IInput
    {
        public float Horizontal { get; private set; }
        public float Vertical { get; private set; }


        private void Update()
        {
            Horizontal = UnityEngine.Input.GetAxis("Horizontal");
            Vertical = UnityEngine.Input.GetAxis("Vertical");
        }
    }
}