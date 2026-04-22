using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ShopMenu : MonoBehaviour
{
    private ShopBox[] shopBoxes;
    public GameObject shopMenu;
    public GameObject shopNPCIcon;
    public GameObject shopNPCDialogue;
    public PlayerInventory playerInventory;
    public ItemDatabase itemDatabase;
    public AlertMenu alertMenuScript;
    private NPC shopNPC;
    private Shop shop;
    private TextMeshProUGUI dialogueText;
    private bool isTyping;
    private Coroutine typingCoroutine;

    private PlayerDataSnapshot testPlayer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (MenuManager.Instance.CheckOpen("shopmenu"))
            {
                CloseShop();
            }
        }
    }

    public void CloseShop()
    {
        MenuManager.Instance.CloseMenu("shopmenu", true);
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
                shopBoxes[i].Price = 10;
                shopBoxes[i].Initalize(shopBoxes[i].Item, shopBoxes[i].Price);
            }
        }

        shopNPCIcon.GetComponent<UnityEngine.UI.Image>().sprite = shopNPC.dialogueData.npcPortrait;
        shopNPCDialogue.GetComponent<TMPro.TextMeshProUGUI>().text = "TEMP TEXT";
        dialogueText = shopNPCDialogue.GetComponent<TMPro.TextMeshProUGUI>();

        testPlayer = PlayerDataSnapshot.CreateDefaultTest();

        StartCoroutine(IdleDialogeLoop());
    }

    public void HandleBuy(ShopBox box)
    {

        /*
        if (!PlayerData.Instance.SpendCoins(box.Price))
        {
            TypeLine(shop.shopDialogue.brokeBoyLine);
            return;
        }
        */
        if(testPlayer.coins < box.Price)
        {
            TypeLine(shop.shopDialogue.brokeBoyLine);
            return;
        }

        string alertMessage = "";
        if (box.Item.itemName == "Mystery Box")
        {
            bool foundUnique = false;
            while(!foundUnique)
            {
                Item randomItem = itemDatabase.GetRandomItem();

                if(!playerInventory.FindItem(randomItem.itemName))
                {
                    if(playerInventory.CheckHasPrerequisite(randomItem.itemName))
                    {
                        playerInventory.AddItem(randomItem);
                        foundUnique = true;

                        alertMessage = "You received a " + randomItem.itemName + "!";
                    }
                }
            }
        } else if(box.Item.itemName == "Very Lucky Box")
        {
            bool foundUnique = false;
            while(!foundUnique)
            {
                Item randomItem = itemDatabase.GetRandomItem();

                if(!playerInventory.FindItem(randomItem.itemName))
                {
                    playerInventory.AddItem(randomItem);
                    foundUnique = true;

                    alertMessage = "You received a " + randomItem.itemName + "!";
                }
            }
        } else if(box.Item.itemName == "Money Bag")
        {
            //PlayerData.Instance.HealHP(PlayerData.Instance.maxHP/2);
            testPlayer.currentHP = Mathf.Min(testPlayer.maxHP, testPlayer.currentHP + testPlayer.maxHP/2);
            alertMessage = "You healed for " + testPlayer.maxHP/2 + " HP!";
        } else if (box.Item.itemName == "Money Chest")
        {
            //PlayerData.Instance.HealHP(PlayerData.Instance.maxHP);
            testPlayer.currentHP = testPlayer.maxHP;
            alertMessage = "You healed for " + testPlayer.maxHP + " HP!";
        }
        alertMenuScript.ShowAlert(alertMessage);
        PlayBuyLine();
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char c in line)
        {
            dialogueText.text += c;
            SoundEffectManager.PlayVoice(shop.shopDialogue.voiceSound, shop.shopDialogue.voicePitch);
            yield return new WaitForSecondsRealtime(shop.shopDialogue.typingSpeed);
        }
        isTyping = false;
    }

    public void PlayRandomLine()
    {
        if (shop.shopDialogue.basicLines.Count == 0) return;

        string line = shop.shopDialogue.basicLines[Random.Range(0, shop.shopDialogue.basicLines.Count)];

        StartTyping(line);
    }

    public void PlayBuyLine()
    {
        if (shop.shopDialogue.buyLines.Count == 0) return;

        string line = shop.shopDialogue.buyLines[Random.Range(0, shop.shopDialogue.buyLines.Count)];

        StartTyping(line);
    }

    void StartTyping(string line)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeLine(line));
    }

    IEnumerator IdleDialogeLoop()
    {
        while (MenuManager.Instance.CheckOpen("shopmenu"))
        {
            PlayRandomLine();
            if (isTyping)
            {
                yield return new WaitUntil(() => !isTyping);
            }
            yield return new WaitForSecondsRealtime(10f);
        }
    }

}
