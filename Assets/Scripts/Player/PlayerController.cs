using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using Unity.VisualScripting;
using Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerStance previousPlayerStance;
    private CharacterController characterController;
    private consoledefence consoledefencescript;
    private DefaultInput defaultInput;

    private DoubleJump double_jump;
    private SpeedBooster double_speed;


    public int jumpCount = 0;
    public int space_pressed = 0;

    public int speed = 0;

    [HideInInspector]
    public Vector2 inputMovement;
    public Vector2 inputView;

    private Vector3 jumpingForce;
    private Vector3 jumpingForceVelocity;

    private Vector3 newCameraRotation;
    private Vector3 newPlayerRotation;

    [Header("Weapon")] private WeaponHandler weaponHandler;

    [Header("Camera")] 
    public CinemachineVirtualCamera playerCam;
    public float defaultFOV = 65.0f;
    public float sprintFOV = 75.0f;
    public float aimingFOV = 45.0f;
    public float fieldOfViewChangeTime = 4.0f;

    [Header("References")] 
    public Transform cameraHolder;
    public Transform feetTransform;
    public GameObject LookAtTarget;

    [Header("Gravity")] public float gravityAmount;
    public float gravityMin;
    private float playerGravity;

    [Header("Settings")]
    public PlayerSettings playerSettings;
    public LayerMask playerMask;

    private Vector3 newMovementSpeed;
    private Vector3 newMovementSpeedVelocity;

    [Header("Stance")] public PlayerStance playerStance;
    public float playerStanceSmoothing;
    public float movementSmoothing;
    public StanceSettings standingSettings;
    public StanceSettings aimingSettings;
    public StanceSettings sprintingSettings;
    public StanceSettings crouchingSettings;

    private float stanceCheckMargin = 0.05f;
    private float cameraHeight;
    private float cameraHeightVelocity;

    public bool isSprinting;
    public bool isCrouching;


    public bool onDirt = false;
    public bool onWater = false;
    public bool onConcrete = false;
    public bool onWood = false;
    public bool onMetal = false;
    public bool onGrass = false;

    public bool onDirtLand = false;
    public bool onWaterLand = false;
    public bool onConcreteLand = false;
    public bool onWoodLand = false;
    public bool onMetalLand = false;
    public bool onGrassLand = false;

    public float airTime;

    private bool shouldShoot;

    public Camera camera;

    [HideInInspector] public bool isDead = true;

    #region - Awake -

    private void Awake()
    {
        isDead = false;
        defaultInput = new DefaultInput();

        defaultInput.Player.Movement.performed += e => inputMovement = e.ReadValue<Vector2>();
        defaultInput.Player.View.performed += e => inputView = e.ReadValue<Vector2>();
        defaultInput.Player.Jump.performed += e => Jump();
        defaultInput.Player.Crouch.performed += e => Crouch();

        defaultInput.Player.Sprint.started += e => StartSprint();
        defaultInput.Player.Sprint.canceled += e => StopSprint();
        defaultInput.Weapon.Aim.started += e => StartAiming();
        defaultInput.Weapon.Aim.canceled += e => StopAiming();

        defaultInput.Enable();
        
        newCameraRotation = cameraHolder.localRotation.eulerAngles;
        newPlayerRotation = transform.localRotation.eulerAngles;

        characterController = GetComponent<CharacterController>();
        cameraHeight = cameraHolder.localPosition.y;
        UIManager.Instance.SetCursor(false);

        playerCam = GameObject.Find("CMPlayer").GetComponent<CinemachineVirtualCamera>();
        double_jump = GameObject.Find("DoubleJumpPower").GetComponent<DoubleJump>();
        double_speed = GameObject.Find("SpeedBoostPower").GetComponent<SpeedBooster>();
    }

    private void Start()
    {
        camera.fieldOfView = defaultFOV;
        weaponHandler = GetComponent<WeaponHandler>();
        weaponHandler.Initialise(this);
        SoundManager.Instance.pController = this;
    }

    #endregion

    #region - Update -

    private void Update()
    {
        CalculateMovement();
        DoubleJumping();
        DoubleSpeed();

        if (!isDead)
        {
            CalculateView();
            CalculateJump();
            CalculateCameraHeight();
            SetLookAtTargetCrosshair();
            
        }
    }

    #endregion

    private void SetLookAtTargetCrosshair()
    {
        // this will need to be replaced with enemies at some point when they are implemented
        // need to add a max distance that you can do this.
        if (UIManager.Instance)
        {
            RaycastHit hit;
            if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, 100.0f))
            {
                ShootingTarget target = hit.transform.GetComponent<ShootingTarget>();
                if (target != null)
                {
                    UIManager.Instance.crosshair.UpdateLookAtColour(true);
                }
                else
                {
                    UIManager.Instance.crosshair.UpdateLookAtColour(false);
                }
            }
        }
    }

    private void CalculateView()
    {
        newPlayerRotation.y += playerSettings.viewXSensitivity *
                               (playerSettings.viewXInverted ? inputView.x : -inputView.x) * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(newPlayerRotation);

        newCameraRotation.x += playerSettings.viewYSensitivity * (playerSettings.viewYInverted ? inputView.y : -inputView.y) * Time.deltaTime;
        newCameraRotation.x = Mathf.Clamp(newCameraRotation.x, playerSettings.viewClampYMin, playerSettings.viewClampYMax);
        cameraHolder.localRotation = Quaternion.Euler(newCameraRotation);
    }

    private void CalculateMovement()
    {
        Vector3 movementSpeed = new Vector3();

        if (!isDead)
        {
            if (inputMovement.y <= 0.2f)
            {
                isSprinting = false;
            }

            var verticalSpeed = GetStanceSettings().stanceForwardMovementSpeed;
            var horizontalSpeed = GetStanceSettings().stanceStrafeMovementSpeed;

            var vertSpeed = verticalSpeed * inputMovement.y * Time.smoothDeltaTime;
            var horSpeed = horizontalSpeed * inputMovement.x * Time.smoothDeltaTime;

            newMovementSpeed = Vector3.SmoothDamp(newMovementSpeed, new Vector3(horSpeed, 0, vertSpeed),
                ref newMovementSpeedVelocity,
                characterController.isGrounded ? movementSmoothing : playerSettings.fallingSmoothing);
            movementSpeed = transform.TransformDirection(newMovementSpeed);
        }

        if (playerGravity > gravityMin)
        {
            playerGravity -= gravityAmount * Time.deltaTime;
        }

        if (playerGravity < -0.1f && characterController.isGrounded)
        {
            playerGravity = -0.1f;
        }
        
        movementSpeed.y += playerGravity;
        movementSpeed += jumpingForce * Time.smoothDeltaTime;

        movementSpeed *= speed;

        characterController.Move(movementSpeed);

        CheckLand();
        CreateSounds(movementSpeed);

        CheckAirTime();

    }
    private void CreateSounds(Vector3 movementSpeed)
    {
        RaycastHit tag;

        if (Physics.Raycast(cameraHolder.transform.position, cameraHolder.transform.up * -1, out tag, 2))
        {
            if (movementSpeed.x > 0.001f ||
            movementSpeed.x < -0.001f ||
            movementSpeed.z > 0.001f ||
            movementSpeed.z < -0.001f)
            {
                if (tag.transform.gameObject.layer == 7)
                {
                    onDirt = true;
                }
                else
                {
                    onDirt = false;
                }
                if (tag.transform.gameObject.layer == 8)
                {
                    onWater = true;
                }
                else
                {
                    onWater = false;
                }
                if (tag.transform.gameObject.layer == 9)
                {
                    onConcrete = true;
                }
                else
                {
                    onConcrete = false;
                }
                if (tag.transform.gameObject.layer == 10)
                {
                    onWood = true;
                }
                else
                {
                    onWood = false;
                }
                if (tag.transform.gameObject.layer == 11)
                {
                    onMetal = true;
                }
                else
                {
                    onMetal = false;
                }
                if (tag.transform.gameObject.layer == 12)
                {
                    onGrass = true;
                }
                else
                {
                    onGrass = false;
                }
            }
            else
            {
                onDirt = false;
                onWater = false;
                onConcrete = false;
                onWood = false;
                onMetal = false;
                onGrass = false;
            }
        }
    }
    private void CalculateJump()
    {
        jumpingForce = Vector3.SmoothDamp(jumpingForce, Vector3.zero, ref jumpingForceVelocity,
            playerSettings.jumpFalloff);
    }

    private void CalculateCameraHeight()
    {
        float stanceFOV = defaultFOV;
        float stanceHeight = standingSettings.cameraHeight;
        float capsuleHeight = standingSettings.capsuleHeight;

        if (playerStance == PlayerStance.PSCrouching)
        {
            // set the crouch fov hear
            stanceHeight = GetStanceSettings().cameraHeight;
            capsuleHeight = GetStanceSettings().capsuleHeight;
            isCrouching = true;
        }
        else
        {
            isCrouching = false;
        }

        if (playerStance == PlayerStance.PSSprinting)
        {
            stanceFOV = sprintFOV;
            stanceHeight = GetStanceSettings().cameraHeight;
            capsuleHeight = GetStanceSettings().capsuleHeight;
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }


        if (playerStance == PlayerStance.PSAiming)
        {
            stanceFOV = aimingFOV;
            stanceHeight = GetStanceSettings().cameraHeight;
            capsuleHeight = GetStanceSettings().capsuleHeight;
        }

        playerCam.m_Lens.FieldOfView = Mathf.Lerp(camera.GetGateFittedFieldOfView(), stanceFOV,
            Time.deltaTime * fieldOfViewChangeTime);
        cameraHeight = Mathf.SmoothDamp(cameraHolder.localPosition.y, stanceHeight,
            ref cameraHeightVelocity, playerStanceSmoothing);

        cameraHolder.localPosition = new Vector3(cameraHolder.localPosition.x, cameraHeight, cameraHolder.localPosition.z);
        characterController.height = capsuleHeight;
    }

    private void Jump()
    {
        if (!isDead)
        {
            if (!characterController.isGrounded && jumpCount <= 1 || space_pressed >= 2)
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
    }

    private void DoubleJumping()
    {
        if(double_jump.doubleJump)
        {
            jumpCount = 2;
        }
        if(!double_jump.doubleJump)
        {
            jumpCount = 1;
        }

        if (characterController.isGrounded)
        {
            space_pressed = 0;
        }
    }

    private void DoubleSpeed()
    {
        if(double_speed.speedBooster)
        {
            speed = 2;
        }

        if(!double_speed.speedBooster)
        {
            speed = 1;
        }
    }
    private void CheckAirTime()
    {
        if (characterController.isGrounded)
        {
            airTime = 0f;

        }
        else
        {
            airTime += Time.deltaTime;
        }
    }
    private void CheckLand()
    {
        if (airTime > 0)
        {
            if (characterController.isGrounded)
            {
                RaycastHit tag;

                if (Physics.Raycast(cameraHolder.transform.position, cameraHolder.transform.up * -1, out tag, 2))
                {
                    if (tag.transform.gameObject.layer == 7)
                    {
                        onDirtLand = true;
                    }
                    else
                    {
                        onDirtLand = false;
                    }
                    if (tag.transform.gameObject.layer == 8)
                    {
                        onWaterLand = true;
                    }
                    else
                    {
                        onWaterLand = false;
                    }
                    if (tag.transform.gameObject.layer == 9)
                    {
                        onConcreteLand = true;
                    }
                    else
                    {
                        onConcreteLand = false;
                    }
                    if (tag.transform.gameObject.layer == 10)
                    {
                        onWoodLand = true;
                    }
                    else
                    {
                        onWoodLand = false;
                    }
                    if (tag.transform.gameObject.layer == 11)
                    {
                        onMetalLand = true;
                    }
                    else
                    {
                        onMetalLand = false;
                    }
                    if (tag.transform.gameObject.layer == 12)
                    {
                        onGrassLand = true;
                    }
                    else
                    {
                        onGrassLand = false;
                    }
                }
            }
        }
        if (airTime == 0)
        {
            onDirtLand = false;
            onGrassLand = false;
            onConcreteLand = false;
            onWoodLand = false;
            onWaterLand = false;
            onMetalLand = false;
        }
    }


    private void Crouch()
    {
        if (!isDead)
        {
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
    }

    private void StartSprint()
    {
        if (!isDead)
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
    }

    private void StopSprint()
    {
        if (!isDead)
        {
            //stops players sprinting again after sprint crouching.. (sliding)
            if (playerStance != PlayerStance.PSCrouching)
            {
                isSprinting = false;
                playerStance = PlayerStance.PSStanding;
            }
        }
    }

    private void StartAiming()
    {
        if (!isDead)
        {
            previousPlayerStance = playerStance;

            // if we are crouching and aiming we need to update our capsule
            // and height to match just crouching
            if (previousPlayerStance == PlayerStance.PSCrouching)
            {
                aimingSettings.cameraHeight = crouchingSettings.cameraHeight;
                aimingSettings.capsuleHeight = crouchingSettings.capsuleHeight;
            }

            playerStance = PlayerStance.PSAiming;
        }
    }

    private void StopAiming()
    {
        if (!isDead)
        {
            playerStance = previousPlayerStance;
            // reset our aim settings back to default
            aimingSettings.cameraHeight = standingSettings.cameraHeight;
            aimingSettings.capsuleHeight = standingSettings.capsuleHeight;
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
            case PlayerStance.PSAiming:
                return aimingSettings;
        }

        return null;
    }
}

public enum PlayerStance
{
    PSStanding,
    PSCrouching,
    PSSprinting,
    PSAiming,
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