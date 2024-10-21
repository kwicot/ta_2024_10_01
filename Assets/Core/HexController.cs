using DG.Tweening;
using UnityEngine;

namespace Core
{
    public class HexController : MonoBehaviour
    {
        [SerializeField] private GameObject rootVisual;
        
        const float _visualAnimationDuration = 0.2f;
        
        public async void SetEnable(bool enable)
        {
            if (enable && !rootVisual.activeSelf)
            {
                rootVisual.SetActive(true);
                rootVisual.transform.DOScale(Vector3.one, _visualAnimationDuration);
            }
            else if(!enable && rootVisual.activeSelf)
            {
                await rootVisual.transform.DOScale(Vector3.zero, _visualAnimationDuration).AsyncWaitForCompletion();
                rootVisual.SetActive(false);
            }
        }
    }
}