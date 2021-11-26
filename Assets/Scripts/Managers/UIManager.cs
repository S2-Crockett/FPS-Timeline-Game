using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        [Header("AmmoUI")] public Text currentAmmoText;
        public Text currentHeldAmmoText;

        [Header("Health")] 
        public PlayerHealthBar healthBar;
        public HealthComponent health;

        [Header("Crosshair")] public GameObject crosshair;

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

        public void UpdateCurrentAmmo(int ammo)
        {
            currentAmmoText.text = ammo.ToString();
        }

        public void UpdateHeldAmmoText(int ammo)
        {
            currentHeldAmmoText.text = ammo.ToString();
        }

        public void UpdateHealthBarDamage()
        {
            healthBar.OnDamage();
        }

        public void UpdateHealthBarHeal()
        {
            healthBar.OnHealed();
        }

        public float GetHealthNormalized()
        {
            return health.GetHealthNormalized();
        }
    }
}
