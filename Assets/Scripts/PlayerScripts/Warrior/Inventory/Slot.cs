using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        // Получаю начальную позицию Gun Image
        defaultPosition = transform.GetChild(2).transform.position;
    }
}
