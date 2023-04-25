using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    /*
     *  Слот пустой ???
    */


    protected bool IsEmpty = true;

    public bool SlotIsEmpty
    { 
        get { return IsEmpty; }
        set { IsEmpty = value; }
    }



    /*
     * Предмет в слоте  
    */


    protected Item item;

    public Item item_in_slot
    {
        get { return item; }
        set { item = value; }
    }



    /*
     * Объект в слоте 
    */



    protected GameObject internal_object;

    public GameObject object_in_slot
    {
        get { return internal_object; }
        set { internal_object = value; }
    }




    /*
     * Дефолтная позиция слота
    */


    private Vector3 defaultPosition;

    public Vector3 SlotDefaultPosition => defaultPosition;






    /*
     * 
     * Методы 
     * слота  
     * 
    */






    public void SetItem(Item item, GameObject gunObj)
    {
        this.item = item;

        internal_object = gunObj;

        IsEmpty = false;
    }

    public void ClearClot()
    {
        item = null;

        internal_object = null;

        IsEmpty = true;
    }







    private void Start()
    {
        /*
        try
        {
            defaultPosition = transform.GetChild(2).transform.position;
            Debug.Log($"Получена дефолтная позиция с индексом 2, координаты равны = {defaultPosition}");
        }
        catch (UnityException)
        {
            defaultPosition = transform.GetChild(1).transform.position;
            Debug.Log($"Получена дефолтная позиция с индексом 1, координаты равны = {defaultPosition}");
        }

        */





        if (transform.gameObject.tag == "ItemSlot")
        {

            defaultPosition = transform.localPosition;
            Debug.Log($"Получена дефолтная позиция с индексом 1, координаты равны = {defaultPosition}");
        }
        else if (transform.gameObject.tag == "GunSlot")
        {
            defaultPosition = transform.GetChild(2).position;
            Debug.Log($"Получена дефолтная позиция с индексом 2, координаты равны = {defaultPosition}");
        }


    }
}
