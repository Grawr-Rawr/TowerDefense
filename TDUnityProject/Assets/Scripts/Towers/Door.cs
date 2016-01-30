using UnityEngine;
using System.Collections;

public class Door : Towers
{

	// Use this for initialization
	void Start ()
    {
	
	}

    protected override void TowerUpdateBehavior()
    {
        //Will be different for evey tower :)
    }

    void OnTriggerEnter(Collider col)
    {

    }

    void OnTriggerExit(Collider col)
    {

    }

    void OnTriggerStay(Collider col)
    {

    }
}
