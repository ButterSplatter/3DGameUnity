using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController I { get; private set; }

    public Transform Player;
    public TextMeshProUGUI ScoreText;
    public GameObject GameOverPanel;

    float startZ;
    int coinScore;

    void Awake()
    {
        I = this;
    }

    void Start()
    {
        if (Player) startZ = Player.position.z;
        if (GameOverPanel) GameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (!Player || !ScoreText) return;

        int distScore = Mathf.FloorToInt(Mathf.Max(0f, Player.position.z - startZ));
        int total = distScore + coinScore;

        ScoreText.text = $"Score: {total}\nCoins: {coinScore}";

        if (GameManager.I != null && GameManager.I.IsGameOver)
        {
            if (GameOverPanel) GameOverPanel.SetActive(true);
        }
    }

    public void AddCoins(int amount)
    {
        coinScore += amount;
    }
}
