using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class EnemyStats : MonoBehaviour
{
	public Boundary boundary;
	public float tilt;
	public float dodge;

    //public float smoothing;

    public float JumpDistance; //How far the enemy moves while its allowed to.
	public Vector2 startWait; //Time before enemy starts AI. Can probably reomve.
	public Vector2 maneuverTime; //the amount of time the enemy can move for before stopping.
	public Vector2 maneuverWait;// How long the enemy waits between manuevering.
    public Vector3 Enemy;
    //Pull player
    public GameObject player;

    //Get variables to face player
    public Quaternion lookRotation;
    public Vector3 direction;
    
    //This is the enemys velocity
    public Vector3 targetManeuver;

    //The JumpDistance the enemy flees at, and the enemys flee speed, and Flee AI, how often the AI does a move, how fast they flee;
    public float EnemyHealth; //Enemys health
    public float FleeDistance; //How close you can get to an enemy before it backs into a safer distance
   // public float FleeSpeed; //How fast the enemy can flee.
    public bool FleeState;
    public float AttackSpeed; //How often the enemy chooses to do something.
    public float EvadeSpeed; //This is apparently just how fast the ship flees.

    //Holds the item prefabs that can drop after defeating enemy
    public GameObject[] Items;
    public GameObject[] ItemClones;


    //Cache healthbar objects
    public GameObject enemyHealthCanvas;
    public Transform enemyHealthBarPanel;
    public Slider enemyHealthBarSlider;



    //Pull gamecontroller
    public GameObject _gameController;

    //Variable for EnemyAI range.
    public int AIRange;

    //Save variable for enemy bullet prefab.
    public GameObject enemyBullet;

    //Pull vfx for reflected bullet explosion
    public GameObject explosion;

    public float Offset;
    public virtual void Start ()
	{
        

        //EnemyHealth = 4;
        //AttackSpeed = 1;
        //EvadeSpeed = 1;
        player = GameObject.FindWithTag("Player");
        _gameController = GameObject.FindWithTag("GameController");


        
        //Personal enemy's maneuvertime and maneuverwait

        //targetManeuver = GetComponent<Rigidbody>().velocity;

        //Set the max and min values for the healthbar
        enemyHealthBarSlider.minValue = 0;
        enemyHealthBarSlider.maxValue = EnemyHealth;
        startWait = new Vector2(1.2f, 1.2f);

        //Run enemy AI. This will choose between different coroutines to make the enemy move.
        StartCoroutine(EnemyAI());

        
        
    }

    

    public virtual void FixedUpdate()
    {
        
        if (player != null)
        {
            
            Movement();
            FacePlayer();
            HealthBar();
            CheckForDeath();
        }
    }
    

    public virtual void Movement()
    {
        if (Vector3.Distance(player.GetComponent<Transform>().position, transform.position) <= FleeDistance)
        {
            FleeState = true;
        }
        //Make the range FleeState deactivates larger than when it enters
        else if (Vector3.Distance(player.GetComponent<Transform>().position, transform.position) >= FleeDistance * 1.2f)
        {
            FleeState = false;
        }

        if (FleeState == false)
        {
            MoveEnemy();
        }
        else if (FleeState == true)
        {
            FleeEnemy();
        }
    }
    public virtual IEnumerator EnemyAI()
    {
        yield return new WaitForSeconds(startWait.x);
        //Assigns the AIRange between the multiple options for the movement choices.
        while (player != null)
            
        {
            if (FleeState == true)
            {
                FleeState = true;
                StartCoroutine(Flee());
            } else if (FleeState == false)
            {
                FleeState = false;
                AIRange = Random.Range(1, 3);
                //print(AIRange);
                switch (AIRange)
                {
                    case 2:
                        StartCoroutine(Chase());
                        break;
                    case 1:
                        StartCoroutine(Dodge());
                        break;
                    default:
                        break;

                }
                
            }

            yield return new WaitForSeconds(Random.Range(AttackSpeed-1, AttackSpeed+1));
        }
    }


	public virtual IEnumerator Chase ()
	{

        //Chase the player
        //yield return new WaitForSeconds(Random.Range(startWait.x, startWait.y));
        //while (FleeState == true)
        //{
        //Make the target of the enemy move towards the player, but not in a way that is dangerous.
        if(player != null) { 
            targetManeuver = new Vector3 ((player.GetComponent<Transform>().position.x - transform.position.x)/3, 0, (player.GetComponent<Transform>().position.z - transform.position.z)*2);
            yield return new WaitForSeconds(Random.Range(maneuverTime.x, maneuverTime.y));
            targetManeuver = new Vector3(0, 0, 0);
            //yield return new WaitForSeconds(Random.Range(maneuverWait.x, maneuverWait.y));
        }
        
    }

    public virtual IEnumerator Dodge()
    {
        //yield return new WaitForSeconds (Random.Range (startWait.x, startWait.y));
        //while (player != null)
        if (player != null)
        {
            targetManeuver = new Vector3 ((-player.GetComponent<Transform>().position.x - transform.position.x), 0, (-player.GetComponent<Transform>().position.z - transform.position.z) * 2);
			yield return new WaitForSeconds (Random.Range (maneuverTime.x, maneuverTime.y));
			targetManeuver = new Vector3(0, 0, 0);
			//yield return new WaitForSeconds (Random.Range (maneuverWait.x, maneuverWait.y));
		}

    }

    public virtual IEnumerator Flee()
    {
        if (player != null)
        {
            //yield return new WaitForSeconds(Random.Range(startWait.x, startWait.y));
            //while (FleeState == true)
            //{
            //--- If I put a +transform.position, the enemies rush the player. This could be useful. Work on this?
            //Also, I dont know if I need to subtract the transform position from the player position, so I'll leave it out for now.
            targetManeuver =  (-player.GetComponent<Transform>().position /*- transform.position*/);
            //targetManeuver = (-transform.forward);

            //I don't know why it has to be called like this but whatever.
            yield return new WaitUntil(() => FleeState == false);
            targetManeuver = new Vector3(0, 0, 0);
        //yield return new WaitForSeconds(Random.Range(maneuverWait.x, maneuverWait.y));
       }

    }


    public virtual void MoveEnemy()
    {
        if (player != null)
        {
            /*Vector3 newManeuver = Vector3.MoveTowards(GetComponent<Rigidbody>().velocity, targetManeuver, JumpDistance * Time.deltaTime);
            GetComponent<Rigidbody>().velocity = newManeuver;

            //This code locks the enemy inside the boundaries, and allows it to move.
            GetComponent<Transform>().position = new Vector3
             (
                 Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
                 0.0f,
                 Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
             );

            transform.rotation = Quaternion.Euler(0, 0, GetComponent<Rigidbody>().velocity.x * -tilt);

            //print("The x is  " + GetComponent<Transform>().position.x);
            //print("The z is  " + GetComponent<Transform>().position.z);
            // print(Vector3.Distance(player.GetComponent<Transform>().position, GetComponent<Transform>().position));
            */
            Vector3 newManeuver = Vector3.MoveTowards(GetComponent<Rigidbody>().velocity, targetManeuver, JumpDistance * Time.deltaTime);
            //GetComponent<Rigidbody>().velocity = newManeuver;
            GetComponent<Rigidbody>().velocity = newManeuver;
            //This code locks the enemy inside the boundaries, and allows it to move.
            GetComponent<Transform>().position = new Vector3
             (
                 Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
                 0.0f,
                 Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
             );

            transform.rotation = Quaternion.Euler(0, 0, GetComponent<Rigidbody>().velocity.x * -tilt);

            //print("The x is  " + GetComponent<Transform>().position.x);
            //print("The z is  " + GetComponent<Transform>().position.z);
            // print(Vector3.Distance(player.GetComponent<Transform>().position, GetComponent<Transform>().position));
        }

    }


    public virtual void FleeEnemy()
    {
        if (player != null)
        {
            Vector3 newManeuver = Vector3.MoveTowards(GetComponent<Rigidbody>().velocity, targetManeuver, JumpDistance * EvadeSpeed);
            GetComponent<Rigidbody>().velocity = -newManeuver;

            //This "unfreezes" the enemy and lets them accelerate, but stay inside the screen.
            GetComponent<Transform>().position = new Vector3
             (
                 Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
                 0.0f,
                 Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
             );

            transform.rotation = Quaternion.Euler(0, 0, GetComponent<Rigidbody>().velocity.x * -tilt);

            //print("The x is  " + GetComponent<Transform>().position.x);
            //print("The z is  " + GetComponent<Transform>().position.z);
            //print(Vector3.Distance(player.GetComponent<Transform>().position, GetComponent<Transform>().position));
        }
    }

    public virtual void FacePlayer()
    {
        if (player != null)
        {


            //Make the enemy face the player. The vector enemy is refering to the player.
            Enemy = player.GetComponent<Transform>().position - transform.position;
            Enemy.y = 0;


            direction = (Enemy - transform.position).normalized;

            Quaternion target = Quaternion.LookRotation(-Enemy, transform.up);

            // transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime);

            transform.rotation = target;
        }
    }
    //Manage damage. Destroy ship on destruction
    public virtual void OnTriggerEnter(Collider other)
    {
        enemyBullet = other.gameObject;
        //Regular bullet damage
        if (other.tag == "Bullet")
        {
            EnemyHealth--;
            Destroy(other.gameObject);
            

        }

        //Regular Missile Damage
        if (other.tag == "MissileExplosion")
        {
            EnemyHealth-= 4;
            //Destroy(other.gameObject);


        }

        if (other.tag == "EnemyBullet" && enemyBullet.GetComponent<EnemyMover>().Reflected)
        {
            EnemyHealth--;
            //Destroy(other.gameObject);
            if (explosion != null)
            {
                Instantiate(explosion, transform.position, transform.rotation);
            }

        }

        if (other.tag == "Melee")
        {
            EnemyHealth -= 2;
            if (explosion != null)
            {
                Instantiate(explosion, transform.position, transform.rotation);
            }

        }
    }

    //Deal laser damage.
    public virtual void OnTriggerStay(Collider other)
    {
        if (other.tag == "Laser")
        {
            EnemyHealth -= .05f;



        }
    }

    //Set enemy healthbar, and make the healthbar follow the enemy, and face the camera always
    public virtual void HealthBar()
    {
        if (ModGlobalControl.Instance.ToggleHealthBar)
        {
            if (enemyHealthCanvas != null) { 
            enemyHealthCanvas.SetActive(true);
            enemyHealthBarSlider.value = EnemyHealth;
        }
        Vector3 CurrentPos = transform.position;
        enemyHealthBarPanel.position = new Vector3(CurrentPos.x, CurrentPos.y + Offset, CurrentPos.z);

        enemyHealthBarPanel.LookAt(Camera.main.transform);
        } else if (!ModGlobalControl.Instance.ToggleHealthBar)
        {
            enemyHealthCanvas.SetActive(false);
        }
    }

    public virtual void CheckForDeath()
    {
        if (EnemyHealth <= 0)
        {
            Destroy(this.gameObject);

            //Introduce a drop chance
            int DropChance = Random.Range(0, 5) + Random.Range(0, 5);

            switch (DropChance)
            {
                case 12:
                    Instantiate(/*gameController.GetComponent<GameController>().*/Items[0], transform.position, Quaternion.identity, transform.parent);
                    break;
                case 11:
                    Instantiate(/*gameController.GetComponent<GameController>().*/Items[2], transform.position, Quaternion.identity, transform.parent);
                    break;
                case 10:
                    Instantiate(/*gameController.GetComponent<GameController>().*/Items[3], transform.position, Quaternion.identity, transform.parent);
                    break;
                case 8:
                    Instantiate(/*gameController.GetComponent<GameController>().*/Items[1], transform.position, Quaternion.identity, transform.parent);
                    break;
                case 7:
                    Instantiate(/*gameController.GetComponent<GameController>().*/Items[1], transform.position, Quaternion.identity, transform.parent);
                    break;
                case 6:
                    Instantiate(/*gameController.GetComponent<GameController>().*/Items[1], transform.position, Quaternion.identity, transform.parent);
                    break;
                case 5:
                    Instantiate(/*gameController.GetComponent<GameController>().*/Items[3], transform.position, Quaternion.identity, transform.parent);
                    break;
                case 3:
                    Instantiate(/*gameController.GetComponent<GameController>().*/Items[4], transform.position, Quaternion.identity, transform.parent);
                    break;
                case 2:
                    Instantiate(/*gameController.GetComponent<GameController>().*/Items[0], transform.position, Quaternion.identity, transform.parent);
                    break;
                default:
                    break;
            }
            print(DropChance);


        }
    }

    
}
