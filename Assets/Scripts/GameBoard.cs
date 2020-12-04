using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameBoard : MonoBehaviour
{
    private static int boardWidth = 48;
    private static int boardHeight = 36;
    public GameObject[,] board = new GameObject[boardWidth, boardHeight];

    public int pacManLives = 3;
    public int score = 0;

    public Text playerText, readyText;

    private bool didStartDeath = false;

    // Start is called before the first frame update
    void Start()
    {
        Object[] objects = GameObject.FindObjectsOfType(typeof(GameObject));

        foreach(GameObject obj in objects){
            Vector2 position = obj.transform.position;

            if(obj.name != "PacMan" && obj.name != "Nodes" && obj.name != "Maze" && obj.tag != "Virus" && obj.name != "Canvas" && obj.name != "PlayerText" && obj.name != "ReadyText"){
                board[(int)position.x, (int)position.y] = obj;
            }
        }
        GameObject.Find("Score").GetComponent<Text>().text = string.Format("{0}", score);
        
        StartGame();
    }

    IEnumerator IncrementScore()
    {
        while (true)
        {
            if (!didStartDeath)
            {
                score++;
                GameObject.Find("Score").GetComponent<Text>().text = string.Format("{0}", score);
                yield return new WaitForSeconds(1);
            }
            else
            {
                break;
            }
        }
    }

    public void StartGame()
    {
        GameObject[] o = GameObject.FindGameObjectsWithTag("Virus");

        foreach (GameObject virus in o)
        {
            virus.transform.GetComponent<SpriteRenderer>().enabled = false;
            virus.transform.GetComponent<Virus>().canMove = false;
        }

        GameObject pacMan = GameObject.Find("PacMan");
        pacMan.transform.GetComponent<SpriteRenderer>().enabled = false;
        pacMan.transform.GetComponent<PacMan>().canMove = false;

        StartCoroutine(ShowObjectsAfter(2));
    }

    IEnumerator ShowObjectsAfter(float delay)
    {
        yield return new WaitForSeconds(delay);

        GameObject[] o = GameObject.FindGameObjectsWithTag("Virus");

        foreach (GameObject virus in o)
        {
            virus.transform.GetComponent<SpriteRenderer>().enabled = true;
        }

        GameObject pacMan = GameObject.Find("PacMan");
        pacMan.transform.GetComponent<SpriteRenderer>().enabled = true;

        playerText.transform.GetComponent<Text>().enabled = false;

        StartCoroutine(StartGameAfter(2));
    }

    IEnumerator StartGameAfter(float delay)
    {
        yield return new WaitForSeconds(delay);

        GameObject[] o = GameObject.FindGameObjectsWithTag("Virus");

        foreach (GameObject virus in o)
        {
            virus.transform.GetComponent<Virus>().canMove = true;
        }

        GameObject pacMan = GameObject.Find("PacMan");
        pacMan.transform.GetComponent<PacMan>().canMove = true;

        readyText.transform.GetComponent<Text>().enabled = false;

        StartCoroutine(IncrementScore());
    }

    public void StartDeath()
    {
        if (!didStartDeath)
        {
            didStartDeath = true;

            GameObject[] o = GameObject.FindGameObjectsWithTag("Virus");

            foreach (GameObject virus in o)
            {
                virus.transform.GetComponent<Virus>().canMove = false;
            }

            GameObject pacMan = GameObject.Find("PacMan");
            pacMan.transform.GetComponent<PacMan>().canMove = false;

            pacMan.transform.GetComponent<Animator>().enabled = false;

            StartCoroutine(ProcessDeathAfter(1f));
        }
    }

    IEnumerator ProcessDeathAfter(float delay)
    {
        yield return new WaitForSeconds(delay);

        GameObject[] o = GameObject.FindGameObjectsWithTag("Virus");

        foreach (GameObject virus in o)
        {
            virus.transform.GetComponent<SpriteRenderer>().enabled = false;
        }

        yield return new WaitForSeconds(delay);

        StartCoroutine(ProcessRestart(1));
    }

    IEnumerator ProcessRestart(float delay)
    {
        pacManLives -= 1;

        if(pacManLives == 0)
        {
            playerText.transform.GetComponent<Text>().enabled = true;

            readyText.transform.GetComponent<Text>().text = "GAME OVER";
            readyText.transform.GetComponent<Text>().color = Color.red;
            readyText.transform.GetComponent<Text>().enabled = true;

            GameObject pacMan = GameObject.Find("PacMan");
            pacMan.transform.GetComponent<SpriteRenderer>().enabled = false;

            StartCoroutine(ProcessGameOver(2));
        }
        else
        {
            playerText.transform.GetComponent<Text>().enabled = true;
            readyText.transform.GetComponent<Text>().enabled = true;

            GameObject pacMan = GameObject.Find("PacMan");
            pacMan.transform.GetComponent<SpriteRenderer>().enabled = false;

            yield return new WaitForSeconds(delay);

            StartCoroutine(ProcessRestartShowObjects(1));
        }
    }

    IEnumerator ProcessGameOver(float delay)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene("GameMenu");
    }

    IEnumerator ProcessRestartShowObjects(float delay)
    {
        playerText.transform.GetComponent<Text>().enabled = false;

        GameObject[] o = GameObject.FindGameObjectsWithTag("Virus");

        foreach (GameObject virus in o)
        {
            virus.transform.GetComponent<SpriteRenderer>().enabled = true;
            virus.transform.GetComponent<Virus>().MoveToStartingPosition();
        }

        GameObject pacMan = GameObject.Find("PacMan");
        pacMan.transform.GetComponent<SpriteRenderer>().enabled = true;
        pacMan.transform.GetComponent<PacMan>().MoveToStartingPosition();

        yield return new WaitForSeconds(delay);

        Restart();
    }

    public void Restart()
    {
        readyText.transform.GetComponent<Text>().enabled = false;
        
        GameObject pacMan = GameObject.Find("PacMan");
        pacMan.transform.GetComponent<PacMan>().Restart();

        GameObject[] o = GameObject.FindGameObjectsWithTag("Virus");

        foreach(GameObject virus in o)
        {
            virus.transform.GetComponent<Virus>().Restart();
        }

        didStartDeath = false;
        StartCoroutine(IncrementScore());
    }


}
