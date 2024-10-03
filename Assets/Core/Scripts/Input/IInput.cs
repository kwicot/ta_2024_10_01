using UnityEngine;

namespace Core.Scripts.Character.Input
{
    public interface IInput
    {
        public float Horizontal { get; }
        public float Vertical { get; }
    }
}