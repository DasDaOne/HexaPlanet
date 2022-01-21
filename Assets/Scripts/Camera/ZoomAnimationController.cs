using UnityEngine;

public class ZoomAnimationController : MonoBehaviour
{    
    [SerializeField] private float hitAnimationFactor;
    [SerializeField] private float hitAnimationSpeed;
    [SerializeField] private ZoomController zoomController;

    private float targetZ;
    
    public bool lockControl;

    private void Update()
    {
        if (lockControl)
            return;
        ZoomAnimation();
    }

    void ZoomAnimation()
    {
        if (targetZ - transform.localPosition.z < .1f)
            targetZ = 0;
        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, 0, targetZ), hitAnimationSpeed);
    }

    public void AddZoomAnimation()
    {
        targetZ += hitAnimationFactor + hitAnimationFactor * Remap(zoomController.zoom, -10, 10, -.8f, 0);
        targetZ = Mathf.Clamp(targetZ, 0, 4);
    }
    
    private float Remap(float value, float min1, float max1, float min2, float max2)
    {
        return (value - min1) / (max1 - min1) * (max2 - min2) + min2;
    }
}
