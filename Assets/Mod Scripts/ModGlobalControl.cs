using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

//This script is used to save and load data between scenes.

public class ModGlobalControl : MonoBehaviour
{
    public static ModGlobalControl Instance;
    
    public GameObject _gameController;
    public GameController DataScript;

    public GameObject pauseCanvas;



    //pull Options menu
    public RectTransform OptionsTransform;
    public GameObject optionsBox;

    //Pull AutoFireToggle option
    public GameObject autoFireToggle;
    public GameObject enemyHealthToggle;


    //Pull dropdown menu
    public GameObject DifficultyDropDown;

    //These variables are used to determine if the levels are beaten, and are named after each boss.

    public bool LaserBoss;
    public bool MissileBoss;
    public bool ShieldBoss;
    public bool MeleeBoss;

    public int Weapon;

    //Each weapon needs an energy bar value
    public float Energy = 200;
    public float MaxEnergy = 200;

    public float LaserWeaponCharge = 300;
    public float LaserWeaponChargeMax = 300;

    public float MissileWeaponCharge = 20;
    public float MissileWeaponChargeMax = 20;

    public float ShieldWeaponCharge = 200;
    public float ShieldWeaponChargeMax = 250;

    public float MeleeWeaponCharge = 300;
    public float MeleeWeaponChargeMax = 300;

    public bool Boss1Defeated;
    public bool Boss2Defeated;
    public bool Boss3Defeated;
    public bool Boss4Defeated;

    public bool BossRoom;
    //Player data
    //This is the default stats for the normal difficulty
    public int Lives = 3;
    public float Health = 300;

        //For some reason the variables MaxLives and MaxHealth refused to change value when created or updated so this is the work around
    public int MaxLive = 3;
    public float MaximumHealth = 300;

    

    public int Score;
    public bool AutoFire ;
    public bool Practice;
    public int Difficulty;

    public bool Resumed;
    public bool Paused;
    public bool ToggleHealthBar;

    void Awake()
    {
        MaxLive = 3;
        MaximumHealth = 300;

     LaserWeaponCharge = 300;
     LaserWeaponChargeMax = 300;

     MissileWeaponCharge = 20;
     MissileWeaponChargeMax = 20;

     ShieldWeaponCharge = 250;
    ShieldWeaponChargeMax = 250;

     MeleeWeaponCharge = 300;
     MeleeWeaponChargeMax = 300;
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        pauseCanvas = GameObject.FindWithTag("PauseCanvas");

        Weapon = 0;

    }
    void Update()
    {

            
        if (Input.GetKeyDown(KeyCode.Q))
                {
                    SavePlayer();
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    LoadPlayer();
                }
        Pause();
        ManageStats();
        
    }
    void Start()
    {
        //GameObject _gameController = GameObject.FindGameObjectWithTag("GameController");
        //AutoFire = false;
        //ToggleHealthBar = true;
        //This is to make sure I can pause in every level
        ToggleHealthBar = true;
        
        pauseCanvas.SetActive(false);
    }


    private void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Resumed)
        {
            if (SceneManager.GetSceneByName("Level1").isLoaded)
            {

            
                if (Paused == false)
                {

                    Time.timeScale = 0;
                    Paused = true;
                    if (SceneManager.GetSceneByName("Level1").isLoaded)
                    {
                        pauseCanvas.SetActive(true);
                    }

                }
                else if (Paused == true)
                {
                    Time.timeScale = 1;
                    Paused = false;
                    if (SceneManager.GetSceneByName("Level1").isLoaded)
                    {
                        pauseCanvas.SetActive(false);
                    }
                    Resumed = false;

                }
            }
        }
    }
    public void ManageStats()
    {
        /*if (Lives > MaxLive)
        {
            Lives = MaxLive;
        }*/
        if (Health > MaximumHealth)
        {
            Health = MaximumHealth;
        }
    }








    public void SavePlayer()
    {
        SaveData.SavePlayer(this);
        print("saved");

    }

    public void LoadPlayer()
    {
        GameData data = SaveData.LoadPlayer();
        Score = data.Score;
        

        LaserWeaponCharge = data.LaserWeaponCharge;
        MissileWeaponCharge = data.MissileWeaponCharge;
        ShieldWeaponCharge = data.ShieldWeaponCharge;
        MeleeWeaponCharge =  data.MeleeWeaponCharge;
        Boss1Defeated = data.Boss1Defeated;
        Boss2Defeated = data.Boss2Defeated;
        Boss3Defeated = data.Boss3Defeated;
        Boss4Defeated = data.Boss4Defeated;


    print("loaded");
        //DataScript.UpdateScore();
        print(Score);


    }

    public void BossSave()
    {
        SaveData.SavePlayer(this);
        print("saved boss");

    }

    public void BossLoad()
    {
        GameData data = SaveData.LoadPlayer();
        Score = data.Score;
        

        LaserWeaponCharge = data.LaserWeaponCharge;
        MissileWeaponCharge = data.MissileWeaponCharge;
        ShieldWeaponCharge = data.ShieldWeaponCharge;
        MeleeWeaponCharge = data.MeleeWeaponCharge;
        Boss1Defeated = data.Boss1Defeated;
        Boss2Defeated = data.Boss2Defeated;
        Boss3Defeated = data.Boss3Defeated;
        Boss4Defeated = data.Boss4Defeated;


        print("loaded Boss");
        //DataScript.UpdateScore();
        print(Score);


    }

}

        