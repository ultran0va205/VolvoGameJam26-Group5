using UnityEngine;


public enum BinType
{
    Metal,
    Plastic
}

public class Bin : MonoBehaviour
{
    [Header("Bin Settings")]
    [SerializeField] private BinType binType; // Assign in inspector
    [SerializeField] private ItemType acceptedType;


    public bool TryDisposeItem(GameObject itemObject)
    {
        if (itemObject == null) return false;

        Item item = itemObject.GetComponent<Item>();
        if (item == null) return false;

        if ((binType == BinType.Metal && item.itemType == ItemType.WheelHub))
        {

            Destroy(itemObject); // remove from scene

            Debug.Log($"Bin {binType} checking item {itemObject.name} of type {item.itemType}");
            return true;
        }

        return false;
    }
}
