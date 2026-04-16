using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Combat/Enemy")]
public class EnemyData : ScriptableObject
{
    [System.Serializable]
    public struct attackStats
    {
        public float[] multiplier;
        public float[] weight;      
    }

    public attackStats stats;
    [Range(1, 100)]
    public int successChance;

    public Sprite sprite;

    public int maxHP;
    public int currentHP;

    public int maxCombatChips;
    public int currentCombatChips;

    public int shield;

    public void InitializeBattle()
    {
        // reset chips at the start of each fight
        currentCombatChips = maxCombatChips;

        currentHP = maxHP;

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
