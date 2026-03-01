using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private Sprite front;
    [SerializeField] private Sprite back;
    [SerializeField] private Sprite left;
    [SerializeField] private Sprite right;

    private SpriteRenderer spriteRenderer;
    private PlayerController playerController;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerController = GetComponentInParent<PlayerController>();
    }

    private void Update()
    {
        Vector2 move = playerController.GetMoveInput();

        if (move.magnitude < 0.1f)
            return;

        if (Mathf.Abs(move.x) > Mathf.Abs(move.y))
        {
            if (move.x > 0)
                spriteRenderer.sprite = right;
            else
                spriteRenderer.sprite = left;
        }
        else
        {
            if (move.y > 0)
                spriteRenderer.sprite = back;
            else
                spriteRenderer.sprite = front;
        }
    }
}
