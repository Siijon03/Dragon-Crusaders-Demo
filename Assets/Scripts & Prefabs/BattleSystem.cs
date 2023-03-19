using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Setting up the BattleConditions to include possible outcomes. 
public enum BattleState { START, PLAYERTURN, ATTACKING,ITEM,ACT,ENEMYTURN, WIN, LOSE, FLEE,NOENERGY}

public class BattleSystem : MonoBehaviour
{
    //Public Access to Prebabs by making them 
    public GameObject PlayerPrefab;
    public GameObject EnemyPrefab;

    //Determines the 'BattleStations' of the Player and Enemy. 
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

    //Public BattleState Adjustment.
    public BattleState CurrentState;

    //Sets the battle set to start and battles upon the SetBattle Function.
    void Start()
    {
        //Makes the Inital State Start.
        CurrentState = BattleState.START;
        StartCoroutine(SetupBattle());
    }


    //Sets up the Battle and Gets the Enemyname.
    IEnumerator SetupBattle()
    {
        //Clones the Player's Prefab and Grid.
        GameObject PlayerTurn = Instantiate(PlayerPrefab, PlayerBattleStation);
        //Gets Components of the Player GameObject.
        PlayerUnit = PlayerTurn.GetComponent<Player>();

        //This Will get the Player's Name.
        UIPlayerName.text = PlayerUnit.MC_Name;
        //This Will get the Player's Level.
        UIPlayerLevel.text = "Lv." + PlayerUnit.PlayerLevel;

        //Clones the Enemy's Prefab and Grid.
        GameObject EnemyTurn = Instantiate(EnemyPrefab, EnemyBattleStation);
        //Gets Components of the Player GameObject.
        EnemyUnit = EnemyTurn.GetComponent<Enemy_Unit>();

        //This Will get the Enemy's Name.
        UIDialogueText.text = EnemyUnit.EnemyName;
        //This Will get the Enemy's Level.
        UIEnemyLevel.text = "Lv." + EnemyUnit.EnemyLevel;

        //Sets the HUDs for each side.
        PlayerHUD.SetPHUD(PlayerUnit);
        EnemyHUD.SetEHUD(EnemyUnit);

        //Updates Player Energy
        PlayerHUD.SetEXP(PlayerUnit.Player_ExperienceAmount);

        //Sets Player Energy
        PlayerHUD.SetEnergy(PlayerUnit.Player_CurrentEnergy);

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
        //Beginning Dialouge.
        UIDialogueText.text = EnemyUnit.EnemyName + " Has Appeared!";
        Debug.Log("Your Turn!");
        //Switches to Player's Turn.
        CurrentState = BattleState.PLAYERTURN;
    }

    

    //Switches to the Player Attack
    IEnumerator PlayerPhysicalAttack()
    {
        //Checks to see if enemy has dropped to 0 HP.
        bool EnemyIsDead = EnemyUnit.TakeDamage(PlayerUnit.Player_AttackStat);

        //Enemy Takes Damage and Calls the 'TakeDamage' Function from the Player Script 
        EnemyUnit.TakeDamage(PlayerUnit.Player_AttackStat);
        //Updates the Enemy's HP on Their HUD by accessing the Slider
        EnemyHUD.SetEnemyHP(EnemyUnit.Enemy_CurrentHealth);
        
        yield return new WaitForSeconds(2f);


        //If enemy is dead then the player wins and the battle ends. 
        if (EnemyIsDead == true)
        {
            EnemyHUD.SetEnemyHP(EnemyUnit.Enemy_CurrentHealth = 0);
            CurrentState = BattleState.WIN;
            EndBattle();
        }

        //If the enemy is not defeated then it will be the enemy turn. 
        else
        {
            CurrentState = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }

        
    }

    //Switches to the Player Attack
    IEnumerator PlayerPowerShotAttack()
    {
        //Checks to see if enemy has dropped to 0 HP.
        bool EnemyIsDead = EnemyUnit.TakeDamage(PlayerUnit.Player_AttackStat);

        float EnergyMultiplier = 1.5f;
        int EnergyAttackBonus = (int)(EnergyMultiplier);

        PlayerUnit.UseEnergy(15);
        PlayerHUD.SetEnergy(PlayerUnit.Player_CurrentEnergy);

        //Enemy Takes Damage and Calls the 'TakeDamage' Function from the Player Script and then Multiplying it by An Energy Stat.
        EnemyUnit.TakeDamage(PlayerUnit.Player_EnergyAttackStat * EnergyAttackBonus);
        //Updates the Enemy's HP on Their HUD by accessing the Slider
        EnemyHUD.SetEnemyHP(EnemyUnit.Enemy_CurrentHealth);

        yield return new WaitForSeconds(2f);

        //If enemy is dead then the player wins and the battle ends. 
        if (EnemyIsDead == true)
        {
            EnemyHUD.SetEnemyHP(EnemyUnit.Enemy_CurrentHealth = 0);
            CurrentState = BattleState.WIN;
            EndBattle();
        }

        //If the enemy is not defeated then it will be the enemy turn. 
        else
        {
            CurrentState = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }

    }

    IEnumerator PlayerFocus()
    {

        UIDialogueText.text = PlayerUnit.MC_Name + " Focuses!";

        float EnergyMultiplier = 1.5f;
        PlayerUnit.FocusBonus(1.5f);
        PlayerUnit.FocusBonus(PlayerUnit.Player_EnergyAttackStat * (int)EnergyMultiplier);

        PlayerUnit.UseEnergy(10);
        PlayerHUD.SetEnergy(PlayerUnit.Player_CurrentEnergy);

        yield return new WaitForSeconds(2f);

        CurrentState = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());

    }

    IEnumerator PlayerMultiShot()
    {

        //Checks to see if enemy has dropped to 0 HP.
        bool EnemyIsDead = EnemyUnit.TakeDamage(PlayerUnit.Player_AttackStat);

        float EnergyMultiplier = 1.5f;
        float SecondShot = 2.5f;
        int EnergyAttackBonus = (int)(EnergyMultiplier);


        PlayerUnit.UseEnergy(30);
        PlayerHUD.SetEnergy(PlayerUnit.Player_CurrentEnergy);

        //Enemy Takes Damage and Calls the 'TakeDamage' Function from the Player Script and then Multiplying it by An Energy Stat.
        EnemyUnit.TakeDamage(PlayerUnit.Player_EnergyAttackStat * (int)(EnergyAttackBonus + SecondShot));
        //Updates the Enemy's HP on Their HUD by accessing the Slider
        EnemyHUD.SetEnemyHP(EnemyUnit.Enemy_CurrentHealth);

        yield return new WaitForSeconds(2f);

        //If enemy is dead then the player wins and the battle ends. 
        if (EnemyIsDead == true)
        {
            EnemyHUD.SetEnemyHP(EnemyUnit.Enemy_CurrentHealth = 0);
            CurrentState = BattleState.WIN;
            EndBattle();
        }

        //If the enemy is not defeated then it will be the enemy turn. 
        else
        {
            CurrentState = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }

    }

    IEnumerator PlayerDragonCannon()
    {

        //Checks to see if enemy has dropped to 0 HP.
        bool EnemyIsDead = EnemyUnit.TakeDamage(PlayerUnit.Player_AttackStat);

        float EnergyMultiplier = 1.5f;
        int EnergyAttackBonus = (int)(EnergyMultiplier);

        PlayerUnit.UseEnergy(65);
        PlayerHUD.SetEnergy(PlayerUnit.Player_CurrentEnergy);

        //Enemy Takes Damage and Calls the 'TakeDamage' Function from the Player Script and then Multiplying it by An Energy Stat.
        EnemyUnit.TakeDamage(PlayerUnit.Player_EnergyAttackStat * (EnergyAttackBonus * 5));
        //Updates the Enemy's HP on Their HUD by accessing the Slider
        EnemyHUD.SetEnemyHP(EnemyUnit.Enemy_CurrentHealth);

        yield return new WaitForSeconds(2f);

        //If enemy is dead then the player wins and the battle ends. 
        if (EnemyIsDead == true)
        {
            EnemyHUD.SetEnemyHP(EnemyUnit.Enemy_CurrentHealth = 0);
            CurrentState = BattleState.WIN;
            EndBattle();
        }

        //If the enemy is not defeated then it will be the enemy turn. 
        else
        {
            CurrentState = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }

    }



    //This is the enemy's turn
    IEnumerator EnemyTurn()
    {

        //Flavour Text.
        UIDialogueText.text = EnemyUnit.EnemyName + " Attacks!";
        Debug.Log("Enemy Attacks");

        yield return new WaitForSeconds(1f);

        //Player will take damage 
        PlayerUnit.TakeDamage(EnemyUnit.Enemy_AttackStat);

        //Updates Player HP.
        PlayerHUD.SetPlayerHP(PlayerUnit.Player_CurrentHealth);

        //Checks to see if player has dropped to 0 HP.
        bool PlayerisDead = PlayerUnit.TakeDamage(EnemyUnit.Enemy_AttackStat);

        //Checks if player is defeated.
        if (PlayerisDead == true)
        {
            CurrentState = BattleState.LOSE;
            EndBattle();
            Debug.Log("End of Battle....you lose...");
        }
        //If Player is not dead then it will switch back to their turn.
        else
        {
            CurrentState = BattleState.PLAYERTURN;
            Player_Turn();
            Debug.Log("Switching back to Player!");
        }

        yield return new WaitForSeconds(1f);

        Player_Turn();
    }

    //Loads ending the battle depending on the result.
    void EndBattle()
    {
        if (CurrentState == BattleState.WIN)
        {
            //Displays A winning message.
            UIDialogueText.text = PlayerUnit.MC_Name + " Wins!";
            //Loops the function as to not load another state.
            EndBattle();
            Debug.Log("You Won!");
        }
        else if (CurrentState == BattleState.LOSE)
        {
            //Displays A losing message.  
            UIDialogueText.text = PlayerUnit.MC_Name + " Loses...";
            //Loops the function as to not load another state.
            EndBattle();
            Debug.Log("You Lose...");
        }
        else if (CurrentState == BattleState.FLEE)
        {
            //Displays A losing message.  
            UIDialogueText.text = PlayerUnit.MC_Name + " Flees...";
            //Loops the function as to not load another state.
            EndBattle();
            Debug.Log("You Escaped...");
        }
    }

    IEnumerator ITEM()
    {
        //Called From our Player Script 
        PlayerUnit.PlayerHeal(50);

        //Updates Health bar on the HUD by a number
        PlayerHUD.SetPlayerHP(PlayerUnit.Player_CurrentHealth);
        //Displays Dialouge Text
        UIDialogueText.text = PlayerUnit.MC_Name + "Heals!";
        yield return new WaitForSeconds(2f);

        //Switches Back to the Enemy's Turn
        CurrentState = BattleState.ENEMYTURN;
        //Starts the Process
        StartCoroutine(EnemyTurn());
    }

    //Initiates A Flee Result.
    IEnumerator FLEE()
    {
        yield return new WaitForSeconds(2f);
        CurrentState = BattleState.FLEE;
    }


    IEnumerator Check()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("Act Successful!");

        //Switches Back to the Enemy's Turn
        CurrentState = BattleState.ENEMYTURN;
        //Starts the Process
        StartCoroutine(EnemyTurn());
    }

    IEnumerator Guard()
    {
        PlayerUnit.PlayerDefenseBoost(10);

        PlayerUnit.PlayerDefenseBoost(PlayerUnit.Player_DefenseStat);

        yield return new WaitForSeconds(2f);
        Debug.Log("Guard Successful!");

        CurrentState = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnergyCharge()
    {
        PlayerUnit.GainEnergy(20);

        PlayerHUD.SetEnergy(PlayerUnit.Player_CurrentEnergy);

        UIDialogueText.text = PlayerUnit.MC_Name + "Charges Up!";
        yield return new WaitForSeconds(2f);

        //Switches Back to the Enemy's Turn
        CurrentState = BattleState.ENEMYTURN;
        //Starts the Process
        StartCoroutine(EnemyTurn());
    }

    IEnumerator SwitchCharacter()
    {
       
        UIDialogueText.text = "You try to switch... But you have no Allies!";
        yield return new WaitForSeconds(2f);

        //Switches Back to the Enemy's Turn
        CurrentState = BattleState.ENEMYTURN;
        //Starts the Process
        StartCoroutine(EnemyTurn());
    }

    //Runs Player Attack Coroutine.
    public void OnAttackButton()
    {
        //Runs attack if it's the player's turn.
        if (CurrentState != BattleState.PLAYERTURN)
        return;
        StartCoroutine(PlayerPhysicalAttack());
        UIDialogueText.text = PlayerUnit.MC_Name + " Attacks!";
        Debug.Log("Successful Attack!");
    }

    public void OnPowerShotButton()
    {
        //Runs attack if it's the player's turn.
        if (CurrentState != BattleState.PLAYERTURN)
            return;
        StartCoroutine(PlayerPowerShotAttack());
        UIDialogueText.text = PlayerUnit.MC_Name + " Shoots an Energy Blast!";
        Debug.Log("Successful Attack!");
    }

    public void OnFocusButton()
    {
        //Runs attack if it's the player's turn.
        if (CurrentState != BattleState.PLAYERTURN)
            return;
        StartCoroutine(PlayerFocus());
        UIDialogueText.text = PlayerUnit.MC_Name + " Focuses!";
        Debug.Log("Successful Attack!");
    }

    public void OnMulti_ShotButton()
    {
        //Runs attack if it's the player's turn.
        if (CurrentState != BattleState.PLAYERTURN)
            return;
        StartCoroutine(PlayerMultiShot());
        UIDialogueText.text = PlayerUnit.MC_Name + " Fires a Series of Blast!";
        Debug.Log("Successful Attack!");

    }

    public void OnDragon_CannonButton()
    {
        //Runs attack if it's the player's turn.
        if (CurrentState != BattleState.PLAYERTURN)
            return;
        StartCoroutine(PlayerDragonCannon());
        UIDialogueText.text = PlayerUnit.MC_Name + " Fires a Devestating Blast!";
        Debug.Log("Successful Attack!");

    }

    public void OnItemButton()
    {
        //This Will Heal the Player when clicked. 
        if (CurrentState != BattleState.PLAYERTURN)
        return;
        StartCoroutine(ITEM());
        UIDialogueText.text = PlayerUnit.MC_Name + " Heals!";
        Debug.Log("Successful Heal!");
    }

    public void OnFleeButton()
    {
        //This will quickly end the battle by 'Skipping it'
        if (CurrentState != BattleState.PLAYERTURN)
        return;
        StartCoroutine(FLEE());
        UIDialogueText.text = PlayerUnit.MC_Name + " Has Escaped!";
        Debug.Log("You escaped the battle!");
    }

    public void OnCheckButton()
    {
        //This Will Heal the Player when clicked. 
        if (CurrentState != BattleState.PLAYERTURN)
            return;
        StartCoroutine(Check());
        //Shows the Player how much health the Enemy Has Left.
        UIDialogueText.text = EnemyUnit.EnemyName + " ... " + EnemyUnit.Enemy_CurrentHealth + " HP out of " + EnemyUnit.Enemy_MaxHealth + " HP Left!" ;
        Debug.Log("Successful Heal!");
    }

    public void OnGuardButton()
    {
        //This Will let the Player guard when clicked. 
        if (CurrentState != BattleState.PLAYERTURN)
            return;
        StartCoroutine(Guard());
        UIDialogueText.text = PlayerUnit.MC_Name + " Guards!";
        Debug.Log("Successful Heal!");
    }

    public void OnEnergyChargeButton()
    {
        //This Will Restore the Player's Energy when clicked. 
        if (CurrentState != BattleState.PLAYERTURN)
            return;
        StartCoroutine(EnergyCharge());
        UIDialogueText.text = PlayerUnit.MC_Name + " Charges Up!";
        Debug.Log(PlayerUnit.MC_Name + "Restores Energy");
    }

    public void OnSwitchButton()
    {
        //This Will let the player swap when clicked. 
        if (CurrentState != BattleState.PLAYERTURN)
            return;
        StartCoroutine(SwitchCharacter());
        Debug.Log(PlayerUnit.MC_Name + "Attempts to Switch...");
    }



}
