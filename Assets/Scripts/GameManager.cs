using UnityEngine;
using System.Collections;
using System.Collections.Generic;		//Allows us to use Lists. 
using UnityEngine.UI;					//Allows us to use UI.

public class GameManager : MonoBehaviour {

    public float LevelStartDelay = 2f;						
    public float TurnDelay = 0.1f;
    public Text infoText;
    public Text _levelText;
    public GameObject _levelImage;
    [HideInInspector] public int HealthP1 = 100;
    [HideInInspector] public int HealthP2 = 100;              
    public static GameManager Instance = null;  
    
    [HideInInspector] public List<Player> players;
    [HideInInspector] public List<Enemy> enemies;
    [HideInInspector] public BoardManager _boardScript;
    [HideInInspector] public bool Godmode = false;
    
    private int _level = 5;                                                                                      
    private bool _doingSetup;                                
    
    void Awake() {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();
        players = new List<Player>();
        _boardScript = GetComponent<BoardManager>();
        DontDestroyOnLoad(_boardScript);
        InitGame();
    }

    void OnLevelWasLoaded(int index) {
        _level++;
        InitGame();
    }

    void InitGame() {
        _doingSetup = true;

        infoText = GameObject.Find("GameInfo").GetComponent<Text>();
        infoText.text = "";

        enemies.Clear();
        this.players.Clear();

        _boardScript.SetupScene(_level);

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
            this.players.Add(players[i].GetComponent<Player>());
    }

    void Update() {
        StartCoroutine(MoveEnemies());

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Godmode = !Godmode;
        }

        if (Godmode)
            infoText.text = "Godmode";
        else
            infoText.text = "";
    }

    public void AddEnemyToList(Enemy script) {
        enemies.Add(script);
    }

    public void GameOver() {
        if (HealthP1 < 0)
            _levelText.text = _level + " Enemies overwelmed Player 1";
        else
            _levelText.text = _level + " Enemies overwelmed Player 2";
        _levelImage.SetActive(true);
        enabled = false;
    }

    public void checkIfAllEnemiesKilled(Enemy enemy) {
        if (enemies.Count == 1 && enabled) {
            enemy.lastShooter.killedLastEnemy();
            AllEnemiesKilled();
        }
    }

    public void AllEnemiesKilled() {
        Invoke("Restart", LevelStartDelay);

        foreach (Player cur in players)
            cur.enabled = false;

        enabled = false;
    }

    private void Restart() {
        Application.LoadLevel(Application.loadedLevel);
    }

    IEnumerator MoveEnemies() {
        yield return new WaitForSeconds(TurnDelay);

        if (enemies.Count == 0 && enabled) {
            AllEnemiesKilled();
            yield break;
        }

        for (int i = 0; i < enemies.Count; i++) {
            if (i < enemies.Count) {
                if (enemies[i] != null)
                    enemies[i].UpdateEnemy();
            }
        }
    }
}
