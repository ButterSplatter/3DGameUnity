using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int Health = 3;
    public float MoveSpeed = 2.5f;

    void Update()
    {
        if (GameManager.I != null && GameManager.I.IsGameOver) return;

        transform.position += Vector3.back * MoveSpeed * Time.deltaTime;
    }

    public void TakeHit(int dmg)
    {
        Health -= dmg;
        if (Health <= 0) Destroy(gameObject);
        Debug.Log("ENEMY HIT");

    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Player"))
        {
            GameManager.I.SetGameOver();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.I.SetGameOver();
        }
    }
}
