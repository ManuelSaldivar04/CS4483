using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyIntent : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI wagerText;
    public Sprite attackSprite;
    public Sprite blockSprite;
    private int intent;
    private int wager;
    public void declareIntent(int cChips)
    {
        // randomly decide attack or block
        intent = Random.Range(0, 2);

        GameManager.Instance.setEnemyAction(intent);

        // randomly choose wager
        wager = Random.Range(1, cChips / 5 + 1) * 5;

        GameManager.Instance.setEnemyWager(wager);

        if (intent == 0)
        {
            icon.sprite = attackSprite;
        }

        else
        {
            icon.sprite = blockSprite;
        }

        wagerText.text = wager.ToString();
    }

    public int getIntent()
    {
        return intent;
    }

    public int getWager()
    {
        return wager;
    }
}
