using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
    public GameObject[] towersToPlace, towerShadows;
    public int towerplacementindex;
    public bool towerPlacingTime;
    public int[] xLeft, xRight;
    public float[] yLower, yUpper;
    //Direction indicated by 1 for right to left moving enemies and -1 for left to right moving enemies
    public int[] levelDirection;
    //Exceptions are an x and y value where a tower can't be placed
    public Vector2[] exceptions;
    public float towerXSnapModifier, towerYSnapModifier;
    GameObject towerGhost, previwTowerLocation, previewTower;
    int lastXpos;
    bool drawNewGhost;

    // Use this for initialization
    void Start ()
    {
        towerPlacingTime = false;
        previwTowerLocation = GameObject.Find("PreviewTower");
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.T))
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
                    for (int i = 0; i < xLeft.Length; i++)
                    {
                        //Check if click is in acceptible placment bounds
                        if (mousePosition.point.x > xLeft[i] && mousePosition.point.x < xRight[i] && mousePosition.point.y > yLower[i] && mousePosition.point.y < yUpper[i])
                        {
                            validPosition = true;
                            bool exceptionFound = false;
                            Vector3 modifiedPlacement = new Vector3((Mathf.Round(mousePosition.point.x) + towerXSnapModifier), (yLower[i] + towerYSnapModifier), 0);
                            RaycastHit bufferCheck;
                            //Check for towers personal space
                            if (Physics.Raycast(modifiedPlacement, Vector3.right * levelDirection[i], out bufferCheck, towersToPlace[towerplacementindex].GetComponent<Towers>().placementBuffer))
                            {
                                if (bufferCheck.collider.gameObject.GetComponent<Towers>() != null)
                                {
                                    //print("Tower ahead!");
                                    exceptionFound = true;
                                }
                            }
                            //Check if tower's range goes out of bounds
                            if (xRight[i] < (modifiedPlacement.x + towersToPlace[towerplacementindex].GetComponent<Towers>().placementBuffer) && levelDirection[i] == 1)
                            {
                                //print("Too Far Over");
                                exceptionFound = true;
                            }
                            else if (xLeft[i] > (modifiedPlacement.x - towersToPlace[towerplacementindex].GetComponent<Towers>().placementBuffer) && levelDirection[i] == -1)
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
                            for (int k = 0; k < exceptions.Length; k++)
                            {
                                //check if there is a designed exception on placement
                                if ((Mathf.Round(mousePosition.point.x) == exceptions[k].x) && (yLower[i] == exceptions[k].y))
                                {
                                    exceptionFound = true;
                                }
                            }
                            if (!exceptionFound)
                            {
                                validPosition = true;
                                //Build Tower
                                if (Input.GetMouseButtonDown(0))
                                {
                                    GameObject tempTowerHolder;
                                    tempTowerHolder = Instantiate(towersToPlace[towerplacementindex], modifiedPlacement, transform.rotation) as GameObject;
                                    if (levelDirection[i] == -1)
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
                                //Draw Ghost tower
                                else if (drawNewGhost)
                                {
                                    towerGhost = Instantiate(towerShadows[towerplacementindex], modifiedPlacement, transform.rotation) as GameObject;
                                    if (levelDirection[i] == -1)
                                    {
                                        towerGhost.transform.Rotate(0, 180, 0);
                                    }
                                }
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
}