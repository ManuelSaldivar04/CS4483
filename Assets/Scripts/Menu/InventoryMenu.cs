using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    public GameObject inventoryMenu;

    public InventorySlot[] inventorySlots;

    public EquipmentSlot[] equipmentSlots;

    public PlayerInventory playerInventory;

    public static bool isInventoryOpen;

    private int NUM_INVENTORY_SLOTS = 12;
    private int NUM_EQUIPMENT_SLOTS = 4;

    void Start()
    {
        isInventoryOpen = false;
        inventoryMenu.SetActive(false);

        // Initialize inventory & equipment slots
        SetupInventorySlots();
        setupEquipmentSlots();

    }

    // Update is called once per frame
    void Update()
    {
        if (isInventoryOpen)
        {
            UpdateInventoryUI();
            UpdateEquipmentUI();
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

        inventorySlots = new InventorySlot[NUM_INVENTORY_SLOTS];

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

    private void setupEquipmentSlots() {
        Transform equipmentBox = inventoryMenu.transform.Find("CharacterBox");

        equipmentSlots = new EquipmentSlot[NUM_EQUIPMENT_SLOTS];

        foreach (Transform child in equipmentBox)
        {
            string name = child.name;
            if (name.StartsWith("EquipmentBox"))
            {
                string numberPart = name.Substring("EquipmentBox".Length);
                if (int.TryParse(numberPart, out int index))
                {
                    index -= 1;
                    if (index >= 0 && index < equipmentSlots.Length)
                    {
                        equipmentSlots[index] = child.gameObject.AddComponent<EquipmentSlot>();
                        equipmentSlots[index].Initalize(child.gameObject);
                    }
                }
            }
        }
    }

    private void UpdateInventoryUI() {
        List<Item> items = playerInventory.GetItems();

        for (int i = 0; i < NUM_INVENTORY_SLOTS; i++)
        {
            if (i < items.Count)
            {
                inventorySlots[i].UpdateSlot(items[i], 1, playerInventory);
            }
            else
            {
                inventorySlots[i].UpdateSlot(null, 0, playerInventory);
            }
        }
    }

    private void UpdateEquipmentUI() {
        List<Item> equippedItems = playerInventory.GetEquippedItems().ToList();

        for (int i = 0; i < NUM_EQUIPMENT_SLOTS; i++)
        {
            if (i < equippedItems.Count)
            {
                equipmentSlots[i].UpdateSlot(equippedItems[i], playerInventory);
            }
            else
            {
                equipmentSlots[i].UpdateSlot(null, playerInventory);
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
