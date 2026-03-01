using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Furnace : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int requiredMetal = 3;
    [SerializeField] private float processingTime = 3f;
    [SerializeField] private GameObject wheelHubPrefab;
    [SerializeField] private Transform outputPoint;

    private List<GameObject> storedMetal = new List<GameObject>();
    private bool isProcessing = false;

    [SerializeField] private SpriteRenderer furnaceRenderer;
    [SerializeField] private SpriteRenderer moldLeftRenderer;
    [SerializeField] private SpriteRenderer moldRightRenderer;

    [SerializeField] private Sprite furnaceOffSprite;
    [SerializeField] private Sprite furnaceOnSprite;

    [SerializeField] private Sprite LmoldOffSprite;
    [SerializeField] private Sprite LmoldOnSprite;

    [SerializeField] private Sprite RmoldOffSprite;
    [SerializeField] private Sprite RmoldOnSprite;

    private bool isOn = false;

    private void Start()
    {
        SetFurnaceState(false);
    }

    public bool TryInsertItem(GameObject itemObject)
    {
        if (isProcessing)
        {
            Debug.Log("Furnace is already processing!");
            return false;
        }

        Item item = itemObject.GetComponent<Item>();

        if (item == null)
            return false;

        if (item.itemType != ItemType.MetalScrap)
        {
            Debug.Log("Only MetalScrap can be inserted!");
            return false;
        }

        StoreMetal(itemObject);
        return true;
    }

    private void StoreMetal(GameObject metal)
    {
        metal.SetActive(false); // hide scrap
        storedMetal.Add(metal);

        Debug.Log($"Metal inserted! Current count: {storedMetal.Count}/{requiredMetal}");

        if (storedMetal.Count >= requiredMetal)
        {
            StartCoroutine(ProcessMetal());
        }
    }

    private IEnumerator ProcessMetal()
    {
        isProcessing = true;

        Debug.Log("Furnace started processing!");
        SetFurnaceState(true);

        float timer = 0f;

        while (timer < processingTime)
        {
            timer += Time.deltaTime;
            Debug.Log($"Processing... {timer:F1}/{processingTime} seconds");
            yield return null;
        }

        Debug.Log("Processing complete! Creating Wheel Hub.");
        SetFurnaceState(false);


        // Destroy stored metal scraps
        foreach (var metal in storedMetal)
        {
            Destroy(metal);
        }
        storedMetal.Clear();

        Instantiate(wheelHubPrefab, outputPoint.position, Quaternion.identity);

        isProcessing = false;
    }

    public void SetFurnaceState(bool on)
    {
        isOn = on;

        if (isOn)
        {
            furnaceRenderer.sprite = furnaceOnSprite;
            moldLeftRenderer.sprite = LmoldOnSprite;
            moldRightRenderer.sprite = RmoldOnSprite;
        }
        else
        {
            furnaceRenderer.sprite = furnaceOffSprite;
            moldLeftRenderer.sprite = LmoldOffSprite;
            moldRightRenderer.sprite = RmoldOffSprite;
        }
    }
}
