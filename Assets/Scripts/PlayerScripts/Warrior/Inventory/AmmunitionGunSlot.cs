using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;
using static Gun;
using static UnityEditor.Progress;

public class AmmunitionGunSlot : MonoBehaviour
{
    public bool IsEmpty = true;

    public bool SlotIsEmpty
    {
        get { return IsEmpty; }
        set { IsEmpty = value; }
    }

    
    public GameObject GunObject;

    public GameObject GunObj
    {
        get { return GunObject; }
        set { GunObject = value; }
    }


    public Item gun;

    public Item GunInSlot
    {
        get { return gun; }
        set { gun = value; }
    }

    private Vector3 defaultPosition;

    public Vector3 SlotDefaultPosition => defaultPosition;





    private void Start()
    {
        // Получаю начальную позицию Gun Image
        defaultPosition = transform.GetChild(2).transform.position;
    }

    public void SetItem(Item item, GameObject gunObj)
    { 
        gun = item;

        GunObject = gunObj;

        IsEmpty = false;
    }

    public void ClearClot()
    {
        gun = null;

        GunObject = null;

        IsEmpty = true;
    }

}
