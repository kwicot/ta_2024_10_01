using System;
using UnityEngine;

namespace Core.Scripts
{
    public class FollowTarget : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset;
        
        [SerializeField] [Range(0.01f,1f)] private float lerpSpeed;
        [SerializeField] [Range(0.01f, 1f)] private float rotationSpeed;
       


        private void Start()
        {
            if(target == null)
                throw new NullReferenceException("target is null");
        }


        private void FixedUpdate()
        {
            Vector3 targetPosition = target.position;
            
            transform.position = Vector3.Lerp(transform.position, targetPosition + offset, lerpSpeed);

            Vector3 direction = (target.position - transform.position).normalized;
            
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed);
        }
    }
}