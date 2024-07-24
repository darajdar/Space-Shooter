using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour
{
    public GameObject shot;
    public Transform shotSpawn;
    public float FireRate;
    public int StartWait;
    public float delay;
    public bool Reloading;
    public int Ammo;
    public int MaxAmmo;
    public bool firing;
    public virtual void Start()
    {
    StartWait = 2;
    //InvokeRepeating ("Fire", delay, fireRate);
    StartCoroutine(AttackPattern());
        Ammo = MaxAmmo;
    }

    public virtual void Update()
    {
        //Quaternion target = Quaternion.LookRotation(new Vector3(0,0,0), transform.up);
        
        Ammunition();
        ResetRotation();
    }
    public virtual void Fire()
    {
        //while(!Reloading)
        Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        GetComponent<AudioSource>().Play();
    }

    public virtual void Ammunition()
    {
        if (Ammo > 1)
        {
            Reloading = false;
        }
    }

    public virtual void ResetRotation()
    {
        shot.transform.Rotate(0, 0, 0);
    }
    public virtual IEnumerator AttackPattern()
    {
        yield return new WaitForSeconds(StartWait);
        while (true)
        {
            while (!Reloading)
            {
                Fire();
                firing = true;
                yield return new WaitForSeconds(delay);
                Ammo -= 1;
                if (Ammo <= 0)
                {
                    Reloading = true;
                    firing = false;
                }
            }
            yield return new WaitForSeconds(FireRate);
            Ammo = MaxAmmo;
        }
    }

}
