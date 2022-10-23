using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattleBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hit");
        if (collision.gameObject.tag == "Enemies")
        {
            // hit an enemy, trigger battle
            // tell the game manager to enter a battle
            Debug.Log("Trigger");
            FindObjectOfType<GameManager>().TriggerBattle(collision.gameObject);
        }
    }
}
