using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/Item Database")]
public class ItemDatabase : ScriptableObject
{
    public Item[] allItems;

    public Item GetRandomItem()
    {
        int randomIndex = Random.Range(0, allItems.Length);
        return allItems[randomIndex];
    }
}
