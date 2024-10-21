using Core.Items;
using Core.Items.Editor;
using UnityEditor;
using UnityEngine;

namespace Core.Scripts.Item
{
    [CustomEditor(typeof(ItemSO))]
    public class ItemSoEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.LabelField("To edit items. Go Windows/Items database");
            if (GUILayout.Button("Open window"))
            {
                EditorWindow.GetWindow<DatabaseWindow>("Items database");
            }
        }
    }
}