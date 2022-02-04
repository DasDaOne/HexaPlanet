using System;
using UnityEngine;

public class Terraformer : MonoBehaviour
{
    [SerializeField] private Material material;

    private void Update()
    {
        foreach (Collider collider in Physics.OverlapSphere(transform.position, 2.5f))
        {
            if (collider.CompareTag("Triangle"))
                collider.GetComponent<MeshRenderer>().material = material;
        }
    }
}
