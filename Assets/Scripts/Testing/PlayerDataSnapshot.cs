using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerDataSnapshot
{
    public int[] items;
    public int maxHP;
    public int currentHP;
    public int maxCombatChips;
    public int currentCombatChips;
    public int combatChipRegen;
    public int bonusMaxHP;
    public int bonusMaxChips;
    public int bonusChipRegen;
    public int shield;
    public int armour;
    public int coins;

    public static PlayerDataSnapshot CreateDefaultTest()
    {
        return new PlayerDataSnapshot
        {
            items = new int[] { 1, 6, 2, 26 },
            maxHP = 100,
            currentHP = 100,
            maxCombatChips = 50,
            currentCombatChips = 50,
            combatChipRegen = 10,
            shield = 0,
            armour = 0,
            coins = 100
        };
    }
}
