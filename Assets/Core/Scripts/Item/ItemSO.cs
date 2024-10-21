using UnityEditor;
using UnityEngine;

namespace Core.Items
{
    public class ItemSO : ScriptableObject
    {
        public string UID { get; private set; }
        public string ItemName { get; set; }
        public string ItemDisplayName { get; set; }
        public string ItemDescription { get; set; }
        public Sprite Sprite { get; set; }
        public GameObject Prefab { get; set; }
        
#if UNITY_EDITOR
        public void SetGuid(string uid)
        {
            UID = uid;
            Save();
        }

        public void Save()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
#endif

    }
}