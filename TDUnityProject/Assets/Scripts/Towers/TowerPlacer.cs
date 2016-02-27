using UnityEngine;
using System;
using System.Collections.Generic;

public class TowerPlacer : MonoBehaviour
{
    const int FALSEWAYPOINTVALUE = 50;
    public GameObject robotTest;
    public GameObject[] towersToPlace, towerShadows;
    public int towerplacementindex;
    public bool towerPlacingTime, robotMovingTime;
    //Exceptions are an x and y value where a tower can't be placed
    public List<Vector2> exceptions;
    public houseLevel[] houseLevels;
    public float towerXSnapModifier, towerYSnapModifier;
    GameObject towerGhost, previwTowerLocation, previewTower;
    int lastXpos;
    bool drawNewGhost;
   
    //Debug stuff
    public bool viewDebugPositions;
    public GameObject debugException, debugBound, debugWarp;
    public List<GameObject> debugObjects;

    public static TowerPlacer Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    // Use this for initialization
    void Start ()
    {
        towerPlacingTime = false;
        previwTowerLocation = GameObject.Find("PreviewTower");
        WarpExceptions();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.T) && !robotMovingTime)
        {
            towerPlacingTime = !towerPlacingTime;
            //Clears Ghost towers when not in tower placeing mode
            if (towerGhost != null)
            {
                Destroy(towerGhost);
                //print("Ghostbusters5");
                lastXpos = int.MaxValue;
            }
            //Clears preview tower when not in tower placing mode
            if (previewTower != null)
            {
                Destroy(previewTower.gameObject);
            }
        }

        DebugDraw();
      
        //Check if towers can be placed
        if (towerPlacingTime)
        {
            drawNewGhost = false;
            //Create tower prieview       
            if (previewTower == null)
            {
                previewTower = towersToPlace[towerplacementindex];
                previewTower = Instantiate(previewTower, previwTowerLocation.transform.position, transform.rotation) as GameObject;
                previewTower.transform.localScale *= 0.5f;
            }
            //Change Towers with scroll wheel
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                towerplacementindex++;
                if (towerplacementindex > towersToPlace.Length - 1)
                {
                    towerplacementindex = 0;
                }
                Destroy(towerGhost);
                drawNewGhost = true;
                Destroy(previewTower);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                towerplacementindex--;
                if (towerplacementindex < 0)
                {
                    towerplacementindex = towersToPlace.Length -1;
                }
                Destroy(towerGhost);
                drawNewGhost = true;
                Destroy(previewTower);
            }
            bool validPosition = false;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit mousePosition;
            //ray cast into the scene from mouse
            if (Physics.Raycast(ray, out mousePosition, 100))
            {
                //if a tower isn't already in clicked position
                if (mousePosition.collider.gameObject.GetComponent<Towers>() == null)
                {
                    for (int i = 0; i < houseLevels.Length; i++)
                    {
                        //Check if click is in acceptible placment bounds
                        if (mousePosition.point.x > houseLevels[i].xLeft && mousePosition.point.x < houseLevels[i].xRight && mousePosition.point.y > houseLevels[i].yLower && mousePosition.point.y < houseLevels[i].yUpper)
                        {
                            validPosition = true;
                            bool exceptionFound = false;
                            Vector3 modifiedPlacement = new Vector3((Mathf.Round(mousePosition.point.x) + towerXSnapModifier), (houseLevels[i].yLower + towerYSnapModifier), 0);
                            RaycastHit bufferCheck;
                            //Check for towers personal space
                            if (Physics.Raycast(modifiedPlacement, Vector3.right * houseLevels[i].levelDirection, out bufferCheck, towersToPlace[towerplacementindex].GetComponent<Towers>().placementBuffer))
                            {
                                if (bufferCheck.collider.gameObject.GetComponent<Towers>() != null)
                                {
                                    //print("Tower ahead!");
                                    exceptionFound = true;
                                }
                            }
                            //Check if tower's range goes out of bounds
                            if (houseLevels[i].xRight < (modifiedPlacement.x + towersToPlace[towerplacementindex].GetComponent<Towers>().placementBuffer) && houseLevels[i].levelDirection == 1)
                            {
                                //print("Too Far Over");
                                exceptionFound = true;
                            }
                            else if (houseLevels[i].xLeft > (modifiedPlacement.x - towersToPlace[towerplacementindex].GetComponent<Towers>().placementBuffer) && houseLevels[i].levelDirection == -1)
                            {
                                //print("Too Far Over");
                                exceptionFound = true;
                            }
                            //Check if mouse has moved out of previous tower placement position
                            if (Mathf.RoundToInt(modifiedPlacement.x) != lastXpos)
                            {
                                lastXpos = Mathf.FloorToInt(modifiedPlacement.x);
                                drawNewGhost = true;
                            }
                            //Destroys ghost marker after mouse moves to new build position
                            if (drawNewGhost && towerGhost != null)
                            {
                                Destroy(towerGhost);
                                //print("Ghostbusters");
                            }
                            //Looks for exceptions specified within the build bounds that have been designated as no build zones
                            for (int k = 0; k < exceptions.Count; k++)
                            {
                                //check if there is a designed exception on placement
                                if (modifiedPlacement.x == exceptions[k].x && (houseLevels[i].yLower == exceptions[k].y))
                                {
                                    exceptionFound = true;
                                }
                                //check if larger towers fall into the exception
                                if (houseLevels[i].levelDirection == 1)
                                {
                                    if ((modifiedPlacement.x + (towersToPlace[towerplacementindex].GetComponent<Towers>().placementBuffer * houseLevels[i].levelDirection) >= exceptions[k].x)
                                        && modifiedPlacement.x < exceptions[k].x && houseLevels[i].yLower == exceptions[k].y)
                                    {
                                        exceptionFound = true;
                                    }
                                }
                                else
                                {
                                    if ((modifiedPlacement.x + (towersToPlace[towerplacementindex].GetComponent<Towers>().placementBuffer * houseLevels[i].levelDirection) <= exceptions[k].x)
                                        && modifiedPlacement.x > exceptions[k].x && houseLevels[i].yLower == exceptions[k].y)
                                    {
                                        exceptionFound = true;
                                    }
                                }
                            }
                            if (!exceptionFound)
                            {
                                validPosition = true;
                                //Build Tower
                                if (Input.GetMouseButtonDown(0))
                                {
                                    robotMovingTime = true;
                                    towerPlacingTime = false;
                                    Vector3 modifiedForRobot = new Vector3(modifiedPlacement.x, modifiedPlacement.y - towerYSnapModifier, modifiedPlacement.z);
                                    robotTest.GetComponent<Robot>().MoveAndBuild(modifiedForRobot);
                                }
                                //Draw Ghost tower
                                else if (drawNewGhost)
                                {
                                    towerGhost = Instantiate(towerShadows[towerplacementindex], modifiedPlacement, transform.rotation) as GameObject;
                                    if (houseLevels[i].levelDirection == -1)
                                    {
                                        towerGhost.transform.Rotate(0, 180, 0);
                                    }
                                }
                            }
                            if (robotMovingTime)
                            {
                               // towerPlacingTime = false;
                                //Vector3 modifiedForRobot = new Vector3(modifiedPlacement.x, modifiedPlacement.y - towerYSnapModifier, modifiedPlacement.z);
                               // robotTest.GetComponent<Robot>().MoveAndBuild(modifiedForRobot);
                            }
                        }
                    }
                    //Kills ghost marker after leaving vaild build bounds
                    if (!validPosition && towerGhost != null)
                    {
                        Destroy(towerGhost);
                        //print("Ghostbusters2");
                        lastXpos = int.MaxValue;
                    }
                }
                //Destroy built towers
                else if (Input.GetMouseButtonDown(1))
                {
                    Destroy(mousePosition.collider.gameObject);
                }
                //kills ghost marker after hovering over a built tower
                else if (towerGhost != null)
                {
                    Destroy(towerGhost);
                    //print("Ghostbusters4");
                    lastXpos = int.MaxValue;
                }
            }
        }
	}

    public void PlaceTower(Vector3 modifiedPlacement, int houseLevel)
    {
        Vector3 modifiedForTower = new Vector3(modifiedPlacement.x, modifiedPlacement.y + towerYSnapModifier, modifiedPlacement.z);
        GameObject tempTowerHolder;
        tempTowerHolder = Instantiate(towersToPlace[towerplacementindex], modifiedForTower, transform.rotation) as GameObject;
        if (houseLevels[houseLevel].levelDirection == -1)
        {
            tempTowerHolder.transform.Rotate(0, 180, 0);
        }
        //Kills ghost marker when tower is placed
        if (towerGhost != null)
        {
            Destroy(towerGhost);
            //print("Ghostbusters3");
        }
    }

    //Draws markers for exceptions bounds and warps
    void DebugDraw()
    {
        if (viewDebugPositions)
        {
            if (debugObjects.Count == 0)
            {
                //Draw exceptions
                for (int e =0; e < exceptions.Count; e++)
                {
                    Vector3 pos = new Vector3(exceptions[e].x, exceptions[e].y, 0);
                    GameObject temp = Instantiate(debugException, pos, gameObject.transform.rotation) as GameObject;
                    debugObjects.Add(temp);
                }
                for (int f = 0; f < houseLevels.Length; f++)
                {
                    //Draw bounds
                    Vector3 pos = new Vector3(houseLevels[f].xLeft, houseLevels[f].yLower, 0);                    
                    GameObject temp = Instantiate(debugBound, pos, gameObject.transform.rotation) as GameObject;
                    debugObjects.Add(temp);
                    pos = new Vector3(houseLevels[f].xRight, houseLevels[f].yLower, 0);
                    temp = Instantiate(debugBound, pos, gameObject.transform.rotation) as GameObject;
                    debugObjects.Add(temp);
                    //Draw Waypoints
                    if (houseLevels[f].waypointUp.x < FALSEWAYPOINTVALUE)
                    {
                        pos = new Vector3(houseLevels[f].waypointUp.x, houseLevels[f].yLower, 0);
                        temp = Instantiate(debugWarp, pos, gameObject.transform.rotation) as GameObject;
                        debugObjects.Add(temp);
                        pos = new Vector3(houseLevels[f].waypointUp.y, houseLevels[f + 1].yLower, 0);
                        temp = Instantiate(debugWarp, pos, gameObject.transform.rotation) as GameObject;
                        debugObjects.Add(temp);
                    }
                    if (houseLevels[f].waypointDown.x < FALSEWAYPOINTVALUE)
                    {
                        pos = new Vector3(houseLevels[f].waypointDown.x, houseLevels[f].yLower, 0);
                        temp = Instantiate(debugWarp, pos, gameObject.transform.rotation) as GameObject;
                        debugObjects.Add(temp);
                        pos = new Vector3(houseLevels[f].waypointDown.y, houseLevels[f - 1].yLower, 0);
                        temp = Instantiate(debugWarp, pos, gameObject.transform.rotation) as GameObject;
                        debugObjects.Add(temp);
                    }
                }
            }
        }
        else
        {
            if (debugObjects.Count > 0)
            {
                foreach (GameObject g in debugObjects)
                {
                    Destroy(g);
                }
                debugObjects.Clear();
            }
        }
    }

    //Make warps also exceptions
    void WarpExceptions()
    {
        for (int f = 0; f < houseLevels.Length; f++)
        {
            if (houseLevels[f].waypointUp.x < FALSEWAYPOINTVALUE)
            {
                Vector2 temp = new Vector2(houseLevels[f].waypointUp.x, houseLevels[f].yLower);
                exceptions.Add(temp);
                temp = new Vector2(houseLevels[f].waypointUp.y, houseLevels[f + 1].yLower);
                exceptions.Add(temp);
            }
            if (houseLevels[f].waypointDown.x < FALSEWAYPOINTVALUE)
            {
                Vector2 temp = new Vector2(houseLevels[f].waypointDown.x, houseLevels[f].yLower);
                exceptions.Add(temp);
                temp = new Vector2(houseLevels[f].waypointDown.y, houseLevels[f - 1].yLower);
                exceptions.Add(temp);
            }
        }
    }
}

[Serializable]
public class houseLevel
{
    public int xLeft, xRight;
    public float yLower, yUpper;
    public int levelDirection;
    //Direction indicated by 1 for right to left moving enemies and -1 for left to right moving enemies
    public Vector2 waypointUp, waypointDown;
    //x values of current level entrance and destination level exit. 
    //Gets y value from yMin of previous or next element in array. 
    //Value of 100 indicates no waypoint
}