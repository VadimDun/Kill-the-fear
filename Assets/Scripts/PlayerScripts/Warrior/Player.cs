using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private GameManagerScript gameManager;

    private int health = 100;
    public int playerHealth { get { return health; } set { health = value; } }

    private bool isDead;

    public bool playerIsDead
    { 
        get { return isDead; }
        set { isDead = value; }
    }

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0 && !isDead) 
        {
            isDead = true;
            gameManager.gameOver();
        }
    }

}