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
    public BattleHUD PlayerHUD;
    public BattleHUD EnemyHUD;

    //Public BattleState Adjsutment
    public BattleState state;

    //Sets the battle set to start and battles upon the SetBattle Function
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    //Sets up the Battle and Gets the Enemyname
    IEnumerator SetupBattle()
    {
        GameObject PlayerTurn = Instantiate(PlayerPrefab, PlayerBattleStation);
        PlayerUnit = PlayerTurn.GetComponent<Player>();

        UIPlayerName.text = PlayerUnit.MC_Name;
        UIPlayerLevel.text = "Lv." + PlayerUnit.PlayerLevel;

        GameObject EnemyTurn = Instantiate(EnemyPrefab, EnemyBattleStation);  
        EnemyUnit = EnemyTurn.GetComponent<Enemy_Unit>();

        UIDialogueText.text = EnemyUnit.EnemyName;
        UIEnemyLevel.text = "Lv." + EnemyUnit.EnemyLevel;

        //Sets the HUDs for each side.
        PlayerHUD.SetPHUD(PlayerUnit);
        EnemyHUD.SetEHUD(EnemyUnit);

        //Waits 2 Seconds 
        yield return new WaitForSeconds(2f);

        //changes battle state.
        state = BattleState.PLAYERTURN;
        //Loads PlayerTurn
        Player_Turn();
    }

    //This indicates playerturn
    void Player_Turn()
    {
        UIDialogueText.text = EnemyUnit.EnemyName + "As Appeared... Take them down!";
    }

    //Switches to the Player Attack
    IEnumerator PlayerAttack()
    {
        //Checks to see if player has dropped to 0 HP.
        bool isDead = PlayerUnit.TakeDamage(PlayerUnit.Player_Damage);
        
        yield return new WaitForSeconds(2f);

        //If enemy is dead then the player wins and the battle ends. 
        if(isDead)
        {
            state = BattleState.WIN;
            EndBattle();
        }
        //If the enemy is not defeated then it will be the enemy turn. 
        else
        {
            state = BattleState.EMEMYTURN;
            StartCoroutine(EnemyTurn());
        }

        //This is the enemy's turn
        IEnumerator EnemyTurn()
        {
            //Flavour Text.
            UIDialogueText.text = EnemyUnit.EnemyName + "Attacks!";

            yield return new WaitForSeconds(1f);

            //Player will take damage 
            PlayerUnit.TakeDamage(EnemyUnit.Enemy_Damage);

            //Updates Player HP.
            PlayerHUD.SetHP(PlayerUnit.Player_CurrentHealth);

            yield return new WaitForSeconds(1f);

            //Checks if player is defeated.
            if(isDead)
            {
                state = BattleState.LOSE;
                EndBattle();
            }
            //Switches back to player turn
            else
            {
                state = BattleState.PLAYERTURN;
                Player_Turn();
            }
        }

        //Loads ending the battle depending on the result.
        void EndBattle()
        {
            if(state == BattleState.WIN)
            {
                UIDialogueText.text = PlayerUnit.MC_Name + "Wins!";
            }
            else if (state == BattleState.LOSE)
            {
                UIDialogueText.text = PlayerUnit.MC_Name + "Loses...";
            }
        }
    }

    //Runs Player Attack Coroutine.
    public void OnAttackButton()
    {
        //Runs attack if it's the player's turn.
        if (state != BattleState.PLAYERTURN)
            return;
        StartCoroutine(PlayerAttack());
        Debug.Log("Successful Attack!");
    }
}
