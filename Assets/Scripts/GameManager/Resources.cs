using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class Resources : MonoBehaviour
{
    [SerializeField] private int[] resources = new int[3];
    [SerializeField] private string[] indexes = new string[3];
    [SerializeField] private TextMeshProUGUI[] texts = new TextMeshProUGUI[3];
    // 0 - Money, 1 - Stone, 2 - Wood

    public void AddResource(string index, int amount)
    {
        resources[StringIndexToInt(index)] += amount;
        UpdateUI();
    }

    public void SubtractResource(string index, int amount)
    {
        resources[StringIndexToInt(index)] -= amount;
        UpdateUI();
    }

    public int GetResource(string index)
    {
        return resources[StringIndexToInt(index)];
    }
    
    public int[] GetAllResources()
    {
        return resources;
    }

    public void SetAllResources(int[] newResources)
    {
        resources = newResources;
        UpdateUI();
    }

    private void UpdateUI()
    {
        for (int i = 0; i < resources.Length; i++)
        {
            int value = resources[i];
            if (value > math.pow(10, 9))
                texts[i].text = Math.Round(value / Math.Pow(10.0, 9), 1) + "B";
            else if (value > Math.Pow(10, 6))
                texts[i].text = Math.Round(value / Math.Pow(10.0, 6), 1) + "M";
            else if (value > Math.Pow(10, 3))
                texts[i].text = Math.Round(value / Math.Pow(10.0, 3), 1) + "K";
            else
                texts[i].text = value.ToString();
        }
    }

    private int StringIndexToInt(string index)
    {
        int intIndex =  Array.FindIndex(indexes, x => x == index);
        if (intIndex == -1)
            Debug.LogError("Wrong Index: " + index);
        return intIndex;
    }
}