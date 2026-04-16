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
        int x = UnityEngine.Random.Range(1, 101);

        if (x <= GameManager.Instance.enemy.actionPref)
            intent = 0;

        else
            intent = 1;

        GameManager.Instance.setEnemyAction(intent);

        wager = setEnemyWagerByStyle(cChips);

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

    public int setEnemyWagerByStyle(int cChips)
    {
        int min = 1;
        int max = cChips;

        switch (GameManager.Instance.enemy.wagerStyle)
        {
            case EnemyData.WagerStyle.Aggressive:
                // biased toward high end
                wager = Mathf.RoundToInt(Mathf.Lerp(min, max,
                    Mathf.Pow(UnityEngine.Random.Range(0f, 1f), 1f - GameManager.Instance.enemy.wagerAggression)));
                break;

            case EnemyData.WagerStyle.Cautious:
                // biased toward low end
                wager = Mathf.RoundToInt(Mathf.Lerp(min, max,
                    Mathf.Pow(UnityEngine.Random.Range(0f, 1f), 1f + GameManager.Instance.enemy.wagerAggression)));
                break;

            case EnemyData.WagerStyle.Random:
                wager = UnityEngine.Random.Range(min, max + 1);
                break;
        }

        wager = Mathf.Clamp(wager, min, max);
        return wager;
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
