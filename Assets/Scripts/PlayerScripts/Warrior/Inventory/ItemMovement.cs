using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Gun;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class ItemMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Vector2 offset; // смещение между позицией картинки и позицией мыши

    private UnityEngine.Transform parentTransform;

    private GameObject gunSlot;

    private GameObject item_slot;

    private GameObject current_slot;

    private bool in_inventory_range = true;

    private bool flag_on_start = true;

    private bool is_dragging = false;

    private PointerEventData ItemEventData;

    private RectTransform inventoryTransform;

    private GameObject[] GunSlotIndicators = new GameObject[3];

    private GameObject[] gunSlots = new GameObject[3];

    private GameObject Camera_main;

    private RectTransform inventoryRootTransform;

    private RectTransform items_UI_transform;

    private RectTransform canvasTransform;

    private RectTransform parent_transform_of_items_UI;

    private RectTransform parent_transform_of_ammunitionUI;

    private RectTransform ammunitionUI_transform;
    
    private Vector2 item_hotspot;

    private Rigidbody2D image_rb;

    //bool target_on_slot;


    private void Start()
    {


        // Получаю объект камеры на старте
        Camera_main = GameObject.Find("Main Camera");

        // Получаю RectTransform корня инвентаря
        inventoryRootTransform = GameObject.Find("InventoryRoot").GetComponent<RectTransform>();

        /*
         * Получаю RectTransform инвентаря  
        */

        inventoryTransform = GameObject.Find("Inventory").GetComponent<RectTransform>();


        /*
         * Получаю Items_UI RectTransform  
        */

        items_UI_transform = GameObject.Find("ItemsUI").GetComponent<RectTransform>();

        /*
         *  Получаю Transform канваса
        */

        canvasTransform = GameObject.Find("Canvas").GetComponent<RectTransform>();


        /*
         * Получаю RectTransform AmmunitionUI 
        */

        ammunitionUI_transform = GameObject.Find("AmmunitionUI").GetComponent<RectTransform>();


        /*
         * Получаю hotspot перетаскиваемой картинки 
        */

        item_hotspot = new Vector2 (inventoryTransform.position.x - inventoryRootTransform.position.x, inventoryTransform.position.y - inventoryRootTransform.position.y);

        image_rb = GetComponent<Rigidbody2D>();
    }










    public void OnBeginDrag(PointerEventData eventData)
    {

        ItemEventData = eventData;

        // Запоминаю Transform родительского объекта
        parentTransform = transform.parent;

        // Запоминаю Transform родительского объекта Items_UI
        parent_transform_of_items_UI = items_UI_transform.parent.gameObject.GetComponent<RectTransform>();

        // Запоминаю Transform родительского объекта AmmunitionUI
        parent_transform_of_ammunitionUI = ammunitionUI_transform.parent.gameObject.GetComponent<RectTransform>();

        // Устанавливаю родительский объект в качестве инвентаря
        transform.SetParent(inventoryRootTransform);

        // Рассчитываю смещение между позицией картинки и позицией мыши
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out offset
        );



        /*
         * Ставлю приоритет рендеринга перетаскиваемой картинке выше всех
        */

        transform.SetSiblingIndex(canvasTransform.childCount - 1);

        // Отключаю Graphics Raycaster
        transform.gameObject.GetComponent<GraphicRaycaster>().enabled = false;

        // Отключаю скорость
        image_rb.velocity = Vector2.zero;

        is_dragging = true;

        item_slot = null;

        gunSlot = null;

        //target_on_slot = true;

    }








    RaycastResult cursorEvent;

    private string[] borders = { "Gun_border", "Item_border" };

    private string[] images = { "GunImage", "ItemImage" };




    public void OnDrag(PointerEventData eventData)
    {

        // Получаю графический луч
        cursorEvent = eventData.pointerCurrentRaycast;

        // Получаю объект, который под курсором мыши
        GameObject TargetObject = cursorEvent.gameObject;




        /*
         * Попытка получить слот, в который передаем предмет 
        */


        if (TargetObject != null)
        {
            // Проверяю если TargetObject является границей слота
            CheckCurrentObjectOnBorders(TargetObject, ref current_slot);

            // Проверяю если TargetObject является картинкой в слоте 
            CheckCurrentObjectOnImages(TargetObject, ref current_slot);
        }
        else
        {

            // Обновляю current_slot, если луч не встречает объектов
            current_slot = null;
        }




        /*
         *  Проверка на нахождение курсора мыши в пределах корня инвентаря  
        */


        // Позиция мыши на экране
        Vector3 mousePosition = Input.mousePosition;

        // Проверка на нахожнение курсора мыши в пределах корня инвентаря 
        bool contains = RectTransformUtility.RectangleContainsScreenPoint(inventoryRootTransform, mousePosition);




        if (contains)
        {
            in_inventory_range = true;
            //target_on_slot = false;
        }
        else if (!contains)
        {
            in_inventory_range = false;
        }


    }









    private void CheckCurrentObjectOnBorders(GameObject current_object, ref GameObject current_slot)
    {
        if (borders.Contains(current_object.name))
        {
            // Получаю слот оружия, либо предмета
            current_slot = GetSlot(current_object);
        }

    }








    private void CheckCurrentObjectOnImages(GameObject current_object, ref GameObject current_slot)
    {
        if (images.Contains(current_object.name))
        {
            // Если картинка не является картинкой перетаскиваемого объекта 
            if (current_object != transform.gameObject)
            {
                // Получаю слот картинки оружия, либо предмета
                current_slot = GetSlot(current_object);
            }
        }
    }










    private GameObject GetSlot(GameObject current_object)
    {
        // Получаю слот, в котором находится текущий объект
        GameObject item_border_parent = current_object.transform.parent.gameObject;

        return item_border_parent;

    }








    
    
    private void Update()
    {
        
        if (is_dragging)
        {
            // Обновляю позицию картинки на Canvas
            Vector2 targetPosition = ItemEventData.position - offset + item_hotspot;

            Vector2 direction = (targetPosition - (transform as RectTransform).anchoredPosition).normalized;
            image_rb.AddForce(direction * 10000);
            (transform as RectTransform).anchoredPosition = targetPosition;
        }

    }

    












    public void OnEndDrag(PointerEventData eventData)
    {
        is_dragging = false;


        // Включаю Graphics Raycaster
        transform.gameObject.GetComponent<GraphicRaycaster>().enabled = true;

        // Сбрасываю скорость объекта
        image_rb.velocity = Vector2.zero;


        /*
         * Возвращаю ItemsUI обратно  
        */

        //items_UI_transform.SetParent(parent_transform_of_items_UI);



        // Устанавливаю в качестве родительского объекта тот, который был родительским до нажатия (для объекта, который перетаскиваем)
        transform.SetParent(parentTransform);



        /*
         *  Если обнаружен оружейный слот 
        */

        //gunSlot
        if (current_slot != null && current_slot.tag == "GunSlot")
        {
            Debug.Log("Тэг ганслота = " + current_slot.tag);
            Debug.Log("Имя слота = " + current_slot.name);
            // Получаю оружие, которое передаю
            GameObject gun = transform.GetChild(0).gameObject;

            // Получаю предмет, который передаю 
            Item item = gun.GetComponent<FloorItem>().getItem;

            // Получаю слот, в который хочу передать оружие
            AmmunitionGunSlot slot = current_slot.GetComponent<AmmunitionGunSlot>();

            bool SuccessAddition;

            // Передаю оружие в новый слот
            Camera_main.GetComponent<InventoryManager>().PutWeaponToSlot(item, gun, slot, out SuccessAddition);

            if (!SuccessAddition)
            {

                // Устанавливаю картинку на исходную позицию
                transform.position = transform.parent.GetComponent<Slot>().SlotDefaultPosition;

            }


            return;

        }



        /*
         * Если обнаружен слот Item_UI 
        */



        if (current_slot != null && current_slot.tag == "ItemSlot")
        {
            Debug.Log("Выполняется");
            // Получаю оружие, которое передаю
            GameObject itemObject = transform.GetChild(0).gameObject;

            // Получаю предмет, который передаю 
            Item item = itemObject.GetComponent<FloorItem>().getItem;

            // Получаю слот, в который хочу передать оружие
            ItemSlot slot = current_slot.GetComponent<ItemSlot>();

            bool SuccessAddition;

            // Передаю оружие в новый слот
            Camera_main.GetComponent<InventoryManager>().PutItemToSlot(item, itemObject, slot, out SuccessAddition);

            if (!SuccessAddition)
            {
                // Устанавливаю картинку на исходную позицию
                transform.position = transform.parent.GetComponent<Slot>().SlotDefaultPosition;
            }


            return;
        }



        /*
         *  Если индикаторов не обнаруженно, и курсор находиться в пределах инвентаря 
        */



        

        if (in_inventory_range == true)
        {
            // Устанавливаю картинку на исходную позицию
            transform.position = transform.parent.GetComponent<Slot>().SlotDefaultPosition;
        }
        else
        {

            // Получаю выбрасываемый объект
            GameObject DroppedObj = transform.GetChild(0).gameObject;

            // Получаю выбрасываемый предмет
            Item item = DroppedObj.GetComponent<FloorItem>().getItem;

            // Получаю слот из которого выбросили предмет
            GameObject slot = transform.parent.gameObject;
            
            // Получаю данные слота
            Slot item_slot = slot.GetComponent<Slot>();


            if (DroppedObj != null && item != null && item_slot != null)
            {
                bool drop_is_success;

                Camera_main.GetComponent<InventoryManager>().DropItemFromInventory(item, DroppedObj, item_slot, out drop_is_success);

                if (!drop_is_success)
                {
                    Debug.Log("Предмет не был выброшен");

                    // Устанавливаю картинку на исходную позицию
                    transform.position = transform.parent.GetComponent<Slot>().SlotDefaultPosition;
                }

            }
            else 
            {
                // Устанавливаю картинку на исходную позицию
                transform.position = transform.parent.GetComponent<Slot>().SlotDefaultPosition;
            }

        }

        
    }
}