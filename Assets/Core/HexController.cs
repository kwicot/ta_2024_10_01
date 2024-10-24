using System.Threading.Tasks;
using DG.Tweening;
using Kwicot.Core.Scripts.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    public class HexController : MonoBehaviour
    {
        [SerializeField] private GameObject rootVisual;
        
        Tween _currentTween;
        
        public async Task SetEnable(bool enable, bool animation = true)
        {
            Debug.Log($"SetEnable: [{enable}] Animation: [{animation}]");
            if (_currentTween != null)
                _currentTween.Complete();
            
            if (enable && !rootVisual.activeSelf)
            {
                rootVisual.SetActive(true);
                _currentTween = rootVisual.transform.DOScale(Vector3.one, animation ? Constants.ShowHideAnimationDuration : 0);
                await _currentTween.AsyncWaitForCompletion();
            }
            else if(!enable && rootVisual.activeSelf)
            {
                _currentTween =
                    rootVisual.transform.DOScale(Vector3.zero, animation ? Constants.ShowHideAnimationDuration : 0);
                await _currentTween.AsyncWaitForCompletion();
                rootVisual.SetActive(false);
            }
        }
    }
}