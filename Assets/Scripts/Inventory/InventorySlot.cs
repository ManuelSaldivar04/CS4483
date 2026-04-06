using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{

    public GameObject itemSlot;
    public GameObject itemIconSlot;
    public Item item;
    public int quantity;

    public void Initalize(GameObject slot)
    {
        this.itemSlot = slot;
        this.itemIconSlot = slot.transform.GetChild(0).gameObject;
        Debug.Log("Initialized inventory slot");
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

    public void OnPointerClick(PointerEventData eventData)
    {
        /*
            Uncomment after testing
        if (item == null)
        {
            return;
        }
        */

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Left clicked on menu item");

            Vector3 mousePosition = Input.mousePosition;
            mousePosition.y -= 20;

            ActionMenu.instance.Open(item, mousePosition);
        }
    }
}
