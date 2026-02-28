using UnityEngine;

public class PickableItem : MonoBehaviour
{
    [SerializeField] Material normalMat;
    [SerializeField] Material highlightedMat;
    [SerializeField] MeshRenderer parentRenderer;

    private bool isPlayerNearby = false;

    private void Awake()
    {
        if (parentRenderer == null)
            parentRenderer = GetComponentInParent<MeshRenderer>();

        if (parentRenderer != null)
            parentRenderer.material = normalMat;
        else
            Debug.LogWarning("PickableItem: No MeshRenderer found in parent!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("Pickable");
            if (parentRenderer != null)
                parentRenderer.material = highlightedMat;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            if (parentRenderer != null)
                parentRenderer.material = normalMat;
        }
    }

    public bool IsInteractible() => isPlayerNearby;

}
