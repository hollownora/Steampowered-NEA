using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // object used to control game flow
    // capture the pause / menu GUI object
    public Vector3 originPos = new Vector3();
    public  GameObject menuScreen;
    public GameObject statsDisplay;
    public GameObject backgroundMask;
    public PlayerStatsController statisticsSystem;
    protected bool paused = false;
    public GameState gameState = new GameState();
    private BattleManager bManager;
    private string returnSceneName = "";
    public bool isReady = false;
    [SerializeField] private GameObject player;

    private string collisionEnemyPath;

    // Start is called before the first frame update
    void Start()
    {
        // set the current scene
        returnSceneName = SceneManager.GetActiveScene().name;
        // connect to the HUD controller for the scen
        // set the game state and set up the sceneload handler method
        gameState = GameState.overworld;
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    // Update is called once per frame
    void Update()
    {
        if (isReady)
        {
            // check game state
            if (gameState == GameState.overworld)
            {
                player.GetComponent<PlayerMovement>().enabled = true;
                // get input from x key
                if (Input.GetKeyUp(KeyCode.X) && gameState != GameState.battle)
                {
                    // x key was pressed, toggle the pause screen
                    SetPaused(Toggle(GetPaused()));
                }

                // set the activity of hud and menu screen
                menuScreen.SetActive(GetPaused());
                backgroundMask.SetActive(GetPaused());
                statsDisplay.SetActive(!GetPaused());
            }
            else if (gameState == GameState.dialogue)
            {
                // disable movement
                player.GetComponent<PlayerMovement>().enabled = false;
                //disable stats frame
                statsDisplay.SetActive(false);
            }
            if (SceneManager.GetActiveScene().name != "TitleScreen")
            {
                statsDisplay.SetActive(true);
            }
            else
            {
                statsDisplay.SetActive(false);
            }

            if (SceneManager.GetActiveScene().name == "BattleRoom")
            {
                menuScreen.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                menuScreen.transform.parent.gameObject.SetActive(false);
            }

            // update stats panel
            statsDisplay.transform.GetChild(0).gameObject.GetComponentInChildren<TextMeshProUGUI>().text = $"HP: {statisticsSystem.hitPoints}/{statisticsSystem.hitPointsCap}";
            statsDisplay.transform.GetChild(1).gameObject.GetComponentInChildren<TextMeshProUGUI>().text = $"PSI: {statisticsSystem.tankPressure}/{statisticsSystem.tankPressureCap}";
        }
    }

    public void SetPaused(bool newPauseValue)
    {
        paused = newPauseValue;
    }

    public bool GetPaused()
    {
        return paused;
    }

    public bool Toggle(bool baseValue)
    {
        return !baseValue;
    }

    public static void RaiseEvent(GameEventTypes gameEvent)
    {
        if (gameEvent == GameEventTypes.BattleStart)
        {
            // set scene to battle room
            SceneManager.LoadScene("BattleRoom");
        }
    }

    public void TriggerBattle(GameObject enemy)
    {
        gameState = GameState.battle;
        originPos = player.transform.position;
        GameObject.Find("Main Camera").transform.rotation = Quaternion.Euler(new Vector3(9.637f, 0.000f, 0.000f));
        GameObject.Find("Main Camera").transform.position = new Vector3(-0.02f, 3.23f, -19.41f);
        // load the battle scene
        collisionEnemyPath = enemy.GetComponent<EnemyData>().prefabAssetPath;
        SceneManager.LoadScene("BattleRoom");
    }

    public void GameOver()
    {
        
    }

    public void OnSceneLoad(Scene sceneCheck, LoadSceneMode mode)
    {
        if (sceneCheck.name == "BattleRoom")
        {
            // battle was entered
            // get battle manager
            List<GameObject> tmpParty = new List<GameObject>();
            tmpParty.Add(FindObjectOfType<PlayerStatsController>().gameObject);
            Debug.Log("Party initialised");
            bManager = FindObjectOfType<BattleManager>();
            statsDisplay.SetActive(false);
            bManager.Init(tmpParty, collisionEnemyPath, FindObjectOfType<EnemyPool>(), returnSceneName);
        }
        else
        {
            if (sceneCheck.name != "TitleScreen" && sceneCheck.name != "PreLoadScene")
            {
                // other room was loaded, set return scene

                returnSceneName = sceneCheck.name;
                player.SetActive(true);
                gameState = GameState.overworld;
            }
        }
    }

    public void LoadReady()
    {
        // load the next scene in the build index
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

public enum GameState
{
    overworld = 0,
    battle = 1,
    dialogue = 2
}

public enum BattleState
{
    playerTurn,
    enemyTurn
}

public enum GameEventTypes
{
    BattleStart,
    BattleWin,
    GameOver
}