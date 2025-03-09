using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroUI : MonoBehaviour
{
    public GameObject introPanel;
    public Button startButton;

    [Header("Difficulty Selection")]
    public Toggle easyToggle;
    public Toggle mediumToggle;
    public Toggle hardToggle;

    void Start()
    {
        Time.timeScale = 0;
        introPanel.SetActive(true);

        string difficulty = PlayerPrefs.GetString("SelectedDifficulty", "Medium");

        easyToggle.isOn = (difficulty == "Easy");
        mediumToggle.isOn = (difficulty == "Medium");
        hardToggle.isOn = (difficulty == "Hard");

        startButton.onClick.AddListener(StartGame);

        easyToggle.onValueChanged.AddListener(delegate { UpdateDifficulty("Easy"); });
        mediumToggle.onValueChanged.AddListener(delegate { UpdateDifficulty("Medium"); });
        hardToggle.onValueChanged.AddListener(delegate { UpdateDifficulty("Hard"); });
    }

    void StartGame()
    {
        if (!easyToggle.isOn && !mediumToggle.isOn && !hardToggle.isOn)
        {
            Debug.LogWarning("Please select a difficulty before starting the game.");
            return;
        }

        Time.timeScale = 1;

        introPanel.SetActive(false);
    }

    void UpdateDifficulty(string difficulty)
    {
        PlayerPrefs.SetString("SelectedDifficulty", difficulty);
        PlayerPrefs.Save();

Object.FindFirstObjectByType<TurretController>()?.ApplyDifficultySettings(difficulty);
    }
}
