using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int stone;

    [Header("Text fields")] 
    public TextMeshProUGUI stoneText;
    
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
