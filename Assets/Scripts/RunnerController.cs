using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class RunnerController : MonoBehaviour
{
    public float ForwardSpeed = 8f;

    public float LaneOffset = 2f;
    public float LaneChangeSpeed = 12f;

    public float JumpHeight = 2.2f;
    public float Gravity = -20f;


    public Transform CameraTransform;

    public float CameraNormalY = 1.6f;
    public float CameraSlideY = 0.9f;
    public float CameraSlideSpeed = 10f;

    public KeyCode SlideKey = KeyCode.LeftControl;
    public float SlideDuration = 0.8f;
    public float SlideSpeedMultiplier = 1.4f;

    public float StandingHeight = 2f;
    public float SlidingHeight = 1f;
    public float StandingCenterY = 1f;
    public float SlidingCenterY = 0.5f;

    CharacterController cc;
    int currentLane = 0;
    float verticalVelocity;

    bool isSliding;
    float slideTimer;

    float jumpBoostTimer;
    float jumpMultiplier = 1f;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        ApplyStanding();

        if (CameraTransform)
        {
            Vector3 p = CameraTransform.localPosition;
            p.y = CameraNormalY;
            CameraTransform.localPosition = p;
        }
    }

    void Update()
    {
        if (GameManager.I != null && GameManager.I.IsGameOver) return;

        TickSlide();
        TickJumpBoost();

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            currentLane = Mathf.Clamp(currentLane - 1, -1, 1);

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            currentLane = Mathf.Clamp(currentLane + 1, -1, 1);

        bool grounded = cc.isGrounded;

        if (grounded && verticalVelocity < 0f)
            verticalVelocity = -2f;

        if (grounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && !isSliding)
        {
            float effectiveJump = JumpHeight * jumpMultiplier;
            verticalVelocity = Mathf.Sqrt(effectiveJump * -2f * Gravity);
        }

        if (grounded && Input.GetKeyDown(SlideKey) && !isSliding)
            StartSlide();

        verticalVelocity += Gravity * Time.deltaTime;

        float targetX = currentLane * LaneOffset;
        float newX = Mathf.Lerp(transform.position.x, targetX, LaneChangeSpeed * Time.deltaTime);

        float speed = ForwardSpeed;
        if (isSliding) speed *= SlideSpeedMultiplier;

        Vector3 motion = Vector3.forward * speed;
        motion.y = verticalVelocity;
        motion *= Time.deltaTime;
        motion.x = newX - transform.position.x;

        cc.Move(motion);
        UpdateCameraHeight();

    }
    void UpdateCameraHeight()
    {
        if (!CameraTransform) return;

        float targetY = isSliding ? CameraSlideY : CameraNormalY;

        Vector3 pos = CameraTransform.localPosition;
        pos.y = Mathf.Lerp(pos.y, targetY, CameraSlideSpeed * Time.deltaTime);
        CameraTransform.localPosition = pos;
    }


    void TickSlide()
    {
        if (!isSliding) return;

        slideTimer -= Time.deltaTime;
        if (slideTimer <= 0f)
        {
            if (CanStandUp())
            {
                StopSlide();
            }
            else
            {
                slideTimer = 0.1f;
            }
        }
    }

    void StartSlide()
    {
        isSliding = true;
        slideTimer = SlideDuration;
        ApplySliding();
    }

    void StopSlide()
    {
        isSliding = false;
        ApplyStanding();
    }

    bool CanStandUp()
    {
        float radius = cc.radius * 0.95f;
        float extra = 0.05f;
        Vector3 origin = transform.position + Vector3.up * (SlidingHeight + extra);
        float checkDistance = (StandingHeight - SlidingHeight) + 0.2f;
        return !Physics.SphereCast(origin, radius, Vector3.up, out _, checkDistance);
    }

    void ApplyStanding()
    {
        cc.height = StandingHeight;
        cc.center = new Vector3(0f, StandingCenterY, 0f);
    }

    void ApplySliding()
    {
        cc.height = SlidingHeight;
        cc.center = new Vector3(0f, SlidingCenterY, 0f);
    }

    void TickJumpBoost()
    {
        if (jumpBoostTimer <= 0f) return;

        jumpBoostTimer -= Time.deltaTime;
        if (jumpBoostTimer <= 0f)
        {
            jumpBoostTimer = 0f;
            jumpMultiplier = 1f;
        }
    }

    public void ActivateJumpBoost(float duration, float multiplier)
    {
        jumpBoostTimer = Mathf.Max(jumpBoostTimer, duration);
        jumpMultiplier = Mathf.Max(jumpMultiplier, multiplier);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Obstacle"))
        {
            if (GameManager.I != null) GameManager.I.SetGameOver();
            return;
        }

        if (hit.collider.CompareTag("LowObstacle"))
        {
            if (!isSliding)
            {
                if (GameManager.I != null) GameManager.I.SetGameOver();
            }
            return;
        }
        if (hit.collider.CompareTag("Enemy"))
        {
            GameManager.I.SetGameOver();
            return;
        }

    }
    public bool IsSliding()
    {
        return isSliding;
    }


    void OnTriggerEnter(Collider other)
    {
        if (GameManager.I != null && GameManager.I.IsGameOver) return;

        if (other.CompareTag("LowObstacle"))
        {
            if (!isSliding)
            {
                GameManager.I.SetGameOver();
            }
            return;
        }

        if (other.CompareTag("Pickup"))
        {
            Pickup p = other.GetComponent<Pickup>();
            if (p != null) p.Collect();
            return;
        }

        if (other.CompareTag("Powerup"))
        {
            JumpBoostPickup jb = other.GetComponent<JumpBoostPickup>();
            if (jb != null) jb.Collect(this);
            return;
        }
    }

}
