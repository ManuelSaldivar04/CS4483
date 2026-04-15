using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopBox : MonoBehaviour
{
    private Item item;
    private int stock;
    private int price;
    public PlayerInventory playerInventory;
    public GameObject itemIconSlot;
    public GameObject itemText;
    public GameObject itemButtonText;

    public Item Item { get => item; set => item = value; }
    public int Stock { get => stock; set => stock = value; }
    public int Price { get => price; set => price = value; }

    public void Initalize(Item item, int stock, int price)
    {
        this.item = item;
        this.stock = stock;
        this.price = price;

        Image img = itemIconSlot.GetComponent<Image>();
        img.sprite = item.icon;
        img.preserveAspect = true;
        itemText.GetComponent<TMPro.TextMeshProUGUI>().text = item.itemName + " x" + stock;
        itemButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = "Buy for " + price + " gold";
    }
    public void Buy()
    {
        if (stock > 0)
        {
            stock--;
            playerInventory.AddItem(item);
            if (stock <= 0)
            {
                itemIconSlot.SetActive(false);
                itemText.GetComponent<TMPro.TextMeshProUGUI>().text = "Out of stock";
                itemButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = "";
            }
            else
            {
                itemText.GetComponent<TMPro.TextMeshProUGUI>().text = item.itemName + " x" + stock;
            }
        }
    }

}
