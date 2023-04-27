using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Gun;


public class InventoryManager : MonoBehaviour
{

    private List<AmmunitionGunSlot> am_gun_slots = new List<AmmunitionGunSlot>();

    public List<ItemSlot> itemSlots = new List<ItemSlot>();

    private RectTransform am_UI;

    private RectTransform item_UI;






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

        item_UI = GameObject.Find("ItemsUI").GetComponent<RectTransform>();


        // Получаем все дефолтные слоты
        for (int i = 0; i < item_UI.childCount; i++)
        {
            if (item_UI.GetChild(i).GetComponent<ItemSlot>() != null)
            {
                itemSlots.Add(item_UI.GetChild(i).GetComponent<ItemSlot>());
            }
        }
        

        // Делаю неактивным 
        GameObject.Find("Inventory").SetActive(false);

    }









    public void MainInventoryManager(Item item, GameObject itemObj)
    {


        bool SuccessGunAddition = false;



        /*
         * Если передаваемый объект является оружием 
        */


        if (item.itemType == ItemType.gun)
        {
            foreach (AmmunitionGunSlot slot in am_gun_slots)
            {
                
                GrabGunItem(item, itemObj, slot, out SuccessGunAddition);


                if (SuccessGunAddition)
                {
                    // Возвращаю дефолтное состояние
                    SuccessGunAddition = false;

                    return;
                }

            }
        }



        /*
         * Если передаваемый предмет не является оружием или броней 
        */


        if (item.itemType != ItemType.gun && item.itemType != ItemType.secondaty_arms && item.itemType != ItemType.armor)
        {
            foreach (ItemSlot slot in itemSlots)
            {
                bool success = false;

                GrabDefaultItem(item, itemObj, slot, out success);


                if (success)
                {
                    // Возвращаю дефолтное состояние
                    success = false;

                    return;
                }
            }
        }


    }








    private void GrabItem(Item item, GameObject TransmittedObject, Slot slot, out bool success)
    {
        
        success = false;


        if (item != null && TransmittedObject != null && slot != null)
        {
            
            // Получаю картинку предмета
            Transform item_image_transform = slot.transform.GetChild(1);




            /*
             *  Устанавливаю подобранный предмет
            */


            // Устанавливаю объект как child object картинки предмета
            TransmittedObject.transform.SetParent(item_image_transform);

            // Убираю спрайт объекта на земле 
            TransmittedObject.GetComponent<SpriteRenderer>().sprite = null;




            /*
             *  Устанавливаю изображение предмета, которого передал в слот
            */

            item_image_transform.GetComponent<Image>().sprite = item.GetInventoryIcon;

            item_image_transform.GetComponent<Image>().enabled = true;




            /*
             * Настраиваю скрипт слота
            */


            // Передаю в слот предмет и представляющий его объект 
            slot.SetItem(item, TransmittedObject);




            /*
             * Успешное завершение передачи оружия в слот 
             */


            // Добавление предмета прошло успешно
            success = true;
        }
        else 
        {
            success = false;        
        }

    }










    private void GrabDefaultItem(Item item, GameObject TransmittedObject, ItemSlot slot, out bool success)
    { 
        success = false;

        if (slot.SlotIsEmpty)
        {
            // Кладу подобранный предмет в инвентарь
            GrabItem(item, TransmittedObject, slot, out success);
        }
        else
        {
            // Слот был занят другим предметом, добавление прошло неуспешно
            success = false;
        }
    }









    private void GrabGunItem(Item item, GameObject TransmittedObject, AmmunitionGunSlot slot, out bool SuccessGunAddition)
    {
        SuccessGunAddition = false;

        if (slot.SlotIsEmpty)
        {
            if (item.itemType == ItemType.gun)
            {
                // Кладу подобранный предмет в инвентарь
                GrabItem(item, TransmittedObject, slot, out SuccessGunAddition);

            }
            else
            {
                // Данный предмет не является оружием, поэтому его нельзя добавить в этот слот
                SuccessGunAddition = false;
            }
        }
        else
        {
            // Слот занят. Добавление прошло неуспешно
            SuccessGunAddition = false;
        }
    }










    private void SetItemToEmptySlot(Item item, GameObject TransmittedObject, Slot slot, out bool ItemAdded)
    {
        ItemAdded = false;

        if (item != null && TransmittedObject != null && slot != null)
        {


            // Получаю Transform картинки, в которую хочу передать предмет
            Transform InputImageTransform = slot.transform.GetChild(1);

            // Получаю Transform картинки, текущей картинки передаваемого предмета
            Transform currentImageTransform = TransmittedObject.transform.parent;




            /*
             * Получаю и устанавливаю объект, который отображает передаваемый предмет 
            */


            // Устанавливаю передаваемый объект на картинку слота, в который мы передаём (как child объект)
            TransmittedObject.transform.SetParent(InputImageTransform);




            /*
             * Устанавливаю изображение в слот, в который передали и включаю её
            */


            InputImageTransform.GetComponent<Image>().sprite = item.GetInventoryIcon;

            InputImageTransform.GetComponent<Image>().enabled = true;




            /*
             * Настраиваю скрипт слота, в который передали 
            */


            // Передаю в слот предмет и представляющий его объект
            slot.SetItem(item, TransmittedObject);




            /*
             * Настраиваю скрипт слота, из которого передавали предмет 
            */


            // Очищаю слот 
            currentImageTransform.parent.gameObject.GetComponent<Slot>().ClearClot();




            /*
             * Настраиваю картинку слота, из которой передавали предмет  
            */


            // Удаляю изображение предмета, который передавали
            currentImageTransform.GetComponent<Image>().sprite = null;

            // Возвращаю картинку дефолтное на место
            currentImageTransform.position = currentImageTransform.parent.gameObject.GetComponent<Slot>().SlotDefaultPosition;

            // Делаю ее неактивной
            currentImageTransform.GetComponent<Image>().enabled = false;




            /*
             * Успешная передача предмета
            */


            // Добавление предмета прошло успешно
            ItemAdded = true;


        }
        else 
        {
            // Добавлени предмета прошло не успешно
            ItemAdded = false;
        }

        
    }









    private void SetItemWithReplace(Item item, GameObject TransmittedObject, Slot slot, out bool ItemAdded)
    {
        

        /*
         * Получаю слот и картинку, в которых хочу передать предмет 
        */


        // Получаю Transform картинки, в которую хочу передать предмет
        Transform InputImageTransform = slot.transform.GetChild(1);

        // Получаю слот входной картинки 
        Slot inputSlot = InputImageTransform.parent.GetComponent<Slot>();




        /*
         * Получаю текущие слот и картинку  
        */


        // Получаю Transform картинки, текущей картинки передаваемого предмета
        Transform currentImageTransform = TransmittedObject.transform.parent;

        // Получаю слот исходной картинки
        Slot currentSlot = currentImageTransform.parent.GetComponent<Slot>();




        /*
         * Меняю местами картинки (в иерархии)
        */


        InputImageTransform.transform.SetParent(currentSlot.transform);

        currentImageTransform.transform.SetParent(inputSlot.transform);




        /*
         * Меняю местами картинки (визуально)
        */


        // Возвращаю исходную картинку на место
        currentImageTransform.position = currentImageTransform.parent.gameObject.GetComponent<Slot>().SlotDefaultPosition;

        InputImageTransform.position = InputImageTransform.parent.gameObject.GetComponent<Slot>().SlotDefaultPosition;




        /*
         * Настраиваю скрипты слотов, в который передали 
        */


        // Слот 1


        // Получаю объект, который сейчас находится в CurrentSlot
        GameObject gm_current = currentImageTransform.GetChild(0).gameObject;

        // Получаю предмет, который сейчас находится в CurrentSlot
        Item item_current = gm_current.GetComponent<FloorItem>().getItem;

        // Устанавливаю их в новый слот
        currentSlot.SetItem(item_current, gm_current);


        // Слот 2


        // Получаю объект, который сейчас находится в InputSlot
        GameObject gm_input = InputImageTransform.GetChild(0).gameObject;

        // Получаю предмет, который сейчас находится в CurrentSlot
        Item item_input = gm_input.GetComponent<FloorItem>().getItem;

        // Устанавливаю их в новый слот 
        inputSlot.SetItem(item_input, gm_input);




        /*
         * Успешная передача предмета
        */


        // Замена предмета прошла успешно
        ItemAdded = true;


    }










    public void PutItemToSlot(Item item, GameObject TransmittedObject, ItemSlot slot, out bool ItemAdded)
    {
        ItemAdded = false;


        if (item.itemType == ItemType.gun || item.itemType == ItemType.armor || item.itemType == ItemType.secondaty_arms)
        {
            // Добавление прошло не успешно 
            ItemAdded = false;
        }
        else
        {
            if (slot.SlotIsEmpty)
            {
                // Устанавливаю предмет в пустой слот
                SetItemToEmptySlot(item, TransmittedObject, slot, out ItemAdded);

            }
            else
            {
                // Устанавливаю предмет в занятый другим предметом слот
                SetItemWithReplace(item, TransmittedObject, slot, out ItemAdded);
            }
        }


    }









    //          предмет, который передаем,         куда передаём,          выходной результат    
    public void PutWeaponToSlot(Item item, GameObject TransmittedObject, AmmunitionGunSlot slot, out bool GunIsAdded)
    {


        GunIsAdded = false;


        if (item.itemType != ItemType.gun)
        {
            // Предмет не является оружием, добавление прошло не успешно 
            GunIsAdded = false;
        }
        else
        {
            if (slot.SlotIsEmpty)
            {

                // Устанавливаю оружие в пустой слот
                SetItemToEmptySlot(item, TransmittedObject, slot, out GunIsAdded);

            }
            else
            {
                // Устанавливаю оружие в занятый другим предметом слот
                SetItemWithReplace(item, TransmittedObject, slot, out GunIsAdded);

            }
        }


    }









    public void DropItemFromInventory(Item item, GameObject currentObject, Slot slot, out bool SuccessDrop)
    {
        SuccessDrop = false;

        if (item != null && currentObject != null)
        {
            
            // Настраиваю слот, в котором лежал предмет
            slot.ClearClot();




            /*
             * Убираю картинку предмету в инвентаре
            */


            // Получаю картинку
            Image image_component = currentObject.transform.parent.gameObject.GetComponent<Image>();

            // Убираю картинку
            image_component.sprite = null;

            // Делаю картинку неактивной
            image_component.enabled = false;




            /*
             *  Ставлю картинку на дефолтное место
            */


            // Получаю объект картинки
            GameObject image_obj = currentObject.transform.parent.gameObject;

            // Ставлю на дефолтное место 
            image_obj.transform.position = slot.SlotDefaultPosition;




            /*
             * Выбрасываем предмет из инвентаря 
            */


            // Поднимаю его на вершину иерархии 
            currentObject.transform.SetParent(null);

            // Устанавливаю выброшенному объекту позицию игрока
            currentObject.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;




            /*
             * Устанавливаю спрайт предмету 
            */


            // Получаю спрайт предмета
            Sprite item_floor_image = item.GetFloorIcon;

            // Устанавливаю спрайт
            currentObject.GetComponent<SpriteRenderer>().sprite = item_floor_image;



            /*
             * Успешное завершение
            */

            SuccessDrop = true;


        }
    }









    public void SetMagToGun(Slot slot, GameObject transmitted_picture, out bool successLoad)
    {

        successLoad = false;

        // Получаю оружие в слоте
        GameObject gun_in_slot = slot.object_in_slot;

        // Получаю предмет в картинке
        GameObject mag_in_pic = transmitted_picture.transform.GetChild(0).gameObject;

        // Получаю слот картинки
        GameObject image_slot = transmitted_picture.transform.parent.gameObject;




        /*
         * Проверяю на то, есть ли магазин в объекте
        */

        if (gun_in_slot.transform.childCount > 0)
        {

            /*
             * Если объект не заряжен
            */

            bool is_loaded = false;

            gun_rifle rifle = gun_in_slot.GetComponent<FloorItem>().getItem as gun_rifle;
            gun_pistol pistol = gun_in_slot.GetComponent<FloorItem>().getItem as gun_pistol;

            if (rifle != null)
            {

                // Устанавливаю магазин в переменную оружия
                gun_in_slot.GetComponent<Internal_rifle_mag>().LoadMagToGun(mag_in_pic, out is_loaded);

                if (is_loaded)
                {
                    // Получаю магазин в объекте
                    GameObject mag_in_gun = gun_in_slot.transform.GetChild(0).gameObject;

                    // Ставлю магазин из оружия в исходный слот инвентаря
                    mag_in_gun.transform.SetParent(transmitted_picture.transform);

                    // Стввлю магазин из инвентаря как дочерний объект оружия
                    mag_in_pic.transform.SetParent(gun_in_slot.transform);

                    // Устанавливаю магазины в инвентаре
                    SetMagInInventoryWithReplace(transmitted_picture, image_slot, mag_in_pic, gun_in_slot, mag_in_gun);

                }

                successLoad = is_loaded;

            }
            else if (pistol != null)
            {
                // Устанавливаю магазин в переменную оружия
                gun_in_slot.GetComponent<Internal_pistol_mag>().LoadMagToGun(mag_in_pic, out is_loaded);

                if (is_loaded)
                {
                    // Получаю магазин в объекте
                    GameObject mag_in_gun = gun_in_slot.transform.GetChild(0).gameObject;

                    // Ставлю магазин из оружия в исходный слот инвентаря
                    mag_in_gun.transform.SetParent(transmitted_picture.transform);

                    // Стввлю магазин из инвентаря как дочерний объект оружия
                    mag_in_pic.transform.SetParent(gun_in_slot.transform);

                    // Устанавливаю магазины в инвентаре
                    SetMagInInventoryWithReplace(transmitted_picture, image_slot, mag_in_pic, gun_in_slot, mag_in_gun);

                }

                successLoad = is_loaded;
            }

        }
        else
        {

            /*
             * Если объект не заряжен
            */

            bool is_loaded = false;

            gun_rifle rifle = gun_in_slot.GetComponent<FloorItem>().getItem as gun_rifle;
            gun_pistol pistol = gun_in_slot.GetComponent<FloorItem>().getItem as gun_pistol;

            if (rifle != null)
            {
                // Устанавливаю магазин в переменную штурмовой винтовки
                gun_in_slot.GetComponent<Internal_rifle_mag>().LoadMagToGun(mag_in_pic, out is_loaded);

                if (is_loaded) 
                {
                    // Устанавливаю магазин в инвентаре
                    SetMagInInventory(transmitted_picture, image_slot, mag_in_pic, gun_in_slot);
                }

                successLoad = is_loaded;

            }
            else if (pistol != null)
            {
                // Устанавливаю магазин в переменную пистолета
                gun_in_slot.GetComponent<Internal_pistol_mag>().LoadMagToGun(mag_in_pic, out is_loaded);

                if (is_loaded)
                {
                    // Устанавливаю магазин в инвентаре
                    SetMagInInventory(transmitted_picture, image_slot, mag_in_pic, gun_in_slot);
                }
                
                successLoad = is_loaded;

            }

            successLoad = is_loaded;


        }
    }









    private void SetMagInInventory(GameObject transmitted_picture, GameObject image_slot, GameObject mag_in_pic, GameObject gun_in_slot)
    {
        // Устанавливаю магазин как дочерний объект оружия 
        mag_in_pic.transform.SetParent(gun_in_slot.transform);

        // Убираю картинку магазина
        transmitted_picture.GetComponent<Image>().sprite = null;

        // Выключаю картинку
        transmitted_picture.GetComponent<Image>().enabled = false;

        // Ставлю картинку на дефолтную позицию
        transmitted_picture.transform.position = image_slot.GetComponent<Slot>().SlotDefaultPosition;

        // Получаю картинку оружия
        Image gun_image = gun_in_slot.transform.parent.gameObject.GetComponent<Image>();

        // Ставлю спрайт заряженного оружия 
        gun_image.sprite = gun_in_slot.GetComponent<FloorItem>().getItem.GetInventoryIcon;

        // Отчищаю слот
        image_slot.GetComponent<Slot>().ClearClot();


    }









    private void SetMagInInventoryWithReplace(GameObject transmitted_picture, GameObject image_slot, GameObject mag_in_pic, GameObject gun_in_slot, GameObject mag_in_gun)
    {
        // Получаю картинку оружия
        Image gun_image = gun_in_slot.transform.parent.gameObject.GetComponent<Image>();

        // Ставлю спрайт заряженного оружия 
        gun_image.sprite = gun_in_slot.GetComponent<FloorItem>().getItem.GetInventoryIcon;

        // Обновляю картинку магазина
        transmitted_picture.GetComponent<Image>().sprite = mag_in_gun.GetComponent<FloorItem>().getItem.GetInventoryIcon;

        // Ставлю картинку на дефолтную позицию
        transmitted_picture.transform.position = image_slot.GetComponent<Slot>().SlotDefaultPosition;

        // Обновляю слот инвентаря
        image_slot.GetComponent<Slot>().SetItem(mag_in_gun.GetComponent<FloorItem>().getItem, mag_in_gun);
    }


}
