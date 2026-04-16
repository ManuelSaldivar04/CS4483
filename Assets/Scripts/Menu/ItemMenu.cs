using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemMenu : MonoBehaviour
{
    public static ItemMenu instance;
    private Item currentItem;
    private PlayerInventory playerInventory;
    public GameObject ItemImage;
    public GameObject ItemNameText;
    public GameObject ItemDescriptionText;
    public GameObject Button1Text;
    private bool isInventoryMenu;
    void Start()
    {
        ItemImage.SetActive(false);
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

    public void Open(Item item, Vector3 position, PlayerInventory inventory, bool isInventoryMenu)
    {
        currentItem = item;
        playerInventory = inventory;
        transform.position = position;
        this.isInventoryMenu = isInventoryMenu;
        Update();
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (currentItem != null)
        {

            Image img = ItemImage.GetComponent<Image>();
            img.sprite = currentItem.icon;

            img.preserveAspect = true;
            
            ItemImage.SetActive(true);


            ItemNameText.GetComponent<TMPro.TextMeshProUGUI>().text = currentItem.itemName;
            //ItemDescriptionText.GetComponent<TMPro.TextMeshProUGUI>().text = currentItem.description;
            if (isInventoryMenu)
            {
                Button1Text.GetComponent<TMPro.TextMeshProUGUI>().text = "Equip";
            } else
            {
                Button1Text.GetComponent<TMPro.TextMeshProUGUI>().text = "Unequip";
            }
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void UseButton1()
    {
        if (isInventoryMenu)
        {
            Equip();
        } else
        {
            UnEquip();
        }
    }

    private void Equip()
    {
        Debug.Log("Equipping item: " + currentItem.itemName);
        if (!playerInventory.EquipItem(currentItem))
        {
            Debug.Log("Failed to equip item");
        }
        gameObject.SetActive(false);
    }

    private void UnEquip()
    {
        Debug.Log("Unequipping item: " + currentItem.itemName);
        playerInventory.UnequipItem(currentItem);
        gameObject.SetActive(false);
    }

}
