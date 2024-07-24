using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HEWeaponController : WeaponController
{
    // Start is called before the first frame update
    public override void Start()
    {
        MaxAmmo = 2;
        Ammo = MaxAmmo;
        delay = .2f;
        FireRate = .7f;
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
        Instantiate(shot, new Vector3(shotSpawn.position.x +.2f, 0, shotSpawn.position.z + +.2f), shotSpawn.rotation);
        Instantiate(shot, new Vector3(shotSpawn.position.x-.2f, 0, shotSpawn.position.z-.2f), shotSpawn.rotation);
        GetComponent<AudioSource>().Play();
    }

   /* IEnumerator AttackPattern()
    {
        while (true)
        {
            while (!Reloading)
            {
                Fire();
                yield return new WaitForSeconds(delay);
                Ammo -= 1;
                if (Ammo <= 0)
                {
                    Reloading = true;
                }
            }
            yield return new WaitForSeconds(FireRate);
            Ammo = MaxAmmo;
        }
    }*/
}
