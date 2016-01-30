using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
    public GameObject[] towersToPlace, towerShadows;
    public int towerplacementindex;
    public bool towerPlacingTime;
    public int[] xLeft, xRight;
    public float[] yLower, yUpper;
    public Vector2[] exceptions;
    public float towerXSnapModifier, towerYSnapModifier;
    GameObject towerGhost;
    int lastXpos;
    bool drawNewGhost;

    // Use this for initialization
    void Start ()
    {
        towerPlacingTime = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            towerPlacingTime = !towerPlacingTime;
        }
      
        //Check if towers can be placed
        if (towerPlacingTime)
        {
            bool validPosition = false;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit mousePosition;
            //ray cast into the scene from mouse
            if (Physics.Raycast(ray, out mousePosition, 100))
            {
                //if a tower isn't already in clicked position
                if (mousePosition.collider.gameObject.GetComponent<Towers>() == null)
                {
                    drawNewGhost = false;
                    for (int i = 0; i < xLeft.Length; i++)
                    {
                        //Check if click is in acceptible placment bounds
                        if (mousePosition.point.x > xLeft[i] && mousePosition.point.x < xRight[i] && mousePosition.point.y > yLower[i] && mousePosition.point.y < yUpper[i])
                        {
                            validPosition = true;
                            Vector3 modifiedPlacement = new Vector3((Mathf.Round(mousePosition.point.x) + towerXSnapModifier), (yLower[i] + towerYSnapModifier), 0);
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
                            bool exceptionFound = false;
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
                                    Instantiate(towersToPlace[towerplacementindex], modifiedPlacement, transform.rotation);
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
                //kills ghost marker after hovering over a built tower
                else if(towerGhost != null)
                {
                    Destroy(towerGhost);
                    //print("Ghostbusters4");
                    lastXpos = int.MaxValue;
                }
            }
        }
	}
}