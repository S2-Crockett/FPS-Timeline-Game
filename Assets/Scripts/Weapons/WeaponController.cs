using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private PlayerController playerController;

    public bool isAiming;

    [Header("Settings")] public WeaponSettings settings;

    [Header("Weapon")] public float fireRate;
    public float damage;
    public float range;
    public float sprayRadius;
    public int magazineSize;
    public int startingAmmo;
    private float nextTimeToFire = 0f;

    [Header("Recoil")] public float recoilSpread;
    public float recoilKickAmount;

    [Header("Idle Sway")] public Transform swayObject;
    public float swayAmountA = 1f;
    public float swayAmountB = 2f;
    public float swayScale = 600f;
    public float swayLerpSpeed = 14f;
    private float swayTime;
    private Vector3 swayPosition;

    [Header("Sights")] public Transform sightTarget;
    public float sightOffset;
    public float aimingTime;
    private Vector3 weaponSwayPosition;
    private Vector3 weaponSwayPositionVelocity;

    [Header("Effects")] public ParticleSystem muzzleParticle;
    public GameObject hitParticle;
    public AudioClip hitmarkerClip;

    [Header("Animations")] private bool isInitialised;
    private Vector3 newWeaponRotation;
    private Vector3 newWeaponRotationVelocity;
    private Vector3 targetWeaponRotation;
    private Vector3 targetWeaponRotationVelocity;

    private Vector3 newWeaponMovementRotation;
    private Vector3 newWeaponMovementRotationVelocity;
    private Vector3 targetWeaponMovementRotation;
    private Vector3 targetWeaponMovementRotationVelocity;

    private AudioSource audioSource;
    float time = 0;

    private void Awake()
    {
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        newWeaponRotation = transform.localRotation.eulerAngles;
        swayPosition = transform.parent.position;
    }

    public void Initialise(PlayerController controller)
    {
        playerController = controller;
        isInitialised = true;
    }

    private void Update()
    {
        swayObject = transform.parent.transform;

        if (!isInitialised)
        {
            return;
        }

        targetWeaponRotation.y += settings.swayAmount *
                                  (settings.swayXInverted
                                      ? -playerController.inputView.x
                                      : playerController.inputView.x) * Time.deltaTime;
        targetWeaponRotation.x += settings.swayAmount *
                                  (settings.swayYInverted
                                      ? playerController.inputView.y
                                      : -playerController.inputView.y) * Time.deltaTime;

        targetWeaponRotation.y = Mathf.Clamp(targetWeaponRotation.y, -settings.swayClampY, settings.swayClampY);
        targetWeaponRotation.x = Mathf.Clamp(targetWeaponRotation.x, -settings.swayClampX, settings.swayClampX);
        targetWeaponRotation.z = targetWeaponRotation.y * 2;

        targetWeaponRotation = Vector3.SmoothDamp(targetWeaponRotation, Vector3.zero, ref targetWeaponRotationVelocity,
            settings.swayResetSmoothing);
        newWeaponRotation = Vector3.SmoothDamp(newWeaponRotation, targetWeaponRotation, ref newWeaponRotationVelocity,
            settings.swaySmoothing);

        targetWeaponMovementRotation.z = settings.movementSwayZ * (settings.movementSwayZInverted
            ? -playerController.inputMovement.x
            : playerController.inputMovement.x);
        targetWeaponMovementRotation.x = settings.movementSwayY * (settings.movementSwayYInverted
            ? -playerController.inputMovement.y
            : playerController.inputMovement.y);

        targetWeaponMovementRotation = Vector3.SmoothDamp(targetWeaponMovementRotation, Vector3.zero,
            ref targetWeaponMovementRotationVelocity,
            settings.movementSwaySmoothing);
        newWeaponMovementRotation = Vector3.SmoothDamp(newWeaponMovementRotation, targetWeaponMovementRotation,
            ref newWeaponMovementRotationVelocity,
            settings.movementSwaySmoothing);

        transform.localRotation = Quaternion.Euler(newWeaponRotation + newWeaponMovementRotation);
        CalculateBreathing();
        CalculateAiming();
    }

    public void Shoot(Camera cam)
    {
        if (Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;

            muzzleParticle.Play();
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
            {
                ShootingTarget target = hit.transform.GetComponent<ShootingTarget>();
                if (target != null)
                {
                    audioSource.PlayOneShot(hitmarkerClip);
                    target.TakeDamage(damage);
                }

                GameObject ImpactObject = Instantiate(hitParticle, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(ImpactObject, 0.4f);
            }
        }
    }

    private void CalculateBreathing()
    {
        if (!isAiming)
        {
            var targetPosition = LissajousCurve(swayTime, swayAmountA, swayAmountB) / swayScale;
            swayPosition = Vector3.Lerp(swayPosition, targetPosition, Time.smoothDeltaTime * swayLerpSpeed);
            swayTime += Time.deltaTime;
            if (swayTime > 6.3f)
            {
                swayTime = 0f;
            }

            swayObject.localPosition = swayPosition;
        }
    }

    private Vector3 LissajousCurve(float time, float a, float b)
    {
        return new Vector3(Mathf.Sin(time), a * Mathf.Sin(b * time + Mathf.PI));
    }

    private void CalculateAiming()
    {
        var targetPosition = swayObject.position;
        if (isAiming)
        {
            targetPosition = playerController.cameraHolder.position +
                             (transform.position - sightTarget.position) +
                             (playerController.cameraHolder.transform.forward * sightOffset);
        }

        weaponSwayPosition = transform.position;
        weaponSwayPosition =
            Vector3.SmoothDamp(weaponSwayPosition, targetPosition, ref weaponSwayPositionVelocity, aimingTime);
        transform.position = weaponSwayPosition;
    }
}

[Serializable]
public class WeaponSettings
{
    [Header("Weapon Sway")]
    public float swayAmount;
    public bool swayYInverted;
    public bool swayXInverted;
    public float swaySmoothing;
    public float swayResetSmoothing;
    public float swayClampX;
    public float swayClampY;

    [Header("Weapon Movement Sway")] public float movementSwayZ;
    public float movementSwayY;
    public bool movementSwayZInverted;
    public bool movementSwayYInverted;
    public float movementSwaySmoothing;
}