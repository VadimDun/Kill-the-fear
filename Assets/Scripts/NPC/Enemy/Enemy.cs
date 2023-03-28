using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int health = 100;
    public int enemyHealth { get { return health; } set { health = value; } }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) { Die(); }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
