using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatMenu : MonoBehaviour
{

    public GameObject cheatMenu;
    public ItemDatabase itemDatabase;
    public PlayerInventory playerInventory;
    public bool isCheatMenuOpen;
    // Start is called before the first frame update
    void Start()
    {
        cheatMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C)) {
            if(isCheatMenuOpen) {
                CloseCheatMenu();
            } else {
                OpenCheatMenu();
            }
        }
    }

    public void OpenCheatMenu() {
        cheatMenu.SetActive(true);
        isCheatMenuOpen = true;
    }

    public void CloseCheatMenu() {
        cheatMenu.SetActive(false);
        isCheatMenuOpen = false;
    }

    public void CheatItems()
    {
        foreach (Item item in itemDatabase.allItems)
        {
            playerInventory.AddItem(item);
            Debug.Log(playerInventory.GetItems());
        }
    }
}
