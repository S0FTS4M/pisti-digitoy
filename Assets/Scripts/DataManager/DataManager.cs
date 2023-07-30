using System;
using System.IO;
using System.Text;
using UnityEngine;

public class DataManager
{
    private readonly string DirectoryPath = Path.Combine(
            Application.persistentDataPath,
            "GameData"
       );

    private string FilePath => Path.Combine(DirectoryPath, FileName + "." + FileExtension);

    private readonly string FileExtension = "json";

    private string fileName = "game_data";

    public string FileName
    {
        get
        {
            if (string.IsNullOrEmpty(fileName))
            {
                Debug.LogError("You forgot to assign file name");
            }

            return fileName;
        }
        set
        {
            fileName = value;
        }
    }

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
        if (Directory.Exists(DirectoryPath) == false ||
               File.Exists(FilePath) == false)
        {
            playerData = new PlayerData();
            return;
        }

        // Get serialized json from storage
        string serializedJson = "";
        using (StreamReader streamReader = File.OpenText(FilePath))
        {
            serializedJson = streamReader.ReadToEnd();
        }


        string previousHash = PlayerPrefs.GetString(PLAYER_DATA_KEY);


        string currHash = ComputeSHA1Hash(serializedJson);


        if (previousHash != currHash)
        {
            playerData = new PlayerData();
           return;
        }

        if (!string.IsNullOrEmpty(serializedJson))
        {
            playerData = JsonUtility.FromJson<PlayerData>(serializedJson);

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


        if (Directory.Exists(DirectoryPath) == false)
        {
            Directory.CreateDirectory(DirectoryPath);

        }

        // Save hash of the serialized json to PlayerPrefs
        string hashOfTheSerializedJson = ComputeSHA1Hash(serializedPlayerData);


        PlayerPrefs.SetString(PLAYER_DATA_KEY, hashOfTheSerializedJson);



        using (var streamWriter = File.CreateText(FilePath))
        {
            streamWriter.Write(serializedPlayerData);
        }


    }

    public static string ComputeSHA1Hash(string text)
    {
        byte[] textAsBytes = Encoding.UTF8.GetBytes(text);
        byte[] hashAsBytes = new System.Security.Cryptography.SHA1Managed().ComputeHash(textAsBytes, 0, Encoding.UTF8.GetByteCount(text));
        string hashAsHex = "";

        foreach (byte x in hashAsBytes)
        {
            hashAsHex += string.Format("{0:x2}", x);
        }

        return hashAsHex;
    }

}
