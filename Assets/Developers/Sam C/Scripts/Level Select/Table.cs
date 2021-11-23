using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{

    public Select input;

    public Transform tableGoal;
    public Transform portalGoal;

    public bool inScreen = false;
    public bool overlapping = false;

    public Camera mainCamera;
    public Camera playerCamera;

    public GameObject player;
    public GameObject Particles;
    public GameObject Screen;

    public TextDisplay text;

    private float timer;

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
                    if(Vector3.Distance(mainCamera.transform.position, tableGoal.position) < 1.0f)
                    {
                        text.level = 1;
                        text.show = true;
                        stage = STAGES.INSCREEN;
                    }
                    break;
            }
            case STAGES.INSCREEN:
            {
                    Screen.SetActive(true);
                    Particles.SetActive(false);
                    text.selectable = true;
                    timer = 3.0f;
                    break;
            }
            case STAGES.ACTIVATEPORTAL:
            {
                    mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, portalGoal.rotation, step * 3.0f);
                    mainCamera.transform.position = Vector3.Slerp(mainCamera.transform.position, portalGoal.position, step);
                    if (mainCamera.transform.rotation.y == portalGoal.rotation.y)
                    {
                        Particles.SetActive(true);
                        text.selectable = false;
                        stage = STAGES.LEAVEPORTAL;
                        Screen.SetActive(false);
                    }
                    break;
            }
            case STAGES.LEAVEPORTAL:
            {
                    timer -= Time.deltaTime;
                    if(timer <= 0)
                    {
                        mainCamera.gameObject.SetActive(false);
                        player.SetActive(true);
                        text.selectable = false;
                        stage = STAGES.OUTSIDE;
                    }
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
        if(stage == STAGES.INSCREEN && text.finished)
        {
            text.ClearText();
            stage = STAGES.LEAVESCREEN;
        }
    }

    private void activateTable()
    {
        if(overlapping && stage == STAGES.OUTSIDE)
        {
            stage = STAGES.ENTERED;
        }
        if(stage == STAGES.INSCREEN && text.finished)
        {
            text.ClearText();
            text.selectable = false;
            stage = STAGES.ACTIVATEPORTAL;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
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
