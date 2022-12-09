using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int health = 100;
    public int playerHealth { get { return health; } set { health = value; } }

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