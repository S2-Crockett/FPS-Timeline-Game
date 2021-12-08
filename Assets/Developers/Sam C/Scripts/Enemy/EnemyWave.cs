using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyWave : MonoBehaviour
{
    // Start is called before the first frame update

    public EnemyInfo[] enemyObjectZone1;
    public EnemyInfo[] enemyObjectZone2;


    public Zone[] zone;

    [Header("Enemies")]
    public GameObject[] enemies;
    Vector3[] newEnemyPos;
    Quaternion[] newEnemyRot;

    public float dead = 0;
    public int waves = 3;
    public float timer = 10;

    Quaternion dead_ = new Quaternion(0, 90, 90, 0);
    bool check = false;
    int index_;


    void Awake()
    {
        zone = GameObject.Find("ZoneChecker").GetComponent<ZoneChecker>().zone;
        newEnemyPos = new Vector3[enemies.Length];
        newEnemyRot = new Quaternion[enemies.Length];
        for (int i = 0; i < enemies.Length; i++)
        {
            enemyObjectZone1[i].enemyObj = zone[0].enemy;
            enemyObjectZone2[i].enemyObj = zone[1].enemy;
            enemyObjectZone1[i].enemyObj = Instantiate(zone[0].enemy, enemies[i].transform.position, enemies[i].transform.rotation);
            enemyObjectZone2[i].enemyObj = Instantiate(zone[1].enemy, enemies[i].transform.position, enemies[i].transform.rotation);
            enemyObjectZone1[i].enemyObj.SetActive(false);
            enemyObjectZone2[i].enemyObj.SetActive(false);
            enemyObjectZone1[i].defaultEnemyMat = enemyObjectZone1[i].enemyObj.GetComponent<MeshRenderer>().material;
            enemyObjectZone2[i].defaultEnemyMat = enemyObjectZone2[i].enemyObj.GetComponent<MeshRenderer>().material;
            newEnemyPos[i] = enemyObjectZone1[i].enemyObj.transform.position;
            newEnemyRot[i] = enemyObjectZone1[i].enemyObj.transform.rotation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckEnemy();

        if(waves == 0 && dead == 6)
        { 
                SceneManager.LoadScene("Start_Scene");
        }

    }

    public void ResetEnemy(int index)
    {
        index_ = index;
        timer -= Time.deltaTime;
        if (timer <= 0 && waves != 0)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemyObjectZone1[i].dead)
                {
                    Destroy(enemyObjectZone1[i].enemyObj);
                    enemyObjectZone1[i].enemyObj = Instantiate(zone[0].enemy, enemies[i].transform.position, enemies[i].transform.rotation);
                    enemyObjectZone1[i].enemyObj.GetComponent<ShootingTarget>().dead = false;
                    enemyObjectZone1[i].dead = false;
                }
                if (enemyObjectZone2[i].dead)
                {
                    Destroy(enemyObjectZone2[i].enemyObj);
                    enemyObjectZone2[i].enemyObj = Instantiate(zone[1].enemy, enemies[i].transform.position, enemies[i].transform.rotation);
                    enemyObjectZone2[i].enemyObj.GetComponent<ShootingTarget>().dead = false;
                    enemyObjectZone2[i].dead = false;
                }
            }
            check = true;
            waves -= 1;
            dead = 0;
            timer = 20.0f;
        }
    }
    public IEnumerator ChangeEnemyObj(int index_, Material Glitch)
    {
        if (!enemyObjectZone1[index_].enemyObj.activeSelf)
        {
            newEnemyPos[index_] = enemyObjectZone2[index_].enemyObj.transform.position;
            if (!enemyObjectZone2[index_].dead)
            {
                newEnemyRot[index_] = enemyObjectZone2[index_].enemyObj.transform.rotation;
            }
            if (enemyObjectZone1[index_].dead)
            {
                newEnemyPos[index_] = enemyObjectZone1[index_].deathPos;
            }
            enemyObjectZone2[index_].enemyObj.GetComponent<MeshRenderer>().material = Glitch;
            yield return new WaitForSeconds(0.2f);
            enemyObjectZone1[index_].enemyObj.SetActive(true);
            enemyObjectZone2[index_].enemyObj.SetActive(false);
            enemyObjectZone2[index_].enemyObj.GetComponent<MeshRenderer>().material = enemyObjectZone2[index_].defaultEnemyMat;
            enemyObjectZone1[index_].enemyObj.transform.position = newEnemyPos[index_];
            enemyObjectZone1[index_].enemyObj.transform.rotation = newEnemyRot[index_];
        }
        else if (!enemyObjectZone2[index_].enemyObj.activeSelf)
        {
            newEnemyPos[index_] = enemyObjectZone1[index_].enemyObj.transform.position;
            if (!enemyObjectZone1[index_].dead)
            {
                newEnemyRot[index_] = enemyObjectZone1[index_].enemyObj.transform.rotation;
            }
            if (enemyObjectZone2[index_].dead)
            {
                newEnemyPos[index_] = enemyObjectZone2[index_].deathPos;
            }
            enemyObjectZone1[index_].enemyObj.GetComponent<MeshRenderer>().material = Glitch;
            yield return new WaitForSeconds(0.2f);
            enemyObjectZone2[index_].enemyObj.SetActive(true);
            enemyObjectZone1[index_].enemyObj.SetActive(false);
            enemyObjectZone1[index_].enemyObj.GetComponent<MeshRenderer>().material = enemyObjectZone1[index_].defaultEnemyMat;
            enemyObjectZone2[index_].enemyObj.transform.position = newEnemyPos[index_];
            enemyObjectZone2[index_].enemyObj.transform.rotation = newEnemyRot[index_];
        }
    }
    private void CheckEnemy()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemyObjectZone1[i].enemyObj.GetComponent<Enemies>().dead)
            {
                if (!enemyObjectZone1[i].dead)
                {
                    enemyObjectZone1[i].deathPos = enemyObjectZone1[i].enemyObj.transform.position;
                    enemyObjectZone1[i].deathPos.y -= 0.3f;
                    dead += 1;
                }
                enemyObjectZone1[i].dead = true;
                enemyObjectZone1[i].enemyObj.transform.position = enemyObjectZone1[i].deathPos;
                enemyObjectZone1[i].enemyObj.GetComponent<LineOfSight>().enabled = false;
                enemyObjectZone1[i].enemyObj.GetComponent<Move>().enabled = false;
                enemyObjectZone1[i].enemyObj.transform.rotation = dead_;
            }
            if (enemyObjectZone2[i].enemyObj.GetComponent<Enemies>().dead)
            {
                if (!enemyObjectZone2[i].dead)
                {
                    enemyObjectZone2[i].deathPos = enemyObjectZone2[i].enemyObj.transform.position;
                    enemyObjectZone2[i].deathPos.y -= 0.3f;
                    dead += 1;
                }
                enemyObjectZone2[i].dead = true;
                enemyObjectZone2[i].enemyObj.transform.position = enemyObjectZone1[i].deathPos;
                enemyObjectZone2[i].enemyObj.GetComponent<LineOfSight>().enabled = false;
                enemyObjectZone2[i].enemyObj.GetComponent<Move>().enabled = false;
                enemyObjectZone2[i].enemyObj.transform.rotation = dead_;
            }

            if(check)
            {
                if (index_ == 0)
                {
                    enemyObjectZone1[i].enemyObj.SetActive(true);
                    enemyObjectZone2[i].enemyObj.SetActive(false);
                }
                else if (index_ == 1)
                {
                    enemyObjectZone1[i].enemyObj.SetActive(false);
                    enemyObjectZone2[i].enemyObj.SetActive(true);
                }
            }
        }
        check = false;
    }
}
