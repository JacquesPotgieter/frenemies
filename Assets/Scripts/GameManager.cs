using UnityEngine;
using System.Collections;
using System.Collections.Generic;		//Allows us to use Lists. 
using UnityEngine.UI;					//Allows us to use UI.

public class GameManager : MonoBehaviour {

    public float LevelStartDelay = 2f;						
    public float TurnDelay = 0.1f;                          
    [HideInInspector] public int HealthP1 = 100;
    [HideInInspector] public int HealthP2 = 100;              
    public static GameManager Instance = null;  
    
    [HideInInspector] public List<Player> players;
    [HideInInspector] public List<Enemy> enemies;
    [HideInInspector] public BoardManager _boardScript;

    private Text _levelText;									
    private GameObject _levelImage;                      
    private int _level = 1;                                                                                      
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
        InitGame();
    }

    void OnLevelWasLoaded(int index) {
        _level++;
        InitGame();
    }

    void InitGame() {
        _doingSetup = true;

        _levelImage = GameObject.Find("LevelImage");
        _levelText = GameObject.Find("LevelText").GetComponent<Text>();
        _levelText.text = "Day " + _level;
        _levelImage.SetActive(true);

        Invoke("HideLevelImage", LevelStartDelay);
        enemies.Clear();
        this.players.Clear();

        _boardScript.SetupScene(_level);

        GameObject camera = GameObject.Find("Main Camera");
        camera.transform.position = new Vector3(_boardScript.BoardWidth / 2f, _boardScript.BoardHeight / 2f, -10f);
        Camera.main.orthographicSize = _boardScript.BoardHeight / 2 + 2;

        Player[] players = GetComponents<Player>();
        for (int i = 0; i < players.Length; i++)
            this.players.Add(players[i]);
    }

    void HideLevelImage() {
        _levelImage.SetActive(false);
        _doingSetup = false;
    }

    void Update() {
        if (_doingSetup)
            return;

        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy script) {
        enemies.Add(script);
    }

    public void GameOver() {
        _levelText.text = "After " + _level + " days, you starved.";
        _levelImage.SetActive(true);
        enabled = false;
    }

    IEnumerator MoveEnemies() {
        yield return new WaitForSeconds(TurnDelay);

        if (enemies.Count == 0) 
            yield return new WaitForSeconds(TurnDelay);        

        for (int i = 0; i < enemies.Count; i++) {
            enemies[i].UpdateEnemy();
            yield return new WaitForSeconds(enemies[i].MoveTime);
        }
    }
}
