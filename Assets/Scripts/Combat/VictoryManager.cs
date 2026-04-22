using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    public Animator[] animators;

    void Start()
    {
        SoundEffectManager.Play("ConfettiPop");
        foreach (Animator anim in animators)
        {
            anim.enabled = true;
            anim.Play("ConfettiPop");
        }
    }

    public void buttonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
}
