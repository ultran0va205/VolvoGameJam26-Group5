using UnityEngine;

public class ConveyorEnd : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ConveyorItem>() != null)
        {
            Destroy(other.gameObject);
        }
    }
}
