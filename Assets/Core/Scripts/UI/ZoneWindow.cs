using System;
using Core.Scripts.Character;
using UnityEngine;
using Zenject;

namespace Core
{
    public class ZoneWindow : MonoBehaviour
    {
        [SerializeField] private Vector3 offset;
        [SerializeField][Range(0.1f, 1f)] private float lerpSpeed;
        [SerializeField] private GameObject rootPanel;
        [SerializeField] private GameObject itemCellPrefab;
        [SerializeField] private GameObject itemCellContainer;

        private Transform _targetObject;
        private Camera _camera;
        private Vector3 _targetPosition;

        [Inject] private Player player;


        private void Awake()
        {
            _camera = Camera.main;
        }

        public void Show(OpennableZone zone)
        {
            _targetObject = zone.transform;
            rootPanel.SetActive(true);
        }
        
        public void Hide()
        {
            _targetObject = null;
            rootPanel.SetActive(false);
        }

        private void Update()
        {
            if (rootPanel.activeSelf && _targetObject != null)
            {
                _targetPosition = _camera.WorldToScreenPoint(_targetObject.position) + offset;
            }
        }

        private void FixedUpdate()
        {
            if (rootPanel.activeSelf && _targetObject != null)
            {
                rootPanel.transform.position = Vector3.Lerp(rootPanel.transform.position, _targetPosition, lerpSpeed);
            }
        }

        
    }
}