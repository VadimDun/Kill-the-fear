using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 10;
    public float LifeTime = 15f;
    float StartTime;
    public float bulletSpeed = 20f;

    void Awake()
    {
        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = new Vector2(bulletSpeed * rb2d.transform.right.x, bulletSpeed * rb2d.transform.right.y);
        StartTime = Time.time;
    }
    void Update()
    {
        if (Time.time - StartTime > LifeTime) { Destroy(gameObject); }
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        Enemy enemy = collider.GetComponent<Enemy>();
        if (enemy != null) { enemy.TakeDamage(damage); }
        Destroy(gameObject);
    }
}
