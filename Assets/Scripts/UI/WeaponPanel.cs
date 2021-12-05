using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPanel : MonoBehaviour
{
    [Header("Colours")] 
    public Color heldAmmoTextColour;
    public Color currentAmmoTextColour;
    public Color ammoPointColour;

    [Header("References")] 
    public RectTransform pointTransform;
    public GameObject ammoPoint;
    public Text currentAmmoText;
    public Text heldAmmoText;

    [Header("Images")] 
    public Image weaponImage;

    [Header("Edits")] 
    public float spacing = 11;

    private List<GameObject> bulletObjectList;
    private List<Image> bulletList;

    private int currentAmmo;
    private int heldAmmo;

    private int maxCurrentAmmo;
    private int maxHeldAmmo;

    private int numOfBullets;

    public void SetAmmo(int currAmmo, int heldAmmo, int maxAmmo)
    {
        bulletObjectList = new List<GameObject>();
        bulletList = new List<Image>();

        currentAmmo = currAmmo;
        this.heldAmmo = heldAmmo;

        maxCurrentAmmo = maxAmmo;
        maxHeldAmmo = this.heldAmmo;

        numOfBullets = maxCurrentAmmo;
        InitiateBullets();
    }

    private void InitiateBullets()
    {
        float start = 0;
        for (int a = 0; a < numOfBullets; a++)
        {
            var point = Instantiate(ammoPoint, pointTransform);
            point.transform.SetParent(pointTransform, false);
            point.GetComponent<RectTransform>().anchoredPosition = new Vector2(start, 0);
            bulletObjectList.Add(point);

            Image tempImg = point.transform.Find("AmmoFill").GetComponent<Image>();
            if (tempImg)
            {
                tempImg.color = ammoPointColour;
                bulletList.Add(tempImg);
            }
            start -= spacing;
        }
    }

    public void UpdateAmmo(int currAmmo, int heldAmmo)
    {
        currentAmmo = currAmmo;
        this.heldAmmo = heldAmmo;
        
        currentAmmoText.text = currentAmmo.ToString();
        heldAmmoText.text = this.heldAmmo.ToString();
        
        for (int a = 1; a < bulletList.Count; a++)
        {
            bulletList[a].fillAmount = DisplayPoint(a, currentAmmo);
        }
    }

    public float DisplayPoint(int point, int amount)
    {
        if (point >= amount)
        {
            return 0;
        }
        else if (amount == 0 && point == 1)
        {
            return 0;
        }
        return 1;
    }

    public void ClearAmmo()
    {
        for (int a = 0; a < bulletObjectList.Count; a++)
        {
            Destroy(bulletObjectList[a]);
        }
        
        for (int a = 0; a < bulletList.Count; a++)
        {
            Destroy(bulletList[a]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
