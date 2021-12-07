using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    public Select input;

    public Transform tableGoal;

    public bool overlapping = false;

    public Camera mainCamera;
    public Camera playerCamera;

    public GameObject player;
    public GameObject Screen;

    public UpgradeText text;


    enum STAGES
    {
        OUTSIDE,
        ENTERED,
        INSCREEN,
        ACTIVATEPORTAL,
        LEAVESCREEN,
        LEAVEPORTAL
    }

    private STAGES stage;

    void Start()
    {
        input = new Select();
        input.Move.Select.performed += e => activateTable();
        input.Move.Leave.performed += e => LeaveTable();
        input.Enable();

        stage = STAGES.OUTSIDE;
    }

    // Update is called once per frame
    void Update()
    {
        float step = 1f * Time.deltaTime;

        switch (stage)
        {
            case STAGES.OUTSIDE:
                {

                    break;
                }
            case STAGES.ENTERED:
                {
                    mainCamera.gameObject.SetActive(true);
                    player.SetActive(false);

                    mainCamera.transform.position = tableGoal.position;
                    mainCamera.transform.rotation = tableGoal.rotation;
                    if (Vector3.Distance(mainCamera.transform.position, tableGoal.position) < 1.0f)
                    {
                        stage = STAGES.INSCREEN;
                    }
                    break;
                }
            case STAGES.INSCREEN:
                {
                    Screen.SetActive(true);
                    text.selectable = true;
                    break;
                }
            
            case STAGES.LEAVESCREEN:
                {
                    mainCamera.gameObject.SetActive(false);
                    player.SetActive(true);
                    text.selectable = false;
                    stage = STAGES.OUTSIDE;

                    break;
                }
        }
    }



    private void LeaveTable()
    {
        if (stage == STAGES.INSCREEN && text.finished)
        {
            stage = STAGES.LEAVESCREEN;
        }
    }

    private void activateTable()
    {
        if (overlapping && stage == STAGES.OUTSIDE)
        {
            stage = STAGES.ENTERED;
        }
        if (stage == STAGES.INSCREEN && text.finished)
        {
            text.selectable = false;
            stage = STAGES.LEAVESCREEN;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            overlapping = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            overlapping = false;
        }
    }
}
