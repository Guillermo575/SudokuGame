using System;
using System.Collections.Generic;
using UnityEngine;
public class SavePlayerPref : MonoBehaviour
{
    #region Singleton
    private static SavePlayerPref SingletonSavePlayerPref;
    private SavePlayerPref() { }
    private void Awake()
    {
        CreateSingleton();
    }
    private void CreateSingleton()
    {
        if (SingletonSavePlayerPref == null)
        {
            SingletonSavePlayerPref = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogError("Ya existe una instancia de SavePlayerPref");
            Destroy(gameObject);
        }
    }
    public static SavePlayerPref GetSingleton()
    {
        return SingletonSavePlayerPref;
    }
    #endregion

    #region Constants
    private const string LAST_GAME_KEY = "LastGameState";
    private const string GAMES_LIST_KEY = "GamesList";
    private const string GAMES_COUNT_KEY = "GamesCount";
    #endregion

    #region Get Methods
    public GameState GetLastGameState()
    {
        if (!PlayerPrefs.HasKey(LAST_GAME_KEY)) return null;
        string json = PlayerPrefs.GetString(LAST_GAME_KEY);
        try
        {
            return JsonUtility.FromJson<GameState>(json);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error deserializando lastGameState: {e.Message}");
            return null;
        }
    }
    public List<GameState> GetAllGames()
    {
        List<GameState> lstGames = new List<GameState>();
        if (!PlayerPrefs.HasKey(GAMES_COUNT_KEY)) return lstGames;
        int count = PlayerPrefs.GetInt(GAMES_COUNT_KEY, 0);
        for (int i = 0; i < count; i++)
        {
            string key = $"{GAMES_LIST_KEY}_{i}";
            if (PlayerPrefs.HasKey(key))
            {
                string json = PlayerPrefs.GetString(key);
                try
                {
                    GameState gameState = JsonUtility.FromJson<GameState>(json);
                    if (gameState != null)
                        lstGames.Add(gameState);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error deserializando juego {i}: {e.Message}");
                }
            }
        }
        return lstGames;
    }
    public GameState GetGameById(string id)
    {
        List<GameState> games = GetAllGames();
        return games.Find(g => g.Id == id);
    }
    #endregion

    #region Set Methods
    public void SetLastGameState(GameState gameState)
    {
        if (gameState == null)
        {
            PlayerPrefs.DeleteKey(LAST_GAME_KEY);
            PlayerPrefs.Save();
            return;
        }
        try
        {
            string json = JsonUtility.ToJson(gameState);
            PlayerPrefs.SetString(LAST_GAME_KEY, json);
            PlayerPrefs.Save();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error serializando lastGameState: {e.Message}");
        }
    }
    public void SaveGame(GameState gameState)
    {
        if (gameState == null) return;
        List<GameState> lstGames = GetAllGames();
        int index = lstGames.FindIndex(p => p.Id == gameState.Id);
        if (index >= 0)
        {
            lstGames[index] = gameState;
        }
        else
        {
            lstGames.Add(gameState);
        }
        SaveAllGames(lstGames);
        SetLastGameState(gameState);
    }
    public void DeleteGame(string id)
    {
        List<GameState> lstGames = GetAllGames();
        lstGames.RemoveAll(g => g.Id == id);
        SaveAllGames(lstGames);
        GameState lastGame = GetLastGameState();
        if (lastGame != null && lastGame.Id == id)
        {
            SetLastGameState(null);
        }
    }
    private void SaveAllGames(List<GameState> lstGames)
    {
        try
        {
            for (int i = 0; i < lstGames.Count; i++)
            {
                string key = $"{GAMES_LIST_KEY}_{i}";
                string json = JsonUtility.ToJson(lstGames[i]);
                PlayerPrefs.SetString(key, json);
            }
            int oldCount = PlayerPrefs.GetInt(GAMES_COUNT_KEY, 0);
            for (int i = lstGames.Count; i < oldCount; i++)
            {
                string key = $"{GAMES_LIST_KEY}_{i}";
                PlayerPrefs.DeleteKey(key);
            }

            PlayerPrefs.SetInt(GAMES_COUNT_KEY, lstGames.Count);
            PlayerPrefs.Save();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error guardando lista de juegos: {e.Message}");
        }
    }
    #endregion

    #region Clear
    public void ClearAllData()
    {
        PlayerPrefs.DeleteKey(LAST_GAME_KEY);
        int count = PlayerPrefs.GetInt(GAMES_COUNT_KEY, 0);
        for (int i = 0; i < count; i++)
        {
            string key = $"{GAMES_LIST_KEY}_{i}";
            PlayerPrefs.DeleteKey(key);
        }

        PlayerPrefs.DeleteKey(GAMES_COUNT_KEY);
        PlayerPrefs.Save();
    }
    #endregion
}