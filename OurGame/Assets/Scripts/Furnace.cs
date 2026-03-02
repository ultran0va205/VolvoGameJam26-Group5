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

    [SerializeField] private GameObject pelletsPrefab;
    //[SerializeField] private Transform plasticOutputPoint;
    private bool isProcessingPlastic = false;
    private float plasticTimer = 0f;
    private List<GameObject> storedPlastic = new List<GameObject>();


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
    private float timer = 0f;

    private void Start()
    {
        SetFurnaceState(false);
    }

    public bool TryInsertItem(GameObject itemObject)
    {
        Item item = itemObject.GetComponent<Item>();
        if (item == null) return false;

        if (item.itemType == ItemType.MetalScrap)
        {
            if (isProcessing) { Debug.Log("Furnace is already processing metal!"); return false; }
            StoreMetal(itemObject);
            return true;
        }
        else if (item.itemType == ItemType.ShreddedPlastic)
        {
            if (isProcessingPlastic || storedPlastic.Count > 0) { Debug.Log("Furnace is already processing plastic!"); return false; }
            StorePlastic(itemObject);
            return true;
        }

        Debug.Log("Item not accepted by furnace!");
        return false;
    }

    private void StorePlastic(GameObject plastic)
    {
        plastic.SetActive(false);
        storedPlastic.Add(plastic);
        Debug.Log("Plastic inserted! Starting processing...");
        StartCoroutine(ProcessPlastic());
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

        timer = 0f;

        while (timer < processingTime)
        {
            timer += Time.deltaTime;
            //Debug.Log($"Processing... {timer:F1}/{processingTime} seconds");
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

    private IEnumerator ProcessPlastic()
    {
        isProcessingPlastic = true;
        Debug.Log("Furnace started processing plastic!");
        SetFurnaceState(true);
        plasticTimer = 0f;
        while (plasticTimer < processingTime)
        {
            plasticTimer += Time.deltaTime;
            yield return null;
        }
        Debug.Log("Processing complete! Creating Pellets.");
        SetFurnaceState(false);
        foreach (var plastic in storedPlastic)
            Destroy(plastic);
        storedPlastic.Clear();
        Instantiate(pelletsPrefab, outputPoint.position, Quaternion.identity);
        isProcessingPlastic = false;
    }

    public int GetMetalCount() => storedMetal.Count;

    public float GetProgress()
    {
        if (!isProcessing) return 0f;
        return timer / processingTime;
    }

    public float GetPlasticProgress() => isProcessingPlastic ? plasticTimer / processingTime : 0f;
    public bool IsProcessingPlastic() => isProcessingPlastic;

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
