using UnityEngine;
using System.Collections;
using System.Collections.Generic;		//Allows us to use Lists. 
using UnityEngine.UI;                   //Allows us to use UI.
using Panda;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager Instance = null;

    private int NumberEnemies = 0;
    public float LevelStartDelay = 2f;						
    public float TurnDelay = 0.1f;
    public Text infoText;
    public Text _levelText;
    public GameObject _levelImage;
    [HideInInspector] public int HealthP1 = 100;
    [HideInInspector] public int HealthP2 = 100;            
      
    
    [HideInInspector] public List<Player> players;
    [HideInInspector] public List<Enemy> enemies;
    [HideInInspector] public BoardManager _boardScript;
    [HideInInspector] public Grid grid;
    [HideInInspector] public Transform enemyHolder;
    [HideInInspector] public int totalEnemiesKilled;
    [HideInInspector] public bool SpecialObjectP1 = false;
    [HideInInspector] public bool SpecialObjectP2 = false;

    [HideInInspector] public GlobalSettings globalSettings = null;              
    
    void Awake() {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        enemies = new List<Enemy>();
        players = new List<Player>();
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        infoText = GameObject.Find("GameInfo").GetComponent<Text>();
        enemyHolder = new GameObject("Enemies").transform;
        _boardScript = GetComponent<BoardManager>();        

        globalSettings = GameObject.Find("GlobalSettings").GetComponent<GlobalSettings>();
        this.NumberEnemies = globalSettings.startingLevel;

        AudioListener.volume = globalSettings.soundLevel;

        InitGame();
    }

    void InitGame() {               
        infoText.text = "";

        enemies.Clear();
        this.players.Clear();
        grid.Setup();
        _boardScript.SpawnEnemies(NumberEnemies, enemyHolder, LevelStartDelay);

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
            this.players.Add(players[i].GetComponent<Player>());
    }

    void Update() {
        if (globalSettings.DebugMode)
            infoText.text = "Debug";
        else
            infoText.text = "";

        if (Input.GetKeyDown(KeyCode.Escape) == true)
            StartCoroutine(OnEscapeStart());

        GameObject.Find("Player_P1").GetComponent<PandaBehaviour>().enabled = globalSettings.AI_Player_1;
        GameObject.Find("Player_P2").GetComponent<PandaBehaviour>().enabled = globalSettings.AI_Player_2;
    }

    public void AddEnemyToList(Enemy script) {
        enemies.Add(script);
    }

    public void GameOver() {
        enabled = false;

        StartCoroutine(BackToMenu());
    }

    public void checkIfAllEnemiesKilled(Enemy enemy) {
        if (enemies.Count == 1 && enabled) {
            enemy.lastShooter.killedLastEnemy();
            if (SpecialObjectP1!=SpecialObjectP2)
            {
                int c = HealthP1;
                HealthP1 = -1000;
                HealthP2 = +1000;

            }
            AllEnemiesKilled();
            Debug.Log("lalalalalal"+SpecialObjectP2+"2:1"+SpecialObjectP1);
        }
    }

    private void AllEnemiesKilled() {
        NumberEnemies++;
        SpawnNewEnemies();
    }

    private void SpawnNewEnemies() {
        _boardScript.SpawnEnemies(NumberEnemies, enemyHolder, LevelStartDelay);
    }

    public IEnumerator BackToMenu() {
        foreach (MovingObject cur in enemies)
            cur.enabled = false;
        foreach (MovingObject cur in players)
            cur.enabled = false;
        yield return new WaitForSeconds(0);
        SceneManager.LoadScene(0);
    }

    private IEnumerator OnEscapeStart() {
        Debug.Log("Started pressing Escape");
        StartCoroutine(BackToMenu());

        if (Input.GetKeyDown(KeyCode.Escape) == true) {
            
        }
        yield return new WaitForSeconds(0);
    }
}
