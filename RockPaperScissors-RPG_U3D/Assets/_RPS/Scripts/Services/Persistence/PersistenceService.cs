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
	        Debug.Log($"[PersistenceService] SaveGame() ->");
	        string json = JsonConvert.SerializeObject(gameContext);
	        JObject jsonObj = JsonConvert.DeserializeObject<JObject>(json);
	        jsonObj["Date"] = RPSTimestamp.ConvertTimestampToDateTime(RPSTimestamp.GetTimestamp()).ToString(CultureInfo.InvariantCulture);
	        json = jsonObj.ToString();
	        File.WriteAllText(Path.Combine(_saveDirectory, gameContext.GameName), json);
        }
        
        /// <summary>Carga una partida por nombre de archivo e invoca el callback con el GameContext deserializado.</summary>
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

        /// <summary>Carga todos los archivos de partida (prefijo "Game_") e invoca el callback con la lista completa.</summary>
        public void LoadGameList(UnityAction<List<GameContext>> OnFinishCallback)
        {
            Debug.Log($"[PersistenceService] LoadGameList() -> ");
            List<string> allSaveFiles = new List<string>(Directory.GetFiles(_saveDirectory, "Game_*"));
            List<GameContext> allGameContexts = new List<GameContext>();

            foreach (string saveFile in allSaveFiles)
            {
                string json = File.ReadAllText(saveFile);
                GameContext gameContext = JsonConvert.DeserializeObject<GameContext>(json);
                allGameContexts.Add(gameContext);
            }
            OnFinishCallback?.Invoke(allGameContexts);
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