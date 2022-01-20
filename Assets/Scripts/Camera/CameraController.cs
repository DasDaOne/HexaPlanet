using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float initRotationSpeed;
    [SerializeField] private ZoomController zoomController;
    [SerializeField] private ZoomAnimationController zoomAnimationController;
    
    private float rotationSpeed;
    private bool moving;
    private List<float> magnitudes = new List<float>();
    private List<Vector3> normales = new List<Vector3>();
    private Vector2 futureRotation;
    
    private GameManager gm;
    private Planet planet;
    private Camera cam;
    private Transform planetTransform;

    private bool lockControl;

    private void Start()
    {
        gm = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        cam = Camera.main;
        planet = GameObject.FindWithTag("Planet").GetComponent<Planet>();
        planetTransform = planet.transform;
        rotationSpeed = initRotationSpeed;
    }

    private void Update()
    {
        if(lockControl)
            return;
        SpinCamera();
        int touchId = -1;
        if(Input.touchCount > 0)
            touchId = Input.touches[0].fingerId;
        if (Input.touchCount == 1 && !(touchId != -1 && EventSystem.current.IsPointerOverGameObject(touchId)))
        {
            StartCoroutine(HitMineral(Input.touches[0]));
        }
    }

    public void SwitchLock()
    {
        lockControl = !lockControl;
        zoomController.lockControl = lockControl;
        zoomAnimationController.lockControl = lockControl;
    }

    private IEnumerator HitMineral(Touch touch)
    {
        yield return new WaitForSeconds(.1f);
        if (touch.phase == TouchPhase.Began && !moving)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(new Vector3(touch.position.x, touch.position.y, 0));
            if (Physics.Raycast(ray, out hit, 100f))
            {
                switch (hit.collider.tag)
                {
                    case "Stone":
                        gm.AddMaterial(1);
                        zoomAnimationController.AddZoomAnimation();
                        break;
                    case "Cosmodrome":
                        gm.ShowShop();
                        break;
                }
            }
        }
    }
    

    void SpinCamera()
    {
        int touchId = -1;
        if(Input.touchCount > 0)
            touchId = Input.touches[0].fingerId;
        rotationSpeed = initRotationSpeed + initRotationSpeed * Remap(zoomController.zoom, -10, 10, -.2f, 2f);
        if (Input.touchCount > 0 && !(touchId != -1 && EventSystem.current.IsPointerOverGameObject(touchId)))
        {
            Vector3 deltaPos = new Vector3(Input.touches.Sum(x => x.deltaPosition.x), Input.touches.Sum(x => x.deltaPosition.y)) / Input.touchCount;
            deltaPos *= rotationSpeed * Mathf.Deg2Rad * Time.deltaTime;
            deltaPos = new Vector3(deltaPos.x, -deltaPos.y);
            if (deltaPos.magnitude > 0)
                moving = true;
            transform.RotateAround(planetTransform.position, transform.up, deltaPos.x);        
            transform.RotateAround(planetTransform.position, transform.right, deltaPos.y);
            normales.Add(deltaPos.normalized);
            if(normales.Count > 5)
                normales.RemoveAt(0);
            magnitudes.Add(deltaPos.magnitude);
            if(deltaPos.magnitude < .1f)
                futureRotation = Vector2.zero;
            if(magnitudes.Count > 15)
                magnitudes.RemoveAt(0);
        }
        else
        {
            if(moving && magnitudes.Count != 0 && normales.Count != 0)
                futureRotation = new Vector3(normales.Sum(x => x.x), normales.Sum(x => x.y)) / normales.Count * (magnitudes.Sum() / magnitudes.Count);
            else if (!moving)
            {
                magnitudes.Clear();
                normales.Clear();
            }

            moving = false;
            transform.RotateAround(planetTransform.position, transform.up, futureRotation.x);
            transform.RotateAround(planetTransform.position, transform.right, futureRotation.y);
            futureRotation = Vector2.Lerp(futureRotation, Vector2.zero, .1f);
        }
    }


    private float Remap(float value, float min1, float max1, float min2, float max2)
    {
        return (value - min1) / (max1 - min1) * (max2 - min2) + min2;
    }
}
