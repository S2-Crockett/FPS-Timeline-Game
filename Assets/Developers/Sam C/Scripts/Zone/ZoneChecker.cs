using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneChecker : MonoBehaviour
{

    [Header("Zones")]
    public Zone[] zone;

    private GameObject player;

    [Header("Enemies")]
    public GameObject[] enemy;

    [Header("Environment")]
    public GameObject floor;




    private WeaponHandler weaponHandler;


    public int index = 0;
    private int prevIndex = 0;



    private bool change = true;
    GameObject[] newGameObject;
    GameObject newFloorObject;


    RaycastHit hit;

    private int layerMask = 1;
    // Start is called before the first frame update
    void Start()
    {
        newGameObject = new GameObject[enemy.Length];
        prevIndex = index;
        player = GameObject.Find("Player");
        weaponHandler = player.GetComponent<WeaponHandler>();
        newFloorObject = Instantiate(zone[index].timezone1, floor.transform.position, floor.transform.rotation);

        for(int i = 0; i < zone.Length; i++)
        {
            zone[i].timezone1.tag = zone[0].name;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if(prevIndex != index)
        {
            prevIndex = index;
            change = true;
        }
        if(Physics.Raycast(player.transform.position, player.transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, layerMask))
        {
            ChangeObjects();
        }
    }

    private void ChangeObjects()
    {
        CheckZone();
        CreateNewObjects();
    }



    private void CreateNewObjects()
    {
        if (change)
        {
            weaponHandler.WeaponIndex = zone[index].weaponIndex;
            weaponHandler.change = true;
            weaponHandler.SwapWeapon(zone[index].weaponIndex);
            Destroy(newFloorObject);
            newFloorObject = Instantiate(zone[index].timezone1, floor.transform.position, floor.transform.rotation);
            for (int i = 0; i < enemy.Length; i++)
            {
                Destroy(newGameObject[i]);
            }
            for (int i = 0; i < enemy.Length; i++)
            {
                newGameObject[i] = Instantiate(zone[index].material, enemy[i].transform.position, enemy[i].transform.rotation);
            }

            change = false;
        }
    }

    private void CheckZone()
    {
        for (int i = 0; i < zone.Length; i++)
        {
            if(hit.transform.gameObject.tag == zone[i].name)
            {
                index = i;
            }
        }
    }
}
