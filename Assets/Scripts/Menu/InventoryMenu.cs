using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMenu : MonoBehaviour
{

    public PauseMenu pauseMenu;
    public GameObject inventoryMenu;

    public static bool isInventoryOpen;

    // Start is called before the first frame update
    void Start()
    {
        inventoryMenu.SetActive(false);
        isInventoryOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I)) {
            if(isInventoryOpen) {
                CloseInventory();
            } else {
                OpenInventory();
            }
        }
    }

    public void OpenInventory() {
        if (PauseMenu.isPaused) {
            return;
        }
        inventoryMenu.SetActive(true);
        isInventoryOpen = true;
    }

    public void CloseInventory() {
        inventoryMenu.SetActive(false);
        isInventoryOpen = false;
    }


}
