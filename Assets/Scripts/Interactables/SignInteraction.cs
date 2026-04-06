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
        //TODO: pause game using the PauseGame script
        informationPanel.SetActive(true);
        
    }

    public void EndDisplay()
    {
        //TODO: unpause game using PauseGame script
        informationPanel.SetActive(false);
    }
  
}
