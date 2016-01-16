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
            //print("TowerPlacingTime is: " + towerPlacingTime);
        }

        //Check if towers can be placed
        if (towerPlacingTime)
        {
            //Once mouse is left clicked
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit mousePosition;
                //ray cast into the scene from mouse
                if (Physics.Raycast(ray, out mousePosition, 100))
                {
                    //print(mousePosition.point);
                    //if a tower isn't already in clicked position
                    if(mousePosition.collider.gameObject.GetComponent<Towers>() == null)
                    {
                        for (int i = 0; i < xLeft.Length; i++)
                        {                            
                            //Check if click is in acceptible placment bounds
                            if (mousePosition.point.x > xLeft[i] && mousePosition.point.x < xRight[i] && mousePosition.point.y > yLower[i] && mousePosition.point.y < yUpper[i])
                            {
                                //print("placement time");
                                bool exceptionFound = false;
                                for (int k = 0; k < exceptions.Length; k++)
                                {
                                    //check if there is a designed exception on placement
                                    if ((Mathf.Round(mousePosition.point.x) == exceptions[k].x) && (yLower[i] == exceptions[k].y))
                                    {
                                        //print(exceptions[k] + "vs (" + Mathf.Round(mousePosition.point.x) + ", " + yLower[i] + ")");
                                        exceptionFound = true;
                                    }
                                }
                                if (!exceptionFound)
                                {
                                    Vector3 modifiedPlacement = new Vector3((Mathf.Round(mousePosition.point.x) + towerXSnapModifier), (yLower[i] + towerYSnapModifier), 0);
                                    Instantiate(towersToPlace[towerplacementindex], modifiedPlacement, transform.rotation);
                                }
                            }                            
                        }
                    }
                }
            }
        }
	}
}