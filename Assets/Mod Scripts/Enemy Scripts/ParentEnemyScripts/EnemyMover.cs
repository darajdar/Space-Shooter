using UnityEngine;
using System.Collections;

//I'll need this parent script for when the bullets or projectiles don't just move in a straight line
public class EnemyMover : MonoBehaviour
{
	public float speed;
    private Quaternion target;

    
    public GameObject _Player;
    public GameObject _Enemy;

    public GameObject enemyBullet;

    //This variable allows enemy shots to hit themselves when reflected.

    public bool Reflected;

    private void Update()
    {
       //target = Quaternion.LookRotation(Vector3.forward, transform.up);
    }
    public virtual void Start ()
	{

        _Player = GameObject.FindWithTag("Player");
        //_Enemy = GameObject.FindWithTag("Enemy");
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
        //transform.rotation = target;
	}

    public virtual void OnTriggerEnter(Collider other)
    {
        enemyBullet = other.gameObject;
        /*if (other.tag == "Enemy" && Reflected)
        {
            _Enemy = other.gameObject;
            //_Enemy.GetComponent<EnemyStats>().EnemyHealth-= 1;
            print(_Enemy.GetComponent<EnemyStats>().EnemyHealth);

            Destroy(this.gameObject);
            
        } else */if (other.tag == "Enemy" /* && !Reflected*/)
        {

            return;
            
        }

        if (other.tag == "Player")
        {
            Destroy(this.gameObject);
        }

        if (other.tag == "Bullet")
        {
            Destroy(this.gameObject);
        }
        if (other.tag == "Laser")
        {
            Destroy(this.gameObject);
        }

        if (other.tag == "Melee")
        {
            Destroy(this.gameObject);
        }

        if (other.tag == "EnemyBullet" && enemyBullet.GetComponent<EnemyMover>().Reflected)
        {
            //Destroy(this.gameObject);
            

        }

        //Deflect bullets if shield is up
        if (other.tag == "Shield")
        {
            Reflected = true;
            transform.rotation = _Player.transform.rotation;
            GetComponent<Rigidbody>().velocity = transform.forward * speed * 1.4f;
            

            //Reduce shield energy
            if (!ModGlobalControl.Instance.Practice)
            {
                ModGlobalControl.Instance.ShieldWeaponCharge -= 5;
            }
        }
    }


}
