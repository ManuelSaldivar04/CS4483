using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public UIManager ui;

    public SlashAnimation slashAnim;
    public MirrorSlashAnimation mirrorSlashAnim;

    public EnemyData[] enemies; 

    public PlayerData player;
    public EnemyData enemy;

    public GameObject enemySprite;

    public PlayerStatBars playerBars;
    public EnemyStatBars enemyBars;
    public EnemyIntent intent;

    public TextMeshProUGUI enemyBattleText;
    public GameObject enemyBattleTextObject;

    public TextMeshProUGUI enemyShieldText;   
    public TextMeshProUGUI playerShieldText;

    private int game;
    private int action;
    private int wager;
    private int enemyWager;
    private int enemyAction;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        player.InitializeBattle();
        enemy.InitializeBattle();
        playerBars.setHealth(player.currentHP, player.maxHP);
        playerBars.setCombat(player.currentCombatChips, player.maxCombatChips);
        enemyBars.setHealth(enemy.currentHP, enemy.maxHP);
        enemyBars.setCombat(enemy.currentCombatChips, enemy.maxCombatChips);
        intent.declareIntent(enemy.currentCombatChips);
        updateEnemyShield();
        updatePlayerShield();
    }

    public void getEnemy(int x)
    {
        enemy = enemies[x];
        enemySprite.GetComponent<SpriteRenderer>().sprite = enemies[x].sprite;
    }

    public void updatePlayerBars()
    {
        playerBars.setHealth(player.currentHP, player.maxHP);
        playerBars.setCombat(player.currentCombatChips, player.maxCombatChips);
    }

    public void updateEnemyBars()
    {
        enemyBars.setHealth(enemy.currentHP, enemy.maxHP);
        enemyBars.setCombat(enemy.currentCombatChips, enemy.maxCombatChips);
    }

    public void updateIntent()
    {
        intent.declareIntent(enemy.currentCombatChips);
    }

    public void updatePlayerShield()
    {
       playerShieldText.text = player.shield.ToString();
    }

    public void updateEnemyShield()
    {
        enemyShieldText.text = enemy.shield.ToString();
    }

    public void winHand(float mult)
    {
        if (action == 0)
        {
            slashAnim.PlaySlash(() =>
            {
                enemy.TakeDamage((int)(wager * mult));
                updateEnemyBars();
                updateEnemyShield();

                if (enemy.isDead())
                {
                    victory();
                    return;
                }
            });
        }

        else
        {
            player.GainShield((int)(wager * mult));
            updatePlayerShield();
        }

        switch (0)
        {
            case 0:
                StartCoroutine(enemyTurnBJ());
                break;
        }

    }

    public void loseHand()
    {
        player.LoseCombatChips(wager);
        updatePlayerBars();

        switch (0)
        {
            case 0:
                StartCoroutine(enemyTurnBJ());
                break;
        }
    }

    IEnumerator enemyTurnBJ()
    {
        yield return new WaitForSeconds(1.5f);
        enemyBattleText.text = "Enemy Playing Blackjack...";
        enemyBattleTextObject.SetActive(true);
        yield return new WaitForSeconds(2f);

        int result;
        float x = UnityEngine.Random.Range(1, 10001);

        if (x <= 500)
            result = 1;

        else if (x > 500 && x <= 5000)
            result = 2;

        else
            result = 3;

        switch (result)
        {
            case 1:
                enemyBattleText.text = "Enemy Blackjack";

                if (enemyAction == 0)
                {
                    mirrorSlashAnim.PlaySlash( () =>
                    {
                        player.TakeDamage((int)(enemyWager * 1.5));
                        updatePlayerBars();
                        updatePlayerShield();
                        
                    });
                    yield return new WaitForSeconds(2.5f);
                    enemyBattleTextObject.SetActive(false);
                    if (player.isDead())
                    {
                        defeat();
                        break;
                    }

                    ui.newTurn();
                    break;
                }

                else
                {
                    enemy.GainShield((int)(enemyWager * 1.5));
                    updateEnemyShield();
                    yield return new WaitForSeconds(2.5f);
                    enemyBattleTextObject.SetActive(false);
                    ui.newTurn();
                    break;
                }

            case 2:
                enemyBattleText.text = "Enemy Win";

                if (enemyAction == 0)
                {
                    mirrorSlashAnim.PlaySlash( () =>
                    {
                        player.TakeDamage((int)(enemyWager));
                        updatePlayerBars();
                        updatePlayerShield();

                    });
                    yield return new WaitForSeconds(2.5f);
                    enemyBattleTextObject.SetActive(false);
                    if (player.isDead())
                    {
                        defeat();
                        break;
                    }

                    ui.newTurn();
                    break;
                }

                else
                {
                    enemy.GainShield(enemyWager);
                    updateEnemyShield();
                    yield return new WaitForSeconds(2.5f);
                    enemyBattleTextObject.SetActive(false);
                    ui.newTurn();
                    break;
                };

            case 3:
                enemyBattleText.text = "Enemy Lose";
                enemy.LoseCombatChips(enemyWager);
                updateEnemyBars();
                yield return new WaitForSeconds(2.5f);
                enemyBattleTextObject.SetActive(false);
                ui.newTurn();
                break;
        }

        if (player.currentHP > 0)
        {
            player.GainCombatChips(10);
            enemy.GainCombatChips(5);
            intent.declareIntent(enemy.currentCombatChips);
            updateEnemyBars();
            updatePlayerBars();
        }
    }

    public void victory()
    {
        Debug.Log("Victory");
    }

    public void defeat()
    {
        Debug.Log("Defeat");
    }

    public void setGame(int g)
    {
        game = g;
    }

    public void setAction(int a)
    {
        action = a;
    }

    public void setWager(int w)
    {
        wager = w;
    }

    public void setEnemyWager(int w)
    {
        enemyWager = w;
    }

    public void setEnemyAction(int a)
    {
        enemyAction = a;
    }

    public int getGame()
    {
        return game;
    }

    public int getAction()
    {
        return action;
    }

    public int getWager()
    {
        return wager;
    }

    public int getEnemyWager()
    {
        return enemyWager;
    }

    public int getEnemyAction()
    {
        return enemyAction;
    }
    
}
