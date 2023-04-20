using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;
using static Gun;

public class AmmunitionGunSlot : MonoBehaviour
{
    private bool IsEmpty = true;

    public bool SlotIsEmpty
    {
        get { return IsEmpty; }
        set { IsEmpty = value; }
    }

    
    private GameObject child;

    public GameObject GunObj
    {
        get { return child; }
        set { child = value; }
    }


    private Item gun;

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
        defaultPosition = transform.GetChild(1).transform.position;
    }

}
