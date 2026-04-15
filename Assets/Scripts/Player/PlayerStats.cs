using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private int health;
    private int maxHealth;
    private int money;
    private int diceLuck;
    private int rouletteLuck;
    private int blackjackLuck;
    private int slotsLuck;

    public int Health { get => health; set => health = value; }
    public int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public int Money { get => money; set => money = value; }
    public int DiceLuck { get => diceLuck; set => diceLuck = value; }
    public int RouletteLuck { get => rouletteLuck; set => rouletteLuck = value; }
    public int BlackjackLuck { get => blackjackLuck; set => blackjackLuck = value; }
    public int SlotsLuck { get => slotsLuck; set => slotsLuck = value; }

}
