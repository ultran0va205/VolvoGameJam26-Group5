using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Onboarding : MonoBehaviour
{
    [SerializeField] private GameObject onboardingPanel;
    [SerializeField] private Image displayImage;
    [SerializeField] private Sprite[] pages;
    [SerializeField] private float pageTime = 10f;

    private int currentPage = 0;
    private float timer = 0f;
    private bool onboardingDone = false;

    private void Start()
    {
        onboardingPanel.SetActive(true);
        displayImage.sprite = pages[0];
        timer = -5f;
        Time.timeScale = 0f;
    }

    private void Update()
    {
        if (onboardingDone) return;

        timer += Time.unscaledDeltaTime; // unscaled because timeScale is 0

        bool advance = timer >= pageTime;

        if (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)
            advance = true;

        if (advance)
            NextPage();
    }

    private void NextPage()
    {
        currentPage++;
        timer = 0f;

        if (currentPage >= pages.Length)
        {
            FinishOnboarding();
            return;
        }

        displayImage.sprite = pages[currentPage];
    }

    private void FinishOnboarding()
    {
        onboardingDone = true;
        onboardingPanel.SetActive(false);
        Time.timeScale = 1f; // unfreeze game
        AudioMgr.Instance.PlayAmbient();
        AudioMgr.Instance.PlayConveyor();
    }
}