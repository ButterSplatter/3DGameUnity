using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform Target;

    public Vector3 NormalOffset = new Vector3(0, 6, -8);
    public Vector3 SlideOffset = new Vector3(0, 4.2f, -7.5f);

    public float Smooth = 10f;

    RunnerController runner;

    void Start()
    {
        if (Target)
            runner = Target.GetComponent<RunnerController>();
    }

    void LateUpdate()
    {
        if (!Target) return;

        Vector3 desiredOffset = NormalOffset;

        if (runner != null && runner.IsSliding())
            desiredOffset = SlideOffset;

        Vector3 desiredPosition = Target.position + desiredOffset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Smooth * Time.deltaTime);

        transform.LookAt(Target.position + Vector3.up * 1.2f);
    }
}
