using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private PlayableDirector spaceshipAnimation;
    
    [SerializeField] private Transform sellContent;
    [SerializeField] private Transform buyContent;
    
    [SerializeField] private string[] sellIndexes;
    [SerializeField] private string[] buyIndexes;
    
    [SerializeField] private int[] sellPrices;
    [SerializeField] private int[] buyPrices;
    
    [SerializeField] private Button shipRequestButton;

    public bool canBuy;
        
    private int[] sellRequest;
    private int[] buyRequest;

    private Resources resources;

    private UnityEvent sellEvent = new UnityEvent();


    private void Start()
    {
        resources = FindObjectOfType<Resources>();
        
        int childCount = sellContent.childCount;
        sellRequest = new int[childCount];
        sellIndexes = new string[childCount];
        sellPrices = new int[childCount];
        int i = 0;
        foreach (Transform child in sellContent)
        {
            sellIndexes[i] = child.GetComponent<ShopPosition>().index;
            sellPrices[i] = child.GetComponent<ShopPosition>().price;
            i++;
        }
        
        childCount = buyContent.childCount;
        buyRequest = new int[childCount];
        buyIndexes = new string[childCount];
        buyPrices = new int[childCount];
        i = 0;
        foreach (Transform child in buyContent)
        {
            buyIndexes[i] = child.GetComponent<ShopPosition>().index;
            buyPrices[i] = child.GetComponent<ShopPosition>().price;
            i++;
        }


    }

    private void Update()
    {
        canBuy = false;
        foreach (int value in sellRequest)
        {
            if (value > 0)
                canBuy = true;
        }

        foreach (int value in buyRequest)
        {
            if (value > 0)
                canBuy = true;
        }
    }

    public void AddSellRequest(string index, int amount)
    {
        sellRequest[StringIndexToInt(index)] = amount;
    }

    public void AddListener(UnityAction action)
    {
        sellEvent.AddListener(action);
    }

    public void LaunchShip()
    {
        spaceshipAnimation.Play();
        foreach(string index in sellIndexes)
        {
            if(index != "Locked")
                resources.SubtractResource(index, sellRequest[StringIndexToInt(index)]);
            if(index != "Locked")
                resources.AddResource("money", sellPrices[StringIndexToInt(index)] * sellRequest[StringIndexToInt(index)]);
        }
        sellEvent.Invoke();
    }
    
    private int StringIndexToInt(string index)
    {
        int intIndex =  Array.FindIndex(sellIndexes, x => x == index);
        if (intIndex == -1)
            Debug.LogError("Wrong Index");
        return intIndex;
    }
}
