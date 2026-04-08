using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TMP_InputField wagerInputField;

    public GameObject gamePrompt;
    public GameObject actionPrompt;
    public GameObject wagerPrompt;
    public GameObject gameButtons;
    public GameObject actionButtons;
    public GameObject wagerButtons;

    public GameObject bj;
    public GameObject roulette;
    public GameObject slots;
    public GameObject dice;

    public void Start()
    {
        gamePrompt.SetActive(true);
        gameButtons.SetActive(true);
        actionPrompt.SetActive(false);
        actionButtons.SetActive(false);
        wagerPrompt.SetActive(false);
        wagerButtons.SetActive(false);
        bj.SetActive(false);
        //roulette.SetActive(false);
        //slots.SetActive(false);
        //dice.SetActive(false);
    }

    public void getGame(int x)
    {
        GameManager.Instance.setGame(x);
        gamePrompt.SetActive(false);
        gameButtons.SetActive(false);
        actionPrompt.SetActive(true);
        actionButtons.SetActive(true);
    }

    public void getAction(int x)
    {
        GameManager.Instance.setAction(x);
        actionPrompt.SetActive(false);
        actionButtons.SetActive(false);
        wagerPrompt.SetActive(true);
        wagerButtons.SetActive(true);
    }

    public void getWager()
    {
        if(int.TryParse(wagerInputField.text, out int x))
        {
            GameManager.Instance.setWager(x);
            wagerPrompt.SetActive(false);
            wagerButtons.SetActive(false);

            switch (GameManager.Instance.getGame())
            {
                case 0:
                    bj.SetActive(true);
                    BJ z = new BJ();
                    z.beginBJ();
                    break;

                case 1:
                    roulette.SetActive(true);
                    break;

                case 2:
                    slots.SetActive(true);
                    break;

                case 3:
                    dice.SetActive(true);
                    break;
            }
        }
          
    }

}
