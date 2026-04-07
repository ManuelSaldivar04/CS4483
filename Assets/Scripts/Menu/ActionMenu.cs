using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionMenu : MonoBehaviour
{
    public static ActionMenu instance;
    private Item currentItem;
    private PlayerInventory playerInventory;

    void Start()
    {
        gameObject.SetActive(false);
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    public void Open(Item item, Vector3 position, PlayerInventory inventory)
    {
        currentItem = item;
        playerInventory = inventory;
        transform.position = position;
        gameObject.SetActive(true);
        Debug.Log(item.itemName);
    }

    public void Equip()
    {
        Debug.Log("Equipping item: " + currentItem.itemName);
        if (!playerInventory.EquipItem(currentItem))
        {
            Debug.Log("Failed to equip item");
        }
        gameObject.SetActive(false);
    }
}
