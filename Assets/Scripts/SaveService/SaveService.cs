using System;
using UnityEngine;
using Newtonsoft.Json;

namespace Assets.Scripts.SaveSystem
{
    public class SaveService
    {
        #region Variables

        private const string DebugPrefix = "SaveService";

        #endregion Variables

        #region Functions

        public void Save<T>(string key, T value)
        {
            if (String.IsNullOrEmpty(key))
            {
                SendLog($"Save error: Key is null or empty.");
                return;
            }

            try
            {
                string jsonToSave = JsonConvert.SerializeObject(value);
                PlayerPrefs.SetString(key, jsonToSave);
            }
            catch (Exception e)
            {
                SendLog($"Save error: Serialization error. {e}");
            }
        }

        public T Load<T>(string key, T defaultValue = default(T))
        {
            if (String.IsNullOrEmpty(key))
            {
                SendLog($"Load error: Key is null or empty, returning default value.");
                return defaultValue;
            }

            try
            {
                string savedJson = PlayerPrefs.GetString(key, String.Empty);

                if (String.IsNullOrEmpty(savedJson))
                {
                    Save<T>(key, defaultValue);
                    SendLog($"Load error: Saved json value is null or empty, laoding default value.");
                    return defaultValue;
                }

                T savedObject = JsonConvert.DeserializeObject<T>(savedJson);
                return savedObject;
            }
            catch (Exception e)
            {
                SendLog($"Get error: Deserialization error, returning default value. {e}");
                return defaultValue;
            }
        }

        public bool HasSave<T>(string key)
        {
            return !String.IsNullOrEmpty(key) && PlayerPrefs.HasKey(key);
        }

        public void Delete<T>(string key)
        {
            if (String.IsNullOrEmpty(key)) return;

            if (!PlayerPrefs.HasKey(key))
            {
                SendLog($"Key does not exist: {key}");
                return;
            }

            PlayerPrefs.DeleteKey(key);
            SendLog($"Key deleted successfully: {key}");
        }

        public void DeleteSave()
        {
            bool isDeleteSuccessful = true;

            try
            {
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
            }
            catch (Exception e)
            {
                isDeleteSuccessful = false;
                SendLog($"Deleting error: {e.Message}");
            }
            finally
            {
                if (isDeleteSuccessful)
                {
                    SendLog($"Save deleted successfully.");
                }
            }
        }

        private void SendLog(string message)
        {
            Debug.Log($"{DebugPrefix}:{message}");
        }

        #endregion Functions
    }
}