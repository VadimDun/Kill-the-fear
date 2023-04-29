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




    private root_item_gun root_gun;

    public root_item_gun set_root_gun { set { root_gun = value; } }

    private int spriteIndex => root_gun.GetSpriteIndex;

    private int soundIndex => root_gun.GetSoundIndex;

    private int AD_Index => root_gun.get_AD_index;

    private int firePointIndex => root_gun.GetFirePointIndex;



    private int StartGun = 1;










    private void Start()
    {
        wm = GetComponent<WarriorMovement>();

        wm.SwitchAD(StartGun);
    }

    void Awake()
    {

        firePoints = GetComponent<FirePoint>();

        
        playerGun.ChangeGun(StartGun);

        firePoints.ChoosePoint(StartGun);

        playerSounds.ChangePlayerSound(StartGun);
        

        changingSprites = GetComponent<PlayerChangeSprites>();

        changingSprites.changeSprite(StartGun);

    }

    void Update()
    {



        //Выбор ствола
        if (Input.GetKey("1"))      { playerGun.ChangeGun(1); playerSounds.ChangePlayerSound(soundIndex); firePoints.ChoosePoint(firePointIndex); wm.SwitchAD(AD_Index); changingSprites.changeSprite(spriteIndex);  }
        else if (Input.GetKey("2")) { playerGun.ChangeGun(2); playerSounds.ChangePlayerSound(soundIndex); firePoints.ChoosePoint(firePointIndex); wm.SwitchAD(AD_Index); changingSprites.changeSprite(spriteIndex);  }
        else if (Input.GetKey("3")) { playerGun.ChangeGun(3); playerSounds.ChangePlayerSound(soundIndex); firePoints.ChoosePoint(firePointIndex); wm.SwitchAD(AD_Index); changingSprites.changeSprite(spriteIndex);  }
        else if (Input.GetKey("0")) { playerGun.ChangeGun(0); }
        if (Input.GetButtonDown("Fire1")) { playerGun.PullTheTrigger(); }
        if (Input.GetButtonUp("Fire1")) { playerGun.PullTheTrigger(); }


        if (playerGun.TriggerIsPulled)
        {
            //Выстрел
            if ((playerGun.GetShootMode() == Gun.ShootMode.auto)) { playerGun.PlayerShoot(); }
        }
        
    }
}
