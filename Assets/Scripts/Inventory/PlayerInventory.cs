using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    int MAX_EQUIPPED_ITEMS = 4;
    List<Item> items;
    Item[] equippedItems;

    public void Start()
    {
        items = new List<Item>();
        equippedItems = new Item[MAX_EQUIPPED_ITEMS];
    }

    public List<Item> GetItems()
    {
        return items;
    }

    public Item[] GetEquippedItems()    
    {
        return equippedItems;
    }

    public void AddItem(Item item)
    {
        items.Add(item);
    }

    public bool EquipItem(Item item)
    {
        for (int i = 0; i < MAX_EQUIPPED_ITEMS; i++)
        {
            if (equippedItems[i] == null)
            {
                equippedItems[i] = item;
                return true;
            }
        }

        Debug.Log("No available slots to equip item");
        return false;
    }

    public void UnequipItem(Item item)
    {
        for (int i = 0; i < MAX_EQUIPPED_ITEMS; i++)
        {
            if (equippedItems[i] == item)
            {
                equippedItems[i] = null;
                return;
            }
        }

        Debug.Log("Item not found in equipped items");
    }

    public bool HasItem(Item item)
    {
        return items.Contains(item);
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
    }
}
