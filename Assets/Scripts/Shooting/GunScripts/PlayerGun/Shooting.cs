using UnityEngine;

public class Shooting : MonoBehaviour
{
    //Ствол игрока 
    [SerializeField]
    private PlayerGun playerGun;

    //Дальномер, от положения которого измеряется дальность
    private RangeFinder rangeFinder;

    //Минимальная дистанция для стрельбы (Чтобы не простреливать коллизию)
    private float MinFireDist = 0.35f;

    void Awake()
    {
        playerGun.ChangeGun(1);

        rangeFinder = playerGun.GetComponent<RangeFinder>();
    }

    void Update()
    {
        //Выбор ствола
        if (Input.GetKey("1"))      { playerGun.ChangeGun(1); }
        else if (Input.GetKey("2")) { playerGun.ChangeGun(2); }
        else if (Input.GetKey("3")) { playerGun.ChangeGun(3); }
        else if (Input.GetKey("4")) { playerGun.ChangeGun(4); }
        if (Input.GetButtonDown("Fire1")) { playerGun.PullTheTrigger(); }
        if (Input.GetButtonUp("Fire1")) { playerGun.PullTheTrigger(); }


        if (playerGun.GetIsTriggered())
        {
            //Выстрел
            if ((playerGun.GetShootMode() == Gun.ShootMode.auto) & (rangeFinder.GetDistToTarget > MinFireDist)) { playerGun.PlayerShoot(); }
        }
        
    }
}
