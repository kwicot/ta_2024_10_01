using UnityEngine;
using Zenject;

namespace Core.Scripts.Character
{
    [RequireComponent(typeof(Animator))]
    public class AnimationController : MonoBehaviour
    {
        [Inject] IMovementController _movementController;
        Animator _animator;

        private const string _speedParameterName = "speed";

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            float speed = _movementController.Velocity.magnitude;
            _animator.SetFloat(_speedParameterName, speed);
        }
    }
}