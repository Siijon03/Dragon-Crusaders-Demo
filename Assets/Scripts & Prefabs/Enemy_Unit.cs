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

    //This how much damage an enemy can deal.
    public int Enemy_Damage;

    public bool TakeDamage(int TakeDamage)
    {
        Enemy_CurrentHealth -= TakeDamage;

        if (Enemy_CurrentHealth <= 0)
            return true;
        else
            return false;
    }

}
