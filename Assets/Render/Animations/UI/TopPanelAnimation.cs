using DG.Tweening;
using UnityEngine;

public class TopPanelAnimation : MonoBehaviour
{
    [SerializeField] private float yPosition;
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
            HideTopPanel();
        else
            ShowTopPanel();
    }
    
    public void PlayShowAnimation()
    {
        if (!shown)
        {
            currentAnimation.Kill();
            ShowTopPanel();
        }
    }
    
    public void PlayHideAnimation()
    {
        if (shown)
        {
            currentAnimation.Kill();
            HideTopPanel();
        }
    }

    private void ShowTopPanel()
    {
        shown = true;
        currentAnimation = DOTween.To(() => rectTransform.anchoredPosition, x => rectTransform.anchoredPosition = x,
            new Vector2(0, yPosition), .7f);
    }

    private void HideTopPanel()
    {
        shown = false;
        currentAnimation = DOTween.To(() => rectTransform.anchoredPosition, x => rectTransform.anchoredPosition = x,
            new Vector2(0,rectTransform.sizeDelta.y), .7f);
    }
}
