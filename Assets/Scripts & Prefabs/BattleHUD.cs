using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    //Calling our GUI Elemments
    public TextMeshProUGUI PlayerName;

    public TextMeshProUGUI PlayerLevel;

    //Accessing our Sliders
    public Slider PlayerHealth;
    public Slider PlayerEnergy;
    public Slider PlayerEXP;

    public Slider EnemyHealth;

    //https://www.youtube.com/watch?v=0tDPxNB2JNs This can be used for our, health, energy and EXP.
    //For this, import your player stats for the correct values by gamecompenent find.

    //P_Unit means player unit.
    //E_Unit means enemy unit.
    //Setting the HUD by calling values from our player script. 
    //Additionally setting the HUD on the player side. 
    public void SetPHUD(Player P_Unit)
    {   
        //This Sets the Player's MaxHealth and Current Health.
        PlayerHealth.maxValue = P_Unit.Player_MaxHealth;
        PlayerHealth.value = P_Unit.Player_CurrentHealth;

        //This Sets the Player's MaxEnergy and Current Energy.
        PlayerEnergy.maxValue = P_Unit.Player_MaxEnergy;
        PlayerEnergy.value = P_Unit.Player_CurrentEnergy;

        //This Sets the Player's MaxEXP to level up and Current EXP.
        PlayerEXP.maxValue = P_Unit.EXPNeeded;
        PlayerEXP.value = P_Unit.Player_ExperienceAmount;

    }

    public void SetEHUD(Enemy_Unit E_Unit)
    {
     
        //This Sets the Enemy's MaxHealth and MinimumHealth.
        EnemyHealth.maxValue = E_Unit.Enemy_MaxHealth;
        EnemyHealth.value = E_Unit.Enemy_CurrentHealth;

    }

    //This Updates the player's health meter on damage or restoration.
    public void SetPlayerHP(int HealthUpdate)
    {
        PlayerHealth.value = HealthUpdate;
    }

    //This Updates the Energy Meter, whenever a special attack is used. 
    public void SetEnergy(int EnergyUpdate)
    {
        PlayerEnergy.value = EnergyUpdate;
    }

    //This Updates the player's EXP whenever they gain EXP.
    public void SetEXP(int EXPUpdate)
    {
        PlayerEXP.value = EXPUpdate;
    }

    public void SetEnemyHP(int EnemyHPUpdate)
    {
        EnemyHealth.value = EnemyHPUpdate;
    }
}
