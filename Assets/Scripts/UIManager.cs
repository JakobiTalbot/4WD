using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField]
    private GameObject m_gameCanvas;

    [Header("Fuel")]
    [SerializeField]
    private Slider m_fuelGauge;
    [SerializeField]
    private Image m_fuelGaugeFill;
    [SerializeField]
    private Color m_maxFuelColour = Color.green;
    [SerializeField]
    private Color m_minFuelColour = Color.red;

    [Header("Game Over")]
    [SerializeField]
    private GameObject m_gameOverUIParent;
    [SerializeField]
    private TextMeshProUGUI m_gameOverMessage;

    [Header("Level Complete")]
    [SerializeField]
    private GameObject m_levelCompleteUIParent;

    private void Awake()
    {
        if (!instance)
            instance = this;
        DontDestroyOnLoad(m_gameCanvas);
    }

    public void SetFuelValue(float value, float maxValue)
    {
        float percent = value / maxValue;
        m_fuelGauge.value = percent;
        m_fuelGaugeFill.color = Color.Lerp(m_minFuelColour, m_maxFuelColour, percent) * 2f;
    }

    public void GameOver(string gameOverMessage)
    {
        m_gameOverMessage.text = gameOverMessage;
        m_gameOverUIParent.SetActive(true);
    }

    public void LevelComplete()
    {
        m_levelCompleteUIParent.SetActive(true);
    }

    public void EnableGameCanvas()
    {
        m_gameCanvas.SetActive(true);
    }

    // disables game over UI parent and level complete UI parent
    public void HideGameUI()
    {
        m_gameOverUIParent.SetActive(false);
        m_levelCompleteUIParent.SetActive(false);
    }
}