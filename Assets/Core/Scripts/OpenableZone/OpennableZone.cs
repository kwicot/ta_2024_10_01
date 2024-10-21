using System.Collections;
using System.Collections.Generic;
using Core.Items;
using Core.OpennableZones;
using Core.Scripts;
using Kwicot.Core.Scripts.Utils.SimplePoolSystem;
using UnityEngine;

namespace Core
{
    public class OpennableZone : MonoBehaviour
    {
        [SerializeField] HexController openableHex;
        [SerializeField] OpennableZone openableZone;
        
        [SerializeField] private TriggerZone triggerZone;
        [SerializeField] private Config config;
        
        private const int _itemsSpawnPerSecond = 10;
        private const int _itemThrowAngle = 40;
        private Vector3 _itemSpawnOffset = new Vector3(0, 1f, 0);
        

        private Dictionary<string, int> _collectedItemsMap;

        private Coroutine _fillCoroutine;
        private Inventory _targetInventory;

        private void Awake()
        {
            triggerZone.onTriggerEnter += OnEnter;
            triggerZone.onTriggerExit += OnExit;
            triggerZone.onTriggerStay += OnStay;
        }

        private void Start()
        {
            openableHex.SetEnable(false);
        }

        public void SetTargetHex(GameObject go)
        {
            if (go.TryGetComponent(out HexController hexController))
            {
                openableHex = hexController;
            }
        }

        private void OnStay(Transform arg) { }

        private void OnExit(Transform arg)
        {
            if (_fillCoroutine != null)
                StopCoroutine(_fillCoroutine);
            
            _fillCoroutine = null;
            _targetInventory = null;
        }

        private void OnEnter(Transform arg)
        {
            _targetInventory = arg.GetComponent<Inventory>();
            if (_targetInventory != null)
            {
                _fillCoroutine = StartCoroutine(Fill());
            }
        }

        IEnumerator Fill()
        {
            float spawnInterval = _itemsSpawnPerSecond / 60f;

            while (true)
            {
                foreach (var item in config.Items)
                {
                    if (!_targetInventory.Contains(item.ItemSO))
                    {
                        continue;
                    }

                    if (_collectedItemsMap.TryGetValue(item.ItemSO.UID, out var currentCount))
                    {
                        var itemsToCollect = item.Count - currentCount;

                        if (itemsToCollect <= 0)
                        {
                            continue;
                        }

                        _collectedItemsMap[item.ItemSO.UID]++;
                    }
                    else
                    {
                        _collectedItemsMap[item.ItemSO.UID] = 1;
                    }

                    SpawnItem(item.ItemSO);
                    yield return new WaitForSecondsRealtime(spawnInterval);
                }
            }
        }

        void SpawnItem(ItemSO item)
        {
            var obj = item.Prefab.Spawn(_targetInventory.transform.position + _itemSpawnOffset, Quaternion.identity);
            if (obj.TryGetComponent(out Rigidbody rigidbody))
            {
                rigidbody.velocity = Vector3.zero;
                
                Vector3 force = CalculateLaunchVelocity(obj.transform.position, openableHex.transform.position, _itemThrowAngle);

                rigidbody.AddForce(force, ForceMode.VelocityChange);
            }
            else
            {
                Debug.LogWarning($"Cant kick item. Cant find [Rigidbody] component on object [{obj.name}]");
            }
        }
        
        private Vector3 CalculateLaunchVelocity(Vector3 start, Vector3 target, float angle)
        {
            Vector3 direction = target - start;
            direction.y = 0; 
            float distance = direction.magnitude;

            float heightDifference = target.y - start.y;

            float angleRad = angle * Mathf.Deg2Rad;

            float velocitySquared = (Physics.gravity.y * distance * distance) / 
                                    (2 * (heightDifference - distance * Mathf.Tan(angleRad)) * Mathf.Pow(Mathf.Cos(angleRad), 2));

            if (velocitySquared <= 0)
            {
                Debug.LogWarning("Cant throw. Not enough data");
                return Vector3.zero;
            }

            float initialVelocity = Mathf.Sqrt(velocitySquared);

            Vector3 velocityXZ = direction.normalized * initialVelocity * Mathf.Cos(angleRad);
            float velocityY = initialVelocity * Mathf.Sin(angleRad);

            Vector3 resultVelocity = velocityXZ;
            resultVelocity.y = velocityY;

            return resultVelocity;
        }
    }
}