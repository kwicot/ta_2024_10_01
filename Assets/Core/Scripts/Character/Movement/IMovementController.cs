using UnityEngine;

namespace Core.Scripts.Character
{
    public interface IMovementController
    {
        public Vector3 Velocity { get; }
        public float MoveSpeed { get; }
    }
}