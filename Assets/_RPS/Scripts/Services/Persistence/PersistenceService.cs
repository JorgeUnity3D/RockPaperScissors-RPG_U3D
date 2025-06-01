using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Kapibara.RPS;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
namespace Kapibara.RPS
{
    public class PersistenceService : ServiceSubscriber<PersistenceService>
    {
        [SerializeField] string _subfolder;
        [SerializeField] [ReadOnly] string _saveDirectory;

        #region UNITY_LIFECYCLE

        protected override void Awake()
        {
            base.Awake();
            _saveDirectory = Path.Combine(Application.persistentDataPath, _subfolder);
            if (!Directory.Exists(_saveDirectory))
            {
                Directory.CreateDirectory(_saveDirectory);
            }
        }

        #endregion
        
        #region CONTROL

        public void SaveGame(GameContext gameContext)
        {
            Debug.Log($"[PersistenceService] SaveGame() ->");
            string json = JsonConvert.SerializeObject(gameContext);
            File.WriteAllText(Path.Combine(_saveDirectory, gameContext.GameName), json);
        }
        
        public void UpdateSaveGame(GameContext gameContext)
        {
	        Debug.Log($"[PersistenceService] SaveGame() ->");
	        string json = JsonConvert.SerializeObject(gameContext);
	        JObject jsonObj = JsonConvert.DeserializeObject<JObject>(json);
	        jsonObj["Date"] = RPSTimestamp.ConvertTimestampToDateTime(RPSTimestamp.GetTimestamp()).ToString(CultureInfo.InvariantCulture);
	        json = jsonObj.ToString();
	        File.WriteAllText(Path.Combine(_saveDirectory, gameContext.GameName), json);
        }
        
        public void LoadGame(string filename, UnityAction<GameContext> OnFinishCallback)
        {
            Debug.Log($"[PersistenceService] LoadGame() -> filename {filename}");
            string filePath = Path.Combine(_saveDirectory, filename);
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                GameContext gameContext = JsonConvert.DeserializeObject<GameContext>(json);
                OnFinishCallback?.Invoke(gameContext);
            }
        }

        public void LoadGameList(UnityAction<List<GameContext>> OnFinishCallback)
        {
            Debug.Log($"[PersistenceService] LoadGameList() -> ");
            List<string> allSaveFiles = new List<string>(Directory.GetFiles(_saveDirectory));
            List<GameContext> allGameContexts = new List<GameContext>();

            foreach (string saveFile in allSaveFiles)
            {
                string json = File.ReadAllText(saveFile);
                GameContext gameContext = JsonConvert.DeserializeObject<GameContext>(json);
                allGameContexts.Add(gameContext);
            }
            OnFinishCallback?.Invoke(allGameContexts);
        }

        public void DeleteGame(string filename)
        {
            Debug.Log($"[PersistenceService] DeleteGame() -> filename {filename}");
            string filePath = Path.Combine(_saveDirectory, filename);
    
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            else
            {
                Debug.Log($"[PersistenceService] DeleteGame() -> File {filename} not found in {_saveDirectory}");
            }
        }
        
        public string GetGamesCount()
        {
            if (Directory.Exists(_saveDirectory))
            {
	            string[] files = Directory.GetFiles(_saveDirectory);
	            string[] gameFiles = Array.FindAll(files, file => !file.Contains("Test"));
	            return gameFiles.Length.ToString("D2");
            }
            return 0.ToString("D2");
        }
        
        #endregion
    }
}