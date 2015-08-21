using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathManager : MonoBehaviour 
{
    public List<GameObject> path = new List<GameObject>();
	
    private static PathManager instance;

    public static PathManager Instance
    {
        get 
        {
             if (instance == null)
             {
                 instance = new PathManager();
            }
            return instance;
        }
    }

    public void Awake()
    {
        instance = this;
    }
}
