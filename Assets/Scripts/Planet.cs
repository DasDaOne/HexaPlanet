using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Planet : MonoBehaviour
{
    [SerializeField] private Transform hexagons;
    [SerializeField] private GameObject[] tiles;
    // 0 - Stone
    // 1 - DeadGround
    
    
    private CameraController cameraController;

    private Vector3 initScale;
    private Vector3 targetScale;
    
    private void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        CreateHexagons();
        initScale = transform.localScale;
    }

    private void CreateHexagons()
    {
        foreach (Transform child in hexagons)
        {
            if (Random.Range(0, 10) < 2)
            {
                GameObject stone = Instantiate(tiles[0], child.position, child.rotation, transform);
                stone.transform.Rotate(new Vector3(0,Random.Range(0,360)), Space.Self);
                stone.transform.localScale = child.localScale;
                Destroy(child.gameObject);
            }
            else if(Random.Range(0, 10) < 5)
            {
                Destroy(child.gameObject);
            }
            else
            {
                GameObject deadGround = Instantiate(tiles[1], child.position, child.rotation, transform);
                deadGround.transform.Rotate(new Vector3(0,Random.Range(0,360)), Space.Self);
                deadGround.transform.localScale = child.localScale;
                Destroy(child.gameObject);
            }
        }
    }

    // void PlanetSizeAnimation()
    // {
    //     if ((targetScale - transform.localScale).magnitude < .1f)
    //     {
    //         targetScale = initScale;
    //     }
    //     transform.localScale = Vector3.Lerp(transform.localScale, targetScale, .5f);
    // }
    //
    //
    //
    // public void AddPlanetSizeAnimation()
    // {
    //     targetScale = transform.localScale + Vector3.one * scaleFactor;
    // }
}
