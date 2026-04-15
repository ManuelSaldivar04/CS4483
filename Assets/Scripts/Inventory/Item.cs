using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public int id;
    public string itemName;
    public Sprite icon;
    public int maxStackSize;
    public bool equippable;
    public string description;
    public int healthChipGain;
    public int betChipGain;
}
