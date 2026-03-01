using UnityEngine;
using System.Collections;

public class Sink : MonoBehaviour
{
    [SerializeField] private float washTime = 3f;

    private bool isWashing = false;
    private float timer = 0f;
    private PlayerController currentPlayer = null;

    private void Update()
    {
        if (!isWashing) return;

        timer += Time.deltaTime;
        Debug.Log($"Washing... {timer:F1}/{washTime}");

        if (timer >= washTime)
        {
            CompleteWashing();
        }
    }

    public void StartWashing(PlayerController player)
    {
        GameObject heldItem = player.GetHeldItem();

        if (heldItem == null) return;

        Item item = heldItem.GetComponent<Item>();
        if (item == null || item.cleanVersion == null) return;
        if (item == null || item.cleanVersion == null) { Debug.Log("Item has no clean version!"); return; }

        Debug.Log($"Started washing {heldItem.name}");
        currentPlayer = player;
        isWashing = true;
        timer = 0f;
        heldItem.SetActive(false); // hide immediately
    }

    public void CancelWashing()
    {
        Debug.Log("Washing cancelled!");
        isWashing = false;
        timer = 0f;
        currentPlayer = null;
        GameObject heldItem = currentPlayer.GetHeldItem();
        if (heldItem != null)
            heldItem.SetActive(true); // bring it back
    }

    private void CompleteWashing()
    {
        Debug.Log("Washing complete!");
        isWashing = false;

        GameObject heldItem = currentPlayer.GetHeldItem();
        Item item = heldItem.GetComponent<Item>();
        Debug.Log($"Item: {heldItem.name} | Clean version: {item.cleanVersion}");

        // Spawn clean version directly into player's hand
        GameObject cleanItem = Instantiate(item.cleanVersion, heldItem.transform.position, Quaternion.identity);
        currentPlayer.ForceSetHeldItem(cleanItem);

        Destroy(heldItem);
        Debug.Log($"Destroying: {heldItem.name}");
        currentPlayer = null;
    }

    public float GetProgress() => timer / washTime;
    public bool IsWashing() => isWashing;
}
