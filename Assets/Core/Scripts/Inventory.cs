using System;
using System.Collections.Generic;
using Core.Items;
using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    public class Inventory : MonoBehaviour
    {
        public Dictionary<string, int> ItemsMap = new();

        public UnityAction OnItemsChanged;


        public bool Contains(ItemSO itemSo)
        {
            return Contains(itemSo.UID);
        }

        public bool Contains(string uid)
        {
            if (ItemsMap.TryGetValue(uid, out var count))
                return count > 0;

            return false;
        }

        public void AddItem(ItemSO item, int count = 1)
        {
            AddItem(item.UID, count);
        }

        public void AddItem(string id, int count = 1)
        {
            if (ItemsMap.ContainsKey(id))
                ItemsMap[id] += count;
            else
                ItemsMap.Add(id, count);

            OnItemsChanged?.Invoke();
        }

        public void RemoveItem(ItemSO itemSo, int count = 1)
        {
            RemoveItem(itemSo.UID, count);
        }

        public void RemoveItem(string id, int count = 1)
        {
            if (ItemsMap.ContainsKey(id))
            {
                ItemsMap[id] -= count;
                if (ItemsMap[id] <= 0)
                    ItemsMap.Remove(id);

                OnItemsChanged?.Invoke();
            }
            else
            {
                throw new NullReferenceException("Cant remove items from inventory. Item not found.");
            }
        }
    }
}