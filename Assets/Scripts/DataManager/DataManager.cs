using System;
using UnityEngine;

public class DataManager
{
    private const string PLAYER_DATA_KEY = "PlayerData";

    private PlayerData playerData;

    private int totalLevelCount;

    public PlayerData PlayerData => playerData;

    public DataManager()
    {
        LoadPlayerData();
    }

    private void LoadPlayerData()
    {
        string serializedPlayerData = PlayerPrefs.GetString(PLAYER_DATA_KEY, string.Empty);

        if (!string.IsNullOrEmpty(serializedPlayerData))
        {
            playerData = JsonUtility.FromJson<PlayerData>(serializedPlayerData);

            playerData.SetLoaded(true);
        }
        else
        {
            playerData = new PlayerData();
            SavePlayerData();
        }
    }

    public void SavePlayerData()
    {
        string serializedPlayerData = JsonUtility.ToJson(playerData);
        PlayerPrefs.SetString(PLAYER_DATA_KEY, serializedPlayerData);
    }
    // Rest of the code remains unchanged.
}
