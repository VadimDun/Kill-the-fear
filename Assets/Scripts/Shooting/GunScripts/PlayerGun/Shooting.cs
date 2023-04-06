using UnityEngine;

public class Shooting : MonoBehaviour
{
    //Ствол игрока 
    [SerializeField]
    private PlayerGun playerGun;

    [SerializeField]
    private PlayerGunSounds playerSounds;

    private FirePoint firePoints;

    private WarriorMovement wm;

    private PlayerChangeSprites changingSprites;

    private int currentGun = 1;

    private void Start()
    {
        wm = GetComponent<WarriorMovement>();

        wm.SwitchAD(currentGun);
    }

    void Awake()
    {

        firePoints = GetComponent<FirePoint>();

        playerGun.ChangeGun(currentGun);

        firePoints.ChoosePoint(currentGun);

        playerSounds.ChangePlayerSound(currentGun);

        changingSprites = GetComponent<PlayerChangeSprites>();

        changingSprites.changeSprite(currentGun);

    }

    void Update()
    {



        //Выбор ствола
        if (Input.GetKey("1"))      { playerGun.ChangeGun(1); playerSounds.ChangePlayerSound(1); firePoints.ChoosePoint(1); wm.SwitchAD(1); changingSprites.changeSprite(1);  }
        else if (Input.GetKey("2")) { playerGun.ChangeGun(2); playerSounds.ChangePlayerSound(2); firePoints.ChoosePoint(2); wm.SwitchAD(2); changingSprites.changeSprite(2);  }
        else if (Input.GetKey("3")) { playerGun.ChangeGun(3); playerSounds.ChangePlayerSound(3); firePoints.ChoosePoint(3); wm.SwitchAD(3); changingSprites.changeSprite(3);  }
        if (Input.GetButtonDown("Fire1")) { playerGun.PullTheTrigger(); }
        if (Input.GetButtonUp("Fire1")) { playerGun.PullTheTrigger(); }


        if (playerGun.TriggerIsPulled)
        {
            //Выстрел
            if ((playerGun.GetShootMode() == Gun.ShootMode.auto)) { playerGun.PlayerShoot(); }
        }
        
    }
}
