using UnityEngine;
using System.Collections;

public class BrickWall : Towers
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
        //Signal Enemy to stop moving?
    }

    void OnTriggerExit(Collider col)
    {
        //Signal Enemy to Start Moving
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
