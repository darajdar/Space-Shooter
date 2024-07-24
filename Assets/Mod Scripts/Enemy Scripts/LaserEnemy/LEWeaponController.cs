using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LEWeaponController : WeaponController
{
    
    // Start is called before the first frame update
    public override void Start()
    {
        firing = false;
        MaxAmmo = 40;
        Ammo = MaxAmmo;
        delay = .02f;
        FireRate = 3f;
        StartWait = 2;
        StartCoroutine(AttackPattern());

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Ammunition();
        base.ResetRotation();
    }

    //This child script will fire two lasers side by side
    public override void Fire()
    {
        //while(!Reloading)
        Instantiate(shot, new Vector3(shotSpawn.position.x, 0, shotSpawn.position.z), shotSpawn.rotation);
        GetComponent<AudioSource>().Play();
    }

    
}
