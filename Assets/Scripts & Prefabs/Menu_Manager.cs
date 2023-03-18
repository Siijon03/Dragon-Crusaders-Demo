using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Manager : MonoBehaviour
{
    //Storing a Bunch of Different Menus that can be called later.
    public GameObject StartMenu;
    public GameObject HUDMenu;
    public GameObject BattleButtonsMenu;
    public GameObject AttackOptionsMenu;
    public GameObject ItemMenu;
    public GameObject ActMenu;
    public GameObject PhysicalAttackMenu;
    public GameObject EnergyAttackMenu;

    //Storing Possible MenuStates
    public enum MenuSelection { STARTMENU, PLAYERHUD, ATTACKOPTIONS, ITEMMENU, ACTMENU, PHYSICALATTACKS, ENERGYATTACKS }

    //Can be Used to Call Listed Menu Selects.
    public MenuSelection CurrentMenuState;

    // This Sets What will be initally shown or hidden.
    void Start()
    {
        //This controls what will be seen upon start up.
        StartMenu.SetActive(true);
        HUDMenu.SetActive(false);
        BattleButtonsMenu.SetActive(false);
        AttackOptionsMenu.SetActive(false);
        ItemMenu.SetActive(false);
        ActMenu.SetActive(false);
        PhysicalAttackMenu.SetActive(false);
        EnergyAttackMenu.SetActive(false);

        CurrentMenuState = MenuSelection.STARTMENU;

    }

    void Update()
    {
        if (CurrentMenuState == MenuSelection.PLAYERHUD)
        {
            if (Input.GetKeyUp(KeyCode.Z))
            {
                HUDMenu.SetActive(false);
                BattleButtonsMenu.SetActive(false);
                StartMenu.SetActive(true);
                CurrentMenuState=MenuSelection.STARTMENU;
            }
        }

        if (CurrentMenuState == MenuSelection.ATTACKOPTIONS)
        {
            if (Input.GetKeyUp(KeyCode.Z))
            {
                BattleButtonsMenu.SetActive(false);
                AttackOptionsMenu.SetActive(false);
                BattleButtonsMenu.SetActive(true);
                HUDMenu.SetActive(true);
                CurrentMenuState = MenuSelection.PLAYERHUD;
            }
        }

        if (CurrentMenuState == MenuSelection.ITEMMENU)
        {
            if (Input.GetKeyUp(KeyCode.Z))
            {
                HUDMenu.SetActive(true);
                BattleButtonsMenu.SetActive(true);
                ItemMenu.SetActive(false);
                CurrentMenuState = MenuSelection.PLAYERHUD;
            }
        }

        if (CurrentMenuState == MenuSelection.ACTMENU)
        {
            if (Input.GetKeyUp(KeyCode.Z))
            {
                HUDMenu.SetActive(true);
                BattleButtonsMenu.SetActive(true);
                ActMenu.SetActive(false);
                CurrentMenuState = MenuSelection.PLAYERHUD;
            }
        }

        if (CurrentMenuState == MenuSelection.PHYSICALATTACKS)
        {
            if (Input.GetKeyUp(KeyCode.Z))
            {
                PhysicalAttackMenu.SetActive(false);
                AttackOptionsMenu.SetActive(true);
                CurrentMenuState = MenuSelection.ATTACKOPTIONS;
            }
        }

        if (CurrentMenuState == MenuSelection.ENERGYATTACKS)
        {
            if (Input.GetKeyUp(KeyCode.Z))
            {
                EnergyAttackMenu.SetActive(false);
                AttackOptionsMenu.SetActive(true);
                CurrentMenuState = MenuSelection.ATTACKOPTIONS;
            }
        }

    }

    //This will Hide the Start Menu.
    IEnumerator StartMenuButtonPressed()
    {

        StartMenu.SetActive(false);
        yield return new WaitForSeconds(0f);

        StartCoroutine(HUDMenuButtonPressed());
        StartCoroutine(LoadBattleButtons());

    }

    //This will Show the Player HUD.
    IEnumerator HUDMenuButtonPressed()
    {
        
        HUDMenu.SetActive(true);
        yield return new WaitForSeconds(0f);
        CurrentMenuState = MenuSelection.PLAYERHUD;
    }

    //This Will Load your options. (Attack, Act, Item and Flee)
    IEnumerator LoadBattleButtons()
    {
        
        BattleButtonsMenu.SetActive(true);
        yield return new WaitForSeconds(0f);
        CurrentMenuState = MenuSelection.PLAYERHUD;

    }

    //This Will Show Two Buttons. Physical Attack or Energy Attack.
    IEnumerator LoadAttackOptionsMenu()
    {

        BattleButtonsMenu.SetActive(true);
        AttackOptionsMenu.SetActive(true);
        PhysicalAttackMenu.SetActive(false);
        EnergyAttackMenu.SetActive(false);

        yield return new WaitForSeconds(0f);
        CurrentMenuState = MenuSelection.ATTACKOPTIONS;
    }

    //This Will Show your Item Menu.
    IEnumerator LoadItemMenu()
    {   
        
        ItemMenu.SetActive(true);
        yield return new WaitForSeconds(0f);
        CurrentMenuState = MenuSelection.ITEMMENU;

    }

    //This Will Show your Act Menu.
    IEnumerator LoadActMenu()
    {

        ActMenu.SetActive(true);
        yield return new WaitForSeconds(0f);
        CurrentMenuState = MenuSelection.ACTMENU;
    }

    //This will show your Physical Attack Options.
    IEnumerator PhysicalAttackOptions()
    {
        PhysicalAttackMenu.SetActive(true);
        yield return new WaitForSeconds(0f);
        CurrentMenuState = MenuSelection.PHYSICALATTACKS;
    }

    //This will show your Energy Attack Options.
    IEnumerator EnergyAttackOptions()
    {
  
        EnergyAttackMenu.SetActive(true);
        yield return new WaitForSeconds(0f);
        CurrentMenuState = MenuSelection.ENERGYATTACKS;
    }


    //When Clicked this will hide the Start Menu.
    public void OnStartMenu()
    {
        //When this will hide the start menu when running
        StartCoroutine(StartMenuButtonPressed());
    }

    //When Attack Button is Pressed this will activate options for an attack Menu.
    public void ActivateAttackMenu()
    {
        StartCoroutine(LoadAttackOptionsMenu());
    }

    //When Item Button is Pressed this will activate options for an Item Menu.
    public void ActivateItemMenu()
    {
        StartCoroutine(LoadItemMenu());
    }

    //When Act Button is Pressed this will activate options for an Act Menu.
    public void ActivateActMenu()
    {
        StartCoroutine(LoadActMenu());
    }

    //When The Physical Attack Button is Pressed in the Attack Options Menu then will load an option for Physical attacks.
    public void ActivatePhysicalAttackMenu()
    {
        StartCoroutine(PhysicalAttackOptions());
    }

    //When The Energy Attack Button is Pressed in the Attack Options Menu then will load an option for Energy attacks.
    public void ActivateEnergyAttackMenu()
    {
        StartCoroutine(EnergyAttackOptions());
    }
}
