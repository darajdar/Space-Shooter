using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

public class LBStats : EnemyStats
{
    //Spice up boss gameplay with multiple quick movements
    public int MoveCounter;
    public int MoveCounterMax;
    public int MoveRestartTime;
    // Start is called before the first frame update
    public override void Start()
    {
        MoveCounter = 0;
        MoveCounterMax = 3; ;

    MoveRestartTime = 3;
        tilt = 3;
        dodge = 5;
        JumpDistance = 70;
        maneuverTime = new Vector2(.4f, .5f);
        maneuverWait = new Vector2(.75f, .75f);
        EnemyHealth = 100;
        FleeDistance = 0;
        AttackSpeed = .75f;
        EvadeSpeed = 1f;
        Offset = 0;
        player = GameObject.FindWithTag("Player");
        _gameController = GameObject.FindWithTag("GameController");
        enemyHealthBarSlider.minValue = 0;
        enemyHealthBarSlider.maxValue = EnemyHealth;
        StartCoroutine(EnemyAI());



        //targetManeuver = GetComponent<Rigidbody>().velocity;

        //Set the max and min values for the healthbar
        enemyHealthBarSlider.minValue = 0;
        enemyHealthBarSlider.maxValue = EnemyHealth;


        //Run enemy AI. This will choose between different coroutines to make the enemy move.
        StartCoroutine(EnemyAI());

        //--- Continue this.
        //StartCoroutine(Chase());
        //StartCoroutine(Dodge());
    }




    // Start is called before the first frame update

    // Update is called once per frame
    public void Update()
    {
        
        //Test is balanced
        /*if (!gameObject.GetComponent<LEWeaponController>().Firing)
        {
            gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }*/
        if(EnemyHealth <= 50)
        {
            MoveCounterMax = 5;
        }
        if (player != null)
        {
            //Movement();
            FacePlayer();
            HealthBar();

        }

       

    }
    

    //Set enemy healthbar, and make the healthbar follow the enemy, and face the camera always


    public override void FacePlayer()
    {
        if (player != null)
        {


            //Make the enemy face the player. The vector enemy is refering to the player.
            Enemy = player.GetComponent<Transform>().position - transform.position;
            Enemy.y = 0;


            direction = (Enemy - transform.position).normalized;

            Quaternion target = Quaternion.LookRotation(Enemy, transform.up);

            // transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime);

            transform.rotation = target;
        }
    }

    //Might need this is other enemys have different AI.
    public override IEnumerator EnemyAI()
    {
        yield return new WaitForSeconds(startWait.x);
        //Assigns the AIRange between the multiple options for the movement choices.
        while (player != null)

        {
            //Let the boss do 3 quick movements, then wait
            if (MoveCounter >= MoveCounterMax)
            {
                yield return new WaitForSeconds(MoveRestartTime);
                MoveCounter = 0;
            } else { 
                if (FleeState == true)
                {
                    FleeState = true;
                    StartCoroutine(Flee());
                }
                else if (FleeState == false)
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
                            StartCoroutine(Chase());
                            break;
                        default:
                            break;

                    }
                    MoveCounter++;

                }
            }
            yield return new WaitForSeconds(AttackSpeed);
        }
    }


    //The two current AI values are here to change the distance the AI moves.
    public override IEnumerator Chase()
    {
        //Chase the player
        yield return new WaitForSeconds(Random.Range(startWait.x, startWait.y));
        //while (FleeState == true)
        //{
        //Make the target of the enemy move towards the player, but not in a way that is dangerous.
        if (player != null)
        {
            targetManeuver = new Vector3((player.GetComponent<Transform>().position.x - transform.position.x) / 1.5f, 0, (player.GetComponent<Transform>().position.z - transform.position.z) * 2);
            yield return new WaitForSeconds(Random.Range(maneuverTime.x, maneuverTime.y));

            targetManeuver = new Vector3(0, 0, 0);
            //yield return new WaitForSeconds(Random.Range(maneuverWait.x, maneuverWait.y));

        }

    }

    public override IEnumerator Dodge()
    {
        yield return new WaitForSeconds(Random.Range(startWait.x, startWait.y));
        //while (player != null)
        if (player != null)
        {
            targetManeuver = new Vector3((-player.GetComponent<Transform>().position.x - transform.position.x), 0, (-player.GetComponent<Transform>().position.z - transform.position.z) * 2.5f);
            yield return new WaitForSeconds(Random.Range(maneuverTime.x, maneuverTime.y));
            targetManeuver = new Vector3(0, 0, 0);
            yield return new WaitForSeconds(Random.Range(maneuverWait.x, maneuverWait.y));
        }

    }

    public override void OnTriggerEnter(Collider other)
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
            EnemyHealth -= 4;
            //Destroy(other.gameObject);


        }

        if (other.tag == "EnemyBullet" && enemyBullet.GetComponent<EnemyMover>().Reflected)
        {
            EnemyHealth -= .5f;
            //Destroy(other.gameObject);
            if (explosion != null)
            {
                Instantiate(explosion, transform.position, transform.rotation);
            }

        }



        if (other.tag == "Melee")
        {
            EnemyHealth -= 2;

            //--- This might be a cool way to do damage with this. Return to this later.
            //Destroy(other.gameObject);
            if (explosion != null)
            {
                Instantiate(explosion, transform.position, transform.rotation);
            }

        }
    }

    //Deal laser damage.
    public override void OnTriggerStay(Collider other)
    {
        if (other.tag == "Laser")
        {
            EnemyHealth -= .01f;



        }
    }
    public override void CheckForDeath()
    {
        if (EnemyHealth <= 0)
        {
            ModGlobalControl.Instance.Boss1Defeated = true;
            
            _gameController.GetComponent<GameController>().Celebration();
            Destroy(this.gameObject);

            //Introduce a drop chance
           /* int DropChance = Random.Range(0, 5) + Random.Range(0, 5);

            switch (DropChance)
            {
                case 12:
                    Instantiate(Items[0], transform.position, Quaternion.identity, transform.parent);
                    break;
                case 11:
                    Instantiate(Items[2], transform.position, Quaternion.identity, transform.parent);
                    break;
                case 10:
                    Instantiate(Items[3], transform.position, Quaternion.identity, transform.parent);
                    break;
                case 8:
                    Instantiate(Items[1], transform.position, Quaternion.identity, transform.parent);
                    break;
                case 7:
                    Instantiate(Items[1], transform.position, Quaternion.identity, transform.parent);
                    break;
                case 6:
                    Instantiate(Items[1], transform.position, Quaternion.identity, transform.parent);
                    break;
                case 5:
                    Instantiate(Items[3], transform.position, Quaternion.identity, transform.parent);
                    break;
                case 3:
                    Instantiate(Items[4], transform.position, Quaternion.identity, transform.parent);
                    break;
                case 2:
                    Instantiate(Items[0], transform.position, Quaternion.identity, transform.parent);
                    break;
                default:
                    break;
            }
            print(DropChance);*/


        }
    }
}
