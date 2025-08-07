using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameDemo : MonoBehaviour
{
    [SerializeField]
    private Text highscoreText;

    [SerializeField]
    private Text scoreText;

    private int score;

	void Start ()
    {
        SoundController.Instance.StopMusic();

        score = 0;

        UpdateScoreText();
        UpdateHighscoreText();
    }

    public void IncreaseScore()
    {
        score += 1;
        UpdateScoreText();

        SoundController.Instance.PlaySoundEffect("coin");

        if (HighscoreController.Instance.TrySetHighscore(score))
        {
            UpdateHighscoreText();
        }
    }

    public void ResetScore()
    {
        SoundController.Instance.PlaySoundEffect("gameover");

        score = 0;
        UpdateScoreText();
    }

    public void ResetHighscore()
    {
        ResetScore();

        HighscoreController.Instance.SetHighscore(0);
        UpdateHighscoreText();
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    private void UpdateHighscoreText()
    {
        highscoreText.text = "Highscore: " + HighscoreController.Instance.GetHighscore();
    }
}
