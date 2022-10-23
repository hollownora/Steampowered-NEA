using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public GameObject npcParent;
    public GameObject indicator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // player is in range for talking
            // show indicator
            indicator.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            indicator.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // check if z is pressed
            if (Input.GetKeyUp(KeyCode.Z) && !npcParent.GetComponent<DialoguePlayer>().playingDialogue)
            {
                // call the playdialogue function
                npcParent.GetComponent<DialoguePlayer>().PlayDialogue();
            }
        }
    }
}
