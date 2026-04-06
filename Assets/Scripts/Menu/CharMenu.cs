using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMenu : MonoBehaviour
{
    public static CharMenu instance;
    private Item currentItem;
    private PlayerInventory playerInventory;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    public void Open(Item item, Vector3 position)
    {
        currentItem = item;
        transform.position = position;
        gameObject.SetActive(true);
    }

    public void Unequip()
    {
        // Add unequip after making
    }
}
