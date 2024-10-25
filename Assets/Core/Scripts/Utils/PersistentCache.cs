using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

namespace Kwicot.Core.Scripts.Utils
{
    public static class PersistentCache
    {
        private static readonly bool _initialized = false;
        private static bool _isLog;

        private static void Initialize()
        {
        }

        public static void Save(object data)
        {
            Save(data, data.GetType().ToString());
        }

        public static void Save(object data, string key)
        {
            if (!_initialized)
                Initialize();

            if (data == null)
                return;

            var path = GetPath(key);

            if (File.Exists(path))
                File.Delete(path);

            var bf = new BinaryFormatter();
            var fs = new FileStream(path, FileMode.OpenOrCreate);

            try
            {
                bf.Serialize(fs, data);

                fs.Close();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                fs.Close();
            }
        }

        public static T TryLoad<T>(string key, T defaultValue = default)
        {
            if (!_initialized)
                Initialize();

            var path = GetPath(key);
            if (File.Exists(path))
            {
                var bf = new BinaryFormatter();
                var fs = new FileStream(path, FileMode.Open);
                try
                {
                    var data = bf.Deserialize(fs);
                    fs.Close();
                    return (T)data;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    fs.Close();
                    return defaultValue;
                }
            }

            Debug.LogError($"Try to load {path} but doesnt exists");
            return defaultValue;
        }

        public static T TryLoad<T>()
        {
            return TryLoad<T>(typeof(T).ToString());
        }

        public static T TryLoad<T>(T defaultValue)
        {
            return TryLoad(typeof(T).ToString(), defaultValue);
        }


        public static void TryDelete(string key)
        {
            if (!_initialized)
                Initialize();

            var path = GetPath(key);
            if (File.Exists(path))
                File.Delete(path);
        }

        public static void TryDelete(params string[] keys)
        {
            if (!_initialized)
                Initialize();

            foreach (var key in keys) TryDelete(key);
        }


#if SFB
        public static void ExportJson<T>(T data)
        {
            string json = JsonConvert.SerializeObject(data);
            string path = StandaloneFileBrowser.SaveFilePanel("", "", "json", "");
            
            var bytes = Encoding.UTF8.GetBytes(json);
            File.WriteAllBytes(path, bytes);
        }

        public static T ImportJson<T>()
        {
            string[] paths = StandaloneFileBrowser.OpenFilePanel("", "", "json", false);
            var path = paths[0];
            var bytes = File.ReadAllBytes(path);
            var json = Encoding.UTF8.GetString(bytes);
            return JsonConvert.DeserializeObject<T>(json);
        }
#endif


        private static string GetPath(string key)
        {
            if (!_initialized)
                Initialize();

            var path = Application.persistentDataPath;
            return Path.Combine(path, key);
        }

        private static void Log(object data)
        {
            if (_isLog)
                Debug.Log(data);
        }

        private static void LogWarning(object data)
        {
            if (_isLog)
                Debug.LogWarning(data);
        }

        private static void LogError(object data)
        {
            if (_isLog)
                Debug.LogError(data);
        }

#if UNITY_EDITOR
        [MenuItem("Tools/PersistentCache/Clear persistent cache")]
#endif

        public static void ClearPersistentCache()
        {
            if (!_initialized)
                Initialize();

            var files = Directory.GetFiles(Application.persistentDataPath);
            foreach (var file in files) File.Delete(file);
        }
#if UNITY_EDITOR
        [MenuItem("Tools/PersistentCache/Clear player prefs cache")]
#endif
        public static void ClearPrefsCache()
        {
            PlayerPrefs.DeleteAll();
        }

#if UNITY_EDITOR
        [MenuItem("Tools/PersistentCache/Clear all cache")]
#endif

        public static void CelarAllCache()
        {
            ClearPersistentCache();
            ClearPrefsCache();
        }
    }
}