using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathMovement : MonoBehaviour 
{
    public float speed = 0.0f;

    private List<GameObject> path = new List<GameObject>();
    int index = -1;
    GameObject currentNode = null;

	// Use this for initialization
	void Start () 
    {
        path = PathManager.Instance.path;
        NewNode();
	}
	
	// Update is called once per frame
	void Update () 
    {
        transform.LookAt(currentNode.transform);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
	}

    void NewNode()
    {
        index++;

        try
        {
            currentNode = path[index];
        }
        catch
        {
            currentNode = this.gameObject;
        }
    }

    void OnTriggerEnter (Collider other)
    {
        if (other.tag == "Node")
        {
            NewNode();
        }
    }
}
