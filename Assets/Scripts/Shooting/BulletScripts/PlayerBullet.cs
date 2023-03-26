using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    //Стрелок
    private GameObject player;
    //Дальномер от позиции стрелка
    private RangeFinder rangeFinder;
    
    //Столкновение со стеной
    private RaycastHit2D WallHit;
    //Столкновение с существом
    private RaycastHit2D EnemyHit;

    //Время смерти пули (игрока)
    private float deathTime;


    //Получаю RB2D нужной пули (игрока)
    [SerializeField]
    private Rigidbody2D PlayerBulletRB;

    public Rigidbody2D GetPlayerBulletRB => PlayerBulletRB;



    //Получаю Collider нужной пули (игрока)
    [SerializeField]
    private BoxCollider2D PlayerBulletCollider;

    public BoxCollider2D GetPlayerBulletCollider => PlayerBulletCollider;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rangeFinder = player.GetComponentInChildren<RangeFinder>();

        BulletSpeed(PlayerBulletRB);
    }

    void FixedUpdate()
    {

        WallHit = hitTheWall(PlayerBulletRB, PlayerBulletCollider);
        EnemyHit = hitTheEnemy(PlayerBulletRB, PlayerBulletCollider);

        //Если столкновение со стеной
        if (WallHit)
        {
            Debug.Log("Wall hit");
            deathTime = DeathTime(WallHit);
            if (rangeFinder.GetDistToTarget < 0.55f)
            {
                Destroy(gameObject, deathTime);
                
            }
            else { Destroy(gameObject, deathTime); }
            

        }
        else if (EnemyHit)
        {
            deathTime = DeathTime(EnemyHit);

            Enemy enemy = EnemyHit.collider.GetComponent<Enemy>();

            Debug.Log("Enemy hit");

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Destroy(gameObject, deathTime);
            }

        }
    }

    /*
    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log(IsKillable);
        if (!IsKillable) return;

        Enemy enemy = collider.GetComponent<Enemy>();
        if ( (enemy != null) && (IsKillable) ) { enemy.TakeDamage(damage); Destroy(gameObject, deathTime); }
    }

    */

}
