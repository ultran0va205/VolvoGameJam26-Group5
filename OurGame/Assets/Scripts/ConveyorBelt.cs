using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public float speed = 2f;
    public Vector3 moveDir = Vector3.forward;

    private List<Rigidbody> onBelt = new List<Rigidbody>();

    private void FixedUpdate()
    {
        // Move all items currently on the belt
        foreach (var rb in onBelt)
        {
            if (rb != null)
            {
                rb.linearVelocity = moveDir.normalized * speed;
            }
        }
    }

    // Use trigger colliders instead of physics collisions
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null && !onBelt.Contains(rb))
        {
            onBelt.Add(rb);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null && onBelt.Contains(rb))
        {
            onBelt.Remove(rb);
            rb.linearVelocity = Vector3.zero;
        }
    }
}
