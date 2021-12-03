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

    [Header("Materials")]
    public Material Glitch;


    private WeaponHandler weaponHandler;


    public int index = 0;
    private int prevIndex = 0;
    private float random = 0;



    private bool change = false;


    GameObject[] newGameObject;
    Material[] oldMaterial;
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
        for (int i = 0; i < enemy.Length; i++)
        {
            newGameObject[i] = Instantiate(zone[index].material, enemy[i].transform.position, enemy[i].transform.rotation);
        }

        weaponHandler.WeaponIndex = zone[index].weaponIndex;
        weaponHandler.change = true;
        weaponHandler.SwapWeapon(zone[index].weaponIndex);
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

    IEnumerator ChangeEnvironmentObj(int index_)
    {
        ChangeMat(enemy, newGameObject, newFloorObject, index_);

        yield return new WaitForSeconds(random);

         Destroy(newGameObject[index_]);
         newGameObject[index_] = Instantiate(zone[index].material, enemy[index_].transform.position, enemy[index_].transform.rotation);
    }

    IEnumerator ChangeFloor()
    {
        random = Random.Range(0.3f, 1f);

        newFloorObject.GetComponent<MeshRenderer>().material = Glitch;

        yield return new WaitForSeconds(random);

        Destroy(newFloorObject);
        newFloorObject = Instantiate(zone[index].timezone1, floor.transform.position, floor.transform.rotation);
    }


    private void ChangeMat(GameObject[] enemy_, GameObject[] newObj, GameObject newFloorObj, int index)
    {
        for (int c = 0; c < newObj[index].transform.childCount; c++)
        {
            newObj[index].transform.GetChild(c).GetComponent<MeshRenderer>().material = Glitch;
            random = Random.Range(0.3f, 1f);
        }
    }

    private void CreateNewObjects()
    {
        if (change)
        {
            for (int i = 0; i < enemy.Length; i++)
            {
                StartCoroutine(ChangeEnvironmentObj(i));
            }
            StartCoroutine(ChangeFloor());

            weaponHandler.WeaponIndex = zone[index].weaponIndex;
            weaponHandler.change = true;
            weaponHandler.SwapWeapon(zone[index].weaponIndex);

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
