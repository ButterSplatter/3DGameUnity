using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject BulletPrefab;
    public Transform FirePoint;
    public float FireCooldown = 0.15f;

    float cd;

    void Update()
    {
        if (GameManager.I != null && GameManager.I.IsGameOver) return;

        cd -= Time.deltaTime;

        if (Input.GetMouseButton(0) && cd <= 0f)
        {
            cd = FireCooldown;
            Fire();
        }
    }

    void Fire()
    {
        if (BulletPrefab == null || FirePoint == null) return;

        Instantiate(BulletPrefab, FirePoint.position, FirePoint.rotation);
    }
}
