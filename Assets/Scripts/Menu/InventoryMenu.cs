using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    public GameObject inventoryMenu;
    public TimeManager timeManager;
    // Start is called before the first frame update
    public static bool isInventoryOpen;
    void Start()
    {
        isInventoryOpen = false;
        inventoryMenu.SetActive(false);
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
        inventoryMenu.SetActive(true);
        timeManager.StopTime();
        isInventoryOpen = true;
    }

    public void CloseInventory() {
        inventoryMenu.SetActive(false);
        timeManager.ResumeTime();
        isInventoryOpen = false;
    }


}
