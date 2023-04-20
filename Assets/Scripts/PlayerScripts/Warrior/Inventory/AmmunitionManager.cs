using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Gun;
using static UnityEditor.Progress;

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
                PutGunItem(item, gameObj, slot, out SuccessGunAddition);

            
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

    private void PutGunItem(Item item, GameObject gameObj, AmmunitionGunSlot slot, out bool SuccessGunAddition)
    {
        SuccessGunAddition = false;

        if (slot.SlotIsEmpty)
        {
            if (item.itemType == ItemType.gun)
            {

                // Получаю картинку в слоте, которая будет отображать оружие
                Transform gunImageTransform = slot.transform.GetChild(1);


                // Устанавливаю объект подобранного оружия
                GameObject child = Instantiate(gameObj, slot.transform);


                // Помещаю переданное оружие в слот хранения
                slot.GunInSlot = child.GetComponent<FloorItem>().getItem;

                // Перемещаю объект оружия в слот хранения
                slot.GunObj = child;

                // Устанавливаю этот объект как child объект картинки, которая его отображает
                child.transform.SetParent(gunImageTransform);

                // Устанавливаю картинку в слот инвентаря
                gunImageTransform.GetComponent<Image>().sprite = slot.GunInSlot.GetInventoryIcon;

                gunImageTransform.GetComponent<Image>().enabled = true;

                // Теперь слот не пустой
                slot.SlotIsEmpty = false;

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


    //                         что передаём,     куда передаём,         выходной результат    
    public void PutWeaponToSlot(GameObject gun, AmmunitionGunSlot slot, out bool GunIsAdded)
    { 
        GunIsAdded = false;

        if (gun.GetComponent<FloorItem>().getItem.itemType != ItemType.gun)
        {
            GunIsAdded = false;
        }
        else 
        {
            if (slot.SlotIsEmpty)
            {


                // Получаю картинку в слоте, которая будет отображать оружие
                Transform gunImageTransform = slot.transform.GetChild(1);

                // Устанавливаю объект передаваемого в слот оружия
                GameObject child = Instantiate(gun, slot.transform);

                // Помещаю переданное оружие в слот хранения
                slot.GunInSlot = child.GetComponent<FloorItem>().getItem;

                // Перемещаю объект оружия в слот хранения
                slot.GunObj = child;

                // Устанавливаю этот объект как child объект картинки, которая его отображает
                child.transform.SetParent(gunImageTransform);

                // Устанавливаю картинку в слот инвентаря
                gunImageTransform.GetComponent<Image>().sprite = slot.GunInSlot.GetInventoryIcon;

                gunImageTransform.GetComponent<Image>().enabled = true;

                // Теперь слот не пустой
                slot.SlotIsEmpty = false;





                
                // Получаю Transform картинки оружия, которое передавал в слот
                Transform currentGunImage = gun.transform.parent;

                //Получаю слот передаваемого оружия
                AmmunitionGunSlot currentSlot = currentGunImage.parent.GetComponent<AmmunitionGunSlot>();

                // Устанавливаю картинку на исходное положение
                currentGunImage.position = currentSlot.SlotDefaultPosition;
                
                // Убираю картинку у спрайта 
                currentGunImage.GetComponent<Image>().sprite = null;

                // Делаю её неактивной
                currentGunImage.GetComponent<Image>().enabled = false;

                //Уничтожаю передаваемый объект
                Destroy(gun);

                // Теперь слот пустой
                currentSlot.SlotIsEmpty = true;

                // Слот не содержит в себе какой-либо объект 
                currentSlot.GunObj = null;

                // Слот не содержит ScriptableObject
                currentSlot.GunInSlot = null;



                // Добавление предмета прошло успешно
                GunIsAdded = true;

            }
            else
            {
                // Получаю картинку в слоте, которая будет отображать оружие
                Transform gunImageTransform = slot.transform.GetChild(1);

                // Получаю Transform картинки оружия, которое передавал в слот
                Transform currentGunImage = gun.transform.parent;

                // Сохряняю дочерний объект картинки
                GameObject InternalObject = Instantiate(gunImageTransform.GetChild(0).gameObject, currentGunImage);

                // Уничтожаю дочерний объект картинки
                Destroy(gunImageTransform.GetChild(0).gameObject);




                // Устанавливаю объект передаваемого в слот оружия
                GameObject child = Instantiate(gun, slot.transform);

                // Уничтожаю исходный объект, клон которого я передал
                Destroy(gun);

                // Помещаю переданное оружие в слот хранения
                slot.GunInSlot = child.GetComponent<FloorItem>().getItem;

                // Перемещаю объект оружия в слот хранения
                slot.GunObj = child;

                // Устанавливаю этот объект как child объект картинки, которая его отображает
                child.transform.SetParent(gunImageTransform);

                // Устанавливаю картинку в слот инвентаря
                gunImageTransform.GetComponent<Image>().sprite = slot.GunInSlot.GetInventoryIcon;

                gunImageTransform.GetComponent<Image>().enabled = true;

                // Теперь слот не пустой
                slot.SlotIsEmpty = false;







                // Получаю ScriptableObject объекта
                Item InternalItem = InternalObject.GetComponent<FloorItem>().getItem;

                //Получаю слот передаваемого оружия
                AmmunitionGunSlot currentSlot = currentGunImage.parent.GetComponent<AmmunitionGunSlot>();



                // Устанавливаю картинку на исходное положение
                currentGunImage.position = currentSlot.SlotDefaultPosition;

                // Ставлю картинку оружия, которое находится в слоте
                currentGunImage.GetComponent<Image>().sprite = InternalItem.GetInventoryIcon;

                // Устанавливаю ScriptableObject в слот
                currentSlot.GunInSlot = InternalItem;

                // Устанавливаю объект оружия в слот
                currentSlot.GunObj = InternalObject;

                // Слот не пустой
                currentSlot.SlotIsEmpty = false;



                // Замена предмета прошла успешно
                GunIsAdded = true;

            }
        }


    }


}
