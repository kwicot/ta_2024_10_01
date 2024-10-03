using System;
using System.Net;
using UnityEngine;
using Random = System.Random;

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
    }
}