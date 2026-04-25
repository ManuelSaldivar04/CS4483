using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;


    public int[] items = {0, 0, 0, 0};

    public int maxHP;
    public int currentHP;

    public int maxCombatChips;
    public int currentCombatChips;
    public int combatChipRegen;

    public int shield;

    public int armour;

    public int coins;

    public int bonusMaxHP = 0;
    public int bonusMaxChips = 0;
    public int bonusChipRegen = 0;


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

    public void InitializeRun()
    {
        // apply any permanent upgrades at the start of a run
        maxHP = 100 + bonusMaxHP;
        maxCombatChips = 50 + bonusMaxChips;

        // reset to full for new run
        currentHP = maxHP;
        currentCombatChips = maxCombatChips;


        combatChipRegen = 10 + bonusChipRegen;
    }

    public void InitializeBattle()
    {
        // reset chips at the start of each fight
        currentCombatChips = maxCombatChips;
        shield = 0;
    }

    public void TakeDamage(int amount)
    {
        if (shield == 0)
        {
            currentHP = Mathf.Max(0, currentHP - amount);
        }

        else
        {
            int x = LoseShield(amount);
            currentHP = Mathf.Max(0, currentHP - x);
        }
    }

    public void HealHP(int amount)
    {
        currentHP = Mathf.Min(maxHP, currentHP + amount);
    }

    public void gainMaxHP(int amount)
    {
        maxHP += amount;
    }

    public void GainShield(int amount)
    {
        shield += amount;
    }

    public int LoseShield(int amount)
    {
        if (amount > shield)
        {
            amount -= shield;
            shield = 0;
            return amount;
        }

        else
        {
            shield -= amount;
            return 0;
        }
    }

    public void AddCoins(int amount)
    {
        coins += amount;
    }

    public bool SpendCoins(int amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            return true;
        }

        // not enough coins
        return false;
    }

    public void LoseCombatChips(int amount)
    {
        currentCombatChips = Mathf.Max(0, currentCombatChips - amount);
    }

    public void GainCombatChips(int amount)
    {
        currentCombatChips = Mathf.Min(maxCombatChips, currentCombatChips + amount);
    }

    public void addCombatChipRegen(int x)
    {
        combatChipRegen += x;
    }

    public void setArmour(int x)
    {
        armour = x;
    }

    public bool isDead()
    {
        if (currentHP <= 0)
            return true;

        else
            return false;
    }

}
