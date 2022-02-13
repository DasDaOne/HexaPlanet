using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
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
    private Vector3 prevMousePosition;
    
    private GameManager gm;
    private Resources resources;
    private Planet planet;
    private Camera cam;
    private Transform planetTransform;

    private bool lockControl;

    private void Start()
    {
        gm = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        resources = gm.GetComponent<Resources>();
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
        if (Input.touchCount > 0)
        {
            touchId = Input.touches[0].fingerId;
        }
        if (Input.touchCount == 1 && !(touchId != -1 && EventSystem.current.IsPointerOverGameObject(touchId)))
        {
            HitMineral(Input.touches[0]);
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0) && Input.touchCount == 0 && !EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log(1);
            HitMineral(Input.mousePosition);
        }
    }

    private void HitMineral(Touch touch)
    {
        Observable.Timer(TimeSpan.FromSeconds(.1f)).TakeUntilDisable(gameObject)
            .Subscribe(x =>
            {
                if (touch.phase == TouchPhase.Began && !moving)
                {
                    CheckMineral(cam.ScreenPointToRay(touch.position));
                }
            });
    }

    private void HitMineral(Vector2 mousePos)
    {
        if (!moving)
        {
            CheckMineral(cam.ScreenPointToRay(mousePos));
        }
    }

    private void CheckMineral(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            switch (hit.collider.tag)
            {
                case "Cosmodrome":
                    gm.ShowShop();
                    break;
                case "Planet":
                    break;
                case "Triangle":
                    break;
                default:
                    resources.AddResource(hit.collider.tag.ToLower(), 1);
                    zoomAnimationController.ZoomAnimation();
                    break;
            }
        }
    }

    void SpinCamera()
    {
        int touchId = -1;
        if(Input.touchCount > 0)
            touchId = Input.touches[0].fingerId;
        rotationSpeed = initRotationSpeed + initRotationSpeed * Remap(zoomController.zoom, -10, 10, -.2f, 2f);
        if (Input.touchCount > 0 && !EventSystem.current.IsPointerOverGameObject(touchId) && Input.touches[0].phase != TouchPhase.Ended)
        {
            Vector3 deltaPos = new Vector3(Input.touches.Sum(x => x.deltaPosition.x), Input.touches.Sum(x => x.deltaPosition.y)) / Input.touchCount;
            SpinCamera(deltaPos);
        }
        else if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && Input.touchCount == 0)
        {
            SpinCamera(Input.mousePosition - prevMousePosition);
        }
        else
        {
            if(moving && magnitudes.Count != 0 && normales.Count != 0)
                futureRotation = new Vector3(normales.Sum(x => x.x), normales.Sum(x => x.y)) / normales.Count * (magnitudes.Sum() / magnitudes.Count);
            if (!moving)
            {
                magnitudes.Clear();
                normales.Clear();
            }
            moving = false;
            transform.RotateAround(planetTransform.position, transform.up, futureRotation.x);
            transform.RotateAround(planetTransform.position, transform.right, futureRotation.y);
            futureRotation = Vector2.Lerp(futureRotation, Vector2.zero, .1f);
        }
        prevMousePosition = Input.mousePosition;
    }

    private void SpinCamera(Vector3 deltaPos)
    {
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


    private float Remap(float value, float min1, float max1, float min2, float max2)
    {
        return (value - min1) / (max1 - min1) * (max2 - min2) + min2;
    }
}
