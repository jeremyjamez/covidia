using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public bool isPortal;

    public bool isPill;

    public bool isMask;

    public bool isSanitizer;

    public bool didConsume;

    public GameObject portalReceiver;
    PacMan pacMan;

    bool increasedLives = false;
    bool immune = false;

    float pillTimeLeft = 60.0f;
    float maskTimeLeft = 45.0f;
    float sanitizerTimeLeft = 30.0f;

    // Start is called before the first frame update
    void Start()
    {
        pacMan = GameObject.Find("PacMan").GetComponent<PacMan>();
    }

    // Update is called once per frame
    void Update()
    {
        CountDownTimer();
    }

    public void CountDownTimer()
    {
        if(didConsume && isPill)
        {
            pillTimeLeft -= Time.deltaTime;
            Text pillTimerText = GameObject.Find("PillTime").GetComponent<Text>();
            pillTimerText.text = string.Format("{0}", (int)pillTimeLeft);

            if (!increasedLives)
            {
                GameObject.Find("Game").transform.GetComponent<GameBoard>().pacManLives += 1;
                pacMan.isImmune = true;
                increasedLives = true;
            }
            
            if (pillTimeLeft < 0)
            {
                pillTimerText.text = "0";
                didConsume = false;
                pacMan.isImmune = false;
                increasedLives = false;
            }
        }

        if(didConsume && isMask)
        {
            maskTimeLeft -= Time.deltaTime;
            Text maskTimerText = GameObject.Find("MaskTime").GetComponent<Text>();
            maskTimerText.text = string.Format("{0}", (int)maskTimeLeft);
            //GameObject.Find("Game").transform.GetComponent<GameBoard>().score += 10;
            if (!immune)
            {
                pacMan.isImmune = true;
                immune = true;
            }

            if (maskTimeLeft < 0)
            {
                maskTimerText.text = "0";
                didConsume = false;
                pacMan.isImmune = false;
                immune = false;
            }
        }

        if (didConsume && isSanitizer)
        {
            sanitizerTimeLeft -= Time.deltaTime;
            Text sanitizerTimerText = GameObject.Find("SanitizerTime").GetComponent<Text>();
            sanitizerTimerText.text = string.Format("{0}", (int)sanitizerTimeLeft);
            //GameObject.Find("Game").transform.GetComponent<GameBoard>().score += 10;

            if (!immune)
            {
                pacMan.isImmune = true;
                immune = true;
            }

            if (sanitizerTimeLeft < 0)
            {
                sanitizerTimerText.text = "0";
                didConsume = false;
                pacMan.isImmune = false;
                immune = false;
            }
        }
    }
}
