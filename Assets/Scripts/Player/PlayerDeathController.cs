using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Cinemachine;
using Managers;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerDeathController : MonoBehaviour
{
    public float test;
    [Header("Post Processing")] 
    public Volume postProcessing;
    public float defaultSaturation = 0;
    public float minSaturation = -1;
    public float timeToChange = 1;

    [Header("Cameras")] 
    public CinemachineVirtualCamera playerCam;
    public CinemachineVirtualCamera deathCam;

    [Header("HUD Swap")]
    public CanvasGroup playerCanvas;
    public CanvasGroup deathCavnas;
    
    
    private bool isDead;
    private bool isRespawning;
    private PlayerController _controller;
    
    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<PlayerController>();
        
        //register both of our cameras with the register
        CameraManager.Register(playerCam);
        CameraManager.Register(deathCam);
        
        CameraManager.SwitchCamera(playerCam);
    }

    private void OnDestroy()
    {
        CameraManager.Unregister(playerCam);
        CameraManager.Unregister(deathCam);
    }

    private void Update()
    {
        if (isDead)
        {
           
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
            if (CameraManager.IsActiveCamera(playerCam))
            {
                CameraManager.SwitchCamera(deathCam);
            }
            
            if (postProcessing.profile.TryGet<ColorAdjustments>(out var saturation))
            {
                LeanTween.value(gameObject, defaultSaturation, minSaturation, timeToChange)
                    .setOnUpdate((value) =>
                    {
                        saturation.saturation.value = value;
                    });
            }
            
            UIManager.Utilities.FadeOutCanvas(playerCanvas, 0.25f,(CompleteAfterDeath));
        }
    }

   public void CompleteAfterDeath()
    {
        UIManager.Utilities.FadeInCanvas(deathCavnas, 0.25f, null);
        UIManager.Instance.SetCursor(true);
    }

    //this might need to be changed..? // added to respawn manager?
    public void SetRespawning(bool results)
    {
        isRespawning = results;
        if (isRespawning)
        {
            if (postProcessing.profile.TryGet<ColorAdjustments>(out var saturation))
            {
                LeanTween.value(gameObject, minSaturation, defaultSaturation, timeToChange)
                    .setOnUpdate((value) =>
                    {
                        saturation.saturation.value = value;
                    });
            }
            if (CameraManager.IsActiveCamera(deathCam))
            {
                CameraManager.SwitchCamera(playerCam);
            }
            // 
            // use the Respawn manager for that..
            // start a function in respawn menu to respawn player and result all their stuff.
            // camera switch
            // update hud to transition to player hud // fade
        }
    }
}
