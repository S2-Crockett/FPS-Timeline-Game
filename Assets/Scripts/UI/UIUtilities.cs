using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUtilities : MonoBehaviour
{
    // Start is called before the first frame update
    public void FadeInCanvas(CanvasGroup canvas, float lerpTime, Action function)
    {
        StartCoroutine(FadeCanvasGroup(canvas, canvas.alpha, 1, lerpTime, function));
    }
    
    public void FadeOutCanvas(CanvasGroup canvas, float lerpTime, Action function)
    {
        StartCoroutine(FadeCanvasGroup(canvas, canvas.alpha, 0, lerpTime, function));
    }

    public IEnumerator FadeCanvasGroup(CanvasGroup group, float start, float end, float lerpTime, Action function)
    {
        float timeStartedLerping = Time.time;
        float timeSinceStart = Time.time - timeStartedLerping;
        float percentageComplete = timeSinceStart / lerpTime;
        
        while (true)
        {
            timeSinceStart = Time.time - timeStartedLerping;
            percentageComplete = timeSinceStart / lerpTime;
            float currentValue = Mathf.Lerp(start, end, percentageComplete);

            group.alpha = currentValue;
            if (percentageComplete >= 1) break;
            
            yield return new WaitForEndOfFrame();
        }
        
        function.Invoke();
    }
    
}
