using System;
using System.Collections.Generic;
using Core.Items;
using Kwicot.Core.Scripts.Utils.SimplePoolSystem;
using UnityEngine;
using Zenject;
using Player = Core.Scripts.Character.Player;

namespace Core
{
    public class PlayerInventoryWindow : MonoBehaviour
    {
        [SerializeField] private GameObject itemCellPrefab;
        [SerializeField] private Transform itemCellContainer;

        ItemDatabaseSO ItemDatabase => ItemDatabaseSO.Instance;
        
        private Inventory _inventory;
        private List<ItemCell> _cells;

        [Inject]
        void Init(Player player)
        {
            _cells = new List<ItemCell>();
            _inventory = player.GetComponent<Inventory>();
            if(_inventory == null)
                throw new NullReferenceException("Cant find inventory on player");

            _inventory.OnItemsChanged += UpdateVisual;
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            int i = 0;
            foreach (var keyPair in _inventory.ItemsMap)
            {
                var item = ItemDatabase[keyPair.Key];
                int count = keyPair.Value;
                
                ItemCell cell;
                if (_cells.Count > i)
                {
                    cell = _cells[i];
                }
                else
                {
                    cell =itemCellPrefab.Spawn().GetComponent<ItemCell>();
                    cell.transform.SetParent(itemCellContainer);
                    _cells.Add(cell);
                }
                
                cell.Init(item, count);
                
                i++;
            }
        }

    }
}