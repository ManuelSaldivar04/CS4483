using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if (MenuManager.Instance.menus["pausemenu"].isOpen) {
                MenuManager.Instance.CloseMenu("pausemenu", true);
            } else {
                if (MenuManager.Instance.menus["controlmenu"].isOpen) {
                    return;
                }
                MenuManager.Instance.OpenMenu("pausemenu", true);
            }
        }
    }

    public void PauseGame() 
    {
        MenuManager.Instance.OpenMenu("pausemenu", true);
    }

    public void ResumeGame()
    {
        MenuManager.Instance.CloseMenu("pausemenu", true);
    }

    public void OpenControlMenu()
    {
        MenuManager.Instance.CloseMenu("pausemenu", true);
        MenuManager.Instance.OpenMenu("controlmenu", true);
    }

    public void GoToMainMenu() 
    {
        MenuManager.Instance.CloseMenu("pausemenu", true);
        SceneManager.LoadScene("MainMenu");
    }
}
