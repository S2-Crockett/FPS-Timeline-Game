using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Objective : MonoBehaviour
{
    public Text objective;
    public GameObject objectivebackground;
    public bool iscompleted;

    void Awake()
    {
        objective.text = " ";
        objectivebackground.SetActive(false);
    }

    void Update()
    {
        
    }

    IEnumerator objectivevisability()
    {
        yield return new WaitForSeconds(5);
        objective.text = " ";
        objectivebackground.SetActive(false);
    }

    void CompletedObjective()
    {
        objective.text = " ";
        objectivebackground.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {       
        if (other.gameObject.tag == "Objective1")
        {
            objective.text = "> Escape to the end of the road";
            objectivebackground.SetActive(true);         
        }
        if (other.gameObject.tag == "Objective1complete")
        {
            objective.text = " ";
            objectivebackground.SetActive(false);
        }
        if (other.gameObject.tag == "Objective2")
        {
            objective.text = "> Reach the zone ahead";
            objectivebackground.SetActive(true);
        }
        if (other.gameObject.tag == "Objective3")
        {
            objective.text = "> Eliminate the enemies";
            objectivebackground.SetActive(true);
        }
        if (other.gameObject.tag == "Objective4")
        {
            objective.text = "> Enter the portal";
            objectivebackground.SetActive(true);
        }
        /* else)
         {
             objective.text = " ";
             objectivebackground.SetActive(false);
         }*/


    }
}
