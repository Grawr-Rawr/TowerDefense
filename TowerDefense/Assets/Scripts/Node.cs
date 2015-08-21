using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour 
{
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, .5f);
    }
}
