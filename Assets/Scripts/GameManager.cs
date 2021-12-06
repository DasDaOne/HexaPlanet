using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Resources")]
    public int stone;

    [Header("Text fields")] 
    [SerializeField] private TextMeshProUGUI stoneText;

    [Header("Panel")] 
    [SerializeField] private GameObject leftPanel;
    
    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        stoneText.text = "Stone: " + stone;
    }

    public void AddMaterial(int amount)
    {
        stone += amount;
    }
}
