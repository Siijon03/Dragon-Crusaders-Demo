using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Setting up the BattleConditions to include possible outcomes. 
public enum BattleState { START, PLAYERTURN, ATTACKING,ITEM,ACT,SELECTION,EMEMYTURN, WIN, LOSE, FLEE}

public class BattleSystem : MonoBehaviour
{
    //Public Access to Prebabs by making them 
    public GameObject PlayerPrefab;
    public GameObject EnemyPrefab;

    //Detrimes the 'BattleStations' of the Player and Enemy. 
    public Transform PlayerBattleStation;
    public Transform EnemyBattleStation;

    //Calls Upon Previous Scripts to Create Variable as it accesses those lines of code.
    Player PlayerUnit;
    Enemy_Unit EnemyUnit;

    //Sets Up UIText that can be called in game for the enemy.
    public TextMeshProUGUI UIDialogueText;
    public TextMeshProUGUI UIEnemyLevel;

    //Sets Up UIText that can be called in game for the player.
    public TextMeshProUGUI UIPlayerName;
    public TextMeshProUGUI UIPlayerLevel;

    //This calls our BattleHUD script so it can be used here. 
    public BattleHUD PlayerSide;
    public BattleHUD EnemySide;

    //Public BattleState Adjsutment
    public BattleState state;

    //Sets the battle set to start and battles upon the SetBattle Function
    void Start()
    {
        state = BattleState.START;
        SetupBattle();
    }

    //Sets up the Battle and Gets the Enemyname
    void SetupBattle()
    {
        GameObject PlayerTurn = Instantiate(PlayerPrefab, PlayerBattleStation);
        PlayerUnit = PlayerTurn.GetComponent<Player>();

        UIPlayerName.text = PlayerUnit.MC_Name;
        UIPlayerLevel.text = "Lv." + PlayerUnit.PlayerLevel;

        GameObject EnemyTurn = Instantiate(EnemyPrefab, EnemyBattleStation);  
        EnemyUnit = EnemyTurn.GetComponent<Enemy_Unit>();

        UIDialogueText.text = EnemyUnit.EnemyName;
        UIEnemyLevel.text = "Lv." + EnemyUnit.EnemyLevel;

        PlayerSide.SetHUD(PlayerUnit);
        EnemySide.SetHUD(EnemyUnit);


    }

}
