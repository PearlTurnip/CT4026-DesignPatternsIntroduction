using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreController
{
    private static readonly HighscoreController _instance = new HighscoreController();

    public static HighscoreController Instance
    {
        get
        {
            return _instance;
        }
    }

    private string HighscorePrefsKey
    {
        get
        {
            return gameName + "_highscore";
        }
    }

    private string gameName = "default";

    /// <summary>
    /// Sets the gamename (enables saving score's of different mini-games in one game)
    /// You don't have to set this to save one game's score
    /// </summary>
    /// <param name="gameName"></param>
    public void SetGameName(string gameName)
    {
        this.gameName = gameName;
    }

    /// <summary>
    /// Will set the highscore new value is better
    /// </summary>
    /// <param name="score"></param>
    /// <returns>If the highscore was beaten</returns>
    public bool TrySetHighscore(int score)
    {
        if (score > GetHighscore())
        {
            PlayerPrefs.SetInt(HighscorePrefsKey, score);
            PlayerPrefs.Save();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Will set the highscore regardless of previous value
    /// </summary>
    /// <param name="score"></param>
    public void SetHighscore(int score)
    {
        PlayerPrefs.SetInt(HighscorePrefsKey, score);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Will get the highscore from disk, defaults to 0
    /// </summary>
    /// <returns>The Highscore</returns>
    public int GetHighscore()
    {
        return PlayerPrefs.GetInt(HighscorePrefsKey, 0);
    }
}