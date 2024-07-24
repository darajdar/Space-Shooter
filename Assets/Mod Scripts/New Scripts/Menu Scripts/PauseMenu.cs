using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    //Find the game objects Im referencing
    public bool _PauseToggleOptions = false;


    //pull Options menu
    public RectTransform OptionsTransform;
    public GameObject pauseOptionsBox;

    //Pull gamecontroller for pause function
    public GameObject gameController;

    //pull pause canvas
    //Pull AutoFireToggle option
    public GameObject autoFireToggle;
    public GameObject enemyHealthToggle;
    public Toggle toggle;





    // Start is called before the first frame update
    void Start()
    {
        pauseOptionsBox = GameObject.Find("PauseOptionsBox");
        //autoFireToggle = GameObject.Find("AutoFireToggle");
        //enemyHealthToggle = GameObject.Find("EnemyHealthToggle");
        toggle = GetComponent<Toggle>();
        pauseOptionsBox.SetActive(false);

  
        //---Make function later
        
        //autoFireToggle.GetComponent<Toggle>().isOn = false;

        //Cache GameController
        gameController = GameObject.FindWithTag("GameController");


        autoFireToggle.GetComponent<Toggle>().isOn = ModGlobalControl.Instance.AutoFire;
        enemyHealthToggle.GetComponent<Toggle>().isOn = ModGlobalControl.Instance.ToggleHealthBar;

    }

    // Update is called once per frame
    void Update()
    {
        /*if (ModGlobalControl.Instance.AutoFire)
        {
            autoFireToggle.GetComponent<Toggle>().isOn = true;
            //Debug.Log(_PauseToggleOptions);

        }
        else if (!ModGlobalControl.Instance.AutoFire)
        {
            autoFireToggle.GetComponent<Toggle>().isOn = false;
            //Debug.Log(_PauseToggleOptions);
        }*/

        //Bring up pause menu when game is paused
    }

    public void Options()
    {

        if (_PauseToggleOptions)
        {
            _PauseToggleOptions = false;
            pauseOptionsBox.SetActive(false);

            
            //Debug.Log(_PauseToggleOptions);

        }
        else if (!_PauseToggleOptions)
        {
            _PauseToggleOptions = true;
            pauseOptionsBox.SetActive(true);
            //Debug.Log(_PauseToggleOptions);
        }

    
    //while (_OptionsX != -350)

    //{ 

    //https://www.youtube.com/watch?time_continue=546&v=zc8ac_qUXQY&feature=emb_logo

    //OptionsTransform.anchoredPosition =  Vector3.Lerp(OptionsTransform.position, _OptionsPositionMax, .2f);
    //OptionsTransform.anchoredPosition = new Vector3(Mathf.Lerp(_OptionsX, -380, Time.deltaTime), _OptionsY, 40);
    //pauseOptionsBox.transform.Translate(Vector3.right * Time.deltaTime * _scrollSpeed);
    // GetComponent<Rigidbody>().velocity = new Vector3(newManeuver, 0.0f, currentSpeed);
    //}
    }

    public void Resume()
    {
        ModGlobalControl.Instance.Resumed = true;

    }

    public void LevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
        ModGlobalControl.Instance.Resumed = true;
    }

    public void MainMenu()
    {

        SceneManager.LoadScene("MainMenu");
        ModGlobalControl.Instance.Resumed = true;
    }

    public void AutoFire()
    {
        if (ModGlobalControl.Instance.AutoFire)
        {
            autoFireToggle.GetComponent<Toggle>().isOn = false;
            ModGlobalControl.Instance.AutoFire = false;

        }
        else if (!ModGlobalControl.Instance.AutoFire)
        {
            autoFireToggle.GetComponent<Toggle>().isOn = true;
            ModGlobalControl.Instance.AutoFire = true;

        }
    }

    private void EnemyHealthBar()
    {

        //enemyHealthToggle.GetComponent<Toggle>().isOn = false;


        //enemyHealthToggle.GetComponent<Toggle>().isOn = true;
        ;
        if (ModGlobalControl.Instance.ToggleHealthBar)
        {
            enemyHealthToggle.GetComponent<Toggle>().isOn = false;
            ModGlobalControl.Instance.ToggleHealthBar = false;

        }
        else if (!ModGlobalControl.Instance.ToggleHealthBar)
        {
            enemyHealthToggle.GetComponent<Toggle>().isOn = true;
            ModGlobalControl.Instance.ToggleHealthBar = true;

        }


    }


}
