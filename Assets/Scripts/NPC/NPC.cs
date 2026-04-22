using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable
{
    [Header("NPC Information/display info")]
    public NPCDialogue dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image portraitImage;

    private int dialogueIndex;
    private bool isTyping, isDialogueActive, isLoadingCombat;

    [Header("Shop info")]
    public bool isShop;
    public Shop shopData;

    [Header("Tutorial Info")]
    public bool isTutorialNPC;

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

        StartCoroutine(TypeLine());

            
    }

    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.SetText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;

        }
        else if(++dialogueIndex < dialogueData.dialogueLines.Length)
        {
            //if another line, type next line
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.SetText("");

        foreach(char letter in dialogueData.dialogueLines[dialogueIndex])
        {
            dialogueText.text += letter;
            SoundEffectManager.PlayVoice(dialogueData.voiceSound, dialogueData.voicePitch);
            yield return new WaitForSecondsRealtime(dialogueData.typingSpeed);
        }

        isTyping = false;

        if (dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSecondsRealtime(dialogueData.autoProgressDelay);
            NextLine();

        }
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        dialogueText.SetText("");
        dialoguePanel.SetActive(false);
        if (!dialogueData.CombatEnemy)
        {
            TimeManager.StartTime(); //only unpause for non-combat NPC's
            if (isShop)
            {
                MenuManager.Instance.OpenMenu("shopmenu", true, this);
            }
        }

        if (isTutorialNPC)
        {
            SceneManager.LoadScene("Center");
        }

        if (dialogueData.CombatEnemy && !isLoadingCombat)
        {
            StartCoroutine(LoadCombatAfterDelay());
        }
    }

    private IEnumerator LoadCombatAfterDelay()
    {
        isLoadingCombat = true;

        //wait for configured delay (so player cna read the last line)
        yield return new WaitForSecondsRealtime(dialogueData.combatTransitionDelay);

        Time.timeScale = 1f;
        TimeManager.StartTime();

        //store the enemy data statically
        CombatData.pendingEnemy = dialogueData.enemyData;
        
        SceneManager.LoadScene(dialogueData.combatSceneName);
    }
}
