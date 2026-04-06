using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    public GameObject inventoryMenu;

    public InventorySlot[] inventorySlots;

    public PlayerInventory playerInventory;

    public static bool isInventoryOpen;

    void Start()
    {
        isInventoryOpen = false;
        inventoryMenu.SetActive(false);

        // Initialize inventory slots
        SetupInventorySlots();

    }

    // Update is called once per frame
    void Update()
    {
        if (isInventoryOpen)
        {
            UpdateInventoryUI();
        }

        if(Input.GetKeyDown(KeyCode.I)) {
            if(isInventoryOpen) {
                CloseInventory();
            } else {
                OpenInventory();
            }
        }
    }

    private void SetupInventorySlots() {
        Transform inventoryBox = inventoryMenu.transform.Find("InventoryBox");

        inventorySlots = new InventorySlot[12];

        foreach (Transform child in inventoryBox)
        {
            string name = child.name;
            if (name.StartsWith("ItemBox"))
            {
                string numberPart = name.Substring("ItemBox".Length);
                if (int.TryParse(numberPart, out int index))
                {
                    index -= 1;
                    if (index >= 0 && index < inventorySlots.Length)
                    {
                        inventorySlots[index] = child.gameObject.AddComponent<InventorySlot>();
                        inventorySlots[index].Initalize(child.gameObject);
                    }
                }
            }
        }
    }

    private void UpdateInventoryUI() {
        List<Item> items = playerInventory.GetItems();

        for (int i = 0; i < 12; i++)
        {
            if (i < items.Count)
            {
                inventorySlots[i].UpdateSlot(items[i], 1);
            }
            else
            {
                inventorySlots[i].UpdateSlot(null, 0);
            }
        }
    }

    public void OpenInventory() {
        inventoryMenu.SetActive(true);
        isInventoryOpen = true;
    }

    public void CloseInventory() {
        inventoryMenu.SetActive(false);
        isInventoryOpen = false;
    }

}
