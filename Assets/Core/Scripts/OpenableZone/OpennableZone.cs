using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Items;
using Core.OpennableZones;
using Core.Scripts;
using Core.Scripts.Installers;
using Core.Scripts.Item;
using DG.Tweening;
using Kwicot.Core.Scripts.Utils;
using Kwicot.Core.Scripts.Utils.SimplePoolSystem;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Core
{
    public class OpennableZone : MonoBehaviour
    {
        [SerializeField] HexController openableHex;
        
        [SerializeField] private TriggerZone fillTrigger;
        [SerializeField] private TriggerZone showTrigger;
        [SerializeField] private Config config;

        [Inject] private ZoneWindowPool zoneWindowPool;
        [Inject] ItemsAnimationController itemsAnimationController; 
        
        private const int _itemsSpawnPerSecond = 10;
        private Vector3 _itemSpawnOffset = new Vector3(0, 1f, 0);

        private Dictionary<string, int> _collectedItemsMap;

        private Coroutine _fillCoroutine;
        
        private Inventory _targetInventory;
        
        public Dictionary<string, int> CollectedItemsMap => _collectedItemsMap;
        public Config Config => config;
        
        public Transform OpennableTransform => openableHex.transform;

        public UnityAction OnCollectedItemsChanged;

        private ZoneWindow _zoneWindow;
        
        
        
        private void Awake()
        {
            fillTrigger.onTriggerEnter += OnEnterFillTrigger;
            fillTrigger.onTriggerExit += OnExitFillTrigger;

            showTrigger.onTriggerEnter += OnEnterShowTrigger;
            showTrigger.onTriggerExit += OnExitShowTrigger;

            _collectedItemsMap = new Dictionary<string, int>();
        }

        private void Start()
        {
            openableHex.SetEnable(false, false);
        }


        IEnumerator FillUpdateCoroutine()
        {
            Debug.Log("StartFillCoroutine");
            float fillInterval = 1f / _itemsSpawnPerSecond;

            foreach (var itemCountModel in config.Items)
            {
                if (!_targetInventory.Contains(itemCountModel.ItemSO))
                {
                    Debug.Log($"Cant find {itemCountModel.ItemSO.ItemDisplayName} ion inventory");
                    continue;
                }

                if (_collectedItemsMap.TryGetValue(itemCountModel.ItemSO.UID, out int count))
                {
                    int need = itemCountModel.Count - count;
                    if (need <= 0)
                    {
                        Debug.Log($"Need {itemCountModel.ItemSO.ItemDisplayName} <= 0");
                        continue;
                    }

                    for (int i = 0; i < need; i++)
                    {
                        Debug.Log("Spawn");

                        SpawnItem(itemCountModel.ItemSO);
                        _targetInventory.RemoveItem(itemCountModel.ItemSO);

                        yield return new WaitForSeconds(fillInterval);
                    }
                }
                else
                {
                    for (int i = 0; i < itemCountModel.Count; i++)
                    {
                        Debug.Log("Spawn");

                        SpawnItem(itemCountModel.ItemSO);
                        _targetInventory.RemoveItem(itemCountModel.ItemSO);

                        yield return new WaitForSeconds(fillInterval);
                    }
                }
            }

            Debug.Log("End fillCoroutine");
        }

        void Fill(ItemSO item)
        {
            if(!_collectedItemsMap.TryAdd(item.UID, 1))
                _collectedItemsMap[item.UID]++;
            
            OnCollectedItemsChanged?.Invoke();

            foreach (var itemCountModel in config.Items)
            {
                if(!_collectedItemsMap.TryGetValue(itemCountModel.ItemSO.UID, out int count) || count < itemCountModel.Count)
                    return;
            }
            
            OnFilled();
        }

        async void OnFilled()
        {
            await openableHex.SetEnable(true);
            await HideWindow();
            await HideFillPanel();
            
            Destroy(fillTrigger.gameObject);
            Destroy(showTrigger.gameObject);
            Destroy(this.gameObject);
        }

        async void SpawnItem(ItemSO item)
        {
            var obj = item.Prefab.Spawn(_targetInventory.transform.position + _itemSpawnOffset, Quaternion.identity);
            obj.transform.position += _itemSpawnOffset;
            
            Vector3 targetPosition = OpennableTransform.position - Vector3.down;
            await itemsAnimationController.Throw(obj, targetPosition);
            obj.Release();
            
            Fill(item);
        }

        void ShowWindow()
        {
            _zoneWindow = zoneWindowPool.GetZoneWindow();
            _zoneWindow.Show(this);
        }

        async Task HideWindow()
        {
            await _zoneWindow.Hide();
            zoneWindowPool.ReturnZoneWindow(_zoneWindow);
        }

        void ShowFillPanel()
        {
            fillTrigger.gameObject.SetActive(true);
            fillTrigger.transform.DOScale(Vector3.one, Constants.ShowHideAnimationDuration);
        }

        async Task HideFillPanel()
        {
            await fillTrigger.transform.DOScale(Vector3.zero, Constants.ShowHideAnimationDuration).AsyncWaitForCompletion();
            fillTrigger.gameObject.SetActive(false);
        }
        
        
        #region TriggerCallbacks

        private void OnExitFillTrigger(Transform arg)
        {
            if (_fillCoroutine != null)
                StopCoroutine(_fillCoroutine);
            
            _fillCoroutine = null;
            _targetInventory = null;
        }

        private void OnEnterFillTrigger(Transform arg)
        {
            _targetInventory = arg.GetComponent<Inventory>();
            
            if (_targetInventory != null)
            { 
                _fillCoroutine = StartCoroutine(FillUpdateCoroutine());
            }
        }
        
        private void OnEnterShowTrigger(Transform arg0)
        {
            ShowWindow();
        }
        private void OnExitShowTrigger(Transform arg0)
        {
            HideWindow();
        }

        
        
        #endregion
        
#if UNITY_EDITOR
        public void SetTargetHex(GameObject go)
        {
            if (go.TryGetComponent(out HexController hexController))
            {
                openableHex = hexController;
                EditorUtility.SetDirty(this);
            }
        }
#endif
    }
}