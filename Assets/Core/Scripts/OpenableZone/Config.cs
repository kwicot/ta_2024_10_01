using System.Collections.Generic;
using Core.Scripts.Item;
using UnityEngine;

namespace Core.OpennableZones
{
    [CreateAssetMenu(fileName = "New Config", menuName = "OpennableZone/Config")]
    public class Config : ScriptableObject
    {
        [SerializeField] private List<ItemCountModel> needItems;

        public List<ItemCountModel> Items => needItems;
    }
}