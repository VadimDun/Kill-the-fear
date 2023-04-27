using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : Slot
{

    public override void SetItem(Item item, GameObject itemObj)
    {
        this.item = item;

        internal_object = itemObj;

        IsEmpty = false;

        internal_object.GetComponent<Collider2D>().enabled = true;

        Debug.Log("Метод вызвался, коллайдер был установлен");

    }

}
