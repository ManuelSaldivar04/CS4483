using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CursedCard : MonoBehaviour
{
    public UIManager ui;

    public Sprite[] cards;
    public Image[] cardsImage;

    public TextMeshProUGUI text;
    public Button[] buttons;

    public int joker;

    public void beginCursedCard()
    {
        text.text = "Select A Card";
        joker = UnityEngine.Random.Range(0, 5);

        for (int i = 0; i < 5; i++)
        {
            cardsImage[i].sprite = cards[0];
            buttons[i].interactable = true;
        }
    }

    public void cardSelected(int x)
    {
        StartCoroutine(checkCard(x));
    }

    IEnumerator checkCard(int x)
    {
        for (int i = 0; i < 5; i++)
        {
            buttons[i].interactable = false;
        }

        if (x == joker)
        {
            cardsImage[x].sprite = cards[1];
            text.text = "YOU LOSE";
            yield return new WaitForSeconds(2f);
            ui.hideGame();
            GameManager.Instance.loseHand();
        }

        else
        {
            cardsImage[x].sprite = cards[UnityEngine.Random.Range(2, 6)];
            text.text = "YOU WIN";
            yield return new WaitForSeconds(2f);
            ui.hideGame();
            GameManager.Instance.winHand(0.25f);
        }
    }
}
