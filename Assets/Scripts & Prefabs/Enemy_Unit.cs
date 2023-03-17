using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Unit : MonoBehaviour
{
    //This is the Enemy's Name
    public string EnemyName;
    //This is the Enemy's Current Level
    public int EnemyLevel;

    //Basic Attack and Defense Stats.
    public int Enemy_AttackStat;
    public int Enemy_DefenseStat;

    //Player Health Values.
    public int Enemy_CurrentHealth;
    public int Enemy_MaxHealth;
    public int Enemy_MinHealth = 0;

    //This is the Enemy's chance to evade a player's attack.
    public int Enemy_DodgeStat;

    
    //This is for when the enemy takes damage.
    public bool TakeDamage(int TakeDamage)
    {
        //Subtracting from the Enemy's health using a basic damage formula.
        Enemy_CurrentHealth -= ((TakeDamage * 2) / Enemy_DefenseStat);

        //Checks if the Enemy's Current Health is Below or Equal to Zero.
        if (Enemy_CurrentHealth <= 0)
            return true;
        else
            return false;
    }

}
