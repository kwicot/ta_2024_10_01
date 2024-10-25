using Core.Items;
using Kwicot.Core.Scripts.Utils;
using UnityEngine;

namespace Core.Scripts
{
    public class TestScript : MonoBehaviour
    {
        private void Start()
        {
            var items = Resources.LoadAll<ItemSO>(Constants.ItemsFolderName);

            Debug.Log($"items count {items.Length}");
            
            var database = Resources.LoadAll<ItemDatabaseSO>(Constants.ItemsFolderName);
            Debug.Log($"Database count {database.Length}");
        }
    }
}