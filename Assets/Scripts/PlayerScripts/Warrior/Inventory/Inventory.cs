using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Предмет который подобрали
    [SerializeField] private Item SelectedItem;

    private List<Item> InventoryItems = new List<Item>();

    public List<Item> GetItems => InventoryItems;

    public void AddItem()
    {
        if (SelectedItem != null)
        {
            InventoryItems.Add(SelectedItem);
            SelectedItem = null;
        }
    }

    private void Update()
    {

    }

}
