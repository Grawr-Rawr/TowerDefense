using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour 
{
	public float speed = 4.0f;
	public float damage = 10.0f;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Translate(Vector3.forward * speed * Time.deltaTime);
	}

	public void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Minion") 
		{
			Destroy (gameObject);
		}

		if (other.gameObject.tag == "ProjectileKiller") 
		{
			Destroy (gameObject);
		}

	}
}
