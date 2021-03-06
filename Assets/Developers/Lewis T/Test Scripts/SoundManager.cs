using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioClip[] dirtFootsteps;
    [SerializeField] private AudioClip[] waterFootsteps;
    [SerializeField] private AudioClip[] concreteFootsteps;
    [SerializeField] private AudioClip[] woodFootsteps;
    [SerializeField] private AudioClip[] metalFootsteps;
    [SerializeField] private AudioClip[] grassFootsteps;

    [SerializeField] private AudioClip[] dirtLanding;
    [SerializeField] private AudioClip[] waterLanding;
    [SerializeField] private AudioClip[] concreteLanding;
    [SerializeField] private AudioClip[] woodLanding;
    [SerializeField] private AudioClip[] metalLanding;
    [SerializeField] private AudioClip[] grassLanding;

    [SerializeField] private AudioSource _FootstepSource;
    [SerializeField] private AudioSource _LandingSource;
    [SerializeField] private AudioSource _PickupSource;
    [SerializeField] private AudioSource _WeaponSource;

    public PlayerController pController;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlaySound(AudioClip clip)
    {
        if (!_FootstepSource.isPlaying)
        {
            _FootstepSource.PlayOneShot(clip);

        }
    }
    public void PlayLandingSound(AudioClip clip)
    {
        if (!_LandingSource.isPlaying)
        {
            _LandingSource.PlayOneShot(clip);

        }
    }
    public void PlayPickupSound(AudioClip clip)
    {
            _PickupSource.PlayOneShot(clip);
    }
    public void PlayWeaponSound(AudioClip clip)
    {
        _WeaponSource.PlayOneShot(clip);
    }
    private void Update()
    {
        AccessSound();
    }

    public void AccessSound()
    {
        if (pController != null)
        {
            float defaultSpeed = 0.6f;
            float defaultVolume = 0.1f;
            float newSpeed = 1.2f;
            float slowSpeed = 0.3f;
            float newVolume = 0.05f;
            if (pController.isSprinting == true)
            {
                _FootstepSource.pitch = newSpeed;
            }
            else
            {
                _FootstepSource.pitch = defaultSpeed;
            }

            if (pController.isCrouching == true)
            {
                _FootstepSource.volume = newVolume;
                _FootstepSource.pitch = slowSpeed;
            }
            else
            {
                _FootstepSource.volume = defaultVolume;
                //_FootstepSource.pitch = defaultSpeed;
            }

            if (pController.onDirt == true)
            {
                Instance.PlaySound(dirtFootsteps[UnityEngine.Random.Range(0, dirtFootsteps.Length)]);
            }
            else if (pController.onWater == true)
            {
                Instance.PlaySound(waterFootsteps[UnityEngine.Random.Range(0, waterFootsteps.Length)]);
            }
            else if (pController.onConcrete == true)
            {
                Instance.PlaySound(concreteFootsteps[UnityEngine.Random.Range(0, concreteFootsteps.Length)]);
            }
            else if (pController.onWood == true)
            {
                Instance.PlaySound(woodFootsteps[UnityEngine.Random.Range(0, woodFootsteps.Length)]);
            }
            else if (pController.onMetal == true)
            {
                Instance.PlaySound(metalFootsteps[UnityEngine.Random.Range(0, metalFootsteps.Length)]);
            }
            else if (pController.onGrass == true)
            {
                Instance.PlaySound(grassFootsteps[UnityEngine.Random.Range(0, grassFootsteps.Length)]);
            }

            if (pController.onDirtLand == true)
            {
                Instance.PlaySound(dirtLanding[UnityEngine.Random.Range(0, dirtLanding.Length)]);
            }
            else if (pController.onWaterLand == true)
            {
                Instance.PlaySound(waterLanding[UnityEngine.Random.Range(0, waterLanding.Length)]);
            }
            else if (pController.onConcreteLand == true)
            {
                Instance.PlaySound(concreteLanding[UnityEngine.Random.Range(0, concreteLanding.Length)]);
            }
            else if (pController.onWoodLand == true)
            {
                Instance.PlaySound(woodLanding[UnityEngine.Random.Range(0, woodLanding.Length)]);
            }
            else if (pController.onMetalLand == true)
            {
                Instance.PlaySound(metalLanding[UnityEngine.Random.Range(0, metalLanding.Length)]);
            }
            else if (pController.onGrassLand == true)
            {
                Instance.PlaySound(grassLanding[UnityEngine.Random.Range(0, grassLanding.Length)]);
            }
        }

    }

}
