using System;
using Core.Scripts.Character.Input;
using UnityEngine;
using Zenject;

namespace Core.Scripts.Character
{
    public class MoveRotation : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 10f;
        
        [Inject] IInput _input;
        
        Vector3 InputVector => new Vector3(_input.Horizontal, 0, _input.Vertical);
        
        void Update()
        {
            Vector3 moveDirection = InputVector;
            moveDirection.y = 0;

            if (moveDirection.magnitude > 0)
            {
                moveDirection.Normalize();

                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}