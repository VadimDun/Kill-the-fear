using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "rifleBullet", menuName = "ScriptableObject/Items/Bullets/rifleBullet")]
public class rifle_bullet_item : bullet_item
{

    private const float rifle_bullet_speed = 10.0f;

    private void Start()
    {
        type_of_bullet = bullet_type.rifleBullet;

        bullet_speed = rifle_bullet_speed;

        itemType = ItemType.bullet;

    }


}
