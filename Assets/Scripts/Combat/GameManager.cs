using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PlayerData player;
    public EnemyData enemy;

    public PlayerStatBars playerBars;
    public EnemyStatBars enemyBars;
    public EnemyIntent intent;

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

    public void winHand(float mult)
    {
        enemy.TakeDamage((int)(wager * mult));
        updateEnemyBars();

        if (enemy.isDead())
            defeat();

        switch (game)
        {
            case 0:
                enemyTurnBJ();
                break;
        }
        
    }

    public void loseHand()
    {
        player.LoseCombatChips(wager);
        updatePlayerBars();

        switch (game)
        {
            case 0:
                enemyTurnBJ();
                break;
        }
    }

    public void enemyTurnBJ()
    {
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
                if (enemyAction == 0)
                {
                    player.TakeDamage((int)(enemyWager * 1.5));
                    updatePlayerBars();
                    if (player.isDead())
                        defeat();
                    break;
                }

                else
                {
                    enemy.GainShield((int)(enemyWager * 1.5));
                    break;
                }

            case 2:
                if (enemyAction == 0)
                {
                    player.TakeDamage(enemyWager);
                    updatePlayerBars();
                    if (player.isDead())
                        defeat();
                    break;
                }

                else
                {
                    enemy.GainShield(enemyWager);
                    updateEnemyBars();
                    break;
                };

            case 3:
                enemy.LoseCombatChips(enemyWager);
                break;
        }

        intent.declareIntent(enemy.currentCombatChips);
        player.GainCombatChips(10);
        enemy.GainCombatChips(5);
    }

    public void victory()
    {

    }

    public void defeat()
    {

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
