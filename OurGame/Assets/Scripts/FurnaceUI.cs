using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FurnaceUI : MonoBehaviour
{
    [SerializeField] private Furnace furnace;
    [SerializeField] private TextMeshProUGUI metalCountText;
    [SerializeField] private Image progressBarFill;

    private void Update()
    {
        metalCountText.text = $"{furnace.GetMetalCount()}/3";
        progressBarFill.fillAmount = furnace.GetProgress();

        Debug.Log($"FillAmount: {progressBarFill.fillAmount} | Progress: {furnace.GetProgress()}");
    }
}
