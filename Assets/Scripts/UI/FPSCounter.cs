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
    [SerializeField] private TMP_InputField fpsLockInput;
    [SerializeField] private Toggle fpsLockToggle;
    private bool showFps;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void SetFpsLock(bool state)
    {
        if (state)
        {
            Application.targetFrameRate = Int32.Parse(fpsLockInput.text);
            fpsLockToggle.isOn = true;
        }
        else
            Application.targetFrameRate = 0;
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
