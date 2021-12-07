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
    
    [Header("References")]
    public GameObject camHolder;
    public GameObject camLookAt;
    public GameObject player;
    
    private bool isDead;
    private bool isRespawning;
    private PlayerController _controller;

    private void Awake()
    {
        postProcessing = GameObject.Find("PPV").GetComponent<Volume>();
        
        playerCam = GameObject.Find("CMPlayer").GetComponent<CinemachineVirtualCamera>();
        deathCam = GameObject.Find("CMDeath").GetComponent<CinemachineVirtualCamera>();
        CinemachineVirtualCamera startCamera = GameObject.Find("CMStart").GetComponent<CinemachineVirtualCamera>();
        
        playerCanvas = GameObject.Find("GameHUD").GetComponent<CanvasGroup>();
        deathCavnas = GameObject.Find("DeathHUD").GetComponent<CanvasGroup>();

        _controller = GetComponent<PlayerController>();
        playerCam.m_Follow = camHolder.transform;
        playerCam.m_LookAt = camLookAt.transform;
        startCamera.m_LookAt = player.transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
        deathCam.m_Follow = camHolder.transform;
        deathCam.m_LookAt = player.transform;
        
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

   public void CompleteAfterRespawn()
   {
       UIManager.Utilities.FadeInCanvas(playerCanvas, 0.25f, null);
       UIManager.Instance.EnableRespawnButton();
       UIManager.Instance.SetCursor(false);
       if (CameraManager.IsActiveCamera(deathCam))
       {
           CameraManager.SwitchCamera(playerCam);
       }
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
            
            UIManager.Utilities.FadeOutCanvas(deathCavnas, 0.25f,(CompleteAfterRespawn));
            // 
            // use the Respawn manager for that..
            // start a function in respawn menu to respawn player and result all their stuff.
            // camera switch
            // update hud to transition to player hud // fade
        }
    }
}
