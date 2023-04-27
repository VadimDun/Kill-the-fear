using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rifle", menuName = "ScriptableObject/Items/Guns/Rifle", order = 2)]
public class gun_rifle : Item
{
    private Gun gunClass;

    private Gun.Guns gunType;

    private Gun.ShootMode shootMode;

    private float delayBetweenShots;

    private int damage;

    private float bulletSpeed;

    private float lastShotTime = Mathf.NegativeInfinity;

    [SerializeField] private Sprite unloaded_rifle_icon;

    [SerializeField] private Sprite unloaded_rifle_inventory_icon;




    public Gun.Guns GetGunType => gunType;

    public Gun.ShootMode GetShootMode => shootMode;

    public float GetDelayBetweenShots => delayBetweenShots;

    public int GetDamage => damage;

    public float GetBulletSpeed => bulletSpeed;

    public float GetLastShotTime => lastShotTime;

    public Sprite GetUnloadedDroppedIcon => unloaded_rifle_icon;

    public Sprite GetUnloadedInventoryIcon => unloaded_rifle_inventory_icon;

    




    private void Start()
    {
        gunClass = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerGun>();

        gunType = Gun.Guns.assaultRifle;

        shootMode = Gun.ShootMode.auto;

        delayBetweenShots = 0.1f;

        damage = 18;

        bulletSpeed = 10f;

        itemType = ItemType.gun;

    }

}
