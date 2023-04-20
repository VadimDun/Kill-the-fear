using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;
using static Gun;

public class AmmunitionGunSlot : MonoBehaviour
{
    private bool IsEmpty = true;

    public bool SlotIsEmpty
    { 
        get { return IsEmpty; }
        set { IsEmpty = value; }
    }

    private GameObject child;

    private Item gun;

    public Item GunInSlot;


    

    /*
    public void AddItem(Item item, out bool SuccessAddition)
    {
        if (IsEmpty)
        {
            if (item.itemType == ItemType.gun)
            {
                // Помещаю переданное оружие в слот хранения
                gun = item;

                // Добавление предмета прошло успешно
                SuccessAddition = true;

                // Теперь слот не пустой
                IsEmpty = false;

                // Получаю картинку в слоте, которая будет отображать оружие
                Transform gunImageTransform = transform.GetChild(1);


                // Устанавливаю объект подобранного оружия
                child = Instantiate(gun.ScriptableGameObject, transform);

                // Устанавливаю этот объект как child объект картинки, которая его отображает
                child.transform.SetParent(gunImageTransform);

                // Устанавливаю картинку в слот инвентаря
                SetImage(gunImageTransform);

            }
            else
            {
                // Это не оружие, добавление прошло не успешно
                SuccessAddition = false;
            }
        }
        else
        {
            // Слот занят. Добавление прошло не успешно
            SuccessAddition = false;
        }
    }


    public void SetImage(Transform imageObject)
    {
        imageObject.GetComponent<Image>().sprite = gun.GetInventoryIcon;

        imageObject.GetComponent<Image>().enabled = true;
    }

    */
}
