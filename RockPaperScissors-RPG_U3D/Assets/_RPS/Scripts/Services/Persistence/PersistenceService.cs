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
    /// <summary>
    /// Servicio de persistencia. Serializa y deserializa GameContext como JSON en el directorio persistente de la aplicación.
    /// </summary>
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

        /// <summary>Guarda una nueva partida en disco; sobreescribe si ya existe el archivo.</summary>
        public void SaveGame(GameContext gameContext)
        {
            Debug.Log($"[PersistenceService] SaveGame() ->");
            string json = JsonConvert.SerializeObject(gameContext);
            File.WriteAllText(Path.Combine(_saveDirectory, gameContext.GameName), json);
        }
        
        /// <summary>Actualiza la partida en disco actualizando además el campo Date con la fecha actual.</summary>
        public void UpdateSaveGame(GameContext gameContext)
        {
	        Debug.Log($"[PersistenceService] UpdateSaveGame() ->");
	        string json = JsonConvert.SerializeObject(gameContext);
	        JObject jsonObj = JsonConvert.DeserializeObject<JObject>(json);
	        jsonObj["LastUpdateDate"] = RPSTimestamp.ConvertTimestampToDateTime(RPSTimestamp.GetTimestamp()).ToString(CultureInfo.InvariantCulture);
	        json = jsonObj.ToString();
	        File.WriteAllText(Path.Combine(_saveDirectory, gameContext.GameName), json);
        }
        
        /// <summary>Devuelve true si existe al menos una partida guardada, sin deserializar.</summary>
        public bool HasAnySave()
        {
            return Directory.Exists(_saveDirectory) && Directory.GetFiles(_saveDirectory, "Game_*").Length > 0;
        }

        /// <summary>Carga una partida por nombre de archivo e invoca el callback con el GameContext deserializado, o null si el archivo no existe o está corrupto.</summary>
        public void LoadGame(string filename, UnityAction<GameContext> OnFinishCallback)
        {
            Debug.Log($"[PersistenceService] LoadGame() -> filename {filename}");
            string filePath = Path.Combine(_saveDirectory, filename);
            if (!File.Exists(filePath))
            {
                Debug.LogError($"[PersistenceService] LoadGame() -> File not found: {filename}");
                OnFinishCallback?.Invoke(null);
                return;
            }
            string json = File.ReadAllText(filePath);
            GameContext gameContext = DeserializeContext(json, filename);
            OnFinishCallback?.Invoke(gameContext);
        }

        /// <summary>Carga todos los archivos de partida (prefijo "Game_") e invoca el callback con la lista completa. Los saves corruptos se omiten.</summary>
        public void LoadGameList(UnityAction<List<GameContext>> OnFinishCallback)
        {
            Debug.Log($"[PersistenceService] LoadGameList() -> ");
            List<string> allSaveFiles = new List<string>(Directory.GetFiles(_saveDirectory, "Game_*"));
            List<GameContext> allGameContexts = new List<GameContext>();
            foreach (string saveFile in allSaveFiles)
            {
                string json = File.ReadAllText(saveFile);
                GameContext gameContext = DeserializeContext(json, saveFile);
                if (gameContext != null)
                    allGameContexts.Add(gameContext);
            }
            OnFinishCallback?.Invoke(allGameContexts);
        }

        /// <summary>Pre-valida el JSON y deserializa a GameContext. Devuelve null si el contenido está vacío, le faltan campos clave o está corrupto.</summary>
        private GameContext DeserializeContext(string json, string filename)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                Debug.LogError($"[PersistenceService] DeserializeContext() -> Empty file: {filename}");
                return null;
            }
            try
            {
                JObject jo = JObject.Parse(json);
                if (jo["GameName"] == null || jo["Player"] == null || jo["TownContext"] == null)
                {
                    Debug.LogError($"[PersistenceService] DeserializeContext() -> Missing required fields: {filename}");
                    return null;
                }
                return JsonConvert.DeserializeObject<GameContext>(json);
            }
            catch (JsonException e)
            {
                Debug.LogError($"[PersistenceService] DeserializeContext() -> Corrupted save ({filename}): {e.Message}");
                return null;
            }
        }

        /// <summary>Elimina el archivo de partida indicado del disco.</summary>
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
        
        /// <summary>Devuelve el número de partidas guardadas (excluyendo tests) formateado en dos dígitos.</summary>
        public string GetGamesCount()
        {
            if (Directory.Exists(_saveDirectory))
            {
	            string[] files = Directory.GetFiles(_saveDirectory, "Game_*");
	            string[] gameFiles = Array.FindAll(files, file => !file.Contains("Test"));
	            return gameFiles.Length.ToString("D2");
            }
            return 0.ToString("D2");
        }
        
        #endregion
    }
}