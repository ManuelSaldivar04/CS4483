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

    private bool FindEquippedItem(string itemName)
    {
        foreach (Item item in equippedItems)
        {
            if (item != null && item.itemName == itemName)
            {
                Debug.Log("Found equipped item: " + itemName);
                return true;
            }
        }
        Debug.Log("Equipped item not found: " + itemName);
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
        if (FindEquippedItem(item.itemName))
        {
            Debug.Log("Item already equipped: " + item.itemName);
            return false;
        }

        for (int i = 0; i < MAX_EQUIPPED_ITEMS; i++)
        {
            if (equippedItems[i] == null)
            {
                if (PlayerData.Instance == null)
                    continue;
                    
                equippedItems[i] = item;
                if (PlayerData.Instance != null)
                {
                    for (int j = 0; j < PlayerData.Instance.items.Length; j++)
                    {
                        if (PlayerData.Instance.items[j] == 0)
                        {
                            PlayerData.Instance.items[j] = item.id;
                            PlayerData.Instance.bonusMaxHP += item.bonusHealthChip;
                            PlayerData.Instance.bonusMaxChips += item.bonusAttackChip;
                            PlayerData.Instance.bonusChipRegen += item.regenAttackChip;
                            PlayerData.Instance.armour += item.bonusArmour;
                            Debug.Log("Equipped item: " + item.itemName);
                            break;
                        }
                    }
                }
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
                if (PlayerData.Instance != null)
                {
                    for (int j = 0; j < PlayerData.Instance.items.Length; j++)
                    {
                        if (PlayerData.Instance.items[j] == item.id)
                        {
                            PlayerData.Instance.items[j] = 0;
                            PlayerData.Instance.bonusMaxHP -= item.bonusHealthChip;
                            PlayerData.Instance.bonusMaxChips -= item.bonusAttackChip;
                            PlayerData.Instance.bonusChipRegen -= item.regenAttackChip;
                            PlayerData.Instance.armour -= item.bonusArmour;
                            Debug.Log("Unequipped item: " + item.itemName);
                            return;
                        }
                    }
                }
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
