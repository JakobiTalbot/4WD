﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

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

    private void Awake()
    {
        instance = this;
    }

    public void SetFuelValue(float value, float maxValue)
    {
        float percent = value / maxValue;
        m_fuelGauge.value = percent;
        m_fuelGaugeFill.color = Color.Lerp(m_minFuelColour, m_maxFuelColour, percent) * 2f;
    }

    public void GameOver()
    {
        m_gameOverUIParent.SetActive(true);
    }
}