using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Node[] neighbours;
    public Vector2[] validDirections;

    // Start is called before the first frame update
    void Start()
    {
        /* validDirections = new Vector2[neighbours.Length];

        for (int i = 0; i < neighbours.Length; i++)
        {
            Node neighbour = neighbours[i];
            Vector2 tempVector = neighbour.transform.localPosition - transform.localPosition;
            validDirections[i] = tempVector.normalized;
        } */
    }
}
