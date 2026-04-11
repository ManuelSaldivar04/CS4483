using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BJ : MonoBehaviour
{
    public UIManager ui;
    public TextMeshProUGUI playerScore;
    public TextMeshProUGUI dealerScore;
    public TextMeshProUGUI result;
    public GameObject resultObject;

    public Button hit;
    public Button stand;

    public Sprite[] cards = new Sprite[52];
    public Image[] playerCardsImage = new Image[12];
    public GameObject[] playerCardsObject = new GameObject[12];
    public Image[] dealerCardsImage = new Image[12];
    public GameObject[] dealerCardsObject = new GameObject[12];

    int numPlayerCards;
    int numDealerCards;
    int playerTotal;
    int dealerTotal;
    int card;
    int playerAces;
    int dealerAces;

    public void beginBJ()
    {
        resultObject.SetActive(false);
        hit.interactable = true;
        stand.interactable = true;
        numPlayerCards = 0;
        numDealerCards = 0;
        playerTotal = 0;
        dealerTotal = 0;
        playerAces = 0;
        dealerAces = 0;

        for (int i = 0; i < 12; i++)
        {
            playerCardsObject[i].SetActive(false);
        }

        for (int i = 0; i < 12; i++)
        {
            dealerCardsObject[i].SetActive(false);
        }

        card = getCard();
        playerCardsImage[numPlayerCards].sprite = cards[card];
        playerCardsObject[numPlayerCards].SetActive(true);
        updatePLayerTotal(card);
        numPlayerCards++;

        card = getCard();
        dealerCardsImage[numDealerCards].sprite = cards[card];
        dealerCardsObject[numDealerCards].SetActive(true);
        updateDealerTotal(card);
        numDealerCards++;

        card = getCard();
        playerCardsImage[numPlayerCards].sprite = cards[card];
        playerCardsObject[numPlayerCards].SetActive(true);
        updatePLayerTotal(card);
        numPlayerCards++;

        if (playerTotal == 21)
        {
            hit.interactable = false;
            stand.interactable = false;
            playerScore.SetText(playerTotal.ToString());
            StartCoroutine(blackjack());
        }
    }

    public int getCard()
    {
        return UnityEngine.Random.Range(0, 52);
    }

    public void playerHit()
    {
        card = getCard();
        playerCardsImage[numPlayerCards].sprite = cards[card];
        playerCardsObject[numPlayerCards].SetActive(true);
        updatePLayerTotal(card);
        numPlayerCards++;

        if (playerBust())
        {
            hit.interactable = false;
            stand.interactable = false;
            StartCoroutine(lose());
        }

        if (numPlayerCards == 12)
            playerStand();
    }

    public void playerStand()
    {
        hit.interactable = false;
        stand.interactable = false;
        playerScore.SetText(playerTotal.ToString());
        StartCoroutine(dealerTurn());
    }

    public void updatePLayerTotal(int card)
    {
        if (card >= 0 && card < 4)
            playerTotal += 2;

        else if (card >= 4 && card < 8)
            playerTotal += 3;

        else if (card >= 8 && card < 12)
            playerTotal += 4;

        else if (card >= 12 && card < 16)
            playerTotal += 5;

        else if (card >= 16 && card < 20)
            playerTotal += 6;

        else if (card >= 20 && card < 24)
            playerTotal += 7;

        else if (card >= 24 && card < 28)
            playerTotal += 8;

        else if (card >= 28 && card < 32)
            playerTotal += 9;

        else if (card >= 32 && card < 36)
        {
            playerTotal += 11;
            playerAces++;
        }

        else if (card >= 36)
            playerTotal += 10;

        while (playerTotal > 21 && playerAces > 0)
        {
            playerTotal -= 10;
            playerAces--;
        }

        if (playerAces > 0)
            playerScore.SetText((playerTotal - 10).ToString() + " " + playerTotal.ToString());

        else
            playerScore.SetText(playerTotal.ToString());

    }

    public void updateDealerTotal(int card)
    {
        if (card >= 0 && card < 4)
            dealerTotal += 2;

        else if (card >= 4 && card < 8)
            dealerTotal += 3;

        else if (card >= 8 && card < 12)
            dealerTotal += 4;

        else if (card >= 12 && card < 16)
            dealerTotal += 5;

        else if (card >= 16 && card < 20)
            dealerTotal += 6;

        else if (card >= 20 && card < 24)
            dealerTotal += 7;

        else if (card >= 24 && card < 28)
            dealerTotal += 8;

        else if (card >= 28 && card < 32)
            dealerTotal += 9;

        else if (card >= 32 && card < 36)
        {
            dealerTotal += 11;
            dealerAces++;
        }

        else if (card >= 36)
            dealerTotal += 10;

        while (dealerTotal > 21 && dealerAces > 0)
        {
            dealerTotal -= 10;
            dealerAces--;
        }

        dealerScore.SetText(dealerTotal.ToString());
    }

    IEnumerator dealerTurn()
    {
        card = getCard();
        dealerCardsImage[numDealerCards].sprite = cards[card];
        dealerCardsObject[numDealerCards].SetActive(true);
        updateDealerTotal(card);
        numDealerCards++;

        if (dealerTotal == 21)
            StartCoroutine(bjLose());

        else
        {
            yield return new WaitForSeconds(1f);

            while (!dealerBust() && dealerTotal < 17)
            {
                card = getCard();
                dealerCardsImage[numDealerCards].sprite = cards[card];
                dealerCardsObject[numDealerCards].SetActive(true);
                updateDealerTotal(card);
                numDealerCards++;

                yield return new WaitForSeconds(1f);
            }

            if (dealerBust())
                StartCoroutine(win());

            else
            {
                if (dealerTotal > playerTotal)
                    StartCoroutine(lose());

                else if (dealerTotal < playerTotal)
                    StartCoroutine(win());

                else
                    StartCoroutine(push());
            }
        }
    }

    public bool playerBust()
    {
        if (playerTotal > 21)
            return true;

        else
            return false;
    }

    public bool dealerBust()
    {
        if (dealerTotal > 21)
            return true;

        else
            return false;
    }

    IEnumerator win()
    {
        result.text = "YOU WIN";
        resultObject.SetActive(true);

        yield return new WaitForSeconds(2f);
        ui.hideGame();
        GameManager.Instance.winHand(1);
    }

    IEnumerator lose()
    {
        result.text = "YOU LOSE";
        resultObject.SetActive(true);

        yield return new WaitForSeconds(2f);
        ui.hideGame();
        GameManager.Instance.loseHand();
    }

    IEnumerator push()
    {
        result.text = "PUSH PLAY AGAIN";
        resultObject.SetActive(true);

        yield return new WaitForSeconds(2f);
        beginBJ();
    }

    IEnumerator blackjack()
    {
        yield return new WaitForSeconds(1.5f);

        card = getCard();
        dealerCardsImage[numDealerCards].sprite = cards[card];
        dealerCardsObject[numDealerCards].SetActive(true);
        updateDealerTotal(card);
        numDealerCards++;

        if (dealerTotal == 21)
            StartCoroutine(push());

        else
        {
            result.text = "BLACKJACK";
            resultObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            ui.hideGame();
            GameManager.Instance.winHand(1.5f);
        }
    }

    IEnumerator bjLose()
    {
        result.text = "DEALER BLACKJACK";
        resultObject.SetActive(true);

        yield return new WaitForSeconds(2f);
        ui.hideGame();
        GameManager.Instance.loseHand();
    }
}
       
    
