using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class consoledefence : MonoBehaviour
{
    //public GameObject ConsoleText;
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

    void Awake()
    {
        miniobjectivescript = GetComponent<MiniObjective>();

        //ConsoleText.SetActive(false);

        //defaultInput = new DefaultInput();

    }

    void OnTriggerEnter(Collider other)
    {      
        if (other.gameObject.tag == "Player")
        {
            //ConsoleText.SetActive(true);

            if (objectivereached == 0)
            {
                StartCoroutine(EnableNotification());
                objectivereached = 1;
            }

            /*if (playerscript.InteractKeyPressed == true)
            {
                Debug.Log("Activating Terminal!");
            }*/
        }
    }
        
    void OnTriggerExit(Collider other)
    {
        //ConsoleText.SetActive(false);

        if (other.gameObject.tag == "Player" && removeAfterExit)
        {
            RemoveNotification();            
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

