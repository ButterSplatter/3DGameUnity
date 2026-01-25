using UnityEngine;

public class Pickup : MonoBehaviour
{
    public int Value = 10;

    public void Collect()
    {
        if (GameManager.I != null && GameManager.I.IsGameOver) return;
        if (UIController.I != null) UIController.I.AddCoins(Value);
        Destroy(gameObject);
    }
}
