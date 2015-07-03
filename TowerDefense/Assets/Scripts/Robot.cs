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

		if(Input.GetKeyDown (KeyCode.LeftArrow))
		{

			transform.Rotate(Vector3.left * rotationSpeed * Time.deltaTime);
		}
		
		if(Input.GetKeyDown (KeyCode.RightArrow))
		{
			transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);

		}


	}
}
