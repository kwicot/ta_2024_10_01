﻿using System;
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
            return Mathf.Atan2(dir.y, dir.x)*Mathf.Rad2Deg;
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
            string advertisingID = "";
            try
            {
                AndroidJavaClass up = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
                AndroidJavaObject currentActivity = up.GetStatic<AndroidJavaObject> ("currentActivity");
                AndroidJavaClass client = new AndroidJavaClass ("com.google.android.gms.ads.identifier.AdvertisingIdClient");
                AndroidJavaObject adInfo = client.CallStatic<AndroidJavaObject> ("getAdvertisingIdInfo", currentActivity);
    
                advertisingID = adInfo.Call<string> ("getId").ToString();  
            }
            catch (Exception e)
            {
                Debug.LogError($"Cant get advert id: {e.Message}");
            }
            return advertisingID;
        }
        public static void Shuffle<T> (T[] array)
        {
            Random rng = new Random();
            int n = array.Length;
            while (n > 1) 
            {
                int k = rng.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }
        public static T RoundToNearest<T>(T value, T nearest) where T : struct, IConvertible
        {
            double doubleValue = Convert.ToDouble(value);
            double doubleNearest = Convert.ToDouble(nearest);

            double result = Math.Round(doubleValue / doubleNearest) * doubleNearest;

            return (T)Convert.ChangeType(result, typeof(T));
        }
        
        public static string GenerateUniqueID()
        {
            Guid id = Guid.NewGuid();
            return id.ToString();
        }

        public static string SaveScriptableObjectItem(ScriptableObject obj, string folderName, string fileName)
        {
            string folderPath = Path.Combine(Constants.ResourcesFolderPath, folderName);
            string path = Path.Combine(folderPath, $"{fileName}.asset");
                
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder(folderPath, Constants.ItemsFolderName);
            }

            AssetDatabase.CreateAsset(obj, path);
            AssetDatabase.SaveAssets();

            return path;
        }
        
        
        public static Vector3 CalculateLaunchVelocity(Vector3 start, Vector3 target, float angle)
        {
            Vector3 direction = target - start;
            direction.y = 0; 
            float distance = direction.magnitude;

            float heightDifference = target.y - start.y;

            float angleRad = angle * Mathf.Deg2Rad;

            float velocitySquared = (Physics.gravity.y * distance * distance) / 
                                    (2 * (heightDifference - distance * Mathf.Tan(angleRad)) * Mathf.Pow(Mathf.Cos(angleRad), 2));

            if (velocitySquared <= 0)
            {
                Debug.LogWarning("Cant throw. Not enough data");
                return Vector3.zero;
            }

            float initialVelocity = Mathf.Sqrt(velocitySquared);

            Vector3 velocityXZ = direction.normalized * initialVelocity * Mathf.Cos(angleRad);
            float velocityY = initialVelocity * Mathf.Sin(angleRad);

            Vector3 resultVelocity = velocityXZ;
            resultVelocity.y = velocityY;

            return resultVelocity;
        }
    }
}