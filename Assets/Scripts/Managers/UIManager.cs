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

        [Header("AmmoUI")] public Text currentAmmoText;
        public Text currentHeldAmmoText;

        [Header("Health")] 
        public PlayerHealthBar healthBar;
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

        public void UpdateCurrentAmmo(int ammo)
        {
            currentAmmoText.text = ammo.ToString();
        }

        public void UpdateHeldAmmoText(int ammo)
        {
            currentHeldAmmoText.text = ammo.ToString();
        }

        public void UpdateHealthDamage()
        {
            healthBar.OnDamage();
        }

        public void UpdateHealthHeal()
        {
            healthBar.OnHealed();
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

    }
}
