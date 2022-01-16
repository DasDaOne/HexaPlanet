using System;
using Unity.Mathematics;
using UnityEngine;

public class HexagonHolder : MonoBehaviour
{
    [SerializeField] private int index;
    [HideInInspector] public Hexagon hexagon;
    
    private void Start()
    {
        hexagon = new Hexagon(index);
    }
}

[Serializable]
public class Hexagon
{
    public int index;
    

    public Hexagon(int index)
    {
        this.index = index;
    }
}

