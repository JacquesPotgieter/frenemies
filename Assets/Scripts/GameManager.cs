using UnityEngine;
using System.Collections;
using System.Collections.Generic;		//Allows us to use Lists. 
using UnityEngine.UI;					//Allows us to use UI.

public class GameManager : MonoBehaviour {

    public float levelStartDelay = 2f;						
    public float turnDelay = 0.1f;                          
    public int player1HP = 100;
    public int player2HP = 100;              
    public static GameManager instance = null;              

    private Text levelText;									
    private GameObject levelImage;							
    private BoardManager boardScript;                       
    private int level = 1;                                  
    private List<Enemy> enemies;                            
    private bool enemiesMoving;                             
    private bool doingSetup;                                
    
    void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();
        boardScript = GetComponent<BoardManager>();
        InitGame();
    }

    void OnLevelWasLoaded(int index) {
        level++;
        InitGame();
    }

    void InitGame() {
        doingSetup = true;

        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day " + level;
        levelImage.SetActive(true);

        Invoke("HideLevelImage", levelStartDelay);
        enemies.Clear();

        boardScript.SetupScene(level);

        GameObject camera = GameObject.Find("Main Camera");
        camera.transform.position = new Vector3(boardScript.BoardWidth / 2f, boardScript.BoardHeight / 2f, -10f);
        Camera.main.orthographicSize = boardScript.BoardHeight / 2 + 2;
    }

    void HideLevelImage() {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    void Update() {
        if (doingSetup)
            return;

        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy script) {
        enemies.Add(script);
    }

    public void GameOver() {
        levelText.text = "After " + level + " days, you starved.";
        levelImage.SetActive(true);
        enabled = false;
    }

    IEnumerator MoveEnemies() {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);

        if (enemies.Count == 0) 
            yield return new WaitForSeconds(turnDelay);        

        for (int i = 0; i < enemies.Count; i++) {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }
    }
}
