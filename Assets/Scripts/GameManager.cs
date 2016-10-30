using UnityEngine;
using System.Collections;
using System.Collections.Generic;		//Allows us to use Lists. 
using UnityEngine.UI;                   //Allows us to use UI.
using Panda;

public class GameManager : MonoBehaviour {

    public int NumberEnemies = 1;
    public float LevelStartDelay = 2f;						
    public float TurnDelay = 0.1f;
    public Text infoText;
    public Text _levelText;
    public GameObject _levelImage;
    public bool DebugMode = false;
    public bool AI_Player1 = false;
    public bool AI_Player2 = false;
    [HideInInspector] public int HealthP1 = 100;
    [HideInInspector] public int HealthP2 = 100;              
    public static GameManager Instance = null;  
    
    [HideInInspector] public List<Player> players;
    [HideInInspector] public List<Enemy> enemies;
    [HideInInspector] public BoardManager _boardScript;
    [HideInInspector] public Grid grid;
    [HideInInspector] public Transform enemyHolder;
    [HideInInspector] public int totalEnemiesKilled;
    [HideInInspector] public bool SpecialObjectP1 = false;
    [HideInInspector] public bool SpecialObjectP2 = false;            
                      
    
    void Awake() {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();
        players = new List<Player>();
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        infoText = GameObject.Find("GameInfo").GetComponent<Text>();
        enemyHolder = new GameObject("Enemies").transform;
        _boardScript = GetComponent<BoardManager>();
        DontDestroyOnLoad(_boardScript);
        InitGame();
    }

    void OnLevelWasLoaded(int index) {
        NumberEnemies++;
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
        if (DebugMode)
            infoText.text = "Debug";
        else
            infoText.text = "";

        players[0].GetComponent<PandaBehaviour>().enabled = AI_Player1;
        players[1].GetComponent<PandaBehaviour>().enabled = AI_Player2;
    }

    public void AddEnemyToList(Enemy script) {
        enemies.Add(script);
    }

    public void GameOver() {
        if (HealthP1 < 0)
            _levelText.text = totalEnemiesKilled + " Enemies died before Player 1";
        else
            _levelText.text = totalEnemiesKilled + " Enemies died before Player 2";
        _levelImage.SetActive(true);
        enabled = false;
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

    private void Restart() {
        Application.LoadLevel(Application.loadedLevel);
    }
}
