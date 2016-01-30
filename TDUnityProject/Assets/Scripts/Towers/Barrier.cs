using UnityEngine;
using System.Collections;

public class Barrier : Towers
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
        //Reduce enemy Movment
    }

    void OnTriggerExit(Collider col)
    {
        //Restore enemy speed
        TowerTakesDamage();
    }

    void OnTriggerStay(Collider col)
    {
        //Get Enemy Attack Speed
        /*
        float timer
        if(enemyAttackSpeed > Timer)
        {
            TowerTakesDamage(enemyDamage);
            timer = 0;
        } 
        else
        {
            timer += Time.deltaTime;
        }
        */       
    }
}
