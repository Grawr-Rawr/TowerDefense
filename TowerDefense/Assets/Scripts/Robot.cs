using UnityEngine;
using System.Collections;

public class Robot : MonoBehaviour 
{
	public float health = 0.0f;
	public float shieldStrength = 0.0f;
	public float distance = 0.0f;
	public float rotationSpeed = 3.0f;

	public Material material;

	public GameObject minion = null;
	public GameObject projectile = null;
    public GameObject item = null;
    public GameObject[] items = null;

	// Use this for initialization
	void Start () 
	{
       
	}
	
	// Update is called once per frame
	void Update () 
	{
		//distance = Vector3.Distance (transform.position, );


		Attack ();
		Die ();
        CollectItem();
	}

	public void Die()
	{
		if (health <= 0.0f) 
		{
			Destroy(gameObject);
		}
		
	}

	public void Attack()
	{
		minion  = GameObject.FindGameObjectWithTag("Minion");

		//transform.LookAt(minion.transform);

		if(Input.GetKeyDown (KeyCode.Space))
		{
			Instantiate (projectile, transform.position, transform.rotation);
		}

		if(Input.GetKey (KeyCode.LeftArrow))
		{

			transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
		}
		
		if(Input.GetKey (KeyCode.RightArrow))
		{
			transform.Rotate(Vector3.down * rotationSpeed * Time.deltaTime);

		}


	}

    public void CollectItem()
    {
         item = GameObject.FindWithTag("Item");
         //items  
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            print("You have collected an item!");
            Destroy(item);
        }

    }
}
