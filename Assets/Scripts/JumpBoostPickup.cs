using UnityEngine;

public class JumpBoostPickup : MonoBehaviour
{
    public float Duration = 10f;
    public float JumpMultiplier = 2.5f;

    public void Collect(RunnerController controller)
    {
        if (controller != null)
            controller.ActivateJumpBoost(Duration, JumpMultiplier);
        Destroy(gameObject);
    }
}
