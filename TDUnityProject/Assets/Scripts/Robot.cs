using UnityEngine;
using System.Collections;

public class Robot : MonoBehaviour
{
    public float moveSpeed, teleSpeed;
    public float yAdjust, xAdjust;

	/* Robot Movement Needs to go Left to Right, then hit a collider and tranport to the left side of the next floor */

    // Use this for initialization
    void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    public void MoveAndBuild(Vector3 destination)
    {
        StartCoroutine(FloorLogic(destination));
    }

    int GetCurrentFloor()
    {
        for (int i = 0; i < TowerPlacer.Instance.houseLevels.Length; i++)
        {
            if (gameObject.transform.position.y - yAdjust == TowerPlacer.Instance.houseLevels[i].yLower)
            {
                return i;
            }
        }
        return 0;
    }

    int SideOf(float compare)
    {
        if (gameObject.transform.position.x > compare)
        {
            return -1;
        }
        else
        {
            return 1;
        }
    }

    IEnumerator FloorLogic(Vector3 destination)
    {
        int floor = GetCurrentFloor();
        Vector3 newDestination = new Vector3(destination.x - (1 * SideOf(destination.x)), destination.y + yAdjust, destination.z);
        Vector3 floorChangeDestination;
        while (TowerPlacer.Instance.robotMovingTime)
        { 
            if (gameObject.transform.position.y == newDestination.y)
            {
                floor = GetCurrentFloor();
                yield return StartCoroutine(MoveBuild(newDestination));
            }
            else if (gameObject.transform.position.y > newDestination.y)
            {
                floor = GetCurrentFloor();
                floorChangeDestination = new Vector3(TowerPlacer.Instance.houseLevels[floor].waypointDown.x, gameObject.transform.position.y, 0);
                yield return StartCoroutine(MoveFloor(floorChangeDestination, -1));

            }
            else if (gameObject.transform.position.y < newDestination.y)
            {
                floor = GetCurrentFloor();
                floorChangeDestination = new Vector3(TowerPlacer.Instance.houseLevels[floor].waypointUp.x, gameObject.transform.position.y, 0);
                yield return StartCoroutine(MoveFloor(floorChangeDestination, 1));
            }
        }
        
        TowerPlacer.Instance.PlaceTower(destination, floor);
        yield return null;
    }

    IEnumerator MoveBuild(Vector3 end)
    {
        Vector3 start = Vector3.zero;
        while (true)
        {
            start = gameObject.transform.position;
            if (start == end)
            {
                break;
            }
            gameObject.transform.position = new Vector3(Vector3.MoveTowards(start, end, Time.deltaTime*moveSpeed).x, TowerPlacer.Instance.houseLevels[GetCurrentFloor()].yLower + yAdjust, 0);
            yield return new WaitForSeconds(Time.deltaTime/moveSpeed);
        }
        TowerPlacer.Instance.robotMovingTime = false;
        TowerPlacer.Instance.towerPlacingTime = true;
        yield return null;
    }

    IEnumerator MoveFloor(Vector3 end, int sign)
    {
        Vector3 start = Vector3.zero;
        while (true)
        {
            start = gameObject.transform.position;
            if (start == end)
            {
                break;
            }
            gameObject.transform.position = new Vector3(Vector3.MoveTowards(start, end, Time.deltaTime * moveSpeed).x, TowerPlacer.Instance.houseLevels[GetCurrentFloor()].yLower + yAdjust, 0);
            yield return new WaitForSeconds(Time.deltaTime / moveSpeed);
        }
        yield return new WaitForSeconds(teleSpeed);
        int floor = GetCurrentFloor();
        if (sign == 1)
        {
            gameObject.transform.position = new Vector3(TowerPlacer.Instance.houseLevels[floor].waypointUp.y, TowerPlacer.Instance.houseLevels[floor + 1].yLower + yAdjust, 0);
        }
        else
        {
            gameObject.transform.position = new Vector3(TowerPlacer.Instance.houseLevels[floor].waypointDown.y, TowerPlacer.Instance.houseLevels[floor + -1].yLower + yAdjust, 0);
        }
        yield return null;
    }
}
