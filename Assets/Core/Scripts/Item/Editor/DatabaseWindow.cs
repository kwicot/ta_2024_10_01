using System.IO;
using Kwicot.Core.Scripts.Utils;
using MyBox;
using UnityEditor;
using UnityEngine;

namespace Core.Items.Editor
{
    public class DatabaseWindow : EditorWindow
    {
        private Vector2 _scrollPosition;
        private ItemDatabaseSO ItemDatabaseSO => ItemDatabaseSO.Instance;


        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal("box");
            if (GUILayout.Button("Add Item"))
            {
                var item = CreateInstance<ItemSO>();
                item.SetGuid(UtilsClass.GenerateUniqueID());
                item.ItemName = item.UID;

                UtilsClass.SaveScriptableObjectItem(item, Constants.ItemsFolderName, item.ItemName);
                EditorUtility.SetDirty(item);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                ItemDatabaseSO.LoadItems();
            }

            if (GUILayout.Button("Save"))
            {
                foreach (var item in ItemDatabaseSO.ItemsMap.Values)
                {
                    var path = AssetDatabase.GetAssetPath(item);
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);

                    EditorUtility.SetDirty(item);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();

                    //Rename
                    if (fileNameWithoutExtension != item.ItemName)
                    {
                        var result = AssetDatabase.RenameAsset(path, item.ItemName);
                        if (result.IsNullOrEmpty())
                        {
                            EditorUtility.SetDirty(item);
                            AssetDatabase.SaveAssets();
                            AssetDatabase.Refresh();
                            Debug.Log($"Renamed to {item.ItemName}");
                        }
                        else
                        {
                            Debug.Log(result);
                        }
                    }
                }

                ItemDatabaseSO.Save();
            }

            EditorGUILayout.EndHorizontal();


            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            foreach (var item in ItemDatabaseSO.ItemsMap.Values)
            {
                EditorGUILayout.BeginVertical("box");

                GUI.enabled = false;
                EditorGUILayout.TextField("UID", item.UID);
                GUI.enabled = true;

                item.ItemName = EditorGUILayout.TextField("File name", item.ItemName);
                item.ItemDisplayName = EditorGUILayout.TextField("Display name", item.ItemDisplayName);

                item.ItemDescription = EditorGUILayout.TextField("Description", item.ItemDescription);

                item.Sprite = EditorGUILayout.ObjectField("Sprite", item.Sprite, typeof(Sprite), false) as Sprite;
                item.Prefab =
                    EditorGUILayout.ObjectField("Prefab", item.Prefab, typeof(GameObject), false) as GameObject;


                EditorGUILayout.BeginHorizontal("box");
                if (GUILayout.Button("Locate"))
                {
                    EditorUtility.FocusProjectWindow();
                    Selection.activeObject = item;
                }

                if (GUILayout.Button("Remove Item"))
                {
                    ItemDatabaseSO.ItemsMap.Remove(item.UID);
                    var path = Path.Combine(AssetDatabase.GetAssetPath(item));

                    AssetDatabase.DeleteAsset(path);
                    AssetDatabase.SaveAssets();

                    return;
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndScrollView();
        }


        [MenuItem("Window/Items database")]
        public static void ShowWindow()
        {
            var window = GetWindow<DatabaseWindow>("Items database");
        }
    }
}