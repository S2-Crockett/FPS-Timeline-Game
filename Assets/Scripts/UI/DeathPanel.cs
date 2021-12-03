using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathPanel : MonoBehaviour
{
    public GameObject button;
    public CanvasGroup respawnPanel;
    public Text timerText;
    public Text respawnText;
    public Image progressBar;
    
    private float timeToSpawn;
    private float timer;

    // Start is called before the first frame update
    
    public void EnableRespawnButton(bool result)
    {
        button.SetActive(result);
    }
    
    public void StartRespawn()
    {
        timeToSpawn = RespawnManager.Instance.GetTimeToSpawn();
        timerText.text = timeToSpawn.ToString("#");
        FadeInPanel();
        StartProgressBar();
        StartCoroutine(StartCountDown(timeToSpawn));
    }

    public void TimerComplete()
    {
        RespawnManager.Instance.Respawn();
        respawnPanel.alpha = 0;
        progressBar.fillAmount = 1;
    }

    private void FadeInPanel()
    {
        LeanTween.value(gameObject, 0, 1, 0.1f)
            .setOnUpdate((value) =>
            {
                respawnPanel.alpha = value;
            });
    }

    private void StartProgressBar()
    {
        LeanTween.value(gameObject, 1, 0, timeToSpawn)
            .setOnUpdate((value) =>
            {
                progressBar.fillAmount = value;
            });
    }

    private IEnumerator StartCountDown(float time)
    {
        timer = time;
        while (timer > 0)
        {
            yield return new WaitForSeconds(1.0f);
            timer--;
            timerText.text = timer.ToString("#");
        }
        TimerComplete();
    }
}
