using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Mag;
using static UnityEditor.Experimental.GraphView.Port;


[CreateAssetMenu(fileName = "rifle_bullets", menuName = "ScriptableObject/Items/Bullets/rifle_bullets", order = 1)]
public class rifle_bullets : BulletStack
{

    private const int rifle_bullets_stack_capacity = 90;

    private void OnEnable()
    {
        stack_capacity = rifle_bullets_stack_capacity;

        bulletsType = BulletsType.rifleBullets;

        itemType = ItemType.bullet;
    }


}
