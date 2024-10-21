using System;
using MyBox;
using UnityEngine;

namespace Kwicot.Core.Scripts.Utils
{
    [ExecuteAlways]
    public class UniqueIdentifier : MonoBehaviour
    {
        [SerializeField][ReadOnly] private string _uniqueID;

        public string GetUniqueID()
        {
            return _uniqueID;
        }

        #if UNITY_EDITOR
        [UnityEditor.CustomEditor(typeof(UniqueIdentifier))]
        public class UniqueIdentifierEditor : UnityEditor.Editor
        {
            public override void OnInspectorGUI()
            {
                var uniqueIdentifier = (UniqueIdentifier)target;
                DrawDefaultInspector();

                if (GUILayout.Button("Set UniqueID"))
                {
                    uniqueIdentifier._uniqueID = Guid.NewGuid().ToString();
                    
                    UnityEditor.EditorUtility.SetDirty(uniqueIdentifier);
                }
            }
        }
        #endif
    }
}