using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Setting up the BattleConditions to include possible outcomes. 
public enum BattleState { START, PLAYERTURN, ATTACKING,ITEM,ACT,SELECTION,ENEMYTURN, WIN, LOSE, FLEE}

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
    public BattleState CurrentState;

    

    //Sets the battle set to start and battles upon the SetBattle Function
    void Start()
    {
        CurrentState = BattleState.START;
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

        //Updates Player Energy
        PlayerHUD.SetEnergy(PlayerUnit.Player_CurrentEnergy);

        //Sets Player Experience
        PlayerHUD.SetEXP(PlayerUnit.Player_CurrentLevel);

        //Waits 2 Seconds 
        yield return new WaitForSeconds(2f);

        //changes battle state.
        CurrentState = BattleState.PLAYERTURN;
        //Loads PlayerTurn
        Player_Turn();
        Debug.Log("Switching to Player");
    }

    //This indicates playerturn
    void Player_Turn()
    {
        UIDialogueText.text = EnemyUnit.EnemyName + " Has Appeared!";
        Debug.Log("Your Turn!");
        CurrentState = BattleState.PLAYERTURN;
    }

    //Switches to the Player Attack
    IEnumerator PlayerAttack()
    {
        //Checks to see if player has dropped to 0 HP.
        bool PlayerisDead = PlayerUnit.TakeDamage(PlayerUnit.Player_Damage);
        bool EnemyIsDead = EnemyUnit.TakeDamage(EnemyUnit.Enemy_Damage);

        EnemyUnit.TakeDamage(PlayerUnit.Player_Damage);
        EnemyHUD.SetEnemyHP(EnemyUnit.Enemy_CurrentHealth);
        
        yield return new WaitForSeconds(2f);

        //If enemy is dead then the player wins and the battle ends. 
        if(EnemyIsDead)
        {
            CurrentState = BattleState.WIN;
            EndBattle();
        }

        if (PlayerisDead)
        {
            CurrentState = BattleState.LOSE;
            EndBattle();
        }
        //If the enemy is not defeated then it will be the enemy turn. 
        else
        {
            CurrentState = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }

        //This is the enemy's turn
        IEnumerator EnemyTurn()
        {
            

            //Flavour Text.
            UIDialogueText.text = EnemyUnit.EnemyName + "Attacks!";
            Debug.Log("Enemy Attacks");

            yield return new WaitForSeconds(1f);

            //Player will take damage 
            PlayerUnit.TakeDamage(EnemyUnit.Enemy_Damage);

            //Updates Player HP.
            PlayerHUD.SetHP(PlayerUnit.Player_CurrentHealth); 


            yield return new WaitForSeconds(1f);

            //Checks if player is defeated.
            if(PlayerisDead == true)
            {
                CurrentState = BattleState.LOSE;
                EndBattle();
                Debug.Log("End of Battle....you lose...");
            }
            //Switches back to player turn
            else
            {
                CurrentState = BattleState.PLAYERTURN;
                Player_Turn();
                Debug.Log("Back To You!");
            }
        }

        //Loads ending the battle depending on the result.
        void EndBattle()
        {
            if(CurrentState == BattleState.WIN)
            {
                UIDialogueText.text = PlayerUnit.MC_Name + " Wins!";
                Debug.Log("You Won!");
            }
            else if (CurrentState == BattleState.LOSE)
            {
                UIDialogueText.text = PlayerUnit.MC_Name + " Loses...";
                Debug.Log("You Lose...");
            }
        }
    }

    //Runs Player Attack Coroutine.
    public void OnAttackButton()
    {
        //Runs attack if it's the player's turn.
        if (CurrentState != BattleState.PLAYERTURN)
            return;
        StartCoroutine(PlayerAttack());
        Debug.Log("Successful Attack!");
    }
}
