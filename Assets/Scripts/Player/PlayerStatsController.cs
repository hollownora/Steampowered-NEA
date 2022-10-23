using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsController : MonoBehaviour
{
    // system to control player data
    // stats variables
    public int hitPoints;
    public int defence;
    public int tankPressure;
    public int experience;
    public int attack;

    // inventory object. shared between party members
    public Inventory partyInventory;
    public Backpack backpack;

    // level caps for each stat
    public int hitPointsCap;
    public int defenceCap;
    public int tankPressureCap;
    public int experienceToNextLevel;

    // flag for checking if player is knocked out in battle
    public bool isPlayerDown = false;

    // Start is called before the first frame update
    void Start()
    {
        // create a test inventory with a few items
        partyInventory = new Inventory();
        partyInventory.AddItem(new Item("Apple", ItemType.healing, 5));
        partyInventory.AddItem(new Item("Hotdog", ItemType.healing, 7));
        partyInventory.AddItem(new Item("Hotdog", ItemType.healing, 7));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HealPlayer(int itemID)
    {
        Item itemToUse = partyInventory.UseItem(itemID);
        // check if the item heals to full hp
        if (itemToUse.GetHealValue() + hitPoints >= hitPointsCap)
        {
            // set hp to full
            hitPoints = hitPointsCap;
        }
        else
        {
            // heal the player by the appropriate amount
            hitPoints += itemToUse.GetHealValue();
        }
    }
}
