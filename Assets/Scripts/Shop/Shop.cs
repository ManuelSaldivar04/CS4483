using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShop", menuName = "Shop/Shop")]
public class Shop : ScriptableObject
{
    private const int MAX_ITEMS = 4;
    public List<Item> itemsForSale = new List<Item>(MAX_ITEMS);
    public ShopDialogue shopDialogue;
}
