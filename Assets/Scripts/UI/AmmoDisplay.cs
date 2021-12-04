using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AmmoDisplay : MonoBehaviour
{
    [Header("References")] 
    public CanvasGroup canvasGroup;
    public Image backgroundImage;

    [Header("Timings")] 
    public float fadeInTime;
    public float fadeOutTime;
    public float pingPongTime;

    private bool isActive;
    
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
        LeanTween.value(gameObject, 1, 0, pingPongTime)
            .setOnUpdate((value) =>
            {
                var tempColor = backgroundImage.color;
                tempColor.a = value;
                backgroundImage.color = tempColor;
            })
            .setLoopPingPong();
    }

    public void FadeOut()
    {
        if (isActive)
        {
            LeanTween.value(gameObject, 1, 0, fadeOutTime)
                .setOnUpdate((value) =>
                {
                    canvasGroup.alpha = value;
                });

            isActive = false;
        }
    }
}
