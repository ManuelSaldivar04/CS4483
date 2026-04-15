using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TMP_InputField wagerInputField;

    public GameObject errorObject;

    public GameObject enemySlash;
    public GameObject playerSlash;

    public GameObject gamePrompt;
    public GameObject actionPrompt;
    public GameObject wagerPrompt;
    public GameObject gameButtons;
    public GameObject actionButtons;
    public GameObject wagerButtons;

    public GameObject bj;
    public GameObject roulette;
    public GameObject cursedCard;
    public GameObject dice;

    public GameObject resultText;

    public Image playerShield;
    public Image enemyShield;

    public GameObject iconObject;
    public Image icon;
    public TextMeshProUGUI wagerText;
    public GameObject wagerObject;
    public Sprite attackSprite;
    public Sprite blockSprite;

    public void Start()
    {
        gamePrompt.SetActive(true);
        gameButtons.SetActive(true);
        actionPrompt.SetActive(false);
        actionButtons.SetActive(false);
        wagerPrompt.SetActive(false);
        wagerButtons.SetActive(false);
        errorObject.SetActive(false);
        bj.SetActive(false);
        //roulette.SetActive(false);
        cursedCard.SetActive(false);
        dice.SetActive(false);
        enemySlash.SetActive(false);
        playerSlash.SetActive(false);
        playerShield.gameObject.SetActive(false);
        enemyShield.gameObject.SetActive(false);
        resultText.SetActive(false);
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
        if (int.TryParse(wagerInputField.text, out int x))
        {
            checkWager(x);
        }
          
    }

    public void checkWager(int wager)
    {
        if (wager > GameManager.Instance.player.currentCombatChips || wager < 0)
        {
            errorObject.SetActive(true);
        }

        else
        {
            errorObject.SetActive(false);
            GameManager.Instance.setWager(wager);
            wagerPrompt.SetActive(false);
            wagerButtons.SetActive(false);

            setPLayerIntent();

            switch (GameManager.Instance.getGame())
            {
                case 0:
                    bj.SetActive(true);
                    bj.GetComponent<BJ>().beginBJ();
                    break;

                case 1:
                    roulette.SetActive(true);
                    break;

                case 2:
                    cursedCard.SetActive(true);
                    cursedCard.GetComponent<CursedCard>().beginCursedCard();
                    break;

                case 3:
                    dice.SetActive(true);
                    dice.GetComponent<Dice>().beginDice();
                    break;
            }
        }
    }

    public void backWager()
    {
        wagerPrompt.SetActive(false);
        wagerButtons.SetActive(false);
        actionPrompt.SetActive(true);
        actionButtons.SetActive(true);
    }

    public void hideGame()
    {
        bj.SetActive(false);
        //roulette.SetActive(false);
        cursedCard.SetActive(false);
        dice.SetActive(false);
    }

    public void newTurn()
    {
        gamePrompt.SetActive(true);
        gameButtons.SetActive(true);
        actionPrompt.SetActive(false);
        actionButtons.SetActive(false);
        wagerPrompt.SetActive(false);
        wagerButtons.SetActive(false);
        wagerObject.SetActive(false);
        iconObject.SetActive(false);
    }

    public void setPLayerIntent()
    {
        if (GameManager.Instance.getAction() == 0)
        {
            icon.sprite = attackSprite;
        }

        else
        {
            icon.sprite = blockSprite;
        }

        wagerText.text = GameManager.Instance.getWager().ToString();
        wagerObject.SetActive(true);
        iconObject.SetActive(true);

    }

    public IEnumerator ShieldBlockEffect(int x, float holdTime = 0.8f, float fadeTime = 0.8f, float fadeInTime = 0.4f)
    {
        resultText.SetActive(true);
        Image shield = null;
        switch (x)
        {
            case 0:
                shield = playerShield;
                break;

            case 1:
                shield = enemyShield;
                break;

        }
        Color c = shield.color;
        c.a = 1f;
        shield.color = c;
        shield.gameObject.SetActive(true);
        float elapsed = 0f;

        while (elapsed < fadeInTime)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, elapsed / fadeInTime);
            shield.color = c;
            yield return null;
        }

        yield return new WaitForSeconds(holdTime);

        elapsed = 0f;
        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, elapsed / fadeTime);
            shield.color = c;
            yield return null;
        }

        shield.gameObject.SetActive(false);
        resultText.SetActive(false);
    }

}
