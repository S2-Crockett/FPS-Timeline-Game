using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniObjective : MonoBehaviour
{
    [SerializeField] private Text MiniObjectiveNotificationText;
    [SerializeField] [TextArea] public string MiniNotificationMessage;
    [SerializeField] private Animator mininotificationAnim;
    private consoledefence consoledefencescript;
    public bool MiniObjectviveAppear;

    void Start()
    {
        consoledefencescript = GetComponent<consoledefence>();
        MiniObjectviveAppear = false;
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "ConsoleTrigger")
        {
            mininotificationAnim.Play("MiniObjectiveWindowFadeIn");
            MiniObjectiveNotificationText.text = "> ACTIVATE AND DEFEND THE CONSOLE!";
        }
        if (other.gameObject.tag == "ReachZoneObjective")
        {
            mininotificationAnim.Play("MiniObjectiveWindowFadeIn");
            MiniObjectiveNotificationText.text = "> REACH THE END OF THE ROAD";
        }
        if (other.gameObject.tag == "ReachZoneObjective2")
        {
            mininotificationAnim.Play("MiniObjectiveWindowFadeIn");
            MiniObjectiveNotificationText.text = "> TRAVERSE THE PLATFORMS";
        }
    }
}
