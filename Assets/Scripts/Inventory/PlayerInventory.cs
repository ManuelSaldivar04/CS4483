using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    int MAX_EQUIPPED_ITEMS = 4;
    List<Item> items;
    Item[] equippedItems;

    private Dictionary<string, string> exclusiveItems = new Dictionary<string, string>()
    {
        {"Armour +1", "Armour"},
        {"Armour +2", "Armour +1"},
        {"Armour +3", "Armour +2"},
        {"Bet Slip +1", "Bet Slip"},
        {"Bet Slip +2", "Bet Slip +1"},
        {"Bet Slip +3", "Bet Slip +2"},
        {"Gold Coin +1", "Gold Coin"},
        {"Gold Coin +2", "Gold Coin +1"},
        {"Gold Coin +3", "Gold Coin +2"},
        {"Golden Chip +1", "Golden Chip"},
        {"Golden Chip +2", "Golden Chip +1"},
        {"Golden Chip +3", "Golden Chip +2"},
    };

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

    public bool FindItem(string itemName)
    {
        foreach (Item item in items)
        {
            if (item.itemName == itemName)
            {
                Debug.Log("Found item: " + itemName);
                return true;
            }
        }
        Debug.Log("Item not found: " + itemName);
        return false;
    }

    public bool CheckHasPrerequisite(string itemName)
    {
        if (exclusiveItems.ContainsKey(itemName))
        {
            if (!FindItem(exclusiveItems[itemName]))
            {
                Debug.Log("Missing prerequisite for " + itemName + ": " + exclusiveItems[itemName]);
                return false;
            }
        }
        return true;
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
