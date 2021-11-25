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
    public GameObject[] floor;


    private WeaponHandler weaponHandler;
    public int index = 0;


    RaycastHit hit;

    private int layerMask = 1;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        weaponHandler = player.GetComponent<WeaponHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Physics.Raycast(player.transform.position, player.transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, layerMask))
        {
            ChangeObjects();
        }
    }

    private void ChangeObjects()
    {
        CheckZone();

        ChangeArrayObjects(floor.Length, floor, zone[index].timezone1);
        ChangeArrayObjects(enemy.Length, enemy, zone[index].material);

    }

    private void ChangeArrayObjects(int length, GameObject[] gameObject, Material material1)
    {
        for (int i = 0; i < length; i++)
        {
            gameObject[i].gameObject.GetComponent<MeshRenderer>().material = material1;
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
