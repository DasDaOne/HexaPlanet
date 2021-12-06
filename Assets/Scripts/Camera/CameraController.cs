using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

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

    private float targetZ;
    
    private GameManager gm;
    private Planet planet;
    private Camera camera;
    private Transform planetTransform;
    


    private void Start()
    {
        gm = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        camera = Camera.main;
        planet = GameObject.FindWithTag("Planet").GetComponent<Planet>();
        planetTransform = planet.transform;
        rotationSpeed = initRotationSpeed;
    }

    private void Update()
    {
        SpinCamera();
        if (Input.touchCount == 1)
        {
            HitMineral();
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
                    zoomAnimationController.AddZoomAnimation();
                }
            }
        }
    }
    

    void SpinCamera()
    {
        rotationSpeed = initRotationSpeed + initRotationSpeed * Remap(zoomController.zoom, -10, 10, -.2f, 2f);
        if (Input.touchCount > 0)
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
