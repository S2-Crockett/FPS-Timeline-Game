using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKillingObjective : MonoBehaviour
{
    private Objective objectivescript;
    private EnemyDead enemydead;

    // Start is called before the first frame update
    void Start()
    {
        enemydead = GetComponent<EnemyDead>();
    }

    // Update is called once per frame
    /*void Update()
    {
        if (enemydead.zone1enemiescount == 0)
        {
            Debug.Log("enemies are all dead");
            objectivescript.iscompleted = true;
        }
    }*/
}
