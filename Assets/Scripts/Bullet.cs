using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed = 40f;
    public float LifeTime = 2f;
    public int Damage = 1;

    public Transform Visual;
    public Vector3 VisualEulerOffset;

    Vector3 dir;

    void Start()
    {
        if (Visual != null)
            Visual.localRotation = Quaternion.Euler(VisualEulerOffset);

        dir = transform.forward.normalized;

        Destroy(gameObject, LifeTime);
    }

    void Update()
    {
        transform.position += dir * Speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        Enemy e = other.GetComponentInParent<Enemy>();
        if (e != null)
        {
            e.TakeHit(Damage);
            Destroy(gameObject);
            return;
        }

        if (!other.isTrigger)
            Destroy(gameObject);
    }
}
