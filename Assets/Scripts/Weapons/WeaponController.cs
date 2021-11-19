using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private PlayerController playerController;
    
    [Header("Settings")] 
    public WeaponSettings settings;

    private bool isInitialised;
    private Vector3 newWeaponRotation;
    private Vector3 newWeaponRotationVelocity;
    private Vector3 targetWeaponRotation;
    private Vector3 targetWeaponRotationVelocity;

    private Vector3 newWeaponMovementRotation;
    private Vector3 newWeaponMovementRotationVelocity;
    private Vector3 targetWeaponMovementRotation;
    private Vector3 targetWeaponMovementRotationVelocity;
    private void Awake()
    {
        
    }

    private void Start()
    {
        newWeaponRotation = transform.localRotation.eulerAngles;
    }

    public void Initialise(PlayerController controller)
    {
        playerController = controller;
        isInitialised = true;
    }

    private void Update()
    {
        if (!isInitialised)
        {
            return;
        }
        
        targetWeaponRotation.y += settings.swayAmount * (settings.swayXInverted ? -playerController.inputView.x : playerController.inputView.x) * Time.deltaTime;
        targetWeaponRotation.x += settings.swayAmount * (settings.swayYInverted ? playerController.inputView.y : -playerController.inputView.y) * Time.deltaTime;
        
        targetWeaponRotation.y = Mathf.Clamp( targetWeaponRotation.y, -settings.swayClampY, settings.swayClampY);
        targetWeaponRotation.x = Mathf.Clamp( targetWeaponRotation.x, -settings.swayClampX, settings.swayClampX);
        targetWeaponRotation.z = targetWeaponRotation.y * 2;
        
        targetWeaponRotation = Vector3.SmoothDamp(targetWeaponRotation, Vector3.zero, ref targetWeaponRotationVelocity,
            settings.swayResetSmoothing);
        newWeaponRotation = Vector3.SmoothDamp(newWeaponRotation, targetWeaponRotation, ref newWeaponRotationVelocity,
            settings.swaySmoothing);

        targetWeaponMovementRotation.z = settings.movementSwayZ * (settings.movementSwayZInverted ? -playerController.inputMovement.x : playerController.inputMovement.x);
        targetWeaponMovementRotation.x = settings.movementSwayY * (settings.movementSwayYInverted ? -playerController.inputMovement.y : playerController.inputMovement.y);
        
        targetWeaponMovementRotation = Vector3.SmoothDamp(targetWeaponMovementRotation, Vector3.zero, ref targetWeaponMovementRotationVelocity,
            settings.movementSwaySmoothing);
        newWeaponMovementRotation = Vector3.SmoothDamp(newWeaponMovementRotation, targetWeaponMovementRotation, ref newWeaponMovementRotationVelocity,
            settings.movementSwaySmoothing);
        
        transform.localRotation = Quaternion.Euler(newWeaponRotation + newWeaponMovementRotation);
    }
}

[Serializable]
public class WeaponSettings
{
    [Header("Weapon Sway")] public float swayAmount;
    public bool swayYInverted;
    public bool swayXInverted;
    public float swaySmoothing;
    public float swayResetSmoothing;
    public float swayClampX;
    public float swayClampY;

    [Header("Weapon Movement Sway")] 
    public float movementSwayZ;
    public float movementSwayY;
    public bool movementSwayZInverted;
    public bool movementSwayYInverted;
    public float movementSwaySmoothing;

}
