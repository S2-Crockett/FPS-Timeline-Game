using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        public static UIUtilities Utilities;
        
        [Header("HUDS")] 
        public CanvasGroup gameCanvas;
        public AmmoDisplay ammoDisplay;
        public ReloadDisplay reloadDisplay;
        public Canvas gameGroup;
        public Canvas deathGroup;

        [Header("AmmoUI")] 
        public Text currentAmmoText;
        public Text currentHeldAmmoText;
        public WeaponPanel weaponPanel;

        [Header("Health")] 
        public HealthPanel healthPanel;
        public HealthComponent health;
        public DeathPanel deathPanel;

        [Header("Crosshair")] 
        public Crosshair crosshair;

        [Header("Checkpoint")] 
        public CanvasGroup checkpointCanvas;
        public Text checkPointNameText;

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

        private void Start()
        {
            Utilities = GetComponent<UIUtilities>();
        }

        public void UpdateAmmo(int currAmmo, int heldAmmo)
        {
            weaponPanel.UpdateAmmo(currAmmo, heldAmmo);
        }

        public void SetAmmo(int maxAmmo, int heldAmmo, int maxCurrentAmmo)
        {
            weaponPanel.SetAmmo(maxAmmo, heldAmmo, maxCurrentAmmo);
        }
        
        public void SetHealthShield(int maxHealth, int maxShield)
        {
            healthPanel.SetHealthInformation(maxHealth, maxShield);
        }

        public void UpdateHealth(int normalizedHealth, int normalizedShield)
        {
            healthPanel.CalculateNewPercentage(normalizedHealth, normalizedShield);
        }
        
        public float GetHealthNormalized()
        {
            return health.GetHealthNormalized();
        }
        
        public float GetShieldNormalized()
        {
            return health.GetShieldNormalized();
        }

        public void  SetCursor(bool visible)
        {
            if (visible)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        public void SetNewCheckpoint(string name)
        {
            checkPointNameText.text = name;
            LeanTween.value(gameObject, 0, 1, 0.75f)
                .setOnUpdate((value) =>
                {
                    checkpointCanvas.alpha = value;
                })
                .setOnComplete(FadeOutNewCheckpoint);
        }
        
        public void FadeOutNewCheckpoint()
        {
            StartCoroutine(FadeOut());
        }

        IEnumerator FadeOut()
        {
            yield return new WaitForSeconds(4f);
            LeanTween.value(gameObject, 1, 0, 0.75f)
                .setOnUpdate((value) =>
                {
                    checkpointCanvas.alpha = value;
                });
        }

        public void EnableRespawnButton()
        {
            deathPanel.EnableRespawnButton(true);
        }

        public void FadeGameHUD(float start, float end, float time)
        {
            LeanTween.value(gameObject, start, end, time)
                .setOnUpdate((value) =>
                {
                    gameCanvas.alpha = value;
                });
        }

        public void SetCameraHUD(Camera cam)
        {
            gameGroup.worldCamera = cam;
            deathGroup.worldCamera = cam;
        }

    }
}
