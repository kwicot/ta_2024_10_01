using UnityEngine;

namespace Core.Scripts.Character
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Inventory inventory;
        
        public Inventory Inventory => inventory;
    }
}