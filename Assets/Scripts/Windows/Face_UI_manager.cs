using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Face_UI_manager : MonoBehaviour
{


    // Массив оружейных слотов инвентаря 
    private AmmunitionGunSlot[] gun_slots = new AmmunitionGunSlot[3];

    // Массив картинок в GunBar
    private GameObject[] gunBarPics = new GameObject[3];


    private void Awake()
    {
        // Получаю оружейные слоты
        gun_slots[0] = GameObject.Find("GunSlot(2)").GetComponent<AmmunitionGunSlot>();
        gun_slots[1] = GameObject.Find("GunSlot(3)").GetComponent<AmmunitionGunSlot>();
        gun_slots[2] = GameObject.Find("GunSlot(1)").GetComponent<AmmunitionGunSlot>();



        // Получаю картинки в GunBar
        GameObject gunBar = GameObject.Find("GunBar");

        for (int i = 0; i < 3; i++)
        {
            gunBarPics[i] = gunBar.transform.GetChild(i).gameObject;
        }

    }

    public void UpdatePic()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject slotBar = gunBarPics[i];

            Image slotBar_image = slotBar.transform.GetChild(1).gameObject.GetComponent<Image>();

            GameObject gun = gun_slots[i].object_in_slot;


            if (gun == null)
            {
                slotBar_image.sprite = null;
                slotBar_image.enabled = false;
            }
            else
            { 
                slotBar_image.sprite = gun.GetComponent<FloorItem>().getItem.GetFloorIcon;
                slotBar_image.enabled = true;
            }

        }
    }


}
