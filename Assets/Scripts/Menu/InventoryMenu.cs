using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    public GameObject inventoryMenu;

    public InventorySlot[] inventorySlots;

    public EquipmentSlot[] equipmentSlots;

    public PlayerInventory playerInventory;

    public GameObject pageText;
    public static bool isInventoryOpen;

    private int currentPage = 0;
    private int NUM_INVENTORY_SLOTS = 12;
    private int NUM_EQUIPMENT_SLOTS = 4;

    void Start()
    {
        // Initialize inventory & equipment slots
        SetupInventorySlots();
        setupEquipmentSlots();
        pageText.GetComponent<TextMeshProUGUI>().text = currentPage.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (MenuManager.Instance.CheckOpen("inventorymenu"))
        {
            UpdateInventoryUI();
            UpdateEquipmentUI();
        }

        if(Input.GetKeyDown(KeyCode.I)) {
            if(MenuManager.Instance.CheckOpen("inventorymenu")) {
                MenuManager.Instance.CloseMenu("inventorymenu", true);
            } else {
                if (MenuManager.Instance.CheckOpen("pausemenu"))
                {
                    return;
                }
                MenuManager.Instance.OpenMenu("inventorymenu", true);
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
            if (i + (currentPage * NUM_INVENTORY_SLOTS) < items.Count)
            {
                inventorySlots[i].UpdateSlot(items[i + currentPage * NUM_INVENTORY_SLOTS], 1, playerInventory);
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

    public void NextPage()
    {
        if (currentPage < playerInventory.GetItems().Count / NUM_INVENTORY_SLOTS)
        {
            currentPage++;
            pageText.GetComponent<TextMeshProUGUI>().text = currentPage.ToString();
            UpdateInventoryUI();
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            pageText.GetComponent<TextMeshProUGUI>().text = currentPage.ToString();
            UpdateInventoryUI();
        }
    }

}
