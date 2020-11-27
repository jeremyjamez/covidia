using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    private static int boardWidth = 48;
    private static int boardHeight = 36;
    public GameObject[,] board = new GameObject[boardWidth, boardHeight];

    // Start is called before the first frame update
    void Start()
    {
        Object[] objects = GameObject.FindObjectsOfType(typeof(GameObject));

        foreach(GameObject obj in objects){
            Vector2 position = obj.transform.position;

            if(obj.name != "PacMan" && obj.name != "Nodes" && obj.name != "NonNodes" && obj.name != "Maze" && obj.name != "Pills" && obj.name != "Medical"){
                board[(int)position.x, (int)position.y] = obj;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
