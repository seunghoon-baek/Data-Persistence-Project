using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Diagnostics;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text BestScoreText;
    public GameObject GameOverText;
    public Button MenuButton;
    public static string bestPlayerName;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    public static int bestPoint;
    // public static MainManager Instance;
    // public int bestScoreForInstance;

    
    // Start is called before the first frame update
    void Start()
    {
        // ==================================================
        // Instance = this;
        DontDestroyOnLoad(gameObject);


        BestScoreText.text = "Best Score: " + bestPoint + " Name: " + bestPlayerName;

        MenuButton.onClick.AddListener(BackToMenu);
        // ==================================================

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = UnityEngine.Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (bestPoint < m_Points)
            {
                bestPoint = m_Points;
                bestPlayerName = MenuManager.playerName;
            }

            // Instance.b

            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

                if (gameObject != null)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);

        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }

    [Serializable]
    public class SaveData
    {
        public string bestPlayerName = MainManager.bestPlayerName;
        public int bestPoint = MainManager.bestPoint;
    }

    public static void SaveScore()
    {
        string path = Application.persistentDataPath + "/saveFile.json";
        string json = JsonUtility.ToJson(new SaveData());

        File.WriteAllText(path, json);
    }

    public static SaveData LoadScore()
    {
        try
        {
            string path = Application.persistentDataPath + "/saveFile.json";
            string json = File.ReadAllText(path);

            SaveData data = JsonUtility.FromJson<SaveData>(json);

            return data;
        } catch (FileNotFoundException)
        {
            SaveData result = new SaveData();
            result.bestPlayerName = "";
            result.bestPoint = 0;

            return result;
        }
          
    }
}
