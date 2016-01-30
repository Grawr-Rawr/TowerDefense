using UnityEngine;
using System.Collections;

public class BigFan : Towers
{

	// Use this for initialization
	void Start ()
    {
	
	}

    protected override void TowerUpdateBehavior()
    {
        //Will be different for evey tower :)
        //Nothing for this tower?
    }

    void OnTriggerEnter(Collider col)
    {
        //Reduce enemy speed
    }

    void OnTriggerExit(Collider col)
    {
        //Restore enemy speed
    }

    void OnTriggerStay(Collider col)
    {
        float timer = 0;
        if (timer > 1.0f)
        {
            TowerTakesDamage();
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
}
