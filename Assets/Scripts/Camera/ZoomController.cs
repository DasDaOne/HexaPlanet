using UnityEngine;

public class ZoomController : MonoBehaviour
{
    [SerializeField] private float zoomSpeed;
    private float initFingerDistance;

    [HideInInspector] public float zoom;

    private Transform planetTransform;
    private float initDistance;
    
    public bool lockControl;
    
    private void Start()
    {
        planetTransform = GameObject.FindWithTag("Planet").transform;
        initDistance = Vector3.Distance(transform.position, planetTransform.position);
    }

    private void Update()
    {
        if(lockControl)
            return;
        zoom = Vector3.Distance(transform.position, planetTransform.position) - initDistance;
        if (Input.touchCount == 2)
        {
            Zoom();
        }
    }

    private void Zoom()
    {
        Touch t1 = Input.touches[0];
        Touch t2 = Input.touches[1];

        if (t2.phase == TouchPhase.Began)
        {
            initFingerDistance = Vector2.Distance(t1.position, t2.position);
        }
        else if (t1.phase == TouchPhase.Moved || t2.phase == TouchPhase.Moved)
        {
            float currentFingerDistance = Vector2.Distance(t1.position, t2.position);
            float zoomDelta = currentFingerDistance - initFingerDistance;

            initFingerDistance = currentFingerDistance;

            float z = Mathf.Clamp(transform.localPosition.z + zoomDelta * Time.deltaTime * zoomSpeed, -10, 10);
            
            transform.localPosition = new Vector3(0, transform.localPosition.y, z);
        }
    }
}
