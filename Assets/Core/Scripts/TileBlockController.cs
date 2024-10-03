using UnityEngine;

namespace Core.Scripts
{
    public class TileBlockController : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            Debug.Log(other.gameObject);
        }
    }
}