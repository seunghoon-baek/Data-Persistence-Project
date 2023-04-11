using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] Button quitButton;
    [SerializeField] TextMeshProUGUI bestScoreText;
    [SerializeField] TMP_InputField playerNameField;

    public static string playerName;

    // Start is called before the first frame update
    void Awake()
    {
        GetBestScore();

        startButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    void StartGame()
    {
        playerName = playerNameField.text;
        SceneManager.LoadScene(1);  
    }

    void QuitGame()
    {
        MainManager.SaveScore();

#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
    }

    void GetBestScore()
    {
        MainManager.SaveData savedBestScore = MainManager.LoadScore();

        if (savedBestScore.bestPoint < MainManager.bestPoint)
        {
            bestScoreText.text = "Best Score: " + MainManager.bestPoint + " Name: " + MainManager.bestPlayerName;
        } else
        {
            MainManager.bestPoint = savedBestScore.bestPoint;
            MainManager.bestPlayerName = savedBestScore.bestPlayerName;

            bestScoreText.text = "Best Score: " + savedBestScore.bestPoint + " Name: " + savedBestScore.bestPlayerName;
        }
        
    }
}