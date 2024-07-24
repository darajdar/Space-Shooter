using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//--- For some reason, using a variable that is equal to the ModGlobalControl instance did not allow the data to be saved and loaded, but directly referencing the isntance did.

//[System.Serializable]
public class GameControllerLS : MonoBehaviour
{



    public Text scoreText;
    public Text restartText;
    public Text gameOverText;
    public Text HealthText;

    private bool gameOver;
    private bool restart;
    public int Score = 0;
    public int Lives;
    public float Health;

    //Pull player data
    public GameObject player;
    public Vector3 PlayerPosition;
    public float PlayerPosX;
    public float PlayerPosZ;

    //Pull the pause menu data



    //Variables for map positioning in game
    private int _MapPositionX = 0;
    private int _MapPositionZ = 0;
    public int[,] MapPosition;
    public bool Paused;

    //Get position of the ship

    //Setting a dummy MenuIsActive for playing purposes
    private bool MenuIsActive;
    void Start()
    {


        //Reset all stats when player leaves and tries to return to level
        Score = ModGlobalControl.Instance.Score;
        Lives = ModGlobalControl.Instance.MaxLive;
        Health = ModGlobalControl.Instance.MaximumHealth;

        ModGlobalControl.Instance.LaserWeaponCharge = ModGlobalControl.Instance.LaserWeaponChargeMax;
        ModGlobalControl.Instance.MissileWeaponCharge = ModGlobalControl.Instance.MissileWeaponChargeMax;
        ModGlobalControl.Instance.ShieldWeaponCharge = ModGlobalControl.Instance.ShieldWeaponChargeMax;
        ModGlobalControl.Instance.MeleeWeaponCharge = ModGlobalControl.Instance.MeleeWeaponChargeMax;
        //Reset map position between worlds
        _MapPositionX = 0;
        _MapPositionZ = 0;


        MapPosition = new int[_MapPositionX, _MapPositionZ];
        //StartCoroutine(SpawnWaves());

        MenuIsActive = false;
        //Get the players position
        player = GameObject.FindGameObjectWithTag("Player");

        //Cache pause canvas
        //Set MapPosition equal to the X and Z variables
    }

    void Update()
    {

        UpdatePosition();
    }


    //Move the ship whenever the map updates.
    public void UpdatePosition()
    {

        //Keep track of the players position for movement
        if (player != null)
        {
            PlayerPosX = player.GetComponent<Transform>().position.x;
            PlayerPosZ = player.GetComponent<Transform>().position.z;
            PlayerPosition = player.GetComponent<Transform>().position;
        }


        {
            //print("Current position is:   " + _MapPositionX + "  " + _MapPositionZ);
        }

        if (PlayerPosition.x > 20)
        {
            //If player goes to the right of the screen, change the x map position by +1 and move the ship to the left of the screen.
            _MapPositionX++;
            player.GetComponent<Rigidbody>().position = new Vector3(-18, 0, PlayerPosZ);
        }

        if (PlayerPosition.x < -20)
        {
            //If player goes to the left of the screen, change the x map position by -1 and move the ship to the right of the screen.
            _MapPositionX--;
            player.GetComponent<Rigidbody>().position = new Vector3(18, 0, PlayerPosZ);
        }

        if (PlayerPosition.z < -3.8)
        {
            //If player goes to the bottom of the screen, change the z map position by -1 and move the ship to the top of the screen.
            _MapPositionZ++;
            player.GetComponent<Rigidbody>().position = new Vector3(PlayerPosX, 0, 13);
            print("true");
        }

        if (PlayerPosition.z > 13.8)
        {
            //If player goes to the top of the screen, change the x map position by +1 and move the ship to the bottom of the screen.
            _MapPositionZ--;
            player.GetComponent<Rigidbody>().position = new Vector3(PlayerPosX, 0, -3);
        }
    }



    //Restore players health on death and spawn at checkpoint
    void Retry()
    {

        ModGlobalControl.Instance.Health = ModGlobalControl.Instance.MaximumHealth;
        Health = ModGlobalControl.Instance.MaximumHealth;
    }


    //Pause the game
   
}