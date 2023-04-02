using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManagerScript gameManager;

    private int health = 100;
    public int playerHealth { get { return health; } set { health = value; } }

    private bool isDead;
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0 && !isDead) 
        {
            isDead = true;
            gameManager.gameOver();
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}