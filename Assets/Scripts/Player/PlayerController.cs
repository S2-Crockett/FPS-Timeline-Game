using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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

        defaultInput.Player.Shoot.started += e => ToggleShoot();
        defaultInput.Player.Shoot.canceled += e => ToggleShoot();

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

        if (shouldShoot)
        {
            if (currentWeapon)
            {
                currentWeapon.Shoot(Camera.main);
            }
        }
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

        CheckLand();
        CreateSounds(movementSpeed);

        CheckAirTime();

    }
    private void CreateSounds(Vector3 movementSpeed)
    {
        RaycastHit tag;

        if (Physics.Raycast(cameraHolder.transform.position, cameraHolder.transform.up * -1, out tag, 2))
        {
            if (movementSpeed.x > 0.01f ||
            movementSpeed.x < -0.01f ||
            movementSpeed.z > 0.01f ||
            movementSpeed.z < -0.01f)
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
        float stanceHeight = standingSettings.cameraHeight;
        float capsuleHeight = standingSettings.capsuleHeight;

        if (playerStance == PlayerStance.PSCrouching)
        {
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
            stanceHeight = GetStanceSettings().cameraHeight;
            capsuleHeight = GetStanceSettings().capsuleHeight;
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }

        cameraHeight = Mathf.SmoothDamp(cameraHolder.localPosition.y, stanceHeight,
            ref cameraHeightVelocity, playerStanceSmoothing);

        cameraHolder.localPosition = new Vector3(cameraHolder.localPosition.x, cameraHeight, cameraHolder.localPosition.z);
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

    private void ToggleShoot()
    {
        //fire the current weapon equipped? might need to check so you can do it whilst doing certain things?
        //apply offset of the camera based on the equipped weapons recoil amounts
        shouldShoot = !shouldShoot;
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
