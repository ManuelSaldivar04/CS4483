using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static EnemyData;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public UIManager ui;

    public SlashAnimation slashAnim;
    public MirrorSlashAnimation mirrorSlashAnim;

    public DiceAnimation anim;
    public Image enemyDice;
    public TextMeshProUGUI enemyDiceText;
    public Sprite[] enemyDiceSprites;

    public EnemyData[] enemies; 

    public EnemyData enemy;

    public GameObject enemySprite;
    public GameObject playerSprite;

    public PlayerStatBars playerBars;
    public EnemyStatBars enemyBars;
    public EnemyIntent intent;

    public TextMeshProUGUI enemyShieldText;   
    public TextMeshProUGUI playerShieldText;

    public TextMeshProUGUI resultText;


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
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    public void Start()
    {
        
        if (CombatData.pendingEnemy != null)
        {
            SetEnemy(CombatData.pendingEnemy);
            CombatData.pendingEnemy = null;
        }
        else
        {
            getEnemy(61);
        }
        PlayerData.Instance.InitializeBattle();
        enemy.InitializeBattle();
        playerBars.setHealth(PlayerData.Instance.currentHP, PlayerData.Instance.maxHP);
        playerBars.setCombat(PlayerData.Instance.currentCombatChips, PlayerData.Instance.maxCombatChips);
        enemyBars.setHealth(enemy.currentHP, enemy.maxHP);
        enemyBars.setCombat(enemy.currentCombatChips, enemy.maxCombatChips);
        intent.declareIntent(enemy.currentCombatChips);
        updateEnemyShield();
        updatePlayerShield();
        enemyDice.gameObject.SetActive(false);
        enemyDiceText.gameObject.SetActive(false);
    }

    public void StartCombat()
    {
        if (CombatData.pendingEnemy != null)
        {
            SetEnemy(CombatData.pendingEnemy);
            CombatData.pendingEnemy = null;
        }
        else
        {
            getEnemy(0);
        }

        PlayerData.Instance.InitializeBattle();
        enemy.InitializeBattle();
        playerBars.setHealth(PlayerData.Instance.currentHP, PlayerData.Instance.maxHP);
        playerBars.setCombat(PlayerData.Instance.currentCombatChips, PlayerData.Instance.maxCombatChips);
        enemyBars.setHealth(enemy.currentHP, enemy.maxHP);
        enemyBars.setCombat(enemy.currentCombatChips, enemy.maxCombatChips);
        intent.declareIntent(enemy.currentCombatChips);
        updateEnemyShield();
        updatePlayerShield();
        enemyDice.gameObject.SetActive(false);
        enemyDiceText.gameObject.SetActive(false);
    }

    public void getEnemy(int x)
    {
        enemy = enemies[x];
        enemySprite.GetComponent<SpriteRenderer>().sprite = enemies[x].sprite;
    }
    //set the enemy through EnemyData
    public void SetEnemy(EnemyData enemy)
    {
        this.enemy = enemy;
        enemySprite.GetComponent<SpriteRenderer>().sprite = this.enemy.sprite;
    }

    public void updatePlayerBars()
    {
        playerBars.setHealth(PlayerData.Instance.currentHP, PlayerData.Instance.maxHP);
        playerBars.setCombat(PlayerData.Instance.currentCombatChips, PlayerData.Instance.maxCombatChips);
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
       playerShieldText.text = PlayerData.Instance.shield.ToString();
    }

    public void updateEnemyShield()
    {
        enemyShieldText.text = enemy.shield.ToString();
    }

    public void winHand(float mult)
    {
        if (action == 0)
        {
            resultText.text = "Player Attacks For " + (int)(wager * mult);
            
            slashAnim.PlaySlash(() =>
            {
                if (enemy.shield >= wager * mult)
                    SoundEffectManager.Play("CombatHitShield");
                else
                    SoundEffectManager.Play("CombatHit");

                StartCoroutine(ShakeSprite(enemySprite));
                enemy.TakeDamage((int)(wager * mult));
                updateEnemyBars();
                updateEnemyShield();

                if (enemy.isDead())
                {
                    StartCoroutine(victory());
                }

                else
                {
                    StartCoroutine(enemyAttack());
                }
            });
        }

        else
        {
            SoundEffectManager.Play("CombatGainShield");
            resultText.text = "Player Gains " + (int)(wager * mult) + " Shield";
            ui.StartCoroutine(ui.ShieldBlockEffect(0));
            PlayerData.Instance.GainShield((int)(wager * mult));
            updatePlayerShield();
            StartCoroutine(enemyAttack());
        }

    }

    public void loseHand()
    {
        SoundEffectManager.Play("CombatFail");
        resultText.text = "Player Loses " + wager + " Combat Chips";
        resultText.gameObject.SetActive(true);
        PlayerData.Instance.LoseCombatChips(wager);
        updatePlayerBars();

        StartCoroutine(enemyAttack());
    }

    IEnumerator enemyAttack()
    {
        yield return new WaitForSeconds(2f);
        resultText.gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);
        resultText.gameObject.SetActive(true);

        bool attackSucceeds = false;
        float mult = 0f;
        int x;

        // Gets enemy mult
        resultText.text = "Rolling For Enemy Mult";
        x = UnityEngine.Random.Range(1, 101);

        for (int i = 0; i < enemy.stats.weight.Length; i++)
        {
            if (x <= enemy.stats.weight[i])
            {
                mult = enemy.stats.multiplier[i];
                break;
            }
        }

        enemyDice.gameObject.SetActive(true);
        anim.RollDice(() =>
        {
            enemyDiceText.gameObject.SetActive(true);
            enemyDice.sprite = enemyDiceSprites[2];
            switch (mult)
            {
                case 0.5f:
                    enemyDiceText.text = "0.5x";
                    break;

                case 1f:
                    enemyDiceText.text = "1x";
                    break;

                case 1.5f:
                    enemyDiceText.text = "1.5x";
                    break;

                case 2f:
                    enemyDiceText.text = "2x";
                    break;

                case 2.5f:
                    enemyDiceText.text = "2.5x";
                    break;

                case 3f:
                    enemyDiceText.text = "3x";
                    break;

                case 4f:
                    enemyDiceText.text = "4x";
                    break;

                case 5f:
                    enemyDiceText.text = "5x";
                    break;
            }
        });

        yield return new WaitForSeconds(3.5f);

        enemyDiceText.gameObject.SetActive(false);

        // Checks if attack is successful
        resultText.text = "Rolling For Enemy Success";
        x = UnityEngine.Random.Range(1, 101);

        anim.RollDice(() =>
        {
            if (x <= enemy.successChance)
            {
                enemyDice.sprite = enemyDiceSprites[0];
                attackSucceeds = true;
            }

            else
            {
                enemyDice.sprite = enemyDiceSprites[1];
                attackSucceeds = false;
            }
        });

        yield return new WaitForSeconds(3.5f);
        enemyDice.gameObject.SetActive(false);

        // apply result
        if (attackSucceeds)
        {
            Debug.Log(PlayerData.Instance.armour);
            int damage = (int)((enemyWager * mult) * (1 - (PlayerData.Instance.armour / 100f)));
            int shield = (int)(enemyWager * mult);

            if (enemyAction == 0)
            {
                resultText.text = "Enemy Attacks For " + damage;
                resultText.gameObject.SetActive(true);

                mirrorSlashAnim.PlaySlash(() =>
                {
                    if (PlayerData.Instance.shield >= damage)
                        SoundEffectManager.Play("CombatHitShield");
                    else
                        SoundEffectManager.Play("CombatHit");

                    StartCoroutine(ShakeSprite(playerSprite));
                    PlayerData.Instance.TakeDamage(damage);
                    updatePlayerBars();
                    updatePlayerShield();
                });

                yield return new WaitForSeconds(1f);
                resultText.gameObject.SetActive(false);

                if (PlayerData.Instance.isDead())
                {
                    StartCoroutine(defeat());
                    yield break;
                }
            }

            else
            {
                SoundEffectManager.Play("CombatGainShield");
                resultText.text = "Enemy Gains " + shield + " Shield";
                resultText.gameObject.SetActive(true);
                ui.StartCoroutine(ui.ShieldBlockEffect(1));
                enemy.GainShield(shield);
                updateEnemyShield();
                yield return new WaitForSeconds(2.5f);
                resultText.gameObject.SetActive(false);
            }
        }

        else
        {
            SoundEffectManager.Play("CombatFail");
            resultText.text = "Enemy Loses " + enemyWager + " Combat Chips";
            resultText.gameObject.SetActive(true);
            enemy.LoseCombatChips(enemyWager);
            updateEnemyBars();

            yield return new WaitForSeconds(2.5f);
            resultText.gameObject.SetActive(false);
        }

        if (PlayerData.Instance.currentHP > 0)
        {
            ui.newTurn();
            PlayerData.Instance.GainCombatChips(PlayerData.Instance.combatChipRegen);
            enemy.GainCombatChips(5);
            intent.declareIntent(enemy.currentCombatChips);
            updateEnemyBars();
            updatePlayerBars();
        }
        
    }

    IEnumerator victory()
    {
        resultText.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.75f);
        enemySprite.transform.Rotate(0, 0, 270);
        yield return new WaitForSeconds(2f);

        if (enemy.id == 40)
            SceneManager.LoadScene("VictoryScreen");

        else
            ui.victoryScreen();
        
    }

    IEnumerator defeat()
    {
        resultText.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.75f);
        playerSprite.transform.Rotate(0, 0, 90);
        yield return new WaitForSeconds(2f);
        ui.defeatScreen();

    }

    public void returnToOverworld()
    {
        
        //Mark the NPC as defeated
        if (!string.IsNullOrEmpty(CombatData.sourceNPCID))
        {
            NPC.MarkDefeated(CombatData.sourceNPCID);
            CombatData.sourceNPCID = null; //clear
        }
        //return to overworld
        LevelLoader ll = FindObjectOfType<LevelLoader>();
        if (ll != null)
        {
            GAMESTATEMANAGER.Instance.currentGameState = GAMESTATEMANAGER.GameState.World;
            ll.LoadReturnScene();
        }
        else
            Debug.LogError("No LevelLoader found to return to overworld!");
        
    }

    public void returnHome()
    {
        GAMESTATEMANAGER.Instance.currentGameState = GAMESTATEMANAGER.GameState.World;
        SceneManager.LoadScene("Tutorial");
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

    IEnumerator ShakeSprite(GameObject sprite, float duration = 0.4f, float magnitude = 0.3f)
    {
        Vector3 originalPos = sprite.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            sprite.transform.localPosition = new Vector3(
                originalPos.x + x,
                originalPos.y + y,
                originalPos.z
            );

            elapsed += Time.deltaTime;
            yield return null;
        }

        sprite.transform.localPosition = originalPos; // snap back
        
    }

}
