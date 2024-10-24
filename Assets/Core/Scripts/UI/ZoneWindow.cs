using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Items;
using Core.Scripts.Character;
using DG.Tweening;
using Kwicot.Core.Scripts.Utils;
using Kwicot.Core.Scripts.Utils.SimplePoolSystem;
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
        [SerializeField] private Transform itemCellContainer;

        private OpennableZone _targetObject;
        private Camera _camera;
        private Vector3 _targetPosition;
        
        ItemDatabaseSO ItemDatabase => ItemDatabaseSO.Instance;

        [Inject] private Player player;

        private Tween _currentTween;

        private List<ItemCell> _cells;

        private void Awake()
        {
            _camera = Camera.main;
            _cells = new List<ItemCell>();
            Hide();
        }

        private void Start()
        {
        }

        public void Show(OpennableZone zone)
        {
            Debug.Log("Show");
            
            if (_currentTween != null)
                _currentTween.Complete();
            
            _targetObject = zone;
            rootPanel.SetActive(true);
            
            _currentTween = rootPanel.transform.DOScale(Vector3.one, Constants.ShowHideAnimationDuration);

            zone.OnCollectedItemsChanged += UpdateVisual;
            UpdateVisual();
        }
        
        public async Task Hide()
        {
            if(_targetObject != null)
                _targetObject.OnCollectedItemsChanged -= UpdateVisual;

            if (_currentTween != null)
                _currentTween.Complete();
            
            _targetObject = null;
            _currentTween = rootPanel.transform.DOScale(Vector3.zero, Constants.ShowHideAnimationDuration);
            await _currentTween.AsyncWaitForCompletion();
            
            rootPanel.SetActive(false);
        }
        
        private void UpdateVisual()
        {
            int i = 0;
            foreach (var keyPair in _targetObject.Config.Items)
            {
                var targetItem = keyPair.ItemSO;
                
                int needCount = keyPair.Count;
                int count = 0;
                _targetObject.CollectedItemsMap.TryGetValue(targetItem.UID, out count);
                
                ItemCell cell;
                if (_cells.Count > i)
                {
                    cell = _cells[i];
                }
                else
                {
                    cell = itemCellPrefab.Spawn().GetComponent<ItemCell>();
                    cell.transform.SetParent(itemCellContainer);
                    _cells.Add(cell);
                }
                
                cell.Init(targetItem, count, needCount);
                cell.transform.localScale = Vector3.one;
                
                i++;
            }
        }

        private void Update()
        {
            if (rootPanel.activeSelf && _targetObject != null)
            {
                rootPanel.transform.position = _camera.WorldToScreenPoint(_targetObject.OpennableTransform.position + offset) ;
            }
        }

        private void FixedUpdate()
        {
            if (rootPanel.activeSelf && _targetObject != null)
            {
                rootPanel.transform.position = Vector3.Lerp(rootPanel.transform.position, _targetPosition, lerpSpeed);
            }
        }
        
        public class Factory : PlaceholderFactory<ZoneWindow>
        {
        }
    }
}