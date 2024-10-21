using UnityEngine;

namespace Kwicot.Core.Scripts.Utils.SimplePoolSystem
{
    public static class GameObjectExtensions
    {
        public static GameObject Spawn(this GameObject gameObject, Vector3 position, Quaternion rotation)
        {
            if (gameObject != null)
            {
                var obj = LazyPoolManager.Instance.Get(gameObject);

                obj.SetActive(true);
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                
                return obj;
            }

            return null;
        }

        public static void Release(this GameObject gameObject)
        {
            if(gameObject != null)
                gameObject.SetActive(false);
        }

        public static void Populate(GameObject gameObject, int count)
        {
            if (gameObject != null)
            {
                LazyPoolManager.Instance.Populate(gameObject, count);
            }
        }
    }
}