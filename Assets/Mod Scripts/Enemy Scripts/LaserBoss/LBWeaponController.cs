using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LBWeaponController : WeaponController
{
    
    public Transform shotSpawn1;
    public Transform shotSpawn2;
    public Transform shotSpawn3;
    public Transform shotSpawn4;
    public GameObject shotSpawnParent;
    public bool Firing;
    public int AttackVariant;


    public override void Start()
    {
        MaxAmmo = 60;
        Ammo = MaxAmmo;
        delay = .02f;
        FireRate = 1.5f;
        StartWait = 2;
        StartWait = 2;
        AttackVariant = 0;
        //InvokeRepeating ("Fire", delay, fireRate);
        StartCoroutine(AttackPattern());
        Ammo = MaxAmmo;
    }

    public override void Update()
    {
        //Quaternion target = Quaternion.LookRotation(new Vector3(0,0,0), transform.up);

        Ammunition();
        ResetRotation();
    }
    public override void Fire()
    {
        //while(!Reloading)
        if (Reloading)
        {
            
        }
        switch (AttackVariant)
        {
            case 0:
                shotSpawnParent.transform.Rotate(0, 0, 0);
                Instantiate(shot, shotSpawn1.position, shotSpawn1.rotation);
                GetComponent<AudioSource>().Play();
                Instantiate(shot, shotSpawn2.position, shotSpawn2.rotation);
                GetComponent<AudioSource>().Play();
                Instantiate(shot, shotSpawn3.position, shotSpawn3.rotation);
                GetComponent<AudioSource>().Play();
                Instantiate(shot, shotSpawn4.position, shotSpawn4.rotation);
                GetComponent<AudioSource>().Play();
                print("Attack Variant:  " + "1");
                break;
            case 1:
                shotSpawnParent.transform.Rotate(0, 0, 0);
                Instantiate(shot, shotSpawn1.position, shotSpawn1.rotation);
                GetComponent<AudioSource>().Play();
                Instantiate(shot, shotSpawn2.position, shotSpawn2.rotation);
                GetComponent<AudioSource>().Play();
                Instantiate(shot, shotSpawn3.position, shotSpawn3.rotation);
                GetComponent<AudioSource>().Play();
                Instantiate(shot, shotSpawn4.position, shotSpawn4.rotation);
                shotSpawnParent.transform.Rotate(0, 0, 120 * Time.deltaTime);
                GetComponent<AudioSource>().Play();
                print("Attack Variant:  " + "2");
                break;
            case 2:
                shotSpawnParent.transform.Rotate(0, 0, 0);
                Instantiate(shot, shotSpawn1.position, shotSpawn1.rotation);
                GetComponent<AudioSource>().Play();
                Instantiate(shot, shotSpawn2.position, shotSpawn2.rotation);
                GetComponent<AudioSource>().Play();
                Instantiate(shot, shotSpawn3.position, shotSpawn3.rotation);
                GetComponent<AudioSource>().Play();
                Instantiate(shot, shotSpawn4.position, shotSpawn4.rotation);
                shotSpawnParent.transform.Rotate(0, 0, -120 * Time.deltaTime);
                GetComponent<AudioSource>().Play();
                print("Attack Variant:  " + "3");
                break;
            case 3:
                shotSpawnParent.transform.Rotate(0, 0, 45);
                transform.Rotate(0, 0, 30 * Time.deltaTime);
                Instantiate(shot, shotSpawn1.position, shotSpawn1.rotation);
                GetComponent<AudioSource>().Play();
                Instantiate(shot, shotSpawn2.position, shotSpawn2.rotation);
                GetComponent<AudioSource>().Play();
                Instantiate(shot, shotSpawn3.position, shotSpawn3.rotation);
                GetComponent<AudioSource>().Play();
                Instantiate(shot, shotSpawn4.position, shotSpawn4.rotation);
                
                GetComponent<AudioSource>().Play();
                shotSpawnParent.transform.Rotate(0, 0, 0);
                Instantiate(shot, shotSpawn1.position, shotSpawn1.rotation);
                GetComponent<AudioSource>().Play();
                Instantiate(shot, shotSpawn2.position, shotSpawn2.rotation);
                GetComponent<AudioSource>().Play();
                Instantiate(shot, shotSpawn3.position, shotSpawn3.rotation);
                GetComponent<AudioSource>().Play();
                Instantiate(shot, shotSpawn4.position, shotSpawn4.rotation);
                shotSpawnParent.transform.Rotate(0, 0, -70 * Time.deltaTime);
                GetComponent<AudioSource>().Play();
                print("Attack Variant:  " + "4");
                break;
            default:
                break;
        }
        
    }

    public override void Ammunition()
    {
        if (Ammo > 1)
        {
            Reloading = false;
        }
    }

    public override void ResetRotation()
    {
        shot.transform.Rotate(0, 0, 0);
    }
    public override IEnumerator AttackPattern()
    {
        yield return new WaitForSeconds(StartWait);
        while (true)
        {
            while (!Reloading)
            {
                Fire();
                Firing = true;
                yield return new WaitForSeconds(delay);
                Ammo -= 1;
                if (Ammo <= 0)
                {
                    Reloading = true;
                    Firing = false;
                    AttackVariant = Random.Range(0, 4);

                }
            }
            yield return new WaitForSeconds(FireRate);
            //Activate Desperation Mode
            if(gameObject.GetComponent<LBStats>().EnemyHealth <= 50)
            {
                MaxAmmo = 100;
            }
            Ammo = MaxAmmo;
        }
    }

}
