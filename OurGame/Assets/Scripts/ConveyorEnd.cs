using UnityEngine;

public class ConveyorEnd : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Destroy(other.gameObject);
        }
    }
}
