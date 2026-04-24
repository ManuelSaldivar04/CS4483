using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject TitleText;
    public GameObject Slot1Button;
    public GameObject Slot2Button;
    public GameObject Slot3Button;
    public GameObject LoadGameButton;
    public GameObject ExitGameButton;
    public LevelLoader levelLoader;

    public void StartGame()
    {
        Slot1Button.SetActive(true);
        Slot2Button.SetActive(true);
        Slot3Button.SetActive(true);
        LoadGameButton.SetActive(false);
        ExitGameButton.SetActive(false);

        TitleText.GetComponent<TMPro.TextMeshProUGUI>().text = "Select a Slot to Load";
    }
    
    public void LoadSlot1()
    {
        // Load the game from slot 1
        Debug.Log("Loading Slot 1...");
        levelLoader.LoadScene("Tutorial");
        GAMESTATEMANAGER.Instance.currentGameState = GAMESTATEMANAGER.GameState.TutorialFirst;
    }

    public void LoadSlot2()
    {
        // Load the game from slot 2
        Debug.Log("Loading Slot 2...");
        levelLoader.LoadScene("Tutorial");
        GAMESTATEMANAGER.Instance.currentGameState = GAMESTATEMANAGER.GameState.TutorialFirst;
    }

    public void LoadSlot3()
    {
        // Load the game from slot 3
        Debug.Log("Loading Slot 3...");
        levelLoader.LoadScene("Tutorial");
        GAMESTATEMANAGER.Instance.currentGameState = GAMESTATEMANAGER.GameState.TutorialFirst;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
