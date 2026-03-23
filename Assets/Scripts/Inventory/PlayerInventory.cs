using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    List<Item> items;

    public void Start()
    {
        items = new List<Item>();
    }

    public List<Item> GetItems()
    {
        return items;
    }

    public void AddItem(Item item)
    {
        items.Add(item);
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
