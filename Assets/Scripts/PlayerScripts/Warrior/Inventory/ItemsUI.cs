using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemsUI : MonoBehaviour
{
    private RectTransform RectTransdormOfItemsUI;

    private Inventory inventory;

    private void Start()
    {
        RectTransdormOfItemsUI = GameObject.Find("ItemsUI").GetComponent<RectTransform>();

        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    private void SetItems()
    {
        foreach (Item elem in inventory.GetItems)
        {

            GameObject item = new GameObject();

            //item.AddComponent<Image>().sprite = elem.GetIcon;
        }
    }
}
