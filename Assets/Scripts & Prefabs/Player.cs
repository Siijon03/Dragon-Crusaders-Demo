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
    public int Player_EnergyAttackStat;
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
    public int Player_ExperienceAmount;
    public int EXPNeeded;
    public int Player_CurrentLevel;


    //This is for when the player takes damage 
    public bool TakeDamage(int TakeDamage)
    {
        //When Damage is taken this will subtract the player's health.
        Player_CurrentHealth -= ((TakeDamage * 2)/Player_DefenseStat);

        //Checks if the Player's current health is equal or below 0.
        if (Player_CurrentHealth <= 0)
            //Returns true if yes
            return true;
        else
            //Returns False if not
            return false;

    }

    //This Decreases Energy Based on Amount of Energy/Mana Used for an Attack.
    public bool UseEnergy(int UseEnergy)
    {
        //Checks if Decreases the amount of energy used by the amount of energy used.
        Player_CurrentEnergy -= UseEnergy;

        //Checks if Current Energy is equal of below Zero.
        if (Player_CurrentEnergy <= 0)
            //If yes then the player cannot use an energy attack.
            return true;
        else
            //If not, they can use another energy attack.
            return false;
    }

    public void GainEnergy(int IncreaseEnergy)
    {
        //Increases the Player's Current Energy.
        Player_CurrentEnergy += IncreaseEnergy;
        //Stops the Player from 'Over Charging.'
        if (Player_CurrentEnergy > Player_MaxEnergy)
            Player_CurrentEnergy = Player_MaxEnergy;

    }

    public void FocusBonus(float FocusBonus)
    {
        Player_EnergyAttackStat += (int)FocusBonus;
    }

    //This will give the Player Experience Points Based on the amount of EXP an Enemy Contains.
    public bool GainExperiencePoints(int GainEXP)
    {
        //This will give the Player more EXP Upon Winning.
        Player_ExperienceAmount += GainEXP;

        //If the Current Experience Amount exceeds the amount needed.
        if (Player_ExperienceAmount >= EXPNeeded)
            //Checks if the Requirements for Level Up are met. 
            return true;
        else
            //Returns if the Requirements are not Met.
            return false;
            
    }

    //Allows the Player to Heal when taken damage.
    public void PlayerHeal(int HealAmount)
    { 
        //Adds A healing amount to the Player's Current Health.
        Player_CurrentHealth += HealAmount;
        //Stops the Player from 'Overhealing.'
        if (Player_CurrentHealth > Player_MaxHealth)
            Player_CurrentHealth = Player_MaxHealth;

    }

    public void PlayerDefenseBoost(int GuardBoost)
    {
        //Adds Extra Defense to the Player.
        Player_DefenseStat += GuardBoost;
    }
    
}
