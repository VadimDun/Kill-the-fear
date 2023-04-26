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


        
        is_dragging = true;

        item_slot = null;

        gunSlot = null;

        //target_on_slot = true;

    }








    RaycastResult cursorEvent;


    public void OnDrag(PointerEventData eventData)
    {




        cursorEvent = eventData.pointerCurrentRaycast;

        GameObject TargetObject = cursorEvent.gameObject;



        if (TargetObject != null)
        {
            // Если курсор мышки на индикаторе,то получаем слот этого индикатора 
            if (TargetObject.name == "Gun_border")
            {
                // Получаю родителя (слот)
                GameObject gun_border_parent = TargetObject.transform.parent.gameObject;

                gunSlot = gun_border_parent;

            }
            else
                gunSlot = null;

            if (TargetObject.name == "Item_border")
            {
                // Получаю родителя (слот)
                GameObject item_border_parent = TargetObject.transform.parent.gameObject;

                item_slot = item_border_parent;


            }
            else
                item_slot = null;

            if (TargetObject.name == "GunImage" && TargetObject != transform.gameObject)
            {
                GameObject gun_border_parent = TargetObject.transform.parent.gameObject;
                gunSlot = gun_border_parent;
            }

            if (TargetObject.name == "ItemImage" && TargetObject != transform.gameObject)
            {
                GameObject item_border_parent = TargetObject.transform.parent.gameObject;
                item_slot = item_border_parent;
            }




            if (item_slot != null)
                Debug.Log($"Обнаружен {cursorEvent.gameObject.name}, при этом Item slot = {item_slot.name}");
            else
                Debug.Log($"Обнаружен {cursorEvent.gameObject.name}, при этом Item slot = Null");


        }
        else 
        {
            gunSlot = null;
            item_slot = null;
        }


        /*
         *  Проверка на нахождение курсора мыши в пределах корня инвентаря  
        */

        // Позиция мыши на экране
        Vector3 mousePosition = Input.mousePosition;

        // Проверка на нахожнение курсора мыши в пределах корня инвентаря 
        bool contains = RectTransformUtility.RectangleContainsScreenPoint(inventoryRootTransform, mousePosition);

        //bool is_slot = false;

        //GameObject current_object_on_cursor = eventData.pointerCurrentRaycast.gameObject;


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


    
    private void LateUpdate()
    {
        if (is_dragging)
        {
            // Обновляю позицию картинки на Canvas
            
            
            Vector2 newPosition = ItemEventData.position - offset + item_hotspot;
            (transform as RectTransform).anchoredPosition = newPosition;

        }


    }












    public void OnEndDrag(PointerEventData eventData)
    {
        is_dragging = false;


        // Включаю Graphics Raycaster
        transform.gameObject.GetComponent<GraphicRaycaster>().enabled = true;


        /*
         * Возвращаю ItemsUI обратно  
        */

        //items_UI_transform.SetParent(parent_transform_of_items_UI);



        // Устанавливаю в качестве родительского объекта тот, который был родительским до нажатия (для объекта, который перетаскиваем)
        transform.SetParent(parentTransform);



        /*
         *  Если обнаружен оружейный слот 
        */


        if (gunSlot != null)
        {

            // Получаю оружие, которое передаю
            GameObject gun = transform.GetChild(0).gameObject;

            // Получаю предмет, который передаю 
            Item item = gun.GetComponent<FloorItem>().getItem;

            // Получаю слот, в который хочу передать оружие
            AmmunitionGunSlot slot = gunSlot.GetComponent<AmmunitionGunSlot>();

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



        if (item_slot != null)
        {
            Debug.Log("Выполняется");
            // Получаю оружие, которое передаю
            GameObject itemObject = transform.GetChild(0).gameObject;

            // Получаю предмет, который передаю 
            Item item = itemObject.GetComponent<FloorItem>().getItem;

            // Получаю слот, в который хочу передать оружие
            ItemSlot slot = item_slot.GetComponent<ItemSlot>();

            bool SuccessAddition;

            // Передаю оружие в новый слот
            Camera_main.GetComponent<InventoryManager>().PutItemToSlot(item, itemObject, slot, out SuccessAddition);

            if (!SuccessAddition)
            {
                // Устанавливаю картинку на исходную позицию
                transform.position = transform.parent.GetComponent<Slot>().SlotDefaultPosition;
                Debug.Log("Что-то пошло по пизде");
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

            Slot item_slot = null;

            if (item.itemType == ItemType.gun)
            {
                item_slot = slot.GetComponent<AmmunitionGunSlot>();
            }

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