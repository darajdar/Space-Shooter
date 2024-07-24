using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByExit : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        //This checks if the gameobject is enabled(along with the script being active, which doesnt matter in this case) and deletes it if it isn't.
        if(!gameObject.activeInHierarchy)
        {
            Destroy(gameObject);
        }
    }
}
