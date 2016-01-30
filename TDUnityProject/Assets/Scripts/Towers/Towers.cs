using UnityEngine;
using System.Collections;

public class Towers : MonoBehaviour
{
    public float towerHealthMax, towerHealthCurrent;
    public float towerCost;
    public float damageToTake;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        gameObject.GetComponent<Towers>().TowerUpdateBehavior();

        if (towerHealthCurrent < 0)
        {
            Destroy(this.gameObject);
        }

        if (towerHealthCurrent > towerHealthMax)
        {
            towerHealthCurrent = towerHealthMax;
        }
	}

    protected virtual void TowerUpdateBehavior()
    {
        //Will be different for evey tower :)
        //print("Error: Towers Update Called...");
    }

    protected void TowerTakesDamage()
    {
        towerHealthCurrent -= damageToTake;
    }

    protected void TowerHealsDamage(float damageToHeal)
    {
        towerHealthCurrent += damageToHeal;
    }

}
