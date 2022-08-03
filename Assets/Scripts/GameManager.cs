using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public string playerName;
    public string currentBestPlayerName;
    public int currentBestScore;
    public PlayerData[] playerList;
    public float paddleSpeedSetting = 1;
    public float ballVelocitySetting = 1;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        LoadPlayerData();

        //this needs to be entered by the player each session
        playerName = null;

        //will only ever be called once in order to give empty data for the programs to work off of
        if (playerList == null)
        {
            InitAndPopulateArray();
        }
    }

    //object to store the direct player info
    [System.Serializable]
    public class PlayerData
    {
        public string name;
        public int score;

        public PlayerData(string name, int score)
        {
            this.name = name;
            this.score = score;
        }
    }

    //object to save the persistent data between sessions for the json file
    [System.Serializable]
    class SaveData
    {
        public string playerName;
        public string currentBestPlayerName;
        public int currentBestScore;
        public PlayerData[] playerList;
        public float paddleSpeedSetting;
        public float ballVelocitySetting;
    }

    //will only ever be called once, the first time the game is loaded, to initialize the array and give it temp data to be overwritten
    private void InitAndPopulateArray()
    {
        playerList = new PlayerData[5];

        for (int i = 0; i < playerList.Length; i++)
        {
            playerList[i] = new PlayerData("temp", 0);
        }
    }

    public void AddPlayerToArray(PlayerData player)
    {
        if (playerList[0].name == null)
        {
            playerList[0] = player;
        }
        else if (player.score > playerList[4].score)
        {
            playerList[4] = player;
            for (int i = 3; i > -1; i--)
            {
                if (player.score > playerList[i].score)
                {
                    PlayerData temp = playerList[i];
                    playerList[i] = player;
                    playerList[i + 1] = temp;
                }
                else if (player.score < playerList[i].score || player.score == playerList[i].score)
                {
                    break;
                }
            }
        }
    }

    public void SavePlayerData()
    {
        SaveData data = new SaveData();

        data.playerName = playerName;
        data.currentBestPlayerName = currentBestPlayerName;
        data.currentBestScore = currentBestScore;
        data.playerList = playerList;
        data.paddleSpeedSetting = paddleSpeedSetting;
        data.ballVelocitySetting = ballVelocitySetting;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadPlayerData()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            SaveData data = JsonUtility.FromJson<SaveData>(json);

            playerName = data.playerName;
            currentBestPlayerName = data.currentBestPlayerName;
            currentBestScore = data.currentBestScore;
            playerList = data.playerList;
            paddleSpeedSetting = data.paddleSpeedSetting;
            ballVelocitySetting = data.ballVelocitySetting;
        }
    }
}
