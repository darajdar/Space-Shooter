using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour
{
	public GameObject explosion;
	public GameObject playerExplosion;
	public int scoreValue;
	public GameController gameController;



    void Start()
    {
        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
        
      
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Boundary" || other.tag == "Enemy" || other.tag == "EnemyBullet")
		{
            
            return;
		}


        

		if (other.tag == "Player")
		{
			
            if (!ModGlobalControl.Instance.Practice)
            {
                ModGlobalControl.Instance.Health -= 15;
               
            }
            
            //The explosions refuse to delete for whatever reason
            if (explosion != null)
            {
                Instantiate(explosion, transform.position, transform.rotation);
            }

            if (ModGlobalControl.Instance.Health <= 0)
            {
                Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                Destroy(other.gameObject);
                
                gameController.GameOver();
            }
			
		}

        //Im making the missile destroy by contact happen in the mover script since it has a special property.


        if (other.tag == "Bullet")
        {
            if (explosion != null)
            {
                Instantiate(explosion, transform.position, transform.rotation);
            }
            Destroy(other.gameObject);
            
        }

        if (other.tag == "Laser")
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }
        if (other.tag == "Missile")
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }

        if (other.tag == "Melee")
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }


        //gameController.AddScore(scoreValue);
        ModGlobalControl.Instance.Score += scoreValue;
            
    }

}