using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Gun;

public class AmmunitionManager : MonoBehaviour
{

    private List<AmmunitionGunSlot> am_gun_slots = new List<AmmunitionGunSlot>();

    private RectTransform am_UI;



    private void Start()
    {

        am_UI = GameObject.Find("AmmunitionUI").GetComponent<RectTransform>();

        // Получаем все слоты для огнестрельного оружия
        for (int i = 0; i < am_UI.childCount; i++)
        {
            if (am_UI.GetChild(i).GetComponent<AmmunitionGunSlot>() != null)
            {
                am_gun_slots.Add(am_UI.GetChild(i).GetComponent<AmmunitionGunSlot>());
            }
        }

        GameObject.Find("Inventory").SetActive(false);

    }


    private bool SuccessGunAddition = false;

    private bool SuccessAdd = false;
    public void PutItem(Item item, GameObject gameObj)
    {
        

        foreach (AmmunitionGunSlot slot in am_gun_slots)
        {
            // Проверка на пустотку и на оружие 
            if (item.itemType == ItemType.gun)
                PutGunItem(item, slot, out SuccessGunAddition);

            
            if (SuccessGunAddition)
            {
                // Возвращаю дефолтное состояние
                SuccessGunAddition = false;

                // Уничтожаю оружие на земле, если добавление прошло успешно 
                Destroy(gameObj);

                Debug.Log("Объект уничтожен");

                return;
            }
            

            // Если элемент добавился - завершаем перебор слотов, ставим дефолтное значение
            if (SuccessAdd)
            {
                SuccessAdd = false;

                return;

            }
        }
    }

    private void PutGunItem(Item item, AmmunitionGunSlot slot, out bool SuccessGunAddition)
    {
        SuccessGunAddition = false;

        if (slot.SlotIsEmpty)
        {
            if (item.itemType == ItemType.gun)
            {

                // Теперь слот не пустой
                slot.SlotIsEmpty = false;

                // Получаю картинку в слоте, которая будет отображать оружие
                Transform gunImageTransform = slot.transform.GetChild(1);



                // Устанавливаю объект подобранного оружия
                GameObject child = Instantiate(item.ScriptableGameObject, slot.transform);




                // Помещаю переданное оружие в слот хранения
                slot.GunInSlot = child.GetComponent<FloorItem>().getItem;

                // Устанавливаю этот объект как child объект картинки, которая его отображает
                child.transform.SetParent(gunImageTransform);

                // Устанавливаю картинку в слот инвентаря
                gunImageTransform.GetComponent<Image>().sprite = slot.GunInSlot.GetInventoryIcon;

                gunImageTransform.GetComponent<Image>().enabled = true;

                // Добавление предмета прошло успешно
                SuccessGunAddition = true;

            }
            else
            {
                // Это не оружие, добавление прошло не успешно
                SuccessGunAddition = false;
            }
        }
        else
        {
            // Слот занят. Добавление прошло не успешно
            SuccessGunAddition = false;
        }
    }


}
