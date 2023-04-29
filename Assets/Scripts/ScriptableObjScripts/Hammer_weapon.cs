using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hammer", menuName = "ScriptableObject/Items/Weapons/Hammer", order = 1)]
public class Hammer_weapon : EdgedWeapon
{


    private void OnEnable()
    {
        damage = 35;

        coolDown = 0.2f;

        itemType = ItemType.edged_weapon;
    }

}
