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

    protected bool IsSetted = false;

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



    public GameObject internal_object;

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

    private Face_UI_manager face_UI_manager;




    /*
     * 
     * Методы 
     * слота  
     * 
    */






    public virtual void SetItem(Item item, GameObject itemObj)
    {
        this.item = item;

        internal_object = itemObj;

        DontDestroyOnLoad(itemObj);

        IsEmpty = false;

        face_UI_manager.UpdatePic();

    }

    public void ClearClot()
    {
        item = null;

        internal_object = null;

        IsEmpty = true;

        face_UI_manager.UpdatePic();
    }







    private void Start()
    {


        if (transform.gameObject.tag == "ItemSlot")
        {
            Items_UI_starter items_UI_Starter = transform.parent.gameObject.GetComponent<Items_UI_starter>();

            if (items_UI_Starter != null)
            {
                // Получаем дефолтную позицию объекта из Dictionary
                defaultPosition = items_UI_Starter.GetDefaultPosition(transform.gameObject.name);

            }
        }
        else if (transform.gameObject.tag == "GunSlot")
        {
            defaultPosition = transform.GetChild(1).position;
        }
        else if (transform.gameObject.tag == "EdgedWeaponSlot")
        {
            defaultPosition = transform.GetChild(1).position;
        }


    }



    private void Awake()
    {
        face_UI_manager = GameObject.Find("FaceUI").GetComponent<Face_UI_manager>();
    }


}
