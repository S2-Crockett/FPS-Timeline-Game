using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Table : MonoBehaviour
{

    public Select input;

    public bool inScreen = false;
    public bool overlapping = false;

    public CinemachineVirtualCamera mainCamera;
    public CinemachineVirtualCamera playerCamera;
        public CinemachineVirtualCamera portalCamera;

    public GameObject player;
    public GameObject Particles;
    public GameObject Screen;

    public Transform reset;
    public GameObject Weapons;

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
        Particles.SetActive(false);

        stage = STAGES.OUTSIDE;

        CameraManager.Register(mainCamera);
        CameraManager.Register(playerCamera);
        CameraManager.Register(portalCamera);
    }

    // Update is called once per frame
    void Update()
    {
        float step = 1f * Time.deltaTime;

        switch (stage)
        {
            case STAGES.OUTSIDE:
            {
                //Weapons.SetActive(true);
                Screen.SetActive(false);
                    CameraManager.SwitchCamera(playerCamera);
                    break;
            }
            case STAGES.ENTERED:
            {
                //Weapons.SetActive(false);
                Screen.SetActive(true);
                text.show = true;
                    CameraManager.SwitchCamera(mainCamera);
                    stage = STAGES.INSCREEN;
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
                    StartCoroutine(StartPortal());
                    break;
            }
            case STAGES.LEAVEPORTAL:
            {
                    timer -= Time.deltaTime;
                    if(timer <= 0)
                    {
                        text.selectable = false;
                        player.transform.position = reset.position;
                        stage = STAGES.OUTSIDE;
                    }
                    break;
            }
            case STAGES.LEAVESCREEN:
                {
                    text.selectable = false;
                    stage = STAGES.OUTSIDE;
      
                    break;
                }
        }
    }

    IEnumerator StartPortal()
    {
        CameraManager.SwitchCamera(portalCamera);
        text.selectable = false;
        yield return new WaitForSeconds(2.0f);
        stage = STAGES.LEAVEPORTAL;
        Screen.SetActive(false); 
        Particles.SetActive(true);
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
