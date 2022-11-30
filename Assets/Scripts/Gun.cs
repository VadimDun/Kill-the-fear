using UnityEngine;
using UnityEngine.U2D;


public class Gun : MonoBehaviour
{
    enum Guns { pistol, shotgun, assaultRifle, sniper };
    enum ShootMode { auto, semiAuto, off };

    Guns current_gun = Guns.shotgun;
    ShootMode shootMode = ShootMode.semiAuto;
    Bullet bullet;

    float delayBetweenShots;
    float lastShotTime = Mathf.NegativeInfinity;
    bool isTriggerPulled = false;

    public Transform firePoint;
    public GameObject bulletPrefab;
    public float pelletsDeviation = 3;
    public float pelletsSpread = 5;

    void Awake()
    {
        bullet = bulletPrefab.GetComponent<Bullet>();
    }

    public void PullTheTrigger()
    {
        isTriggerPulled = !isTriggerPulled;
        if (isTriggerPulled)
        {
            if (shootMode == ShootMode.semiAuto) { Shoot(); }
        }
    }

    public void Shoot()
    {
        if (Time.time - lastShotTime < delayBetweenShots) { return; }
        lastShotTime = Time.time;
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        switch (current_gun)
        {
            case Guns.shotgun:
                Vector2 direction = transform.right;
                float normalAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                for (int i = -4; i < 4; ++i)
                {
                    if (i != 0)
                    {
                        float angle = normalAngle + pelletsSpread * i + Random.Range(-pelletsDeviation, pelletsDeviation);
                        Instantiate(bulletPrefab, firePoint.position, Quaternion.AngleAxis(angle, Vector3.forward));
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
                    bullet.damage = 34;
                    bullet.bulletSpeed = 20f;
                    shootMode = ShootMode.semiAuto;
                    lastShotTime = Mathf.NegativeInfinity;
                }
                break;
            case 2:
                if (current_gun != Guns.shotgun)
                {
                    current_gun = Guns.shotgun;
                    delayBetweenShots = 1.0f;
                    bullet.damage = 11;
                    bullet.bulletSpeed = 20f;
                    shootMode = ShootMode.semiAuto;
                    lastShotTime = Mathf.NegativeInfinity;
                }
                break;
            case 3:
                if (current_gun != Guns.assaultRifle)
                {
                    current_gun = Guns.assaultRifle;
                    delayBetweenShots = 0.1f;
                    bullet.damage = 18;
                    bullet.bulletSpeed = 20f;
                    shootMode = ShootMode.auto;
                    lastShotTime = Mathf.NegativeInfinity;
                }
                break;
            case 4:
                if (current_gun != Guns.sniper)
                {
                    current_gun = Guns.sniper;
                    delayBetweenShots = 1.5f;
                    bullet.damage = 63;
                    bullet.bulletSpeed = 40f;
                    shootMode = ShootMode.semiAuto;
                    lastShotTime = Mathf.NegativeInfinity;
                }
                break;
        }    
    }

    void Update()
    {
        if (isTriggerPulled)
        {
            if(shootMode == ShootMode.auto) { Shoot(); }
        }
    }
}
