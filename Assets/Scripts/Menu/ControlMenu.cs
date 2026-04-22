using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMenu : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (!MenuManager.Instance.menus["pausemenu"].isOpen) {
                if (MenuManager.Instance.menus["controlmenu"].isOpen) {
                    ReturntoPauseMenu();
                }
            }
        }
    }
    public void OpenControlMenu()
    {
        MenuManager.Instance.OpenMenu("controlmenu", true);
    }

    public void ReturntoPauseMenu()
    {
        MenuManager.Instance.CloseMenu("controlmenu", false);
        MenuManager.Instance.OpenMenu("pausemenu", true);
    }
}
