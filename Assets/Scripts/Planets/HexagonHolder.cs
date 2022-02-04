using System;
using UnityEngine;

public class HexagonHolder : MonoBehaviour
{
    [SerializeField] private int index;
    [HideInInspector] public Hexagon hexagon;

    private Resources resources;
    private ZoomAnimationController zoomAnimationController;
    private GameManager gameManager;
    
    private void Start()
    {
        hexagon = new Hexagon(index);
        resources = FindObjectOfType<Resources>();
        zoomAnimationController = FindObjectOfType<ZoomAnimationController>();
        gameManager = resources.GetComponent<GameManager>();
    }

    private void OnMouseDown()
    {
        switch (tag)
        {
            case "Cosmodrome":
                gameManager.ShowShop();
                break;
            case "Planet":
                break;
            default:
                resources.AddResource(tag.ToLower(), 1);
                zoomAnimationController.ZoomAnimation();
                break;
        }
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

