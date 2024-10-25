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
        private const int _itemsSpawnPerSecond = 10;
        [SerializeField] private HexController openableHex;

        [SerializeField] private TriggerZone fillTrigger;
        [SerializeField] private TriggerZone showTrigger;
        [SerializeField] private Config config;
        [SerializeField] private Vector3 itemSpawnOffset = new(0, 1f, 0);

        private Coroutine _fillCoroutine;

        private Inventory _targetInventory;

        private ZoneWindow _zoneWindow;
        [Inject] private ItemsAnimationController itemsAnimationController;

        public UnityAction OnCollectedItemsChanged;

        [Inject] private ZoneWindowPool zoneWindowPool;

        public Dictionary<string, int> CollectedItemsMap { get; private set; }

        public Config Config => config;

        public Transform OpennableTransform => openableHex.transform;


        private void Awake()
        {
            fillTrigger.onTriggerEnter += OnEnterFillTrigger;
            fillTrigger.onTriggerExit += OnExitFillTrigger;

            showTrigger.onTriggerEnter += OnEnterShowTrigger;
            showTrigger.onTriggerExit += OnExitShowTrigger;

            CollectedItemsMap = new Dictionary<string, int>();
        }

        private void Start()
        {
            openableHex.SetEnable(false, false);
        }


        private IEnumerator FillUpdateCoroutine()
        {
            var fillInterval = 1f / _itemsSpawnPerSecond;

            foreach (var itemCountModel in config.Items)
            {
                if (!_targetInventory.Contains(itemCountModel.ItemSO))
                {
                    continue;
                }

                if (CollectedItemsMap.TryGetValue(itemCountModel.ItemSO.UID, out var count))
                {
                    var need = itemCountModel.Count - count;
                    if (need <= 0)
                    {
                        continue;
                    }

                    for (var i = 0; i < need; i++)
                    {
                        SpawnItem(itemCountModel.ItemSO);
                        _targetInventory.RemoveItem(itemCountModel.ItemSO);

                        yield return new WaitForSeconds(fillInterval);
                    }
                }
                else
                {
                    for (var i = 0; i < itemCountModel.Count; i++)
                    {
                        SpawnItem(itemCountModel.ItemSO);
                        _targetInventory.RemoveItem(itemCountModel.ItemSO);

                        yield return new WaitForSeconds(fillInterval);
                    }
                }
            }

        }

        private void Fill(ItemSO item)
        {
            if (!CollectedItemsMap.TryAdd(item.UID, 1))
                CollectedItemsMap[item.UID]++;

            OnCollectedItemsChanged?.Invoke();

            foreach (var itemCountModel in config.Items)
                if (!CollectedItemsMap.TryGetValue(itemCountModel.ItemSO.UID, out var count) ||
                    count < itemCountModel.Count)
                    return;

            OnFilled();
        }

        private async void OnFilled()
        {
            await openableHex.SetEnable(true);
            await HideWindow();
            await HideFillPanel();

            Destroy(fillTrigger.gameObject);
            Destroy(showTrigger.gameObject);
            Destroy(gameObject);
        }

        private async void SpawnItem(ItemSO item)
        {
            var position = _targetInventory.transform.position;
            var spawnPosition = position + _targetInventory.transform.right * itemSpawnOffset.x
                                         + _targetInventory.transform.up * itemSpawnOffset.y
                                         + _targetInventory.transform.forward * itemSpawnOffset.z;

            var obj = item.Prefab.Spawn(spawnPosition, Quaternion.identity);

            var targetPosition = OpennableTransform.position - Vector3.down;
            await itemsAnimationController.Throw(obj, targetPosition);
            obj.Release();

            Fill(item);
        }

        private void ShowWindow()
        {
            _zoneWindow = zoneWindowPool.GetZoneWindow();
            _zoneWindow.Show(this);
        }

        private async Task HideWindow()
        {
            await _zoneWindow.Hide();
            zoneWindowPool.ReturnZoneWindow(_zoneWindow);
        }

        private void ShowFillPanel()
        {
            fillTrigger.gameObject.SetActive(true);
            fillTrigger.transform.DOScale(Vector3.one, Constants.ShowHideAnimationDuration);
        }

        private async Task HideFillPanel()
        {
            await fillTrigger.transform.DOScale(Vector3.zero, Constants.ShowHideAnimationDuration)
                .AsyncWaitForCompletion();
            fillTrigger.gameObject.SetActive(false);
        }

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

            if (_targetInventory != null) _fillCoroutine = StartCoroutine(FillUpdateCoroutine());
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
    }
}