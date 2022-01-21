using DG.Tweening;
using UnityEngine;

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
