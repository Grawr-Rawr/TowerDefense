using UnityEngine;
using System.Collections;

public class Robot : MonoBehaviour 
{
	public float health = 0.0f;
	public float shieldStrength = 0.0f;

	public Material material;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		Die ();
	}

	public void Die()
	{
		if (health <= 0.0f) 
		{
			Destroy(gameObject);
		}
		
	}
}
