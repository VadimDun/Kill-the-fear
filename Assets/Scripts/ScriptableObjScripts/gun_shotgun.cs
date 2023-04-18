using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shotgun", menuName = "ScriptableObject/Items/Shotgun", order = 3)]
public class gun_shotgun : Item
{
    private Gun gunClass;

    private Gun.Guns gunType;

    private Gun.ShootMode shootMode;

    private float delayBetweenShots;

    private int damage;

    private float bulletSpeed;

    private float lastShotTime = Mathf.NegativeInfinity;




    public Gun.Guns GetGunType => gunType;

    public Gun.ShootMode GetShootMode => shootMode;

    public float GetDelayBetweenShots => delayBetweenShots;

    public int GetDamage => damage;

    public float GetBulletSpeed => bulletSpeed;

    public float GetLastShotTime => lastShotTime;




    private void Start()
    {
        gunClass = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerGun>();

        gunType = Gun.Guns.shotgun;

        shootMode = Gun.ShootMode.semiAuto;

        delayBetweenShots = 1.0f;

        damage = 11;

        bulletSpeed = 10f;

    }
}
