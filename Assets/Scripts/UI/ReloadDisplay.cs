using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadDisplay : MonoBehaviour
{
    [Header("References")] 
    public CanvasGroup canvasGroup;
    public Image buttonImage;
    public Text reloadText;

    [Header("Timings")] 
    public float fadeInTime;
    public float fadeOutTime;
    public float pingPongTime;

    [Header("Colour")] 
    public Color originalColour;
    public Color fadeColour;
    
    private bool isActive;

    private void Start()
    {
        //set original colour at start;
        buttonImage.color = originalColour;
        reloadText.color = originalColour;
    }

    public void FadeIn()
    {
        if (!isActive)
        {
            LeanTween.value(gameObject, 0, 1, fadeInTime)
                .setOnUpdate((value) =>
                {
                    canvasGroup.alpha = value;
                })
                .setOnComplete(PlayBackgroundPingPong);
        }
    }

    private void PlayBackgroundPingPong()
    {
        isActive = true;
        
        LeanTween.value(gameObject, originalColour.g, fadeColour.g, pingPongTime)
            .setOnUpdate((value) =>
            {
                var tempColor = buttonImage.color;
                tempColor.g = value;
                
                buttonImage.color = tempColor;
                reloadText.color = tempColor;
            })
            .setLoopPingPong();
        
        LeanTween.value(gameObject, originalColour.b, fadeColour.b, pingPongTime)
            .setOnUpdate((value) =>
            {
                var tempColor = buttonImage.color;
                tempColor.b = value;
                
                buttonImage.color = tempColor;
                reloadText.color = tempColor;
            })
            .setLoopPingPong();
        
            /*
        LeanTween.color(gameObject, fadeColour, pingPongTime)
            .setOnUpdate((color) =>
            {
                buttonImage.color = color;
                reloadText.color = color;
            }).setLoopPingPong();
            */
    
    }

    public void FadeOut()
    {
        if (isActive)
        {
            LeanTween.value(gameObject, 1, 0, fadeOutTime)
                .setOnUpdate((value) =>
                {
                    canvasGroup.alpha = value;
                })
                .setOnComplete(ResetColours);

            isActive = false;
        }
    }

    public void ResetColours()
    {
        buttonImage.color = originalColour;
        reloadText.color = originalColour;
    }
}
