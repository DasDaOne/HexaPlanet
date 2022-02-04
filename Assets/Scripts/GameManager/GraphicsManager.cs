using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField fpsLockInput;
    [SerializeField] private Toggle fpsLockToggle;

    private void Start()
    {
        Application.targetFrameRate = 60;
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

}
