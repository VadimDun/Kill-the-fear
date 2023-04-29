using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Gun;


public class InventoryManager : MonoBehaviour
{

    private List<AmmunitionGunSlot> am_gun_slots = new List<AmmunitionGunSlot>();

    private List<SecondArmSlot> second_arm_slots = new List<SecondArmSlot>();

    public List<ItemSlot> itemSlots = new List<ItemSlot>();

    private RectTransform am_UI;

    private RectTransform item_UI;

    private Gun gun;








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
            else if (am_UI.GetChild(i).GetComponent<SecondArmSlot>() != null)
            {
                second_arm_slots.Add(am_UI.GetChild(i).GetComponent<SecondArmSlot>());
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

        // Отключаю рендеринг картинке
        second_arm_slots[0].transform.GetChild(1).GetComponent<Image>().enabled = false;

        // Загрузка инвентаря (матрица позиций слотов)
        Invoke("EndInventoryLoad", 0.1f);

        // Получаю скрипт оружия
        gun = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerGun>();

    }








    private void EndInventoryLoad()
    { 
        GameObject.Find("Inventory").SetActive(false);
        second_arm_slots[0].transform.GetChild(1).GetComponent<Image>().enabled = true;
    }








    private bool is_reloading = false;

    private void Update() 
    {
        if (Input.GetKey(KeyCode.R) && !is_reloading)
        {
            // Получаю слот текущего оружия
            GameObject current_gun_slot = gun.GetCurrentSlot();

            // Получаю оружие в слоте 
            GameObject gun_in_slot = current_gun_slot.GetComponent<Slot>().object_in_slot;

            // Получаю время перезарядки
            float reload_time = gun.get_current_reload_time;

            Gun.Guns gun_type = gun.GetGunType();

            // Ищу слот с непустым магазином
            GameObject mag_slot = null;

            if (gun_type == Gun.Guns.hammer || gun_type == Gun.Guns.none) { return; }

            if (gun_type == Gun.Guns.shotgun) { mag_slot = SearchSlotWithBulletStack(); }
            else { mag_slot = SearchSlotWithMag(gun_type); }

            if (mag_slot != null && gun_in_slot != null) 
            {
                if (gun_type == Gun.Guns.shotgun)
                {
                }
                else
                {
                    Debug.Log($"Дефолтная позиция слота обоймы = {mag_slot.GetComponent<Slot>().SlotDefaultPosition}");
                    StartCoroutine(ReloadGun(current_gun_slot, mag_slot, reload_time));
                }
            }

        }
    }









    private GameObject SearchSlotWithMag(Gun.Guns gun_type)
    {
        foreach (Slot slot in itemSlots)
        {

            if (gun_type == Gun.Guns.assaultRifle)
            {
                // Получаю предмет в слоте
                RifleMag mag_in_slot = slot.item_in_slot as RifleMag;
                if (mag_in_slot != null && slot.object_in_slot.transform.childCount > 0)
                    return slot.gameObject;
            }
            else
            {
                // Получаю предмет в слоте
                PistolMag mag_in_slot = slot.item_in_slot as PistolMag;
                if (mag_in_slot != null && slot.object_in_slot.transform.childCount > 0)
                    return slot.gameObject;
            }
        }
        return null;
    }








    private GameObject SearchSlotWithBulletStack()
    {
        foreach (Slot slot in itemSlots)
        {
            // Получаю предмет в слоте
            BulletStack internal_bullet_stack = slot.item_in_slot as BulletStack;
            if (internal_bullet_stack != null && slot.object_in_slot.transform.childCount > 0)
                return slot.gameObject;
        }
        return null;
    }








    IEnumerator ReloadGun(GameObject gun_slot, GameObject mag_slot, float reload_time)
    {
        is_reloading = true;
        
        yield return new WaitForSeconds(reload_time);

        bool flag = false;
        
        SetMagToGun(gun_slot, mag_slot, out flag);

        Debug.Log($"Замена прошла успешо, = {flag}");
        
        is_reloading = false;
    }










    public void MainInventoryManager(Item item, GameObject itemObj)
    {




        /*
         * Если передаваемый объект является оружием 
        */


        if (item.itemType == ItemType.gun)
        {
            foreach (AmmunitionGunSlot slot in am_gun_slots)
            {

                bool SuccessGunAddition = false;

                GrabGunItem(item, itemObj, slot, out SuccessGunAddition);


                if (SuccessGunAddition) return;

            }
        }



        /*
         * Если передаваемый предмет не является оружием или броней 
        */


        if (item.itemType != ItemType.gun && item.itemType != ItemType.secondaty_arms && item.itemType != ItemType.armor && item.itemType != ItemType.edged_weapon) 
        {
            foreach (ItemSlot slot in itemSlots)
            {
                bool success = false;

                GrabDefaultItem(item, itemObj, slot, out success);


                if (success) return;
            }
        }




        if (item.itemType == ItemType.edged_weapon)
        {
            GrabEdgedWeapon(item, itemObj);
        }


    }









    private void GrabEdgedWeapon(Item item, GameObject itemObj)
    {

        Slot slot = second_arm_slots[0];

        if (slot.SlotIsEmpty)
        {

            // Получаю картинку предмета
            Transform image = slot.transform.GetChild(1);




            /*
             *  Настраиваю подобранный предмет
            */

            // Убираю спрайт объекта на земле 
            itemObj.GetComponent<SpriteRenderer>().sprite = null;

            // Выключаю коллайдер пердмету на земле, чтобы его нельзя было подобрать дважды
            itemObj.GetComponent<Collider2D>().enabled = false;




            /*
             *  Устанавливаю изображение предмета, которого передал в слот
            */

            image.GetComponent<Image>().sprite = item.GetInventoryIcon;

            image.GetComponent<Image>().enabled = true;




            /*
             * Настраиваю скрипт слота
            */

            // Передаю в слот предмет и представляющий его объект 
            slot.SetItem(item, itemObj);


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
             *  Настраиваю подобранный предмет
            */


            // Убираю спрайт объекта на земле 
            TransmittedObject.GetComponent<SpriteRenderer>().sprite = null;

            // Выключаю коллайдер пердмету на земле, чтобы его нельзя было подобрать дважды
            TransmittedObject.GetComponent<Collider2D>().enabled = false;




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










    private void SetItemToEmptySlot(Item item, GameObject TransmittedObject, Slot slot, Slot current_slot, out bool ItemAdded)
    {
        ItemAdded = false;

        if (item != null && TransmittedObject != null && slot != null)
        {


            // Получаю Transform картинки, в которую хочу передать предмет
            Transform InputImageTransform = slot.transform.GetChild(1);

            // Получаю Transform картинки, текущей картинки передаваемого предмета
            Transform currentImageTransform = current_slot.transform.GetChild(1);




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
            current_slot.ClearClot();




            /*
             * Настраиваю картинку слота, из которой передавали предмет  
            */


            // Удаляю изображение предмета, который передавали
            currentImageTransform.GetComponent<Image>().sprite = null;

            // Возвращаю картинку дефолтное на место
            currentImageTransform.position = current_slot.SlotDefaultPosition;

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









    private void SetItemWithReplace(Item item, GameObject TransmittedObject, Slot slot, Slot current_slot, out bool ItemAdded)
    {
        

        /*
         * Получаю картинки слотов
        */


        // Получаю Transform картинки, в которую хочу передать предмет
        Transform InputImageTransform = slot.transform.GetChild(1);

        // Получаю Transform картинки, текущей картинки передаваемого предмета
        Transform currentImageTransform = current_slot.transform.GetChild(1);




        /*
         * Меняю местами картинки (в иерархии)
        */


        InputImageTransform.SetParent(current_slot.transform);

        currentImageTransform.SetParent(slot.transform);




        /*
         * Меняю местами картинки (визуально)
        */


        // Возвращаю исходную картинку на место
        currentImageTransform.position = slot.SlotDefaultPosition;

        InputImageTransform.position = current_slot.SlotDefaultPosition;




        /*
         * Настраиваю скрипты слотов, в который передали 
        */


        // Получаю объект, который сейчас находится в CurrentSlot
        GameObject current_obj = current_slot.object_in_slot;

        // Получаю предмет, который сейчас находится в CurrentSlot
        Item item_current = current_slot.item_in_slot;

        // Получаю объект, который сейчас находится в InputSlot
        GameObject input_obj = slot.object_in_slot;

        // Получаю предмет, который сейчас находится в CurrentSlot
        Item item_input = slot.item_in_slot;

        // Устанавливаю их в новый слот
        slot.SetItem(item_current, current_obj);

        // Устанавливаю их в новый слот 
        current_slot.SetItem(item_input, input_obj);




        /*
         * Успешная передача предмета
        */


        // Замена предмета прошла успешно
        ItemAdded = true;


    }










    public void PutItemToSlot(Item item, GameObject TransmittedObject, ItemSlot slot, ItemSlot current_slot, out bool ItemAdded)
    {
        ItemAdded = false;


        if (item.itemType == ItemType.gun || item.itemType == ItemType.armor || item.itemType == ItemType.secondaty_arms || item.itemType == ItemType.edged_weapon)
        {
            // Добавление прошло не успешно 
            ItemAdded = false;
        }
        else
        {
            if (slot.SlotIsEmpty)
            {
                // Устанавливаю предмет в пустой слот
                SetItemToEmptySlot(item, TransmittedObject, slot, current_slot, out ItemAdded);

            }
            else
            {
                // Устанавливаю предмет в занятый другим предметом слот
                SetItemWithReplace(item, TransmittedObject, slot, current_slot, out ItemAdded);
            }
        }


    }









    //          предмет, который передаем,         куда передаём,          выходной результат    
    public void PutWeaponToSlot(Item item, GameObject TransmittedObject, AmmunitionGunSlot slot, AmmunitionGunSlot current_slot, out bool GunIsAdded)
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
                SetItemToEmptySlot(item, TransmittedObject, slot, current_slot, out GunIsAdded);

            }
            else
            {
                // Устанавливаю оружие в занятый другим предметом слот
                SetItemWithReplace(item, TransmittedObject, slot, current_slot, out GunIsAdded);

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
            Image image_component = slot.transform.GetChild(1).gameObject.GetComponent<Image>();

            // Убираю картинку
            image_component.sprite = null;

            // Делаю картинку неактивной
            image_component.enabled = false;




            /*
             *  Ставлю картинку на дефолтное место
            */


            // Получаю объект картинки
            GameObject image_obj = slot.transform.GetChild(1).gameObject;

            // Ставлю на дефолтное место 
            image_obj.transform.position = slot.SlotDefaultPosition;




            /*
             * Вызываю текущий слот, чтобы обновить данные текущего оружия 
            */

            gun.ChangeGun(gun.get_current_slot); 



            /*
             * Выбрасываем предмет из инвентаря 
            */


            // Устанавливаю выброшенному объекту позицию игрока
            currentObject.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;




            /*
             * Устанавливаю спрайт предмету 
            */


            // Получаю спрайт предмета
            Sprite item_floor_image = item.GetFloorIcon;

            // Устанавливаю спрайт
            currentObject.GetComponent<SpriteRenderer>().sprite = item_floor_image;

            // Включаю коллайдер обратно, чтобы его можно было подобрать
            currentObject.GetComponent<Collider2D>().enabled = true;




            /*
             * Успешное завершение
            */

            SuccessDrop = true;


        }
    }









    public void SetMagToGun(GameObject input_slot, GameObject current_slot, out bool successLoad)
    {

        successLoad = false;

        // Получаю данные входнохо слота
        Slot input_slot_data = input_slot.GetComponent<Slot>();

        // Получаю оружие во входном слоте
        GameObject gun_in_input_slot = input_slot_data.object_in_slot;

        // Получаю данные текущего слота
        Slot current_slot_data = current_slot.GetComponent<Slot>();

        // Получаю предмет в исходном слоте
        GameObject mag_in_current_slot = current_slot_data.object_in_slot;




        /*
         * Проверяю на то, есть ли магазин в объекте
        */

        if (gun_in_input_slot.transform.childCount > 0)
        {

            /*
             * Если объект заряжен
            */

            bool is_loaded = false;

            gun_rifle rifle = gun_in_input_slot.GetComponent<FloorItem>().getItem as gun_rifle;
            gun_pistol pistol = gun_in_input_slot.GetComponent<FloorItem>().getItem as gun_pistol;

            if (rifle != null)
            {

                // Устанавливаю магазин в переменную оружия
                gun_in_input_slot.GetComponent<Internal_rifle_mag>().LoadMagToGun(mag_in_current_slot, out is_loaded);

                if (is_loaded)
                {
                    // Получаю магазин в объекте
                    GameObject dropped_gun_mag = gun_in_input_slot.transform.GetChild(0).gameObject;

                    // Убираю магазин в свободное плавание в иерархии проекта
                    dropped_gun_mag.transform.SetParent(null);

                    // Стввлю магазин из инвентаря как дочерний объект оружия
                    mag_in_current_slot.transform.SetParent(gun_in_input_slot.transform);

                    // Устанавливаю магазины в инвентаре
                    SetMagInInventoryWithReplace(input_slot, current_slot, dropped_gun_mag);

                    // Обновляю ссылку на обойму
                    gun.ChangeGun(gun.get_current_slot);

                }

                successLoad = is_loaded;

            }
            else if (pistol != null)
            {
                // Устанавливаю магазин в переменную оружия
                gun_in_input_slot.GetComponent<Internal_pistol_mag>().LoadMagToGun(mag_in_current_slot, out is_loaded);

                if (is_loaded)
                {
                    // Получаю магазин в объекте
                    GameObject dropped_gun_mag = gun_in_input_slot.transform.GetChild(0).gameObject;

                    // Убираю магазин в свободное плавание в иерархии проекта
                    dropped_gun_mag.transform.SetParent(null);

                    // Стввлю магазин из инвентаря как дочерний объект оружия
                    mag_in_current_slot.transform.SetParent(gun_in_input_slot.transform);

                    // Устанавливаю магазины в инвентаре
                    SetMagInInventoryWithReplace(input_slot, current_slot, dropped_gun_mag);

                    // Обновляю ссылку на обойму
                    gun.ChangeGun(gun.get_current_slot);

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

            gun_rifle rifle = gun_in_input_slot.GetComponent<FloorItem>().getItem as gun_rifle;
            gun_pistol pistol = gun_in_input_slot.GetComponent<FloorItem>().getItem as gun_pistol;

            if (rifle != null)
            {
                // Устанавливаю магазин в переменную штурмовой винтовки
                gun_in_input_slot.GetComponent<Internal_rifle_mag>().LoadMagToGun(mag_in_current_slot, out is_loaded);

                // Обновляю ссылку на обойму
                gun.ChangeGun(gun.get_current_slot);

                if (is_loaded) 
                {
                    // Устанавливаю магазин в инвентаре
                    SetMagInInventory(input_slot, current_slot);
                }

                successLoad = is_loaded;

            }
            else if (pistol != null)
            {
                // Устанавливаю магазин в переменную пистолета
                gun_in_input_slot.GetComponent<Internal_pistol_mag>().LoadMagToGun(mag_in_current_slot, out is_loaded);

                // Обновляю ссылку на обойму
                gun.ChangeGun(gun.get_current_slot);

                if (is_loaded)
                {
                    // Устанавливаю магазин в инвентаре
                    SetMagInInventory(input_slot, current_slot);
                }
                
                successLoad = is_loaded;

            }

            successLoad = is_loaded;


        }
    }









    private void SetMagInInventory(GameObject input_slot, GameObject current_slot)
    {

        // Получаю магазин в текущей картинке
        GameObject mag_in_pic = current_slot.GetComponent<Slot>().object_in_slot;

        // Получаю оружие во входном слоте
        GameObject gun_in_slot = input_slot.GetComponent<Slot>().object_in_slot;

        // Получаю картинку оружия
        Image gun_image = input_slot.transform.GetChild(1).gameObject.GetComponent<Image>();

        // Получаю картинку текущего слота
        Image transmitted_picture = current_slot.transform.GetChild(1).gameObject.GetComponent<Image>();




        // Устанавливаю магазин как дочерний объект оружия 
        mag_in_pic.transform.SetParent(gun_in_slot.transform);

        // Убираю картинку магазина
        transmitted_picture.GetComponent<Image>().sprite = null;

        // Выключаю картинку
        transmitted_picture.GetComponent<Image>().enabled = false;

        // Ставлю картинку на дефолтную позицию
        transmitted_picture.transform.position = current_slot.GetComponent<Slot>().SlotDefaultPosition;

        // Ставлю спрайт заряженного оружия 
        gun_image.sprite = gun_in_slot.GetComponent<FloorItem>().getItem.GetInventoryIcon;

        // Отчищаю слот
        current_slot.GetComponent<Slot>().ClearClot();


    }









    private void SetMagInInventoryWithReplace(GameObject input_slot, GameObject current_slot, GameObject dropped_gun_mag)
    {
        // Получаю картинку оружия
        Image gun_image = input_slot.transform.GetChild(1).gameObject.GetComponent<Image>();

        // Получаю оружие в слоте
        GameObject gun_in_input_slot = input_slot.GetComponent<Slot>().object_in_slot;

        // Получаю картинку текущего слота
        GameObject current_image = current_slot.transform.GetChild(1).gameObject;




        // Ставлю спрайт заряженного оружия 
        gun_image.sprite = gun_in_input_slot.GetComponent<FloorItem>().getItem.GetInventoryIcon;

        // Обновляю картинку магазина
        current_image.GetComponent<Image>().sprite = dropped_gun_mag.GetComponent<FloorItem>().getItem.GetInventoryIcon;

        // Ставлю картинку на дефолтную позицию
        current_image.transform.position = current_slot.GetComponent<Slot>().SlotDefaultPosition;
        Debug.Log(current_image.transform.position);

        // Обновляю слот инвентаря
        current_slot.GetComponent<Slot>().SetItem(dropped_gun_mag.GetComponent<FloorItem>().getItem, dropped_gun_mag);

    }









    public void ResetInventory()
    {
        foreach (Slot slot in am_gun_slots)
        {
            ResetSlot(slot);
        }

        foreach (Slot slot in second_arm_slots)
        {
            ResetSlot(slot);
        }

        foreach (Slot slot in itemSlots)
        {
            ResetSlot(slot);
        }
    }









    private void ResetSlot(Slot slot)
    {
        // Отчищаем слот
        slot.ClearClot();

        // Убираем картинку
        slot.transform.GetChild(1).GetComponent<Image>().sprite = null;

        // Делаем картинку неактивной
        slot.transform.GetChild(1).GetComponent<Image>().enabled = false;
    }



}
