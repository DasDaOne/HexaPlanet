using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float initRotationSpeed;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float hitAnimationFactor;
    [SerializeField] private float hitAnimationSpeed;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform cameraZoomAnimationTransform;

    private float rotationSpeed;
    private bool moving;
    private List<float> magnitudes = new List<float>();
    private List<Vector3> normales = new List<Vector3>();
    private Vector2 futureRotation;
    private float zoom;

    private float targetZ;
    
    private float initDistance;
    
    private GameManager gm;
    private Planet planet;
    private Camera camera;
    private Transform planetTransform;

    private float initFingerDistance;


    private void Start()
    {
        gm = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        camera = Camera.main;
        planet = GameObject.FindWithTag("Planet").GetComponent<Planet>();
        planetTransform = planet.transform;
        rotationSpeed = initRotationSpeed;
        initDistance = Vector3.Distance(transform.position, planetTransform.position);
    }

    private void Update()
    {
        SpinCamera();
        ZoomAnimation();
        if (Input.touchCount == 1 && !moving)
        {
            HitMineral();
        }
        else if (Input.touchCount == 2)
        {
            Zoom();
        }
    }

    private void HitMineral()
    {
        Touch touch = Input.touches[0];
        if (touch.phase == TouchPhase.Ended)
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(new Vector3(touch.position.x, touch.position.y, 0));
            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.collider.CompareTag("Stone"))
                {
                    gm.AddMaterial(1);
                    AddZoomAnimation();
                }
            }
        }
    }

    void ZoomAnimation()
    {
        if (targetZ - cameraZoomAnimationTransform.localPosition.z < .1f)
            targetZ = 0;
        cameraZoomAnimationTransform.localPosition = Vector3.Lerp(cameraZoomAnimationTransform.localPosition, new Vector3(0, 0, targetZ), hitAnimationSpeed);
    }

    void AddZoomAnimation()
    {
        targetZ += hitAnimationFactor + hitAnimationFactor * Remap(zoom, -10, 10, -.8f, 0);
        targetZ = Mathf.Clamp(targetZ, 0, 4);
    }

    void SpinCamera()
    {
        zoom = Vector3.Distance(cameraTransform.position, planetTransform.position) - initDistance;
        rotationSpeed = initRotationSpeed + initRotationSpeed * Remap(zoom, -10, 10, -.2f, 2f);
        if (Input.touchCount >= 1 && Input.touchCount <= 2)
        {
            Vector3 deltaPos;
            if (Input.touchCount == 1)
                deltaPos = Input.touches[0].deltaPosition;
            else
                deltaPos = new Vector3(Input.touches.Sum(x => x.deltaPosition.x), Input.touches.Sum(x => x.deltaPosition.y)) / 2;
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
            if(deltaPos.magnitude < .01f)
                futureRotation = Vector2.zero;
            if(magnitudes.Count > 15)
                magnitudes.RemoveAt(0);
        }
        else if (Input.touchCount == 0)
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

            float z = Mathf.Clamp(cameraTransform.localPosition.z + zoomDelta * Time.deltaTime * zoomSpeed, -10, 10);
            
            cameraTransform.localPosition = new Vector3(0,0, z);
        }
    }

    private float Remap(float value, float min1, float max1, float min2, float max2)
    {
        return (value - min1) / (max1 - min1) * (max2 - min2) + min2;
    }
}
