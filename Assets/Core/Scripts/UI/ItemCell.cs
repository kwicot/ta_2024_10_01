using Core.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class ItemCell : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text text;


        public void Init(ItemSO item, int count)
        {
            image.sprite = item.Sprite;
            text.text = $"{count:0}";
        }

        public void Init(ItemSO item, int count, int need)
        {
            image.sprite = item.Sprite;
            text.text = $"{count:0} / {need:0}";
        }
    }
}