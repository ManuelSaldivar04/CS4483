using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour
{
    public DiceAnimation anim;
    public UIManager ui;

    public Sprite [] diceFaces;
    public Image gameDiceImage;

    public Image dealerDiceImage;
    public GameObject dealerDiceObject;
    public Image [] playerDiceImage;
    public GameObject [] playerDiceObject;

    public Button roll;
    public Button stop;
    public Button add;

    public TextMeshProUGUI resultText;
    public TextMeshProUGUI multText;

    public int playerDiceAmount;
    public int [] playerNum;
    public int dealerNum;
    public float mult;
    public int dieNum;
    public float [] multValues;

    public void beginDice()
    {
        anim.enabled = false;
        roll.interactable = false;
        stop.interactable = false;
        add.interactable = false;
        for (int i = 0; i < 5; i++)
        {
            playerDiceObject[i].SetActive(false);
            playerNum[i] = -1;
        }

        dealerDiceObject.SetActive(false);
        playerDiceAmount = 0;
        mult = 0;
        multText.text = "";

        gameDiceImage.sprite = diceFaces[2];

        // step 1 - roll player die
        resultText.text = "Rolling For Player Number";
        anim.RollDice(() =>
        {
            playerNum[playerDiceAmount] = UnityEngine.Random.Range(0, 6);
            gameDiceImage.sprite = diceFaces[playerNum[playerDiceAmount]];
            playerDiceImage[playerDiceAmount].sprite = diceFaces[playerNum[playerDiceAmount]];
            playerDiceObject[playerDiceAmount].SetActive(true);
            playerDiceAmount++;
            StartCoroutine(getDealerNum());


        });

    }

    public void rollDie()
    {
        dieNum = UnityEngine.Random.Range(0, 6);
        rollDie(dieNum);

    }
    
    public void rollDie(int num)
    {
        disableButtons();

        anim.RollDice( () =>
        {
            gameDiceImage.sprite = diceFaces[num];

            if (num == dealerNum)
            {
                StartCoroutine(lose());
                return;
            }

            if (playerNum.Contains(num))
            {
                mult += multValues[playerDiceAmount - 1];
                resultText.text = "+" + multValues[playerDiceAmount - 1] + "X\nRoll Or Stop";
                multText.text = mult + "X";
                roll.interactable = true;
                stop.interactable = true;
            }

            else
            {
                resultText.text = "Add, Roll, Or Stop";
                roll.interactable = true;
                stop.interactable = true;
                add.interactable = true;
            }
        });
    }

    public void disableButtons()
    {
        roll.interactable = false;
        stop.interactable = false;
        add.interactable = false;
    }

    IEnumerator getDealerNum()
    {
        yield return new WaitForSeconds(2f);

        resultText.text = "Rolling For Dealer Number";
        anim.RollDice(() =>
        {
            dealerNum = UnityEngine.Random.Range(0, 6);
            while (dealerNum == playerNum[0])
                dealerNum = UnityEngine.Random.Range(0, 6);

            gameDiceImage.sprite = diceFaces[dealerNum];
            dealerDiceImage.sprite = diceFaces[dealerNum];
            dealerDiceObject.SetActive(true);

            resultText.text = "Roll the die!";
            multText.text = mult + "X";
            roll.interactable = true;
        });
    }

    IEnumerator lose()
    {
        resultText.text = "YOU LOSE";
        roll.interactable = false;
        stop.interactable = false;
        add.interactable = false;

        yield return new WaitForSeconds(2f);
        ui.hideGame();
        GameManager.Instance.loseHand();
    }

    public void stopPressed()
    {
        StartCoroutine(win());
    }

    IEnumerator win()
    {
        resultText.text = mult + "X WIN!";
        roll.interactable = false;
        stop.interactable = false;
        add.interactable = false;

        yield return new WaitForSeconds(2f);
        ui.hideGame();
        GameManager.Instance.winHand(mult);
    }

    public void addPressed()
    {
        playerNum[playerDiceAmount] = dieNum;
        playerDiceImage[playerDiceAmount].sprite = diceFaces[playerNum[playerDiceAmount]];
        playerDiceObject[playerDiceAmount].SetActive(true);
        playerDiceAmount++;
        rollDie();
    }
}
