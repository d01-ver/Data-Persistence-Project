using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuManager : MonoBehaviour
{
    private static int highscore;
    private static string bestPlayer;
    public static int currentHighscore;
    private static string currentPlayer;
    private static bool newHighScore=false;

    public TMP_InputField playerName;
    public TextMeshProUGUI highscoreText;

    private const string DEFAULT_PLAYER_NAME = "Player1";

    private void Start()
    {
        if (newHighScore)
        {
            playerName.text = currentPlayer;
            highscoreText.text = highscore + " (" + bestPlayer + ")";
            SaveHighscore();
            newHighScore = false;
        }
        else
        {
            HighScore hs = LoadHighscore();
            if (hs != null)
            {
                highscore = hs.highscore;
                bestPlayer = hs.bestPlayer;
                currentPlayer = hs.lastPlayer;
                playerName.text = currentPlayer;
                highscoreText.text = highscore + " (" + bestPlayer + ")";
            }
            else
            {
                highscore = 0;
                bestPlayer = "";
                currentHighscore = 0;
                currentPlayer = DEFAULT_PLAYER_NAME;
                playerName.text = currentPlayer;
                highscoreText.text = "0";
            }
        }
    }

    private class HighScore
    {
        public string bestPlayer;
        public int highscore;
        public string lastPlayer;
        public HighScore(string bestPlayer, int highscore, string lastPlayer) { this.bestPlayer = bestPlayer; this.highscore = highscore; this.lastPlayer = lastPlayer; }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
                EditorApplication.ExitPlaymode();
        #else
                Application.Quit();        
        #endif
    }

    private void SaveHighscore()
    {
        HighScore hs = new HighScore(bestPlayer, highscore, currentPlayer);
        string json = JsonUtility.ToJson(hs);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    HighScore LoadHighscore()
    {
        if (File.Exists(Application.persistentDataPath + "/savefile.json"))
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/savefile.json");
            HighScore hs = JsonUtility.FromJson<HighScore>(json);
            return hs;
        }
        return null;
    }

    public static void UpdateHighscore(int score)
    {
        currentHighscore = score;
        if (score > highscore)
        {
            highscore = score;
            bestPlayer = currentPlayer;
            newHighScore = true;
        }
    }

    public void PlayerNameChanged()
    {
        if (playerName.text != currentPlayer)
        {
            if (playerName.text == "")
            {
                playerName.text = currentPlayer;
            }
            else
            {
                currentPlayer = playerName.text;
            }
            SaveHighscore();
        }
    }
}
