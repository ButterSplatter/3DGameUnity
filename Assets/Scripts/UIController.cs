using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public Transform Player;
    public TextMeshProUGUI ScoreText;
    public GameObject GameOverPanel;

    float startZ;

    void Start()
    {
        if (Player) startZ = Player.position.z;
        if (GameOverPanel) GameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (!Player || !ScoreText) return;

        int score = Mathf.FloorToInt(Mathf.Max(0f, Player.position.z - startZ));
        ScoreText.text = $"Score: {score}";

        if (GameManager.I != null && GameManager.I.IsGameOver)
        {
            if (GameOverPanel) GameOverPanel.SetActive(true);
        }
    }
}
