using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class FireDirection : MonoBehaviour
{
    //Для получения актуального угла разницы AngleDifference 
    [SerializeField]
    WarriorMovement wm;
    
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
        
        //Угол разницы между стволом и спрайтом 
        float RotateAngle = wm.angleDifference;
        
        //Поворот
        Quaternion q = Quaternion.AngleAxis(-RotateAngle, Vector3.forward);

        //Направление красной оси
        Vector3 StartDirection = firePoint.right;
        

        //Это уже повернутый куда надо fireDir
        fireDirection = q * StartDirection;
    }
}
