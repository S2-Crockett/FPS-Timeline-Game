using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpTexts1 : MonoBehaviour
{
    public DoubleJump dj;
    public SpeedBooster sb;


    public Text djText;
    public Text sbText;
    public Text djTextActive;
    public Text sbTextActive;

    private void Awake()
    {
        djText.enabled = false;
        sbText.enabled = false;
        djTextActive.enabled = false;
        sbTextActive.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if(dj.doubleJumpText)
        {
            djText.enabled = true;
        }
        else
        {
            djText.enabled = false;
        }


        if(sb.speedBoosterText)
        {
            sbText.enabled = true;
        }
        else
        {
            sbText.enabled = false;
        }


        if(dj.doubleJumpActiveText)
        {
            djTextActive.enabled = true;
        }
        else
        {
            djTextActive.enabled = false;
        }


        if (sb.speedBoosterActiveText)
        {
            sbTextActive.enabled = true;
        }
        else
        {
            sbTextActive.enabled = false;
        }
    }
}
