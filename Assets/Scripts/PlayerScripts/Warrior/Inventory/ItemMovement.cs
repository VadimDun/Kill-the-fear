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
    
    private Vector2 item_hotspot;


    private void Start()
    {


        // Получаю объект камеры на старте
        Camera_main = GameObject.Find("Main Camera");

        // Получаю RectTransform корня инвентаря
        inventoryRootTransform = GameObject.Find("InventoryRoot").GetComponent<RectTransform>();


        /*
         * Получаю оружейные слоты 
        */

        for (int i = 0; i < gunSlots.Count(); i++)
        {
            GameObject gunSlotObj = GameObject.Find($"GunSlot({i + 1})");
            gunSlots[i] = gunSlotObj;
            GunSlotIndicators[i] = gunSlotObj.transform.GetChild(1).gameObject;
        }



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

        // Устанавливаю родительский объект в качестве инвентаря
        transform.SetParent(inventoryRootTransform);

        // Рассчитываю смещение между позицией картинки и позицией мыши
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out offset
        );


        

        // Ставлю индикаторы на передний план

        foreach (GameObject indicator in GunSlotIndicators)
        {

            if (indicator != null)
            {
                // Перед тем как установить родителя в качестве инвентаря - запоминаем исходного родителя
                Indicator ind = indicator.GetComponent<Indicator>();
                if (ind != null)
                {
                    if (flag_on_start)
                    {
                        // Перед тем как установить родителя в качестве инвентаря - запоминаем исходного родителя
                        ind.RememperParent(indicator.transform.parent.gameObject);
                    }

                    // Устанавливаем родителя в качестве инвентаря, чтобы поместить индикатор на передний план
                    indicator.transform.SetParent(inventoryTransform);
                }

            }


        }




        /*
         * Items_UI на передний план 
        */

        /*
        items_UI_transform.SetParent(canvasTransform);
        */

        /*
         * Ставлю приоритет рендеринга перетаскиваемой картинке выше всех
        */

        transform.SetSiblingIndex(canvasTransform.childCount - 1);

        // Отключаю коллайдер перетаскиваемому объекту, чтобы луч не разбивался об него 
        transform.GetChild(0).gameObject.GetComponent<Collider2D>().enabled = true;
        
        is_dragging = true;

        item_slot = null;
    }








    RaycastResult cursorEvent;

    public void OnDrag(PointerEventData eventData)
    {

        cursorEvent = eventData.pointerCurrentRaycast;

        gunSlot = null;

        

        if (cursorEvent.gameObject != null)
        {
            // Если курсор мышки на индикаторе,то получаем слот этого индикатора 
            if (cursorEvent.gameObject.name == "GunSlotIndicator")
            {
                Indicator ind = cursorEvent.gameObject.GetComponent<Indicator>();
                if (ind != null)
                {
                    gunSlot = ind.GetIndicatorParent;
                }

            }


        }



        






        /*
         *  Проверка на нахождение курсора мыши в пределах корня инвентаря  
        */



        // Позиция мыши на экране
        Vector3 mousePosition = Input.mousePosition;


        if (RectTransformUtility.RectangleContainsScreenPoint(inventoryRootTransform, mousePosition))
        {
            in_inventory_range = true;
        }
        else
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


    private void Update()
    {


        if (is_dragging)
        {
            if (cursorEvent.gameObject != null)
            {

                if (cursorEvent.gameObject.name == "Item_border")
                {
                    // Получаю родителя (слот)
                    GameObject item_border_parent = cursorEvent.gameObject.transform.parent.gameObject;

                    item_slot = item_border_parent;

                    Debug.Log("Найден");
                }


            }
        }
    }









    public void OnEndDrag(PointerEventData eventData)
    {
        is_dragging = false;



        /*
         *  Устанавливаю индикаторы обратно 
        */






        for (int i = 0; i < GunSlotIndicators.Count(); i++)
        {



            if (GunSlotIndicators[i] != null)
            {
                Indicator ind = GunSlotIndicators[i].GetComponent<Indicator>();

                if (ind != null)
                {

                    if (GunSlotIndicators[i] != null)
                    {
                        // Устанавливаю родителя
                        GunSlotIndicators[i].transform.SetParent(gunSlots[i].transform);

                        // Устанавливаю индикатор на нужный мне индекс 
                        GunSlotIndicators[i].transform.SetSiblingIndex(1);

                        // Получаю родителя
                        GameObject ind_parent = GunSlotIndicators[i].transform.parent.gameObject;


                        // Запоминаю родителя
                        ind.RememperParent(ind_parent);


                        // Устанавливаю индикатор на позицию родительского слота
                        GunSlotIndicators[i].transform.position = ind_parent.GetComponent<AmmunitionGunSlot>().SlotDefaultPosition;

                        // Последующие разы в начале не будем запоминать родительский объект за ненадобностью
                        flag_on_start = false;
                    }
                }
            }




        }





        /*
         * Возвращаю ItemsUI обратно  
        */

        //items_UI_transform.SetParent(parent_transform_of_items_UI);



        // Устанавливаю в качестве родительского объекта тот, который был родительским до нажатия (для объекта, который перетаскиваем)
        transform.SetParent(parentTransform);






        /*
         *  Если обнаружен индикатор слота с оружием 
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
                // Логика если оружие не добавилось в слот 

            }


            return;

        }




        if (item_slot != null)
        { 
            Debug.Log($"Имя объекта, на который мы ставим картинку = {item_slot.name}");
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
                // Логика если оружие не добавилось в слот 

            }

            Debug.Log("Оружие должно быть добавлено в новый слот");
            return;
        }



        // Отключаю коллайдер перетаскиваемому объекту, чтобы луч не разбивался об него 
        transform.GetChild(0).gameObject.GetComponent<Collider2D>().enabled = true;


        /*
         *  Если индикаторов не обнаруженно, и курсор находиться в пределах инвентаря 
        */



        /*

        if (in_inventory_range == true)
        {
            // Устанавливаю картинку на исходную позицию
            transform.position = transform.parent.GetComponent<AmmunitionGunSlot>().SlotDefaultPosition;
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
                }

            }

        }

        */
    }
}