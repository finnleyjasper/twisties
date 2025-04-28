using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    private int score;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    

    private void Start()
    {
        score = PlayerPrefs.GetInt("score", 0);
        scoreText.text = score.ToString() + " POINTS";
    }

    public void RestartButton()
    {
        SceneManager.LoadSceneAsync("Main");
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("Start");
    }
}
