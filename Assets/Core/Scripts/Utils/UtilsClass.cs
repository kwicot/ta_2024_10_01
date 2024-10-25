using System;
using System.IO;
using System.Net;
using UnityEngine;
using Random = System.Random;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Kwicot.Core.Scripts.Utils
{
    public static class UtilsClass
    {
        public static float AngleFromVectorFloat(Vector2 dir)
        {
            return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        }

        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://www.google.com"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static string GetAndroidAdvertiserId()
        {
            var advertisingID = "";
            try
            {
                var up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                var currentActivity = up.GetStatic<AndroidJavaObject>("currentActivity");
                var client = new AndroidJavaClass("com.google.android.gms.ads.identifier.AdvertisingIdClient");
                var adInfo = client.CallStatic<AndroidJavaObject>("getAdvertisingIdInfo", currentActivity);

                advertisingID = adInfo.Call<string>("getId");
            }
            catch (Exception e)
            {
                Debug.LogError($"Cant get advert id: {e.Message}");
            }

            return advertisingID;
        }

        public static void Shuffle<T>(T[] array)
        {
            var rng = new Random();
            var n = array.Length;
            while (n > 1)
            {
                var k = rng.Next(n--);
                var temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }

        public static T RoundToNearest<T>(T value, T nearest) where T : struct, IConvertible
        {
            var doubleValue = Convert.ToDouble(value);
            var doubleNearest = Convert.ToDouble(nearest);

            var result = Math.Round(doubleValue / doubleNearest) * doubleNearest;

            return (T)Convert.ChangeType(result, typeof(T));
        }

        public static string GenerateUniqueID()
        {
            var id = Guid.NewGuid();
            return id.ToString();
        }

        #region UnityEditor

        

#if UNITY_EDITOR
        public static string SaveScriptableObjectItem(ScriptableObject obj, string folderName, string fileName)
        {
            var folderPath = Path.Combine(Constants.ResourcesFolderPath, folderName);
            var path = Path.Combine(folderPath, $"{fileName}.asset");

            if (!AssetDatabase.IsValidFolder(folderPath))
                AssetDatabase.CreateFolder(folderPath, Constants.ItemsFolderName);

            AssetDatabase.CreateAsset(obj, path);
            AssetDatabase.SaveAssets();

            return path;
        }
#endif
        
        #endregion
       


        public static Vector3 CalculateLaunchVelocity(Vector3 start, Vector3 target, float angle)
        {
            var direction = target - start;
            direction.y = 0;
            var distance = direction.magnitude;

            var heightDifference = target.y - start.y;

            var angleRad = angle * Mathf.Deg2Rad;

            var velocitySquared = Physics.gravity.y * distance * distance /
                                  (2 * (heightDifference - distance * Mathf.Tan(angleRad)) *
                                   Mathf.Pow(Mathf.Cos(angleRad), 2));

            if (velocitySquared <= 0)
            {
                Debug.LogWarning("Cant throw. Not enough data");
                return Vector3.zero;
            }

            var initialVelocity = Mathf.Sqrt(velocitySquared);

            var velocityXZ = direction.normalized * initialVelocity * Mathf.Cos(angleRad);
            var velocityY = initialVelocity * Mathf.Sin(angleRad);

            var resultVelocity = velocityXZ;
            resultVelocity.y = velocityY;

            return resultVelocity;
        }
    }
}