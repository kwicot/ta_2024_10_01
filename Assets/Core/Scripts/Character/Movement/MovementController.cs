using Core.Scripts.Character.Input;
using MyBox;
using UnityEngine;
using Zenject;

namespace Core.Scripts.Character
{
    [RequireComponent(typeof(Rigidbody))]
    public class MovementController : MonoBehaviour, IMovementController
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float acceleration = 10f;
        [SerializeField] private float slopeLimit = 45f;
        
        [SerializeField] private Vector3 rayOffset;
        [SerializeField] private float rayDistance;
        [SerializeField] private float rayAngle;
        
        [Inject] private IInput _input;

        Rigidbody _rb;
        private bool _isCanMoveForwad;

        public Vector3 Velocity { get; private set; }
        public float MoveSpeed => Velocity.magnitude * moveSpeed;

        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            
        }

        private void Update()
        {
            Vector3 targetDirection = new Vector3(_input.Horizontal, 0f, _input.Vertical).normalized;

            _isCanMoveForwad = CanMoveForward();

            if (targetDirection.magnitude > 0f && _isCanMoveForwad)
            {
                Velocity = Vector3.Lerp(Velocity, targetDirection, acceleration * Time.deltaTime);
            }
            else
            {
                Velocity = Vector3.Lerp(Velocity, Vector3.zero, acceleration * Time.deltaTime); 
            }
        }
        
        
        private void FixedUpdate()
        {
            if (_isCanMoveForwad)
            {
                Vector3 targetPosition = _rb.position + Velocity * moveSpeed * Time.fixedDeltaTime;
                _rb.MovePosition(targetPosition);
            }
        }

        
        bool CanMoveForward()
        {
            Vector3 startPosition = transform.position + rayOffset;
            Quaternion rotation = Quaternion.AngleAxis(rayAngle, transform.right);
            Vector3 rotatedDirection = rotation * transform.forward;
            
            Ray ray = new Ray(startPosition, rotatedDirection);
            Debug.DrawRay(startPosition, rotatedDirection * rayDistance, Color.red);
            
            if (Physics.Raycast(ray, out RaycastHit hitInfo, rayDistance))
            {
                if (hitInfo.transform.CompareTag("Hexagon"))
                    return true;
            }

            return false;
        }



        private void OnDrawGizmos()
        {
        }

    }
}