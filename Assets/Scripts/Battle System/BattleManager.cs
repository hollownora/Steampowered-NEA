using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public List<GameObject> partyMembers;
    public List<GameObject> enemies;
    private List<GameObject> enemyArchive = new List<GameObject>();
    public GameObject battleMenuScreen;
    private bool initDone = false;
    public TextMeshProUGUI captionText;
    public TextMeshProUGUI healthLabel;
    public TextMeshProUGUI psiLabel;
    [SerializeField] private GameObject pool;
    private string returnRoom = "";
    private int partyTurnCount = 0;
    private int enemyTurnCount = 0;
    private bool startEnemyTurn = false;
    private bool startPlayerTurn = true;
    private BattleState turnState = new BattleState();
    private bool finishedAttacking = false;
    //private bool runningEnemyMoves = false;

    readonly Vector3[] enemyPositions = {
        new Vector3(8, 1.3f, -10),
        new Vector3(5, 1.3f, -7),
        new Vector3(2, 1.3f, -4)
    };

    readonly Vector3[] partyPositions =
    {
        new Vector3(-7, 1.3f, -10),
        new Vector3(-5, 1.3f, -5),
        new Vector3()
    };

    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(679283);
        captionText.gameObject.SetActive(true);
    }

    // Update is called once per
    void Update()
    {
        if (initDone)
        {
            if (startEnemyTurn)
            {
                for (int menuItemIndex = 0; menuItemIndex < GameObject.Find("BattleCanvas/Menuframe/ActionsPanel").transform.childCount; menuItemIndex++)
                {
                    GameObject.Find("BattleCanvas/Menuframe/ActionsPanel").transform.GetChild(menuItemIndex).gameObject.GetComponent<UnityEngine.UI.Button>().enabled = false;
                }
                startEnemyTurn = false;
                Debug.Log("Starting enemy turn");
                StartCoroutine(AttackPlayer());
            }
            else if (startPlayerTurn)
            {
                Debug.Log("starting player turn");
                // set the battle caption to one of the enemies captions
                var enemyObj = enemies[Random.Range(0, enemies.Count - 1)];
                captionText.text = enemyObj.GetComponent<EnemyData>().battlecaptions[Random.Range(0, enemyObj.GetComponent<EnemyData>().battlecaptions.Length - 1)];
                startPlayerTurn = false;
                for (int menuItemIndex = 0; menuItemIndex < GameObject.Find("BattleCanvas/Menuframe/ActionsPanel").transform.childCount; menuItemIndex++)
                {
                    GameObject.Find("BattleCanvas/Menuframe/ActionsPanel").transform.GetChild(menuItemIndex).gameObject.GetComponent<UnityEngine.UI.Button>().enabled = true;
                }

            }

            // check if the enemy's turn has ende
            if (finishedAttacking)
            {
                // check if all players are down
                if (AreAllPlayersDown())
                {
                    // game over
                    // TODO: add gameover sequence
                }
                else
                {
                    IncrementTurn(false);
                }
                finishedAttacking = false;
            }
            // update UI
            healthLabel.text = $"HP: {partyMembers[0].GetComponent<PlayerStatsController>().hitPoints}/{partyMembers[0].GetComponent<PlayerStatsController>().hitPointsCap}";
            psiLabel.text = $"PSI: {partyMembers[0].GetComponent<PlayerStatsController>().tankPressure}/{partyMembers[0].GetComponent<PlayerStatsController>().tankPressureCap}";
        }
    }

    public void Init(List<GameObject> party, string sourceEnemyPath, EnemyPool poolOfEnemies, string roomToReturnTo)
    {
        returnRoom = roomToReturnTo;
        for (int partyMemberIndex = 0; partyMemberIndex < party.Count; partyMemberIndex++)
        {
            party[partyMemberIndex].transform.position = partyPositions[partyMemberIndex];
            // rotate player to face enemy
            party[partyMemberIndex].transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            party[partyMemberIndex].GetComponent<PlayerMovement>().enabled = false;
            // store the party members in the manager's party list
            partyMembers.Add(party[partyMemberIndex]);
        }
        // create the enemies
        try
        {
            enemies.Add(Instantiate(Resources.Load<GameObject>(sourceEnemyPath)));
        }
        catch
        {
            // unable to load enemy for some reason?
            Debug.Log("Could not load enemy at " + sourceEnemyPath);
        }

        for (int randAdditionalEnemy = 0; randAdditionalEnemy < Random.Range(0, 2); randAdditionalEnemy++)
        {
            // add a new enemy
            try
            {
                enemies.Add(Instantiate(Resources.Load<GameObject>(poolOfEnemies.GetRandomEnemy())));
            }
            catch
            {
                Debug.Log("Could not add enemy");
            }
        }

        // position all of the enemies
        for (int enemyPosIndex = 0; enemyPosIndex < enemies.Count; enemyPosIndex++)
        {
            enemies[enemyPosIndex].transform.position = enemyPositions[enemyPosIndex];
            enemies[enemyPosIndex].transform.rotation = Quaternion.Euler(new Vector3(0, 270, 0));
        }
        Debug.Log("enemies positioned");
        // set the battle caption for the encounter based off of one of the captions from the source enemy
        captionText.text = enemies[0].GetComponent<EnemyData>().battlecaptions[Random.Range(0, enemies[0].GetComponent<EnemyData>().battlecaptions.Length - 1)];
        initDone = true;
    }

    IEnumerator AttackPlayer()
    {
        Debug.Log("Attacking player");
        // player is attacked, happens over several frames
        int targetIndex = Random.Range(0, partyMembers.Count - 1);
        // attack the targeted enemy, set the caption as necessary
        captionText.gameObject.SetActive(true);
        captionText.text = $"{enemies[enemyTurnCount].name} attacks {partyMembers[targetIndex].name}!";
        // roll for damage
        int damage = Random.Range(0, 1) + enemies[enemyTurnCount].GetComponent<EnemyData>().attack;
        // damage the player by this amount
        partyMembers[targetIndex].GetComponent<PlayerStatsController>().hitPoints -= damage;
        // check if player should be down
        if (partyMembers[targetIndex].GetComponent<PlayerStatsController>().hitPoints <= 0)
        {
            // set down status to true
            partyMembers[targetIndex].GetComponent<PlayerStatsController>().isPlayerDown = true;
        }

        yield return new WaitForSeconds(2f);
        Debug.Log("Finished attacking");
        finishedAttacking = true;
    }

    public void FleeBattle()
    {
        // currently 100% chance of success, set state to overworld
        FindObjectOfType<GameManager>().gameState = GameState.overworld;
        // load the scene where the battle was initiated
        partyMembers[0].transform.position = FindObjectOfType<GameManager>().originPos;
        SceneManager.LoadScene(returnRoom);
    }

    private bool AreAllPlayersDown()
    {
        bool allDown = true;
        for (int playerToCheck = 0; playerToCheck < partyMembers.Count; playerToCheck++)
        {
            if (!partyMembers[playerToCheck].GetComponent<PlayerStatsController>().isPlayerDown)
            {
                allDown = false;
                break;
            }
        }
        return allDown;
    }

    public void IncrementTurn(bool forPlayer=true)
    {
        Debug.Log("Incrementing turn");
        if (forPlayer)
        {
            partyTurnCount++;
            // check if turn count > party member count
            if (partyTurnCount > partyMembers.Count - 1)
            {
                // all players have had turn
                partyTurnCount = 0;
                startEnemyTurn = true;
                turnState = BattleState.enemyTurn;
            }
            else
            {
                startPlayerTurn = true;
            }
        }
        else
        {
            enemyTurnCount++;
            // check if enemy turn count > no of enemies
            if (enemyTurnCount > enemies.Count - 1)
            {
                // all enemies have had turn
                enemyTurnCount = 0;
                startPlayerTurn = true;
                turnState = BattleState.partyTurn;
            }
            else
            {
                startEnemyTurn = true;
            }
        }
    }

    public void UseItem(int itemIndex, int memberIndex)
    {
        partyMembers[memberIndex].GetComponent<PlayerStatsController>().partyInventory.UseItem(itemIndex);
        IncrementTurn();
    }

    public void AttackEnemy(int enemyIndex)
    {
        // attack the enemy
        Debug.Log("Attacking enemy");
        captionText.text = $"{partyMembers[partyTurnCount].name} attacks {enemies[enemyIndex].name}!";
        enemies[enemyIndex].GetComponent<EnemyData>().DamageEnemy(partyMembers[partyTurnCount].GetComponent<PlayerStatsController>().attack);
        // remove enemy selection buttons
        FindObjectOfType<BattleUIController>().ClearBattleMenu();
        // check if the enemy needs to be removed
        if (enemies[enemyIndex].GetComponent<EnemyData>().GetHitPoints() <= 0)
        {
            // move the enemy to archival storage
            enemyArchive.Add(enemies[enemyIndex]);
            enemies[enemyIndex].SetActive(false);
            enemies.RemoveAt(enemyIndex);
            // check if all enemies are cleared
            if (enemies.Count == 0)
            {
                // all enemies are gone, clear memory
                for (int removalIndex = 0; removalIndex < enemyArchive.Count; removalIndex++)
                {
                    Destroy(enemyArchive[removalIndex]);
                }
                enemyArchive.Clear();
                // return to initial scene
                Debug.Log(returnRoom);
                partyMembers[0].transform.position = FindObjectOfType<GameManager>().originPos;
                SceneManager.LoadScene(returnRoom);
            }
            else
            {
                IncrementTurn();
            }
        }
        else
        {
            IncrementTurn();
        }
    }

    public void SkipTurn()
    {
        IncrementTurn();
    }

    private enum BattleState
    {
        partyTurn = 0,
        enemyTurn = 1
    }

}

enum SkillTypes
{
    healing,
    offensive
}