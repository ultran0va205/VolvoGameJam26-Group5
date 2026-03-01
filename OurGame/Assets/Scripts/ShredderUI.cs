using UnityEngine;
using UnityEngine.UI;

public class ShredderUI : MonoBehaviour
{
    [SerializeField] private Shredder shredder;
    [SerializeField] private Image progressBarFill;
    [SerializeField] private GameObject progressBarParent;

    private void Update()
    {
        progressBarParent.SetActive(shredder.IsShredding());
        progressBarFill.fillAmount = shredder.IsShredding() ? shredder.GetProgress() : 0f;
    }
}
