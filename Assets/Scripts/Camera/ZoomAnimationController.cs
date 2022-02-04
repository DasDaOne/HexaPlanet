using DG.Tweening;
using UnityEngine;

public class ZoomAnimationController : MonoBehaviour
{
    private float targetZ;
    
    public bool lockControl;

    public void ZoomAnimation()
    {
        if(lockControl)
            return;
        Sequence anim = DOTween.Sequence();
        anim.Append(transform.DOLocalMove(new Vector3(0,0,2f), .05f));
        anim.Append(transform.DOLocalMove(new Vector3(0,0, -.4f), .05f));
        anim.Append(transform.DOLocalMove(new Vector3(0,0, 0), .05f));
    }
}
