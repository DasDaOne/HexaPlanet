using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Planet : MonoBehaviour
{
    [SerializeField] private Transform hexagons;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private GameObject[] tiles;
    // 0 - Stone
    // 1 - DeadGround
    
    
    private void Start()
    {
        CreateHexagons();
    }

    private void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
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
}
