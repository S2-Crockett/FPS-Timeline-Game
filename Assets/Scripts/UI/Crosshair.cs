using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [Header("Colours")] 
    public Color baseCrosshairColour;
    public Color enemyLookAtColour;
    public Color hitMarkerColour;
    public Color hitMarkerKillColour;
    
    [Header("Shooting Inner Crosshair")]
    public float idleSize = 10f;
    public float maxSize = 30f;
    public float speed;
    
    [Header("Hitmarker")]
    public float hitMarkerInitialSize = 10f;
    public float hitMarkerMaxSize = 25f;
    public float hitMarkerSpeed;
    
    private Image crosshairBorder;
    private Image crosshairInner;
    private float currentSize;
    private float recoil;

    private Image hitmarkerTopLeft;
    private Image hitmarkerTopRight;
    private Image hitmarkerBottomLeft;
    private Image hitmarkerBottomRight;

    private float hitMarkerView;
    private float currentHitMarkerSize;

    private RectTransform innerCrosshairTransform;
    private RectTransform hitMarkerTransform;
    
    

    // Start is called before the first frame update
    void Start()
    {
        //crosshair
        crosshairBorder = this.GetComponent<Image>();
        crosshairInner = transform.Find("CrosshairInner").GetComponent<Image>();
        innerCrosshairTransform = transform.Find("CrosshairInner").GetComponent<RectTransform>();
        crosshairBorder.color = baseCrosshairColour;
        crosshairInner.color = baseCrosshairColour;
        
        //hitmarker
        hitMarkerTransform = transform.Find("HitMarkerPanel").GetComponent<RectTransform>();
        hitmarkerTopLeft =  hitMarkerTransform.Find("TopLeft").GetComponent<Image>();
        hitmarkerTopRight =  hitMarkerTransform.Find("TopRight").GetComponent<Image>();
        hitmarkerBottomLeft =  hitMarkerTransform.Find("BottomLeft").GetComponent<Image>();
        hitmarkerBottomRight =  hitMarkerTransform.Find("BottomRight").GetComponent<Image>();
        
        SetHitMarkerAlpha(0);
        hitMarkerTransform.sizeDelta = new Vector2(hitMarkerInitialSize, hitMarkerInitialSize);
        currentHitMarkerSize = hitMarkerInitialSize;
    }

    // Update is called once per frame
    void Update()
    {
        //handles the inner radius of the thing if we start shooting // set to false as soon as a shot is fired.
        if (recoil > 0)
        {
            currentSize = Mathf.Lerp(currentSize, maxSize, Time.deltaTime * speed);
            innerCrosshairTransform.sizeDelta = new Vector2(currentSize, currentSize);
            recoil -= Time.deltaTime;
        }
        else
        {
            recoil = 0;
            currentSize = Mathf.Lerp(currentSize, idleSize, Time.deltaTime * speed);
            innerCrosshairTransform.sizeDelta = new Vector2(currentSize, currentSize);
        }

        if (hitMarkerView > 0)
        {
            SetHitMarkerAlpha(Mathf.Lerp(0,255, Time.deltaTime * hitMarkerSpeed));
            currentHitMarkerSize = Mathf.Lerp(currentHitMarkerSize, hitMarkerMaxSize, Time.deltaTime * hitMarkerSpeed);
            hitMarkerTransform.sizeDelta = new Vector2(currentHitMarkerSize, currentHitMarkerSize);
            hitMarkerView -= Time.deltaTime;
        }
        else
        {
            hitMarkerView = 0;
            SetHitMarkerAlpha(Mathf.Lerp(hitmarkerTopLeft.color.a,0, Time.deltaTime * hitMarkerSpeed * 2));                                                 
            currentHitMarkerSize = Mathf.Lerp(currentHitMarkerSize, hitMarkerInitialSize, Time.deltaTime * hitMarkerSpeed);            
            hitMarkerTransform.sizeDelta = new Vector2(currentHitMarkerSize, currentHitMarkerSize);
        }
    }
    
    public void UpdateLookAtColour(bool show)
    {
        if (show)
        {
            crosshairBorder.color = enemyLookAtColour;
            crosshairInner.color = enemyLookAtColour;
        }
        else
        {
            crosshairBorder.color = baseCrosshairColour;
            crosshairInner.color = baseCrosshairColour;
        }
    }

    public void SetCrosshairRecoil(float recoil)
    {
        this.recoil += recoil;
    }

    public void SetHitmarker()
    {
        if (IsHitMarkerVisible())
        {
            // reset here
        }
        // need to check if there is something already playing?
        // if there is we need to reset hit marker then play from the original
        
        this.hitMarkerView = 0.3f;
    }

    private bool IsHitMarkerVisible()
    {
        return currentHitMarkerSize > 10;
    }

    private void SetHitMarkerAlpha(float a)
    {
        var tempColour = Color.white;
        tempColour.a = a;
        
        hitmarkerTopLeft.color = tempColour;
        hitmarkerTopRight.color = tempColour;
        hitmarkerBottomLeft.color = tempColour;
        hitmarkerBottomRight.color = tempColour;
    }
    
}
