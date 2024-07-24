using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HEStats : EnemyStats
{

    public override void Start()
    {
        tilt = 10;
        dodge = 5;
        JumpDistance = 50;
        maneuverTime = new Vector2(.1f, .2f);
        maneuverWait = new Vector2(4, 6);
        EnemyHealth = 8;
        FleeDistance = 5;
        AttackSpeed = 2.5f;
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
    void Update()
    {

        if (player != null)
        {
            //Movement();
            FacePlayer();
            HealthBar();
            
        }
    }

    //Might need this is other enemys have different AI.
    public override IEnumerator EnemyAI()
    {
        yield return new WaitForSeconds(startWait.x);
        //Assigns the AIRange between the multiple options for the movement choices.
        while (player != null)

        {
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
                        StartCoroutine(Dodge());
                        break;
                    default:
                        break;

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
            targetManeuver = new Vector3((player.GetComponent<Transform>().position.x - transform.position.x) /1.5f, 0, (player.GetComponent<Transform>().position.z - transform.position.z) * 2);
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

}
