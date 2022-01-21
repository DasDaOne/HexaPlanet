using DG.Tweening;
using UnityEngine;

public class LeftPanelAnimation : MonoBehaviour
{
    [SerializeField] private float xPosition;
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
    
    public void PlayShowAnimation()
    {

        if (!shown)
        {
            currentAnimation.Kill();
            ShowLeftPanel();
        }
    }
    
    public void PlayHideAnimation()
    {
        if (shown)
        {
            currentAnimation.Kill();
            HideLeftPanel();
        }
    }

    private void ShowLeftPanel()
    {
        shown = true;
        currentAnimation = DOTween.To(() => rectTransform.anchoredPosition, x => rectTransform.anchoredPosition = x,
            new Vector2(xPosition, 0), .7f);
    }

    private void HideLeftPanel()
    {
        shown = false;
        currentAnimation = DOTween.To(() => rectTransform.anchoredPosition, x => rectTransform.anchoredPosition = x,
            new Vector2(-rectTransform.sizeDelta.x - xPosition,0), .7f);
    }
}
