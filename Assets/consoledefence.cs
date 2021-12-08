using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class consoledefence : MonoBehaviour
{
    //GameObjects//
    public GameObject ConsoleText;
    public GameObject ConsoleNotActiveText;
    public GameObject ConsoleActiveText;
    public GameObject ConsoleLight;
    public GameObject[] SpotLights = new GameObject[5];
    public GameObject[] ButtonsPressed = new GameObject[3];
    public GameObject[] EnemySpawnPoints = new GameObject[3];

    //Vector//
    

    //Scripts//
    private MiniObjective miniobjectivescript;
    private DefaultInput defaultInput;
    private PlayerController playerscript;
    private EnemySpawner enemyspawner;

    //Text//
    [SerializeField] public Text ObjectiveNotificationUIText;
    [SerializeField] [TextArea] private string notificationmessage;

    //Bools//
    [SerializeField] private bool removeAfterExit = false;
    [SerializeField] private bool disableAfterTimer = false;
    public bool InteractKeyPressed = false;
    bool inZone = false;

    //Int//
    private int objectivereached = 0;

    //Floats//
    [SerializeField] float disabletimer = 1.0f;

    //Animator//
    [SerializeField] private Animator notificationAnim;

    public Select input;

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
        SpotLights[0].SetActive(false);
        SpotLights[1].SetActive(false);
        SpotLights[2].SetActive(false);
        SpotLights[3].SetActive(false);
        SpotLights[4].SetActive(false);

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
                //enemyspawner.SpawnEnemies();

                if (inZone == false)
                {
                    ConsoleText.SetActive(false);
                }

                ConsoleLight.SetActive(true);
                SpotLights[0].SetActive(true);
                SpotLights[1].SetActive(true);
                SpotLights[2].SetActive(true);
                SpotLights[3].SetActive(true);
                SpotLights[4].SetActive(true);
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

