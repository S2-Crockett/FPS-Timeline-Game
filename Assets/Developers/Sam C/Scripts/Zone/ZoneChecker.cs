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


    private int index = 0;
    private int prevIndex = 0;



    private bool change = true;
    GameObject[] newGameObject;


    RaycastHit hit;

    private int layerMask = 1;
    // Start is called before the first frame update
    void Start()
    {
        newGameObject = new GameObject[enemy.Length];
        prevIndex = index;
        player = GameObject.Find("Player");
        weaponHandler = player.GetComponent<WeaponHandler>();
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
        ChangeArrayObjects(floor.Length, floor, zone[index].timezone1);
        CreateNewObjects();
    }

    private void ChangeArrayObjects(int length, GameObject[] gameObject, Material material1)
    {
        for (int i = 0; i < length; i++)
        {
            gameObject[i].gameObject.GetComponent<MeshRenderer>().material = material1;
        }
    }

    private void CreateNewObjects()
    {
        if (change)
        {
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
