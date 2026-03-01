using UnityEngine;

public class Shredder : MonoBehaviour
{
    [SerializeField] private float shredTime = 3f;
    [SerializeField] private Transform outputPoint;
    [SerializeField] private GameObject shreddedPlastic;

    private bool isShredding = false;
    private float timer = 0f;
    private PlayerController currentPlayer = null;

    private void Update()
    {
        if (!isShredding) return;

        timer += Time.deltaTime;

        if (timer >= shredTime)
            CompleteShredding();
    }

    public void StartShredding(PlayerController player)
    {
        GameObject heldItem = player.GetHeldItem();
        if (heldItem == null) { Debug.Log("No held item!"); return; }

        Item item = heldItem.GetComponent<Item>();
        if (item == null || item.itemType != ItemType.CleanBottle)
        {
            Debug.Log("Shredder only accepts clean bottles!");
            return;
        }

        Debug.Log($"Started shredding {heldItem.name}");
        currentPlayer = player;
        isShredding = true;
        timer = 0f;
        heldItem.SetActive(false); // hide immediately
    }

    public void CancelShredding()
    {
        if (!isShredding) return;
        Debug.Log("Shredding cancelled!");
        isShredding = false;
        timer = 0f;

        GameObject heldItem = currentPlayer.GetHeldItem();
        if (heldItem != null)
            heldItem.SetActive(true); // bring it back

        currentPlayer = null;
    }

    private void CompleteShredding()
    {
        Debug.Log("Shredding complete!");
        isShredding = false;

        GameObject heldItem = currentPlayer.GetHeldItem();
        Item item = heldItem.GetComponent<Item>();

        GameObject shredded = Instantiate(shreddedPlastic, outputPoint.position, Quaternion.identity);

        Destroy(heldItem);
        //currentPlayer.ForceSetHeldItem(shredded);
        currentPlayer = null;
    }

    public float GetProgress() => timer / shredTime;
    public bool IsShredding() => isShredding;
}
