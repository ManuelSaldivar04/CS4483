using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class GAMESTATEMANAGER : MonoBehaviour
{
    public static GAMESTATEMANAGER Instance;

    public enum GameState
    {
        MainMenu,
        TutorialFirst,
        World,
        Combat
    }

    public GameState currentGameState = GameState.MainMenu;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
