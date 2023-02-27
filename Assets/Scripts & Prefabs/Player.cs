using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Setting up all of the Player's Stats. 
    //This is the Player's name.
    public string MC_Name;

    //Basic Attack and Defense Stats.
    public int Player_AttackStat;
    public int Player_DefenseStat;
    
    //Player Health Values.
    public int Player_CurrentHealth;
    public int Player_MaxHealth;
    public int Player_MinHealth = 0;

    //Player EnergyValues.
    public int Player_MaxEnergy;
    public int Player_CurrentEnergy;
    public int Player_MinEnergy = 0;

    //Player Levelup multipliers, Lv is the current level while LvUp is how much they need to level up. Also, CurLvl is how much exp they currently have. 
    public int PlayerLevel;
    public int Player_LvUp;
    public int Player_CurrentLevel;

    //This is how much damage the player can deal.
    public int Player_Damage;

    //This is for when the player takes damage 
    public bool TakeDamage(int TakeDamage)
    {
        //When Damage is taken this will subtract the player's health.
        Player_CurrentHealth -= TakeDamage;

        //Checks if the Player's current health is equal or below 0.
        if (Player_CurrentHealth <= 0)
            return true;
        else
            return false;
    }
    
}
