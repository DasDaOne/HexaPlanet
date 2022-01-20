using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LeftPanelAnimation : MonoBehaviour
{
    private RectTransform rectTransform;
    private bool shown;
    private Tween currentAnimation;
    
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void PlayAnimation()
    {
        currentAnimation.Kill();
        if(shown)
            HideLeftPanel();
        else
            ShowLeftPanel();
    }

    private void ShowLeftPanel()
    {
        currentAnimation = DOTween.To(() => rectTransform.anchoredPosition, x => rectTransform.anchoredPosition = x,
            Vector2.zero, .7f);
        shown = true;
    }

    private void HideLeftPanel()
    {
        currentAnimation = DOTween.To(() => rectTransform.anchoredPosition, x => rectTransform.anchoredPosition = x,
            new Vector2(-rectTransform.sizeDelta.x,0), .7f);
        shown = false;
    }
}
