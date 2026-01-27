using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed = 40f;
    public float LifeTime = 2f;
    public int Damage = 1;

    void Start()
    {
        //transform.rotation.Set(0,90,0,0);
        Destroy(gameObject, LifeTime);
    }

    void Update()
    {
        transform.position += transform.forward * Speed * Time.deltaTime;
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
