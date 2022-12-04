using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;


public class Gun : MonoBehaviour
{

    enum Guns { pistol, shotgun, assaultRifle, sniper, none };
    public enum ShootMode { auto, semiAuto, off };

    Guns current_gun = Guns.none;
    ShootMode shootMode = ShootMode.off;
    public ShootMode GetShootMode() => shootMode;
    Bullet bullet;
    WarriorMovement correction;

    float delayBetweenShots;
    float lastShotTime = Mathf.NegativeInfinity;
    int damage;
    float bulletSpeed;
    bool isTriggerPulled = false;
    public bool GetIsTriggered() => isTriggerPulled;

    public Transform firePoint;
    public GameObject bulletPrefab;
    public float pelletsDeviation = 3;
    public float pelletsSpread = 5;

    //Минимальная дистанция для стрельбы
    private float MinFireDist = 0.35f;


    //По дефолту = 1. Без DV первого выстрела не будет 
    private float DistToTarget = 1f;
    public float GetDistToTarget => DistToTarget;

    void Start()
    {
        correction = GetComponent<WarriorMovement>();    
    }

    public void PullTheTrigger()
    {
        isTriggerPulled = !isTriggerPulled;
        if (isTriggerPulled)
        {
            //Выстрел
            if ((shootMode == ShootMode.semiAuto) & (DistToTarget > MinFireDist)) { Shoot(); }
        }
    }

    public void Shoot()
    {
        if (Time.time - lastShotTime < delayBetweenShots) { return; }
        lastShotTime = Time.time;
        bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<Bullet>();
        bullet.damage = damage;
        bullet.bulletSpeed = bulletSpeed;
        switch (current_gun)
        {
            case Guns.shotgun:
                Vector2 direction = transform.right;
                float normalAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + (360f - correction.angleDifference);
                for (int i = -4; i < 4; ++i)
                {
                    if (i != 0)
                    {
                        float angle = normalAngle + pelletsSpread * i + Random.Range(-pelletsDeviation, pelletsDeviation);
                        bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.AngleAxis(angle, Vector3.forward)).GetComponent<Bullet>();
                        bullet.damage = damage;
                        bullet.bulletSpeed = bulletSpeed;
                    }
                }
                break;
        }
    }

    public void ChangeGun(int numberOfGun)
    {
        switch (numberOfGun)
        {
            case 1:
                if (current_gun != Guns.pistol)
                {
                    current_gun = Guns.pistol;
                    delayBetweenShots = 0.3f;
                    damage = 34;
                    bulletSpeed = 10f;
                    shootMode = ShootMode.semiAuto;
                    lastShotTime = Mathf.NegativeInfinity;
                }
                break;
            case 2:
                if (current_gun != Guns.shotgun)
                {
                    current_gun = Guns.shotgun;
                    delayBetweenShots = 1.0f;
                    damage = 11;
                    bulletSpeed = 10f;
                    shootMode = ShootMode.semiAuto;
                    lastShotTime = Mathf.NegativeInfinity;
                }
                break;
            case 3:
                if (current_gun != Guns.assaultRifle)
                {
                    current_gun = Guns.assaultRifle;
                    delayBetweenShots = 0.1f;
                    damage = 18;
                    bulletSpeed = 10f;
                    shootMode = ShootMode.auto;
                    lastShotTime = Mathf.NegativeInfinity;
                }
                break;
            case 4:
                if (current_gun != Guns.sniper)
                {
                    current_gun = Guns.sniper;
                    delayBetweenShots = 1.5f;
                    damage = 63;
                    bulletSpeed = 10f;
                    shootMode = ShootMode.semiAuto;
                    lastShotTime = Mathf.NegativeInfinity;
                }
                break;
        }    
    }
}
