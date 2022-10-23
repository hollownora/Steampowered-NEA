using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTrigger : MonoBehaviour
{
    // Start is called before the first frame update

    [HideInInspector] public GameObject enemy;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerBattle()
    {
        FindObjectOfType<GameManager>().TriggerBattle(enemy);
    }


    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Enemies")
        {
            enemy = hit.gameObject;
            TriggerBattle();
        }
    }
}
