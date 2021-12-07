using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class consoledefence : MonoBehaviour
{
    public GameObject ConsoleText;
    public GameObject ConsoleNotActiveText;
    public GameObject ConsoleActiveText;
    public GameObject ConsoleLight;
    public GameObject[] ButtonsPressed = new GameObject[3];
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

        ConsoleNotActiveText.SetActive(true);
        ConsoleActiveText.SetActive(false);
        ConsoleLight.SetActive(false);

        if (ConsoleText != null)
        {
            ConsoleText.SetActive(false);
        }
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
                ConsoleNotActiveText.SetActive(false);
                ConsoleActiveText.SetActive(true);
                StartCoroutine(EnableNotification());
                ConsoleText.SetActive(false);
                objectivereached = 1;
                inZone = false;
                ConsoleLight.SetActive(true);
                ButtonsPressed[1].transform.position += new Vector3(0, 0, 0);
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

