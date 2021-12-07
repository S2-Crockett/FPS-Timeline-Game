using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class consoledefence : MonoBehaviour
{
    public GameObject ConsoleText;
    private MiniObjective miniobjectivescript;
    [SerializeField] public Text ObjectiveNotificationUIText;
    [SerializeField] [TextArea] private string notificationmessage;
    [SerializeField] private bool removeAfterExit = false;
    [SerializeField] private bool disableAfterTimer = false;
    [SerializeField] float disabletimer = 1.0f;
    [SerializeField] private Animator notificationAnim;
    private DefaultInput defaultInput;
    private PlayerController playerscript;
    public Select input;
    private int objectivereached = 0;

    public bool InteractKeyPressed = false;
    bool inZone = false;


    void Awake()
    {
        miniobjectivescript = GetComponent<MiniObjective>();


        playerscript = GameObject.Find("Player").GetComponent<PlayerController>();


        defaultInput = new DefaultInput();

        defaultInput.Player.Interact.performed += e => Interact();

        defaultInput.Enable();

        if (ConsoleText != null)
        {
            ConsoleText.SetActive(false);
        }

        //defaultInput = new DefaultInput();

    }

    void Update()
    {

    }

    void Interact()
    {
        if (inZone)
        {
            InteractKeyPressed = true;
            if (ConsoleText != null)
            {
                ConsoleText.SetActive(false);
            }
        }
    }


    void OnTriggerStay(Collider other)
    {
        if (this.gameObject.tag == "ConsoleTrigger")
        {
            inZone = true;
            if (ConsoleText != null && inZone)
            {
                ConsoleText.SetActive(true);
            }
            if (objectivereached == 0 && InteractKeyPressed)
            {
                StartCoroutine(EnableNotification());
                objectivereached = 1;
                inZone = false;
            }
        }
        else
        {
            if (objectivereached == 0)
            {
                StartCoroutine(EnableNotification());
                objectivereached = 1;
                inZone = false;
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (ConsoleText != null)
        {
            ConsoleText.SetActive(false);
        }
        if (other.gameObject.tag == "Player" && removeAfterExit)
        {
            RemoveNotification();
            inZone = false;
        }
    }

    IEnumerator EnableNotification()
    {
        notificationAnim.Play("ObjectiveWindowFadeIn");
        ObjectiveNotificationUIText.text = notificationmessage;

        if (disableAfterTimer)
        {
            yield return new WaitForSeconds(disabletimer);
            RemoveNotification();           
        }
    }

    void RemoveNotification()
    {
        notificationAnim.Play("ObjectiveWindowFadeOut");
    }
}

