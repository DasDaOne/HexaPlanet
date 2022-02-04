using System;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonAnimation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    
    private Button button;

    private Tween currentTween;

    private void Start()
    {
        button = GetComponent<Button>();
        button.ObserveEveryValueChanged(x => x.interactable).Subscribe(x =>
        {
            if(x)
                text.color = Color.black;
            else
                text.color = Color.white;
        });
    }
}
