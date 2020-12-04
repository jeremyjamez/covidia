﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PacMan : MonoBehaviour
{
    public Vector2 orientation;

    public float speed = 4.0f;

    public int health = 10;

    public bool atePill = false;

    public bool wearingMask = false;

    public bool hasSanitized = false;

    public Sprite idleSprite;

    public bool isImmune = false;
    public bool canMove = true;

    private Vector2 direction = Vector2.zero;
    private Vector2 nextDirection;

    private Node currentNode, previousNode, targetNode;
    public Node startingPosition;
    Text livesText;

    // Start is called before the first frame update
    void Start()
    {
        livesText = GameObject.Find("Lives").GetComponent<Text>();
        livesText.text = string.Format("{0}", GameObject.Find("Game").transform.GetComponent<GameBoard>().pacManLives);
        Node node = GetNodeAtPosition(transform.localPosition);
        if(node != null){
            currentNode = node;
        }
        direction = Vector2.left;
        orientation = Vector2.left;
        ChangePosition(direction);
    }

    public void MoveToStartingPosition()
    {
        transform.position = startingPosition.transform.position;

        direction = Vector2.left;
        orientation = Vector2.left;

        UpdateOrientation();
    }

    public void Restart()
    {
        canMove = true;
        currentNode = startingPosition;
        nextDirection = Vector2.left;
        transform.GetComponent<Animator>().enabled = true;

        ChangePosition(direction);
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            CheckInput();
            Move();
            UpdateOrientation();
            UpdateAnimationState();
            ConsumePill();

            livesText.text = string.Format("{0}", GameObject.Find("Game").transform.GetComponent<GameBoard>().pacManLives);
        }
    }

    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangePosition(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangePosition(Vector2.right);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangePosition(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangePosition(Vector2.down);
        }
    }

    void ChangePosition(Vector2 d){
        if(d != direction)
            nextDirection = d;

        if(currentNode != null){
            Node moveToNode = CanMove(d);
            if(moveToNode != null){
                direction = d;
                targetNode = moveToNode;
                previousNode = currentNode;
                currentNode = null;
            }
        }
    }

    void Move()
    {
        if(targetNode != currentNode && targetNode != null){

            if(nextDirection == direction * -1){
                direction *= -1;

                Node tempNode = targetNode;

                targetNode = previousNode;

                previousNode = tempNode;
            }

            if(OverShotTarget()){

                currentNode = targetNode;
                transform.localPosition = currentNode.transform.position;

                GameObject otherPortal = GetPortal(currentNode.transform.position);

                if(otherPortal != null){
                    transform.localPosition = otherPortal.transform.position;

                    currentNode = otherPortal.GetComponent<Node>();
                }

                Node moveToNode = CanMove(nextDirection);

                if(moveToNode != null)
                    direction = nextDirection;

                if(moveToNode == null)
                    moveToNode = CanMove(direction);

                if(moveToNode != null){
                    targetNode = moveToNode;
                    previousNode = currentNode;
                    currentNode = null;
                } else {
                    direction = Vector2.zero;
                }
            } else {
                transform.localPosition += (Vector3)(direction * speed) * Time.deltaTime;
            }
        }
    }

    void UpdateOrientation()
    {

        if (direction == Vector2.left)
        {
            orientation = Vector2.left;
            transform.localScale = new Vector3(-1, 1, 1);
            transform.localRotation = Quaternion.Euler(0, 0, 0);

        }
        else if (direction == Vector2.right)
        {
            orientation = Vector2.right;
            transform.localScale = new Vector3(1, 1, 1);
            transform.localRotation = Quaternion.Euler(0, 0, 0);

        }
        else if (direction == Vector2.up)
        {
            orientation = Vector2.up;
            transform.localScale = new Vector3(1, 1, 1);
            transform.localRotation = Quaternion.Euler(0, 0, 90);

        }
        else if (direction == Vector2.down)
        {
            orientation = Vector2.down;
            transform.localScale = new Vector3(1, 1, 1);
            transform.localRotation = Quaternion.Euler(0, 0, 270);
        }
    }

    Node GetNodeAtPosition(Vector2 pos)
    {
        GameObject tile = GameObject.Find("Game").GetComponent<GameBoard>().board[(int)pos.x, (int)pos.y];
        if(tile != null){
            return tile.GetComponent<Node>();
        }
        return null;
    }

    void UpdateAnimationState(){
        if(direction == Vector2.zero){
            GetComponent<Animator>().enabled = false;
            GetComponent<SpriteRenderer>().sprite = idleSprite;
        } else {
            GetComponent<Animator>().enabled = true;
        }
    }

    Node CanMove(Vector2 d){
        Node moveToNode = null;
        for(int i = 0; i < currentNode.neighbours.Length; i++){
            if(currentNode.validDirections[i] == d){
                moveToNode = currentNode.neighbours[i];
                break;
            }
        }
        return moveToNode;
    }

    bool OverShotTarget(){
        float nodeToTarget = LengthFromNode(targetNode.transform.position);
        float nodeToSelf = LengthFromNode(transform.localPosition);
        return nodeToSelf > nodeToTarget;
    }

    float LengthFromNode(Vector2 targetPosition){
        Vector2 vec = targetPosition - (Vector2)previousNode.transform.position;
        return vec.sqrMagnitude;
    }

    GameObject GetPortal(Vector2 pos)
    {
        GameObject tile = GameObject.Find("Game").GetComponent<GameBoard>().board[(int)pos.x, (int)pos.y];

        if(tile != null)
        {

            if(tile.GetComponent<Tile>() != null)
            {
                if(tile.GetComponent<Tile>().isPortal)
                {
                    GameObject otherPortal = tile.GetComponent<Tile>().portalReceiver;
                    return otherPortal;
                }
            }
            
        }
        return null;
    }

    void ConsumePill(){
        GameObject obj = GetTileAtPosition(transform.position);

        if(obj != null){
            Tile tile = obj.GetComponent<Tile>();

            if(tile != null){
                if(!tile.didConsume && (tile.isPill || tile.isMask || tile.isSanitizer)){
                    obj.GetComponent<SpriteRenderer>().enabled = false;
                    tile.didConsume = true;
                }
            }
        }
    }

    GameObject GetTileAtPosition(Vector2 pos){
        int tileX = Mathf.RoundToInt(pos.x);
        int tileY = Mathf.RoundToInt(pos.y);

        GameObject tile = GameObject.Find("Game").GetComponent<GameBoard>().board[tileX, tileY];

        if(tile != null){
            return tile;
        }
        return null;
    }
}