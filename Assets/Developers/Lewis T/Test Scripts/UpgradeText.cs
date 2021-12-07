using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeText : MonoBehaviour
{

    public float speed = 0.1f;
    private string weaponText;
    private string upgradeText;
    private string descriptionText;


    public Text TextWeapon;
    public Text TextUpgrade;
    public Text TextDescription;


    public bool finished = true;
    public bool selectable = false;

    public Select input;

    void Start()
    {
        input = new Select();

        input.Move.Select.performed += e => SetUpgrade();

        input.Enable();
    }

    private void Update()
    {
        SetText();
    }

    private void SetUpgrade()
    {
    }

    

    private void SetText()
    {
        weaponText = "WEAPON UPGRADE";
        upgradeText = "UPGRADE REQUIRED 2000 CREDITS";
        descriptionText = "THIS UPGRADE DOUBLES YOUR RATE OF FIRE";
        
    }

}

