using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerRunner : MonoBehaviour
{
    public float LaneOffset = 2f;
    public float LaneChangeSpeed = 12f;

    public float ForwardSpeed = 8f;

    public float JumpHeight = 2.2f;
    public float Gravity = -20f;

    CharacterController cc;
    int currentLane = 0;
    float verticalVelocity;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (GameManager.I != null && GameManager.I.IsGameOver) return;

        HandleInput();
        Move();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            currentLane = Mathf.Clamp(currentLane - 1, -1, 1);

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            currentLane = Mathf.Clamp(currentLane + 1, -1, 1);

        bool grounded = cc.isGrounded;

        if (grounded && verticalVelocity < 0f)
            verticalVelocity = -2f;

        if (grounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
            verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

        verticalVelocity += Gravity * Time.deltaTime;
    }

    void Move()
    {
        float targetX = currentLane * LaneOffset;
        float newX = Mathf.Lerp(transform.position.x, targetX, LaneChangeSpeed * Time.deltaTime);

        Vector3 motion = Vector3.forward * ForwardSpeed;
        motion.y = verticalVelocity;
        motion *= Time.deltaTime;
        motion.x = newX - transform.position.x;

        cc.Move(motion);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Obstacle"))
        {
            if (GameManager.I != null)
                GameManager.I.SetGameOver();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (GameManager.I != null && GameManager.I.IsGameOver) return;

        if (other.CompareTag("Pickup"))
        {
            Pickup p = other.GetComponent<Pickup>();
            if (p != null) p.Collect();
        }
    }

}
