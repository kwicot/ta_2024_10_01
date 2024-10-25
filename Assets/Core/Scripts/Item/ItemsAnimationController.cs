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
            var startPosition = obj.transform.position;

            var path = GenerateParabolicTrajectory(startPosition, targetPosition, 5).ToArray();

            var randomRotationAxis =
                new Vector3(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f)).normalized;
            var sequence = DOTween.Sequence();
            sequence.Append(obj.transform.DOPath(path, throwDuration, PathType.CatmullRom).SetEase(Ease.InFlash));
            sequence.Join(obj.transform.DORotate(randomRotationAxis * rotationAmount, throwDuration,
                RotateMode.FastBeyond360));

            await sequence.AsyncWaitForCompletion();
        }

        public List<Vector3> GenerateParabolicTrajectory(Vector3 startPosition, Vector3 endPosition, int pointCount)
        {
            var points = new List<Vector3>();

            var randomOffsetX = Random.Range(-randomOffsetRange, randomOffsetRange);
            var randomOffsetZ = Random.Range(-randomOffsetRange, randomOffsetRange);

            endPosition = new Vector3(endPosition.x + randomOffsetX, endPosition.y, endPosition.z + randomOffsetZ);

            for (var i = 0; i < pointCount; i++)
            {
                var t = (float)i / (pointCount - 1);

                var point = Vector3.Lerp(startPosition, endPosition, t);

                var parabolaHeight = 4 * arcHeight * (t - t * t);

                point += new Vector3(0, parabolaHeight, 0);

                points.Add(point);
            }

            return points;
        }
    }
}