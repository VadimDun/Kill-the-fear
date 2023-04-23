using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Vector2 offset; // смещение между позицией картинки и позицией мыши

    private Transform parentTransform;

    private GameObject gunSlot;

    private bool in_inventory_range = true;

    private bool flag_on_start = true;

    private bool is_dragging = false;

    private PointerEventData ItemEventData;

    [SerializeField] private RectTransform inventoryTransform;

    [SerializeField] private GameObject[] GunSlotIndicators = new GameObject[3];

    private GameObject[] gunSlots = new GameObject[3];

    private GameObject Camera_main;

    private RectTransform inventoryRootTransform;

    



    private void Start()
    {
        // Получаю оружейные слоты инвентаря на старте
        for (int i = 0; i < gunSlots.Count(); i++)
        {
            gunSlots[i] = GameObject.Find($"GunSlot({i + 1})");
        }

        // Получаю объект камеры на старте
        Camera_main = GameObject.Find("Main Camera");

        // Получаю RectTransform корня инвентаря
        inventoryRootTransform = GameObject.Find("InventoryRoot").GetComponent<RectTransform>();

    }










    public void OnBeginDrag(PointerEventData eventData)
    {

        ItemEventData = eventData;

        // Запоминаю Transform родительского объекта
        parentTransform = transform.parent;

        // Устанавливаю родительский объект в качестве инвентаря
        transform.SetParent(inventoryTransform);

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


            is_dragging = true;


        }
    }










    public void OnDrag(PointerEventData eventData)
    {

        RaycastResult cursorEvent = eventData.pointerCurrentRaycast;

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
            
            
            Vector2 newPosition = ItemEventData.position - offset;
            (transform as RectTransform).anchoredPosition = newPosition;

        }
    }









    public void OnEndDrag(PointerEventData eventData)
    {
        is_dragging = false;






        /*
         *  Устанавливаю индикаторы на обратно 
        */






        for (int i = 0; i < GunSlotIndicators.Count(); i++)
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

            
        }






        /*
         *  Если индикаторов не обнаруженно, и курсор находиться в пределах инвентаря 
        */





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


    }
}