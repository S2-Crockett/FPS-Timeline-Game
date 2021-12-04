using System;
using UnityEngine;
using System.Collections;
using Managers;
using Random = UnityEngine.Random;
// ReSharper disable All

public class WeaponController : MonoBehaviour
{
    private PlayerController playerController;
    
    [HideInInspector] public bool isAiming;

    [Header("Settings")] 
    public WeaponSettings settings;

    [Header("Weapon")] public float fireRate;
    public float damage;
    public float range;
    public float sprayRadius;

    [Header("Reload")] 
    public int magazineSize = 30;
    public int equippedAmmo = 120;
    public float reloadTime = 1f;
    private int currentAmmo;
    private float nextTimeToFire = 0f;
    private bool isReloading = false;

    [Header("Recoil")] public float recoilSpread;
    public float maxRecoilX = -20.0f;
    public float maxRecoilY = -10.0f;
    public float maxTransY = 1.0f;
    public float maxTransZ = -1.0f;
    public float recoilSpeed = 10.0f;
    private float recoil = 0.0f;
    
    private Vector3 newRecoilCameraRotation;
    private Vector3 newRecoilCameraVelocity;
    private Vector3 newRecoilTargetCameraRotation;
    private Vector3 newRecoilTargetCameraVelocity; 

    [Header("Idle Sway")] 
    public Transform swayObject;
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

    [Header("Animations")] 
    
    private bool isInitialised;
    private Vector3 newWeaponRotation;
    private Vector3 newWeaponRotationVelocity;
    private Vector3 targetWeaponRotation;
    private Vector3 targetWeaponRotationVelocity;

    private Vector3 newWeaponMovementRotation;
    private Vector3 newWeaponMovementRotationVelocity;
    private Vector3 targetWeaponMovementRotation;
    private Vector3 targetWeaponMovementRotationVelocity;

    private AudioSource audioSource;
    private Camera cam;

    private void Awake()
    {
        currentAmmo = magazineSize;
    }

    private void OnEnable()
    {
        isReloading = false;
        UIManager.Instance.UpdateCurrentAmmo(currentAmmo);
        UIManager.Instance.UpdateHeldAmmoText(equippedAmmo);
    }
    
    private void Start()
    {
        if (playerController != null)
        {
            cam = playerController.camera;
        }
        audioSource = GetComponent<AudioSource>();
        newWeaponRotation = transform.localRotation.eulerAngles;
        swayPosition = transform.parent.position;
    }

    public void Initialise(PlayerController controller)
    {
        playerController = controller;
        isInitialised = true;
    }

    public void InitialiseEnemy()
    {
        isInitialised = true;
    }

    private void Update()
    {
        swayObject = transform.parent.transform;

        if (!isInitialised)
        {
            return;
        }
        if (this.tag == "Player")
        {
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
            CalculateRecoil();
        }
      
    }

    public IEnumerator Reload()
    {
        isReloading = true;
        if (currentAmmo == magazineSize)
        {
            // no need to reload instantly return
            yield return new WaitForSeconds(0.01f);
            isReloading = false;
        }
        if (equippedAmmo >= magazineSize - currentAmmo)
        {
            yield return new WaitForSeconds(reloadTime);
            equippedAmmo -= (magazineSize - currentAmmo);
            currentAmmo = magazineSize;
            isReloading = false;
        }
        else
        {
            yield return new WaitForSeconds(reloadTime);
            currentAmmo = equippedAmmo;
            equippedAmmo = 0;
            isReloading = false;
        }
        
        UIManager.Instance.UpdateCurrentAmmo(currentAmmo);
        UIManager.Instance.UpdateHeldAmmoText(equippedAmmo);
    }

    public void AddAmmo(int ammo)
    {
        equippedAmmo += ammo;
        UIManager.Instance.UpdateHeldAmmoText(equippedAmmo);
    }

    public void Shoot(Transform cam)
    {
        if (isReloading)
        {
            return;
        }
        
        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        if (Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;

            currentAmmo--;
            UIManager.Instance.UpdateCurrentAmmo(currentAmmo);
            UIManager.Instance.crosshair.SetCrosshairRecoil(0.1f);
            recoil += 0.1f;
            muzzleParticle.Play();
            
            RaycastHit hit;
            if (Physics.Raycast(cam.position, cam.forward, out hit, range))
            {
                ShootingTarget target = hit.transform.GetComponent<ShootingTarget>();
                if (target != null)
                {
                    audioSource.PlayOneShot(hitmarkerClip);
                    UIManager.Instance.crosshair.SetHitmarker();
                    target.TakeDamage(damage);
                }
                EnemyDead enemy = hit.transform.GetComponent<EnemyDead>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
                //GameObject ImpactObject = Instantiate(hitParticle, hit.point, Quaternion.LookRotation(hit.normal));
                //Destroy(ImpactObject, 0.4f);
            }
        }
        //UIManager.Instance.crosshair.SetIsShooting(false);
    }

    private void CalculateBreathing()
    {
        if (recoil < 0.01f)
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
    
    private void CalculateCameraRecoil()
    {
        var targetRotation = new Vector3(cam.transform.localRotation.x - 10.0f, cam.transform.localRotation.y, cam.transform.localRotation.z);

        newRecoilCameraRotation = playerController.cameraHolder.localRotation.eulerAngles;
        newRecoilCameraRotation =
            Vector3.SmoothDamp(newRecoilCameraRotation, targetRotation, ref newRecoilCameraVelocity, 2.0f);
        cam.transform.localRotation = Quaternion.Euler(newRecoilCameraRotation);
    }


    private void CalculateRecoil()
    {
        if (recoil > 0)
        {
            float currentTrans = cam.transform.localRotation.x;
            var newRotation = currentTrans -= 5;
            newRecoilCameraRotation = new Vector3(currentTrans, cam.transform.localRotation.y, cam.transform.localRotation.z);
            cam.transform.localRotation = Quaternion.Slerp(cam.transform.localRotation, Quaternion.Euler(newRecoilCameraRotation),  Time.deltaTime * recoilSpeed);
            
            var maxRecoil = Quaternion.Euler(Random.Range(transform.parent.localRotation.x, maxRecoilX), Random.Range(transform.parent.localRotation.y - maxRecoilY, maxRecoilY), transform.parent.localRotation.z);
            
            //playerController.camera.transform.localRotation = Quaternion.Slerp(transform.parent.localRotation, maxRecoil, Time.deltaTime * recoilSpeed);
            transform.parent.localRotation = Quaternion.Slerp(transform.parent.localRotation, maxRecoil, Time.deltaTime * recoilSpeed);
 
            var maxTranslation = new Vector3(transform.parent.localPosition.x, Random.Range(transform.parent.localPosition.y, maxTransY), transform.parent.localPosition.z);
            
            transform.parent.localPosition = Vector3.Slerp(transform.parent.localPosition, maxTranslation, Time.deltaTime * recoilSpeed);
            recoil -= Time.deltaTime;
        }
        else
        {
            recoil = 0;
            var minRecoil = Quaternion.Euler(Random.Range(0, transform.parent.localRotation.x), Random.Range(0, transform.parent.localRotation.y), transform.parent.localRotation.z);
            transform.parent.localRotation = Quaternion.Slerp(transform.parent.localRotation, minRecoil, Time.deltaTime * recoilSpeed / 2);
            
            var minRotation = Quaternion.Euler(0, cam.transform.localRotation.y, cam.transform.localRotation.z);
            cam.transform.localRotation = Quaternion.Slerp(cam.transform.localRotation, minRotation,  Time.deltaTime * recoilSpeed);
            
            var minTranslation = new Vector3(
                transform.parent.localPosition.x, Random.Range(0, transform.parent.localPosition.y), transform.parent.localPosition.z);
            transform.parent.localPosition = Vector3.Slerp(transform.parent.localPosition, minTranslation, Time.deltaTime * recoilSpeed);
        }
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