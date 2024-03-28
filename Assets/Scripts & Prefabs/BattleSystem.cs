using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting;
using static Enemy_Attack_Manager;
using Random = UnityEngine.Random;

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

    public Enemy_Attack_Manager RandomAttackSelection;

    //Sets the battle set to start and battles upon the SetBattle Function.
    void Start()
    {
        GameObject RandomAttackSelection = GameObject.Find("Attack Manager");
        Enemy_Attack_Manager AttackSelection = RandomAttackSelection.GetComponent<Enemy_Attack_Manager>();

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

    

    //Switches to the Player Punch.
    IEnumerator PlayerPunchAttack()
    {
        //Checks to see if enemy has dropped to 0 HP.
        bool EnemyIsDead = EnemyUnit.TakeDamage(PlayerUnit.Player_AttackStat);

        //Enemy Takes Damage and Calls the 'TakeDamage' Function from the Player Script 
        EnemyUnit.TakeDamage(PlayerUnit.Player_AttackStat);
        //Updates the Enemy's HP on Their HUD by accessing the Slider
        EnemyHUD.SetEnemyHP(EnemyUnit.Enemy_CurrentHealth);

        //Will Wait to If the Boolean Returns True or False.
        yield return new WaitForSeconds(5f);

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

    //Increases The Player's Attack Stat.
    IEnumerator PlayerWorkUp()
    {
        //Basic Dialogue Text.
        UIDialogueText.text = PlayerUnit.MC_Name + " Works Up!";

        //Creating A Multiplier Variable.
        float WorkUp = 1.5f;
        //Workup Bonus is Added to The Player's Attack Stat and Will Let them Deal Increased Phyical Damage.
        PlayerUnit.WorkUpBonus(1.5f);
        //Multiplies The Attack Stat by The Workup Multiplier. 
        PlayerUnit.WorkUpBonus(PlayerUnit.Player_AttackStat * (int)WorkUp);

        yield return new WaitForSeconds(5f);

        CurrentState = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }
    
    //Launches a Slightly Stronger Kick.
    IEnumerator PlayerRoundHouseKickAttack()
    {
        //Checks to see if enemy has dropped to 0 HP.
        bool EnemyIsDead = EnemyUnit.TakeDamage(PlayerUnit.Player_AttackStat);

        //Adds a Little more Damage to the RoundHouse Kick to Make a Viable Option to Punch.
        float RoundHouseKickBonus = 1.2f;
        //Converting that Boolean to Integer.
        int KickingBonus = (int)RoundHouseKickBonus;

        //Enemy Takes Damage and Calls the 'TakeDamage' Function from the Player Script 
        //This Will also Multiply the Attack Stat by the Kicking Bonus.
        EnemyUnit.TakeDamage(PlayerUnit.Player_AttackStat * KickingBonus);
        //Updates the Enemy's HP on Their HUD by accessing the Slider
        EnemyHUD.SetEnemyHP(EnemyUnit.Enemy_CurrentHealth);

        yield return new WaitForSeconds(5f);

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
    //Launches a Slightly Stronger Kick.
    IEnumerator PlayerMultiRushAttack()
    {
        //Checks to see if enemy has dropped to 0 HP.
        bool EnemyIsDead = EnemyUnit.TakeDamage(PlayerUnit.Player_AttackStat);

        //Adds a Little more Damage to the RoundHouse Kick to Make a Viable Option to Punch.
        float MultiRushBonus = 2f;
        int MultiPunch = (int)MultiRushBonus;

        //Enemy Takes Damage and Calls the 'TakeDamage' Function from the Player Script 
        //To Make the Attack a 'Rush' We can Do this Attack twice to Give the Illusion of Attacking Multiple Times.
        EnemyUnit.TakeDamage(PlayerUnit.Player_AttackStat * MultiPunch);
        UIDialogueText.text = PlayerUnit.MC_Name + "Attacks Again!";
        EnemyUnit.TakeDamage(PlayerUnit.Player_AttackStat * MultiPunch);
        //Updates the Enemy's HP on Their HUD by accessing the Slider
        EnemyHUD.SetEnemyHP(EnemyUnit.Enemy_CurrentHealth);

        yield return new WaitForSeconds(5f);

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
        //Checks Current Energy Upon Use.
        Debug.Log(PlayerUnit.Player_CurrentEnergy);
        //Checks if the Player's Energy is Below 10. 
        bool HasEnoughEnergy = PlayerUnit.Player_CurrentEnergy < 10;

        //If the Current Energy does not Meet the Required Energy than It will switch to a 'No Energy' State.
        if (HasEnoughEnergy == true)
        {

            //Switches to That New State. 
            CurrentState = BattleState.NOENERGY;
            StartCoroutine(PowerShotOutOfEnergy());
            
        }

        //If the Player does meet that Requirement, then they can Successfully Launch that Attack.
        else if (HasEnoughEnergy == false)
        {
            //Setting the Amount of Energy this Attack Uses.
            PlayerUnit.UseEnergy(15);
            //Updates the Energy Slider.
            PlayerHUD.SetEnergy(PlayerUnit.Player_CurrentEnergy);

            //This Gives our Energy Attacks Extra Impact!
            float EnergyMultiplier = 1.5f;
            //This Turns our Energy Multiplier into an Int as to Avoid an Error.
            int EnergyAttackBonus = (int)(EnergyMultiplier);

            UIDialogueText.text = PlayerUnit.MC_Name + " Shoots an Energy Blast!";

            //Enemy Takes Damage and Calls the 'TakeDamage' Function from the Player Script and then Multiplying it by An Energy Stat.
            EnemyUnit.TakeDamage(PlayerUnit.Player_EnergyAttackStat * EnergyAttackBonus);
            //Updates the Enemy's HP on Their HUD by accessing the Slider
            EnemyHUD.SetEnemyHP(EnemyUnit.Enemy_CurrentHealth);
            yield return new WaitForSeconds(0f);

        }

        yield return new WaitForSeconds(5f);

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

    //This Is the Energy Attack equivalent of 'Work up' but includes the Parameters of An Energy Attack. 
    IEnumerator PlayerFocus()
    {

        bool HasEnoughEnergy = PlayerUnit.Player_CurrentEnergy < 10;
        Debug.Log(PlayerUnit.Player_CurrentEnergy);

        if (HasEnoughEnergy == true)
        {
            yield return new WaitForSeconds(0f);
            CurrentState = BattleState.NOENERGY;
            StartCoroutine(FocusOutOfEnergy());
        }

        else if (HasEnoughEnergy == false)
        {
            PlayerUnit.UseEnergy(10);
            PlayerHUD.SetEnergy(PlayerUnit.Player_CurrentEnergy);

            UIDialogueText.text = PlayerUnit.MC_Name + " Focuses!";

            float EnergyMultiplier = 1.5f;
            PlayerUnit.FocusBonus(1.5f);
            PlayerUnit.FocusBonus(PlayerUnit.Player_EnergyAttackStat * (int)EnergyMultiplier);
            Debug.Log(PlayerUnit.Player_EnergyAttackStat);

            CurrentState = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());

        }

        yield return new WaitForSeconds(5f);

    }

    //This Is the Energy Attack equivalent of 'Multi Rush' but includes the Parameters of An Energy Attack. 
    IEnumerator PlayerMultiShot()
    {
        
        //Checks to see if enemy has dropped to 0 HP.
        bool EnemyIsDead = EnemyUnit.TakeDamage(PlayerUnit.Player_AttackStat);
        bool HasEnoughEnergy = PlayerUnit.Player_CurrentEnergy < 30;

        if (HasEnoughEnergy == true)
        {

            yield return new WaitForSeconds(0f);
            CurrentState = BattleState.NOENERGY;
            StartCoroutine(MultiShotOutofEnergy());
        }

        else if (HasEnoughEnergy == false)
        {

            PlayerUnit.UseEnergy(30);
            PlayerHUD.SetEnergy(PlayerUnit.Player_CurrentEnergy);

            float EnergyMultiplier = 1.5f;
            float SecondShot = 1.5f;
            int EnergyAttackBonus = (int)(EnergyMultiplier);

            UIDialogueText.text = PlayerUnit.MC_Name + " Fires a Barrage of Blast!";

            //Enemy Takes Damage and Calls the 'TakeDamage' Function from the Player Script and then Multiplying it by An Energy Stat.
            EnemyUnit.TakeDamage(PlayerUnit.Player_EnergyAttackStat * (int)(EnergyAttackBonus + SecondShot));
            EnemyUnit.TakeDamage(PlayerUnit.Player_EnergyAttackStat * (int)(EnergyAttackBonus + SecondShot));
            //Updates the Enemy's HP on Their HUD by accessing the Slider
            EnemyHUD.SetEnemyHP(EnemyUnit.Enemy_CurrentHealth);
            yield return new WaitForSeconds(0f);

        }

        yield return new WaitForSeconds(5f);

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

    //This is the Player's Ultimate Attack, Highest Damaging but for the Trade off of Consuming A Massive Portion of Energy.
    //Still Includes the Parameters of Other Energy Attacks.
    IEnumerator PlayerDragonCannon()
    {

        //Checks to see if enemy has dropped to 0 HP.
        bool EnemyIsDead = EnemyUnit.TakeDamage(PlayerUnit.Player_AttackStat);
        bool HasEnoughEnergy = PlayerUnit.Player_CurrentEnergy < 65;

        if (HasEnoughEnergy == true)
        {
            yield return new WaitForSeconds(0f);
            CurrentState = BattleState.NOENERGY;
            StartCoroutine(DragonCannonOutofEnergy());
        }


        else if (HasEnoughEnergy == false)
        {
            PlayerUnit.UseEnergy(65);
            PlayerHUD.SetEnergy(PlayerUnit.Player_CurrentEnergy);

            float EnergyMultiplier = 1.5f;
            int EnergyAttackBonus = (int)(EnergyMultiplier);

            //To Make this A True Ultimate Attack, We need a massive damage multiplier.
            float MassiveMultiplier = 7.5f;
            int DragonBonus = (int)(MassiveMultiplier);

            UIDialogueText.text = PlayerUnit.MC_Name + " Fires a Barrage of Blast!";

            //Enemy Takes Damage and Calls the 'TakeDamage' Function from the Player Script and then Multiplying it by An Energy Stat.
            //Though To Truly make this an ultimate attack, we need a unique modifier to make it truly powerful.
            EnemyUnit.TakeDamage(PlayerUnit.Player_EnergyAttackStat * EnergyAttackBonus * DragonBonus);
            //Updates the Enemy's HP on Their HUD by accessing the Slider
            EnemyHUD.SetEnemyHP(EnemyUnit.Enemy_CurrentHealth);
            yield return new WaitForSeconds(0f);

        }

        yield return new WaitForSeconds(5f);

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

    //These Unique IEnums do the same thing but for each of the four attacks. 
    //While there's a Waiting time for Each Move, These Coroutines can be Ran in the Meantime.
    //It's to Basically Say 'You cannot use this Attack.'
    //This Works By Comparing the Current Energy to the prerequisite. If not met then it would run these.
    IEnumerator PowerShotOutOfEnergy()
    {
        if (CurrentState == BattleState.NOENERGY)
        {
            if (PlayerUnit.Player_CurrentEnergy < 15)
            {
                UIDialogueText.text = ("Cannot Use Power Shot!");
                Debug.Log("No PowerShot");
                yield return new WaitForSeconds(1f);
            }
        }
    }

    IEnumerator FocusOutOfEnergy()
    {
        if (CurrentState == BattleState.NOENERGY)
        {
            if (PlayerUnit.Player_CurrentEnergy < 10)
            {
                UIDialogueText.text = ("Cannot Use Focus!");
                Debug.Log("No Focus");
                yield return new WaitForSeconds(3f);
                //Focus does not Normally Switch to An Enemies turn so we needed to run it here if the Amount Required is not Met.
                CurrentState = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
            }
          
        }
    }

    IEnumerator MultiShotOutofEnergy()
    {
        if (CurrentState == BattleState.NOENERGY)
        {
            if (PlayerUnit.Player_CurrentEnergy < 30)
            {
                UIDialogueText.text = ("Cannot Use MultiShot!");
                Debug.Log("No MultiShot");
                yield return new WaitForSeconds(1f);
            }

        }
    }

    IEnumerator DragonCannonOutofEnergy()
    {
        if (CurrentState == BattleState.NOENERGY)
        {
            if (PlayerUnit.Player_CurrentEnergy < 65)
            {
                UIDialogueText.text = ("Cannot Use Dragon Cannon!");
                Debug.Log("No Dragon Cannon");
                yield return new WaitForSeconds(1f);
            }

        }
    }

    //This is the enemy's turn
    IEnumerator EnemyTurn()
    {

        //Flavour Text.
        UIDialogueText.text = EnemyUnit.EnemyName + " Attacks!";
        Debug.Log("Enemy Attacks");

        AttackSelection AttackID = (AttackSelection)Random.Range(0,4);

        //Testing for a Specific Attack.
        //AttackSelection AttackID = (AttackSelection)3;

        if (AttackSelection.SinisterBeam == AttackID)
        {
            UIDialogueText.text = EnemyUnit.EnemyName + " Cast Sinister Beam!";
            yield return new WaitForSeconds(2f);
            StartCoroutine((RandomAttackSelection.GetComponent<Enemy_Attack_Manager>().SinisterBeamAttack()));
        }
        else if (AttackSelection.LightningBolt == AttackID)
        {
            UIDialogueText.text = EnemyUnit.EnemyName + " Cast Lightning Bolt!";
            yield return new WaitForSeconds(2f);
            StartCoroutine((RandomAttackSelection.GetComponent<Enemy_Attack_Manager>().LightningBoltAttack()));
        }
        else if (AttackSelection.Whiplash == AttackID)
        {
            UIDialogueText.text = EnemyUnit.EnemyName + " Cast Whiplash!";
            yield return new WaitForSeconds(2f);
            StartCoroutine((RandomAttackSelection.GetComponent<Enemy_Attack_Manager>().Whiplash()));

        }
        else if (AttackSelection.SpikeTrap == AttackID)
        {
            UIDialogueText.text = EnemyUnit.EnemyName + " Cast Spike Trap!";
            yield return new WaitForSeconds(2f);
            StartCoroutine((RandomAttackSelection.GetComponent<Enemy_Attack_Manager>().SpikeTrap()));
        }



        yield return new WaitForSeconds(5f);


        /*
        //Player will take damage 
        PlayerUnit.TakeDamage(EnemyUnit.Enemy_AttackStat);

        yield return new WaitForSeconds(2f);

        //Updates Player HP.
        PlayerHUD.SetPlayerHP(PlayerUnit.Player_CurrentHealth);
       */

        //Checks to see if player has dropped to 0 HP.
        bool PlayerisDead = PlayerUnit.TakeDamage(EnemyUnit.Enemy_AttackStat);


        //Checks if player is defeated.
        if (PlayerisDead == true)
        {
            PlayerHUD.SetPlayerHP(PlayerUnit.Player_CurrentHealth = 0);
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
    }

    //Loads ending the battle depending on the result.
    void EndBattle()
    {
        if (CurrentState == BattleState.WIN)
        {
            //Displays A winning message.
            UIDialogueText.text = PlayerUnit.MC_Name + " Wins!";
            //Loops the function as to not load another state.
            Debug.Log("You Won!");
            Application.Quit();
        }
        else if (CurrentState == BattleState.LOSE)
        {
            //Displays A losing message.  
            UIDialogueText.text = PlayerUnit.MC_Name + " Loses...";
            //Loops the function as to not load another state.
            Debug.Log("You Lose...");
            Application.Quit();

        }
        else if (CurrentState == BattleState.FLEE)
        {
            //Displays A losing message.  
            UIDialogueText.text = PlayerUnit.MC_Name + " Flees...";
            //Loops the function as to not load another state.
            Debug.Log("You Escaped...");
            Application.Quit();
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
       

        //Switches Back to the Enemy's Turn
        CurrentState = BattleState.ENEMYTURN;
        yield return new WaitForSeconds(5f);
        //Starts the Process
        StartCoroutine(EnemyTurn());
    }

    //Initiates A Flee Result.
    //Ends the Battle Early.
    IEnumerator FLEE()
    {
        yield return new WaitForSeconds(2f);
        CurrentState = BattleState.FLEE;
    }


    IEnumerator Check()
    {
        Debug.Log("Check Successful!");

        //Switches Back to the Enemy's Turn
        CurrentState = BattleState.ENEMYTURN;
        yield return new WaitForSeconds(5f);
        //Starts the Process
        StartCoroutine(EnemyTurn());
    }

    IEnumerator Guard()
    {   
        //Adds to The Player's Defense Amount by 10.
        PlayerUnit.PlayerDefenseBoost(10);

        PlayerUnit.PlayerDefenseBoost(PlayerUnit.Player_DefenseStat);

        Debug.Log("Guard Successful!");

        CurrentState = BattleState.ENEMYTURN;
        yield return new WaitForSeconds(5f);
        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnergyCharge()
    {
        //As a Way to ReGain Energy once lost, the Player can 'Charge' their Energy though they'd have to sacrifice their turn. Increasing the Current Energy by 20.
        PlayerUnit.GainEnergy(20);

        PlayerHUD.SetEnergy(PlayerUnit.Player_CurrentEnergy);

        UIDialogueText.text = PlayerUnit.MC_Name + "Charges Up!";

        //Switches Back to the Enemy's Turn
        CurrentState = BattleState.ENEMYTURN;
        yield return new WaitForSeconds(5f);
        //Starts the Process
        StartCoroutine(EnemyTurn());
    }

    IEnumerator SwitchCharacter()
    {
       
        //Since there's No Other Avaliable Playable Characters, this does Nothing. Making for an Easy Turn Skip.
        //Though this can be Used if the Player had access to other characters.
        UIDialogueText.text = "You try to switch... But you have no Allies!";
        
        //Switches Back to the Enemy's Turn
        CurrentState = BattleState.ENEMYTURN;
        yield return new WaitForSeconds(5f);
        //Starts the Process
        StartCoroutine(EnemyTurn());
    }

    //Here is a List of Button Functions, Once thbe Buttons are Pressed. They Each Run their Own Seperate CoRoutines, with their own functions.

    //Runs Player Attack Coroutine.
    public void OnPunchButton()
    {
        //Runs attack if it's the player's turn.
        if (CurrentState != BattleState.PLAYERTURN)
        return;
        StartCoroutine(PlayerPunchAttack());
        UIDialogueText.text = PlayerUnit.MC_Name + " Attacks!";
        Debug.Log("Successful Attack!");
    }
    public void OnWorkUpButton()
    {
        //Runs attack if it's the player's turn.
        if (CurrentState != BattleState.PLAYERTURN)
            return;
        StartCoroutine(PlayerWorkUp());
        UIDialogueText.text = PlayerUnit.MC_Name + " Works Himself Up!";
        Debug.Log("Successful Attack!");
    }
    public void OnRoundHouseKickButton()
    {
        //Runs attack if it's the player's turn.
        if (CurrentState != BattleState.PLAYERTURN)
            return;
        StartCoroutine(PlayerRoundHouseKickAttack());
        UIDialogueText.text = PlayerUnit.MC_Name + " Launches a Devestating Kick!";
        Debug.Log("Successful Attack!");
    }

    public void OnMultiRushButton()
    {
        //Runs attack if it's the player's turn.
        if (CurrentState != BattleState.PLAYERTURN)
            return;
        StartCoroutine(PlayerMultiRushAttack());
        UIDialogueText.text = PlayerUnit.MC_Name + " Launches a Series of Attacks!";
        Debug.Log("Successful Attack!");
    }


    public void OnPowerShotButton()
    {
        //Runs attack if it's the player's turn.
        if (CurrentState != BattleState.PLAYERTURN)
            return;
        StartCoroutine(PlayerPowerShotAttack());
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
