using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartWave : MonoBehaviour
{
    private DefaultInput defaultInput;
    public EnemyWave wave;
    private ZoneChecker zoneChecker;

    bool inside = false;
    void Start()
    {
        zoneChecker = GameObject.Find("ZoneChecker").GetComponent<ZoneChecker>();
        defaultInput = new DefaultInput();

        defaultInput.Player.Interact.started += e => Press();

        defaultInput.Enable();
    }

    void Press()
    {
        print("PRessed");
        if(inside)
        {
            wave.enabled = true;
            for (int i = 0; i < wave.enemies.Length; i++)
            {
                zoneChecker.enemyWave = wave;
                wave.enemyObjectZone1[i].enemyObj.SetActive(true);
            }
        }
    }



    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            inside = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inside = false;
        }
    }
}
