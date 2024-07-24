using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidStats : MonoBehaviour
{
    public float AsteroidHealth;
    public GameObject AsteroidExplosion;
    // This script is only to take enemy health and damage values bc Im lazy
    void Start()
    {
        AsteroidHealth = Random.Range(5, 25);
}

    // Update is called once per frame
    void Update()
    {
        CheckForDeath();
    }



    public virtual void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            if (!ModGlobalControl.Instance.Practice)
            { 
                ModGlobalControl.Instance.Health -= 50f;
            }

        }
        //Regular bullet damage
        if (other.tag == "Bullet")
        {
            AsteroidHealth-= 2;


        }

        //Regular Missile Damage
        if (other.tag == "MissileExplosion")
        {
            AsteroidHealth -= 25;
            //Destroy(other.gameObject);


        }


        if (other.tag == "EnemyBullet")
        {
            AsteroidHealth-= 2;
            if (AsteroidExplosion != null)
            {
                Instantiate(AsteroidExplosion, transform.position, transform.rotation);
            }
            Destroy(other.gameObject);
        }

        if (other.tag == "Melee")
        {
            AsteroidHealth -= 3;
            if (AsteroidExplosion != null)
            {
                Instantiate(AsteroidExplosion, transform.position, transform.rotation);
            }

        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Laser")
        {
            AsteroidHealth -= .2f;



        }
    }

    void CheckForDeath()
    {
        if (AsteroidHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
