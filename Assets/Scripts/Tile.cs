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

    bool increasedHealth = false;

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

            if (!increasedHealth)
            {
                pacMan.health += 5;
                pacMan.atePill = true;
                increasedHealth = true;
            }
            
            if (pillTimeLeft < 0)
            {
                pillTimerText.text = "0";
                didConsume = false;
                pacMan.atePill = false;
                increasedHealth = false;
            }
        }

        if(didConsume && isMask)
        {
            maskTimeLeft -= Time.deltaTime;
            Text maskTimerText = GameObject.Find("MaskTime").GetComponent<Text>();
            maskTimerText.text = string.Format("{0}", (int)maskTimeLeft);

            if (maskTimeLeft < 0)
            {
                maskTimerText.text = "0";
                didConsume = false;
            }
        }

        if (didConsume && isSanitizer)
        {
            sanitizerTimeLeft -= Time.deltaTime;
            Text sanitizerTimerText = GameObject.Find("SanitizerTime").GetComponent<Text>();
            sanitizerTimerText.text = string.Format("{0}", (int)sanitizerTimeLeft);

            if (sanitizerTimeLeft < 0)
            {
                sanitizerTimerText.text = "0";
                didConsume = false;
            }
        }
    }
}
