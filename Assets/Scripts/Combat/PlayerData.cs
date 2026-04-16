using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{

    public int[] items;
    public int maxHP = 100;
    public int currentHP = 100;

    public int maxCombatChips = 100;
    public int currentCombatChips = 100;

    public int shield;

    public int coins = 0;

    public int bonusMaxHP = 0;
    public int bonusMaxChips = 0;


    public void InitializeRun()
    {
        // apply any permanent upgrades at the start of a run
        maxHP = 100 + bonusMaxHP;
        maxCombatChips = 100 + bonusMaxChips;

        // reset to full for new run
        currentHP = maxHP;
        currentCombatChips = maxCombatChips;
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

    public bool isDead()
    {
        if (currentHP <= 0)
            return true;

        else
            return false;
    }

}
