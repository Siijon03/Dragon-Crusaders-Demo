using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy_Namer : MonoBehaviour
{
    //This Script Handles the Inital Panel that you will see when starting the game.
    //Getting our Enemy Prefab.
    public GameObject EnemyPrefab;
    //Calling the Current Panel.
    public GameObject HidePanel;

    //Defining our Enemy Unit.
    Enemy_Unit EnemyUnit;

    //Defining Text.
    public TextMeshProUGUI ButtonText;

    void Start()
    {
        //On Start we are getting the information for our enemy unit.
        EnemyUnit = EnemyPrefab.GetComponent<Enemy_Unit>();
        //The Panel is Currently Set to True.
        HidePanel.SetActive(true);
        //Getting the Enemy's Name.
        ButtonText.text = "Fight " + EnemyUnit.EnemyName;

    }

    //Setting up an IEmuerator.
    IEnumerator Loading()
    {
        //This Will Hide the Panel.
        HidePanel.SetActive(false);
        yield return new WaitForSeconds(0f);
    }

    //Basic Button Logic.
    public void OnOpeningButton()
    {
        StartCoroutine(Loading());
        Debug.Log("You Clicked the Loading Button!");
    }

}
