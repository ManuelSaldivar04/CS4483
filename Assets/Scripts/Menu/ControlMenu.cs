using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMenu : MonoBehaviour
{
    public MenuManager menuManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (!menuManager.menus["pausemenu"].isOpen) {
                if (menuManager.menus["controlmenu"].isOpen) {
                    ReturntoPauseMenu();
                }
            }
        }
    }
    public void OpenControlMenu()
    {
        menuManager.OpenMenu("controlmenu", true);
    }

    public void ReturntoPauseMenu()
    {
        menuManager.CloseMenu("controlmenu", false);
        menuManager.OpenMenu("pausemenu", true);
    }
}
