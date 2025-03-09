using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    
    public static UIManager Instance { get; private set; }

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bombText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;

    private int score = 0;
    public int bombCount = 0;
    private int highScore = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    void Start()
    {
        UpdateUI();
        gameOverPanel.SetActive(false);
    }

    public void AddScore(int points)
    {
        score += points;

        if (score / 200 > bombCount)
        {
            bombCount++;
        }

        UpdateUI();
    }

 public void UseBomb()
{
    if (bombCount > 0)
    {
        bombCount--;
        UpdateUI();
        Debug.Log("Bomb Used! Destroying all paratroopers.");

Paratrooper[] paratroopers = Object.FindObjectsByType<Paratrooper>(FindObjectsSortMode.None);

        foreach (Paratrooper paratrooper in paratroopers)
        {
            paratrooper.DestroyParatrooper();

        }
    }
}



    public void GameOver()
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        gameOverPanel.SetActive(true);
        gameOverText.text = $"Game Over\nScore: {score}\nHigh Score: {highScore}";

        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void UpdateUI()
    {
        scoreText.text = "Score: " + score;
        bombText.text = "Bombs: " + bombCount;
    }
} // ✅ Make sure this closing bracket exists!
