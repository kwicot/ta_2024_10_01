using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Scripts
{
    public class TriggerZone : MonoBehaviour
    {
        [SerializeField] private LayerMask triggerLayer;
        
        public UnityAction<Transform> onTriggerEnter;
        public UnityAction<Transform> onTriggerExit;
        public UnityAction<Transform> onTriggerStay;

        private string _triggerTag;

        private void OnTriggerEnter(Collider other)
        {
            if(TagInLayer(other.gameObject))
                onTriggerEnter?.Invoke(other.transform);
        }

        private void OnTriggerExit(Collider other)
        {
            if(TagInLayer(other.gameObject))
                onTriggerExit?.Invoke(other.transform);
        }

        private void OnTriggerStay(Collider other)
        {
            if(TagInLayer(other.gameObject))
                onTriggerStay?.Invoke(other.transform);
        }

        bool TagInLayer(GameObject gameObject) =>
            gameObject.layer == triggerLayer && gameObject.CompareTag(_triggerTag);

#if UNITY_EDITOR
        [UnityEditor.CustomEditor(typeof(TriggerZone))]
        public class TriggerZoneCustomEditor : UnityEditor.Editor
        {
            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();
                TriggerZone triggerZone = (TriggerZone)target;
                triggerZone._triggerTag = EditorGUILayout.TagField("Trigger tag", triggerZone._triggerTag);
            }
        }
        
#endif

    }
}