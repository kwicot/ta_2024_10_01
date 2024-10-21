using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Core.Scripts.OpenableZone.Editor
{
    [CustomEditor(typeof(OpennableZone))]
    public class OpennableZoneEditor : UnityEditor.Editor
    {
        static int _rayDistance = 2;
        static int _angle = 60;
        static int _angleOffset = 30;
        private static float _rayOffsetY = -2;
        
        private bool _isHighlighted = false;
        private float _highlightTime = 2f;
        private float _highlightDuration = 2f;
        GameObject _highlightObject;

        private int selectedNeighbour = -1;
        
        public override void OnInspectorGUI()
        {
            var opennableZone = (OpennableZone) target;
            
            DrawDefaultInspector();

            EditorGUILayout.BeginVertical("box");
            
            EditorGUILayout.LabelField("Neighbour helper", EditorStyles.boldLabel);
            _rayDistance = EditorGUILayout.IntField("Ray Distance", _rayDistance);
            _angle = EditorGUILayout.IntField("Angle", _angle);
            _angleOffset = EditorGUILayout.IntField("Angle Offset", _angleOffset);
            _rayOffsetY = EditorGUILayout.FloatField("Ray Offset Y", _rayOffsetY);
            
            EditorGUILayout.Space(10);
            
            EditorGUILayout.LabelField("Highlight");
            _highlightTime = EditorGUILayout.FloatField("Highlight Duration", _highlightTime);
            
            EditorGUILayout.EndVertical();

            if (GUILayout.Button("Select neighbour"))
            {
                //opennableZone.GetPrivateProperty<HexController>("openableHex");
                var neighbours = GetNeighbours(opennableZone.transform.parent);
                selectedNeighbour++;

                Debug.Log($"Selected {selectedNeighbour} count {neighbours.Count}");
                
                if (selectedNeighbour >= neighbours.Count)
                    selectedNeighbour = 0;

                if (neighbours.Count > 0)
                {
                    _highlightObject = neighbours[selectedNeighbour];
                    Debug.Log($"Select{_highlightObject.name}");

                    opennableZone.SetTargetHex(_highlightObject);
                    StartHighlight();
                }
            }
        }

        private List<GameObject> GetNeighbours(Transform rootTransform)
        {
            var list = new List<GameObject>();
            
            for (int i = 0; i < 360; i += _angle)
            {
                Vector3 direction = rootTransform.transform.TransformDirection(Quaternion.Euler(0, i + _angleOffset, 0) * Vector3.forward);
                
                RaycastHit hit;
                
                Vector3 origin = rootTransform.position;
                origin.y += _rayOffsetY;
                
                if (Physics.Raycast(origin, direction, out hit, _rayDistance))
                {
                    Debug.DrawRay(origin, direction * _rayDistance, Color.red, _highlightTime);
                    list.Add(hit.transform.root.gameObject);
                }
                else
                {
                    Debug.DrawRay(origin, direction * _rayDistance, Color.green, _highlightTime);
                }
            }

            return list;
        }
        
        private void StartHighlight()
        {
            _isHighlighted = true;
            _highlightDuration = _highlightTime * 100;
            EditorApplication.update += UpdateHighlight;
        }
        private void UpdateHighlight()
        {
            _highlightDuration -= Time.deltaTime;

            if (_highlightDuration <= 0)
            {
                _isHighlighted = false;
                EditorApplication.update -= UpdateHighlight;
            }

            SceneView.RepaintAll();
        }
        
        private void OnSceneGUI()
        {
            if (_isHighlighted)
            {
                MeshFilter[] meshFilters = _highlightObject.GetComponentsInChildren<MeshFilter>();
                foreach (var meshFilter in meshFilters)
                {
                    Mesh mesh = meshFilter.sharedMesh;

                    if (mesh != null)
                    {
                        Handles.color = Color.red;

                        for (int i = 0; i < mesh.triangles.Length; i += 3)
                        {
                            Vector3 p0 = meshFilter.transform.TransformPoint(mesh.vertices[mesh.triangles[i]]);
                            Vector3 p1 = meshFilter.transform.TransformPoint(mesh.vertices[mesh.triangles[i + 1]]);
                            Vector3 p2 = meshFilter.transform.TransformPoint(mesh.vertices[mesh.triangles[i + 2]]);

                            Handles.DrawLine(p0, p1);
                            Handles.DrawLine(p1, p2);
                            Handles.DrawLine(p2, p0);
                        }
                    }
                }
            }
        }
    }
}