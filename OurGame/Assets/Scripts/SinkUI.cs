using UnityEngine;
using UnityEngine.UI;

public class SinkUI : MonoBehaviour
{
    [SerializeField] private Sink sink;
    [SerializeField] private Image progressBarFill;
    [SerializeField] private GameObject progressBarParent;

    private void Update()
    {
        progressBarParent.SetActive(sink.IsWashing());
        progressBarFill.fillAmount = sink.IsWashing() ? sink.GetProgress() : 0f;
    }
}
