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

    public int[] joker;

    public bool retry;

    public void beginCursedCard()
    {
        retry = true;
        text.text = "Select A Card";
        joker[0] = UnityEngine.Random.Range(0, 5);

        for (int i = 0; i < PlayerData.Instance.items.Length; i++)
        {
            if (PlayerData.Instance.items[i] == 26)
            {
                joker[1] = UnityEngine.Random.Range(0, 5);

                while (joker[1] == joker[0])
                    joker[1] = UnityEngine.Random.Range(0, 5);
            }
        }

        for (int j = 0; j < 5; j++)
        {
            cardsImage[j].sprite = cards[0];
            buttons[j].interactable = true;
        }
    }

    public void replay()
    {
        text.text = "Select A Card";
        joker[0] = UnityEngine.Random.Range(0, 5);

        for (int i = 0; i < PlayerData.Instance.items.Length; i++)
        {
            if (PlayerData.Instance.items[i] == 26)
            {
                joker[1] = UnityEngine.Random.Range(0, 5);

                while (joker[1] == joker[0])
                    joker[1] = UnityEngine.Random.Range(0, 5);
            }
        }

        for (int j = 0; j < 5; j++)
        {
            cardsImage[j].sprite = cards[0];
            buttons[j].interactable = true;
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

        for (int i = 0; i < PlayerData.Instance.items.Length; i++)
        {
            if (PlayerData.Instance.items[i] == 26)
            {
                if (x == joker[1])
                {
                    cardsImage[x].sprite = cards[1];

                    for (int j = 0; j < PlayerData.Instance.items.Length; j++)
                    {
                        if (PlayerData.Instance.items[j] == 27 && retry)
                        {
                            retry = false;
                            text.text = "Lucky Hidden Card Try Again";
                            yield return new WaitForSeconds(2f);
                            replay();
                            yield break;
                        }
                    }

                    text.text = "YOU LOSE";
                    yield return new WaitForSeconds(2f);
                    ui.hideGame();
                    GameManager.Instance.loseHand();
                }
                break;
            }
        }

        if (x == joker[0])
        {
            cardsImage[x].sprite = cards[1];

            for (int i = 0; i < PlayerData.Instance.items.Length; i++)
            {
                if (PlayerData.Instance.items[i] == 27 && retry)
                {
                    retry = false;
                    text.text = "Lucky Hidden Card Try Again";
                    yield return new WaitForSeconds(2f);
                    replay();
                    yield break;
                }
            }

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

            for (int i = 0; i < PlayerData.Instance.items.Length; i++)
            {
                if (PlayerData.Instance.items[i] == 26)
                {
                    GameManager.Instance.winHand(0.75f);
                    yield break;
                }
            }
            GameManager.Instance.winHand(0.25f);
        }
    }
}
