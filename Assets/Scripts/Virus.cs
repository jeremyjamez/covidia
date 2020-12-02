using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus : MonoBehaviour
{
    public float moveSpeed = 3.9f;

    public int virus2ReleaseTimer = 5;
    public int virus3ReleaseTimer = 14;
    public int virus4ReleaseTimer = 21;

    public float virusTimer = 0;

    public bool isInBase = false;

    public Node startingPosition;

    public enum VirusType
    {
        Virus1,
        Virus2,
        Virus3,
        Virus4
    }

    public VirusType virusType = VirusType.Virus1;

    private GameObject pacMan;

    private Node currentNode, targetNode, previousNode;
    private Vector2 direction, nextDirection;

    //use this for initialization
    void Start()
    {

        pacMan = GameObject.FindGameObjectWithTag("PacMan");

        Node node = GetNodeAtPosition(transform.localPosition);

        if (node != null)
        {
            currentNode = node;
            //startingPosition = currentNode;
        }

        if (isInBase)
        {
            direction = Vector2.up;
            targetNode = currentNode.neighbours[0];
        } 
        else
        {
            direction = Vector2.left;
            targetNode = ChooseNextNode();
        }

        previousNode = currentNode;

    }

    //Update called once per frame
    void Update()
    {
        Move();

        ReleaseVirus();

        //CheckCollision();
    }

    void Move()
    {

        if (targetNode != currentNode && targetNode != null && !isInBase)
        {
            if (nextDirection == direction * -1)
            {
                direction *= -1;

                Node tempNode = targetNode;

                targetNode = previousNode;

                previousNode = tempNode;
            }

            if (OverShotTarget())
            {
                currentNode = targetNode;

                transform.localPosition = currentNode.transform.position;

                GameObject otherPortal = GetPortal(currentNode.transform.position);

                if (otherPortal != null)
                {

                    transform.localPosition = otherPortal.transform.position;

                    currentNode = otherPortal.GetComponent<Node>();
                }

                targetNode = ChooseNextNode();

                previousNode = currentNode;

                currentNode = null;
            }
            else
            {
                transform.localPosition += (Vector3)direction * moveSpeed * Time.deltaTime;
            }
        }

    }

    Node ChooseNextNode()
    {

        Vector2 targetTile = Vector2.zero;

        targetTile = GetTargetTile();

        Node moveToNode = null;

        Node[] foundNodes = new Node[4];
        Vector2[] foundNodesDirection = new Vector2[4];

        int nodeCounter = 0;

        for (int i = 0; i < currentNode.neighbours.Length; i++)
        {

            if (currentNode.validDirections[i] != direction * -1)
            {

                foundNodes[nodeCounter] = currentNode.neighbours[i];
                foundNodesDirection[nodeCounter] = currentNode.validDirections[i];
                nodeCounter++;
            }
        }

        if (foundNodes.Length == 1)
        {

            moveToNode = foundNodes[0];
            direction = foundNodesDirection[0];
        }

        if (foundNodes.Length > 1)
        {
            float leastDistance = Mathf.Infinity;

            for (int i = 0; i < foundNodes.Length; i++)
            {

                if (foundNodesDirection[i] != Vector2.zero)
                {

                    float distance = GetDistance(foundNodes[i].transform.position, targetTile);

                    if (distance < leastDistance)
                    {
                        leastDistance = distance;
                        moveToNode = foundNodes[i];
                        direction = foundNodesDirection[i];
                    }
                }
            }
        }
        return moveToNode;
    }

    Vector2 GetVirus1TargetTile()
    {
        Vector2 pacmanPosition = pacMan.transform.localPosition;
        Vector2 targetTile = new Vector2(Mathf.RoundToInt(pacmanPosition.x), Mathf.RoundToInt(pacmanPosition.y));
        return targetTile;
    }

    Vector2 GetVirus2TargetTile()
    {
        Vector2 pacmanPosition = pacMan.transform.localPosition;
        Vector2 pacmanOrientation = pacMan.GetComponent<PacMan>().orientation;

        int pacmanPositionX = Mathf.RoundToInt(pacmanPosition.x);
        int pacmanPositionY = Mathf.RoundToInt(pacmanPosition.y);

        Vector2 pacmanTile = new Vector2(pacmanPositionX, pacmanPositionY);

        Vector2 targetTile = pacmanTile + (4 * pacmanOrientation);

        return targetTile;
    }

    Vector2 GetVirus3TargetTile()
    {
        Vector2 pacmanPosition = pacMan.transform.localPosition;
        Vector2 pacmanOrientation = pacMan.GetComponent<PacMan>().orientation;

        int pacmanPositionX = Mathf.RoundToInt(pacmanPosition.x);
        int pacmanPositionY = Mathf.RoundToInt(pacmanPosition.y);

        Vector2 pacmanTile = new Vector2(pacmanPositionX, pacmanPositionY);

        Vector2 targetTile = pacmanTile + (2 * pacmanOrientation);

        Vector2 tempVirus1Position = GameObject.Find("virus").transform.localPosition;

        int v1PositionX = Mathf.RoundToInt(tempVirus1Position.x);
        int v1PositionY = Mathf.RoundToInt(tempVirus1Position.y);

        tempVirus1Position = new Vector2(v1PositionX, v1PositionY);
        float distance = GetDistance(tempVirus1Position, targetTile);

        distance *= 2;

        targetTile = new Vector2(tempVirus1Position.x + distance, tempVirus1Position.y + distance);

        return targetTile;
    }

    Vector2 GetVirus4TargetTile()
    {
        Vector2 pacmanPosition = pacMan.transform.localPosition;

        float distance = GetDistance(transform.localPosition, pacmanPosition);

        Vector2 targetTile = Vector2.zero;

        if(distance >= 8)
        {
            targetTile = new Vector2(Mathf.RoundToInt(pacmanPosition.x), Mathf.RoundToInt(pacmanPosition.y));
        }
        else if(distance <= 8)
        {
            targetTile = Vector2.zero;
        }

        return targetTile;
    }

    Vector2 GetTargetTile()
    {
        Vector2 targetTile = Vector2.zero;
        if(virusType == VirusType.Virus1)
        {
            targetTile = GetVirus1TargetTile();
        }
        else if(virusType == VirusType.Virus2)
        {
            targetTile = GetVirus2TargetTile();
        }
        else if(virusType == VirusType.Virus3)
        {
            targetTile = GetVirus3TargetTile();
        }
        else if(virusType == VirusType.Virus4)
        {
            targetTile = GetVirus4TargetTile();
        }
        return targetTile;
    }

    void ReleaseVirus2()
    {
        if (virusType == VirusType.Virus2 && isInBase)
            isInBase = false;
    }

    void ReleaseVirus3()
    {
        if (virusType == VirusType.Virus3 && isInBase)
            isInBase = false;
    }

    void ReleaseVirus4()
    {
        if (virusType == VirusType.Virus4 && isInBase)
            isInBase = false;
    }

    void ReleaseVirus()
    {
        virusTimer += Time.deltaTime;

        if (virusTimer > virus2ReleaseTimer)
            ReleaseVirus2();

        if (virusTimer > virus3ReleaseTimer)
            ReleaseVirus3();

        if (virusTimer > virus4ReleaseTimer)
            ReleaseVirus4();
    }

    Node GetNodeAtPosition(Vector2 pos)
    {

        GameObject tile = GameObject.Find("Game").GetComponent<GameBoard>().board[(int)pos.x, (int)pos.y];

        if (tile != null)
        {

            if (tile.GetComponent<Node>() != null)
            {
                return tile.GetComponent<Node>();
            }
        }

        return null;
    }

    GameObject GetPortal(Vector2 pos)
    {
        GameObject tile = GameObject.Find("Game").GetComponent<GameBoard>().board[(int)pos.x, (int)pos.y];

        if (tile != null)
        {

            if (tile.GetComponent<Tile>() != null)
            {
                if (tile.GetComponent<Tile>().isPortal)
                {
                    GameObject otherPortal = tile.GetComponent<Tile>().portalReceiver;
                    return otherPortal;
                }
            }

        }
        return null;
    }

    float GetDistance(Vector2 posA, Vector2 posB)
    {

        float dx = posA.x - posB.x;
        float dy = posA.y - posB.y;

        float distance = Mathf.Sqrt(dx * dx + dy * dy);

        return distance;

    }

    float LengthFromNode(Vector2 targetPosition)
    {
        Vector2 vec = targetPosition - (Vector2)previousNode.transform.position;
        return vec.sqrMagnitude;
    }

    bool OverShotTarget()
    {
        float nodeToTarget = LengthFromNode(targetNode.transform.position);
        float nodeToSelf = LengthFromNode(transform.position);
        return nodeToSelf > nodeToTarget;
    }

    public void CheckCollision()
    {
        Rect virusRect = new Rect(transform.position, transform.GetComponent<SpriteRenderer>().sprite.bounds.size / 4);
        Rect pacmanRect = new Rect(pacMan.transform.position, pacMan.GetComponent<SpriteRenderer>().sprite.bounds.size / 4);

        if (virusRect.Overlaps(pacmanRect))
        {
            if(!pacMan.GetComponent<PacMan>().atePill)
                pacMan.GetComponent<PacMan>().health--;
        }
    }
}
