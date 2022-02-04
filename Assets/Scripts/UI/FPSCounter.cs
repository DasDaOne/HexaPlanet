using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]
public class FPSCounter : MonoBehaviour
{
    private TextMeshProUGUI text;
    private float timeForNextUpdate;
    [SerializeField] private float updateTime;
    private bool showFps;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

 
    public void ShowFps(bool state)
    {
        showFps = state;
    }

    private void Update()
    {
        if (!showFps)
        {
            text.text = "";
            return;
        }
        if (Time.time > timeForNextUpdate)
        {
            text.text = ((int) (1f / Time.unscaledDeltaTime)).ToString();
            timeForNextUpdate += updateTime;
        }
    }
}
