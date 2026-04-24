using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public int id;
    public string itemName;
    public Sprite icon;
    public bool equippable;
    public string description;
    public int bonusHealthChip;
    public int bonusAttackChip;
    public int regenAttackChip;
    public int bonusArmour;
}
