using System;
using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Counter : MonoBehaviour
{
    private Resources resources;
    private int value;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private string index;
    private Shop shop;
    private Image image;

    private void Start()
    {
        shop = FindObjectOfType<Shop>();
        resources = FindObjectOfType<Resources>();
        image = GetComponent<Image>();
        shop.AddListener(Reset);
    }

    public void Add()
    {
        if(value < 1000)
            value += 1;
        else if (value < 1000000)
            value += 100;
        else if (value < 1000000000)
            value += 1000;
        else
            value += 10000;
        if (value >= resources.GetResource(index))
        {
            value = resources.GetResource(index);
            ReachedBoundsAnimation();
        }
        ChangeText();
        AddSellRequest();
    }

    public void Subtract()
    {
        if(value < 1000)
            value -= 1;
        else if (value < 1000000)
            value -= 100;
        else if (value < 1000000000)
            value -= 1000;
        else
            value -= 10000;
        if (value < 0)
        {
            value = 0;
            ReachedBoundsAnimation();
        }
        ChangeText();
        AddSellRequest();
    }

    private void Reset()
    {
        value = 0;
        ChangeText();
    }

    private void ReachedBoundsAnimation()
    {
        Sequence reachedBoundsAnimation = DOTween.Sequence();
        reachedBoundsAnimation.Append(image.DOColor(Color.red, .1f));
        reachedBoundsAnimation.Append(image.DOColor(Color.white, .1f));
    }

    private void ChangeText()
    {
        if (value > math.pow(10, 9))
            text.text = Math.Round(value / Math.Pow(10.0, 9), 1) + "B";
        else if (value > Math.Pow(10, 6))
            text.text = Math.Round(value / Math.Pow(10.0, 6), 1) + "M";
        else if (value > Math.Pow(10, 3))
            text.text = Math.Round(value / Math.Pow(10.0, 3), 1) + "K";
        else
            text.text = value.ToString();
    }
    
    private void AddSellRequest() 
    {
        shop.AddSellRequest(index, value);
    }
}
