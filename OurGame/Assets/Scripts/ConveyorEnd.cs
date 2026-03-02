using UnityEngine;

public class ConveyorEnd : MonoBehaviour
{
    private static int itemsLost = 0;
    [SerializeField] private int maxItemsLost;

    [SerializeField] private Camera mainCamera;
    private float shakeDuration = 0f;
    private Vector3 originalCamPos;

    private void Start()
    {
        itemsLost = 0;
        if (mainCamera == null)
            mainCamera = Camera.main;
        originalCamPos = mainCamera.transform.localPosition;
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return;

        if (shakeDuration > 0)
        {
            float intensity = GetShakeIntensity();
            mainCamera.transform.localPosition = originalCamPos + Random.insideUnitSphere * intensity;
            shakeDuration -= Time.deltaTime;
        }
        else
        {
            mainCamera.transform.localPosition = originalCamPos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            AudioMgr.Instance.PlaySewage();
            Destroy(other.gameObject);
            itemsLost++;
            FirstSewageTip.Instance.TriggerTip();

            Debug.Log($"Items lost: {itemsLost}/{maxItemsLost}");

            TriggerShake();

            if (itemsLost >= maxItemsLost)
            {
                GameOver();
            }
        }
    }

    private float GetShakeIntensity()
    {
        // Scales from 0.02 at 1 item to 0.15 at 7 items
        return Mathf.Lerp(0.02f, 0.15f, (float)itemsLost / maxItemsLost);
    }

    private void TriggerShake()
    {
        shakeDuration = 0.3f;
        originalCamPos = mainCamera.transform.localPosition; // re-anchor in case cam moved
    }

    private void GameOver()
    {
        Debug.Log("GAME OVER - 7 items lost!");
        GameMgr.instance.TriggerGameOver();
        // TODO: trigger game over UI here
        // e.g. GameManager.Instance.TriggerGameOver();
    }
}
