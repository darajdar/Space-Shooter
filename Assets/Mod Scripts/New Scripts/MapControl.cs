using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapControl : MonoBehaviour
{

    private GameObject _player;
    private GameObject _gameController;
    public GameObject[] Backgrounds;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _gameController = GameObject.FindWithTag("GameController");

        //Set backgrounds to all the gameobjects with the tag background. Then set them all to inactive.
        Backgrounds = GameObject.FindGameObjectsWithTag("Background");
        
        /*foreach(GameObject Background in Backgrounds)
        {
            Background.SetActive(false);
        }*/
        //Backgrounds[0].SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
       // ChangeMap();
    }

    public void ChangeMap()
    {
        switch (_gameController.GetComponent<GameController>()._MapPositionX | _gameController.GetComponent<GameController>()._MapPositionZ)
        {
            case (0 | 0):
                foreach (GameObject Background in Backgrounds)
                {
                    Background.SetActive(false);
                } 
                Backgrounds[0].SetActive(true);


                break;
            case (1 | 0):
                foreach (GameObject Background in Backgrounds)
                {
                    Background.SetActive(false);
                }
                
                Backgrounds[1].SetActive(true);
                break;

        }

    }
}
