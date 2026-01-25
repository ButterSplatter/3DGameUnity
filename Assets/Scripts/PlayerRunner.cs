using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    public float MoveSpeed = 6f;
    public float JumpHeight = 1.6f;
    public float Gravity = -20f;

    CharacterController cc;
    float verticalVelocity;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (GameManager.I != null && GameManager.I.IsGameOver) return;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        move *= MoveSpeed;

        if (cc.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;

        if (cc.isGrounded && Input.GetKeyDown(KeyCode.Space))
            verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

        verticalVelocity += Gravity * Time.deltaTime;

        Vector3 velocity = Vector3.up * verticalVelocity;

        cc.Move((move + velocity) * Time.deltaTime);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Obstacle"))
        {
            if (GameManager.I != null) GameManager.I.SetGameOver();
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
