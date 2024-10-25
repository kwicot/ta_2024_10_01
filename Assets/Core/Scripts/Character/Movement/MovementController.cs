using Core.Scripts.Character.Input;
using UnityEngine;
using Zenject;

namespace Core.Scripts.Character
{
    [RequireComponent(typeof(Rigidbody))]
    public class MovementController : MonoBehaviour, IMovementController
    {
        [SerializeField] [Header("Movement")] private float moveSpeed;
        [SerializeField] private float acceleration = 10f;
        [SerializeField] private float slopeLimit = 45f;

        [SerializeField] [Header("GroundRay")] private Vector3 rayOffset;
        [SerializeField] private float rayDistance;
        [SerializeField] private float rayAngle;
        [SerializeField] private LayerMask rayLayer;

        [SerializeField] [Header("Step")] private float stepHeight = 0.5f;
        [SerializeField] private float stepSmooth = 0.1f;
        [SerializeField] private float stepRayDistance;

        [SerializeField] private bool debug;

        [Inject] private IInput _input;
        private bool _isCanMoveForwad;

        private Rigidbody _rb;


        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            var targetDirection = new Vector3(_input.Horizontal, 0f, _input.Vertical);

            _isCanMoveForwad = CanMoveForward();

            if (targetDirection.magnitude > 0f && _isCanMoveForwad)
                Velocity = Vector3.Lerp(Velocity, targetDirection, acceleration * Time.deltaTime);
            else
                Velocity = Vector3.Lerp(Velocity, Vector3.zero, acceleration * Time.deltaTime);

            StepClimb();
        }


        private void FixedUpdate()
        {
            if (_isCanMoveForwad)
            {
                var targetPosition = _rb.position + Velocity * moveSpeed * Time.fixedDeltaTime;
                _rb.MovePosition(targetPosition);
            }
        }


        private void OnDrawGizmos()
        {
        }

        public Vector3 Velocity { get; private set; }
        public float MoveSpeed => Velocity.magnitude * moveSpeed;

        private void StepClimb()
        {
            var origin = transform.position + Vector3.up * 0.1f;
            var direction = transform.forward;

            if (debug)
                Debug.DrawRay(origin, direction * stepRayDistance, Color.red);

            if (Physics.Raycast(origin, direction, out var hitLower, stepRayDistance, rayLayer))
            {
                origin = transform.position + Vector3.up * stepHeight;

                if (debug)
                    Debug.DrawRay(origin, direction * stepRayDistance, Color.red);

                if (!Physics.Raycast(origin, direction, out var hitUpper, stepRayDistance, rayLayer))
                    _rb.MovePosition(transform.position + Vector3.up * stepSmooth);
            }
        }


        private bool CanMoveForward()
        {
            var startPosition = transform.position + rayOffset;
            var rotation = Quaternion.AngleAxis(rayAngle, transform.right);
            var rotatedDirection = rotation * transform.forward;

            var ray = new Ray(startPosition, rotatedDirection);
            Debug.DrawRay(startPosition, rotatedDirection * rayDistance, Color.red);

            if (Physics.Raycast(ray, out var hitInfo, rayDistance, rayLayer))
                if (hitInfo.transform.CompareTag("Hexagon"))
                    return true;

            return false;
        }
    }
}