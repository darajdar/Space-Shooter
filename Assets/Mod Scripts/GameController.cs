using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//--- For some reason, using a variable that is equal to the ModGlobalControl instance did not allow the data to be saved and loaded, but directly referencing the isntance did.

//[System.Serializable]
public class GameController : MonoBehaviour
{
    

    
    
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;

    public Text scoreText;
    public Text restartText;
    public Text gameOverText;
    public Text HealthText;
    public Text LivesText;
    public Text EnergyText;
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
    
    public GameObject pauseMenuBox;

    //Variables for map positioning in game
    public int _MapPositionX = 0;
    public int _MapPositionZ = 0;
    public int[,] MapPosition;
    public bool Transitioning;
    public bool Paused;

    //Variables to check which level it is, as well as the size of the map of the level for x and z
    public int Level;
    public int MinMapX;
    public int MinMapZ;
    public int MaxMapX;
    public int MaxMapZ;
    //Change the size of this with every enemy, or learn how to grab prefabs through c#
    public GameObject[] Backgrounds;
    public GameObject[,] test;
    //Grab enemy game objects to instantiate later.
    public GameObject[] Enemies /* = new GameObject[2]*/;
    public GameObject[] EnemyClones /* = new GameObject[2]*/;

    //Grab the item game objects to instantiate them later.
    public GameObject[] Items;
    public GameObject[] ItemClones;
    //public GameObject[] PermanentItems;

    //Grab the asteroids to instantiate them later.
    public GameObject[] Hazards;
    public GameObject[] HazardClones;

    //Cache Healthbar data
    public Transform healthBarPanel;
    public Slider healthBarSlider;

    //Cache energy bar data
    public GameObject energyPanelDisplay;
    public Transform energyBarPanel;
    public Slider energyBarSlider;
    public Image energyBarBackground;
    public Image energyBarFill;
    //Get position of the ship

    //Get the main GUI data, create variable for checking if in bossroom
    //public GameObject titleCanvas;
    public GameObject[] GUIMap;
    public GameObject Map;
    
    //Get playerweaponmenu gameobject for weapon switching
    public GameObject playerWeaponMenu;
    public bool MenuIsActive;
    void Start()
    {
        
        HealthText.text = "";
        EnergyText.text = "";
        scoreText.text = "";
        gameOverText.text = "";
        restartText.text = "";
        LivesText.text = "";
        gameOver = false;
        restart = false;
        Score = ModGlobalControl.Instance.Score;
        Lives = ModGlobalControl.Instance.MaxLive;
        Health = ModGlobalControl.Instance.MaximumHealth;

        //Reset map position between worlds
        _MapPositionX = 0;
        _MapPositionZ = 0;

        //Set values for healthbar
        healthBarSlider.minValue = 0;
        healthBarSlider.maxValue = ModGlobalControl.Instance.MaximumHealth;

        //Set values for energybar
        energyBarSlider.minValue = 0;
        energyBarSlider.maxValue = ModGlobalControl.Instance.MaxEnergy;

        //Reset health to max once level starts
        ModGlobalControl.Instance.Health = ModGlobalControl.Instance.MaximumHealth;
        

        //Cache all the components to the gui map
        GUIMap = GameObject.FindGameObjectsWithTag("GUIMap");

        //Set wepaon menu to false at start
        playerWeaponMenu = GameObject.Find("PlayerWeaponMenu");
        playerWeaponMenu.SetActive(false);
        MenuIsActive = false;
        UpdateScore();

        //Get the players position
        player = GameObject.FindGameObjectWithTag("Player");



        

        //Get all the backgrounds in a level, all enemies in the backgrounds,  and Set all maps off at start of level
        Backgrounds = GameObject.FindGameObjectsWithTag("Background");
        //Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        

        //Find and set the variables for the level
        if (SceneManager.GetSceneByName("Level1").isLoaded)
        {
            Level = 1;
            //Max size of the grid in the game
            MinMapX = 1;
            MinMapZ = 0;
            MaxMapX = 3;
            MaxMapZ = 4;
            _MapPositionX = 2;
            _MapPositionZ = 0;

            //If player has been to bossroom, as long as they have lives and die, send them back to bossroom.
            if (ModGlobalControl.Instance.BossRoom)
            {
                _MapPositionX = 2;
                _MapPositionZ = 3;
                ModGlobalControl.Instance.BossLoad();

                //Make sure player still loses a life
                
            } else
            {
                ModGlobalControl.Instance.LaserWeaponCharge = ModGlobalControl.Instance.LaserWeaponChargeMax;
                ModGlobalControl.Instance.MissileWeaponCharge = ModGlobalControl.Instance.MissileWeaponChargeMax;
                ModGlobalControl.Instance.ShieldWeaponCharge = ModGlobalControl.Instance.ShieldWeaponChargeMax;
                ModGlobalControl.Instance.MeleeWeaponCharge = ModGlobalControl.Instance.MeleeWeaponChargeMax;
                ModGlobalControl.Instance.BossRoom = false;
            }
        }

        MapPosition = new int[_MapPositionX, _MapPositionZ];
        ChangeMap();
    }

    void Update()
    {
        //Show the map while x is being pressed
        if (Input.GetKey(KeyCode.X))
        {
            StartCoroutine(FlashMap());
        } 
        

        ManageHealth();
        ManageEnergy();
        DeathFunction();
        UpdateScore();
        WeaponChange();
        StartCoroutine(Respawn());
    }

    void FixedUpdate()
    {
        UpdatePosition();
    }

    //Reset stats after gameover, placeholder for later.
    void ResetStats()
    {
        ModGlobalControl.Instance.Score = 0;
        ModGlobalControl.Instance.Lives = ModGlobalControl.Instance.MaxLive;
        ModGlobalControl.Instance.Health = ModGlobalControl.Instance.MaximumHealth;

        ModGlobalControl.Instance.LaserWeaponCharge = ModGlobalControl.Instance.LaserWeaponChargeMax;
        ModGlobalControl.Instance.MissileWeaponCharge = ModGlobalControl.Instance.MissileWeaponChargeMax;
        ModGlobalControl.Instance.ShieldWeaponCharge = ModGlobalControl.Instance.ShieldWeaponChargeMax;
        ModGlobalControl.Instance.MeleeWeaponCharge = ModGlobalControl.Instance.MeleeWeaponChargeMax;
        Score = 0;
        Lives = ModGlobalControl.Instance.MaxLive;
        Health = ModGlobalControl.Instance.MaximumHealth;
        //--- Get back to this. Lives should go back to whatever difficulty it is set to.
    }
    IEnumerator Respawn()
    {
        //yield return new WaitForSeconds(startWait);
        //while (true)
        //{
            //for (int i = 0; i < hazardCount; i++)
            //{
             //   GameObject hazard = hazards[Random.Range(0, hazards.Length)];
             //   Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
             //   Quaternion spawnRotation = Quaternion.identity;
             //   Instantiate(hazard, spawnPosition, spawnRotation);
             //   yield return new WaitForSeconds(spawnWait);
            //}
            

            if (gameOver)
            {
            yield return new WaitForSeconds(waveWait);
            restart = true;
            }
        //}
    }

    public void AddScore(int newScoreValue)
    {
        //--- For some reason, using a variable that is equal to the ModGlobalControl instance did not allow the data to be saved and loaded, but directly referencing the isntance did. Maybe 
        // because the data isn't being directly modified in that script is why I cant use it that way.
        Score += newScoreValue;
        ModGlobalControl.Instance.Score += newScoreValue;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + ModGlobalControl.Instance.Score;
    }

    public void GameOver()
    {
        gameOverText.text = "Game Over!";
        gameOver = true;
        //Lives--;
        ModGlobalControl.Instance.Lives--;
        SaveDataToPlayerControl();
    }

    public void SaveDataToPlayerControl() {
        //---RUnning a function to update the instance values didnt seem to work either.

        //ModGlobalControl.Instance.Lives = Lives;
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

        
       
            //print("Current position is:   " + _MapPositionX + "  " + _MapPositionZ);
        

        if (Level == 1)
        {
            if (PlayerPosition.x > 20.2 && _MapPositionX < MaxMapX &&!ModGlobalControl.Instance.BossRoom)
            {
                //If player goes to the right of the screen, change the x map position by +1 and move the ship to the left of the screen. 
                player.GetComponent<Rigidbody>().position = new Vector3(-19.6f, 0, PlayerPosZ);
                _MapPositionX++;
                //Update the map position in game
                ChangeMap();
                print("Current position is:   " + _MapPositionX + "  " + _MapPositionZ);
            }

            if (PlayerPosition.x < -20.2 && _MapPositionX > MinMapX && !ModGlobalControl.Instance.BossRoom)
            {
                //If player goes to the left of the screen, change the x map position by -1 and move the ship to the right of the screen.
                
                player.GetComponent<Rigidbody>().position = new Vector3(19.6f, 0, PlayerPosZ);
                _MapPositionX--;
                //Update the map position in game
                ChangeMap();
                print("Current position is:   " + _MapPositionX + "  " + _MapPositionZ);
            }

            if (PlayerPosition.z < -3.8  && _MapPositionZ > MinMapZ && !ModGlobalControl.Instance.BossRoom)
            {
                //If player goes to the bottom of the screen, change the z map position by -1 and move the ship to the top of the screen.
                
                player.GetComponent<Rigidbody>().position = new Vector3(PlayerPosX, 0, 13.6f);
                _MapPositionZ--;
                //Update the map position in game
                ChangeMap();
                print("Current position is:   " + _MapPositionX + "  " + _MapPositionZ);

            }

            if (PlayerPosition.z > 13.8 && _MapPositionZ < MaxMapZ && !ModGlobalControl.Instance.BossRoom)
            {
                //If player goes to the top of the screen, change the x map position by +1 and move the ship to the bottom of the screen.
                
                player.GetComponent<Rigidbody>().position = new Vector3(PlayerPosX, 0, -3.6f);
                _MapPositionZ++;
                //Update the map position in game
                ChangeMap();
                print("Current position is:   " + _MapPositionX + "  " + _MapPositionZ);
            }
        }
        //MapPosition = new int[_MapPositionX, _MapPositionZ];
    }
    
    void ManageHealth()
    {
        //Convert the health value to a string
        HealthText.text = ModGlobalControl.Instance.Health.ToString() + "/" + ModGlobalControl.Instance.MaximumHealth.ToString();
        LivesText.text = ModGlobalControl.Instance.Lives.ToString();

        ModGlobalControl.Instance.Health = Mathf.Clamp(ModGlobalControl.Instance.Health, 0, ModGlobalControl.Instance.MaximumHealth);

        healthBarSlider.value = ModGlobalControl.Instance.Health;
        
    }

    void ManageEnergy()
    {//Switch between energy bars for each weapon
        switch(ModGlobalControl.Instance.Weapon)
        {
            case 0:
                energyPanelDisplay.SetActive(false);
                break;

            case 1:
                //Laser Weapon Display
                energyPanelDisplay.SetActive(true);
                //ModGlobalControl.Instance.Energy = ModGlobalControl.Instance.LaserWeaponCharge;
                //ModGlobalControl.Instance.MaxEnergy = ModGlobalControl.Instance.LaserWeaponChargeMax;

                
                EnergyText.text = ModGlobalControl.Instance.LaserWeaponCharge.ToString() + "/" + ModGlobalControl.Instance.LaserWeaponChargeMax.ToString();
                ModGlobalControl.Instance.LaserWeaponCharge = Mathf.Clamp(ModGlobalControl.Instance.LaserWeaponCharge, 0, ModGlobalControl.Instance.LaserWeaponChargeMax);
                energyBarSlider.value = ModGlobalControl.Instance.LaserWeaponCharge;

                energyBarPanel.GetComponent<Image>().color = new Color(1f, .73f, 0f, .6f);
                energyBarFill.color = new Color(1f, .73f, 0f, .6f);
                energyBarBackground.color = new Color(.4f, .3f, 0f, .4f);
                
                //Set values for energybar
                energyBarSlider.minValue = 0;
                energyBarSlider.maxValue = ModGlobalControl.Instance.LaserWeaponChargeMax;
                break;

            case 2:
                //Shield Weapon Display
                energyPanelDisplay.SetActive(true);

                energyBarPanel.GetComponent<Image>().color = new Color(.05f, 1f, 0f, .6f);
                energyBarFill.color = new Color(.05f, 1f, 0f, .6f);
                energyBarBackground.color = new Color(.2f, .3f, .1f, .4f);

                EnergyText.text = ModGlobalControl.Instance.ShieldWeaponCharge.ToString() + "/" + ModGlobalControl.Instance.ShieldWeaponChargeMax.ToString();
                ModGlobalControl.Instance.ShieldWeaponCharge = Mathf.Clamp(ModGlobalControl.Instance.ShieldWeaponCharge, 0, ModGlobalControl.Instance.ShieldWeaponChargeMax);
                energyBarSlider.value = ModGlobalControl.Instance.ShieldWeaponCharge;

                //Set values for energybar

                energyBarSlider.minValue = 0;
                energyBarSlider.maxValue = ModGlobalControl.Instance.ShieldWeaponChargeMax;
                break;

            case 3:
                //Missile Weapon Display

                energyPanelDisplay.SetActive(true);
                //ModGlobalControl.Instance.Energy = ModGlobalControl.Instance.LaserWeaponCharge;
                //ModGlobalControl.Instance.MaxEnergy = ModGlobalControl.Instance.LaserWeaponChargeMax;

                energyBarPanel.GetComponent<Image>().color = new Color(.8f, .25f, .2f, .6f);
                energyBarFill.color = new Color(.8f, .25f, .2f, .6f);
                energyBarBackground.color = new Color(.5f, .1f, .05f, .4f);

                EnergyText.text = ModGlobalControl.Instance.MissileWeaponCharge.ToString() + "/" + ModGlobalControl.Instance.MissileWeaponChargeMax.ToString();
                ModGlobalControl.Instance.MissileWeaponCharge = Mathf.Clamp(ModGlobalControl.Instance.MissileWeaponCharge, 0, ModGlobalControl.Instance.MissileWeaponChargeMax);
                energyBarSlider.value = ModGlobalControl.Instance.MissileWeaponCharge;

                

                //Set values for energybar
                energyBarSlider.minValue = 0;
                energyBarSlider.maxValue = ModGlobalControl.Instance.MissileWeaponChargeMax;
                break;

            case 4:

                energyPanelDisplay.SetActive(true);
                //ModGlobalControl.Instance.Energy = ModGlobalControl.Instance.LaserWeaponCharge;
                //ModGlobalControl.Instance.MaxEnergy = ModGlobalControl.Instance.LaserWeaponChargeMax;

                energyBarPanel.GetComponent<Image>().color = new Color(.75f, .2f, .75f, .6f);
                energyBarFill.color = new Color(.75f, .2f, .75f, .6f);
                energyBarBackground.color = new Color(.5f, .2f, .75f, .4f);

                EnergyText.text = ModGlobalControl.Instance.MeleeWeaponCharge.ToString() + "/" + ModGlobalControl.Instance.MeleeWeaponChargeMax.ToString();
                ModGlobalControl.Instance.MeleeWeaponCharge = Mathf.Clamp(ModGlobalControl.Instance.MeleeWeaponCharge, 0, ModGlobalControl.Instance.MeleeWeaponChargeMax);
                energyBarSlider.value = ModGlobalControl.Instance.MeleeWeaponCharge;



                //Set values for energybar
                energyBarSlider.minValue = 0;
                energyBarSlider.maxValue = ModGlobalControl.Instance.MeleeWeaponChargeMax;

                break;
            default:
                break;
        }

    }

    //Restore players health on death and spawn at checkpoint
    void Retry()
    {
        
        ModGlobalControl.Instance.Health = ModGlobalControl.Instance.MaximumHealth;
        Health = ModGlobalControl.Instance.MaximumHealth;
    }

    void DeathFunction()
    {
        //If you die and have lives, start the game over immediately after wave ends
        if (restart && ModGlobalControl.Instance.Lives > 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Retry();

            /*//If defeated by boss, give players another try.
            if (ModGlobalControl.Instance.BossRoom)
            {
                _MapPositionX = 2;
                _MapPositionZ = 3;
                ModGlobalControl.Instance.LoadPlayer();
            }*/

        }
        else if (restart && ModGlobalControl.Instance.Lives <= 0)
        //If you die and have no lives, require and input for restart 
        {

            restartText.text = "Press 'R' for Restart";
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                ResetStats();
                ModGlobalControl.Instance.BossRoom = false;
            }
        }
    }

    void ChangeMap()
    {
        //Do this set of work for the first level.
        if (SceneManager.GetSceneByName("Level1").isLoaded)
        { 
            //For level, the x range is 1-3, and the z range is 0,5
            switch (_MapPositionZ)
            {
                //Change the other backgrounds to false, delete all enemy clones in the area, and enable the one you are currently in when called.


                case (0):

                    switch (_MapPositionX)
                    {
                        case 0:
                            foreach (GameObject Background in Backgrounds)
                            {
                                Background.SetActive(false);
                            }
                            foreach (GameObject Enemy in EnemyClones)
                            {

                                Destroy(Enemy);


                            }
                            foreach (GameObject Item in ItemClones)
                            {
                                Destroy(Item);


                            }
                            foreach (GameObject Hazard in HazardClones)
                            {
                                Destroy(Hazard);


                            }
                            foreach (GameObject Map in GUIMap)
                            {
                                Map.GetComponent<Image>().color = new Color(.2f, .2f, .2f, .5f);
                            }

                            Backgrounds[0].SetActive(true);


                            //Create clones of enemies in the area. Then delete them one the area is changed.


                            //Create clones of the hazards in the area.

                            GUIMap[0].GetComponent<Image>().color = new Color(1f, 0, 0, .5f);
                            StartCoroutine(FlashMap());
                            break;

                        case 1:
                            foreach (GameObject Background in Backgrounds)
                            {
                                Background.SetActive(false);

                            }
                            foreach (GameObject Enemy in EnemyClones)
                            {
                                Destroy(Enemy);


                            }
                            foreach (GameObject Item in ItemClones)
                            {
                                Destroy(Item);


                            }
                            foreach (GameObject Hazard in HazardClones)
                            {
                                Destroy(Hazard);


                            }
                            foreach (GameObject Map in GUIMap)
                            {
                                Map.GetComponent<Image>().color = new Color(.2f, .2f, .2f, .5f);
                            }

                            Backgrounds[1].SetActive(true);
                            EnemyClones[0] = Instantiate(Enemies[0], new Vector3(-3.4f, 0, 10.3f), Quaternion.identity, Backgrounds[1].transform);
                            EnemyClones[1] = Instantiate(Enemies[0], new Vector3(-8.3f, 0, 6.75f), Quaternion.identity, Backgrounds[1].transform);
                            EnemyClones[2] = Instantiate(Enemies[1], new Vector3(5.72f, 0, 8.85f), Quaternion.identity, Backgrounds[1].transform);

                            GUIMap[1].GetComponent<Image>().color = new Color(1f, 0, 0, .5f);
                            StartCoroutine(FlashMap());
                            break;

                        case 2:
                            foreach (GameObject Background in Backgrounds)
                            {
                                Background.SetActive(false);
                            }
                            foreach (GameObject Enemy in EnemyClones)
                            {

                                Destroy(Enemy);


                            }
                            foreach (GameObject Item in ItemClones)
                            {
                                Destroy(Item);


                            }
                            foreach (GameObject Hazard in HazardClones)
                            {
                                Destroy(Hazard);


                            }
                            foreach (GameObject Map in GUIMap)
                            {
                                Map.GetComponent<Image>().color = new Color(.2f, .2f, .2f, .5f);
                            }
                            Backgrounds[2].SetActive(true);
                            


                            GUIMap[2].GetComponent<Image>().color = new Color(1f, 0, 0, .5f);
                            StartCoroutine(FlashMap());

                            break;

                        case 3:
                            foreach (GameObject Background in Backgrounds)
                            {
                                Background.SetActive(false);

                            }
                            foreach (GameObject Enemy in EnemyClones)
                            {

                                Destroy(Enemy);


                            }
                            foreach (GameObject Item in ItemClones)
                            {
                                Destroy(Item);


                            }
                        
                            foreach (GameObject Map in GUIMap)
                            {
                                Map.GetComponent<Image>().color = new Color(.2f, .2f, .2f, .5f);
                            }
                            Backgrounds[3].SetActive(true);
                            EnemyClones[0] = Instantiate(Enemies[0], new Vector3(-.23f, 0, 3.69f), Quaternion.identity, Backgrounds[3].transform);
                            EnemyClones[1] = Instantiate(Enemies[1], new Vector3(-8.84f, 0, .5f), Quaternion.identity, Backgrounds[3].transform);
                            EnemyClones[2] = Instantiate(Enemies[1], new Vector3(-9.2f, 0, 4.76f), Quaternion.identity, Backgrounds[3].transform);
                            
                            GUIMap[3].GetComponent<Image>().color = new Color(1f, 0, 0, .5f);
                            StartCoroutine(FlashMap());
                            break;

                        case 4:
                            foreach (GameObject Background in Backgrounds)
                            {
                                Background.SetActive(false);

                            }
                            foreach (GameObject Enemy in EnemyClones)
                            {

                                Destroy(Enemy);


                            }
                            foreach (GameObject Item in ItemClones)
                            {
                                Destroy(Item);


                            }
                            foreach (GameObject Hazard in HazardClones)
                            {
                                Destroy(Hazard);


                            }
                            foreach (GameObject Map in GUIMap)
                            {
                                Map.GetComponent<Image>().color = new Color(.2f, .2f, .2f, .5f);
                            }
                            Backgrounds[4].SetActive(true);
                            EnemyClones[0] = Instantiate(Enemies[0], new Vector3(4.5f, 0, 5), Quaternion.identity, Backgrounds[4].transform);
                            EnemyClones[1] = Instantiate(Enemies[1], new Vector3(-14.5f, 0, 5), Quaternion.identity, Backgrounds[4].transform);


                            GUIMap[4].GetComponent<Image>().color = new Color(1f, 0, 0, .5f);
                            StartCoroutine(FlashMap());
                            break;


                        default:
                            break;
                    }


                    break;
                








                case (1):
                    switch (_MapPositionX)
                    {

                        case 0:
                            //Not accessible, so ignore.
                            break;
                        case 1:
                            foreach (GameObject Background in Backgrounds)
                            {
                                Background.SetActive(false);

                            }
                            foreach (GameObject Enemy in EnemyClones)
                            {

                                Destroy(Enemy);


                            }
                            foreach (GameObject Item in ItemClones)
                            {
                                Destroy(Item);


                            }
                            foreach (GameObject Hazard in HazardClones)
                            {
                                Destroy(Hazard);


                            }
                            foreach (GameObject Map in GUIMap)
                            {
                                Map.GetComponent<Image>().color = new Color(.2f, .2f, .2f, .5f);
                            }
                            Backgrounds[6].SetActive(true);
                            EnemyClones[0] = Instantiate(Enemies[2], new Vector3(10.2f, 0, 11.5f), Quaternion.identity, Backgrounds[6].transform);
                            EnemyClones[1] = Instantiate(Enemies[0], new Vector3(2.28f, 0, 9.52f), Quaternion.identity, Backgrounds[6].transform);
                            EnemyClones[2] = Instantiate(Enemies[0], new Vector3(12.78f, 0, .4f), Quaternion.identity, Backgrounds[6].transform);

                            GUIMap[6].GetComponent<Image>().color = new Color(1f, 0, 0, .5f);
                            StartCoroutine(FlashMap());
                            break;
                        case 2:
                            foreach (GameObject Background in Backgrounds)
                            {
                                Background.SetActive(false);

                            }
                            foreach (GameObject Enemy in EnemyClones)
                            {

                                Destroy(Enemy);


                            }
                            foreach (GameObject Item in ItemClones)
                            {
                                Destroy(Item);


                            }
                            foreach (GameObject Hazard in HazardClones)
                            {
                                Destroy(Hazard);


                            }
                            foreach (GameObject Map in GUIMap)
                            {
                                Map.GetComponent<Image>().color = new Color(.2f, .2f, .2f, .5f);
                            }
                            Backgrounds[7].SetActive(true);
                            EnemyClones[0] = Instantiate(Enemies[2], new Vector3(10.2f, 0, 11.5f), Quaternion.identity, Backgrounds[7].transform);
                            EnemyClones[1] = Instantiate(Enemies[0], new Vector3(2.28f, 0, 9.52f), Quaternion.identity, Backgrounds[7].transform);
                            EnemyClones[2] = Instantiate(Enemies[0], new Vector3(12.78f, 0, .4f), Quaternion.identity, Backgrounds[7].transform);

                            GUIMap[7].GetComponent<Image>().color = new Color(1f, 0, 0, .5f);
                            StartCoroutine(FlashMap());
                            break;
                        case 3:
                            foreach (GameObject Background in Backgrounds)
                            {
                                Background.SetActive(false);

                            }
                            foreach (GameObject Enemy in EnemyClones)
                            {

                                Destroy(Enemy);


                            }
                            foreach (GameObject Item in ItemClones)
                            {
                                Destroy(Item);


                            }
                            foreach (GameObject Hazard in HazardClones)
                            {
                                Destroy(Hazard);


                            }
                            foreach (GameObject Map in GUIMap)
                            {
                                Map.GetComponent<Image>().color = new Color(.2f, .2f, .2f, .5f);
                            }
                            Backgrounds[8].SetActive(true);
                            EnemyClones[0] = Instantiate(Enemies[1], new Vector3(8.5f, 0, 8.6f), Quaternion.identity, Backgrounds[8].transform);
                            EnemyClones[1] = Instantiate(Enemies[1], new Vector3(-8.2f, 0, 8.6f), Quaternion.identity, Backgrounds[8].transform);

                            GUIMap[8].GetComponent<Image>().color = new Color(1f, 0, 0, .5f);
                            StartCoroutine(FlashMap());
                            break;
                        default:
                            break;
                    }
                    break;

                case (2):
                    switch (_MapPositionX)
                    {
                        case 0:
                            //unaccessible, so ignore.
                            break;
                        case 1:
                            foreach (GameObject Background in Backgrounds)
                            {
                                Background.SetActive(false);

                            }
                            foreach (GameObject Enemy in EnemyClones)
                            {

                                Destroy(Enemy);


                            }
                            foreach (GameObject Item in ItemClones)
                            {
                                Destroy(Item);


                            }
                            foreach (GameObject Hazard in HazardClones)
                            {
                                Destroy(Hazard);


                            }
                            foreach (GameObject Map in GUIMap)
                            {
                                Map.GetComponent<Image>().color = new Color(.2f, .2f, .2f, .5f);
                            }
                            Backgrounds[11].SetActive(true);
                            EnemyClones[0] = Instantiate(Enemies[0], new Vector3(-7.27f, 0, 9.51f), Quaternion.identity, Backgrounds[11].transform);
                            EnemyClones[1] = Instantiate(Enemies[1], new Vector3(5.65f, 0, 9.96f), Quaternion.identity, Backgrounds[11].transform);
                            EnemyClones[2] = Instantiate(Enemies[2], new Vector3(0f, 0, 3.38f), Quaternion.identity, Backgrounds[11].transform);

                            GUIMap[11].GetComponent<Image>().color = new Color(1f, 0, 0, .5f);
                            StartCoroutine(FlashMap());
                            break;
                        case 2:

                            foreach (GameObject Background in Backgrounds)
                            {
                                Background.SetActive(false);

                            }
                            foreach (GameObject Enemy in EnemyClones)
                            {

                                Destroy(Enemy);


                            }
                            foreach (GameObject Item in ItemClones)
                            {
                                Destroy(Item);


                            }
                            foreach (GameObject Hazard in HazardClones)
                            {
                                Destroy(Hazard);


                            }
                            foreach (GameObject Map in GUIMap)
                            {
                                Map.GetComponent<Image>().color = new Color(.2f, .2f, .2f, .5f);
                            }

                            Backgrounds[12].SetActive(true);
                            EnemyClones[0] = Instantiate(Enemies[1], new Vector3(-4.72f, 0, 9.51f), Quaternion.identity, Backgrounds[12].transform);
                            EnemyClones[1] = Instantiate(Enemies[1], new Vector3(4.72f, 0, 9.51f), Quaternion.identity, Backgrounds[12].transform);
                            EnemyClones[2] = Instantiate(Enemies[1], new Vector3(9.56f, 0, 6.05f), Quaternion.identity, Backgrounds[12].transform);
                            EnemyClones[3] = Instantiate(Enemies[1], new Vector3(-9.56f, 0, 6.05f), Quaternion.identity, Backgrounds[12].transform); 
                            EnemyClones[4] = Instantiate(Enemies[1], new Vector3(-6.62f, 0, 1.23f), Quaternion.identity, Backgrounds[12].transform); 
                            EnemyClones[5] = Instantiate(Enemies[1], new Vector3(6.62f, 0, 1.23f), Quaternion.identity, Backgrounds[12].transform);

                            GUIMap[12].GetComponent<Image>().color = new Color(1f, 0, 0, .5f);
                            StartCoroutine(FlashMap());

                            break;
                        case 3:
                            foreach (GameObject Background in Backgrounds)
                            {
                                Background.SetActive(false);

                            }
                            foreach (GameObject Enemy in EnemyClones)
                            {

                                Destroy(Enemy);


                            }
                            foreach (GameObject Item in ItemClones)
                            {
                                Destroy(Item);


                            }
                            foreach (GameObject Hazard in HazardClones)
                            {
                                Destroy(Hazard);


                            }
                            foreach (GameObject Map in GUIMap)
                            {
                                Map.GetComponent<Image>().color = new Color(.2f, .2f, .2f, .5f);
                            }

                            Backgrounds[13].SetActive(true);
                            EnemyClones[0] = Instantiate(Enemies[1], new Vector3(-4.72f, 0, 9.51f), Quaternion.identity, Backgrounds[13].transform);
                            EnemyClones[1] = Instantiate(Enemies[1], new Vector3(4.72f, 0, 9.51f), Quaternion.identity, Backgrounds[13].transform);
                            EnemyClones[2] = Instantiate(Enemies[1], new Vector3(6.62f, 0, 1.23f), Quaternion.identity, Backgrounds[13].transform);

                            GUIMap[13].GetComponent<Image>().color = new Color(1f, 0, 0, .5f);
                            StartCoroutine(FlashMap());
                            break;
                        default:
                            break;
                    }
                    break;







                case (3):
                    switch (_MapPositionX)
                    {
                        case 0:
                            //unaccessible, so ignore.
                            break;
                        case 1:
                            foreach (GameObject Background in Backgrounds)
                            {
                                Background.SetActive(false);

                            }
                            foreach (GameObject Enemy in EnemyClones)
                            {

                                Destroy(Enemy);


                            }
                            foreach (GameObject Item in ItemClones)
                            {
                                Destroy(Item);


                            }
                            foreach (GameObject Hazard in HazardClones)
                            {
                                Destroy(Hazard);


                            }
                            foreach (GameObject Map in GUIMap)
                            {
                                Map.GetComponent<Image>().color = new Color(.2f, .2f, .2f, .5f);
                            }

                            Backgrounds[16].SetActive(true);
                            EnemyClones[0] = Instantiate(Enemies[2], new Vector3(14.13f, 0, 10.2f), Quaternion.identity, Backgrounds[16].transform);
                            EnemyClones[1] = Instantiate(Enemies[2], new Vector3(14.13f, 0, 4f), Quaternion.identity, Backgrounds[16].transform);
                            EnemyClones[2] = Instantiate(Enemies[2], new Vector3(14.13f, 0, -1.01f), Quaternion.identity, Backgrounds[16].transform);

                            GUIMap[16].GetComponent<Image>().color = new Color(1f, 0, 0, .5f);
                            StartCoroutine(FlashMap());
                            break;
                        case 2:
                            //BossRoom
                            ModGlobalControl.Instance.BossRoom = true;
                            //Save player data prior to entering boss fight
                            ModGlobalControl.Instance.BossSave();
                            foreach (GameObject Background in Backgrounds)
                            {
                                Background.SetActive(false);

                            }
                            foreach (GameObject Enemy in EnemyClones)
                            {

                                Destroy(Enemy);


                            }
                            foreach (GameObject Item in ItemClones)
                            {
                                Destroy(Item);


                            }
                            foreach (GameObject Hazard in HazardClones)
                            {
                                Destroy(Hazard);


                            }
                            foreach (GameObject Map in GUIMap)
                            {
                                Map.GetComponent<Image>().color = new Color(.2f, .2f, .2f, .5f);
                            }

                            Backgrounds[17].SetActive(true);
                            EnemyClones[0] = Instantiate(Enemies[3], new Vector3(0f, 0, 15f), Quaternion.identity, Backgrounds[17].transform);
                            

                            GUIMap[17].GetComponent<Image>().color = new Color(1f, 0, 0, .5f);
                            StartCoroutine(FlashMap());
                            break;
                        case 3:
                            foreach (GameObject Background in Backgrounds)
                            {
                                Background.SetActive(false);

                            }
                            foreach (GameObject Enemy in EnemyClones)
                            {

                                Destroy(Enemy);


                            }
                            foreach (GameObject Item in ItemClones)
                            {
                                Destroy(Item);


                            }
                            foreach (GameObject Hazard in HazardClones)
                            {
                                Destroy(Hazard);


                            }
                            foreach (GameObject Map in GUIMap)
                            {
                                Map.GetComponent<Image>().color = new Color(.2f, .2f, .2f, .5f);
                            }

                            Backgrounds[18].SetActive(true);
                            EnemyClones[0] = Instantiate(Enemies[2], new Vector3(-14.13f, 0, 10.2f), Quaternion.identity, Backgrounds[18].transform);
                            EnemyClones[1] = Instantiate(Enemies[2], new Vector3(-14.13f, 0, 4f), Quaternion.identity, Backgrounds[18].transform);
                            EnemyClones[2] = Instantiate(Enemies[2], new Vector3(-14.13f, 0, -1.01f), Quaternion.identity, Backgrounds[18].transform);

                            GUIMap[18].GetComponent<Image>().color = new Color(1f, 0, 0, .5f);
                            StartCoroutine(FlashMap());
                            break;
                        default:
                            break;
                    }
                    break;

                case (4):
                    switch (_MapPositionX)
                    {
                        case 0:
                            //inaccessible, so ignore.
                            break;
                        case 1:

                            foreach (GameObject Background in Backgrounds)
                            {
                                Background.SetActive(false);

                            }
                            foreach (GameObject Enemy in EnemyClones)
                            {

                                Destroy(Enemy);


                            }
                            foreach (GameObject Item in ItemClones)
                            {
                                Destroy(Item);


                            }
                            foreach (GameObject Hazard in HazardClones)
                            {
                                Destroy(Hazard);


                            }
                            foreach (GameObject Map in GUIMap)
                            {
                                Map.GetComponent<Image>().color = new Color(.2f, .2f, .2f, .5f);
                            }

                            Backgrounds[21].SetActive(true);
                            EnemyClones[0] = Instantiate(Enemies[1], new Vector3(8.49f, 0, 7.2f), Quaternion.identity, Backgrounds[21].transform);
                            EnemyClones[1] = Instantiate(Enemies[2], new Vector3(-11.38f, 0, 10.3f), Quaternion.identity, Backgrounds[21].transform);
                            EnemyClones[2] = Instantiate(Enemies[1], new Vector3(-14.3f, 0, 2.89f), Quaternion.identity, Backgrounds[21].transform);

                            GUIMap[21].GetComponent<Image>().color = new Color(1f, 0, 0, .5f);
                            StartCoroutine(FlashMap());
                            break;
                        case 2:
                            foreach (GameObject Background in Backgrounds)
                            {
                                Background.SetActive(false);

                            }
                            foreach (GameObject Enemy in EnemyClones)
                            {

                                Destroy(Enemy);


                            }
                            foreach (GameObject Item in ItemClones)
                            {
                                Destroy(Item);


                            }
                            foreach (GameObject Hazard in HazardClones)
                            {
                                Destroy(Hazard);


                            }
                            foreach (GameObject Map in GUIMap)
                            {
                                Map.GetComponent<Image>().color = new Color(.2f, .2f, .2f, .5f);
                            }

                            Backgrounds[22].SetActive(true);
                            
                            GUIMap[22].GetComponent<Image>().color = new Color(1f, 0, 0, .5f);
                            StartCoroutine(FlashMap());
                            break;
                        case 3:
                            foreach (GameObject Background in Backgrounds)
                            {
                                Background.SetActive(false);

                            }
                            foreach (GameObject Enemy in EnemyClones)
                            {

                                Destroy(Enemy);


                            }
                            foreach (GameObject Item in ItemClones)
                            {
                                Destroy(Item);


                            }
                            foreach (GameObject Hazard in HazardClones)
                            {
                                Destroy(Hazard);


                            }
                            foreach (GameObject Map in GUIMap)
                            {
                                Map.GetComponent<Image>().color = new Color(.2f, .2f, .2f, .5f);
                            }

                            Backgrounds[23].SetActive(true);
                            EnemyClones[0] = Instantiate(Enemies[0], new Vector3(-7.86f, 0, 7.06f), Quaternion.identity, Backgrounds[23].transform);
                            EnemyClones[1] = Instantiate(Enemies[0], new Vector3(-9.65f, 0, 3.12f), Quaternion.identity, Backgrounds[23].transform);
                            EnemyClones[2] = Instantiate(Enemies[0], new Vector3(-5f, 0, 1f), Quaternion.identity, Backgrounds[23].transform);
                            EnemyClones[3] = Instantiate(Enemies[1], new Vector3(6f, 0, 1.24f), Quaternion.identity, Backgrounds[23].transform);
                            EnemyClones[4] = Instantiate(Enemies[2], new Vector3(12.05f, 0, 8.5f), Quaternion.identity, Backgrounds[23].transform);

                            GUIMap[23].GetComponent<Image>().color = new Color(1f, 0, 0, .5f);
                            StartCoroutine(FlashMap());
                            break;
                        default:
                            break;
                    }
                    break;


                default:
                    break;
            }

        }
    }

    //Show map when traveling across grid.
    IEnumerator FlashMap()
    {
        Map.SetActive(true);
        yield return new WaitForSeconds(1);
        if (!Input.GetKey(KeyCode.X))
        {
            Map.SetActive(false);
        }
    }

    //Convert player position to screen position and have the weapon menu follow the player.
    void WeaponChange()
    {
        if (player != null)
        { 
        Vector3 PlayerScreenPos = Camera.main.WorldToScreenPoint(player.transform.position);
        playerWeaponMenu.transform.position = PlayerScreenPos;
        if (Input.GetKeyDown(KeyCode.Space) && !MenuIsActive)
        {
            playerWeaponMenu.SetActive(true);
            MenuIsActive = true;
            Time.timeScale = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && MenuIsActive)
        {

            print("test");
            MenuIsActive = false;
            playerWeaponMenu.SetActive(false);
            Time.timeScale = 1;
        }

    }
    }

    public void PlasmaChange()
    {
        ModGlobalControl.Instance.Weapon = 0;
        playerWeaponMenu.SetActive(false);
        MenuIsActive = false;
        Time.timeScale = 1;
    }

    public void BeamChange()
    {
        ModGlobalControl.Instance.Weapon = 1;
        playerWeaponMenu.SetActive(false);
        MenuIsActive = false;
        Time.timeScale = 1;
    }

    public void ShieldChange()
    {
        ModGlobalControl.Instance.Weapon = 2;
        playerWeaponMenu.SetActive(false);
        MenuIsActive = false;
        Time.timeScale = 1;
    }

    public void MissileChange()
    {
        ModGlobalControl.Instance.Weapon = 3;
        playerWeaponMenu.SetActive(false);
        MenuIsActive = false;
        Time.timeScale = 1;
    }

    public void MeleeChange()
    {
        ModGlobalControl.Instance.Weapon = 4;
        playerWeaponMenu.SetActive(false);
        MenuIsActive = false;
        Time.timeScale = 1;
    }

    //Hooray you won!
    public void Celebration()
    {

        ModGlobalControl.Instance.LaserWeaponCharge = ModGlobalControl.Instance.LaserWeaponChargeMax;
        ModGlobalControl.Instance.MissileWeaponCharge = ModGlobalControl.Instance.MissileWeaponChargeMax;
        ModGlobalControl.Instance.ShieldWeaponCharge = ModGlobalControl.Instance.ShieldWeaponChargeMax;
        ModGlobalControl.Instance.MeleeWeaponCharge = ModGlobalControl.Instance.MeleeWeaponChargeMax;

        ModGlobalControl.Instance.SavePlayer();
        ModGlobalControl.Instance.BossRoom = false;
        SceneManager.LoadScene("LevelSelect");
        print("hm...");
    }
}