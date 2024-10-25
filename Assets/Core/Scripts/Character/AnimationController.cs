using UnityEngine;
using Zenject;

namespace Core.Scripts.Character
{
    [RequireComponent(typeof(Animator))]
    public class AnimationController : MonoBehaviour
    {
        private const string _speedParameterName = "speed";
        private Animator _animator;
        [Inject] private IMovementController _movementController;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            var speed = _movementController.Velocity.magnitude;
            _animator.SetFloat(_speedParameterName, speed);
        }
    }
}