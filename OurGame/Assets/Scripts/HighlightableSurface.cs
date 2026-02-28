using UnityEngine;

public class HighlightableSurface : MonoBehaviour
{
    [SerializeField] private Material normalMat;
    [SerializeField] private Material highlightedMat;

    private MeshRenderer meshRenderer;
    private bool isPlayerNearby = false;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = normalMat;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            meshRenderer.material = highlightedMat;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            meshRenderer.material = normalMat;
        }
    }

    public bool IsInteractible() => isPlayerNearby;
}
