using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{

    public GameObject itemSlot;
    public GameObject itemIconSlot;
    public Item item;
    public int quantity;

    public void Initalize(GameObject slot)
    {
        this.itemSlot = slot;
        this.itemIconSlot = slot.transform.GetChild(0).gameObject;
    }

    public void UpdateSlot(Item newItem, int newQuantity)
    {
        this.item = newItem;
        this.quantity = newQuantity;

        if (item != null)
        {
            Image img = itemIconSlot.GetComponent<Image>();
            img.sprite = item.icon;
            itemIconSlot.SetActive(true);
        } 
        else
        {
            itemIconSlot.SetActive(false);
        }
    }
}
