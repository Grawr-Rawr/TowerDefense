using UnityEngine;
using System.Collections;

public class Minion : MonoBehaviour 
{
	public float speed = 0.0f;
	public float health = 0.0f;
    public bool isAtHouse = false;

	public GameObject projectile = null;


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

		if (health <= 0.0f) 
		{
            Destroy(gameObject);
		}
	
	}


    public void MoveToHouse()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "House")
        {
            isAtHouse = true;
            Destroy(gameObject);

        }

		if(other.gameObject.tag == "Projectile")
		{
			health -= projectile.GetComponent<Projectile>().damage;
			
		}

        
    }
}
