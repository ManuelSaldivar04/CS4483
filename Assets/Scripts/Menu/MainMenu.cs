using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject TitleText;
    public GameObject LoadGameButton;
    public GameObject ExitGameButton;
    public LevelLoader levelLoader;

    public void StartGame()
    {
        levelLoader.LoadScene("Tutorial");
        GAMESTATEMANAGER.Instance.currentGameState = GAMESTATEMANAGER.GameState.TutorialFirst;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
