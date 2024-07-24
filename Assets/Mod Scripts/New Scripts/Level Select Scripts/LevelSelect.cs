using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Level1")
        {
            SceneManager.LoadScene("Level1");
        }

        /*if (other.tag == "Level2")
        {
            SceneManager.LoadScene("Level2");
        }

        if (other.tag == "Level3")
        {
            SceneManager.LoadScene("Level3");
        }

        if (other.tag == "Level4")
        {
            SceneManager.LoadScene("Level4");
        }*/
    }
}
