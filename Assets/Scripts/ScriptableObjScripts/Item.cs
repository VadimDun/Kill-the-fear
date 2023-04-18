using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item : ScriptableObject
{

    private enum ItemType { armor, gun, mag, usable, default_item }

    [SerializeField] private string ItemName;

    public string GetName => ItemName;

    [SerializeField] private Sprite icon;

    public Sprite GetIcon => icon;

    private int MaxItemCount; 
}
