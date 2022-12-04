using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Gun gun;


    //Поля для луча, определяющего дистанцию до цели

    private RaycastHit2D HitLookDir;
    private GameObject Warrior;
    private Transform WarriorPos;
    private WarriorMovement PlayerMovement;
    private CircleCollider2D PlayerCollider;

    //Минимальная дистанция для стрельбы
    private float MinFireDist = 0.35f;


    //По дефолту = 1. Без DV первого выстрела не будет 
    private float DistToTarget = 1f;

    public float GetDistToTarget => DistToTarget;

    void Awake()
    {
        gun.ChangeGun(1);
        Warrior = GameObject.FindWithTag("Player");
        WarriorPos = Warrior.GetComponent<Transform>();
        PlayerMovement = Warrior.GetComponent<WarriorMovement>();
        PlayerCollider = Warrior.GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        if (Input.GetKey("1"))      { gun.ChangeGun(1); }
        else if (Input.GetKey("2")) { gun.ChangeGun(2); }
        else if (Input.GetKey("3")) { gun.ChangeGun(3); }
        else if (Input.GetKey("4")) { gun.ChangeGun(4); }
        if (Input.GetButtonDown("Fire1")) { gun.PullTheTrigger(); }
        if (Input.GetButtonUp("Fire1")) { gun.PullTheTrigger(); }

        //Луч до цели который нужен для определения дистанции до цели
        HitLookDir = Physics2D.Raycast(WarriorPos.position, new Vector2(PlayerMovement.WarriorLookDir.x, PlayerMovement.WarriorLookDir.y), PlayerCollider.radius * 100, LayerMask.GetMask("Player", "Creatures"));


        if (gun.GetIsTriggered())
        {
            //Дистанция до цели 
            DistToTarget = Vector2.Distance(new Vector2(WarriorPos.position.x, WarriorPos.position.y), HitLookDir.point);
            //Выстрел
            if ((gun.GetShootMode() == Gun.ShootMode.auto) & (DistToTarget > MinFireDist)) { gun.Shoot(); }
        }
        
    }
}
