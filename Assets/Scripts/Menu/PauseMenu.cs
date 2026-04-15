using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public MenuManager menuManager;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if (menuManager.menus["pausemenu"].isOpen) {
                menuManager.CloseMenu("pausemenu", true);
            } else {
                menuManager.OpenMenu("pausemenu", true);
            }
        }
    }

    public void PauseGame() 
    {
        menuManager.OpenMenu("pausemenu", true);
    }

    public void ResumeGame()
    {
        menuManager.CloseMenu("pausemenu", true);
    }

    public void OpenControlMenu()
    {
        menuManager.CloseMenu("pausemenu", true);
        menuManager.OpenMenu("controlmenu", true);
    }

    public void GoToMainMenu() 
    {
        menuManager.CloseMenu("pausemenu", true);
        SceneManager.LoadScene("MainMenu");
    }
}
