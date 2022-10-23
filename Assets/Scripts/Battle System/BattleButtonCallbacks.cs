using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleButtonCallbacks : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayEnemiesCallback()
    {
        FindObjectOfType<BattleUIController>().DisplayEnemies();
    }

    public void DisplayItemsCallback()
    {
        Debug.Log("Showing Items");
        FindObjectOfType<BattleUIController>().DisplayInventory();
    }

    public void DisplayPartyMembersCallback()
    {

    }

    public void DisplaySkillsCallback()
    {
        FindObjectOfType<BattleUIController>().DisplayMiscMenu();
    }

    public void DisplayMiscOptionsCallback()
    {
        FindObjectOfType<BattleUIController>().DisplayMiscMenu();
    }
}
