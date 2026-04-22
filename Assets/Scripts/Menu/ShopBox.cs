using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopBox : MonoBehaviour
{
    private Item item;
    private int price;
    public GameObject itemIconSlot;
    public GameObject itemText;
    public GameObject itemButtonText;
    public ShopMenu shopMenuScript;

    public Item Item { get => item; set => item = value; }
    public int Price { get => price; set => price = value; }

    public void Initalize(Item item, int price)
    {
        this.item = item;
        this.price = price;

        Image img = itemIconSlot.GetComponent<Image>();
        img.sprite = item.icon;
        img.preserveAspect = true;
        itemText.GetComponent<TMPro.TextMeshProUGUI>().text = item.itemName;
        itemButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = "Buy for " + price + " gold";
    }
    public void Buy()
    {
        shopMenuScript.HandleBuy(this);
    }

}
