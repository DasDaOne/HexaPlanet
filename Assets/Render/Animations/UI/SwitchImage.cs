using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SwitchImage : MonoBehaviour
{
    [SerializeField] private List<Sprite> images;
    private Image image;
    private int currentIndex = 0;
    
    private void Start()
    {
        image = GetComponent<Image>();
    }

    public void Switch()
    {
        currentIndex++;
        if (currentIndex >= images.Count)
            currentIndex = 0;
        image.sprite = images[currentIndex];
    }
}
