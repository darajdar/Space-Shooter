using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : PlayerController
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Point();
    }

    public new void Point()
    {
        MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MousePosition.y = 0;
        
        //print("Mouse Position is currently:   " + MousePosition);
        //direction = (MousePosition - transform.position).normalized;
        direction = (MousePosition - transform.position).normalized;
        // print("Mouse position is currently:   " + MousePosition);
        // print("Ship position is currently:   " + transform.position);

        print("Ship position is currently:   " + direction);

        lookRotation = Quaternion.LookRotation(direction, transform.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime);
        //transform.rotation = Quaternion.Euler(0, 0, 0);

    }
}
