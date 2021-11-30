using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{

    private ZoneChecker LevelManager;
    private GameObject player;

    [Header("Weapon")]
    private WeaponController currentWeapon;
    private int currentActiveIndex;
    private GameObject[] weaponRefs = new GameObject[5];
    public WeaponSlot[] weaponSlots;
    public Transform weaponHolder;


    public int speed = 2;

    void Start()
    {
        LevelManager = GameObject.Find("LevelManager").GetComponent<ZoneChecker>();
        player = GameObject.Find("Player");
    }

    private void Awake()
    {
        Initialise();
    
        for(int i = 0; i < weaponSlots.Length; i++)
        {
            weaponSlots[i].weaponObject.gameObject.transform.localScale = new Vector3(1,1,1);
        }

    }

    void Update()
    {

        Vector3 targetDir = player.transform.position - transform.position;

        float rotation = Vector3.Angle(targetDir, transform.forward);
        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (rotation < 45 && distance < 15)
        {
            weaponHolder.LookAt(player.transform);
            if (currentWeapon)
            {
                currentWeapon.Shoot(player.GetComponent<Camera>());
            }
        }
    }

    public void Initialise()
    {
        InitiateBaseWeapons();
    }

    private void InitiateBaseWeapons()
    {
        int x = 0;
        foreach (var slot in weaponSlots)
        {
            GameObject obj = Instantiate(slot.weaponObject, weaponHolder.position, weaponHolder.rotation);
            obj.transform.parent = weaponHolder;
            obj.GetComponent<WeaponController>().InitialiseEnemy();
            obj.SetActive(false);
            weaponRefs[x] = obj;
            x++;
        }
        StartCoroutine(SpawnPrimaryWeapon());
    }

    IEnumerator SpawnPrimaryWeapon()
    {
        yield return new WaitForSeconds(0.1f);
        weaponRefs[0].SetActive(true);
        currentActiveIndex = 0;
        currentWeapon = weaponRefs[0].GetComponent<WeaponController>();
    }
}
