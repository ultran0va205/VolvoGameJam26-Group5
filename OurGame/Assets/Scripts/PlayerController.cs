using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float upDownSpeedMultiplier = 1.3f; // for camera compensation

    [SerializeField] private bool lockYPosition = true;
    [SerializeField] private float fixedYPosition = 0f;

    private CharacterController controller;
    private Vector2 moveInput;

    [SerializeField] private Transform holdPoint;
    private PickableItem currentInteractable;
    private GameObject heldItem;

    private int heldItemOriginalSortingOrder;

    private Furnace currentFurnace;

    private Bin currentBin = null;

    [SerializeField] private Animator animator;
    private static readonly int BlendX = Animator.StringToHash("BlendX");
    private static readonly int BlendY = Animator.StringToHash("BlendY");
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        //animator = GetComponent<Animator>();
    }

    void LateUpdate()
    {
        if (lockYPosition)
        {
            Vector3 pos = transform.position;
            pos.y = fixedYPosition;
            transform.position = pos;
        }
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
            Rigidbody rb = heldItem.GetComponent<Rigidbody>();
            Collider[] hits = Physics.OverlapSphere(transform.position, 1f);

            //First: Try furnace
            if (currentFurnace != null)
            {
                bool accepted = currentFurnace.TryInsertItem(heldItem);

                if (accepted)
                {
                    heldItem = null;
                    return;
                }
            }

            if (currentBin != null)
            {
                bool accepted = currentBin.TryDisposeItem(heldItem);
                if (accepted)
                {
                    heldItem = null;
                    return; // done
                }
            }

            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;

            //Otherwise drop
            Vector3 dropPosition = transform.position + transform.forward * 0.6f;
            dropPosition.y = 0.8f;

            heldItem.transform.SetParent(null);
            heldItem.transform.position = dropPosition;
            heldItem.GetComponentInChildren<SpriteRenderer>().sortingOrder = 1;
            heldItem = null;
            return;
        }

        //PICK UP nearby cube
        if (heldItem == null && currentInteractable != null && currentInteractable.IsInteractible())
        {
            heldItem = currentInteractable.GetComponentInParent<Rigidbody>()?.gameObject;

            // Stop physics
            Rigidbody rb = heldItem.GetComponent<Rigidbody>();
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;

            // Snap to hold point
            heldItem.transform.SetParent(holdPoint);
            heldItem.transform.localPosition = Vector3.zero;
            heldItem.transform.localRotation = Quaternion.identity;

            heldItemOriginalSortingOrder = heldItem.GetComponentInChildren<SpriteRenderer>().sortingOrder;
            heldItem.GetComponentInChildren<SpriteRenderer>().sortingOrder = 2;

            currentInteractable = null;
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

        if (animator != null)
        {
            animator.SetFloat(BlendX, Mathf.Lerp(animator.GetFloat(BlendX), moveInput.x, Time.deltaTime * 10f));
            animator.SetFloat(BlendY, Mathf.Lerp(animator.GetFloat(BlendY), moveInput.y, Time.deltaTime * 10f));
        }

        animator.SetBool(IsWalking, moveInput.magnitude > 0.1f);

        if (heldItem != null)
        {
            heldItem.transform.position = transform.position + Vector3.up * 1f + transform.forward * 0.5f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Furnace furnace = other.GetComponentInParent<Furnace>();
        if (furnace != null)
        {
            currentFurnace = furnace;
        }

        Bin bin = other.GetComponentInChildren<Bin>();
        if (bin != null)
        {
            currentBin = bin;
            //Debug.Log($"Entered {bin.name}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Furnace furnace = other.GetComponentInParent<Furnace>();
        if (furnace != null && furnace == currentFurnace)
        {
            currentFurnace = null;
        }

        Bin bin = other.GetComponentInChildren<Bin>();
        if (bin != null && currentBin == bin)
        {
            currentBin = null;
            //Debug.Log($"Exited {bin.name}");
        }
    }

   

}
