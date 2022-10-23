using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update
    // helper field to set the inventory object
    private static Inventory partyInventory;
    public GameObject inventoryPanel;
    [SerializeField] PlayerStatsController statsSystem;
    [SerializeField] private GameObject inventoryTab;
    
    private static List<GameObject> itemButtons = new List<GameObject>();
    public GameObject btnTemplate;

    void Start()
    {
        if (gameObject.tag == "Tab")
        {
            // set the inventory object
            partyInventory = statsSystem.partyInventory;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInventorySelected()
    {
        // starting y of the button
        float yPosition = -50f;
        const float startXPos = 300f;
        Button buttonComponent;
        Navigation nav;
        // populate the inventory screen with buttons for items
        for (int itemIndex = 0; itemIndex < partyInventory.GetNoOfItems(); itemIndex++)
        {
            // instantiate the button
            itemButtons.Add(Instantiate(btnTemplate));
            // parent the item to the inventory panel
            itemButtons[itemIndex].transform.SetParent(inventoryPanel.transform);
            // position the button
            itemButtons[itemIndex].GetComponent<RectTransform>().anchoredPosition = new Vector2(startXPos, yPosition);
            // lower the y position
            yPosition -= 25f;
            // set the text of the button to the name of the item
            itemButtons[itemIndex].GetComponentInChildren<TextMeshProUGUI>().text = partyInventory.GetItem(itemIndex).GetName();
            Debug.Log("Added item");
        }

        // for loop to set up the navigation for each button object
        if (itemButtons.Count != 0)
        {
            for (int buttonIndex = 0; buttonIndex < itemButtons.Count; buttonIndex++)
            {
                // check which button is having navigation set up
                if (buttonIndex == 0)
                {
                    // first button, set top navigation to the inventory "tab"
                    buttonComponent = itemButtons[buttonIndex].GetComponent<Button>();
                    nav = buttonComponent.navigation;
                    // set the selection field
                    nav.selectOnUp = inventoryTab.GetComponent<Button>();
                    // check if not at end
                    if (!(buttonIndex == itemButtons.Count - 1))
                    {
                        // not at end. set down navigation to item below
                        nav.selectOnDown = itemButtons[buttonIndex].GetComponent<Button>();
                    }
                }
                else if (buttonIndex > 0 && buttonIndex < itemButtons.Count - 1)
                {
                    // mid area of inventory list. must assign navigation above and below
                    // set navigation above
                    buttonComponent = itemButtons[buttonIndex].GetComponent<Button>();
                    // nav is used as a middle ground for assigning navigation
                    nav = buttonComponent.navigation;
                    nav.selectOnUp = itemButtons[buttonIndex - 1].GetComponent<Button>();
                    // set navigation below
                    nav.selectOnDown = itemButtons[buttonIndex + 1].GetComponent<Button>();
                }
                else
                {
                    // at final button. set down navigation to first item and up navigation to previous item
                    buttonComponent = itemButtons[buttonIndex].GetComponent<Button>();
                    nav = buttonComponent.navigation;
                    nav.selectOnDown = itemButtons[0].GetComponent<Button>();
                    // set upwards navigation
                    nav.selectOnUp = itemButtons[buttonIndex - 1].GetComponent<Button>();
                }
            }
        }

    }

    public void UseItemFromInventoryScreen()
    {
        // find this button in the list, via a linear search
        int buttonPos = 0;
        for (int counter = 0; counter < itemButtons.Count; counter++)
        {
            // check if current position is the correct button
            if (gameObject == itemButtons[counter])
            {
                // button is found. break here
                buttonPos = counter;
                break;
            }
        }
        // found the position of the button. use this item and destroy this button
        FindObjectOfType<PlayerStatsController>().HealPlayer(buttonPos);
        // loop through the button list and shuffle the remaining items up
        for (int offsetIndex = buttonPos + 1; offsetIndex < itemButtons.Count; offsetIndex++)
        {
            itemButtons[offsetIndex].GetComponent<RectTransform>().anchoredPosition += new Vector2(0f, 25f);
        }
        // remove button from list and destroy the gameobject
        itemButtons.RemoveAt(buttonPos);
        Destroy(gameObject);
    }
}