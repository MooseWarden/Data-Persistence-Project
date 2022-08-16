using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

//execute this after game manager to make sure its all loaded up first before setting up the ui
[DefaultExecutionOrder(1000)]

public class MenuUIHandler : MonoBehaviour
{
    public TMP_InputField playerNameInput;
    public Text currentBestInfo;
    public Slider paddleSlider;
    public Slider ballSlider;

    // Start is called before the first frame update
    void Start()
    {
        //only run this in the score table scene, maybe change to use string names for scenes
        if (SceneManager.GetActiveScene().name == "score table")// .buildIndex == 2)
        {
            currentBestInfo.text = "Score List \nBest Score : " + GameManager.instance.currentBestPlayerName + " : " + GameManager.instance.currentBestScore + "\n";

            for (int i = 0; i < 5; i++)
            {
                if (GameManager.instance.playerList.Length == 0)// == null) //error check
                {
                    currentBestInfo.text += "\n none : yet";
                }
                else
                {
                    currentBestInfo.text += "\n" + GameManager.instance.playerList[i].name + " : " + GameManager.instance.playerList[i].score;
                }
            }
        }

        if (SceneManager.GetActiveScene().name == "settings")
        {
            paddleSlider.value = GameManager.instance.paddleSpeedSetting;
            ballSlider.value = GameManager.instance.ballVelocitySetting;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    //to be used only for the settings and score table scenes
    public void Back()
    {
        if (SceneManager.GetActiveScene().name == "settings")
        {
            GameManager.instance.paddleSpeedSetting = paddleSlider.value;
            GameManager.instance.ballVelocitySetting = ballSlider.value;
            GameManager.instance.SavePlayerData();
        }

        SceneManager.LoadScene("start menu");
    }

    public void GoToScoreTable()
    {
        SceneManager.LoadScene("score table");
    }

    public void GoToSettings()
    {
        SceneManager.LoadScene("settings");
    }

    public void StartGame()
    {
        //since the input field is "" by default and the player can enter empty spaces, this needs to be checked to make sure there is an actual name or not in the field
        if (playerNameInput.text.Trim().Length == 0)
        {
            GameManager.instance.playerName = null;
        }
        else
        {
            GameManager.instance.playerName = playerNameInput.text;
            GameManager.instance.SavePlayerData();
        }

        SceneManager.LoadScene("main");
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
