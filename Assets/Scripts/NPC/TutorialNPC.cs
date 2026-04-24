using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialNPC : MonoBehaviour, IInteractable
{
    [Header("NPC Information/display info")]
    public TutorialNPCDialogue dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image portraitImage;
    public string npcID; //

    private int dialogueIndex;
    private bool isTyping, isDialogueActive, isLoadingCombat;

    [Header("Shop info")]
    public Shop shopData;

    [Header("Tutorial Info")]
    private bool foughtOnceTutorial;
    private bool openedShopTutorial;


    private void Awake()
    {
        if(string.IsNullOrEmpty(npcID))
            npcID = gameObject.name;

        if (GAMESTATEMANAGER.Instance.currentGameState == GAMESTATEMANAGER.GameState.TutorialFirst)
        {
            foughtOnceTutorial = false;
            openedShopTutorial = false;
        } else
        {
            foughtOnceTutorial = true;
            openedShopTutorial = false;
        }
    }

    public bool CanInteract()
    {
        return !isDialogueActive && !isLoadingCombat;
    }

    public void Interact()
    {
        //if no dialogue data of the game is paused and no dialogue is active
        if (dialogueData == null || (TimeManager.isTimeStopped && !isDialogueActive)) 
            return;

        if (isDialogueActive)
        {
            NextLine();
        }
        else
        {
            StartDialogue();
        }
    }

    void StartDialogue()
    {
        isDialogueActive = true;
        dialogueIndex = 0;

        nameText.SetText(dialogueData.npcName);
        portraitImage.sprite = dialogueData.npcPortrait;

        dialoguePanel.SetActive(true);
        //pause game
        TimeManager.PauseTime();
        StartCoroutine(TypeLine(GetLines()));
    }

    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.SetText(GetLines()[dialogueIndex]);
            isTyping = false;

        }
        else if(++dialogueIndex < GetLines().Length)
        {
            //if another line, type next line
            StartCoroutine(TypeLine(GetLines()));
        }
        else
        {
            EndDialogue();
        }
    }

    string[] GetLines()
    {
        if (!foughtOnceTutorial)
        {
            return dialogueData.dialogueLines1;
        } else if (GAMESTATEMANAGER.Instance.currentGameState == GAMESTATEMANAGER.GameState.TutorialFirst)
        {
            return dialogueData.dialogueLines2;
        } else if (!openedShopTutorial)
        {
            return dialogueData.dialogueLines3;
        }
        else
        {
            return dialogueData.dialogueLines4;
        }
    }

    IEnumerator TypeLine(string[] lines)
    {
        isTyping = true;
        dialogueText.SetText("");

        foreach(char letter in lines[dialogueIndex])
        {
            dialogueText.text += letter;
            SoundEffectManager.PlayVoice(dialogueData.voiceSound, dialogueData.voicePitch);
            yield return new WaitForSecondsRealtime(dialogueData.typingSpeed);
        }

        isTyping = false;
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        dialogueText.SetText("");
        dialoguePanel.SetActive(false);
        if (!foughtOnceTutorial)
        {
            LoadCombatAfterDelay();
        } else if (foughtOnceTutorial && GAMESTATEMANAGER.Instance.currentGameState == GAMESTATEMANAGER.GameState.TutorialFirst)
        {
            GAMESTATEMANAGER.Instance.currentGameState = GAMESTATEMANAGER.GameState.World;
            SceneManager.LoadScene("Center");
            TimeManager.StartTime();
        } else if (!openedShopTutorial)
        {
            MenuManager.Instance.OpenMenu("shopmenu", true, this);
            openedShopTutorial = true;
        } else if (openedShopTutorial)
        {
            GAMESTATEMANAGER.Instance.currentGameState = GAMESTATEMANAGER.GameState.World;
            SceneManager.LoadScene("Center");
            TimeManager.StartTime();
        }
       
    }

    private IEnumerator LoadCombatAfterDelay()
    {
        isLoadingCombat = true;
        Debug.Log("Loading Combat");

        //wait for configured delay (so player cna read the last line)
        yield return new WaitForSecondsRealtime(dialogueData.combatTransitionDelay);

        //store current scene and player position BEFORE leaving
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            string currentScene = SceneManager.GetActiveScene().name;
            Vector3 PlayerPos = player.transform.position;
            LevelLoader.SetReturnLocation(currentScene, PlayerPos);
            Debug.Log($"Stored return: scene={currentScene}, pos={PlayerPos}");
        }
        else
        {
            Debug.LogError("Player not found when trying to store return location");
        }

        CombatData.sourceNPCID = npcID;
        Time.timeScale = 1f;
        TimeManager.StartTime();

        //store the enemy data statically
        CombatData.pendingEnemy = dialogueData.enemyData;
        if(GameManager.Instance)
            GameManager.Instance.StartCombat();
        SceneManager.LoadScene(dialogueData.combatSceneName);
    }
}
