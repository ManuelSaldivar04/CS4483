using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BJ : MonoBehaviour
{
    public TextMeshProUGUI playerScore;
    public TextMeshProUGUI enemyScore;
    public TextMeshProUGUI result;

    public Sprite [] cards = new Sprite [52];      
    public GameObject [] playerCards = new GameObject [12];
    public GameObject [] enemyCards = new GameObject [12];

    int numPlayerCards = 0;
    int numEnemyCards = 0;

    //public void beginBJ()
    //{
    //    Debug.Log(playerCards[numPlayerCards]);
    //    Debug.Log(cards.Length);
    //    Debug.Log(getCard());
    //    playerCards[numPlayerCards].GetComponent<UnityEngine.UI.Image>().sprite = cards[getCard()];
    //    playerCards[numPlayerCards].SetActive(true);
    //    numPlayerCards++;
    //    enemyCards[numEnemyCards].GetComponent<UnityEngine.UI.Image>().sprite = cards[getCard()];
    //    enemyCards[numEnemyCards].SetActive(true);
    //    numEnemyCards++;
    //    playerCards[numPlayerCards].GetComponent<UnityEngine.UI.Image>().sprite = cards[getCard()];
    //    playerCards[numPlayerCards].SetActive(true);
    //    numPlayerCards++;
    //}

    public int getCard()
    {
        return UnityEngine.Random.Range(0, 52);
    }

    public void playerHit()
    {
        playerCards[numPlayerCards].GetComponent<UnityEngine.UI.Image>().sprite = cards[getCard()];
        numPlayerCards++;
    }
}
