using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kwicot.Core.Scripts.Utils.SimplePoolSystem
{
    public class LazyPoolManager : MonoBehaviour
    {
        static LazyPoolManager _instance;
        public static LazyPoolManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject("PoolManager", typeof(LazyPoolManager))
                        .GetComponent<LazyPoolManager>();
                    
                    DontDestroyOnLoad(_instance.gameObject);
                    
                    Debug.LogWarning("PoolManager not found. Create new one");
                }

                return _instance;
            }
        }
        
        private Dictionary<GameObject, List<GameObject>> _prefabToListMap = new ();

        private void Awake()
        {
            if (_instance && _instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            _instance = this;
        }

        public void Populate(GameObject prefab, int count)
        {
            if (!_prefabToListMap.TryGetValue(prefab, out var list))
            {
                list = new List<GameObject>();
                _prefabToListMap.Add(prefab, list);
            }

            for (int i = 0; i < count; i++)
            {
                list.Add(Create(prefab));
            }
        }
        
        public GameObject Get(GameObject prefab)
        {
            if (_prefabToListMap.TryGetValue(prefab, out var list))
            {
                foreach (var obj in list)
                {
                    if (!obj.activeSelf)
                        return obj;
                }
                list.Add(Create(prefab));
                
            }
            else
            {
                list = new List<GameObject>();
                list.Add(Create(prefab));
                
                _prefabToListMap.Add(prefab, list);
            }
            
            return list[^1];
        }

        GameObject Create(GameObject prefab)
        {
            var obj = Instantiate(prefab);
            obj.SetActive(false);
            return obj;
        }


    }
}