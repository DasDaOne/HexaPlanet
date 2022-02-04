using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ShipRequestButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private int delay;
    [SerializeField] private Shop shop;
    private Button button;
    public DateTime timer;
    
    private void Start()
    {
        button = GetComponent<Button>();
    }

    public void AddDelay()
    {
        timer = DateTime.Now.AddSeconds(delay);
    }

    private void LateUpdate()
    {
        if (DateTime.Now > timer)
        {
            text.text = "REQUEST SPACESHIP";
            button.interactable = shop.canBuy;
        }
        else
        {
            button.interactable = false;
            TimeSpan remainingTime = timer.Subtract(DateTime.Now);
            if (remainingTime.Minutes > 0)
                text.text = remainingTime.ToString("%m' min.'");
            else
                text.text = remainingTime.ToString("%s' sec.'");
        }
    }
}
