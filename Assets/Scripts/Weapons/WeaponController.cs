using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Managers;
using Random = UnityEngine.Random;

// ReSharper disable All

public class WeaponController : MonoBehaviour
{
    public class Bullet
    {
        public float time;
        public Vector3 initialPosition;
        public Vector3 initialVelocity;
        public TrailRenderer tracer;
    }

    private PlayerController playerController;
    public bool isAiming;

    [Header("Settings")] public WeaponSettings settings;

    [Header("HUD")] public int minHeldAmmoDisplay = 10;
    public int minCurrentAmmoDisplay = 5;

    [Header("Weapon")] public float fireRate;
    public float damage;
    public float range;
    public float bulletSpreadIdle = 0f;
    public float bulletSpreadAim = 0f;
    public float bulletSpeed = 1000.0f;
    public float bulletDrop = 0.0f;

    private List<Bullet> spawnedBullets = new List<Bullet>();
    private List<TrailRenderer> traiList = new List<TrailRenderer>();

    [Header("Reload")] public int magazineSize = 30;
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
    public float cameraRecoil = 1.0f;
    private float recoil = 0.0f;

    private Vector3 newRecoilCameraPosition;
    private Vector3 newRecoilCameraVelocity;
    private Vector3 newRecoilTargetCameraRotation;
    private Vector3 newRecoilTargetCameraVelocity;

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
    public TrailRenderer tracerEffect;
    public Transform bulletOriginTransform;

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
    private GameObject lookAtTarget;
    public AudioClip weaponSound;

    private void Awake()
    {
        currentAmmo = magazineSize;
        minHeldAmmoDisplay = magazineSize;
        minCurrentAmmoDisplay = magazineSize / 3;
    }

    private void OnEnable()
    {
        isReloading = false;

        UIManager.Instance.SetAmmo(currentAmmo, equippedAmmo, magazineSize);
        UIManager.Instance.UpdateAmmo(currentAmmo, equippedAmmo);

        CheckHeldAmmo();
        CheckCurrentAmmo();
    }

    private void OnDisable()
    {
        UIManager.Instance.ammoDisplay.FadeOut();
        UIManager.Instance.weaponPanel.ClearAmmo();
        //fade out the current ammo stuff if it is visible?

        // check the visibility in the UI manager for whether its visile or not? 
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
        lookAtTarget = playerController.LookAtTarget;
        isInitialised = true;

        // update the currently equipped weapon image.
    }

    public void InitialiseEnemy()
    {
        isInitialised = true;
    }

    Vector3 GetBulletPosition(Bullet bullet)
    {
        Vector3 gravity = Vector3.down * bulletDrop;
        return (bullet.initialPosition) + (bullet.initialVelocity * bullet.time) +
               (0.5f * gravity * bullet.time * bullet.time);
    }

    Bullet CreateBullet(Vector3 position, Vector3 velocity)
    {
        Bullet bullet = new Bullet();
        bullet.initialPosition = position;
        bullet.initialVelocity = velocity;
        bullet.time = 0.0f;
        bullet.tracer = Instantiate(tracerEffect, position, Quaternion.identity);
        bullet.tracer.AddPosition(position);
        Destroy(bullet.tracer.gameObject, bullet.tracer.time);
        return bullet;
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
            CalculateRecoil();
     
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

        CheckHeldAmmo();
        CheckCurrentAmmo();

        UIManager.Instance.UpdateAmmo(currentAmmo, equippedAmmo);
    }

    private void CheckHeldAmmo()
    {
        if (isActiveAndEnabled)
        {
            if (equippedAmmo <= minHeldAmmoDisplay)
            {
                UIManager.Instance.ammoDisplay.FadeIn();
            }
            else
            {
                UIManager.Instance.ammoDisplay.FadeOut();
            }
        }
    }

    private void CheckCurrentAmmo()
    {
        if (isActiveAndEnabled)
        {
            if (currentAmmo <= minCurrentAmmoDisplay)
            {
                UIManager.Instance.reloadDisplay.FadeIn();
            }
            else
            {
                UIManager.Instance.reloadDisplay.FadeOut();
            }
        }
    }

    public void AddAmmo(int ammo)
    {
        equippedAmmo += ammo;
        UIManager.Instance.UpdateAmmo(currentAmmo, equippedAmmo);
        CheckHeldAmmo();
    }

    public void Shoot(Camera cam)
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

            UIManager.Instance.UpdateAmmo(currentAmmo, equippedAmmo);
            UIManager.Instance.crosshair.SetCrosshairRecoil(0.1f);

            recoil += 0.1f;

            muzzleParticle.Play();
            CheckCurrentAmmo();

            SoundManager.Instance.PlayWeaponSound(weaponSound);

            RaycastHit hit;
            Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity);

            Vector3 direction = GetShotDirection();
            Vector3 hitpoint = cam.transform.TransformPoint(direction * range);
            Vector3 velocity = (hitpoint - bulletOriginTransform.position).normalized * bulletSpeed;
            var bullet = CreateBullet(bulletOriginTransform.position, velocity);
            spawnedBullets.Add(bullet);
        }
    }

    private Vector3 GetShotDirection()
    {
        Vector3 direction = Vector3.forward;

        if (!isAiming)
        {
            direction += new Vector3(
                Random.Range(-bulletSpreadIdle, bulletSpreadIdle),
                Random.Range(-bulletSpreadIdle, bulletSpreadIdle),
                Random.Range(-bulletSpreadIdle, bulletSpreadIdle));
            direction.Normalize();
            return direction;
        }

        return direction;
    }

    public void UpdateBullets(float delta)
    {
        SimulateBullets(delta);
        DestroyBullets();
    }

    private void SimulateBullets(float deltaTime)
    {
        spawnedBullets.ForEach(bullet =>
        {
            Vector3 p0 = GetBulletPosition(bullet);
            bullet.time += deltaTime;
            Vector3 p1 = GetBulletPosition(bullet);
            RaycastSegment(p0, p1, bullet);
        });
    }

    private void DestroyBullets()
    {
        spawnedBullets.RemoveAll(bullet => bullet.time >= 0.5f);
    }

    void RaycastSegment(Vector3 start, Vector3 end, Bullet bullet)
    {
        float distance = (end - start).magnitude;
        RaycastHit hit;
        if (Physics.Raycast(start, end - start, out hit, distance))
        {
            var target = hit.transform.gameObject.GetComponent<ShootingTarget>();

            if (bullet.tracer)
            {
               bullet.tracer.transform.position = hit.point; 
            }
            
            if (target != null && hit.transform.tag == "Enemy")
            {
                print("Hit Enemy");
                audioSource.PlayOneShot(hitmarkerClip);
                UIManager.Instance.crosshair.SetHitmarker();
                hit.transform.gameObject.GetComponent<ShootingTarget>().TakeDamage(damage);
            }
            
            bullet.time = 0.5f;
            GameObject ImpactObject = Instantiate(hitParticle, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(ImpactObject, 0.4f);
        }
        else
        {
            if (bullet.tracer)
            {
               bullet.tracer.transform.position = end; 
            }
        }
    }

    /*
    IEnumerator SpawnTrail(TrailRenderer tracer, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPosition = tracer.transform.position;
        while (time < 1)
        {
            tracer.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / tracer.time;
            yield return null;
        }

        tracer.transform.position = hit.point;
        Destroy(tracer.gameObject, tracer.time);
    }
    */

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
            Debug.Log("Aiming True");
            targetPosition = playerController.cameraHolder.position +
                             (transform.position - sightTarget.position) +
                             (playerController.cameraHolder.transform.forward * sightOffset);
        }

        weaponSwayPosition = transform.position;
        weaponSwayPosition =
            Vector3.SmoothDamp(weaponSwayPosition, targetPosition, ref weaponSwayPositionVelocity, aimingTime);
        transform.position = weaponSwayPosition;
    }

    private void CalculateRecoil()
    {
        if (recoil > 0)
        {
            float currentTrans = lookAtTarget.transform.localPosition.y;
            var newRotation = currentTrans += cameraRecoil;
            newRecoilCameraPosition = new Vector3(lookAtTarget.transform.localPosition.x, currentTrans,
                lookAtTarget.transform.localPosition.z);
            lookAtTarget.transform.localPosition = Vector3.Slerp(lookAtTarget.transform.localPosition,
                newRecoilCameraPosition, Time.deltaTime * recoilSpeed);

            var maxRecoil = Quaternion.Euler(Random.Range(transform.parent.localRotation.x, maxRecoilX),
                Random.Range(transform.parent.localRotation.y - maxRecoilY, maxRecoilY),
                transform.parent.localRotation.z);

            //playerController.camera.transform.localRotation = Quaternion.Slerp(transform.parent.localRotation, maxRecoil, Time.deltaTime * recoilSpeed);
            transform.parent.localRotation =
                Quaternion.Slerp(transform.parent.localRotation, maxRecoil, Time.deltaTime * recoilSpeed);

            var maxTranslation = new Vector3(transform.parent.localPosition.x,
                Random.Range(transform.parent.localPosition.y, maxTransY), transform.parent.localPosition.z);

            transform.parent.localPosition = Vector3.Slerp(transform.parent.localPosition, maxTranslation,
                Time.deltaTime * recoilSpeed);
            recoil -= Time.deltaTime;
        }
        else
        {
            recoil = 0;
            var minRecoil = Quaternion.Euler(Random.Range(0, transform.parent.localRotation.x),
                Random.Range(0, transform.parent.localRotation.y), transform.parent.localRotation.z);
            transform.parent.localRotation = Quaternion.Slerp(transform.parent.localRotation, minRecoil,
                Time.deltaTime * recoilSpeed / 2);

            Vector3 minPosition = new Vector3(lookAtTarget.transform.localPosition.x, 0,
                lookAtTarget.transform.localPosition.z);
            lookAtTarget.transform.localPosition = Vector3.Slerp(lookAtTarget.transform.localPosition, minPosition,
                Time.deltaTime * recoilSpeed);

            var minTranslation = new Vector3(
                transform.parent.localPosition.x, Random.Range(0, transform.parent.localPosition.y),
                transform.parent.localPosition.z);
            transform.parent.localPosition = Vector3.Slerp(transform.parent.localPosition, minTranslation,
                Time.deltaTime * recoilSpeed);
        }
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

    [Header("Weapon Movement Sway")] public float movementSwayZ;
    public float movementSwayY;
    public bool movementSwayZInverted;
    public bool movementSwayYInverted;
    public float movementSwaySmoothing;
}