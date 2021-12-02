using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Managers;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerDeathController : MonoBehaviour
{
    [Header("Post Processing")] 
    public Volume postProcessing;
    public float defaultSaturation = 1;
    public float minSaturation;

    [Header("Cameras")] 
    public CinemachineVirtualCamera playerCam;
    public CinemachineVirtualCamera deathCam;

    [Header("HUD Swap")] 
    public GameObject playerHUD;
    public GameObject deathHUD;
    public CanvasGroup playerCanvas;
    private CanvasGroup deathCavnas;
    
    
    private bool isDead;
    private bool isRespawning;
    private PlayerController _controller;
    
    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<PlayerController>();
        // feed information to the camera switcher
    }

    private void Update()
    {
        if (isDead)
        {
            //lerp post processing
        }
        else if(isRespawning && !isDead)
        {
            //if we aren't  dead and respawning lerp back to the 
        }
    }

    public void SetIsDead(bool results)
    {
        isDead = results;
        _controller.isDead = this.isDead;
        
        if (isDead)
        {
            // camera switch
            if (postProcessing.profile.TryGet<ColorAdjustments>(out var saturation))
            {
                saturation.saturation.value = -100;
            }
            UIManager.Utilities.FadeOutCanvas(playerCanvas, 0.25f,(CompleteAfterDeath));
        }
    }

   public void CompleteAfterDeath()
    {
        // set the canvas group to disabled? 
    }

    public void SetRespawning(bool results)
    {
        isRespawning = results;
        if (isRespawning)
        {
            // start a function in respawn menu to respawn player and result all their stuff.
            // camera switch
            // update hud to transition to player hud // fade
        }
    }
}
