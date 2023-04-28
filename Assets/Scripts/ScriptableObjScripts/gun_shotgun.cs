using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shotgun", menuName = "ScriptableObject/Items/Guns/Shotgun", order = 3)]
public class gun_shotgun : root_item_gun
{

    private int Capasity = 8;

    public int GetCapasity => Capasity;




    private void OnEnable()
    {

        gunType = Gun.Guns.shotgun;

        shootMode = Gun.ShootMode.semiAuto;

        delayBetweenShots = 1.0f;

        damage = 11;

        bulletSpeed = 10f;

        itemType = ItemType.gun;

        spriteIndex = 3;

        soundIndex = 3;

        firePointIndex = 3;

        AD_index = 3;

    }
}
