using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CounterButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float originalPeriod;
    [SerializeField] private float speedUpPeriod;
    [SerializeField] private UnityEvent holdEvent;
    private bool isPointerDown;
    private float timer;
    private float speedUpTimer;
    private float period;

    private void Start()
    {
        period = originalPeriod;
    }

    private void Update()
    {
        if (!isPointerDown)
        {
            timer = 0;
            speedUpTimer = 0;
            return;
        }

        if (timer >= period)
        {
            holdEvent.Invoke();
            timer = 0;
        }

        if (speedUpTimer > speedUpPeriod)
        {
            period /= 1.1f;
            speedUpTimer = 0;
        }

        speedUpTimer += Time.deltaTime;
        timer += Time.deltaTime;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        holdEvent.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        period = originalPeriod;
        isPointerDown = false;
    }
}
