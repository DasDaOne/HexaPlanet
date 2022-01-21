using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class Planet : MonoBehaviour
{
    [SerializeField] private GameManager gm;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private GameObject hexagonsPrefab;
    [SerializeField] private GameObject[] tiles;
    // 0 - Stone
    // 1 - DeadGround
    [SerializeField] private GameObject[] buildings;


    // 0 - Cosmodrome


    private bool canRotate = true;

    private GameObject hexagonsHolder;
    
    private void Update()
    {
        if(canRotate)
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    public void SwitchRotationLock()
    {
        canRotate = !canRotate;
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
        hexagonsHolder = Instantiate(hexagonsPrefab, transform);
        hexagonsHolder.name = "Hexagons";
        List<Transform> hexagonChildren = new List<Transform>();
        foreach (Transform child in hexagonsHolder.transform)
        {
            hexagonChildren.Add(child);
        }
        foreach (Transform child in hexagonChildren)
        {
            if (child.name == "33")
            {
                Transform cosmodrome = transform.Find("Cosmodrome");
                cosmodrome.SetParent(hexagonsHolder.transform);
                SetBuildingScaleName(cosmodrome.gameObject, child);
                Destroy(child.gameObject);
                continue;
            }
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

    private void SetBuilding(int buildingId, int tileId)
    {
        Transform tile = hexagonsHolder.transform.Find(tileId.ToString());
        GameObject building = Instantiate(buildings[buildingId], tile.position, tile.rotation, hexagonsHolder.transform);
        SetBuildingScaleName(building, tile);
        Destroy(tile.gameObject);
    }

    private void SetBuildingScaleName(GameObject building, Transform tile)
    {
        building.transform.localScale = tile.localScale;
        building.name = tile.name;
    }

    public Dictionary<int, Hexagon> SaveHexagons()
    {
        hexagonsHolder = null;
        foreach (Transform child in transform)
        {
            if (child.name == "Hexagons")
                hexagonsHolder = child.gameObject;
        }
        Dictionary<int, Hexagon> hexagons = new Dictionary<int, Hexagon>();
        
        foreach (Transform child in hexagonsHolder.transform)
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
        hexagonsHolder = Instantiate(hexagonsPrefab, transform);
        hexagonsHolder.name = "Hexagons";
        List<Transform> hexagonChildren = new List<Transform>();
        foreach (Transform child in hexagonsHolder.transform)
        {
            hexagonChildren.Add(child);
        }
        foreach (Transform child in hexagonChildren)
        {
            int childName = Int32.Parse(child.name);
            if (hexagons.ContainsKey(childName))
            {
                GameObject tile;
                if (hexagons[childName].index >= 0)
                {
                    if (hexagons[childName].index >= tiles.Length)
                    {
                        gm.ResetProgress();
                        return;
                    }
                    tile = Instantiate(tiles[hexagons[childName].index], child.position,
                        child.rotation, hexagonsHolder.transform);
                    SetRotationScaleName(tile, child);
                }
                else if(hexagons[childName].index != -1)
                {
                    tile = Instantiate(buildings[hexagons[childName].index + 1], child.position,
                        child.rotation, hexagonsHolder.transform);
                    SetRotationScaleName(tile, child);
                }
            }
            Destroy(child.gameObject);
        }
    }
}
