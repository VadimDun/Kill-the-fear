using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerRange : MonoBehaviour
{

    private int hammer_damage;

    private void Start()
    {
        Hammer_weapon hw = GameObject.Find("Hammer").GetComponent<FloorItem>().getItem as Hammer_weapon;
        hammer_damage = hw.GetDamage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage(hammer_damage);
        }

    }
}
