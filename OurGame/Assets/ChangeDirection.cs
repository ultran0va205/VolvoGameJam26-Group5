using UnityEngine;

public class ChangeDirection : MonoBehaviour
{
    [SerializeReference] private Vector3 newDir;

    private void OnTriggerEnter(Collider other)
    {
        ConveyorItem item = other.GetComponent<ConveyorItem>();
        Debug.Log("Box hit trigger");
        if (item != null)
        {
            item.SetDirection(newDir);
            Debug.Log("Change direction");
        }
    }
}
