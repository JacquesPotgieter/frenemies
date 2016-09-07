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

    private Text _levelText;									
    private GameObject _levelImage;							
    private BoardManager _boardScript;                       
    private int _level = 1;                                  
    private List<Enemy> _enemies;                                                       
    private bool _doingSetup;                                
    
    void Awake() {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        _enemies = new List<Enemy>();
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
        _enemies.Clear();

        _boardScript.SetupScene(_level);

        GameObject camera = GameObject.Find("Main Camera");
        camera.transform.position = new Vector3(_boardScript.BoardWidth / 2f, _boardScript.BoardHeight / 2f, -10f);
        Camera.main.orthographicSize = _boardScript.BoardHeight / 2 + 2;
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
        _enemies.Add(script);
    }

    public void GameOver() {
        _levelText.text = "After " + _level + " days, you starved.";
        _levelImage.SetActive(true);
        enabled = false;
    }

    IEnumerator MoveEnemies() {
        yield return new WaitForSeconds(TurnDelay);

        if (_enemies.Count == 0) 
            yield return new WaitForSeconds(TurnDelay);        

        for (int i = 0; i < _enemies.Count; i++) {
            _enemies[i].UpdateEnemy();
            yield return new WaitForSeconds(_enemies[i].MoveTime);
        }
    }
}
