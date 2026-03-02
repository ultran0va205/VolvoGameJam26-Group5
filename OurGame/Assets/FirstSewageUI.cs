using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class FirstSewageTip : MonoBehaviour
{
    public static FirstSewageTip Instance;

    [SerializeField] private GameObject tipPanel;
    [SerializeField] private float autoHideTime = 5f;

    private bool hasShown = false;
    private float timer = 0f;
    private bool isShowing = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        tipPanel.SetActive(false);
    }

    private void Update()
    {
        if (!isShowing) return;

        timer += Time.unscaledDeltaTime;

        bool dismiss = timer >= autoHideTime;
        if (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)
            dismiss = true;

        if (dismiss) Hide();
    }

    public void TriggerTip()
    {
        if (hasShown) return;
        hasShown = true;
        tipPanel.SetActive(true);
        Time.timeScale = 0f;
        isShowing = true;
        timer = 0f;
    }

    private void Hide()
    {
        isShowing = false;
        tipPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}