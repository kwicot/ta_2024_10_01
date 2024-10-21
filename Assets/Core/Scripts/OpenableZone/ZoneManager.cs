using System.Collections.Generic;
using Kwicot.Core.Scripts.Utils;
using UnityEngine;

namespace Core.Scripts.OpenableZone
{
    public class ZoneManager : MonoBehaviour
    {
        private Dictionary<string, OpennableZone> zoneMap = new();

        private void Awake()
        {
            var zones = FindObjectsByType<OpennableZone>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (var opennableZone in zones)
            {
                if (opennableZone.TryGetComponent(out UniqueIdentifier uniqueIdentifier))
                {
                    if (!zoneMap.TryAdd(uniqueIdentifier.GetUniqueID(), opennableZone))
                    {
                        Debug.LogError($"Zone {opennableZone.name} has not unique id");
                    }
                }
            }
        }

        public GameObject Get(string id)
        {
            
        }

        public void Disable(string id)
        {
            
        }

        public void Enable(string id)
        {
            
        }
    }
}