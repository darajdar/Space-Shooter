using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//I need this line to get access to UI tags
using UnityEngine.UI;

//This code needs to be in the script to manipulate the scenes (from what I understand)
using UnityEngine.SceneManagement;
//Used for click commands, etc

public class MainMenu : MonoBehaviour
{
    

    //These variables will be used to slide the options box into frame
    //public GameObject optionsBox;
    private int _ScrollSpeed = 5;
    private Vector3 _OptionsPositionMax = new Vector3(-380, -110, 40);
    private Vector3 _OptionsPositionMin = new Vector3(-1500, -110, 40);
    private float  _OptionsX;
    private float  _OptionsY;
    private float  _OptionsZ;
    private Vector3 NewPosition;
    public bool _ToggleOptions = false;

    
    //pull Options menu
    public RectTransform OptionsTransform;
    public GameObject optionsBox;

    //Pull AutoFireToggle option
    public GameObject mainAutoFireToggle;
    public GameObject mainEnemyHealthToggle;
    public Toggle toggle;

    //Pull dropdown menu
    public GameObject DifficultyDropDown;
    public Dropdown dropDown;
   
    //Pull GlobalControl script
    public ModGlobalControl Load;
    
    void OnEnable()
    {
        optionsBox = GameObject.Find("OptionsBox");
        mainAutoFireToggle = GameObject.Find("MainAutoFireToggle");
        mainEnemyHealthToggle = GameObject.Find("MainEnemyHealthToggle");
        DifficultyDropDown = GameObject.Find("DifficultyDropDown");
    }
    // Start is called before the first frame update
    void Start()
    {
        //Find the game objects Im referencing
        

        //Load = gameController.GetComponent<GameController>();
        //Set variables to values that I will manipulate on other objects
        OptionsTransform = optionsBox.GetComponent<RectTransform>();
        toggle = mainAutoFireToggle.GetComponent<Toggle>();
        dropDown = DifficultyDropDown.GetComponent<Dropdown>();
        //Disable options box on startup
        optionsBox.SetActive(false);
        mainAutoFireToggle.SetActive(false);
        mainEnemyHealthToggle.SetActive(false);
        DifficultyDropDown.SetActive(false);

       /* optionsBox = GameObject.Find("OptionsBox");
        mainAutoFireToggle = GameObject.Find("MainAutoFireToggle");
        mainEnemyHealthToggle = GameObject.Find("MainEnemyHealthToggle");
        DifficultyDropDown = GameObject.Find("DifficultyDropDown");*/

        //_OptionsX = OptionsTransform.anchoredPosition.x;
        //_OptionsY = OptionsTransform.anchoredPosition.y;
        _OptionsX = OptionsTransform.position.x;
        _OptionsY = OptionsTransform.position.y;
        _OptionsZ = OptionsTransform.position.z;


        //Set up orientation of menu options
        //Set up the orientation of menu options
        //MouseLocation = Input.mousePosition;
        //---Make function later
        mainAutoFireToggle.GetComponent<Toggle>().isOn = ModGlobalControl.Instance.AutoFire;
        mainEnemyHealthToggle.GetComponent<Toggle>().isOn = ModGlobalControl.Instance.ToggleHealthBar;
    }



    //This code will create a function for when the play button is clicked will load the main game scene.
    public void NewGame()
    {
        SceneManager.LoadScene("LevelSelect");

        //--- This code can be used to change the current scene to the next one in the build menu
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //References the instance of ModGlobalControl and changes the data between scenes from the file
    public void LoadGame()
    {
            ModGlobalControl.Instance.LoadPlayer();
            SceneManager.LoadScene("LevelSelect");
    }

    //This code toggles the options menu
    public virtual void Options()
    {
        
        if (_ToggleOptions)
        {
            _ToggleOptions = false;
            optionsBox.SetActive(false);
            mainAutoFireToggle.SetActive(false);
            mainEnemyHealthToggle.SetActive(false);
            DifficultyDropDown.SetActive(false);
            //Debug.Log(_ToggleOptions);

        }
        else if (!_ToggleOptions)
        {
            _ToggleOptions = true;
            optionsBox.SetActive(true);
            mainAutoFireToggle.SetActive(true);
            mainEnemyHealthToggle.SetActive(true);
            DifficultyDropDown.SetActive(true);
            //Debug.Log(_ToggleOptions);
        }
        //while (_OptionsX != -350)


        //{ 

        //https://www.youtube.com/watch?time_continue=546&v=zc8ac_qUXQY&feature=emb_logo

        //OptionsTransform.anchoredPosition =  Vector3.Lerp(OptionsTransform.position, _OptionsPositionMax, .2f);
        //OptionsTransform.anchoredPosition = new Vector3(Mathf.Lerp(_OptionsX, -380, Time.deltaTime), _OptionsY, 40);
        //optionsBox.transform.Translate(Vector3.right * Time.deltaTime * _scrollSpeed);
        // GetComponent<Rigidbody>().velocity = new Vector3(newManeuver, 0.0f, currentSpeed);
        //}
    }
    
    //Change the difficulty
   public void Difficulty()
    {
        switch(dropDown.value)
        {
            case 3:
                ModGlobalControl.Instance.MaxLive = 1;
                ModGlobalControl.Instance.MaximumHealth = 200;
                ModGlobalControl.Instance.Lives = 1;
                ModGlobalControl.Instance.Health = 200;
                
                break;
            case 2:
                ModGlobalControl.Instance.MaxLive = 3;
                ModGlobalControl.Instance.MaximumHealth = 300;
                ModGlobalControl.Instance.Lives = 3;
                ModGlobalControl.Instance.Health = 300;
                
                break;
            case 1:
                ModGlobalControl.Instance.MaxLive = 5;
                ModGlobalControl.Instance.MaximumHealth = 500;
                ModGlobalControl.Instance.Lives = 5;
                ModGlobalControl.Instance.Health = 500;
                break;
            case 0:
                ModGlobalControl.Instance.MaxLive = 99;
                ModGlobalControl.Instance.MaximumHealth = 999;
                ModGlobalControl.Instance.Lives = 99;
                ModGlobalControl.Instance.Health = 999;
                ModGlobalControl.Instance.Practice = true;
                break;
            default:
                break;
        }
        print(ModGlobalControl.Instance.MaxLive);
        print(ModGlobalControl.Instance.MaximumHealth);
    }

    //Quit the game
    public void Quit()
    {
        Application.Quit();
    }

    //Toggle Autofire
    public virtual void AutoFire()
    {

        //mainAutoFireToggle.GetComponent<Toggle>().isOn = false;


        //mainAutoFireToggle.GetComponent<Toggle>().isOn = true;



        if (ModGlobalControl.Instance.AutoFire)
        {
            mainAutoFireToggle.GetComponent<Toggle>().isOn = false;
            ModGlobalControl.Instance.AutoFire = false;

        }
        else if(!ModGlobalControl.Instance.AutoFire)
        {
            mainAutoFireToggle.GetComponent<Toggle>().isOn = true;
            ModGlobalControl.Instance.AutoFire = true;

        }

    }

    //Toggle Enemy Health Bar
    public void EnemyHealthBar()
    {

        //mainEnemyHealthToggle.GetComponent<Toggle>().isOn = false;


        //mainEnemyHealthToggle.GetComponent<Toggle>().isOn = true;
;
        if (ModGlobalControl.Instance.ToggleHealthBar)
        {
            mainEnemyHealthToggle.GetComponent<Toggle>().isOn = false;
            ModGlobalControl.Instance.ToggleHealthBar = false;

        }
        else if (!ModGlobalControl.Instance.ToggleHealthBar)
        {
            mainEnemyHealthToggle.GetComponent<Toggle>().isOn = true;
            ModGlobalControl.Instance.ToggleHealthBar = true;

        }

        
    }

}
