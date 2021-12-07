using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPanel : MonoBehaviour
{
    [Header("Health")] public Color healthTextColour;
    public Color healthColour;
    public int healthPerPoint = 10;
    public int health = 100;

    [Header("Shield")] public Color shieldTextColour;
    public Color shieldColour;
    public int shieldPerPoint = 10;
    public int shield = 50;

    [Header("Health References")] public Text healthText;
    public GameObject healthPoint;
    public RectTransform pointTransform;

    [Header("Shield References")] public Text shieldText;
    public GameObject shieldPoint;
    public RectTransform shieldTransform;

    public List<Image> healthList = new List<Image>();
    public List<Image> shieldList = new List<Image>();
    private List<GameObject> objectList = new List<GameObject>();

    private int numHealthPoints;
    private int numShieldPoints;
    private float spacing = 14.0f;
    private int maxHealth;
    private int maxShield;

    // Start is called before the first frame update

    public void SetHealthInformation(int health, int shield)
    {
        healthList = new List<Image>();
        shieldList = new List<Image>();
        objectList = new List<GameObject>();
        
        this.health = health;
        this.shield = shield;
        maxHealth = health;
        maxShield = shield;

        numShieldPoints = shield / shieldPerPoint;
        numHealthPoints = health / healthPerPoint;

        InitiatePoints();

        healthText.text = health.ToString("#");
        shieldText.text = shield.ToString("#");

        healthText.color = healthTextColour;
        shieldText.color = shieldTextColour;
    }

    public void InitiatePoints()
    {
        float healthStart = 0;
        float shieldStart = 0;

        for (int a = 0; a < numHealthPoints; a++)
        {
            var point = Instantiate(healthPoint, pointTransform);
            point.transform.SetParent(pointTransform, false);
            point.GetComponent<RectTransform>().anchoredPosition = new Vector2(healthStart, 0);
            objectList.Add(point);

            Image tempImg = point.transform.Find("Mask").transform.Find("HealthFill").GetComponent<Image>();
            if (tempImg)
            {
                tempImg.color = healthColour;
                healthList.Add(tempImg);
            }

            healthStart += spacing;
        }

        for (int b = 0; b < numShieldPoints; b++)
        {
            var point = Instantiate(shieldPoint, shieldTransform);
            point.transform.SetParent(shieldTransform, false);
            point.GetComponent<RectTransform>().anchoredPosition = new Vector2(shieldStart, 0);
            objectList.Add(point);

            Image tempImg = point.transform.Find("Mask").transform.Find("HealthFill").GetComponent<Image>();
            if (tempImg)
            {
                tempImg.color = shieldColour;
                shieldList.Add(tempImg);
            }

            shieldStart += spacing;
        }
    }

    public void CalculateNewPercentage(int health, int shield)
    {
        this.health = health;
        this.shield = shield;

        healthText.text = this.health.ToString();
        shieldText.text = this.shield.ToString();

        float newShield = (float) (maxShield - shield) / shieldPerPoint;
        float newHealth = (float) (maxHealth - health) / healthPerPoint;


        UpdateShield(newShield);
        UpdateHealth(newHealth);
    }

    private void UpdateHealth(float newHealth)
    {
        for (int a = 0; a < healthList.Count; a++)
        {
            healthList[a].fillAmount = DisplayPoint(a, health);
        }
    }

    private void UpdateShield(float newShield)
    {
        for (int a = 0; a < shieldList.Count; a++)
        {
            shieldList[a].fillAmount = DisplayPoint(a, shield);
        }
    }

    private float DisplayPoint(int point, float amount)
    {
        if ((point * 10) >= amount)
            return 0;
        else
            return 1;
    }

    public void ClearPoints()
    {
        for (int a = 0; a < objectList.Count; a++)
        {
            Destroy(objectList[a]);
        }
        
        for (int a = 0; a < healthList.Count; a++)
        {
            Destroy(healthList[a]);
        }

        for (int a = 0; a < shieldList.Count; a++)
        {
            Destroy(healthList[a]);
        }

        
    }
}