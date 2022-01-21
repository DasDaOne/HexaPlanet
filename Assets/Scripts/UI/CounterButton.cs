using UnityEngine;
using UnityEngine.EventSystems;

public class CounterButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float period;
    [SerializeField] private float speedUpPeriod;
    [SerializeField] private LongUnityEvent holdEvent;
    private bool isPointerDown;
    private float timer;
    private float speedUpTimer;
    private long toAdd;


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
            holdEvent.Invoke(toAdd);
            timer = 0;
        }

        if (speedUpTimer > speedUpPeriod)
        {
            toAdd += Mathf.RoundToInt(toAdd * .8f);
            speedUpTimer = 0;
        }

        speedUpTimer += Time.deltaTime;
        timer += Time.deltaTime;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        holdEvent.Invoke(1);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
        toAdd = 1;
    }
}
