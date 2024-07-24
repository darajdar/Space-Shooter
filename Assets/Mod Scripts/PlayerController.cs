using UnityEngine;
using System.Collections;

//This lets the new class which is not recognized by Unity become serialized, which will let me edit the variables' values in Unity
[System.Serializable]
public class Boundary 
{
    public float xMin = -20, xMax = 20, zMin = -4, zMax = 14;
}

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float tilt;
    public Boundary boundary;

    //Create all the projectiles the player can use, as well as the location is spawns
    public GameObject shot;
    public GameObject Laser;
    public GameObject Shield;
    public GameObject Missile;
    public GameObject Melee;
    public GameObject MeleeObject;
    public bool MeleeFired;


    public Transform shotSpawn;
    public bool Reloading;

    //Cache GameController
    public GameObject GameController;
    public GameController gameController;

    public GameObject enginesPlayer;
    //Variables for turning towards mouse pointer

    public Vector3 MousePosition;
    public Quaternion lookRotation;
    public Vector3 direction;

    public bool ShieldOn;
    public bool MissileFired;
    //--- I don't rememebr what these variables are for
    //private float _MovingX;
    //private float _MovingZ;

    //public float fireRate;

    //private float nextFire;



    void Start()
    {
        //--- I dont remember what this does so I guess I'll get rid of it for now
        //_MovingX = GetComponent<Rigidbody>().velocity.x;
        //_MovingZ = GetComponent<Rigidbody>().velocity.z;

        gameController = GameController.GetComponent<GameController>();

        if (gameController != null)
        { 
        Shield.SetActive(false);
        }

        ModGlobalControl.Instance.Weapon = 0;
        //Set weapons variable to starting weapon

    }
    void Update()
    {
        if (gameController != null)
        {

            if (!ModGlobalControl.Instance.Paused && !gameController.MenuIsActive)
            {

                FireWeapon();
            }
        } else if (gameController == null)
        {

            if (!ModGlobalControl.Instance.Paused)
            {

                FireWeapon();
            }
        }
    }

    void FixedUpdate()
    {
        Move();
        Point();
        ChangeWeapons();

        HealthUpdater();
        
    }

    void HealthUpdater()
    {
  

        //The explosions refuse to delete for whatever reason

       
    }
    void FireWeapon()
    {

        if (gameController != null)
        {
            switch (ModGlobalControl.Instance.Weapon)
            {
                case 0:
                    Destroy(MeleeObject);
                    Shield.SetActive(false);
                    if (ModGlobalControl.Instance.AutoFire)
                    {

                        if (Input.GetKey(KeyCode.Mouse0) && !Reloading)
                        {

                            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
                            GetComponent<AudioSource>().Play();
                            StartCoroutine(AutoFireTimer());
                        }
                    }
                    else
                    //What I edited out will make the bullet fire every time the left mouse button is pressed.
                    if (Input.GetKeyDown(KeyCode.Mouse0) /*&& Time.time > nextFire*/)
                    {
                        //nextFire = Time.time + fireRate;

                        Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
                        GetComponent<AudioSource>().Play();


                    }
                    break;

                case 1:
                    Destroy(MeleeObject);
                    Shield.SetActive(false);
                    if (Input.GetKey(KeyCode.Mouse0) && ModGlobalControl.Instance.LaserWeaponCharge > 0 /*&& Time.time > nextFire*/)
                    {
                        //nextFire = Time.time + fireRate;

                        Instantiate(Laser, shotSpawn.position, shotSpawn.rotation);
                        if (!ModGlobalControl.Instance.Practice)
                        {
                            ModGlobalControl.Instance.LaserWeaponCharge -= 1f;
                        }
                        GetComponent<AudioSource>().Play();


                    }
                    break;

                //Shield Weapon
                case 2:
                    Destroy(MeleeObject);
                    if (Input.GetKey(KeyCode.Mouse0) && ModGlobalControl.Instance.Weapon == 2 && ModGlobalControl.Instance.ShieldWeaponCharge > 0)
                    {

                        Shield.SetActive(true);
                        //GetComponent<AudioSource>().Play();

                    }
                    else
                    {
                        Shield.SetActive(false);
                    }


                    break;

                case 3:
                    Destroy(MeleeObject);
                    Shield.SetActive(false);
                    if (Input.GetKeyDown(KeyCode.Mouse0) && ModGlobalControl.Instance.MissileWeaponCharge > 0 && !MissileFired)
                    {
                        //nextFire = Time.time + fireRate;

                        Instantiate(Missile, shotSpawn.position, shotSpawn.rotation);
                        GetComponent<AudioSource>().Play();
                        if (!ModGlobalControl.Instance.Practice)
                        {
                            ModGlobalControl.Instance.MissileWeaponCharge -= 1f;
                        }
                        MissileFired = true;


                    }
                    break;

                case 4:
                    Shield.SetActive(false);
                    if (Input.GetKey(KeyCode.Mouse0) && ModGlobalControl.Instance.MeleeWeaponCharge > 0 && !MeleeFired)
                    {
                        //nextFire = Time.time + fireRate;

                        MeleeObject = Instantiate(Melee, transform.position, transform.rotation);
                        GetComponent<AudioSource>().Play();
                        MeleeFired = true;

                        MeleeObject.transform.position = transform.position;
                        if (!ModGlobalControl.Instance.Practice)
                        {
                            ModGlobalControl.Instance.MeleeWeaponCharge -= 1f;
                        }



                    } 
                    if (MeleeFired && Input.GetKey(KeyCode.Mouse0) && ModGlobalControl.Instance.MeleeWeaponCharge > 0 && ModGlobalControl.Instance.Weapon == 4)
                    {
                        //nextFire = Time.time + fireRate;

                        MeleeObject.transform.Rotate(0, 200 * Time.deltaTime, 0);
                        MeleeObject.transform.position = transform.position;
                        if (!ModGlobalControl.Instance.Practice)
                        {
                            ModGlobalControl.Instance.MeleeWeaponCharge -= 1f;
                        }



                    }
                    else 
                    {
                        
                        Destroy(MeleeObject);
                        MeleeFired = false;
                    }

                    break;
                default:
                    break;
            }
        } else if (gameController == null)
        {
            
 
                    if (ModGlobalControl.Instance.AutoFire)
                    {

                        if (Input.GetKey(KeyCode.Mouse0) && !Reloading)
                        {

                            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
                            GetComponent<AudioSource>().Play();
                            StartCoroutine(AutoFireTimer());
                        }
                    }
                    else
                    //What I edited out will make the bullet fire every time the left mouse button is pressed.
                    if (Input.GetKeyDown(KeyCode.Mouse0) /*&& Time.time > nextFire*/)
                    {
                        //nextFire = Time.time + fireRate;

                        Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
                        GetComponent<AudioSource>().Play();


                    }

            }
    }

    void ChangeWeapons()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ModGlobalControl.Instance.Weapon = 0;

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ModGlobalControl.Instance.Weapon = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ModGlobalControl.Instance.Weapon = 2;
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ModGlobalControl.Instance.Weapon = 3;
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ModGlobalControl.Instance.Weapon = 4;
        }
    }

    void Move()
    {
        float moveHorizontal /*= Input.GetAxis("Horizontal")*/;
        float moveVertical /*= Input.GetAxis("Vertical")*/;

        //Make movement more rigid and instant for Horizontal and Vertical movement.
        if (Input.GetKey(KeyCode.A))
        {
            moveHorizontal = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveHorizontal = 1;
        }
        else
        {
            moveHorizontal = 0;
        }

        if (Input.GetKey(KeyCode.W))
        {
            moveVertical = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveVertical = -1;
        }
        else
        {
            moveVertical = 0;
        }

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        GetComponent<Rigidbody>().velocity = movement * speed;

        GetComponent<Rigidbody>().position = new Vector3
            (
            Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
        );

        //GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);

        //This messes with laser rotation so make it not work when laser is equipped
        if (ModGlobalControl.Instance.Weapon != 1)
        {
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
        }




    }

    //Point towards the cursor
    public void Point()
    {
        Vector3 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        MousePosition.y = 0;

        direction = (MousePosition - transform.position).normalized;


        Quaternion target = Quaternion.LookRotation(MousePosition, transform.up);



        //Nerf the laser by making the ship turn slower when it is in use.
        if (ModGlobalControl.Instance.Weapon == 1 && Input.GetKey(KeyCode.Mouse0))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime);
            //print("test");
        }
        else
        {
            transform.rotation = target;
        }
        //enginesPlayer.transform.rotation = target;
    }

    //Make autofire not unbalanced
    IEnumerator AutoFireTimer()
    {
        Reloading = true;
        yield return new WaitForSeconds(.1f);
        Reloading = false;
    }



    //Consumables triggers. Restore player stats based on the item touched.
    private void OnTriggerEnter(Collider other)
    {
        

        if (other.tag == "HealthPickUp")
        {
            ModGlobalControl.Instance.Health += (ModGlobalControl.Instance.MaximumHealth * .2f);
            Destroy(other.gameObject);
        }

        if (other.tag == "BigHealthPickUp")
        {
            ModGlobalControl.Instance.Health += (ModGlobalControl.Instance.MaximumHealth * .5f);
            Destroy(other.gameObject);
        }

        if (other.tag == "EnergyPickUp")
        {
            switch (ModGlobalControl.Instance.Weapon)
            {
                case 0:

                    break;
                case 1:
                    ModGlobalControl.Instance.LaserWeaponCharge += (ModGlobalControl.Instance.LaserWeaponChargeMax * .25f);
                    Destroy(other.gameObject);
                    break;
                case 2:
                    ModGlobalControl.Instance.ShieldWeaponCharge += (ModGlobalControl.Instance.ShieldWeaponChargeMax * .25f);
                    Destroy(other.gameObject);
                    break;
                case 3:
                    ModGlobalControl.Instance.MissileWeaponCharge += (ModGlobalControl.Instance.MissileWeaponChargeMax * .25f);
                    Destroy(other.gameObject);
                    break;
                case 4:
                    ModGlobalControl.Instance.MeleeWeaponCharge += (ModGlobalControl.Instance.MeleeWeaponChargeMax * .25f);
                    Destroy(other.gameObject);
                    break;
                default:
                    break;
            }
        }

        if (other.tag == "BigEnergyPickUp")
        {
            switch (ModGlobalControl.Instance.Weapon)
            {
                case 0:
                    
                    break;
                case 1:
                    ModGlobalControl.Instance.LaserWeaponCharge += (ModGlobalControl.Instance.LaserWeaponChargeMax * .75f);
                    Destroy(other.gameObject);
                    break;
                case 2:
                    ModGlobalControl.Instance.ShieldWeaponCharge += (ModGlobalControl.Instance.ShieldWeaponChargeMax * .75f);
                    Destroy(other.gameObject);
                    break;
                case 3:
                    ModGlobalControl.Instance.MissileWeaponCharge += (ModGlobalControl.Instance.MissileWeaponChargeMax * .75f);
                    Destroy(other.gameObject);
                    break;
                case 4:
                    ModGlobalControl.Instance.MeleeWeaponCharge += (ModGlobalControl.Instance.MeleeWeaponChargeMax * .75f);
                    Destroy(other.gameObject);
                    break;
                default:
                    break;
            }
            
        }

        if (other.tag == "ExtraLife")
        {
            ModGlobalControl.Instance.Lives += 1;
            Destroy(other.gameObject);
        }
    }

    
}
  

