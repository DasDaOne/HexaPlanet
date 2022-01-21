using UnityEngine;

public class BackgroundPlanet : MonoBehaviour
{
    [SerializeField] private Vector3 rotation;
    [SerializeField] private float rotationSpeed;
    void Update()
    {
        transform.Rotate(rotation, rotationSpeed * Time.deltaTime);
    }
}
