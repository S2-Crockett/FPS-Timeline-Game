using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private AudioClip footsteps;

    private CharacterController characterController;
    private DefaultInput defaultInput;
    
    [HideInInspector]
    public Vector2 inputMovement;
    public Vector2 inputView;
    
    private Vector3 jumpingForce;
    private Vector3 jumpingForceVelocity;

    private Vector3 newCameraRotation;
    private Vector3 newPlayerRotation;

    [Header("Weapon")] 
    public WeaponController currentWeapon;

    [Header("References")] 
    public Transform cameraHolder;
    public Transform feetTransform;

    [Header("Gravity")] 
    public float gravityAmount;
    public float gravityMin;
    private float playerGravity;
    
    [Header("Settings")]
    public PlayerSettings playerSettings;
    public LayerMask playerMask;

    [Header("Movement")] 
    public float movementSmoothing;

    private Vector3 newMovementSpeed;
    private Vector3 newMovementSpeedVelocity;

    [Header("Stance")]
    public PlayerStance playerStance;
    public float playerStanceSmoothing;
    public StanceSettings standingSettings;
    public StanceSettings sprintingSettings;
    public StanceSettings crouchingSettings;
    
    private float stanceCheckMargin = 0.05f;
    private float cameraHeight;
    private float cameraHeightVelocity;
    private bool isSprinting;

    public bool onDirt = false;
    public bool onWater = false;
    public bool onConcrete = false;
    
    private void Awake()
    {
        Cursor.visible = false;
        defaultInput = new DefaultInput();

        defaultInput.Player.Movement.performed += e => inputMovement = e.ReadValue<Vector2>();
        defaultInput.Player.View.performed += e => inputView = e.ReadValue<Vector2>();
        defaultInput.Player.Jump.performed += e => Jump();
        defaultInput.Player.Crouch.performed += e => Crouch();

        defaultInput.Player.Sprint.started += e => StartSprint();
        defaultInput.Player.Sprint.canceled += e => StopSprint();

        defaultInput.Enable();

        newCameraRotation = cameraHolder.localRotation.eulerAngles;
        newPlayerRotation = transform.localRotation.eulerAngles;

        characterController = GetComponent<CharacterController>();
        cameraHeight = cameraHolder.localPosition.y;
        
        if (currentWeapon)
        {
            currentWeapon.Initialise(this);
        }
    }

    private void Update()
    {
        CalculateView();
        CalculateMovement();
        CalculateJump();
        CalculateCameraHeight();
    }

    private void CalculateView()
    {
        newPlayerRotation.y += playerSettings.viewXSensitivity * (playerSettings.viewXInverted ? inputView.x : -inputView.x) * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(newPlayerRotation);
        
        newCameraRotation.x += playerSettings.viewYSensitivity * (playerSettings.viewYInverted ? inputView.y : -inputView.y) * Time.deltaTime;
        newCameraRotation.x = Mathf.Clamp(newCameraRotation.x, playerSettings.viewClampYMin, playerSettings.viewClampYMax);
        cameraHolder.localRotation = Quaternion.Euler(newCameraRotation);
    }

    private void CalculateMovement()
    {
        if (inputMovement.y <= 0.2f)
        {
            isSprinting = false;
        }

        var verticalSpeed = GetStanceSettings().stanceForwardMovementSpeed;
        var horizontalSpeed = GetStanceSettings().stanceStrafeMovementSpeed;

        var vertSpeed = verticalSpeed * inputMovement.y * Time.deltaTime;
        var horSpeed = horizontalSpeed * inputMovement.x * Time.deltaTime;

        newMovementSpeed = Vector3.SmoothDamp(newMovementSpeed, new Vector3(horSpeed, 0, vertSpeed),
            ref newMovementSpeedVelocity, characterController.isGrounded ? movementSmoothing : playerSettings.fallingSmoothing);
        var movementSpeed = transform.TransformDirection(newMovementSpeed);

        if (playerGravity > gravityMin)
        {
            playerGravity -= gravityAmount * Time.deltaTime;
        }

        if (playerGravity < -0.1f && characterController.isGrounded)
        {
            playerGravity = -0.1f;
        }

        movementSpeed.y += playerGravity;
        movementSpeed += jumpingForce * Time.deltaTime;

        characterController.Move(movementSpeed);

        if( movementSpeed.x > 0.01f ||
            movementSpeed.x < -0.01f || 
            movementSpeed.z > 0.01f || 
            movementSpeed.z < -0.01f)
        {
            if(onDirt)
            {
                SoundManager.Instance.PlaySound(footsteps);
            }
            if(onWater)
            {
                //insert water steps here
            }
            if(onConcrete)
            {
                //insert concrete steps here
            }
            
        }
        

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "FloorDirt")
        {
            onDirt = true;
            onWater = false;
            onConcrete = false;
        }
        if (other.gameObject.tag == "FloorWater")
        {
            onDirt = false;
            onWater = true;
            onConcrete = false;
        }
        if (other.gameObject.tag == "FloorConcrete")
        {
            onDirt = false;
            onWater = false;
            onConcrete = true;
        }
    }
    
    private void CalculateJump()
    {
        jumpingForce = Vector3.SmoothDamp(jumpingForce, Vector3.zero, ref jumpingForceVelocity,
            playerSettings.jumpFalloff);
    }

    private void CalculateCameraHeight()
    {
        float stanceHeight = standingSettings.cameraHeight;
        float capsuleHeight = standingSettings.capsuleHeight;
        
        if (playerStance == PlayerStance.PSCrouching)
        {
            stanceHeight = GetStanceSettings().cameraHeight;
            capsuleHeight = GetStanceSettings().capsuleHeight;
        }

        if (playerStance == PlayerStance.PSSprinting)
        {
            stanceHeight = GetStanceSettings().cameraHeight;
            capsuleHeight = GetStanceSettings().capsuleHeight;
        }
        
        cameraHeight = Mathf.SmoothDamp(cameraHolder.localPosition.y, stanceHeight,
            ref cameraHeightVelocity, playerStanceSmoothing);
        
        cameraHolder.localPosition = new Vector3(cameraHolder.localPosition.x,cameraHeight,cameraHolder.localPosition.z);
        characterController.height = capsuleHeight;
    }

    private void Jump()
    {
        if (!characterController.isGrounded)
        {
            return;
        }

        if (playerStance != PlayerStance.PSStanding)
        {
            if (!StanceCheck(standingSettings.capsuleHeight))
            {
                playerStance = PlayerStance.PSStanding;
                return;
            }
        }
        
        jumpingForce = Vector3.up * playerSettings.jumpHeight;
        playerGravity = 0;
    }

    private void Crouch()
    {
        Debug.Log("c pressed");
        switch (playerStance)
        {
            case PlayerStance.PSCrouching:
                if (StanceCheck(standingSettings.capsuleHeight))
                {
                    return;
                }
                playerStance = PlayerStance.PSStanding;
                break;
            case PlayerStance.PSStanding:
                playerStance = PlayerStance.PSCrouching;
                characterController.center = new Vector3(0, 0, 0);
                break;
            case PlayerStance.PSSprinting:
                
                // could go into a slide here?
                playerStance = PlayerStance.PSCrouching;
                break;
        }
    }

    private void StartSprint()
    {
        // checks the player is actually moving before sprinting
        if (inputMovement.y <= 0.2f)
        {
            isSprinting = false;
            return;
        }
        
        isSprinting = true;
        playerStance = PlayerStance.PSSprinting;
    }
    

    private void StopSprint()
    {
        //stops players sprinting again after sprint crouching.. (sliding)
        if (playerStance != PlayerStance.PSCrouching)
        {
             isSprinting = false;
             playerStance = PlayerStance.PSStanding;
        }
    }

    private bool StanceCheck(float stanceCheckHeight)
    {
        Vector3 startPos = new Vector3(feetTransform.position.x, feetTransform.position.y + characterController.radius + stanceCheckMargin, feetTransform.position.z);
        Vector3 endPos = new Vector3(feetTransform.position.x, feetTransform.position.y - characterController.radius - stanceCheckMargin + stanceCheckHeight, feetTransform.position.z);
        
        return Physics.CheckCapsule(startPos, endPos, characterController.radius, playerMask);
    }

    StanceSettings GetStanceSettings()
    {
        switch (playerStance)
        {
            case PlayerStance.PSCrouching:
                return crouchingSettings;
            case PlayerStance.PSSprinting:
                return sprintingSettings;
            case PlayerStance.PSStanding:
                return standingSettings;
        }
        return null;
    }
}

public enum PlayerStance
{
    PSStanding,
    PSCrouching,
    PSSprinting,
}

[Serializable]
public class StanceSettings
{
    public float cameraHeight;
    public float capsuleHeight;
    public float stanceForwardMovementSpeed;
    public float stanceStrafeMovementSpeed;
}

[Serializable]
public class PlayerSettings
{
    [Header("View Settings")] 
    public float viewXSensitivity;
    public float viewYSensitivity;

    public bool viewXInverted;
    public bool viewYInverted;
    
    public float viewClampYMin = -70;
    public float viewClampYMax = 80;

    [Header("Jumping")] 
    public float jumpHeight;
    public float jumpFalloff;
    public float fallingSmoothing;
}
