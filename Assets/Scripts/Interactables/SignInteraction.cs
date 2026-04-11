using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SignInteraction : MonoBehaviour, IInteractable
{
    // VARIABLES
    public GameObject informationPanel;
    private bool isDialogueActive;
    public bool CanInteract()
    {
        return !isDialogueActive;
    }

    public void Interact()
    {
        DisplayInformation();
    }

    void DisplayInformation()
    {
        TimeManager.PauseTime();
        informationPanel.SetActive(true);
        
    }

    public void EndDisplay()
    {
        TimeManager.StartTime();
        informationPanel.SetActive(false);
    }
  
}
