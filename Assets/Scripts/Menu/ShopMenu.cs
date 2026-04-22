using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMenu : MonoBehaviour
{
    private ShopBox[] shopBoxes;
    public GameObject shopMenu;
    public GameObject shopNPCIcon;
    public GameObject shopNPCDialogue;
    private NPC shopNPC;
    private Shop shop;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (MenuManager.Instance.CheckOpen("shopmenu"))
            {
                MenuManager.Instance.CloseMenu("shopmenu", true);
            }
        }
    }

    public void Initalize(NPC shopNPC)
    {
        this.shopNPC = shopNPC;
        shop = shopNPC.shopData;
        Transform shopPanel = shopMenu.transform.Find("ShopPanel");
        shopBoxes = shopPanel.GetComponentsInChildren<ShopBox>();
        for (int i = 0; i < shopBoxes.Length; i++)
        {
            if (i < shop.itemsForSale.Count)
            {
                shopBoxes[i].Item = shop.itemsForSale[i];
                shopBoxes[i].Stock = 5;
                shopBoxes[i].Price = 10;
                shopBoxes[i].Initalize(shopBoxes[i].Item, shopBoxes[i].Stock, shopBoxes[i].Price);
            }
        }

        shopNPCIcon.GetComponent<UnityEngine.UI.Image>().sprite = shopNPC.dialogueData.npcPortrait;
        shopNPCDialogue.GetComponent<TMPro.TextMeshProUGUI>().text = shopNPC.dialogueData.npcName + ": TEMP TEXT";
    }

    public void BuyItem()
    {
        
    }

}
