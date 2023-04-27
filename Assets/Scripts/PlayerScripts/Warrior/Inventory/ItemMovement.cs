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
    private Vector2 offset; // �������� ����� �������� �������� � �������� ����

    private UnityEngine.Transform parentTransform;

    private GameObject current_slot;

    private bool in_inventory_range = true;

    private bool is_dragging = false;

    private PointerEventData ItemEventData;

    private RectTransform inventoryTransform;

    private GameObject Camera_main;

    private RectTransform inventoryRootTransform;

    private RectTransform canvasTransform;
    
    private Vector2 item_hotspot;

    private Rigidbody2D image_rb;

    //bool target_on_slot;


    private void Start()
    {


        // ������� ������ ������ �� ������
        Camera_main = GameObject.Find("Main Camera");

        // ������� RectTransform ����� ���������
        inventoryRootTransform = GameObject.Find("InventoryRoot").GetComponent<RectTransform>();

        //������� RectTransform ���������  
        inventoryTransform = GameObject.Find("Inventory").GetComponent<RectTransform>();

        // ������� Transform �������
        canvasTransform = GameObject.Find("Canvas").GetComponent<RectTransform>();




        /*
         * ������� hotspot ��������������� �������� (��-�� ��������� ��������� ItemsUI, ammunitionUI)
        */

        item_hotspot = new Vector2 (inventoryTransform.position.x - inventoryRootTransform.position.x, inventoryTransform.position.y - inventoryRootTransform.position.y);

        image_rb = GetComponent<Rigidbody2D>();

    }










    public void OnBeginDrag(PointerEventData eventData)
    {

        ItemEventData = eventData;

        // ��������� Transform ������������� �������
        parentTransform = transform.parent;

        // ������������ ������������ ������ � �������� ���������
        transform.SetParent(inventoryRootTransform);

        // ����������� �������� ����� �������� �������� � �������� ����
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out offset
        );




        /*
         * ������ ��������� ���������� ��������������� �������� ���� ����
        */

        transform.SetSiblingIndex(canvasTransform.childCount - 1);

        // �������� Graphics Raycaster
        transform.gameObject.GetComponent<GraphicRaycaster>().enabled = false;

        // �������� ��������
        image_rb.velocity = Vector2.zero;

        is_dragging = true;

    }








    RaycastResult cursorEvent;

    private string[] borders = { "Gun_border", "Item_border" };

    private string[] images = { "GunImage", "ItemImage" };




    public void OnDrag(PointerEventData eventData)
    {

        // ������� ����������� ���
        cursorEvent = eventData.pointerCurrentRaycast;

        // ������� ������, ������� ��� �������� ����
        GameObject TargetObject = cursorEvent.gameObject;




        /*
         * ������� �������� ����, � ������� �������� ������� 
        */


        if (TargetObject != null)
        {
            // �������� ���� TargetObject �������� �������� �����
            CheckCurrentObjectOnBorders(TargetObject, ref current_slot);

            // �������� ���� TargetObject �������� ��������� � ����� 
            CheckCurrentObjectOnImages(TargetObject, ref current_slot);
        }
        else
        {

            // �������� current_slot, ���� ��� �� ��������� ��������
            current_slot = null;
        }




        /*
         *  �������� �� ���������� ������� ���� � �������� ����� ���������  
        */


        // ������� ���� �� ������
        Vector3 mousePosition = Input.mousePosition;

        // �������� �� ���������� ������� ���� � �������� ����� ��������� 
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
            // ������� ���� ������, ���� ��������
            current_slot = GetSlot(current_object);
        }

    }








    private void CheckCurrentObjectOnImages(GameObject current_object, ref GameObject current_slot)
    {
        if (images.Contains(current_object.name))
        {
            // ���� �������� �� �������� ��������� ���������������� ������� 
            if (current_object != transform.gameObject)
            {
                // ������� ���� �������� ������, ���� ��������
                current_slot = GetSlot(current_object);
            }
        }
    }










    private GameObject GetSlot(GameObject current_object)
    {
        // ������� ����, � ������� ��������� ������� ������
        GameObject item_border_parent = current_object.transform.parent.gameObject;

        return item_border_parent;

    }








    
    
    private void Update()
    {
        
        if (is_dragging)
        {
            // �������� ������� �������� �� Canvas
            Vector2 targetPosition = ItemEventData.position - offset + item_hotspot;

            Vector2 direction = (targetPosition - (transform as RectTransform).anchoredPosition).normalized;
            image_rb.AddForce(direction * 10000);
            (transform as RectTransform).anchoredPosition = targetPosition;
        }

    }

    












    public void OnEndDrag(PointerEventData eventData)
    {

        is_dragging = false;


        // ������� Graphics Raycaster
        transform.gameObject.GetComponent<GraphicRaycaster>().enabled = true;

        // ��������� �������� �������
        image_rb.velocity = Vector2.zero;

        // ������������ � �������� ������������� ������� ���, ������� ��� ������������ �� ������� (��� �������, ������� �������������)
        transform.SetParent(parentTransform);




        /*
         *  ���� ��������� ��������� ���� 
        */


        if (current_slot != null && current_slot.tag == "GunSlot")
        {

            // ������� ������� ������, ������� ��������� � ��������
            GameObject current_item = transform.GetChild(0).gameObject;

            // ���� ��� �������
            if (current_item.GetComponent<mag>() != null)
            {

                if (current_slot.transform.GetChild(1).childCount > 0)
                {
                    bool succesLoad = false;

                    Slot current_slot_data = current_slot.GetComponent<Slot>();

                    Camera_main.GetComponent<InventoryManager>().SetMagToGun(current_slot_data, transform.gameObject, out succesLoad);

                    if (succesLoad)
                    {
                        return;
                    }
                    else
                    {
                        Debug.Log("���������� �������� ������ ���������!");
                        transform.position = transform.parent.GetComponent<Slot>().SlotDefaultPosition;
                    }
                }
                
            }









            // ������� ������, ������� �������
            GameObject gun = transform.GetChild(0).gameObject;

            // ������� �������, ������� ������� 
            Item item = gun.GetComponent<FloorItem>().getItem;

            // ������� ����, � ������� ���� �������� ������
            AmmunitionGunSlot slot = current_slot.GetComponent<AmmunitionGunSlot>();

            bool SuccessAddition;

            // ������� ������ � ����� ����
            Camera_main.GetComponent<InventoryManager>().PutWeaponToSlot(item, gun, slot, out SuccessAddition);

            if (!SuccessAddition)
            {

                // ������������ �������� �� �������� �������
                transform.position = transform.parent.GetComponent<Slot>().SlotDefaultPosition;

            }


            return;

        }
        



        /*
         * ���� ��������� ���� Item_UI 
        */


        if (current_slot != null && current_slot.tag == "ItemSlot")
        {

            // ������� ������, ������� �������
            GameObject itemObject = transform.GetChild(0).gameObject;

            // ������� �������, ������� ������� 
            Item item = itemObject.GetComponent<FloorItem>().getItem;

            // ������� ����, � ������� ���� �������� ������
            ItemSlot slot = current_slot.GetComponent<ItemSlot>();

            bool SuccessAddition;

            // ������� ������ � ����� ����
            Camera_main.GetComponent<InventoryManager>().PutItemToSlot(item, itemObject, slot, out SuccessAddition);

            if (!SuccessAddition)
            {
                // ������������ �������� �� �������� �������
                transform.position = transform.parent.GetComponent<Slot>().SlotDefaultPosition;
            }


            return;
        }




        /*
         *  ���� ����������� �� �����������, � ������ ���������� � �������� ��������� 
        */


        if (in_inventory_range == true)
        {
            // ������������ �������� �� �������� �������
            transform.position = transform.parent.GetComponent<Slot>().SlotDefaultPosition;
        }
        else
        {

            // ������� ������������� ������
            GameObject DroppedObj = transform.GetChild(0).gameObject;

            // ������� ������������� �������
            Item item = DroppedObj.GetComponent<FloorItem>().getItem;

            // ������� ���� �� �������� ��������� �������
            GameObject slot = transform.parent.gameObject;
            
            // ������� ������ �����
            Slot item_slot = slot.GetComponent<Slot>();


            if (DroppedObj != null && item != null && item_slot != null)
            {
                bool drop_is_success;

                Camera_main.GetComponent<InventoryManager>().DropItemFromInventory(item, DroppedObj, item_slot, out drop_is_success);

                if (!drop_is_success)
                {
                    // ������������ �������� �� �������� �������
                    transform.position = transform.parent.GetComponent<Slot>().SlotDefaultPosition;
                }

            }
            else 
            {
                // ������������ �������� �� �������� �������
                transform.position = transform.parent.GetComponent<Slot>().SlotDefaultPosition;
            }

        }

        
    }
}