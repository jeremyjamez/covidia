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

    float pillTimeLeft = 60.0f;
    float maskTimeLeft = 45.0f;
    float sanitizerTimeLeft = 30.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CountDownPillTimer();
    }

    public void CountDownPillTimer()
    {
        if(didConsume && isPill)
        {
            pillTimeLeft -= Time.deltaTime;
            Text pillTimerText = GameObject.Find("PillTime").GetComponent<Text>();
            pillTimerText.text = string.Format("{0}", (int)pillTimeLeft);

            if (pillTimeLeft < 0)
            {
                pillTimerText.text = "0";
                didConsume = false;
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
    }
}
