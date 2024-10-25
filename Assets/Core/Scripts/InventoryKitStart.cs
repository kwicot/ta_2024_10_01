using System.Collections.Generic;
using Core.Scripts.Item;
using UnityEngine;

namespace Core.Scripts
{
    [RequireComponent(typeof(Inventory))]
    public class InventoryKitStart : MonoBehaviour
    {
        [SerializeField] private List<ItemCountModel> items;

        private Inventory _targetInventory;

        private void Awake()
        {
            _targetInventory = GetComponent<Inventory>();
        }

        private void Start()
        {
            foreach (var itemCountModel in items)
                _targetInventory.AddItem(itemCountModel.ItemSO.UID, itemCountModel.Count);

            Destroy(this);
        }
    }
}