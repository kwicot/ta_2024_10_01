using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Core.Scripts.Item
{
    public class ItemsAnimationController : MonoBehaviour
    {
        [SerializeField] private float throwDuration = 2f;
        [SerializeField] private float arcHeight = 2;
        [SerializeField] private float randomOffsetRange;
        [SerializeField] private int rotationAmount = 2;
        
        public async Task Throw(GameObject obj, Vector3 targetPosition)
        {
            Vector3 startPosition = obj.transform.position;

            Vector3[] path = GenerateParabolicTrajectory(startPosition, targetPosition, 5).ToArray();
            
            Vector3 randomRotationAxis = new Vector3(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f)).normalized;
            Sequence sequence = DOTween.Sequence();
            sequence.Append(obj.transform.DOPath(path, throwDuration, PathType.CatmullRom).SetEase(Ease.InFlash));
            sequence.Join(obj.transform.DORotate(randomRotationAxis * rotationAmount, throwDuration, RotateMode.FastBeyond360));
            
            await sequence.AsyncWaitForCompletion();
        }
        
        public List<Vector3> GenerateParabolicTrajectory(Vector3 startPosition, Vector3 endPosition, int pointCount)
        {
            List<Vector3> points = new List<Vector3>();

            float randomOffsetX = Random.Range(-randomOffsetRange, randomOffsetRange);
            float randomOffsetZ = Random.Range(-randomOffsetRange, randomOffsetRange);
            
            endPosition = new Vector3(endPosition.x + randomOffsetX, endPosition.y, endPosition.z + randomOffsetZ);
            
            for (int i = 0; i < pointCount; i++)
            {
                float t = (float)i / (pointCount - 1); 
                
                Vector3 point = Vector3.Lerp(startPosition, endPosition, t);
                
                float parabolaHeight = 4 * arcHeight * (t - t * t);

                point += new Vector3(0, parabolaHeight, 0);
            
                points.Add(point);
            }

            return points;
        }
    }
}