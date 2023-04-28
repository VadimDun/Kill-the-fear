using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "pistol_bullets", menuName = "ScriptableObject/Items/Bullets/pistol_bullets", order = 2)]
public class pistol_bullets : BulletStack
{

    private const int pistol_bullets_stack_capacity = 120;

    private void OnEnable()
    {
        stack_capacity = pistol_bullets_stack_capacity;

        bulletsType = BulletsType.pistolBullets;

        itemType = ItemType.bullet;
    }


}
