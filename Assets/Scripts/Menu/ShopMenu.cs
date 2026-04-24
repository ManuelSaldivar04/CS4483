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
    public GameObject playerCoinsText;
    public PlayerInventory playerInventory;
    public ItemDatabase itemDatabase;
    public AlertMenu alertMenuScript;
    private NPC shopNPC;
    private TutorialNPC tutorialShopNPC;
    private Shop shop;
    private TextMeshProUGUI dialogueText;
    private bool isTyping;
    private Coroutine typingCoroutine;

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
                shopBoxes[i].Price = shopBoxes[i].Item.price;
                shopBoxes[i].Initalize(shopBoxes[i].Item, shopBoxes[i].Price);
            }
        }

        shopNPCIcon.GetComponent<UnityEngine.UI.Image>().sprite = shopNPC.dialogueData.npcPortrait;
        shopNPCDialogue.GetComponent<TMPro.TextMeshProUGUI>().text = "TEMP TEXT";
        dialogueText = shopNPCDialogue.GetComponent<TMPro.TextMeshProUGUI>();

        if (PlayerData.Instance != null)
        {
            playerCoinsText.GetComponent<TMPro.TextMeshProUGUI>().text = "Your coins: " + PlayerData.Instance.coins;
        } else
        {
            playerCoinsText.GetComponent<TMPro.TextMeshProUGUI>().text = "Your coins: ?";
        }

        StartCoroutine(IdleDialogeLoop());
    }

    public void Initalize(TutorialNPC shopNPC)
    {
        this.tutorialShopNPC = shopNPC;
        shop = shopNPC.shopData;
        Transform shopPanel = shopMenu.transform.Find("ShopPanel");
        shopBoxes = shopPanel.GetComponentsInChildren<ShopBox>();
        for (int i = 0; i < shopBoxes.Length; i++)
        {
            if (i < shop.itemsForSale.Count)
            {
                shopBoxes[i].Item = shop.itemsForSale[i];
                shopBoxes[i].Price = shopBoxes[i].Item.price;
                shopBoxes[i].Initalize(shopBoxes[i].Item, shopBoxes[i].Price);
            }
        }

        shopNPCIcon.GetComponent<UnityEngine.UI.Image>().sprite = tutorialShopNPC.dialogueData.npcPortrait;
        shopNPCDialogue.GetComponent<TMPro.TextMeshProUGUI>().text = "TEMP TEXT";
        dialogueText = shopNPCDialogue.GetComponent<TMPro.TextMeshProUGUI>();

        if (PlayerData.Instance != null)
        {
            playerCoinsText.GetComponent<TMPro.TextMeshProUGUI>().text = "Your coins: " + PlayerData.Instance.coins;
        } else
        {
            playerCoinsText.GetComponent<TMPro.TextMeshProUGUI>().text = "Your coins: ?";
        }

        StartCoroutine(IdleDialogeLoop());
    }

    public void HandleBuy(ShopBox box)
    {
        if (PlayerData.Instance == null)
        {
            StopAllCoroutines();
            StartCoroutine(TypeLine(shop.shopDialogue.brokeBoyLine));
            return;
        }
        if (!PlayerData.Instance.SpendCoins(box.Price))
        {
            StopAllCoroutines();
            StartCoroutine(TypeLine(shop.shopDialogue.brokeBoyLine));
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
            PlayerData.Instance.HealHP(PlayerData.Instance.maxHP/2);
            alertMessage = "You healed for " + PlayerData.Instance.maxHP/2 + " HP!";
        } else if (box.Item.itemName == "Money Chest")
        {
            PlayerData.Instance.HealHP(PlayerData.Instance.maxHP);
            alertMessage = "You healed for " + PlayerData.Instance.maxHP + " HP!";
        } else if (box.Item.itemName == "Increase Attack")
        {
            PlayerData.Instance.bonusMaxChips += 50;
            alertMessage = "Your attack chip count increased by 50!";
        } else if (box.Item.itemName == "Increase Health")
        {
            PlayerData.Instance.bonusMaxHP += 50;
            alertMessage = "Your maximum health chip count increased by 50!";
        } else if (box.Item.itemName == "Increase Attack Regen")
        {
            PlayerData.Instance.bonusChipRegen += 10;
            alertMessage = "Your attack chip regeneration increased by 10!";
        } else if (box.Item.itemName == "Increase Shield")
        {
            PlayerData.Instance.armour += 5;
            alertMessage = "Your armour increased by 5!";
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
