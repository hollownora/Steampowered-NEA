using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class BattleUIController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject buttonTemplate;
    public  GameObject battleGUIMenu;
    [SerializeField] private TextMeshProUGUI hpLabel;
    [SerializeField] private TextMeshProUGUI psiLabel;
    private Inventory inventory;
    private List<GameObject> buttonList = new List<GameObject>();

    void Start()
    {
        inventory = FindObjectOfType<PlayerStatsController>().partyInventory;
    }

    // Update is called once per frame
    void Update()
    {
        // use this to refresh the HUD every frame
        //hpLabel.text = "HP: " + FindObjectOfType<BattleManager>().partyMembers[0].GetComponent<PlayerStatsController>().hitPoints + "/" + FindObjectOfType<BattleManager>().partyMembers[0].GetComponent<PlayerStatsController>().hitPointsCap;
        //psiLabel.text = "PSI: " + FindObjectOfType<BattleManager>().partyMembers[0].GetComponent<PlayerStatsController>().tankPressure + "/" + FindObjectOfType<BattleManager>().partyMembers[0].GetComponent<PlayerStatsController>().tankPressureCap;
    }

    public void DisplayInventory()
    {
        // clear the panel
        FindObjectOfType<BattleManager>().captionText.gameObject.SetActive(false);
        ClearBattleMenu();

        for (int btnIndex = 0; btnIndex < inventory.GetNoOfItems(); btnIndex++)
        {
            // create the button
            buttonList.Add(Instantiate(buttonTemplate));
            // set the text to the name of the item
            buttonList[btnIndex].GetComponentInChildren<TextMeshProUGUI>().text = inventory.GetItem(btnIndex).GetName();
            Debug.Log(buttonList[btnIndex].GetComponentInChildren<TextMeshProUGUI>().text);
            // set the callback to use the item connected to that button - parameters csn be used by using () => fn(par)
            buttonList[btnIndex].GetComponent<Button>().onClick.AddListener(MakeButton(ButtonType.Targets, btnIndex));
            // make button child of menu
            buttonList[btnIndex].transform.SetParent(battleGUIMenu.transform);
        }
    }

    public void DisplayEnemies()
    {
        ClearBattleMenu();
        FindObjectOfType<BattleManager>().captionText.gameObject.SetActive(false);

        for (int enemyIndex = 0; enemyIndex < FindObjectOfType<BattleManager>().enemies.Count; enemyIndex++)
        {
            buttonList.Add(Instantiate(buttonTemplate));
            buttonList[enemyIndex].GetComponentInChildren<TextMeshProUGUI>().text = FindObjectOfType<BattleManager>().enemies[enemyIndex].name;
            buttonList[enemyIndex].GetComponent<Button>().onClick.AddListener(MakeButton(ButtonType.Enemy, enemyIndex));
            // make button child of the menu box
            buttonList[enemyIndex].transform.SetParent(battleGUIMenu.transform);
            buttonList[enemyIndex].transform.localScale = new Vector3(1, buttonList[enemyIndex].transform.localScale.y, buttonList[enemyIndex].transform.localScale.z);
        }
    }

    public void DisplayMiscMenu()
    {
        ClearBattleMenu();
        FindObjectOfType<BattleManager>().captionText.gameObject.SetActive(false);
        UnityAction[] miscOps =
        {
            () => FindObjectOfType<BattleManager>().FleeBattle(),
            () => FindObjectOfType<BattleManager>().IncrementTurn()
        };

        string[] operationNames =
        {
            "Flee Battle",
            "Skip Turn"
        };

        // set up the misc options buttons

        for (int miscBtnIndex = 0; miscBtnIndex < miscOps.Length; miscBtnIndex++)
        {
            buttonList.Add(Instantiate(buttonTemplate));
            buttonList[miscBtnIndex].GetComponentInChildren<TextMeshProUGUI>().text = operationNames[miscBtnIndex];
            buttonList[miscBtnIndex].GetComponent<Button>().onClick.AddListener(miscOps[miscBtnIndex]);
        }
    }

    public void ClearBattleMenu()
    {
        for (int childIndex = 0; childIndex < buttonList.Count; childIndex++)
        {
            Destroy(buttonList[childIndex]);
        }
        buttonList.Clear();
    }

    public void DisplayItemTargets(int itemNo, bool targetParty=true)
    {
        // method to display targets of inventory items
        ClearBattleMenu();
        if (targetParty)
        {
            Debug.Log("length " + FindObjectOfType<BattleManager>().partyMembers.Count);
            // create buttons to list party members
            for (int partyMemberIndex = 0; partyMemberIndex < FindObjectOfType<BattleManager>().partyMembers.Count; partyMemberIndex++)
            {
                // create button from template
                buttonList.Add(Instantiate(buttonTemplate));
                // set text to party member name
                buttonList[partyMemberIndex].GetComponentInChildren<TextMeshProUGUI>().text = FindObjectOfType<BattleManager>().partyMembers[partyMemberIndex].name;
                // set callback to use the item
                //buttonList[partyMemberIndex].GetComponent<Button>().onClick.AddListener();
                // make button child of the menu
                buttonList[partyMemberIndex].transform.SetParent(battleGUIMenu.transform);
                buttonList[partyMemberIndex].GetComponent<Button>().onClick.AddListener(MakeButton(ButtonType.Items, itemNo, partyMemberIndex));
                Debug.Log("Button set up");
            }
            Debug.Log("Targets shown");
        }
    }

    private enum BattleUIStates
    {
        TopMenu,
        SelectingPartyMember,
        SelectingEnemy,
        SelectingItem,
        SelectingSkill
    }

    // credit to Alex Martelli on StackOverflow for this solution
    // https://stackoverflow.com/a/3431699
    /* the initial problem was late binding, which also happens in python with lambda functions
        - basically the lambda function takes the value *after the loop*, which means it takes the
          current value of the loop variable rather than what it was during the binding
     */

    private UnityAction MakeButton(ButtonType type, int index, int partyIndex = 0)
    {
        // make a pressable button with the associated value index

        if (type == ButtonType.Enemy)
        {
            return () => { FindObjectOfType<BattleManager>().AttackEnemy(index); };
        }
        else if (type == ButtonType.Targets)
        {
            return () => { DisplayItemTargets(index); };
        }
        else
        {
            return () => { FindObjectOfType<BattleManager>().UseItem(partyIndex, index); };
        }
    }

    private enum ButtonType
    {
        Enemy,
        Targets,
        Items
    }
}