using Core.Scripts.Character.Input;
using UnityEngine;
using Zenject;

namespace Core.Scripts.Character
{
    public class MoveRotation : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 10f;

        [Inject] private IInput _input;

        private Vector3 InputVector => new(_input.Horizontal, 0, _input.Vertical);

        private void Update()
        {
            var moveDirection = InputVector;
            moveDirection.y = 0;

            if (moveDirection.magnitude > 0)
            {
                moveDirection.Normalize();

                var targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation =
                    Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}