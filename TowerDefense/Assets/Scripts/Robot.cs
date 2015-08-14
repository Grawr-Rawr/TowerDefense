using UnityEngine;
using System.Collections;

public class Robot : MonoBehaviour 
{
	public float health = 0.0f;
	public float shieldStrength = 0.0f;
	public float distance = 0.0f;
	public float rotationSpeed = 3.0f;

    bool isFiring = false;

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


        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
        Turn();
		Die ();
        if (Input.GetKeyDown(KeyCode.C))
        {
            CollectItem();
        }
	}

    private void Turn()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {

            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(Vector3.down * rotationSpeed * Time.deltaTime);

        }
    }

    public void DetermineAutoAttack(bool isFiring)
    {
        if (isFiring)
        {
            if (!this.isFiring)
            {
                StartCoroutine("AutoAttack");
                this.isFiring = true;
            }
        }
        else
        {
            StopCoroutine("AutoAttack");
            this.isFiring = false;
        }
    }

	public void Die()
	{
		if (health <= 0.0f) 
		{
			Destroy(gameObject);
		}
		
	}

	protected void Attack()
	{
		minion  = GameObject.FindGameObjectWithTag("Minion");

		//transform.LookAt(minion.transform);

		
	    Instantiate (projectile, transform.position, transform.rotation);
		

		
	}

    public void CollectItem()
    {
         item = GameObject.FindWithTag("Item");
         //items  
        
        
            print("You have collected an item!");
            Destroy(item);
        

    }

    IEnumerator AutoAttack()
    {
        Attack();

        yield return new WaitForSeconds(.5f);

        StartCoroutine("AutoAttack");
    }
}
