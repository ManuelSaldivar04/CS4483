using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour, IPointerClickHandler
{
    public GameObject equipmentSlot;
    public GameObject itemIconSlot;
    public Item item;
    private PlayerInventory playerInventory;

    public void Initalize(GameObject slot)
    {
        this.equipmentSlot = slot;
        this.itemIconSlot = slot.transform.GetChild(0).gameObject;
    }

    public void UpdateSlot(Item newItem, PlayerInventory inventory)
    {
        this.item = newItem;
        this.playerInventory = inventory;

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
        if (item == null)
        {
            return;
        }

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Left clicked on menu item");

            Vector3 mousePosition = Input.mousePosition;
            mousePosition.y -= 20;

            CharMenu.instance.Open(item, mousePosition, playerInventory);
        }
    }
}
