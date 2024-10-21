using System.Collections.Generic;
using Kwicot.Core.Scripts.Utils;
using UnityEngine;

namespace Core.Items
{
    public class ItemDatabaseSO : ScriptableObject
    {
        static ItemDatabaseSO _instance = null;
        public static ItemDatabaseSO Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<ItemDatabaseSO>("ItemDatabaseSO");
#if UNITY_EDITOR
                    if (_instance == null)
                    {
                        _instance = ScriptableObject.CreateInstance<ItemDatabaseSO>();
                        _instance.LoadItems();
                        
                        UtilsClass.SaveScriptableObjectItem(_instance,Constants.ItemsFolderName, "ItemDatabaseSO");
                        
                        UnityEditor.EditorUtility.SetDirty(_instance);
                        UnityEditor.AssetDatabase.SaveAssets();
                    }
#else
                    Debug.LogError("Items database not found");
#endif
                }

                return _instance;
            }
        }

        
        public Dictionary<string, ItemSO> ItemsMap;
        
        public void LoadItems()
        {
            var items = Resources.LoadAll<ItemSO>("Items");
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
            UnityEditor.AssetDatabase.SaveAssets();
        }
        
        #endif
    }
}