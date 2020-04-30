using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    private UIManager m_uiManager;

    private FourWheelDrive m_player;

    private void Awake()
    {
        if (!instance)
            instance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level > 0)
        {
            m_player = FindObjectOfType<FourWheelDrive>();
        }
    }

    public void GameOver(string gameOverMessage)
    {
        // disable player movement
        m_player.SetCanDrive(false);
        // show game over UI
        m_uiManager.GameOver(gameOverMessage);
    }

    public void RestartLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void PlayerEnteredFinishLine()
    {
        // disable player movement
        m_player.SetCanDrive(false);
        // show level complete UI
        m_uiManager.LevelComplete();
    }

    public void NextLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
}