using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDirection : MonoBehaviour
{
    //Игрок
    private GameObject player;
    //Ствол игрока 
    private PlayerGun gun;
    //Откуда будет ведётся стрельба
    private Transform firePoint;

    //Направление стрельбы
    private Vector2 fireDirection;

    //Направление стрельбы, значение которого, можно присвоить в других скриптах
    public Vector2 GetFireDir => fireDirection;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); 
        gun = player.GetComponent<PlayerGun>();
        firePoint = gun.GetComponent<Transform>();
    }

    void Update()
    {
        //Получаю направление стрельбы (Красную ось, или же ось Х)
        fireDirection = firePoint.right;
    }
}
