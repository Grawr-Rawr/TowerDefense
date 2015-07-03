using UnityEngine;
using System.Collections;

public class MinionSpawner : MonoBehaviour 
{
	public GameObject minion = null;
	// Use this for initialization
	void Start () 
	{
		//print ("hello");
		Instantiate (minion, transform.position, transform.rotation);
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Instantiate (minion, transform.position, transform.rotation);
	
	}
}
