using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
public class TutorialUI : MonoBehaviour
{
    [SerializeField] private Image displayImage;
    [SerializeField] private Sprite[] pages;
    [SerializeField] private GameObject startObj;
    [SerializeField] private TextMeshProUGUI page1Text;
    [SerializeField] private TextMeshProUGUI page2Text;
    private int currentPage = 0;

    private void Start()
    {
        displayImage.sprite = pages[0];
        startObj.SetActive(false);
        UpdatePageIndicator();
    }

    private void Update()
    {
        if (Gamepad.current == null) return;

        bool goRight = Gamepad.current.rightShoulder.wasPressedThisFrame
            || Gamepad.current.dpad.right.wasPressedThisFrame
            || Gamepad.current.leftStick.right.wasPressedThisFrame
            || Gamepad.current.rightStick.right.wasPressedThisFrame;

        bool goLeft = Gamepad.current.leftShoulder.wasPressedThisFrame
            || Gamepad.current.dpad.left.wasPressedThisFrame
            || Gamepad.current.leftStick.left.wasPressedThisFrame
            || Gamepad.current.rightStick.left.wasPressedThisFrame;

        if (goRight) ChangePage(1);
        if (goLeft) ChangePage(-1);

        if (Gamepad.current.buttonEast.wasPressedThisFrame)
            SceneManager.LoadScene("MainMenu");

        if (Gamepad.current.buttonSouth.wasPressedThisFrame)
            SceneManager.LoadScene("Game");
    }

    private void ChangePage(int dir)
    {
        currentPage = Mathf.Clamp(currentPage + dir, 0, pages.Length - 1);
        displayImage.sprite = pages[currentPage];

        if (currentPage >= 1)
            startObj.SetActive(true);

        UpdatePageIndicator();
    }

    private void UpdatePageIndicator()
    {
        if (currentPage == 0)
        {
            page1Text.fontSize = 75;
            page2Text.fontSize = 50;
        }
        else
        {
            page1Text.fontSize = 50;
            page2Text.fontSize = 75;
        }
    }
}
