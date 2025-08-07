using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuDemo : MonoBehaviour
{
    [SerializeField]
    private Text highscoreText;

	void Start ()
    {
        SoundController.Instance.PlayMusic("frozen");
        highscoreText.text = "Highscore: " + HighscoreController.Instance.GetHighscore();
    }
	
	public void LoadGame ()
    {
        SceneManager.LoadScene("Game");
	}
}
