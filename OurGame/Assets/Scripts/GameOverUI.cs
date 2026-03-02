using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI Instance;

    [SerializeField] private GameObject gameOverPanel; // drag your Panel here

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        gameOverPanel.SetActive(false); // hidden at start
    }

    public void Show()
    {
        gameOverPanel.SetActive(true);
    }
}
