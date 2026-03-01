using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float upDownSpeedMultiplier = 1.3f; // for camera compensation

    private CharacterController controller;
    private Vector2 moveInput;

    private PickableItem currentInteractable;
    private GameObject heldItem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    public void SetCurrentInteractable(PickableItem item)
    {
        currentInteractable = item;
    }

    public void ClearCurrentInteractable(PickableItem item)
    {
        if (currentInteractable == item)
            currentInteractable = null;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        //Debug.Log($"Move Input: {moveInput}");
    }

    public Vector2 GetMoveInput()
    {
        return moveInput;
    }

    public void OnPickButton (InputAction.CallbackContext context) //X button
    {
        if (!context.performed) return;

        // DROP held item (if any)
        if (heldItem != null)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, 1f);

            //First: Try furnace
            foreach (var hit in hits)
            {
                Furnace furnace = hit.GetComponent<Furnace>();
                if (furnace != null)
                {
                    bool accepted = furnace.TryInsertItem(heldItem);

                    if (accepted)
                    {
                        heldItem = null;
                        return;
                    }
                }
            }

            //Then: Try normal placeable surfaces
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Placeable Surface"))
                {
                    heldItem.transform.position = hit.transform.position + Vector3.up * 1f;
                    heldItem.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                    heldItem.transform.SetParent(null);
                    heldItem = null;
                    return;
                }
            }

            //Otherwise drop in front
            heldItem.transform.position = transform.position + transform.forward * 0.5f;
            heldItem.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            heldItem.transform.SetParent(null);
            heldItem = null;

            return;
        }

        //PICK UP nearby cube
        if (heldItem == null && currentInteractable != null && currentInteractable.IsInteractible())
        {
            heldItem = currentInteractable.transform.parent != null
                ? currentInteractable.transform.parent.gameObject
                : currentInteractable.gameObject;

            heldItem.transform.SetParent(null); // unparent from belt
            heldItem.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;

            currentInteractable = null; // clear since picked up
        }
    }

    public void OnSpamButton(InputAction.CallbackContext context) //Square Button
    {
        if (context.performed)
        {
            Debug.Log("Spam Button - Square");
        }
    }

    private void Update()
    {
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        //Vector3 move = camForward * moveInput.y + camRight * moveInput.x;
        //controller.Move(move * speed * Time.deltaTime);

        Vector3 move = camForward * moveInput.y * upDownSpeedMultiplier + camRight * moveInput.x;
        controller.Move(move * speed * Time.deltaTime);

        if (heldItem != null)
        {
            heldItem.transform.position = transform.position + Vector3.up * 1f + transform.forward * 0.5f;
        }
    }

   
}
