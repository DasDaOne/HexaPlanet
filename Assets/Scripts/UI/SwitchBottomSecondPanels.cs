using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchBottomSecondPanels : MonoBehaviour
{
    [SerializeField] private Image panel1;
    [SerializeField] private GameObject panel2;

    public void ShowPanel1()
    {
        panel2.SetActive(false);
        panel1.color = Color.white;
    }

    public void ShowPanel2()
    {
        panel1.color = Color.gray;
        panel2.SetActive(true);
    }
}
