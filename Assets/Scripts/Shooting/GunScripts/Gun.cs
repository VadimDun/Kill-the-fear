using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;


public class Gun : MonoBehaviour
{
    //������
    public enum Guns { pistol, shotgun, assaultRifle, none };

    //������ ����
    public enum ShootMode { auto, semiAuto, off };

    //��������� ������
    protected Guns current_gun = Guns.none;
    //����� �������� 
    protected ShootMode shootMode = ShootMode.off;
    public ShootMode GetShootMode() => shootMode;


    //��������� ������
    protected float delayBetweenShots;
    protected float lastShotTime = Mathf.NegativeInfinity;
    protected int damage;
    protected float bulletSpeed;
    protected float pelletsDeviation = 1;
    protected float pelletsSpread = 0.5f;

    //��������� �����
    private bool isTriggerPulled = false;
    public bool TriggerIsPulled
    {
        get { return isTriggerPulled; }
        set { isTriggerPulled = value; }
    }





    // ������ ������ ��������� (��� ������)
    private AmmunitionGunSlot[] gun_slots = new AmmunitionGunSlot[3];

    private SecondArmSlot[] secondArmSlots = new SecondArmSlot[2];

    private void Awake()
    {
        gun_slots[0] = GameObject.Find("GunSlot(2)").GetComponent<AmmunitionGunSlot>();
        gun_slots[1] = GameObject.Find("GunSlot(3)").GetComponent<AmmunitionGunSlot>();
        gun_slots[2] = GameObject.Find("GunSlot(1)").GetComponent<AmmunitionGunSlot>();
        
        secondArmSlots[0] = GameObject.Find("SecondArmSlot(1)").GetComponent<SecondArmSlot>();


        shootingScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Shooting>();

        playerGun = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerGun>();

        changingSprites = GetComponent<PlayerChangeSprites>();

    }







    private PlayerGun playerGun;

    public void PullTheTrigger()
    {
        isTriggerPulled = !isTriggerPulled;
        if (isTriggerPulled)
        {
            //��� ���������� ��������
            if ((shootMode == ShootMode.semiAuto) ) { Shoot(); }
            if (SecondArmWeapon != null) { playerGun.Kick(); }
        }
    }









    
    private Bullet bullet;
    private GameObject bulletPrefab;
    private Transform firePoint;







    protected virtual void Shoot()
    {
        if (Time.time - lastShotTime < delayBetweenShots) { return; }
        lastShotTime = Time.time;
        bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<Bullet>();
        bullet.damage = damage;
        bullet.bulletSpeed = bulletSpeed;
        
    }





    // ������� ������
    protected bullets_capacity current_capacity;

    // ������
    protected GameObject gunObject;

    // ������ �������� ���
    protected EdgedWeapon SecondArmWeapon;

    private Shooting shootingScript;

    PlayerChangeSprites changingSprites;

    public virtual void ChangeGun(int numberOfGun)
    {
        switch (numberOfGun)
        {
            case 1:
                if (gun_slots[0] != null)
                {
                    if (gun_slots[0].object_in_slot != null)
                    {
                        // ������� ���������� �� ������ � ���� �����
                        root_item_gun gun_data = gun_slots[0].object_in_slot.GetComponent<FloorItem>().getItem as root_item_gun;

                        current_gun = gun_data.GetGunType;
                        delayBetweenShots = gun_data.GetDelayBetweenShots;
                        damage = gun_data.GetDamage;
                        bulletSpeed = gun_data.GetBulletSpeed;
                        shootMode = gun_data.GetShootMode;
                        lastShotTime = gun_data.GetLastShotTime;

                        // ������� ������ � ������� �� ����
                        gunObject = gun_slots[0].object_in_slot;
                        shootingScript.set_root_gun = gunObject.GetComponent<FloorItem>().getItem as root_item_gun;

                        if (current_gun != Guns.shotgun)
                        {
                            current_capacity = gunObject.GetComponent<GunMag>().GetMagInGun.GetComponent<mag>();
                        }
                        else 
                        {
                            current_capacity = gunObject.GetComponent<shotgun_capacity>();
                        }

                    }
                    else
                        ResetGun();
                }
                
                break;
            case 2:
                if (gun_slots[1] != null)
                {
                    if (gun_slots[1].object_in_slot != null)
                    {
                        // ������� ���������� �� ������ � ���� �����
                        root_item_gun gun_data = gun_slots[1].object_in_slot.GetComponent<FloorItem>().getItem as root_item_gun;

                        current_gun = gun_data.GetGunType;
                        delayBetweenShots = gun_data.GetDelayBetweenShots;
                        damage = gun_data.GetDamage;
                        bulletSpeed = gun_data.GetBulletSpeed;
                        shootMode = gun_data.GetShootMode;
                        lastShotTime = gun_data.GetLastShotTime;

                        // ������� ������ � ������� �� ����
                        gunObject = gun_slots[1].object_in_slot;
                        shootingScript.set_root_gun = gunObject.GetComponent<FloorItem>().getItem as root_item_gun;

                        if (current_gun != Guns.shotgun)
                        {
                            current_capacity = gunObject.GetComponent<GunMag>().GetMagInGun.GetComponent<mag>();
                        }
                        else
                        {
                            current_capacity = gunObject.GetComponent<shotgun_capacity>();
                        }
                    }
                    else
                        ResetGun();
                }

                
                break;
            case 3:
                if (gun_slots[2] != null)
                {
                    if (gun_slots[2].object_in_slot != null)
                    {
                        // ������� ���������� �� ������ � ���� �����
                        root_item_gun gun_data = gun_slots[2].object_in_slot.GetComponent<FloorItem>().getItem as root_item_gun;

                        current_gun = gun_data.GetGunType;
                        delayBetweenShots = gun_data.GetDelayBetweenShots;
                        damage = gun_data.GetDamage;
                        bulletSpeed = gun_data.GetBulletSpeed;
                        shootMode = gun_data.GetShootMode;
                        lastShotTime = gun_data.GetLastShotTime;

                        // ������� ������ � ������� �� ����
                        gunObject = gun_slots[2].object_in_slot;
                        shootingScript.set_root_gun = gunObject.GetComponent<FloorItem>().getItem as root_item_gun;

                        if (current_gun != Guns.shotgun)
                        {
                            current_capacity = gunObject.GetComponent<GunMag>().GetMagInGun.GetComponent<mag>();
                        }
                        else
                        {
                            current_capacity = gunObject.GetComponent<shotgun_capacity>();
                        }
                    }
                    else
                        ResetGun();
                }
                break;
            case 0:
                if (secondArmSlots[0] != null)
                {
                    ResetGun();
                    GameObject second_arm_weapon = secondArmSlots[0].object_in_slot;
                    if (second_arm_weapon != null)
                    {
                        Hammer_weapon ew = second_arm_weapon.GetComponent<FloorItem>().getItem as Hammer_weapon;

                        if (ew != null)
                        {
                            SecondArmWeapon = ew;

                            changingSprites.changeSprite(0);
                        }
                    }
                }
                break;
        }
    }








    private void ResetGun() => shootMode = ShootMode.off;


}
