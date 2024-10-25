using UnityEditor;
using UnityEngine;

namespace Core.Items
{
    public class ItemSO : ScriptableObject
    {
        public string UID;
        public string ItemName;
        public string ItemDisplayName;
        public string ItemDescription;
        public Sprite Sprite;
        public GameObject Prefab;

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
            AssetDatabase.Refresh();
        }
#endif
    }
}