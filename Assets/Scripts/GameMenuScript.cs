using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenuScript : MonoBehaviour
{
    public bool isStart = true;

    Button startButton;

    void Start()
    {
        startButton = GameObject.Find("StartButton").GetComponent<Button>();
        startButton.onClick.AddListener(StartGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }
}
