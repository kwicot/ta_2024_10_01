﻿using System.Collections.Generic;
using System.IO;
using Kwicot.Core.Scripts.Utils;
using UnityEditor;
using UnityEngine;

namespace Core.Items
{
    public class ItemDatabaseSO : ScriptableObject
    {
        static ItemDatabaseSO _instance = null;
        private const string _fileName = "ItemDatabaseSO";
        public static ItemDatabaseSO Instance
        {
            get
            {
                if (_instance == null)
                {
                    var path = Path.Combine(Constants.ItemsFolderName, _fileName);
                    _instance = Resources.Load<ItemDatabaseSO>(path);
#if UNITY_EDITOR
                    if (_instance == null)
                    {
                        _instance = ScriptableObject.CreateInstance<ItemDatabaseSO>();
                        _instance.LoadItems();
                        
                        UtilsClass.SaveScriptableObjectItem(_instance,Constants.ItemsFolderName, _fileName);
                        
                        UnityEditor.EditorUtility.SetDirty(_instance);
                        UnityEditor.AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                        
                    }
                    else
                    {
                        _instance.LoadItems();
                    }
#else
                    Debug.LogError("Items database not found");
#endif
                }

                return _instance;
            }
        }

        public Dictionary<string, ItemSO> ItemsMap;

        public ItemSO this[string uid] => ItemsMap.GetValueOrDefault(uid);

        public void LoadItems()
        {
            var items = Resources.LoadAll<ItemSO>(Constants.ItemsFolderName);
            Debug.Log($"[{_fileName}] Loaded {items.Length} items");
            
            ItemsMap??= new Dictionary<string, ItemSO>();
            
            foreach (var itemSo in items)
            {
                ItemsMap.TryAdd(itemSo.UID, itemSo);
            }
        }

#if UNITY_EDITOR
        
        public static void Save()
        {
            UnityEditor.EditorUtility.SetDirty(Instance);

            foreach (var itemSo in _instance.ItemsMap)
            {
                EditorUtility.SetDirty(itemSo.Value);
            }
            
            UnityEditor.AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        #endif
    }
}