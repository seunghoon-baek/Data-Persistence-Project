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
        bestScoreText.text = "Best Score: " + MainManager.bestScore + " Name: " + MainManager.bestPlayerName;
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
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
    }
}