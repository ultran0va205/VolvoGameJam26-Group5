using UnityEngine;

public class PickableItem : MonoBehaviour
{
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite highlightedSprite;
    [SerializeField] private SpriteRenderer parentRenderer;

    private bool isPlayerNearby = false;

    private void Awake()
    {
        if (parentRenderer == null)
            parentRenderer = GetComponentInChildren<SpriteRenderer>();

        if (parentRenderer != null)
            parentRenderer.sprite = normalSprite;
        else
            Debug.LogWarning("PickableItem: No SpriteRenderer found in parent!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;

            if (parentRenderer != null)
                parentRenderer.sprite = highlightedSprite;

            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc != null)
                pc.SetCurrentInteractable(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;

            if (parentRenderer != null)
                parentRenderer.sprite = normalSprite;

            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc != null)
                pc.ClearCurrentInteractable(this);
        }
    }

    public bool IsInteractible() => isPlayerNearby;

}
