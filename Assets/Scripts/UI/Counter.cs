using System;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class Counter : MonoBehaviour
{
    public long value;
    public TextMeshProUGUI text;

    public void Add(long toAdd)
    {
        value += toAdd;
        text.text = value.ToString();
        ChangeText();
    }

    public void Subtract(long toSubtract)
    {
        value -= toSubtract;
        if (value < 0)
            value = 0;
        ChangeText();
    }

    private void ChangeText()
    {
        if (value > math.pow(10, 9))
            text.text = Math.Round(value / Math.Pow(10.0, 9), 1) + "B";
        else if (value > Math.Pow(10, 6))
            text.text = Math.Round(value / Math.Pow(10.0, 6), 1) + "M";
        else if (value > Math.Pow(10, 3))
            text.text = Math.Round(value / Math.Pow(10.0, 3), 1) + "K";
    }
}
