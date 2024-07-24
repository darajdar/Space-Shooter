using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour
{
    
	public float speed;
    public float boost = 15;
    public GameObject _Player;

    //These are for tracking the enemy with missile shot.
    private Transform target;
    private Rigidbody rb;
    private Quaternion lookRotation;



    //This explosion damages enemies when the touch it
    public GameObject MissileExplosion;
    void Start()
    {
        //testing
        
        rb = GetComponent<Rigidbody>();

        _Player = GameObject.FindWithTag("Player");
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
        StartCoroutine(RocketBoost());
    }

    //This is supposed to make sure the bullet doesn't turn with the ships turning. It doesn't. The tilt on enemy ships is too high I guess.
    private void Update()
    {
        MeleeFunction();
        //Quaternion target = Quaternion.LookRotation(new Vector3(0, 0, 0), transform.up);
        

    }
    
    void MeleeFunction()
    {
        if (gameObject.tag == "Melee" && Input.GetKey(KeyCode.Mouse0))
        {
            
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {


        }
    }
    //If the bullet is a missile, speed up after finding an enemy.
    IEnumerator RocketBoost()
    {
        yield return new WaitUntil(() => gameObject.tag == "Missile");

        yield return new WaitForSeconds(.5f);


        target = FindTarget();

        
        //GetComponent<Rigidbody>().velocity = transform.forward * boost;

    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;


            lookRotation = Quaternion.LookRotation(direction, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2.5f);

            transform.position = (transform.position + transform.forward * boost * Time.deltaTime);
            /*GetComponent<Transform>().position = new Vector3
             (
            Mathf.Clamp(GetComponent<Rigidbody>().position.x, -20.2f, 20.2f),
            0.0f,
            Mathf.Clamp(GetComponent<Rigidbody>().position.z, -4.4f, 14.4f)
            );*/
        } else
        {
            //Destroy(this.gameObject);
        }
    }

    //Search through all the enemies currently enabled, then set the transform to that enemies transform.
    public Transform FindTarget()
    {
        GameObject[] candidates = GameObject.FindGameObjectsWithTag("Enemy");
        float minDistance = Mathf.Infinity;
        Transform closest;

        if (candidates.Length == 0)
            return null;

        closest = candidates[0].transform;
        for (int i = 0; i < candidates.Length; ++i)
        {
            //If the next enemys distance is shorter than the previous, that enemy is saved as the closest.
            float distance = (candidates[i].transform.position - transform.position).sqrMagnitude;

            if (distance < minDistance)
            {
                closest = candidates[i].transform;
                minDistance = distance;
            }
        }
        return closest;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.tag == "Missile")
        if (other.tag == "EnemyBullet" || other.tag == "Enemy" || other.tag == "Boss" || other.tag == "Asteroid")
        {
            if (MissileExplosion != null)
            {
                Instantiate(MissileExplosion, transform.position, transform.rotation);
            }
            Destroy(gameObject);

                //Return Missile Fired as false so another missile can be shot
                _Player.GetComponent<PlayerController>().MissileFired = false;
                

        }

        if(other.tag == "Player")
        {
            return;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Boundary")
        {
            if (_Player != null)
            {
                _Player.GetComponent<PlayerController>().MissileFired = false;
            }
        }
    }
}
