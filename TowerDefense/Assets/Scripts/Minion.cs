using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Minion : MonoBehaviour 
{
	public float speed = 0.0f;
	public float health = 0.0f;
    public bool isAtHouse = false;

	public GameObject projectile = null;
    public GameObject item = null;

    public Slider healthBar;

	// Use this for initialization
	void Start () 
	{
	    
	}
	
	// Update is called once per frame
	void Update () 
	{

        if (isAtHouse != true)
        {
            MoveToHouse();
        }

		
	
	}


    public void MoveToHouse()
    {
        //transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "House")
        {
            isAtHouse = true;
            Destroy(gameObject);

        }

		

        
    }

    public void TakeDamage (float amount)
    {
        health -= amount;
        healthBar.value = (health / 100.0f);
            //add maxHealth variable later


        if (health <= 0.0f)
        {
            Instantiate(item, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
