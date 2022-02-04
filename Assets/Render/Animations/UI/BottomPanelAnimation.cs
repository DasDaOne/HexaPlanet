using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class BottomPanelAnimation : MonoBehaviour
{
    private RectTransform rectTransform;
    private bool shown;
    private Tween currentAnimation;
    
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (Input.touchCount > 0 && !EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId) 
            || Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
        { 
            currentAnimation.Kill();
            HideBottomPanel();
        }
    }

    public void PlayAnimation()
    {
        currentAnimation.Kill();
        if(shown)
            HideBottomPanel();
        else
            ShowBottomPanel();
    }

    private void ShowBottomPanel()
    {
        currentAnimation = DOTween.To(() => rectTransform.anchoredPosition, x => rectTransform.anchoredPosition = x,
            Vector2.zero, .7f);
        shown = true;
    }

    private void HideBottomPanel()
    {
        currentAnimation = DOTween.To(() => rectTransform.anchoredPosition, x => rectTransform.anchoredPosition = x,
            new Vector2(0, -rectTransform.sizeDelta.y), .7f);
        shown = false;
    }
}
