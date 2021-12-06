using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Planet : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private GameObject hexagonsPrefab;
    [SerializeField] private GameObject[] tiles;
    // 0 - Stone
    // 1 - DeadGround
    
    private void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
    
    public void InitializeHexagons()
    {
        foreach (Transform child in transform)
        {
            if (child.name == "Hexagons")
            {
                Destroy(child.gameObject);
            }
        }
        GameObject hexagonsHolder = Instantiate(hexagonsPrefab, transform);
        hexagonsHolder.name = "Hexagons";
        List<Transform> hexagonChilds = new List<Transform>();
        foreach (Transform child in hexagonsHolder.transform)
        {
            hexagonChilds.Add(child);
        }
        foreach (Transform child in hexagonChilds)
        {
            if (Random.Range(0, 10) < 2)
            {
                GameObject stone = Instantiate(tiles[0], child.position, child.rotation, hexagonsHolder.transform);
                SetRotationScaleName(stone, child);
                Destroy(child.gameObject);
            }
            else if(Random.Range(0, 10) < 5)
            {
                Destroy(child.gameObject);
            }
            else
            {
                GameObject deadGround = Instantiate(tiles[1], child.position, child.rotation, hexagonsHolder.transform);
                SetRotationScaleName(deadGround, child);
                Destroy(child.gameObject);
            }
        }
    }

    private void SetRotationScaleName(GameObject go, Transform child)
    {
        go.transform.Rotate(new Vector3(0,Random.Range(0,360)), Space.Self);
        go.transform.localScale = child.localScale;
        go.name = child.name;
    }

    public Dictionary<int, Hexagon> SaveHexagons()
    {
        Transform hexagonsHolder = null;
        foreach (Transform child in transform)
        {
            if (child.name == "Hexagons")
                hexagonsHolder = child;
        }
        Dictionary<int, Hexagon> hexagons = new Dictionary<int, Hexagon>();
        
        foreach (Transform child in hexagonsHolder)
        {
            hexagons[Int32.Parse(child.name)] = child.GetComponent<HexagonHolder>().hexagon;
        }
        
        return hexagons;
    }

    public void LoadHexagons(Dictionary<int, Hexagon> hexagons)
    {
        foreach (Transform child in transform)
        {
            if (child.name == "Hexagons")
            {
                Destroy(child.gameObject);
            }
        }
        GameObject hexagonsHolder = Instantiate(hexagonsPrefab, transform);
        hexagonsHolder.name = "Hexagons";
        List<Transform> hexagonChilds = new List<Transform>();
        foreach (Transform child in hexagonsHolder.transform)
        {
            hexagonChilds.Add(child);
        }
        foreach (Transform child in hexagonChilds)
        {
            int childName = Int32.Parse(child.name);
            if (hexagons.ContainsKey(childName))
            {
                GameObject deadGround = Instantiate(tiles[hexagons[childName].index], child.position, child.rotation, hexagonsHolder.transform);
                SetRotationScaleName(deadGround, child);
                Destroy(child.gameObject);
            }
            else
                Destroy(child.gameObject);
        }
    }
}
