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

    [SerializeField] private AudioClip[] dirtRunning;
    [SerializeField] private AudioClip[] waterRunning;
    [SerializeField] private AudioClip[] concreteRunnings;
    [SerializeField] private AudioClip[] woodRunning;
    [SerializeField] private AudioClip[] metalRunning;
    [SerializeField] private AudioClip[] grassRunning;

    [SerializeField] private AudioSource _FootstepSource;

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
            float newVolume = 0.05f;
            if(pController.isSprinting == true)
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
            }
            else
            {
                _FootstepSource.volume = defaultVolume;
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
        }

    }

}
