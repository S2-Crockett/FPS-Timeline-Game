using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        [Header("AmmoUI")]
        public Text currentAmmoText;
        public Text currentHeldAmmoText;
    
        void Awake()
        {
            if(Instance == null)
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
    }
}
